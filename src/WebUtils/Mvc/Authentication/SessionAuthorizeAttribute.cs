using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUtils.Mvc.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SessionAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string[] _rolesSplit = new string[0];
        private string _roles = string.Empty;

        public string Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }

        #region IAuthorizationFilter Members

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.HttpContext.Session == null || !IsUserAuthenticated(filterContext.HttpContext.Session))
            {
                HandleUnauthenticatedRequest(filterContext);
                return;
            }

            var user = filterContext.HttpContext.Session["User"] as AuthenticatedUser;
            if (user == null)
            {
                HandleUnauthenticatedRequest(filterContext);
                return;
            }
            if (_rolesSplit.Any() && !_rolesSplit.Intersect(user.Permissions).Any())
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }
            var cache = filterContext.HttpContext.Response.Cache;
            cache.SetProxyMaxAge(new TimeSpan(0L));
            cache.AddValidationCallback(CacheValidateHandler, null);
        }

        #endregion

        protected virtual bool IsUserAuthenticated(HttpSessionStateBase session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            return session["User"] != null;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual void HandleUnauthenticatedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            throw new HttpException(403,"Unauthorized Request");
        }

        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            return IsUserAuthenticated(httpContext.Session) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            return original
                .Split(new[] { ',' })
                .Select(piece => new { piece, trimmed = piece.Trim() })
                .Where(param0 => !string.IsNullOrEmpty(param0.trimmed))
                .Select(param0 => param0.trimmed)
                .ToArray();
        }
    }
}