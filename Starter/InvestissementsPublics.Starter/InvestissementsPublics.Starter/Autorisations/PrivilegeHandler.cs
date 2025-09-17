using InvestissementsPublics.Starter.ApplicationUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InvestissementsPublics.Starter.Autorisations
{
    public class PrivilegeHandler : AuthorizationHandler<PrivilegeRequirement>
    {
        private readonly IPrivilegeService _privService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PrivilegeHandler(IPrivilegeService privService, UserManager<ApplicationUser> userManager)
        {
            _privService = privService;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PrivilegeRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true) return;

            // 1) optimisation : si le claim "privilege" est présent (ex: claims-on-login), on vérifie d'abord.
            if (context.User.Claims.Any(c => c.Type == "privilege" && c.Value == requirement.PrivilegeName))
            {
                context.Succeed(requirement);
                return;
            }

            // 2) sinon on récupère depuis la base via le service (cache inside service)
            var userId = _userManager.GetUserId(context.User);
            if (string.IsNullOrEmpty(userId)) return;

            var privileges = await _privService.GetPrivilegesForUserAsync(userId);
            if (privileges.Contains(requirement.PrivilegeName))
            {
                context.Succeed(requirement);
            }
        }
    }
}
