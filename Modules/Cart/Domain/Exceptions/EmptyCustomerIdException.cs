using Ecommerce.Shared.Contract.Exceptions;

namespace Ecommerce.Cart.Domain.Exceptions;

public class EmptyCustomerIdException : BadRequestException
{
    public EmptyCustomerIdException()
        : base("Customer ID cannot be null or empty")
    { }

    public EmptyCustomerIdException(string message)
        : base(message)
    { }

    public EmptyCustomerIdException(string message, Exception innerException)
        : base(message, innerException)
    { }
}