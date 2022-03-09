
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace KioskStream.Core.Security.Authorization
{
    public class PolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public PolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Check static policies first
            AuthorizationPolicy policy = await base.GetPolicyAsync(policyName);
            if (policy != null) return policy;

            policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.AddRequirements(new PermissionRequirement(policyName))
                .Build();

            // Add policy to the AuthorizationOptions, so we don't have to re-create it each time
            _options.AddPolicy(policyName, policy);

            return policy;
        }
    }
}
