using Ecommerce.Cart.Domain.Entities;

namespace Ecommerce.Cart.Domain.Abstractions
{
    public interface ICartRepository
    {
        Task<CustomerCart?> GetCartAsync(string customerId, CancellationToken cancellationToken = default);
        Task<CustomerCart?> UpdateCartAsync(CustomerCart cart, CancellationToken cancellationToken = default);
        Task<bool> DeleteCartAsync(string customerId, CancellationToken cancellationToken = default);
    }
}
