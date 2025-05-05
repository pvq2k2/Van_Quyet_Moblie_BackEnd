using Microsoft.AspNetCore.Authorization;

namespace API.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            // Sử dụng một policy tổng quát
            Policy = "PermissionPolicy";
            Permission = permission;
        }

        public string Permission { get; }
    }
}
