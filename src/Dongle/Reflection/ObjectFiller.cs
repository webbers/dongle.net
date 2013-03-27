using System;

namespace Dongle.Reflection
{
    public static class ObjectFiller
    {
        public static void Fill(Type typeOfSource, object sourceObj, Type typeOfDestination, object destObj, bool overwriteWithNullValues = true)
        {
            var sourceProperties = typeOfSource.GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destProperty = typeOfDestination.GetProperty(sourceProperty.Name);

                if (destProperty == null || destProperty.CanWrite == false)
                {
                    continue;
                }

                var sourcePropertyValue = sourceProperty.GetValue(sourceObj, null);
                if (sourcePropertyValue == null && overwriteWithNullValues == false)
                {
                    continue;
                }

                if (sourceProperty.PropertyType == destProperty.PropertyType)
                {
                    destProperty.SetValue(destObj, sourcePropertyValue, null);
                }
                else if ((sourceProperty.PropertyType == typeof(byte) ||
                          sourceProperty.PropertyType == typeof(short) ||
                          sourceProperty.PropertyType == typeof(int) ||
                          sourceProperty.PropertyType == typeof(long)) &&
                          destProperty.PropertyType == typeof(bool))
                {
                    destProperty.SetValue(destObj, Convert.ToBoolean(sourcePropertyValue), null);
                }
                else if (sourceProperty.PropertyType == typeof(bool) &&
                        (destProperty.PropertyType == typeof(byte) ||
                         destProperty.PropertyType == typeof(short) ||
                         destProperty.PropertyType == typeof(int) ||
                         destProperty.PropertyType == typeof(long)))
                {
                    destProperty.SetValue(destObj, Convert.ToByte(sourcePropertyValue), null);
                }
                else if (IsNullable(sourceProperty.PropertyType)
                    && Nullable.GetUnderlyingType(sourceProperty.PropertyType) == destProperty.PropertyType)
                {
                    destProperty.SetValue(destObj, sourcePropertyValue ?? Activator.CreateInstance(destProperty.PropertyType), null);
                }
                else if (IsNullable(destProperty.PropertyType)
                    && Nullable.GetUnderlyingType(destProperty.PropertyType) == sourceProperty.PropertyType)
                {
                    destProperty.SetValue(destObj, sourcePropertyValue, null);
                }
            }
        }

        private static bool IsNullable(Type type)
        {
            if (!type.IsValueType)
            {
                return true;
            }
            return Nullable.GetUnderlyingType(type) != null;
        }
    }

    public class ObjectFiller<TSource, TDestination> where TSource : class where TDestination : class
    {
        public static void Fill(TSource sourceObj, TDestination destObj)
        {
            ObjectFiller.Fill(typeof(TSource), sourceObj, typeof(TDestination), destObj);
        }

        public static TDestination Fill(TSource sourceObj)
        {
            var destObj = Activator.CreateInstance<TDestination>();
            Fill(sourceObj, destObj);
            return destObj;
        }

        public static void Merge(TSource sourceObj, TDestination destObj)
        {
            ObjectFiller.Fill(typeof(TSource), sourceObj, typeof(TDestination), destObj, false);
        }
    }
}
