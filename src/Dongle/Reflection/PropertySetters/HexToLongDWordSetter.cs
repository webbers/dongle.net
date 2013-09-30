using System;
using System.Globalization;
using Dongle.System;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Converte uma string hexadecimal para um int32
    /// </summary>
    public class HexToLongDWordSetter: PropertySetterBase
    {        
        /// <summary>
        /// Construtor
        /// </summary>
        public HexToLongDWordSetter(FieldMapData fieldMap) : base(fieldMap)
        {            
        }

        public override string Get(object obj)
        {
            return ((long)FieldMap.Property.GetValue(obj, null)).ToHexFast();
        }

        public override void Set(object obj, string value)
        {
            try
            {
                int newValue;
                if (int.TryParse(value, NumberStyles.HexNumber, null, out newValue)) //Parse com Int32 para virar um DWORD
                {
                    SetValue(obj, (long)newValue);
                }
                else
                {
                    long longValue;
                    if (long.TryParse(value, NumberStyles.HexNumber, null, out longValue)) //Não deu, tenta fazer o Parse com Int64
                    {
                        SetValue(obj, longValue);
                    }
                }
            }
            catch (Exception)
            {
                long newValue;
                var parseSuccess = long.TryParse(value, NumberStyles.HexNumber, null, out newValue);
                if (parseSuccess)
                {
                    SetValue(obj, newValue);
                }
            }
        }
    }
}