using Ecommerce.Shared.EventBus.Abtractions;
using Ecommerce.Shared.EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;


namespace Ecommerce.Shared.EventBus.Extensions
{
    public static class EventBusBuilderExtensions
    {
        public static IEventBusBuilder ConfigureJsonOptions(this IEventBusBuilder eventBusBuilder, Action<JsonSerializerOptions> configure)
        {
            eventBusBuilder.Services.AddSingleton<IConfigureOptions<EventBusSubscriptionInfo>>(sp =>
                new ConfigureOptions<EventBusSubscriptionInfo>(o =>
                {
                    configure(o.JsonSerializerOptions);
                })
            );

            return eventBusBuilder;
        }

        public static IEventBusBuilder AddSubscription<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH>(this IEventBusBuilder eventBusBuilder)
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
        {
            eventBusBuilder.Services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));

            eventBusBuilder.Services.AddSingleton<IConfigureOptions<EventBusSubscriptionInfo>>(sp =>
                new ConfigureOptions<EventBusSubscriptionInfo>(o =>
                {
                    o.EventTypes[typeof(T).Name] = typeof(T);
                })
            );

            return eventBusBuilder;
        }
    }
}
