namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Verifica o tamanho de uma string antes de inserí-la, garantindo que não seja maior que o tamanho do campo
    /// </summary>
    public class VarCharSetter: PropertySetterBase
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public VarCharSetter(FieldMapData fieldMap) : base(fieldMap)
        {
        }

        /// <summary>
        /// Retorna o valor do campo
        /// </summary>
        public override string Get(object obj)
        {
            return GetStringValue(obj);
        }

        /// <summary>
        /// Define o valor do campo
        /// </summary>
        public override void Set(object obj, string value)
        {                    
            SetValue(obj, value);
        }
    }
}
