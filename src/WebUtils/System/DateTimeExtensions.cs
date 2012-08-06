using System;
using System.Globalization;
using WebUtils.Resources;

namespace WebUtils.System
{
    public static class DateTimeExtensions
    {
        public static DateTime FromTimeStampToDateTime(this long timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddMilliseconds(timestamp).ToLocalTime();
        }

        public static long ToTimeStamp(this DateTime date)
        {
            return (date.ToFileTimeUtc() / 10000) - 11644462800000;
        }

        public static string ToRssTime(this DateTime date)
        {
            return date.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
        }

        public static string ToFriendlyString(this TimeSpan timeSpan)
        {
            var completeDays = Math.Floor(timeSpan.TotalDays);
            if (completeDays >= 1)
            {
                return completeDays > 1 ? String.Format(WebUtilsResource.XDaysAgo, completeDays) : WebUtilsResource.Yesterday;
            }
            if (timeSpan.Hours <= 0)
            {
                if (timeSpan.Minutes > 1)
                {
                    return String.Format(WebUtilsResource.XMinutesAgo, timeSpan.Minutes);
                }
                return timeSpan.Minutes == 1 ? WebUtilsResource.OneMinuteAgo : WebUtilsResource.FewSecondsAgo;
            }
            if (timeSpan.Hours > 1)
            {
                return String.Format(WebUtilsResource.XHoursAgo, timeSpan.Hours);
            }
            return WebUtilsResource.OneHourAgo;
        }

        public static string ToFriendlyString(this DateTime datetime, DateTime? now = null)
        {
            if (!now.HasValue)
            {
                now = DateTime.Now;
            }

            var timeSpan = now.Value - datetime;

            string result;
            if (datetime.Day == now.Value.Day &&
                datetime.Month == now.Value.Month &&
                datetime.Year == now.Value.Year)
            {
                result = datetime.ToString("HH:mm");
            }
            else if (datetime.Year == now.Value.Year)
            {
                result = datetime.ToString("dd MMM");
            }
            else
            {
                return datetime.ToString();
            }
            if (timeSpan.Days < 7)
            {
                return result + " (" + timeSpan.ToFriendlyString() + ")";
            }
            return result;
        }

        public static int WorkDays(this DateTime initalDate, DateTime finalDate)
        {
            var allDays = finalDate < initalDate ? (initalDate - finalDate).Days : (finalDate - initalDate).Days;
            var workDays = 0;

            for (var i = 0; i < allDays; i++)
            {
                var currentDate = initalDate.AddDays(i);
                if (currentDate.DayOfWeek != DayOfWeek.Sunday && currentDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    workDays++;
                }
            }
            return workDays;
        }

        public static DateTime FromLogDateToDateTime(this String str)
        {
            try
            {
                return DateTime.ParseExact(str, "dd/MMM/yyyy:HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return DateTime.ParseExact(str, "dd/MMM/yyyy:HH:mm:ss", new CultureInfo("pt-BR"));
            }
        }
    }
}
