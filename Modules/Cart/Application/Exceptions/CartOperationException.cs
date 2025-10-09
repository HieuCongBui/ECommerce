using Ecommerce.Shared.Contract.Exceptions;

namespace Ecommerce.Cart.Application.Exceptions;

public class CartOperationException : DomainException
{
    public CartOperationException(string message)
        : base("Cart Operation Failed", message)
    {
    }

    public CartOperationException(string operation, string customerId, string details)
        : base("Cart Operation Failed", $"Failed to {operation} for customer '{customerId}': {details}")
    {
    }

    public CartOperationException(string message, Exception innerException)
        : base("Cart Operation Failed", message, innerException)
    {
    }

    public static CartOperationException RepositoryError(string operation, string customerId, Exception innerException)
    {
        return new CartOperationException($"Repository error during {operation} for customer '{customerId}'", innerException);
    }

    public static CartOperationException ServiceError(string operation, string customerId, string details)
    {
        return new CartOperationException(operation, customerId, details);
    }

    public static CartOperationException ConcurrencyError(string customerId)
    {
        return new CartOperationException("update cart", customerId, "Concurrency conflict detected");
    }
}