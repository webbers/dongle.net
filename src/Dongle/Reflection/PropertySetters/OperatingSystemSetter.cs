using Dongle.System;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Define o nome do sistema operacional.
    /// </summary>
    public class OperatingSystemSetter : PropertySetterBase
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public OperatingSystemSetter(FieldMapData fieldMap) : base(fieldMap)
        {
        }

        public override string Get(object obj)
        {
            return GetStringValue(obj);
        }

        public override void Set(object obj, string value)
        {
            var osInfo = OsVersion.GetFromVersion(value);
            if (FieldMap.SetterParameters == "name")
            {
                SetValue(obj, osInfo.Name);
            }
            else if (FieldMap.SetterParameters == "shortname")
            {
                SetValue(obj, osInfo.ShortName);
            }
            else if (FieldMap.SetterParameters == "producttype")
            {
                SetValue(obj, osInfo.ProductType);
            }
            else
            {
                SetValue(obj, osInfo.Version);
            }            
        }        
    }
}
