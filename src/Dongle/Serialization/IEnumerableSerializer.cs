using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Dongle.Serialization
{
    public interface IEnumerableSerializer<in TEntity>
    {
        byte[] Serialize(IEnumerable<TEntity> items, CultureInfo cultureInfo, Encoding encoding);
    }
}