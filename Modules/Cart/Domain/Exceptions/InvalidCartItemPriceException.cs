using Ecommerce.Shared.Contract.Exceptions;

namespace Ecommerce.Cart.Domain.Exceptions;

public class InvalidCartItemPriceException : BadRequestException
{
    public InvalidCartItemPriceException(decimal price)
        : base($"Invalid price '{price}'. Unit price cannot be negative")
    { }

    public InvalidCartItemPriceException(Guid productId, decimal price)
        : base($"Invalid price '{price}' for product '{productId}'. Unit price cannot be negative")
    { }

    public InvalidCartItemPriceException(string message)
        : base(message)
    { }

    public InvalidCartItemPriceException(string message, Exception innerException)
        : base(message, innerException)
    { }
}