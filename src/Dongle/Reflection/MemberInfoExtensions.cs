using System;
using System.Linq;
using System.Reflection;

namespace Dongle.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool HasAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute
        {
            return member.GetCustomAttributes(true).Any(ca => ca.GetType() == typeof(TAttribute));
        }
    }
}
