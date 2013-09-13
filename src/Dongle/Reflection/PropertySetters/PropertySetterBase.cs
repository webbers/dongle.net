using System.Reflection;
using Dongle.System;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Base para os PropertySetters
    /// </summary>
    public abstract class PropertySetterBase
    {
        /// <summary>
        /// Dados sobre mapeamento de campos
        /// </summary>
        public class FieldMapData
        {
            /// <summary>
            /// Parãmetros do Setter
            /// </summary>
            public string SetterParameters { get; set; }

            /// <summary>
            /// Informações do campo a ser setado (se MemberIsField)
            /// </summary>
            public FieldInfo Field { get; set; }

            /// <summary>
            /// Informações da propriedade a ser setada
            /// </summary>
            public PropertyInfo Property { get; set; }

            /// <summary>
            /// Define que o membro a ser setado é um campo e não uma propriedade 
            /// </summary>
            public bool MemberIsField { get; set; }

            /// <summary>
            /// Tamanho máximo do campo
            /// </summary>
            public int MaxLength { get; set; }
            
            /// <summary>
            /// Construtor
            /// </summary>
            public FieldMapData()
            {
                
            }

            /// <summary>
            /// Construtor
            /// </summary>
            public FieldMapData(MemberInfo member, string setterParameters = null)
            {
                var info = member as PropertyInfo;
                if (info != null)
                {
                    Property = info;
                    MemberIsField = false;
                    MaxLength = Property.GetMaxLength();                    
                }
                else
                {
                    Field = (FieldInfo)member;
                    MemberIsField = true;
                    MaxLength = Field.GetMaxLength();                    
                }
                SetterParameters = setterParameters;
            }
        }

        /// <summary>
        /// Informações sobre o campo a ser utilizado
        /// </summary>
        public FieldMapData FieldMap;

        /// <summary>
        /// Define o valor do campo especificado no construtor
        /// </summary>
        public abstract void Set(object obj, string value);

        /// <summary>
        /// Obtém valor do campo especificado no construtor
        /// </summary>
        public abstract string Get(object obj);

        /// <summary>
        /// Construtor padrão
        /// </summary>
        protected PropertySetterBase(FieldMapData fieldMap)
        {
            FieldMap = fieldMap;
        }

        /// <summary>
        /// Retorna o valor do campo especificado no construtor
        /// </summary>
        protected string GetStringValue(object obj)
        {
            return (GetValue(obj) ?? "").ToString();
        }

        /// <summary>
        /// Obtém valor do campo especificado no construtor
        /// </summary>
        protected object GetValue(object obj)
        {
            if (FieldMap.MemberIsField)
            {                
                return FieldMap.Field.GetValue(obj);
            }
            return FieldMap.Property.GetValue(obj, null);
        }

        /// <summary>
        /// Define o valor do campo especificado no construtor
        /// </summary>
        protected void SetValue(object obj, string value)
        {
            SetObjectValue(obj, value.Limit(FieldMap.MaxLength));
        }

        /// <summary>
        /// Define o valor do campo especificado no construtor
        /// </summary>
        protected void SetValue(object obj, object value)
        {
            SetObjectValue(obj, value);
        }

        /// <summary>
        /// Define o valor do campo especificado no construtor
        /// </summary>
        private void SetObjectValue(object obj, object value)
        {
            if (FieldMap.MemberIsField)
            {
                FieldMap.Field.SetValue(obj, value);
            }
            FieldMap.Property.SetValue(obj, value, null);
        }               
    }
}
