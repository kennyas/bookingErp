using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Tornado.Shared.Enums;

namespace Tornado.Shared.Extensions
{
    public static class ConfigExtensions
    {
        public static bool ToBool(this string value, ConfigDataTypes type)
        {
            if (type != ConfigDataTypes.BOOLEAN)
                throw new InvalidOperationException("The configuration type must be 'bool' in the database to run this method");
            if (!bool.TryParse(value, out bool result))
                throw new InvalidOperationException("Could not convert string value to bool");

            return result;
        }
        public static double ToNumber(this string value, ConfigDataTypes type)
        {
            if (type != ConfigDataTypes.NUMBER)
                throw new InvalidOperationException("The configuration type must be 'number' in the database to run this method");

            if (!double.TryParse(value, out double result))
                throw new InvalidOperationException("Could not convert string value to number");

            return result;
        }

        public static List<string> ValuesArray(this string values, ConfigDataTypes type = ConfigDataTypes.STRING)
        {
            if (type != ConfigDataTypes.STRING)
                throw new InvalidOperationException("The configuration type must be 'string[]' in the database to run this method");

            return values.Split(',').ToList();
        }
        public static List<double> ToNumberCollection(this string values, ConfigDataTypes type = ConfigDataTypes.NUMBER)
        {
            List<double> doubleVals = default;

            if (type != ConfigDataTypes.NUMBER)
                throw new InvalidOperationException("The configuration type must be 'number' in the database to run this method");

            string[] vals = values.Split(',');

            Array.ForEach(vals, val => {
                if (double.TryParse(val, out double result))
                    doubleVals.Add(result);
            });

            return doubleVals;
        }

        //public static List<bool> ToBooleanCollection(this string values, ConfigDataTypes type = ConfigDataTypes.BOOLEAN)
        //{
        //    List<bool> boolVals = default;

        //    if (type != ConfigDataTypes.BOOLEAN)
        //        throw new InvalidOperationException("The configuration type must be 'bool[]' in the database to run this method");

        //    string[] vals = values.Split(',');

        //    Array.ForEach(vals, val => {
        //        if (bool.TryParse(val, out bool result))
        //            boolVals.Add(result);
        //    });

        //    return boolVals;
        //}

    }
}
