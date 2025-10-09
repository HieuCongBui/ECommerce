using Ecommerce.Cart.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Cart.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCartApplication(this IServiceCollection services)
    {
        services.AddScoped<CartService>();

        return services;
    }
}