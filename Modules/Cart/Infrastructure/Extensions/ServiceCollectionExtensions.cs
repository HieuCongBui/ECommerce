using Ecommerce.Cart.Domain.Abstractions;
using Ecommerce.Cart.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Ecommerce.Cart.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, string redisConnectionString)
    {

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "CartCache:";
        });

        services.AddScoped<ICartRepository, RedisCartRepository>();

        return services;
    }
}