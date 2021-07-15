using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Helpers
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string DateTimeFormat = "dd MMMM, yyyy hh:mm tt";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";

        public const int PageIndex = 1;
        public const int PageTotal = 50;
    }
}