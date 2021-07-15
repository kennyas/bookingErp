using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wallet.Core.Dtos;
using Wallet.Core.Models;
using Wallet.Core.ViewModels;

namespace Wallet.Core.Services.Interfaces
{
    public interface ICustomerWalletService
    {
        CustomerWallet GetCustomerWallet(Guid customerId);
        Task<IEnumerable<CustomerWalletDto>> GetCustomerWallet(string mobile_number);
        Task<bool> AddCustomerWalletAsync(CustomerWallet model);        
        Task<IEnumerable<CustomerWallet>> CustomerWalletTransferAsync(WalletTransferDto model);
        Task<IEnumerable<CustomerWallet>> DebitCreditWalletAsync(WalletTransferDto model, int transType);
    }
}
