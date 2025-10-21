using Ecommerce.Catalog.Infrastructure.Persistence;
using Ecommerce.Shared.DataAccess.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Catalog.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCatalogModule(
        this IServiceCollection services)
        {
            // Add DbContext with module-specific schema
            services.AddModuleDatabase<CatalogDbContext>(nameof(Catalog), "catalog");
            return services;
        }
    }
}
