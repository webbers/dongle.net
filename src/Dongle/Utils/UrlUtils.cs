using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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
        /// Valida se o item é IPV4 ou IPV6
        /// </summary>
        public static bool IsIp(string text)
        {
            return IsIpV4Like(text) || IsIpV6Like(text);
        }

        /// <summary>
        /// Valida se o item é IPV4
        /// </summary>
        private static bool IsIpV4Like(string text)
        {
            const string ipv4Pattern = @"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.){3,3}(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            return Regex.IsMatch(text, ipv4Pattern);
        }

        /// <summary>
        /// Valida se o item é IPV6
        /// </summary>
        private static bool IsIpV6Like(string text)
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
