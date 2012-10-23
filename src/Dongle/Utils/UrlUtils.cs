using System;

namespace Dongle.Utils
{
    public static class UrlUtils
    {
        /// <summary>
        /// Obtém uma URL sem querystring
        /// </summary>
        public static string GetUrlWithoutQueryString(string url)
        {
            try
            {
                if (url == null)
                    return null;
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
            try
            {
                if (url == null)
                    return null;
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
    }
}
