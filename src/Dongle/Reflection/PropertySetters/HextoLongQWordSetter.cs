using System;
using Dongle.System;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Converte uma string hexadecimal para um int64
    /// </summary>
    public class HexToLongQWordSetter: PropertySetterBase
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public HexToLongQWordSetter(FieldMapData fieldMap) : base(fieldMap)
        {
        }

        public override string Get(object obj)
        {
            return ((long)FieldMap.Property.GetValue(obj, null)).ToHexFast();
        }

        public override void Set(object obj, string value)
        {
            Int64 newValue;
            var parseSuccess = Int64.TryParse(value, global::System.Globalization.NumberStyles.HexNumber, null, out newValue);
            if (parseSuccess)
            {
                SetValue(obj, newValue);
            }
        }
    }
}
