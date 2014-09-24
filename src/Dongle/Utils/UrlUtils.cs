using System;
using System.Net;
using System.Net.Sockets;

namespace Dongle.Utils
{
    public static class UrlUtils
    {
        /// <summary>
        /// Obtém uma URL sem querystring
        /// </summary>
        public static string GetUrlWithoutQueryString(string url)
        {
            if (url == null)
            {
                return "";
            }
            try
            {                
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    var ret = "";
                    ret += uri.Scheme + "://";
                    ret += uri.Authority + uri.PathAndQuery.Split('?')[0];
                    return ret;
                }
                return url;
            }
            catch (Exception)
            {
                return url;
            }
        }

        /// <summary>
        /// Obtém uma URL sem querystring e sem porta
        /// </summary>
        public static string GetUrlWithoutPortAndQueryString(string url)
        {
            if (url == null)
            {
                return "";
            }
            try
            {                
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    var ret = "";
                    ret += uri.Scheme + "://";
                    ret += uri.Host + uri.LocalPath;
                    return ret;
                }
                return url;
            }
            catch (Exception)
            {
                return url;
            }
        }

        /// <summary>
        /// Obtém o dominio de uma URL
        /// </summary>
        public static string GetDomain(string url, bool includeSchema)
        {
            if (url == null)
            {
                return null;
            }
            try
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    var ret = "";
                    if (includeSchema)
                    {
                        ret += uri.Scheme + "://";
                    }
                    ret += uri.Host;
                    return ret;
                }
                return url;
            }
            catch (Exception)
            {
                if (!includeSchema)
                {
                    var parts = url.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        return parts[1];
                    }
                }
                return url;
            }
        }

        /// <summary>
        /// Verifica se é IPV4 ou IPV6
        /// </summary>
        public static bool IsIp(string text)
        {
            return IsIpV4Like(text) || IsIpV6Like(text);
        }

        /// <summary>
        /// Valida se está em um formato parecido com IP v4. Não faz uma checagem completa pois seria mais lento, então apenas verifica se está no formato 999.999.999.999.
        /// </summary>
        public static bool IsIpV4Like(string text)
        {
            var chars = text.ToCharArray();
            if (chars.Length > 15)
            {
                return false;
            }
            for (var i = 0; i < chars.Length; i++)
            {
                var chr = chars[i];
                if ((chr < 48 || chr > 57) && chr != 46)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Valida se o item é IPV6
        /// </summary>
        public static bool IsIpV6Like(string text)
        {
            IPAddress address;
            if (IPAddress.TryParse(text, out address))
            {
                if (AddressFamily.InterNetworkV6.Equals(address.AddressFamily))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
