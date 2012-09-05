using System.DirectoryServices.AccountManagement;

namespace Dongle.Web.Authentication
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

        private UserPrincipal GetUser(string username)
        {
            var principalContext = GetPrincipalContext();
            var userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, username);
            return userPrincipal;
        }

        private PrincipalContext GetPrincipalContext()
        {
            var principalContext = new PrincipalContext(ContextType.Domain, _domain, _container);
            return principalContext;
        }

        public bool Authenticate(string username, string password)
        {
            using (var principalContext = GetPrincipalContext())
            {
                return principalContext.ValidateCredentials(username, password);
            }
        }
    }
}