using Ecommerce.Shared.Contract.Exceptions;

namespace Ecommerce.Cart.Domain.Exceptions;

public class InvalidCartItemQuantityException : BadRequestException
{
    public InvalidCartItemQuantityException(int quantity)
        : base($"Invalid quantity '{quantity}'. Quantity must be greater than zero")
    { }

    public InvalidCartItemQuantityException(Guid productId, int quantity)
        : base($"Invalid quantity '{quantity}' for product '{productId}'. Quantity must be greater than zero")
    { }

    public InvalidCartItemQuantityException(string message)
        : base(message)
    { }

    public InvalidCartItemQuantityException(string message, Exception innerException)
        : base(message, innerException)
    { }
}