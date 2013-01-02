using System.Globalization;
using System.IO;

using Dongle.System.IO;
using Newtonsoft.Json;

namespace Dongle.Serialization
{
    public static class JsonSimpleSerializer
    {
        public static void SerializeToFile<T>(FileInfo fileInfo, T obj)
        {
            fileInfo.Directory.CreateRecursively();
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
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
                using (var streamReader = new StreamReader(fileStream))
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

                var settings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    NullValueHandling = NullValueHandling.Ignore,
                    Culture = cultureInfo ?? CultureInfo.CurrentUICulture
                };
                return JsonConvert.DeserializeObject<T>(raw, settings);
            }
            return null;
        }
    }
}
