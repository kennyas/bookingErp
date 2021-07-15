using System;
using System.Collections.Generic;
using System.Text;

namespace Notify.Core.Utils
{
   public static class StringExtensions
    {
        public static string ToNigeriaMobile(this string str)
        {
            const string naijaPrefix = "234";
            if (string.IsNullOrEmpty(str))
                return str;

            str = str.TrimStart('+');
            var prefix = str.Remove(3);

            if (prefix.Equals(naijaPrefix))
            {
                return str;
            }
            str = str.TrimStart('0');
            str = naijaPrefix + str;
            return str;
        }
    }
}
