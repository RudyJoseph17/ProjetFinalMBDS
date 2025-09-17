using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace InvestissementsPublics.Starter.Autorisations
{
    public class PrivilegePolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public PrivilegePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy?> GetDefaultPolicyAsync() =>
            _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Si policyName commence par "Privilege:" on en crée une ad-hoc
            const string prefix = "Privilege:";
            if (policyName != null && policyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var privilege = policyName.Substring(prefix.Length);
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PrivilegeRequirement(privilege))
                    .Build();
                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            // fallback default behaviour
            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
