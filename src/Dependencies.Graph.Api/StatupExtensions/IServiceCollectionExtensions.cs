using AutoMapper;
using Dependencies.Graph.Dtos;
using Dependencies.Graph.Models;
using Dependencies.Graph.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dependencies.Graph.Api.StatupExtensions
{
    public static class IServiceCollectionExtensionsons
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var graphClient = new GraphDriverService(configuration.GetValue<string>("GraphConfig:Uri"),
                                                     configuration.GetValue<string>("GraphConfig:User"),
                                                     configuration.GetValue<string>("GraphConfig:Password"));

            services.AddSingleton(graphClient);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<AssemblyDto, Assembly>();
                cfg.CreateMap<Assembly, AssemblyDto>();
            });

            services.AddSingleton(config);

            services.AddScoped<AssemblyService>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}
