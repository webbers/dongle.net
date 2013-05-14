using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;

namespace Dongle.Web.Authentication
{
    public class LdapAuthenticator : IAuthenticator
    {
        private readonly string _domain;
        private readonly string _path;

        public LdapAuthenticator(string domain, string path)
        {
            _domain = domain;
            _path = path;
        }

        public AuthenticatedUser Authenticate(string username, string password)
        {
            try
            {
                var domainAndUsername = _domain + @"\" + username;
                var entry = new DirectoryEntry(_path, domainAndUsername, password);
                var search = new DirectorySearcher(entry) { Filter = "(SAMAccountName=" + username + ")" };

                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("displayName");
                search.PropertiesToLoad.Add("mail");
                var result = search.FindOne();

                if (result != null)
                {
                    var email = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0] : "";
                    var name = result.Properties["displayName"].Count > 0 ? result.Properties["displayName"][0] : "";

                    return new AuthenticatedUser
                    {
                        Email = email.ToString(),
                        Name = name.ToString(),
                        UserName = username
                    };
                }
            }
            catch (DirectoryServicesCOMException)
            {
            }
            return null;
        }
        public ICollection<string> GetUserGroups(string accountName, bool throwException = false)
        {
            var groups = new List<string>();
            try
            {
                using (var principalContext = new PrincipalContext(ContextType.Domain, _domain))
                {
                    using (var userPrincipal = UserPrincipal.FindByIdentity(principalContext, accountName))
                    {
                        if (userPrincipal == null)
                        {
                            if (throwException) throw new ActiveDirectoryObjectNotFoundException(accountName);
                            return groups;
                        }
                        groups.AddRange(userPrincipal.GetGroups().ToList().Select(principal => principal.Name));
                    }
                }
            }
            catch (PrincipalServerDownException)
            {
                if (throwException) throw;
            }
            return groups;
        }
    }
}