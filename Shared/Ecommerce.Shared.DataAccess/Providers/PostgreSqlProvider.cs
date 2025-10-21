using Ecommerce.Shared.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Reflection;

namespace Ecommerce.Shared.DataAccess.Providers
{
    public class PostgreSqlProvider : IDatabaseProvider
    {
        public string ProviderName => "PostgreSQL";

        public void ConfigureOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, Assembly migrationAssembly)
        {
            ConfigureOptions(optionsBuilder, connectionString, migrationAssembly, null);
        }

        public void ConfigureOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, Assembly migrationAssembly, string? schema)
        {
            using var connection = CreateConnection(connectionString);
            string migrationAssemblyName = migrationAssembly.GetName().Name!;

            optionsBuilder.UseNpgsql(
                connection,
                options => 
                {
                    options.MigrationsAssembly(migrationAssemblyName);
                    
                    // Set schema if provided
                    if (!string.IsNullOrWhiteSpace(schema))
                    {
                        options.MigrationsHistoryTable("__EFMigrationsHistory", schema);
                    }
                });
        }

        public DbConnection CreateConnection(string connectionString)
            => new NpgsqlConnection(connectionString);

        public void ApplyNamingConventions(DbContextOptionsBuilder optionsBuilder)
        { }
    }
}
