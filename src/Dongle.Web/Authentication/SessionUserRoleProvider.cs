using System.Linq;
using System.Web.Security;

namespace WebUtils.Mvc.SessionAuthorize
{
    public class SessionUserRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string email, string actionPermissionName)
        {
            /*using (var db = new OctopusDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);

                return user != null && user.UserProfile.ActionPermissions.Any(ap => ap.Name == actionPermissionName);
            } */
            return false;
        }

        public override string[] GetRolesForUser(string email)
        {
            /*using (var db = new OctopusDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                return user == null ? new string[0] : user.UserProfile.ActionPermissions.Select(up => up.Name).ToArray();
            }  */
            return null;
        }

        public override void CreateRole(string actionPermissionName)
        {
            /*using (var db = new OctopusDbContext())
            {
                db.ActionPermissions.Add(new ActionPermission { Name = actionPermissionName });
                db.SaveChanges();
            } */
        }

        public override bool DeleteRole(string actionPermissionName, bool throwOnPopulatedRole)
        {
            /*using (var db = new OctopusDbContext())
            {
                var actionPermission = db.ActionPermissions.FirstOrDefault(ap => ap.Name == actionPermissionName);

                if (actionPermission != null)
                {
                    db.ActionPermissions.Remove(actionPermission);
                    db.SaveChanges();
                }
                return true;
            }   */
            return false;
        }

        public override bool RoleExists(string actionPermissionName)
        {
            /*using (var db = new OctopusDbContext())
            {
                return db.ActionPermissions.Any(ap => ap.Name == actionPermissionName);
            } */
            return false;
        }

        public override void AddUsersToRoles(string[] usernames, string[] actionPermissionNames)
        {
            return;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] actionPermissionNames)
        {
            return;
        }

        public override string[] GetUsersInRole(string actionPermissionName)
        {
            /*using (var db = new OctopusDbContext())
            {
                return
                    db.UserProfiles
                        .Where(up => up.ActionPermissions.Any(ap => ap.Name == actionPermissionName))
                        .SelectMany(up => up.Users)
                        .Distinct()
                        .Select(u => u.Email).ToArray();
            }  */
            return null;
        }

        public override string[] GetAllRoles()
        {
            /*using (var db = new OctopusDbContext())
            {
                return db.ActionPermissions.Select(ap => ap.Name).ToArray();
            } */
            return null;
        }

        public override string[] FindUsersInRole(string actionPermissionName, string usernameToMatch)
        {
            return GetUsersInRole(actionPermissionName)
                .Where(u => u == usernameToMatch)
                .ToArray();
        }

        public override string ApplicationName { get; set; }
    }
}
