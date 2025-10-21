using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Shared.DataAccess.Extensions;
using Ecommerce.Catalog.Infrastructure.Persistence;

namespace Ecommerce.Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services)
    {
        // Add DbContext with module-specific schema
        services.AddModuleDatabase<CatalogDbContext>(nameof(Catalog), "catalog");
        return services;
    }
}