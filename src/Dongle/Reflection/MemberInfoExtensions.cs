using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Dongle.Reflection
{
    public static class MemberInfoExtensions
    {
        public static int GetMaxLength(this MemberInfo member)
        {
            if (member == null)
            {
                return 0;
            }
            var stringLengthAttribute = member.GetCustomAttribute<StringLengthAttribute>();
            if (stringLengthAttribute != null)
            {
                return stringLengthAttribute.MaximumLength;
            }
            var maxLengthAttribute = member.GetCustomAttribute("System.ComponentModel.DataAnnotations.MaxLengthAttribute");
            if (maxLengthAttribute != null)
            {
                try
                {
                    return (int)maxLengthAttribute.GetType().GetProperty("Length").GetValue(maxLengthAttribute, null);                    
                }
                catch
                {
                    return 0;
                }                
            }
            return 0;
        }

        public static T GetCustomAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return (T)member.GetCustomAttributes(typeof (T), true).FirstOrDefault();
        }

        public static object GetCustomAttribute(this MemberInfo member, string name)
        {
            foreach (object attribute in member.GetCustomAttributes(true))
            {
                if (attribute.GetType().FullName == name)
                {
                    return attribute;
                }
            }
            return null;
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute
        {
            return member.GetCustomAttributes(true).Any(ca => ca.GetType() == typeof(TAttribute));
        }
    }
}
