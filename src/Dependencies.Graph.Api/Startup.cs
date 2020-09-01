using System;
using System.Reflection;
using Dependencies.Graph.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Dependencies.Graph.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public static string Version =>
            typeof(Startup).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version.Replace(".", "_", StringComparison.OrdinalIgnoreCase);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.RegisterServices(Configuration);
            services.ConfigureCors();

            services.AddSwaggerGen(c => c.SwaggerDoc($"v{Version}", new OpenApiInfo { Title = "Dependency Graph Services", Version = $"v{ Version }" }));

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment _, ILogger<Startup> logger)
        {
            if (Configuration.GetValue<bool>("ForceHttpsRedirection"))
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"./swagger/v{Version}/swagger.json", "Dependency Graph Services");
                c.RoutePrefix = string.Empty;
            });

            app.ConfigureExceptionHandler(logger);
            
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
