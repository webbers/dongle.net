using System;
using System.Collections;
using System.Collections.Generic;

namespace Dongle.System
{
    public static class TypeExtensions
    {
        public static bool IsNumeric(this Type type)
        {
            if (
                Type.GetTypeCode(type) == TypeCode.Byte ||
                Type.GetTypeCode(type) == TypeCode.Decimal ||
                Type.GetTypeCode(type) == TypeCode.Double ||
                Type.GetTypeCode(type) == TypeCode.Int16 ||
                Type.GetTypeCode(type) == TypeCode.Int32 ||
                Type.GetTypeCode(type) == TypeCode.Int64 ||
                Type.GetTypeCode(type) == TypeCode.SByte ||
                Type.GetTypeCode(type) == TypeCode.Single ||
                Type.GetTypeCode(type) == TypeCode.UInt16 ||
                Type.GetTypeCode(type) == TypeCode.UInt32 ||
                Type.GetTypeCode(type) == TypeCode.UInt64)
            {
                return true;
            }
            return false;
        }
        public static bool IsDate(this Type type)
        {
            if (Type.GetTypeCode(type) == TypeCode.DateTime)
            {
                return true;
            }
            return false;
        }

        public static bool IsDictionaryType(Type type)
        {
            if (typeof(IDictionary).IsAssignableFrom(type) || ImplementsGenericDefinition(type, typeof(IDictionary<,>)))
            {
                return true;
            }
            return false;
        }

        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
        {
            if (type.IsInterface && type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericInterfaceDefinition == genericTypeDefinition)
                {
                    return true;
                }
            }
            foreach (var type1 in type.GetInterfaces())
            {
                if (type1.IsGenericType)
                {
                    var genericTypeDefinition = type1.GetGenericTypeDefinition();
                    if (genericInterfaceDefinition == genericTypeDefinition)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
