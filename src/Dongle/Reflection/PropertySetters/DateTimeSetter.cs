using System;
using System.Globalization;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Converte uma string no formato 20/Dez/2010:00:02:52 (entre outros) para um DateTime.   
    /// </summary>
    public class DateTimeSetter : PropertySetterBase
    {
        private static readonly CultureInfo PtbrCulture = new CultureInfo("pt-BR");

        private readonly static string[] Formats =
        {
            "dd/MMM/yyyy:HH:mm:ss",
            "dd/MMM/yyyy:HH:mm:ss zz00",
            "yyyy/MM/dd HH:mm:ss",
            "yyyy/dd/MM HH:mm:ss",
            "dd/MM/yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss"
        };

        /// <summary>
        /// Construtor
        /// </summary>
        public DateTimeSetter(FieldMapData fieldMap)
            : base(fieldMap)
        {
        }

        public static DateTime? GetValue(string value)
        {
            foreach (var format in Formats)
            {
                DateTime date;
                var successParse = DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date);

                if (successParse)
                {
                    return date;
                }
                if (DateTime.TryParseExact(value, format, PtbrCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
            }
            return null;
        }

        public override void Set(object obj, string value)
        {
            if (TrySet(obj, value, FieldMap.SetterParameters))
            {
                return;
            }
            foreach (var format in Formats)
            {
                if (TrySet(obj, value, format))
                {
                    return;
                }
            }
            //Não conseguiu com nenhum formato. Pode ser que tenha vindo no formato "epoch" (RT 20006).
            long ticks;
            if (!string.IsNullOrEmpty(value) && long.TryParse(value, out ticks))
            {
                if (ticks == 11111111) //RT 20028
                {
                    return;
                }
                SetValue(obj, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ticks));
            }
        }

        private bool TrySet(object obj, object value, string format)
        {
            DateTime date;
            var successParse = DateTime.TryParseExact(value.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date);
            if (!successParse)
            {
                if (!DateTime.TryParseExact(value.ToString(), format, PtbrCulture, DateTimeStyles.None, out date))
                    return false;
            }
            SetValue(obj, date);
            return true;
        }

        public override string Get(object obj)
        {
            var value = GetValue(obj);
            return value == null ? "" : ((DateTime)value).ToString(FieldMap.SetterParameters);
        }
    }
}
