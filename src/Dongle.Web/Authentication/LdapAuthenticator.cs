using System.DirectoryServices;

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

        public virtual AuthenticatedUser Authenticate(string username, string password)
        {
            try
            {
                var domainAndUsername = _domain + @"\" + username;
                var entry = new DirectoryEntry(_path, domainAndUsername, password);
                var search = new DirectorySearcher(entry) { Filter = "(SAMAccountName=" + username + ")" };

                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("displayName");
                search.PropertiesToLoad.Add("email");
                var result = search.FindOne();

                if (result != null)
                {
                    var email = result.Properties["email"].Count > 0 ? result.Properties["email"][0] : "";
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

    }
}