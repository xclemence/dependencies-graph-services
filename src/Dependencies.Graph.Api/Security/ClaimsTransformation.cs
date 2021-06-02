using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Dependencies.Graph.Api.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Dependencies.Graph.Api.Security
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly IOptions<SecurityOption> securityOption;
        private readonly IDictionary<string, RoleMappingItem> roleMappings;

        public ClaimsTransformation(IOptions<SecurityOption> security)
        {
            securityOption = security;

            if (!string.IsNullOrWhiteSpace(security.Value.RoleMappings))
            {
                roleMappings = JsonSerializer.Deserialize<List<RoleMappingItem>>(security.Value.RoleMappings ?? string.Empty)
                                             .ToDictionary(x => x.Server);
            }
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;

            if (!claimsIdentity.IsAuthenticated)
                return Task.FromResult(principal);

            if (!claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
                return Task.FromResult(principal);

            var realmAccessClaim = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");

            var document = JsonDocument.Parse(realmAccessClaim.Value);

            if (!document.RootElement.TryGetProperty(securityOption.Value.Oidc.ClientId, out var applicationElement))
                return Task.FromResult(principal);

            if (!applicationElement.TryGetProperty("roles", out var rolesElement))
                return Task.FromResult(principal);

            var roles = rolesElement.EnumerateArray().Select(x => GetApplicationRoleName(x.ToString())).ToList();

            foreach (var role in roles)
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

            return Task.FromResult(principal);
        }

        private string GetApplicationRoleName(string name)
        {
            if (roleMappings?.ContainsKey(name) ?? false)
                return roleMappings[name].App;

            return name;
        }
    }
}