using Ecommerce.Shared.Contract.Exceptions;

namespace Ecommerce.Cart.Domain.Exceptions;

public class CartItemNotFoundException : NotFoundException
{
    public CartItemNotFoundException(Guid productId)
        : base($"Cart item with product ID '{productId}' not found")
    { }

    public CartItemNotFoundException(string customerId, Guid productId)
        : base($"Cart item with product ID '{productId}' not found in cart for customer '{customerId}'")
    { }

    public CartItemNotFoundException(Guid productId, Exception innerException)
        : base($"Cart item with product ID '{productId}' not found", innerException)
    { }
}