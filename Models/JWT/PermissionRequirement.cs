using Microsoft.AspNetCore.Authorization;

namespace Arihant.Models.JWT
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public PermissionRequirement(string permission) => Permission = permission;
    }
}

