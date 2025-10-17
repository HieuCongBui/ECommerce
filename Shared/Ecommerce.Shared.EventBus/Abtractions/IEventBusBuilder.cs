using Microsoft.Extensions.DependencyInjection;
namespace Ecommerce.Shared.EventBus.Abtractions
{
    public interface IEventBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
