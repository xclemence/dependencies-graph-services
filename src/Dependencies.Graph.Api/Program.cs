using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Dependencies.Graph.Api
{
    public static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseKestrel()
                          .UseUrls("http://*:80")
                          .UseStartup<Startup>()
                          .ConfigureLogging(logging =>
                          {
                              logging.ClearProviders();
                              logging.SetMinimumLevel(LogLevel.Trace);
                              logging.AddConsole();
                              logging.AddNLog();
                          });
        }
    }
}
