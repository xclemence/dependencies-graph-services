using System;
using System.Reflection;
using Dependencies.Graph.Api.Configuration;
using Dependencies.Graph.Api.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddMvc(opts =>
            {
                opts.EnableEndpointRouting = false;
                opts.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AllowAnonymousFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.RegisterServices(Configuration);

            services.AddOptions();
            services.Configure<SecurityOption>(Configuration.GetSection("Security"));

            services.ConfigureCors();

            services.AddSwagger(Version, Configuration);

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

            services.ConfigureAuthorization(Configuration);
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

            app.UseSwaggerUI(Version, Configuration);

            app.ConfigureExceptionHandler(logger);

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
