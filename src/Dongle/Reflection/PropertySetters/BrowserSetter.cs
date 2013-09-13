using System.Globalization;

namespace Dongle.Reflection.PropertySetters
{
    /// <summary>
    /// Separa o nome e versão do browser. Caso não venha a versão (browser desconhecido), seta toda a string como o nome do browser
    /// </summary>
    public class BrowserSetter : PropertySetterBase
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public BrowserSetter(FieldMapData fieldMap) : base(fieldMap)
        {
        }

        /// <summary>
        /// Define o valor do campo especificado no construtor
        /// </summary>
        public override void Set(object obj, string value)
        {
            var browser = value.Replace("[", "").Replace("]", "");
            var browserShortName = browser.Length >= 2 ? browser.Substring(0, 2).ToUpper() : browser;
            var browserName = browserShortName;
            var browserVersion = browser.Length > 2 ? browser.Substring(2) : "";
            var fieldType = FieldMap.SetterParameters.ToLower().Trim();
            
            if (fieldType == "name")
            {
                if (browserName == "MZ")
                {
                    browserName = "FIREFOX";
                }
                else if (browserName == "IE")
                {
                    browserName = "IEXPLORE";
                }
                else if (browserName == "NS")
                {
                    browserName = "NETSCAPE";
                }
                else if (browserName == "CR")
                {
                    browserName = "CHROME";
                }
                else if (browserName == "OP")
                {
                    browserName = "OPERA";
                }
                else if (browserName == "SA")
                {
                    browserName = "SAFARI";
                }
                else
                {
                    browserName = "*";
                }
                SetValue(obj, browserName);
                return;
            }
            if (fieldType == "shortname")
            {
                if (string.IsNullOrEmpty(browserShortName))
                {
                    browserShortName = "*";
                }
                SetValue(obj, browserShortName);                      
                return;
            }
            if (string.IsNullOrEmpty(browserVersion))
            {
                return;
            }
            byte b;
            if (byte.TryParse(browserVersion[0].ToString(CultureInfo.InvariantCulture), out b))
            {
                SetValue(obj, browserVersion);
            }
        }

        /// <summary>
        /// Obtém valor do campo especificado no construtor
        /// </summary>
        public override string Get(object obj)
        {
            return GetStringValue(obj);
        }
    }
}	
