using Ecommerce.Cart.Application.Extensions;
using Ecommerce.Cart.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Cart.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCartModule(this IServiceCollection services, string redisConnectionString)
    {
        // Add all cart module services
        services.AddCartApplication();
        services.AddCartInfrastructure(redisConnectionString);

        return services;
    }
}