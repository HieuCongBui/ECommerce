
using Ecommerce.Shared.EventBus.Events;

namespace Ecommerce.Shared.EventBus.Abtractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent @event);
    }
}
