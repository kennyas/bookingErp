using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Dapper.Services;
//using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;
using Wallet.Core.Dtos;
using Wallet.Core.Models;
using Wallet.Core.ViewModels;

namespace Wallet.Core.Services.Interfaces
{
    public interface IPaymentService : IService<PaymentDetail>
    {
        Task<ApiResponse<PaymentDetailViewModel>> CreatePaymentDetailAsync(CreatePaymentDetailDto model, string ip);        
        Task<ApiResponse<PaymentDetaildto>> GetPaymentDetailAsync(string paymentreference);
        Task<ApiResponse<CustomerWallet>> GetWalletBalance(Guid customerId);
        Task<ApiResponse<CustomerWalletDto>> NameEnquiryAsync(string mobile_number);
        Task<ApiResponse<CustomerWallet>> CreditCustomerWalletAsync(CreditWalletViewModel model);
        Task<ApiResponse<WalletDebitDto>> DebitCustomerWalletAsync(CustomerWalletViewModel model);
        Task<ApiResponse<CustomerWallet>> CreateCustomerWalletAsync(CreateWalletViewModel model);
        Task<ApiResponse<WalletTransferDto>> CustomerWalletTransferAsync(WalletTransferViewModel model);
        Task<ApiResponse<IEnumerable<WalletTransHistory>>> CustomerWalletTransHistoryAsync(WalletHistoryViewModel model); 
    }
}
