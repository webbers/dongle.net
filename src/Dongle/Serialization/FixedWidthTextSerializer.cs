using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

using Dongle.Reflection;
using Dongle.Serialization.Attributes;

namespace Dongle.Serialization
{
    public class FixedWidthTextSerializer<TEntity> : IEnumerableSerializer<TEntity>
    {
        private const int DefaultWidth = 20;
        private readonly Dictionary<string, int> _fieldWidths = new Dictionary<string, int>();

        private readonly ResourceManager _resourceManager;

        public FixedWidthTextSerializer(ResourceManager resourceManager = null)
        {
            _resourceManager = resourceManager;
        }

        public byte[] Serialize(IEnumerable<TEntity> items, CultureInfo cultureInfo = null, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.GetEncoding(1252);
            cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            var properties = typeof(TEntity).GetProperties();
            var builder = new StringBuilder();

            var totalWidth = 0;
            var colBuilder = new StringBuilder();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.HasAttribute<IgnoreAttribute>())
                {
                    continue;
                }
                var attr = Attribute.GetCustomAttribute(propertyInfo, typeof(FixedWidthAttribute)) as FixedWidthAttribute;
                
                var width = (attr == null ? DefaultWidth : attr.Width) + 1;
                _fieldWidths[propertyInfo.Name] = width;

                var name = propertyInfo.Name;
                if (_resourceManager != null)
                {
                    var tempName = this._resourceManager.GetString(propertyInfo.Name);
                    if (!string.IsNullOrEmpty(tempName))
                    {
                        name = tempName;
                    }
                }
                colBuilder.Append(Pad(name, width));
                totalWidth += width;
            }
            for (var i = 0; i < totalWidth; i++)
            {
                builder.Append("-");
            }
            builder.AppendLine();
            builder.AppendLine(colBuilder.ToString());
            for (var i = 0; i < totalWidth; i++)
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