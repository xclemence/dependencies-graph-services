using Dependencies.Graph.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dependencies.Graph.Api.Configuration
{
    public static class AuthorizationConfiguration
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            if (!configuration.GetSecurityEnabled())
            {
                services.AddAuthorization(x => x.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build());
            }
            else
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(o =>
                {
                    o.Authority = configuration.GetSecurityAuthority();
                    o.Audience = configuration.GetSecurityClientId();
                    o.RequireHttpsMetadata = false;
                    o.IncludeErrorDetails = true;

                    o.TokenValidationParameters.ValidateAudience = false;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = o.Authority,
                        ValidateLifetime = false
                    };
                });
            }
        }
    }
}
