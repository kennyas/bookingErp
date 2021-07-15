using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Dapper.Interfaces;
using Tornado.Shared.Dapper.Services;
//using Tornado.Shared.EF;
//using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using Wallet.Core.Dtos;
using Wallet.Core.Enums;
using Wallet.Core.Events;
using Wallet.Core.Helpers;
using Wallet.Core.Models;
using Wallet.Core.Services.Interfaces;
using Wallet.Core.Utility;
using Wallet.Core.ViewModels;
using static Wallet.Core.Models.PaystackObjectV1;
using Data = Wallet.Core.Models.Data;

namespace Wallet.Core.Services
{
    public class PaymentService : Service<PaymentDetail>, IPaymentService
    {
        private readonly IHttpUserService _currentUserService;
        private readonly ICustomerWalletService _customerWalletService;
        private readonly IConfiguration configuration;
        private readonly IWalletEventService _walletEventService;

        public PaymentService(IUnitOfWork unitOfWork, IHttpUserService currentUserService, ICustomerWalletService customerWalletService, IWalletEventService walletEventService,
            IConfiguration iconfig) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
            _customerWalletService = customerWalletService;
            configuration = iconfig;
            _walletEventService = walletEventService;
        }

        public async Task<ApiResponse<IEnumerable<WalletTransHistory>>> CustomerWalletTransHistoryAsync(WalletHistoryViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model?.CustomerId))
            {
                return new ApiResponse<IEnumerable<WalletTransHistory>>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
            }

            var currentUserId = _currentUserService.GetCurrentUser().UserId;
            Guid.TryParse(model.CustomerId, out Guid customerId);

            if (!Guid.Equals(customerId, currentUserId))
            {
                return new ApiResponse<IEnumerable<WalletTransHistory>>(codes: ApiResponseCodes.EXCEPTION, errors: "incorrect customer id");
            }

            int pageStart = model.PageIndex ?? 0;
            int pageEnd = model.PageTotal ?? 10;
            int GETTOTAL = 1;

            DynamicParameters _params = new DynamicParameters();
            _params.AddDynamicParams(new
            {
                GETTOTAL,
                customerId,
                model.PageIndex,
                model.PageTotal,
                model.StartDate,
                model.EndDate

            });
            _params.Add("@TOTALRECORDS", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string sql = "sp_WalletHistory_paginated";
            var result = await ExecuteStoredProcedure<WalletTransHistory>(sql, _params);
            int TOTALRECORDS = _params.Get<int>("@TOTALRECORDS");
            var modelEntities = result.ToList();
            modelEntities?.ForEach(x => x.Amount.ToString("N2"));
            return modelEntities != null && modelEntities.Any()
                ? new ApiResponse<IEnumerable<WalletTransHistory>>(message: "Successful",
                              codes: ApiResponseCodes.OK, data: modelEntities, totalCount: TOTALRECORDS)
                : new ApiResponse<IEnumerable<WalletTransHistory>>(errors: "No wallet transaction history found", codes: ApiResponseCodes.NOT_FOUND);
        }

        public async Task<ApiResponse<CustomerWallet>> CreateCustomerWalletAsync(CreateWalletViewModel model)
        {

            if (model == null || string.IsNullOrEmpty(model?.PhoneNumber))
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
            }

            var currentPhoneNumber = _currentUserService.GetCurrentUser().PhoneNumber;
            var currentUserId = _currentUserService.GetCurrentUser().UserId;

            if (!string.Equals(model.PhoneNumber, currentPhoneNumber) || !Guid.Equals(model.UserId, currentUserId))
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.EXCEPTION, errors: "incorrect phone number or customer id");
            }

            var existingPayment = _customerWalletService.GetCustomerWallet(currentUserId);

            if (existingPayment != null)
            {
                return new ApiResponse<CustomerWallet>(errors: "Customer has an exisiting wallet", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            var wallet = new CustomerWallet
            {
                CreatedBy = currentUserId,
                CreatedOn = Clock.Now,
                IsDeleted = false,
                AvailableBalance = 0,
                BlockedBalance = 0,
                CustomerId = currentUserId,
                LedgerBalance = 0,
                PhoneNumber = currentPhoneNumber,
                Fullname = $"{_currentUserService.GetCurrentUser().FirstName} {_currentUserService.GetCurrentUser().LastName}",
            };
            var addedEntity = await _customerWalletService.AddCustomerWalletAsync(wallet);


            return addedEntity ? new ApiResponse<CustomerWallet>(message: "Successful",
                            codes: ApiResponseCodes.OK, data: wallet)
                : new ApiResponse<CustomerWallet>(errors: "Could not create customer wallet", codes: ApiResponseCodes.FAILED);


        }
        public async Task<ApiResponse<CustomerWallet>> GetWalletBalance(Guid UserId)
        {

            CustomerWallet existingwallet = null;
            var currentUserId = _currentUserService.GetCurrentUser().UserId;

            if (!Guid.Equals(UserId, currentUserId))
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.EXCEPTION, errors: "user does not exist or invalid user");
            }
            await Task.Run(() =>
            {
                existingwallet = _customerWalletService.GetCustomerWallet(UserId);
            });
            if (existingwallet == null)
            {
                return new ApiResponse<CustomerWallet>(errors: "Customer Wallet not found", codes: ApiResponseCodes.INVALID_REQUEST);
            }
            return new ApiResponse<CustomerWallet>(message: "Successful", codes: ApiResponseCodes.OK, data: existingwallet, totalCount: 1);
        }

        public async Task<ApiResponse<CustomerWalletDto>> NameEnquiryAsync(string mobile_number)
        {
            var currentPhoneNumber = _currentUserService.GetCurrentUser().PhoneNumber;

            if (string.IsNullOrWhiteSpace(mobile_number))
            {
                return new ApiResponse<CustomerWalletDto>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
            }
            var walletEnquiry = await _customerWalletService.GetCustomerWallet(mobile_number);

            return walletEnquiry != null && walletEnquiry.Any() ? new ApiResponse<CustomerWalletDto>(message: "Successful",
                            codes: ApiResponseCodes.OK, data: walletEnquiry?.FirstOrDefault(), totalCount: walletEnquiry?.Count())
                : new ApiResponse<CustomerWalletDto>(errors: "Wallet not found", codes: ApiResponseCodes.FAILED);
        }

        public async Task<ApiResponse<PaymentDetailViewModel>> CreatePaymentDetailAsync(CreatePaymentDetailDto model, string ip)
        {

            var response = new ApiResponse<PaymentDetailViewModel> { Code = ApiResponseCodes.OK };
            if (model == null || model.Amount <= 0)
            {
                return new ApiResponse<PaymentDetailViewModel>(codes: ApiResponseCodes.INVALID_REQUEST,
                                                         errors: "invalid request");
            }
            Guid.TryParse(model.UserId, out Guid guidId);
            var currentUserId = _currentUserService.GetCurrentUser().UserId;


            if (!Equals(guidId, currentUserId))
            {
                return new ApiResponse<PaymentDetailViewModel>(codes: ApiResponseCodes.INVALID_REQUEST,
                                                      errors: "invalid user id");
            }
            var danfoReference = $"{"DANFO"}{WalletMgt.RandomString(20)}";

            model.PaystackReference = string.IsNullOrEmpty(model.PaystackReference) ? danfoReference
                                        : model.PaystackReference;
            //{DateTime.Now.ToString("ddMMyyyyhhmmss")}
            IDictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "Reference", model.PaystackReference }
            };
            var existingpayment = Find(@"SELECT * FROM PaymentDetail WHERE Reference = @Reference", parameter);

            if (existingpayment != null && existingpayment.Any())
            {
                return new ApiResponse<PaymentDetailViewModel>(errors: "Payment record already exists", codes: ApiResponseCodes.INVALID_REQUEST);
            }
            var payment = new PaymentDetail
            {
                CreatedBy = guidId, //_currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                IsDeleted = false,
                Gateway_response = string.Empty,
                Email = model.Email,
                Amount = model.Amount,
                Currency = model.Currency,
                CustomerId = guidId,
                Message = string.Empty,
                //Mobileno = model.Mobileno,
                PhoneNumber = model.PhoneNumber,
                Reference = model.PaystackReference,
                TransStatus = string.Empty,
                Ip_address = ip,
                IsUsed = false,
                DanfoReference = danfoReference
            };

            // var addedEntity = await AddAsync(payment);

            await AddAsync(payment);

            var viewModel = new PaymentDetailViewModel();
            PropertyCopier<CreatePaymentDetailDto, PaymentDetailViewModel>.Copy(model, viewModel);

            viewModel.Id = payment.Id;
            viewModel.CreatedBy = payment.CreatedBy;
            viewModel.CreatedOn = payment.CreatedOn;
            viewModel.Reference = payment.Reference;
            viewModel.Amount = payment.Amount.ToString("N2");
            viewModel.Reference = model.PaystackReference;

            return payment.Id != null ? new ApiResponse<PaymentDetailViewModel>(message: "Successful",
                            codes: ApiResponseCodes.OK, data: viewModel)
                : new ApiResponse<PaymentDetailViewModel>(errors: "Could not add payment detail", codes: ApiResponseCodes.FAILED);
        }

        public async Task<ApiResponse<WalletTransferDto>> CustomerWalletTransferAsync(WalletTransferViewModel model)
        {
            var paymentreference = $"{"DANFO"}{WalletMgt.RandomString(20)}";
            if (model == null || model.Amount <= 0)
            {
                return new ApiResponse<WalletTransferDto>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
            }

            if (model.DebitWalletId == model.CreditWalletId)
            {
                return new ApiResponse<WalletTransferDto>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request. check wallet ID");
            }
            //  Guid.TryParse(model.CustomerId, out Guid guidId);
            var existingwallet = _customerWalletService.GetCustomerWallet(model.CustomerId);

            if (existingwallet == null || existingwallet.Id != model.DebitWalletId)
            {
                return new ApiResponse<WalletTransferDto>(errors: "Customer Wallet not found", codes: ApiResponseCodes.INVALID_REQUEST);
            }
            bool isFundOK = new WalletMgt(_currentUserService).IsFundSufficient(model.Amount, existingwallet.AvailableBalance, existingwallet.BlockedBalance);
            if (isFundOK)
            {
                // new WalletMgt(_currentUserService).GetWalletModel(model.Amount, ref existingwallet, (int)TransactionType.DEBIT);
                var walletdto = new WalletTransferDto
                {
                    Amount = model.Amount,
                    CreatedBy = model.CustomerId, //_currentUserService.GetCurrentUser().UserId, // model.CustomerId,
                    CreatedOn = Clock.Now,
                    CreditWalletId = model.CreditWalletId,
                    CustomerId = model.CustomerId,
                    DebitWalletId = model.DebitWalletId,
                    PaymentReference = paymentreference, // model.PaymentReference,
                    TransactionDesc = model.TransactionDesc,
                    ModifiedBy = model.CustomerId, //_currentUserService.GetCurrentUser().UserId,
                    ModifiedOn = Clock.Now,
                    NewBalance = existingwallet.AvailableBalance - model.Amount,
                    TransType = "DR",
                    //Reference = ""
                };

                var transfer = await _customerWalletService.CustomerWalletTransferAsync(walletdto);
                string responseCode = transfer != null && transfer.Any() ? ApiResponseCodes.OK.GetDescription() : ApiResponseCodes.FAILED.GetDescription();
                string responseMessage = transfer != null && transfer.Any() ? "Successful" : "Could not debit wallet";
                decimal availablebalance = transfer != null && transfer.Any() ? transfer.FirstOrDefault().AvailableBalance : existingwallet.AvailableBalance;

                return transfer != null && transfer.Any() ? new ApiResponse<WalletTransferDto>(message: "Successful",
                              codes: ApiResponseCodes.OK, data: walletdto)
                : new ApiResponse<WalletTransferDto>(errors: "Could not debit wallet" + paymentreference, codes: ApiResponseCodes.FAILED);
            }

            return new ApiResponse<WalletTransferDto>(errors: "Could not debit wallet/Insufficient fund", codes: ApiResponseCodes.FAILED);

        }

        public async Task<ApiResponse<WalletDebitDto>> DebitCustomerWalletAsync(CustomerWalletViewModel model)
        {
            var paymentReference = $"{"DANFO"}{WalletMgt.RandomString(20)}";
            IEnumerable<CustomerWallet> debitWallet = new List<CustomerWallet>();
            var dto = new WalletDebitDto();

            if (model == null || model.Amount <= 0)
            {
                return new ApiResponse<WalletDebitDto>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
            }
            Guid.TryParse(model.CustomerId, out Guid guidId);
            var existingwallet = _customerWalletService.GetCustomerWallet(guidId);

            if (existingwallet == null)
            {
                return new ApiResponse<WalletDebitDto>(errors: "Customer Wallet not found", codes: ApiResponseCodes.INVALID_REQUEST);
            }
            if (new WalletMgt(_currentUserService).IsFundSufficient(model.Amount, existingwallet.AvailableBalance, existingwallet.BlockedBalance))
            {
                //new WalletMgt(_currentUserService).GetWalletModel(model.Amount, ref existingwallet, (int)TransactionType.DEBIT);               
                var walletdto = new WalletDebitDto
                {
                    Amount = model.Amount,
                    CreatedBy = _currentUserService.GetCurrentUser().UserId, // model.CustomerId,
                    CreatedOn = Clock.Now,
                    CustomerId = guidId,
                    DebitWalletId = existingwallet.Id,
                    PaymentReference = paymentReference, //model.PaymentReference,
                    TransactionDesc = model.TransactionDesc,
                    ModifiedBy = _currentUserService.GetCurrentUser().UserId,
                    ModifiedOn = Clock.Now,
                    NewBalance = existingwallet.AvailableBalance - model.Amount,
                    TransType = "DR",
                    Reference = model.Reference
                };
                dto = walletdto;
                var viewModel = new WalletTransferDto();
                PropertyCopier<WalletDebitDto, WalletTransferDto>.Copy(walletdto, viewModel);

                debitWallet = await _customerWalletService.DebitCreditWalletAsync(viewModel, (int)TransactionType.DEBIT);
            }

            string responseCode = debitWallet != null && debitWallet.Any() ? ApiResponseCodes.OK.GetDescription() : ApiResponseCodes.FAILED.GetDescription();
            string responseMessage = debitWallet != null && debitWallet.Any() ? "Successful" : "Insufficient amount";
            decimal availablebalance = debitWallet != null && debitWallet.Any() ? debitWallet.FirstOrDefault().AvailableBalance : existingwallet.AvailableBalance;

            await _walletEventService.PublishEvent(new PaymentSucceededIntegrationEvent(guidId, model.Reference,
                 responseCode, responseMessage, model.Amount, availablebalance, Clock.Now.ToString(CoreConstants.DateFormat),
                 _currentUserService?.GetCurrentUser()?.Email, CoreConstants.TransactionType.WalletDebit, _currentUserService?.GetCurrentUser()?.FirstName,
                _currentUserService?.GetCurrentUser()?.PhoneNumber));

            return debitWallet != null && debitWallet.Any() ? new ApiResponse<WalletDebitDto>(message: "Successful",
                            codes: ApiResponseCodes.OK, data: dto)
                : new ApiResponse<WalletDebitDto>(errors: "Could not debit wallet/Insufficient fund", codes: ApiResponseCodes.FAILED);
        }

        public async Task<ApiResponse<CustomerWallet>> CreditCustomerWalletAsync(CreditWalletViewModel model)
        {
            // var response = new ApiResponse<PaymentDetail> { Code = ApiResponseCodes.OK };
            //try
            //{
            if (model == null || model.Amount <= 0)
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
            }
            Guid.TryParse(model.CustomerId, out Guid guidId);
            var existingwallet = _customerWalletService.GetCustomerWallet(guidId);

            if (existingwallet == null)
            {
                return new ApiResponse<CustomerWallet>(errors: "Customer Wallet not found", codes: ApiResponseCodes.INVALID_REQUEST);
            }
            IDictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "Reference", model.PaymentReference }
            };
            var existingpayment = Find(@"SELECT * FROM PaymentDetail WHERE Reference = @Reference", parameter)?.FirstOrDefault();

            // var existingpayment = Find().FirstOrDefault(p => string.Equals(model.PaymentReference, p?.Reference, StringComparison.OrdinalIgnoreCase));

            if (existingpayment == null)
            {
                return new ApiResponse<CustomerWallet>(errors: "Payment record does not exists", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            //check for amount paid
            if (Convert.ToDouble(existingpayment?.Amount) != Convert.ToDouble(model.Amount))
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "Inconsistence in transaction amount");
            }
            //check for unsuccessful payment
            if (existingpayment?.TransStatus != ApiResponseCodes.OK.GetDescription())
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "Payment was not successful"); // string.IsNullOrWhiteSpace(existingpayment.Message) ? "Payment not verified" : existingpayment.Message);
            }

            if (existingpayment.IsUsed)
            {
                return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.UNAUTHORIZED, errors: "Wallet credited before with this reference, please check balance");
            }
            existingpayment.IsUsed = true;
            existingpayment.ModifiedBy = guidId; // _currentUserService.GetCurrentUser().UserId;
            existingpayment.ModifiedOn = Clock.Now;
            await UpdateAsync(existingpayment);
            if (existingpayment.Id != null)
            {
                // new WalletMgt(_currentUserService).GetWalletModel(model.Amount, ref existingwallet, (int)TransactionType.CREDIT);
                var walletdto = new WalletTransferDto
                {
                    Amount = model.Amount,
                    CreatedBy = guidId, 
                    CreatedOn = Clock.Now,
                    CustomerId = guidId,
                    CreditWalletId = existingwallet.Id,
                    PaymentReference = model.PaymentReference,
                    TransactionDesc = model.TransactionDesc,
                    ModifiedBy = guidId, 
                    ModifiedOn = Clock.Now,
                    NewBalance = existingwallet.AvailableBalance + model.Amount
                };

                var creditWallet = await _customerWalletService.DebitCreditWalletAsync(walletdto, (int)TransactionType.CREDIT);
                string responseCode = creditWallet != null && creditWallet.Any() ? ApiResponseCodes.OK.GetDescription() : ApiResponseCodes.FAILED.GetDescription();
                string responseMessage = creditWallet != null && creditWallet.Any() ? "Successful" : "Wallet credited before with this reference, please check balance";
                decimal availablebalance = creditWallet != null && creditWallet.Any() ? creditWallet.FirstOrDefault().AvailableBalance : existingwallet.AvailableBalance;


                return creditWallet != null && creditWallet.Any() ? new ApiResponse<CustomerWallet>(message: "Successful",
                                codes: ApiResponseCodes.OK, data: creditWallet?.FirstOrDefault())
                    : new ApiResponse<CustomerWallet>(errors: "Could not fund wallet", codes: ApiResponseCodes.FAILED);
            }

            return new ApiResponse<CustomerWallet>(codes: ApiResponseCodes.EXCEPTION, errors: "Wallet credit failed");

            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        public async Task<ApiResponse<PaymentDetaildto>> GetPaymentDetailAsync(string paymentreference)
        {
            try
            {
                var response = new ApiResponse<PaymentDetaildto> { Code = ApiResponseCodes.OK };
                var obj = new PayStackbject();
                var objV = new PayStackbjectV();

                if (paymentreference == null)
                {
                    return new ApiResponse<PaymentDetaildto>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
                }

                // await Task.Run(() => { });
                //var existingpayment = FirstOrDefault(p => string.Equals(paymentreference, p?.Reference, StringComparison.OrdinalIgnoreCase));
                IDictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "Reference", paymentreference }
            };
                string gateway_message = "";
                var existingpayment = Find(@"SELECT * FROM PaymentDetail WHERE Reference = @Reference", parameter)?.FirstOrDefault();
                if (existingpayment == null)
                {
                    return new ApiResponse<PaymentDetaildto>(errors: "Payment record not found", codes: ApiResponseCodes.INVALID_REQUEST);
                }
                if (string.IsNullOrWhiteSpace(existingpayment.TransStatus))
                {
                    //call payment gateway for payment validation.                       
                    string Url = configuration.GetValue<string>("AppSettings:PayStackKeys:paystack_gateway");
                    string key = configuration.GetValue<string>("AppSettings:PayStackKeys:paystack_secret_key");

                    Url = Url.Replace("#paymentReference", paymentreference);
                    var paystackresponse = await ExternalServiceCall.Post(Url, key);

                    try
                    {
                        objV = paystackresponse != null ? JsonConvert.DeserializeObject<PayStackbjectV>(paystackresponse) :
                                                           new PayStackbjectV { status = false, message = "No response from Payment Gateway" };

                        PropertyCopier<PayStackbjectV, PayStackbject>.Copy(objV, obj);
                        obj.data = new Data
                        {
                            amount = objV.data.amount,
                            status = objV.data.status
                        };
                    }
                    catch //(Exception)
                    {
                        obj = paystackresponse != null ? JsonConvert.DeserializeObject<PayStackbject>(paystackresponse) :
                                                           new PayStackbject { status = false, message = "No response from Payment Gateway" };                     
                    }

                    if (obj.status)
                    {
                        double paystackamount = Convert.ToDouble(obj.data?.amount.Replace(",", "")) / 100.0;
                        if (paystackamount != Convert.ToDouble(existingpayment.Amount))
                        {
                            //return new ApiResponse<PaymentDetail>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "Inconsistence in transaction amount");
                            obj.message = "Inconsistence in transaction amount";
                            obj.status = false;
                        }
                    }
                    //else
                    //{
                    //    return new ApiResponse<PaymentDetail>(codes: ApiResponseCodes.INVALID_REQUEST, errors: obj.message);
                    //}
                    if (obj.data != null)
                    {
                        gateway_message = obj.data.status;
                        existingpayment.TransStatus = obj.data.status.Equals("success") ? ApiResponseCodes.OK.GetDescription() : ApiResponseCodes.FAILED.GetDescription();
                        existingpayment.Gateway_response = paystackresponse;
                        existingpayment.Message = obj.message; // paystackresponse ?? "No response from Payment Gateway";
                        existingpayment.ModifiedOn = Clock.Now;
                        existingpayment.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
                    }
                    //var result = await UpdateAsync(existingpayment);
                    await UpdateAsync(existingpayment);
                    //return new ApiResponse<PaymentDetail>(message: existingpayment.Message, codes: ApiResponseCodes.OK, data: existingpayment);
                    existingpayment.Gateway_response = string.Empty;
                }
                //for exisiting record that has been verified before.
                else
                {
                    obj.status = true;
                    gateway_message = existingpayment.TransStatus.ToLower();
                }

                var viewModel = new PaymentDetaildto();
                PropertyCopier<PaymentDetail, PaymentDetaildto>.Copy(existingpayment, viewModel);

                if (obj.status && gateway_message.Equals("success"))
                    return new ApiResponse<PaymentDetaildto>(message: viewModel.Message, codes: ApiResponseCodes.OK, data: viewModel);

                return new ApiResponse<PaymentDetaildto>(codes: obj.status ? ApiResponseCodes.FAILED : ApiResponseCodes.NOT_FOUND, errors: gateway_message);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaymentDetaildto>(codes: ApiResponseCodes.EXCEPTION, errors: ex.Message);
            }
        }
    }
}
