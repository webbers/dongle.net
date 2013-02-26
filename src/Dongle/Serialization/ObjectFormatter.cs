using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;

using Dongle.Resources;

namespace Dongle.Serialization
{
    public static class ObjectFormatter
    {
        public static string Format(object value, CultureInfo cultureInfo, bool useTextQualifier = true)
        {
            string retVal;
            if (value is IEnumerable && !(value is string))
            {
                retVal = ((IEnumerable)value).Cast<object>().Aggregate("", (current, obj) => current + (RawFormat(obj, cultureInfo) + ", "));
                if (retVal.Length > 0)
                {
                    retVal = retVal.Substring(0, retVal.Length - 2);
                }
            }
            else
            {
                retVal = RawFormat(value, cultureInfo);
            }
            if (useTextQualifier && (retVal.Contains(",") || retVal.Contains("\"")))
            {
                retVal = '"' + retVal.Replace("\"", "\"\"") + '"';
            }
            return retVal;
        }

        private static string RawFormat(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return String.Empty;
            }
            if (value is INullable && ((INullable)value).IsNull)
            {
                return String.Empty;
            }
            if (value is DateTime)
            {
                if (Math.Abs(((DateTime)value).TimeOfDay.TotalSeconds - 0) < float.Epsilon)
                {
                    return ((DateTime)value).ToString("yyyy-MM-dd");
                }
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (value is bool)
            {
                if ((bool)value)
                {
                    return DongleResource.ResourceManager.GetString("True", cultureInfo).ToUpper();
                }
                return DongleResource.ResourceManager.GetString("False", cultureInfo).ToUpper();
            }
            return String.Format(cultureInfo, "{0:G}", value);
        }
    }
}