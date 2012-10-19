using System.IO;
using System.Xml;

namespace Dongle.Configuration
{
    /// <summary>
    /// Permite ler e escrever configurações em arquivos App.Config
    /// </summary>
    public static class AppConfig
    {
        private const string KeyPropertyName = "key";
        private const string AddElementName = "add";
        private const string ValuePropertyName = "value";

        /// <summary>
        /// Define o valor de uma configuração
        /// </summary>
        /// <param name="file">Caminho do arquivo (ex: c:\program.exe.config)</param>
        /// <param name="key">Nome da configuração</param>
        /// <param name="value">Valor a ser salvo</param>
        /// <returns></returns>
        public static bool SetValue(string file, string key, string value)
        {
            var fileDocument = new XmlDocument();
            fileDocument.Load(file);
            var nodes = fileDocument.GetElementsByTagName(AddElementName);

            if (nodes.Count == 0)
            {
                return false;
            }

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes.Item(i);
                if (node == null || node.Attributes == null || node.Attributes.GetNamedItem(KeyPropertyName) == null)
                    continue;
                
                if (node.Attributes.GetNamedItem(KeyPropertyName).Value == key)
                {
                    node.Attributes.GetNamedItem(ValuePropertyName).Value = value;
                }
            }

            var writer = new XmlTextWriter(file, null) { Formatting = Formatting.Indented };
            fileDocument.WriteTo(writer);
            writer.Flush();
            writer.Close();
            return true;
        }

        /// <summary>
        /// Obtém o valor de uma configuração
        /// </summary>
        /// <param name="file">Caminho do arquivo (ex: c:\program.exe.config)</param>
        /// <param name="key">Nome da configuração</param>
        /// <returns></returns>
        public static string GetValue(string file, string key)
        {
            if (!File.Exists(file))
            {
                return string.Empty;
            }

            var fileDocument = new XmlDocument();
            fileDocument.Load(file);
            var nodes = fileDocument.GetElementsByTagName(AddElementName);

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes.Item(i);
                if (node == null || node.Attributes == null)
                    continue;
                if (node.Attributes.GetNamedItem(KeyPropertyName)!= null && node.Attributes.GetNamedItem(KeyPropertyName).Value == key)
                {
                    return node.Attributes.GetNamedItem(ValuePropertyName).Value;
                }
            }
            return string.Empty;
        }
    }
}