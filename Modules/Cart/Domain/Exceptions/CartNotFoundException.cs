using Ecommerce.Shared.Contract.Exceptions;

namespace Ecommerce.Cart.Domain.Exceptions;

public class CartNotFoundException : NotFoundException
{
    public CartNotFoundException(string customerId)
        : base($"Cart not found for customer '{customerId}'")
    { }

    public CartNotFoundException(string customerId, Exception innerException)
        : base($"Cart not found for customer '{customerId}'", innerException)
    { }
}