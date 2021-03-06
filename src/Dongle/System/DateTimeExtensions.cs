﻿using System;
using System.Globalization;
using Dongle.Resources;

namespace Dongle.System
{
    public static class DateTimeExtensions
    {
        public static DateTime FromDosTimeStampToDateTime(this long timestamp)
        {
            var date = (timestamp & 0xFFFF0000) >> 16;
            var time = (timestamp & 0x0000FFFF);
            var year = (date >> 9) + 1980;
            var month = (date & 0x01e0) >> 5;
            var day = date & 0x1F;
            var hour = time >> 11;
            var minute = (time & 0x07e0) >> 5;
            var second = (time & 0x1F) * 2;
            return new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second);
        }

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
                return completeDays > 1 ? String.Format(DongleResource.XDaysAgo, completeDays) : DongleResource.Yesterday;
            }
            if (timeSpan.Hours <= 0)
            {
                if (timeSpan.Minutes > 1)
                {
                    return String.Format(DongleResource.XMinutesAgo, timeSpan.Minutes);
                }
                return timeSpan.Minutes == 1 ? DongleResource.OneMinuteAgo : DongleResource.FewSecondsAgo;
            }
            if (timeSpan.Hours > 1)
            {
                return String.Format(DongleResource.XHoursAgo, timeSpan.Hours);
            }
            return DongleResource.OneHourAgo;
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

        /// <summary>
        /// Coverte uma data para string no formato especificado
        /// </summary>
        public static string ToString(this DateTime? value, string format)
        {
            return !value.HasValue ? "" : value.Value.ToString(format);
        }
    }
}
