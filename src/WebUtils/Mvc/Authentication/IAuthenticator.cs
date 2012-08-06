namespace WebUtils.Mvc.Authentication
{
    public interface IAuthenticator
    {
        AuthenticatedUser Authenticate(string username, string password);
    }
}