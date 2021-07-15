using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Timing;
using Wallet.Core.Enums;
using Wallet.Core.Models;
using Wallet.Core.ViewModels;

namespace Wallet.Core.Utility
{
    public class WalletMgt
    {
        private static IHttpUserService _currentUserService;
        private static readonly Random random = new Random();
        public WalletMgt(IHttpUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        //public void GetWalletModel(decimal Amount, ref CustomerWallet wallet, int transType)
        //{
        //    // return new CustomerWallet
        //    // {
        //    wallet.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
        //    wallet.ModifiedOn = Clock.Now;
        //    wallet.AvailableBalance = transType == (int)TransactionType.CREDIT ? Amount + wallet.AvailableBalance : wallet.AvailableBalance - Amount;
        //    wallet.LedgerBalance = transType == (int)TransactionType.CREDIT ? Amount + wallet.LedgerBalance : wallet.LedgerBalance - Amount;
        //    //   CustomerId = wallet.CustomerId,
        //    //   Id = wallet.Id
        //    // };
        //    //  return wallet;
        //}

        public static string RandomString(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                              .Select(s => s[random.Next(s.Length)]).ToArray()).ToLower();
        }

        public bool IsFundSufficient(decimal transamount, decimal walletbalance, decimal BlockedBalance)
        {
            return (walletbalance - BlockedBalance) > transamount;
        }
    }
}
