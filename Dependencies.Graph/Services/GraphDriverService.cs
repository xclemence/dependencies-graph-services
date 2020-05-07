using System;
using Neo4j.Driver;

namespace Dependencies.Graph.Services
{
    public class GraphDriverService : IDisposable
    {
        private readonly string uri;
        private readonly string user;
        private readonly string password;

        private IDriver driver;

        public GraphDriverService(string uri, string user, string password)
        {
            this.uri = uri;
            this.user = user;
            this.password = password;
        }

        public void Dispose() => driver?.Dispose();

        public IDriver GetDriver()
        {
            if (driver == null)
            {
                var authToken = AuthTokens.None;

                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(user))
                    authToken = AuthTokens.Basic(user, password);

                // TODO Add Logger !
                driver = GraphDatabase.Driver(uri, authToken);
            }

            return driver;
        }
    }
}
