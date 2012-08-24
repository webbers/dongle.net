using System;
using System.Collections.Generic;

namespace Dongle.Algorithms
{
    public static class LevensteinDistance
    {
        public static double Similarity(string str1, string str2, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            var str1Len = str1.Length;
            var str2Len = str2.Length;

            double dist = Calculate(str1, str2, stringComparison);
            double maxLength = Math.Max(str1Len, str2Len);

            if (maxLength > 0)
            {
                return 1.0D - (dist / maxLength);
            }
            return 0;
        }

        public static int Calculate(string str1, string str2, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            var str1Len = str1.Length;
            var str2Len = str2.Length;
            var d = new int[str1Len + 1, str2Len + 1];

            // Step 1
            if (str1Len == 0) return str2Len;
            if (str2Len == 0) return str1Len;

            // Step 2
            for (var i = 0; i <= str1Len; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= str2Len; d[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= str1Len; i++)
            {
                //Step 4
                for (var j = 1; j <= str2Len; j++)
                {
                    // Step 5
                    var cost = (str2.Substring(j - 1, 1).Equals(str1.Substring(i - 1, 1), stringComparison) ? 0 : 1); // cost

                    // Step 6
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                              d[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return d[str1Len, str2Len];

        }

        public static SimilarityResult<TSource> GetMostSimilarTo<TSource>(this IEnumerable<TSource> source, Func<TSource, string> predicate, string str, StringComparison stringComparison = StringComparison.CurrentCulture) where TSource : class
        {
            TSource greaterObject = null;
            double greaterValue = 0;

            foreach (var tempObject in source)
            {
                var tempValue = Similarity(str, predicate.Invoke(tempObject), stringComparison);
                if (tempValue <= greaterValue)
                {
                    continue;
                }
                if (Math.Abs(tempValue - 1) < Double.Epsilon)
                {
                    return new SimilarityResult<TSource>(tempValue, tempObject);
                }
                greaterValue = tempValue;
                greaterObject = tempObject;
            }
            return greaterObject != null ? new SimilarityResult<TSource>(greaterValue, greaterObject) : null;
        }

        public static SimilarityResult<string> GetMostSimilarTo(this IEnumerable<string> source, string str, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            return source.GetMostSimilarTo(s => s, str, stringComparison);
        }
    }

    public class SimilarityResult<T>
    {
        public SimilarityResult(double rate, T obj)
        {
            Rate = rate;
            Item = obj;
        }
        public  double Rate { get; set; }
        public T Item { get; set; }
    }
}
