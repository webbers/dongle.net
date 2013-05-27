using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Dongle.System.IO;

namespace Dongle.Serialization
{
    /// <summary>
    /// Serialização em XML
    /// </summary>
    [Serializable]
    public static class XmlSimpleSerializer
    {
        /// <summary>
        /// Carrega o objeto do arquivo
        /// </summary>
        public static T UnserializeFromFile<T>(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fs))
            using (var memoryStream = new MemoryStream(StringToUtf8ByteArray(streamReader.ReadToEnd())))            
            {
                var xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(memoryStream);                
            }
        }

        /// <summary>
        /// Salva o objeto para um arquivo
        /// </summary>
        public static void SerializeToFile<T>(T obj, string filename)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                var xmlTextWriter = new XmlTextWriter(memoryStream, DongleEncoding.Default) {Formatting = Formatting.Indented};
                serializer.Serialize(xmlTextWriter, obj);
                var baseStream = (MemoryStream)xmlTextWriter.BaseStream;
                var xmlText = Utf8ByteArrayToString(baseStream.ToArray());
                File.WriteAllText(filename, xmlText, DongleEncoding.Default);
            }
        }

        private static string Utf8ByteArrayToString(Byte[] data)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(data);
            return (constructedString);
        }

        private static byte[] StringToUtf8ByteArray(String text)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(text);
            return byteArray;
        }
    }
}
