using System;
using Neo4j.Driver;

namespace Dependencies.Graph.Services
{
    public class GraphDriverService : IDisposable
    {
        private readonly Uri uri;
        private readonly string user;
        private readonly string password;
        private IDriver driver;
        private bool disposedValue;

        public GraphDriverService(Uri uri, string user, string password)
        {
            this.uri = uri;
            this.user = user;
            this.password = password;
        }

        public IDriver GetDriver()
        {
            if (driver == null)
            {
                var authToken = AuthTokens.None;

                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(user))
                    authToken = AuthTokens.Basic(user, password);

                driver = GraphDatabase.Driver(uri, authToken);
            }

            return driver;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    driver?.Dispose();

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
