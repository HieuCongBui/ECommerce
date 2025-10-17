using Ecommerce.Cart.Application.Services;
using Ecommerce.Cart.Application.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Cart.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCartApplication(this IServiceCollection services)
    {
        services.AddScoped<CartService>();

        services.AddScoped<IValidationService, ValidationService>();

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}