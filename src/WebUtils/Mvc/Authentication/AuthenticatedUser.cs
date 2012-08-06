using System.Collections.Generic;

namespace WebUtils.Mvc.Authentication
{
    public class AuthenticatedUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Permissions = new List<string>();
    }
}