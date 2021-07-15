using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;

namespace Tornado.Shared.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null) ?? DBNull.Value;
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static string ArrayToCommaSeparatedString(this string[] array)
        {
            var newArray = new string[array.Length];
            var i = 0;

            foreach (var s in array)
            {
                newArray.SetValue(s.SeperateWords(), i);
                i++;
            }
            return string.Join(",", newArray);
        }
        public static string SeperateWords(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            string output = "";
            char[] chars = str.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == chars.Length - 1 || i == 0 || Char.IsWhiteSpace(chars[i]))
                {
                    output += chars[i];
                    continue;
                }

                if (char.IsUpper(chars[i]) && Char.IsLower(chars[i - 1]))
                    output += " " + chars[i];
                else
                    output += chars[i];
            }

            return output;
        }
        public static string UrlEncode(this string src)
        {
            if (src == null)
                return null;
            return HttpUtility.UrlEncode(src);
        }
    }
}
