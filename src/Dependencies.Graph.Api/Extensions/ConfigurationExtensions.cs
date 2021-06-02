using Microsoft.Extensions.Configuration;

namespace Dependencies.Graph.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool GetSecurityEnabled(this IConfiguration configuration) =>
            configuration.GetValue<bool>("Security:Enabled");

        public static string GetSecurityClientId(this IConfiguration configuration) =>
            configuration.GetValue<string>("Security:Oidc:ClientId");

        public static string GetSecurityAuthority(this IConfiguration configuration) =>
            configuration.GetValue<string>("Security:Oidc:Authority");

        public static string GetSecuritySwaggerClientId(this IConfiguration configuration) =>
            configuration.GetValue<string>("Security:Swagger:ClientId");

        public static string GetSecuritySwaggerClientSecret(this IConfiguration configuration) =>
            configuration.GetValue<string>("Security:Swagger:ClientSecret");
    }
}
