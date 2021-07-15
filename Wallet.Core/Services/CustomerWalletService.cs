using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Dapper.Interfaces;
using Tornado.Shared.Dapper.Services;
using Wallet.Core.Dtos;
using Wallet.Core.Enums;
//using Tornado.Shared.EF;
//using Tornado.Shared.EF.Services;
using Wallet.Core.Models;
using Wallet.Core.Services.Interfaces;
using Wallet.Core.ViewModels;

namespace Wallet.Core.Services
{
    public class CustomerWalletService : Service<CustomerWallet>, ICustomerWalletService
    {
        //private readonly ICurrentUserService _currentUserService;

        public CustomerWalletService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            // _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<CustomerWalletDto>> GetCustomerWallet(string mobile_number)
        {
            DynamicParameters _params = new DynamicParameters();
            _params.AddDynamicParams(new
            {
                mobile_number
            });
            string sql = "Sp_GetCustomerWalletByMobileNo";
            var result = await ExecuteStoredProcedure<CustomerWalletDto>(sql, _params);
            return result.ToList();
        }
        public async Task<bool> AddCustomerWalletAsync(CustomerWallet model)
        {
            if (model == null) return false;
            await AddAsync(model);
            return model.Id != null;
        }
        public CustomerWallet GetCustomerWallet(Guid customerId)
        {

            //CustomerWallet existingwallet = Find().FirstOrDefault(p => Equals(customerId, p?.CustomerId));
            IDictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "customerId", customerId }
            };

            CustomerWallet existingwallet = Find(@"SELECT * FROM CustomerWallet WHERE customerId = @customerId", parameter)?.FirstOrDefault();
            return existingwallet;
        }

        //public async Task<bool> UpdateCustomerWalletAsync(CustomerWallet model)
        //{
        //    if (model == null) return false;
        //    await UpdateAsync(model);
        //    return true; // result > 0;
        //}

        public async Task<IEnumerable<CustomerWallet>> CustomerWalletTransferAsync(WalletTransferDto model)
        {
            if (model == null) return new List<CustomerWallet>();
            DynamicParameters _params = new DynamicParameters();
            _params.AddDynamicParams(new
            {                
                model.PaymentReference,
                model.Amount,
                model.CreditWalletId,
                model.DebitWalletId,
                model.CustomerId,
                model.CreatedBy,
                model.CreatedOn,
                model.TransactionDesc,
                model.ModifiedOn,
                model.ModifiedBy,
                model.Reference
            });
            string sql = "Sp_WalletTransfer";
            var result = await ExecuteStoredProcedure<CustomerWallet>(sql, _params);
            return result;
        }

        public async Task<IEnumerable<CustomerWallet>> DebitCreditWalletAsync(WalletTransferDto model, int transType) 
        {
            if (model == null) return new List<CustomerWallet>();
            model.TransType = transType == (int)TransactionType.DEBIT ? "DR" : "CR";
            var walletId = transType == (int)TransactionType.DEBIT ? model.DebitWalletId : model.CreditWalletId;
            DynamicParameters _params = new DynamicParameters();
            _params.AddDynamicParams(new
            {
                model.PaymentReference,
                model.Amount,
                walletId,
                model.CustomerId,
                model.CreatedBy,
                model.CreatedOn,
                model.TransType,
                model.TransactionDesc,
                model.NewBalance,
                model.ModifiedBy,
                model.ModifiedOn,
                model.Reference
            });
            string sql = "Sp_WalletDebitCredit";
            var result = await ExecuteStoredProcedure<CustomerWallet>(sql, _params);
            return result;
        }
    }
}
