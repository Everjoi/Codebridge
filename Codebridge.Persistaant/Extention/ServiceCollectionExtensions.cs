using Autofac.Extensions.DependencyInjection;
using Autofac;
using Codebridge.Persistant.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebridge.Persistant.ModuleContainer;
using System.Reflection;

namespace Codebridge.Persistant.Extention
{
    public static class ServiceCollectionExtensions
    {

        public static void AddPersistenceLayer(this IServiceCollection  services,IConfiguration configuration,IHostBuilder host)
        {
            services.AddDbContext(configuration);
            host.AddRepositories();
        }



        public static void AddDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CodebridgeContext>(options =>
               options.UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(CodebridgeContext).Assembly.FullName)));
        }


        private static void AddRepositories(this IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
                ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new CodebridgeModule());
                });

        }

    }
}

