using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Timing;

namespace Tornado.Shared.Utils
{
    public static class DateHelper
    {
        public static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() -
                              new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                             .TotalSeconds);


        public static string ToTimeString(this TimeSpan time)
        {
            var current = DateTime.Today + time;
            return current.ToString("hh:mm tt");
        }

        public static DateTime GetStartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return dateTime.Date.AddTicks(-1).AddDays(1);
        }

        public static double GetHoursTillEndOfDay(this DateTime dateTime)
        {
            return (Clock.Now.GetEndOfDay() - dateTime).Hours;
        }

        public static int GetMillisecondsTillEndOfDay(this DateTime dateTime)
        {
            return (Clock.Now.GetEndOfDay() - dateTime).Milliseconds;
        }

        public static TimeSpan GetTimespanTillEndOfDay(this DateTime dateTime)
        {
            return (Clock.Now.GetEndOfDay() - dateTime);
        }
    }
}