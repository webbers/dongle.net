using System.Collections.Generic;

namespace Dongle.Authentication
{
    public interface IAuthenticator
    {
        AuthenticatedUser Authenticate(string username, string password);
        ICollection<string> GetUserGroups(string username, bool throwException = false);
    }
}