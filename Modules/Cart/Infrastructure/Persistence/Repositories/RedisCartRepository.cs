using Ecommerce.Cart.Domain.Abstractions;
using Ecommerce.Cart.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ecommerce.Cart.Infrastructure.Persistence.Repositories
{
    public class RedisCartRepository(ILogger<RedisCartRepository> logger, IDistributedCache cache) : ICartRepository
    {
        private readonly TimeSpan DefaultTimeExpiration = TimeSpan.FromMinutes(15);

        private static RedisKey RedisKeyPrefix = "cart:"u8.ToArray();
        private static RedisKey GetCartKey(string customerId) => RedisKeyPrefix.Append(System.Text.Encoding.UTF8.GetBytes(customerId));

        public async Task<bool> DeleteCartAsync(string customerId, CancellationToken cancellationToken = default)
        {
            await cache.RemoveAsync(GetCartKey(customerId), cancellationToken);
            return true;
        }

        public async Task<CustomerCart?> GetCartAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var data = await cache.GetAsync(GetCartKey(customerId), cancellationToken);

            return data is not null
                ? JsonSerializer.Deserialize(data, CartSerializationContext.Default.CustomerCart)
                : null;
        }

        public async Task<CustomerCart?> UpdateCartAsync(CustomerCart cart, CancellationToken cancellationToken = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(cart, CartSerializationContext.Default.CustomerCart);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = DefaultTimeExpiration
            };

            await cache.SetAsync(GetCartKey(cart.CustomerId), bytes, options, cancellationToken);

            logger.LogInformation("Cart persisted successfully.");
            return await GetCartAsync(cart.CustomerId, cancellationToken);
        }
    }

    [JsonSerializable(typeof(CustomerCart))]
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    public partial class CartSerializationContext : JsonSerializerContext
    { }
}
