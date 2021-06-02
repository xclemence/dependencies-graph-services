using System;
using Dependencies.Graph.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Dependencies.Graph.Api.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwagger(this IServiceCollection services, string version, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{version}", new OpenApiInfo { Title = "Dependency Graph Services", Version = $"v{ version }" });

                if (configuration.GetSecurityEnabled())
                {
                    c.AddSecurityDefinition("Dependencies", GetScheme(new Uri($"{configuration.GetSecurityAuthority()}/.well-known/openid-configuration")));
                    c.AddSecurityRequirement(GetRequirement("Dependencies"));
                }
            });
        }

        private static OpenApiSecurityScheme GetScheme(Uri openIdConnectUri) => new()
        {
            Type = SecuritySchemeType.OpenIdConnect,
            OpenIdConnectUrl = openIdConnectUri,
            In = ParameterLocation.Header,
            Name = "Authorization",
            Scheme = JwtBearerDefaults.AuthenticationScheme,
        };

        private static OpenApiSecurityRequirement GetRequirement(string referenceId) => new()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = referenceId
                    }
                },
                Array.Empty<string>()
            }
        };


        public static void UseSwaggerUI(this IApplicationBuilder app, string version, IConfiguration configuration)
        {
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint($"v{version}/swagger.json", "Dependency Graph Services");

                if (configuration.GetSecurityEnabled())
                {
                    c.OAuthClientId(configuration.GetSecuritySwaggerClientId());
                    c.OAuthClientSecret(configuration.GetSecuritySwaggerClientSecret());
                }
            });
        }
    }
}
