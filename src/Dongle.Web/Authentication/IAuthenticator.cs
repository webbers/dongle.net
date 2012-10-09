namespace Dongle.Web.Authentication
{
    public interface IAuthenticator
    {
        AuthenticatedUser Authenticate(string username, string password);
    }
}