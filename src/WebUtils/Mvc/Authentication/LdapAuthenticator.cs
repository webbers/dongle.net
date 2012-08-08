using System.DirectoryServices.AccountManagement;
using WebUtils.Resources;

namespace WebUtils.Mvc.Authentication
{
    public class LdapAuthenticator : IAuthenticator
    {
        private readonly string _domain;
        private readonly string _container;

        public LdapAuthenticator(string domain, string container)
        {
            _domain = domain;
            _container = container;
        }

        public virtual AuthenticatedUser Authenticate(string username, string password)
        {
            if (!ValidateCredentials(username, password))
            {
                throw new BadUsernameOrPasswordException(WebUtilsResource.BadUsernameOrPassword);
            }

            var userPrincipal = GetUser(username);

            return new AuthenticatedUser
            {
                Email = userPrincipal.EmailAddress,
                Name = userPrincipal.Name,
                UserName = username
            };
        }

        public UserPrincipal GetUser(string username)
        {
            var principalContext = GetPrincipalContext();
            var userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, username);
            return userPrincipal;
        }
        public bool ValidateCredentials(string username, string password)
        {
            using (var principalContext = GetPrincipalContext())
            {
                return principalContext.ValidateCredentials(username, password);
            }
        }

        private PrincipalContext GetPrincipalContext()
        {
            var principalContext = new PrincipalContext(ContextType.Domain, _domain, _container);
            return principalContext;
        }
    }
}