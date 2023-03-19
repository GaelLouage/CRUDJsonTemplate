using CRUDJsonTemplate.Models;
using Infrastructuur.Services.Classes;
using Infrastructuur.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDJsonTemplate.ExtensionServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonDatabase<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class, new()
        {
            return services.AddScoped<IJsonDatabase<MyEntity>>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var filename = configuration.GetValue<string>("JsonDatabase:MyEntity");
                return new JsonDatabase<MyEntity>(filename);
            });
      
        }
        // Build configuration
        public static IConfigurationRoot ConfigurationBuilder(this WebApplicationBuilder builder)
        {
            // Build configuration
                 return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .Build();
        }
    }
}
