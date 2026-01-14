using Microsoft.AspNetCore.Authorization;

namespace Arihant.Models.JWT
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            var hasPermission = context.User.Claims.Any(c =>
        c.Type == "permission" &&
        c.Value.Trim().Equals(requirement.Permission.Trim(), StringComparison.OrdinalIgnoreCase));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
