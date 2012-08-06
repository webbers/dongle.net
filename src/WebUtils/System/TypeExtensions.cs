using System;

namespace WebUtils.System
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
    }
}
