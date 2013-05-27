using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;

using Dongle.Reflection;
using Dongle.Serialization.Attributes;
using Dongle.System.IO;

namespace Dongle.Serialization
{
    public class FixedWidthTextSerializer<TEntity> : IEnumerableSerializer<TEntity>
    {
        private const int DefaultWidth = 20;
        private readonly Dictionary<string, int> _fieldWidths = new Dictionary<string, int>();
        private readonly ResourceManager _resourceManager;

        public string[] ColumnHeaderTexts;

        public bool UseDynamicSizes;

        public FixedWidthTextSerializer(ResourceManager resourceManager = null)
        {
            _resourceManager = resourceManager;
        }

        public byte[] Serialize(IEnumerable<TEntity> items, CultureInfo cultureInfo = null, Encoding encoding = null)
        {
            encoding = encoding ?? DongleEncoding.Default;
            cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            var properties = typeof(TEntity).GetProperties();
            var builder = new StringBuilder();

            var totalWidth = 0;
            var colBuilder = new StringBuilder();
            var i = 0;
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.HasAttribute<IgnoreAttribute>())
                {
                    continue;
                }
                int width;
                if (UseDynamicSizes)
                {
                    width = items.Max(it => ObjectFormatter.Format(propertyInfo.GetValue(it, null), cultureInfo, false).Length) + 2;
                }
                else
                {
                    var attr = Attribute.GetCustomAttribute(propertyInfo, typeof(FixedWidthAttribute)) as FixedWidthAttribute;
                    width = (attr == null ? DefaultWidth : attr.Width) + 1;                       
                }                

                var name = propertyInfo.Name;
                if (ColumnHeaderTexts != null && ColumnHeaderTexts.Length > i)
                {
                    name = ColumnHeaderTexts[i];
                }
                else if (_resourceManager != null)
                {
                    var tempName = _resourceManager.GetString(propertyInfo.Name);
                    if (!string.IsNullOrEmpty(tempName))
                    {
                        name = tempName;
                    }
                }
                if (width < name.Length + 1)
                {
                    width = name.Length + 1;
                }
                _fieldWidths[propertyInfo.Name] = width;
                colBuilder.Append(Pad(name, width));
                totalWidth += width;
                i++;
            }
            for (i = 0; i < totalWidth; i++)
            {
                builder.Append("-");
            }
            builder.AppendLine();
            builder.AppendLine(colBuilder.ToString());
            for (i = 0; i < totalWidth; i++)
            {
                builder.Append("-");
            }

            foreach (var entity in items)
            {
                builder.AppendLine();
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.HasAttribute<IgnoreAttribute>())
                    {
                        continue;
                    }
                    var width = this._fieldWidths[propertyInfo.Name];
                    var value = ObjectFormatter.Format(propertyInfo.GetValue(entity, null), cultureInfo, false);
                    var txt = Pad(value, width);
                    builder.Append(txt);
                }
            }
            return encoding.GetBytes(builder.ToString());
        }

        private static string Pad(string value, int width)
        {
            width--;
            var txt = value.Length < width ? value.PadRight(width, ' ') : value.Substring(0, width);
            return txt + " ";
        }
    }
}