using Abp.Authorization;
using VQM.Authorization.Roles;
using VQM.Authorization.Users;

namespace VQM.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
