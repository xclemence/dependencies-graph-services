namespace Dependencies.Graph.Api.Options
{
    public class SecurityOption
    {
        public bool Enabled { get; init; }
        public SecurityOidc Oidc { get; init; }
        public SecuritySwagger Swagger { get; init; }
        public string RoleMappings { get; init; }
    }
}
