using System;
using System.ComponentModel;
using System.Reflection;

namespace Tornado.Shared.Enums
{
    public enum ApiResponseCodes
    {
        EXCEPTION = -5,
        [Description("Unauthorized Access")]
        UNAUTHORIZED = -4,
        NOT_FOUND = -3,
        INVALID_REQUEST = -2,
        [Description("Server error occured, please try again.")]
        ERROR = -1,
        [Description("FAILED")]
        FAILED = 2,
        [Description("SUCCESS")]
        OK = 1,

    }
    public static class ResponseCodeHelper
    {
        public const int OK = 200;

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    var attr =
                        Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return value.ToString();
        }
    }
}
