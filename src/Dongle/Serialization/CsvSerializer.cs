using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

using Dongle.Reflection;
using Dongle.Serialization.Attributes;
using Dongle.System;
using Dongle.System.IO;

namespace Dongle.Serialization
{
    public class CsvSerializer<TEntity> : IEnumerableSerializer<TEntity>
    {
        private readonly ResourceManager _resourceManager;

        public CsvSerializer(ResourceManager resourceManager = null)
        {
            _resourceManager = resourceManager;
        }

        public byte[] Serialize(IEnumerable<TEntity> items, CultureInfo cultureInfo = null, Encoding encoding = null)
        {
            encoding = encoding ?? DongleEncoding.Default;
            cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;

            var properties = typeof(TEntity).GetProperties();
            var rows = new List<Dictionary<string, object>>();
            var fields = new List<string>();

            if (TypeExtensions.IsDictionaryType(typeof(TEntity)))
            {
                foreach (var entity in items)
                {
                    var dicItems = entity as IDictionary;
                    if (dicItems == null)
                    {
                        continue;
                    }

                    var row = new Dictionary<string, object>();
                    rows.Add(row);
                    foreach (DictionaryEntry d in dicItems)
                    {
                        var name = d.Key.ToString();
                        if (!fields.Contains(name))
                        {
                            fields.Add(name);
                        }
                        row.Add(name, d.Value);
                    }
                }
            }
            else
            {
                foreach (var entity in items)
                {
                    var row = new Dictionary<string, object>();
                    rows.Add(row);

                    foreach (var propertyInfo in properties)
                    {
                        var name = propertyInfo.Name;
                        if (_resourceManager != null)
                        {
                            var tempName = this._resourceManager.GetString(propertyInfo.Name);
                            if (!string.IsNullOrEmpty(tempName))
                            {
                                name = tempName;
                            }
                        }
                        if (propertyInfo.HasAttribute<IgnoreAttribute>())
                        {
                            continue;
                        }
                        if (!fields.Contains(name))
                        {
                            fields.Add(name);
                        }
                        row[name] = propertyInfo.GetValue(entity, null);
                    }
                }
            }
            return Serialize(fields, rows, cultureInfo, encoding);
        }

        #region PrivateMethods

        private static byte[] Serialize(IList<string> fields, IEnumerable<Dictionary<string, object>> rows, CultureInfo cultureInfo, Encoding encoding)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                builder.Append(field);
                if (i < fields.Count - 1)
                {
                    builder.Append(",");
                }
            }
            builder.AppendLine();

            foreach (var row in rows)
            {
                for (var i = 0; i < fields.Count; i++)
                {
                    var field = fields[i];
                    builder.Append(ObjectFormatter.Format(row[field], cultureInfo));
                    if (i < fields.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
                builder.AppendLine();
            }
            return encoding.GetBytes(builder.ToString());
        }
        #endregion
    }
}