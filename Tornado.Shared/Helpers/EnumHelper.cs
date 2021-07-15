using System;
using System.ComponentModel;

namespace Tornado.Shared.Helpers
{
    public static class EnumHelper
    {
        public static T Parse<T>(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return default(T);
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
