using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web.Mvc;

namespace Dongle.Web.Extensions
{
    public static class EnumExtensions
    {
        private static ResourceManager _resourceManager;

        public static IEnumerable<SelectListItem> ToSelectList(this Type enumType, Func<SelectListItem, bool> filter = null)
        {
            var values = Enum.GetValues(enumType);
            var elementsValue = values.Cast<object>().Select((t, i) => Convert.ToInt32(values.GetValue(i))).ToList();

            var listItems = elementsValue.Select(element => new SelectListItem
            {
                Selected = false,
                Value = Convert.ToString(element, CultureInfo.InvariantCulture),
                Text = GetIdNameResource(element, enumType)
            });
            return listItems;
        }

        public static string GetIdNameResource(long id, Type enumType)
        {
            var name = enumType.GetEnumName(id);
            if (name == null)
            {
                return id.ToString(CultureInfo.InvariantCulture);
            }
            if (_resourceManager != null)
            {
                return _resourceManager.GetString(name) ?? name;
            }
            return name;
        }

        public static void SetResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
    }
}
