using System;
using System.Collections.Generic;
using System.Linq;

namespace Dongle.System
{
    public static class EnumerableExtensions
    {
        public static string ToCsv<TEntity>(this IEnumerable<TEntity> values)
        {
            return ToCsv(values, entity => entity.ToString());
        }

        public static string ToCsv<TEntity>(this IEnumerable<TEntity> values, Func<TEntity, string> word)
        {
            var items = values.Select(word).ToList();
            if (!items.Any())
            {
                return "";
            }

            var value = items.Aggregate("", (c, v) => c + (v + ", "));
            return value.Substring(0, value.Length - 2);
        }

        public static string ToSmartCsv<TEntity>(this IEnumerable<TEntity> values, string itemsWord, int maxToDisplay = 3)
        {
            return ToSmartCsv(values, entity => entity.ToString(), itemsWord, maxToDisplay);
        }

        public static string ToSmartCsv<TEntity>(this IEnumerable<TEntity> values, Func<TEntity, string> word, string itemsWord, int maxToDisplay = 3)
        {
            var count = values.Count();
            if (count > 0 && count <= maxToDisplay)
            {
                return ToCsv(values, word);
            }
            return count + " " + itemsWord;
        }
    }
}