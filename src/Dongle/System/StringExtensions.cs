using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Dongle.Algorithms;

namespace Dongle.System
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

        /// <summary>
        /// Calcula o MD5 de uma string, não é thread-safe.
        /// </summary>
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

        /// <summary>
        /// Calcula o MD5 de uma string, thread-safe.
        /// </summary>
        public static string ToMd5Safe(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            var md5 = MD5.Create();
            var inputBytes = encoding.GetBytes(value);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Remove caracteres de acentuação e espaços de uma string, usando sucessivos replaces
        /// </summary>
        public static string RemoveSpecialChars(this string text)
        {
            const string past = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç ";
            const string future = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc_";
            const string not = "?#\"'\\/:<>|*-+";
            for (var i = 0; i < past.Length; i++)
            {
                text = text.Replace(past[i].ToString(), future[i].ToString());
            }
            for (var i = 0; i < not.Length; i++)
            {
                text = text.Replace(not[i].ToString(), "");
            }
            return text;
        }

        /// <summary>
        /// Remove caracteres de acentuação de uma string
        /// </summary>
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

        /// <summary>
        /// Remove caracteres de pontuação de uma string
        /// </summary>
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

        /// <summary>
        /// Obtem o valor em Base64 de uma string
        /// </summary>
        public static string ToBase64(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return Convert.ToBase64String(encoding.GetBytes(value));
        }

        /// <summary>
        /// Obtem a string com base em um texto em Base64
        /// </summary>
        public static string FromBase64ToString(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(Convert.FromBase64String(value.Replace("+", " ").Replace(";", "/")));
        }

        /// <summary>
        /// Retorna o CRC32 de uma string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToCrc32(this string value)
        {
            return Crc32.Compute(value);
        }

        /// <summary>
        /// Converte uma string de um hexa para um int
        /// </summary>
        public static int FromHexToInt32(this string hexValue)
        {
            int value;
            try
            {
                value = Convert.ToInt32(hexValue, 16);
            }
            catch (Exception)
            {
                value = 0;
            }
            return value;
        }

        /// <summary>
        /// Converte uma string de um hexa para um Int64
        /// </summary>
        public static long FromHexToInt64(this string hexValue)
        {
            Int64 value;
            try
            {
                value = Convert.ToInt64(hexValue, 16);
            }
            catch (Exception)
            {
                value = 0;
            }
            return value;
        }

        /// <summary>
        /// Inverte uma string.
        /// </summary>
        public static string Reverse(this string valueToReverse)
        {
            var array = valueToReverse.ToCharArray();
            Array.Reverse(array);
            return (new string(array));
        }

        /// <summary>
        /// Remove os ultimos caracteres do texto que corresponderem ao parametro. Em geral é mais rápida que TrimRight.
        /// </summary>
        public static string RemoveRightChar(this string value, string rightText)
        {
            if (value.EndsWith(rightText))
                value = value.Substring(0, value.Length - rightText.Length);
            return value;
        }

        /// <summary>
        /// Converte uma string para um texto compatível HTML (ex.: Ç para Ccedil;)
        /// </summary>
        public static string HtmlEncode(this string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Converte uma string em HTML para um texto normal (ex.: Ccedil; para Ç)
        /// </summary>
        public static string HtmlDecode(this string value)
        {
            return WebUtility.HtmlDecode(value);
        }

        /// <summary>
        /// Conta as ocorrências um caracter em uma string
        /// </summary>
        public static int CountOccurrences(this string text, string textToFind)
        {
            return text.Length - text.Replace(textToFind, "").Length;
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
            if (count == 0)
            {
                return str;
            }
            if (count > 0 && str.Length > count)
            {
                return str.Substring(0, count);
            }
            return str;
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
