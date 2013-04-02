using System;
using System.Reflection;

namespace Dongle.Serialization
{
    public static class SerializationHelper
    {
        public static TEntity Deserialize<TEntity>(this string command) where TEntity : class
        {
            if (String.IsNullOrEmpty(command))
            {
                return null;
            }
            var communicationT = default(TEntity);
            var assembly = typeof(TEntity).Assembly;
            var type = GetTypeFromJson(command, "CurrentTypeFullName", assembly);
            if (type != null)
            {
                communicationT = JsonSimpleSerializer.UnserializeObject(command, type) as TEntity;
            }
            return communicationT;
        }

        public static TEntity DeserializeConcrete<TEntity>(this string command) where TEntity : class
        {
            if (String.IsNullOrEmpty(command))
            {
                return null;
            }
            var type = typeof(TEntity);
            return JsonSimpleSerializer.UnserializeObject(command, type) as TEntity;
        }

        public static Type GetTypeFromJson(string json, string typePropertyName)
        {
            var typeFullName = JsonSimpleSerializer.GetNodeValueFromJson(json, typePropertyName);
            return Type.GetType(typeFullName);
        }

        public static Type GetTypeFromJson(string json, string typePropertyName, Assembly assembly)
        {
            var typeFullName = JsonSimpleSerializer.GetNodeValueFromJson(json, typePropertyName);
            return assembly.GetType(typeFullName);
        }

        public static Type GetTypeFromJson(string json, string typePropertyName, string assemblyPathPropertyName)
        {
            var typeFullName = JsonSimpleSerializer.GetNodeValueFromJson(json, typePropertyName);
            var assemblyPath = JsonSimpleSerializer.GetNodeValueFromJson(json, assemblyPathPropertyName);
            return Assembly.LoadFrom(assemblyPath).GetType(typeFullName);
        }
    }
}