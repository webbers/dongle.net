using System;
using System.Collections.Generic;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Setter mais básico de todos. Apenas transfere o valor para o objeto.
    /// </summary>
    public class BypassSetter : PropertySetterBase
    {
        private static readonly Dictionary<Type, MemberTypeEnum> MemberTypeConvertion = new Dictionary<Type, MemberTypeEnum>
        {
            { typeof(string), MemberTypeEnum.String } ,
            { typeof(int), MemberTypeEnum.Int32},
            { typeof(int?), MemberTypeEnum.Int32},
            { typeof(long), MemberTypeEnum.Int64},
            { typeof(long?), MemberTypeEnum.Int64},
            { typeof(short), MemberTypeEnum.Int16},
            { typeof(short?), MemberTypeEnum.Int16},
            { typeof(bool), MemberTypeEnum.Boolean},
            { typeof(bool?), MemberTypeEnum.Boolean},
            { typeof(byte), MemberTypeEnum.Byte},
            { typeof(byte?), MemberTypeEnum.Byte},
            { typeof(Guid), MemberTypeEnum.Guid},
            { typeof(Guid?), MemberTypeEnum.Guid},
            { typeof(DateTime), MemberTypeEnum.DateTime},
            { typeof(DateTime?), MemberTypeEnum.DateTime},
            { typeof(double), MemberTypeEnum.Double},
            { typeof(double?), MemberTypeEnum.Double},
            { typeof(decimal), MemberTypeEnum.Decimal},
            { typeof(decimal?), MemberTypeEnum.Decimal},
        };

        /// <summary>
        /// Tipo da propriedade ou campo
        /// </summary>
        public MemberTypeEnum MemberType { get; set; }

        /// <summary>
        /// Informa se a propriedade é nullable
        /// </summary>
        public bool MemberIsNullable { get; set; }

        /// <summary>
        /// Tipo da propriedade ou campo
        /// </summary>
        public enum MemberTypeEnum
        {
            /// <summary>
            /// String
            /// </summary>
            String,
            /// <summary>
            /// Int / Int32
            /// </summary>
            Int32,
            /// <summary>
            /// Int64 / long
            /// </summary>
            Int64,
            /// <summary>
            /// Int16
            /// </summary>
            Int16,
            /// <summary>
            /// Bool
            /// </summary>
            Boolean,
            /// <summary>
            /// Byte
            /// </summary>
            Byte,
            /// <summary>
            /// Guid
            /// </summary>
            Guid,
            /// <summary>
            /// DateTime
            /// </summary>
            DateTime,
            /// <summary>
            /// Double
            /// </summary>
            Double,
            /// <summary>
            /// Decimal
            /// </summary>
            Decimal
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public BypassSetter(FieldMapData fieldMap) : base(fieldMap)
        {
            SetMemberType();
        }                

        private void SetMemberType()
        {
            var typeOfT = FieldMap.MemberIsField ? FieldMap.Field.FieldType : FieldMap.Property.PropertyType;
            MemberTypeEnum localMemberType;
            if (MemberTypeConvertion.TryGetValue(typeOfT, out localMemberType))
            {
                MemberType = localMemberType;
            }
            else
            {
                MemberType = MemberTypeEnum.String;
            }            
            MemberIsNullable = typeOfT.FullName.StartsWith("System.Nullable");
        }

        public override void Set(object obj, string value)
        {
            if (MemberType == MemberTypeEnum.String)
            {
                SetValue(obj, value);
            }
            else
            {
                SetValue(obj, GetNewValue(value));
            }
        }

        private object GetNewValue(object value)
        {
            if (value == null && MemberType != MemberTypeEnum.String && !MemberIsNullable)
            {
                value = "";
            }
            object newValue ;
            if (value == null)
            {
                newValue = null;
            }
            else if (MemberType == MemberTypeEnum.String)
            {
                newValue = value.ToString();
            }
            else if (MemberType == MemberTypeEnum.Int32)
            {
                int v;
                int.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.Int64)
            {
                long v;
                long.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.Int16)
            {
                short v;
                short.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.Boolean)
            {
                if (value.ToString() == "1")
                {
                    newValue = true;
                }
                else if (value.ToString() == "0")
                {
                    newValue = false;
                }
                else
                {
                    bool v;
                    bool.TryParse(value.ToString(), out v);
                    newValue = v;
                }
            }
            else if (MemberType == MemberTypeEnum.Byte)
            {
                byte v;
                byte.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.Guid)
            {
                Guid v;
                Guid.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.DateTime)
            {
                DateTime v;
                DateTime.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.Double)
            {
                double v;
                double.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else if (MemberType == MemberTypeEnum.Decimal)
            {
                decimal v;
                decimal.TryParse(value.ToString(), out v);
                newValue = v;
            }
            else
            {
                newValue = value.ToString();
            }
            return newValue;            
        }

        public override string Get(object obj)
        {
            return GetStringValue(obj);
        }
    }
}	