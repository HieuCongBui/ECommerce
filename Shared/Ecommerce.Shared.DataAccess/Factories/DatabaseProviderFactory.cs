using Ecommerce.Shared.DataAccess.Configuration;
using Ecommerce.Shared.DataAccess.Contracts;
using Ecommerce.Shared.DataAccess.Providers;

namespace Ecommerce.Shared.DataAccess.Factories
{
    public class DatabaseProviderFactory
    {
        public static IDatabaseProvider CreateProvider(DatabaseProviderType providerType)
        {
            return providerType switch
            {
                DatabaseProviderType.PostgreSQL => new PostgreSqlProvider(),
                _ => throw new NotSupportedException($"The database provider '{providerType}' is not supported.")
            };
        }
    }
}