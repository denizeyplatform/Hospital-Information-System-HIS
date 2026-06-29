using Microsoft.AspNetCore.Authorization;


namespace Identity.Infrastructure.Services.Identity
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
