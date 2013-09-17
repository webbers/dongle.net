using System;
using System.Globalization;

namespace Dongle.Reflection.PropertySetters
{
    public class EnumSetter<T> : PropertySetterBase where T : struct 
    {
        public EnumSetter(FieldMapData fieldMap) : base(fieldMap)
        {
        }

        public override void Set(object obj, string value)
        {
            T newValue;
            if(Enum.TryParse(value, out newValue))
            {
                SetValue(obj, newValue);
            }
        }

        public override string Get(object obj)
        {
            return ((int)GetValue(obj)).ToString(CultureInfo.InvariantCulture);
        }
    }
}
