using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Filters;
using Tornado.Shared.AuditLogEvent;
using Tornado.Shared.AuditLogEvent.Interface;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;
using Wallet.Core.Dtos;
using Wallet.Core.Events;
using Wallet.Core.Models;
using Wallet.Core.Services.Interfaces;
using Wallet.Core.ViewModels;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    // [ApiController]
   // [AllowAnonymous]
    public class WalletApiController : BaseController
    {
        private readonly IPaymentService _paymentService;
        // private readonly IWalletEventService _walletEventService;
        private readonly IAuditLogEventService _auditLogEventService;
        private IHttpUserService _currentUserService;

        public WalletApiController(IPaymentService paymentService, IAuditLogEventService auditLogEventService, IHttpUserService currentUserService)
        {
            _paymentService = paymentService;
            _auditLogEventService = auditLogEventService;
            _currentUserService = currentUserService;
        }

        [ServiceFilter(typeof(GIGAuditLogAttribute))]
        [HttpPost]
        public async Task<ApiResponse<CustomerWallet>> CreateCustomerWallet([FromBody]CreateWalletViewModel model)
        {
            await AuditLogPublisher.PublishEventAsync(Request, _currentUserService, _auditLogEventService);
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.CreateCustomerWalletAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [ServiceFilter(typeof(GIGAuditLogAttribute))]
        [HttpPost]
        public async Task<ApiResponse<PaymentDetailViewModel>> CreatePaymentDetail([FromBody]CreatePaymentDetailDto model)
        {
            await AuditLogPublisher.PublishEventAsync(Request, _currentUserService, _auditLogEventService);
            return await HandleApiOperationAsync(async () =>
            {
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                return await _paymentService.CreatePaymentDetailAsync(model, ip).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaymentDetaildto>> VerifyPayment(string paymentreference)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.GetPaymentDetailAsync(paymentreference).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<CustomerWalletDto>> WalletNameEnquiry(string phoneNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.NameEnquiryAsync(phoneNumber).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<CustomerWallet>> GetWalletBalance(Guid userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.GetWalletBalance(userId).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [ServiceFilter(typeof(GIGAuditLogAttribute))]
        [HttpPost]
        public async Task<ApiResponse<CustomerWallet>> FundCustomerWallet([FromBody]CreditWalletViewModel model)
        {
            await AuditLogPublisher.PublishEventAsync(Request, _currentUserService, _auditLogEventService);
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.CreditCustomerWalletAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [ServiceFilter(typeof(GIGAuditLogAttribute))]
        [HttpPost]
        public async Task<ApiResponse<WalletDebitDto>> DebitCustomerWallet([FromBody]CustomerWalletViewModel model)
        {
            await AuditLogPublisher.PublishEventAsync(Request, _currentUserService, _auditLogEventService);
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.DebitCustomerWalletAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [ServiceFilter(typeof(GIGAuditLogAttribute))]
        [HttpPost]
        public async Task<ApiResponse<WalletTransferDto>> WalletTransfer([FromBody]WalletTransferViewModel model)
        {
            await AuditLogPublisher.PublishEventAsync(Request, _currentUserService, _auditLogEventService);
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.CustomerWalletTransferAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<IEnumerable<WalletTransHistory>>> WalletTransHistory([FromBody]WalletHistoryViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _paymentService.CustomerWalletTransHistoryAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}