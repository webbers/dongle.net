using System;
using System.Globalization;
using System.IO;
using System.Text;
using Dongle.System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dongle.Serialization
{
    public static class JsonSimpleSerializer
    {
        public static void SerializeToFile<T>(FileInfo fileInfo, T obj)
        {
            fileInfo.Directory.CreateRecursively();
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream, DongleEncoding.Default))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonTextWriter.Formatting = Formatting.None;
                jsonTextWriter.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                var serializer = new JsonSerializer();
                serializer.Serialize(jsonTextWriter, obj);
            }
        }

        public static T UnserializeFromFile<T>(FileInfo fileInfo) where T : class
        {
            if (fileInfo.Exists)
            {
                using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
                using (var streamReader = new StreamReader(fileStream, DongleEncoding.Default))
                {
                    string json = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            return null;
        }

        public static string SerializeToString<T>(T obj, CultureInfo cultureInfo = null)
        {
            var settings = new JsonSerializerSettings
                               {
                                   Formatting = Formatting.None,
                                   DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,                                   
                                   Culture = cultureInfo ?? CultureInfo.CurrentUICulture
                               };
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T UnserializeFromString<T>(string raw, CultureInfo cultureInfo = null) where T : class
        {
            if (raw != null)
            {
                //necessário para deserializar forms com checkbox
                //http://stackoverflow.com/questions/5462967/razor-viewengine-html-checkbox-method-creates-a-hidden-input-why
                raw = raw.Replace("[\"true\",\"false\"]", "\"true\"");

                var settings = GetDefatultJsonSerializerSettings(cultureInfo);

                return JsonConvert.DeserializeObject<T>(raw, settings);
            }
            return null;
        }

        public static Object UnserializeObject(string raw, Type type, CultureInfo cultureInfo = null)
        {
            if (raw != null)
            {
                //necessário para deserializar forms com checkbox
                //http://stackoverflow.com/questions/5462967/razor-viewengine-html-checkbox-method-creates-a-hidden-input-why
                raw = raw.Replace("[\"true\",\"false\"]", "\"true\"");

                var settings = GetDefatultJsonSerializerSettings(cultureInfo);
                
                return JsonConvert.DeserializeObject(raw, type, settings);
            }
            return null;
        }

        public static string GetNodeValueFromJson(string raw,string nodeName)
        {
            var nodeValue = JObject.Parse(raw);
            return (string)nodeValue[nodeName];
        }

        private static JsonSerializerSettings GetDefatultJsonSerializerSettings(CultureInfo cultureInfo)
        {
            var settings = new JsonSerializerSettings
                               {
                                   DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                                   NullValueHandling = NullValueHandling.Ignore,
                                   Culture = cultureInfo ?? CultureInfo.CurrentUICulture
                               };
            return settings;
        }
    }
}
