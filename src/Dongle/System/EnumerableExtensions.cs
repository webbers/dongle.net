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

        public static IEnumerable<int> Distribute(this IEnumerable<double> weights, int amount)
        {
            var weightsList = weights.ToList();
            var totalWeight = weightsList.Sum();
            var length = weightsList.Count();

            var actual = new double[length];
            var error = new double[length];
            var rounded = new int[length];

            var added = 0;

            var i = 0;
            foreach (var w in weightsList)
            {
                actual[i] = amount * (w / totalWeight);
                rounded[i] = (int)Math.Floor(actual[i]);
                error[i] = actual[i] - rounded[i];
                added += rounded[i];
                i += 1;
            }

            while (added < amount)
            {
                var maxError = 0.0;
                var maxErrorIndex = -1;
                for (var e = 0; e < length; ++e)
                {
                    if (error[e] > maxError)
                    {
                        maxError = error[e];
                        maxErrorIndex = e;
                    }
                }

                rounded[maxErrorIndex] += 1;
                error[maxErrorIndex] -= 1;

                added += 1;
            }

            return rounded;
        }
    }
}