using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;

namespace Ecommerce.Shared.DataAccess.Contracts
{
    public interface IDatabaseProvider
    {
        string ProviderName { get; }
        DbConnection CreateConnection(string connectionString);
        void ConfigureOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, Assembly migrationAssembly);
        void ApplyNamingConventions(DbContextOptionsBuilder optionsBuilder);
    }
}
