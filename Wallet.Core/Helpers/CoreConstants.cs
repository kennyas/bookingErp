using System;
using System.Collections.Generic;
using System.Text;

namespace Wallet.Core.Helpers
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";

        public class TransactionType
        {
            public const string WalletDebit = "Wallet Debit";
            public const string WalletTransfer = "Wallet Transfer";
        }
    }

    
}
