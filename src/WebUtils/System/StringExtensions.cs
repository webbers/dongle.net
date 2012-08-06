using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebUtils.System
{
    public static class StringExtensions
    {
        private static readonly MD5CryptoServiceProvider Md5Provider = new MD5CryptoServiceProvider();

        public static byte[] ToBytes(this String str)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string FromBytesToString(this byte[] bytes)
        {
            var encoding = new UTF8Encoding();
            var value = encoding.GetString(bytes);
            return value.IndexOf('\0') > -1 ? value.Substring(0, value.IndexOf('\0')) : value;
        }

        public static string ToMd5(this String str)
        {
            var encodedBytes = Md5Provider.ComputeHash(str.ToBytes());

            var sb = new StringBuilder();

            foreach (var encodedByte in encodedBytes)
            {
                sb.AppendFormat("{0:x2}", encodedByte);
            }
            return sb.ToString();
        }

        public static string RemoveAccents(this String str)
        {
            str = str.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in str.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                builder.Append(c);
            }

            return builder.ToString();
        }

        public static string RemovePunctuation(this String str)
        {
            var builder = new StringBuilder();
            foreach (var c in str)
            {
                if (!Char.IsPunctuation(c))
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        static public string ToBase64(this string str)
        {
            var encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64ToString(this String str)
        {
            var encoding = new UTF8Encoding();
            var encodedDataAsBytes = Convert.FromBase64String(str);
            return encoding.GetString(encodedDataAsBytes);
        }

        public static string Capitalize(this string value)
        {
            if (value.Length == 0)
            {
                return value;
            }

            var builder = new StringBuilder(value);
            builder[0] = Char.ToUpper(builder[0]);
            for (var i = 1; i < builder.Length; ++i)
            {
                var c = builder[i];
                var lastC = builder[i - 1];

                if (lastC == ' ')
                {
                    builder[i] = Char.ToUpper(c);
                }
                else
                {
                    builder[i] = Char.ToLower(c);
                }
            }
            return builder.ToString();
        }

        public static string OnlyNumbers(this string value)
        {
            var onlyNumbers = new Regex(@"[\d]+");
            var numbers = onlyNumbers.Matches(value);
            return numbers.Cast<object>().Aggregate("", (current, number) => current + number);
        }

        public static string ToSlug(this string phrase, int maxLength = 255)
        {
            var str = phrase.ToLower().Trim();

            //Substitui caracteres encodedurl por espaço
            str = Regex.Replace(str, @"\%[a-z0-9]{2}", " ");

            str = str.RemoveAccents();

            //Substitui caracteres que não são alfanumericos em espaço
            str = Regex.Replace(str, @"[^a-z0-9\s-]", " ");

            //Converte um ou mais espaços em hífen    
            str = Regex.Replace(str, @"[\s]+", "-");

            //Converte um ou mais hífens em um único       
            str = Regex.Replace(str, @"[-]+", "-");

            //Corta a string no máximo de caracteres
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength);

            return str;
        }

        public static string StripTags(this string str)
        {
            var sb = new StringBuilder();
            var inside = false;

            foreach (var c in str)
            {
                if (c == '<')
                {
                    inside = true;
                    continue;
                }
                if (c == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string Take(this string str, int count)
        {
            return str.Substring(0, str.Length > count ? count : str.Length);
        }

        public static List<int> IndexOfAll(this string str, string value)
        {
            int pos;
            var offset = 0;

            var indexes = new List<int>();

            while ((pos = str.IndexOf(value, StringComparison.Ordinal)) > 0)
            {
                str = str.Substring(pos+ value.Length);
                offset += pos;

                if(indexes.Count > 0)
                {
                    offset += value.Length;
                }
                indexes.Add(offset);
            }

            return indexes;
        }
    }
}
