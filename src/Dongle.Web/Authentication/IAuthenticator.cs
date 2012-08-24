using System.Web.Security;

namespace Dongle.Web.Authentication
{
    public interface IAuthenticator
    {
        bool Authenticate(string username, string password);
    }
}