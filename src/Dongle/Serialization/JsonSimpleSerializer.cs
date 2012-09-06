using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Dongle.System.IO;

namespace Dongle.Serialization
{
    public static class JsonSimpleSerializer
    {
        public static void SerializeToFile<T>(FileInfo fileInfo, T obj)
        {
            fileInfo.Directory.CreateRecursively();
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Create))
            {
                serializer.WriteObject(fileStream, obj);
            }
        }

        public static T UnserializeFromFile<T>(FileInfo fileInfo) where T: class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            if (fileInfo.Exists)
            {
                using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    return (T) serializer.ReadObject(fileStream);
                }
            }
            return null;
        }

        public static string SerializeToString<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T UnserializeFromString<T>(string raw) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            if (raw != null)
            {
                var bytes = Encoding.ASCII.GetBytes(raw);
                using (var memoryStream = new MemoryStream(bytes))
                {
                    return (T)serializer.ReadObject(memoryStream);
                }
            }
            return null;
        }
    }
}
