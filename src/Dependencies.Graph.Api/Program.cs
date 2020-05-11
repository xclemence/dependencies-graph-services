using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dependencies.Graph.Api
{
    public static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureLogging(logging =>
                     {
                         logging.ClearProviders();
                         logging.SetMinimumLevel(LogLevel.Trace);
                         logging.AddConsole();
                         logging.AddFile("logs/dependencies-graph-api-{Date}.txt");
                     });

                });
    }
}
