using System;
using Core.Settings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
       public static IServiceCollection AddAndMigrateTenantDatabase(this IServiceCollection services, IConfiguration config)
        {
            var option = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            var defaultConnectionString = option.Default?.ConnectionString;
            var defaultDbProvider = option.Default?.DBProvider;
            if(defaultDbProvider?.ToLower() == "mssql")
            {
                services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            var tenants = option.Tenants;
            foreach(var a in tenants)
            {
                string connectionString;
                if (string.IsNullOrEmpty(a.ConnectionString))
                {
                    connectionString = defaultConnectionString!;
                }
                else
                {
                    connectionString = a.ConnectionString;
                }
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if(dbContext.Database.GetMigrations().Count() > 0)
                {
                    dbContext.Database.Migrate();
                }
            }

            return services;
        }

        public static T GetOptions<T> (this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;

        }
    }
}

