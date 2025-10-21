using Ecommerce.Shared.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecommerce.Shared.DataAccess
{
    public class DatabaseContextBuilder : IDbContextBuilder
    {
        private readonly IDatabaseProvider _databaseProvider;
        private readonly string _connectionString;
        private readonly Assembly _migrationAssembly;

        public DatabaseContextBuilder(
            IDatabaseProvider databaseProvider,
            string connectionString,
            Assembly migrationAssembly)
        {
            _databaseProvider = databaseProvider;
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        public void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database provider
            _databaseProvider.ConfigureOptions(optionsBuilder, _connectionString, _migrationAssembly);

            // Apply naming conventions if any
            _databaseProvider.ApplyNamingConventions(optionsBuilder);

            // Common configurations for all providers
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.EnableServiceProviderCaching();
        }
    }
}
