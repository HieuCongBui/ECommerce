using Ecommerce.Shared.DataAccess.Configuration;
using Ecommerce.Shared.DataAccess.Contracts;
using Ecommerce.Shared.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecommerce.Shared.DataAccess.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly migrationAssembly,
            string configurationSection = "Database")
        {
            var databaseOptions = configuration.GetSection(configurationSection).Get<DatabaseOptions>()
                ?? throw new InvalidOperationException($"Database configuration section '{configurationSection}' not found");

            return services.AddDatabase(databaseOptions, migrationAssembly);
        }

        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            DatabaseOptions databaseOptions,
            Assembly migrationAssembly)
        {
            // Register database provider
            var provider = DatabaseProviderFactory.CreateProvider(databaseOptions.Provider);
            services.AddSingleton<IDatabaseProvider>(provider);

            // Register database options
            services.AddSingleton(databaseOptions);

            // Register context builder
            services.AddScoped<IDbContextBuilder>(serviceProvider =>
            {
                var databaseProvider = serviceProvider.GetRequiredService<IDatabaseProvider>();
                return new DatabaseContextBuilder(
                    databaseProvider,
                    databaseOptions.ConnectionString,
                    migrationAssembly);
            });

            return services;
        }

        // Extension method to add module-specific database context
        public static IServiceCollection AddModuleDatabase<TContext>(
            this IServiceCollection services,
            string moduleName,
            string? schema = null)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>((serviceProvider, options) =>
            {
                var contextBuilder = serviceProvider.GetRequiredService<IDbContextBuilder>();
                var databaseOptions = serviceProvider.GetRequiredService<DatabaseOptions>();
                
                // Use module schema if provided, otherwise use from configuration
                var moduleSchema = schema ?? 
                    databaseOptions.ModuleSchemas.GetValueOrDefault(moduleName) ?? 
                    databaseOptions.DefaultSchema;

                contextBuilder.ConfigureContext(options, moduleSchema);
            });

            return services;
        }
    }
}