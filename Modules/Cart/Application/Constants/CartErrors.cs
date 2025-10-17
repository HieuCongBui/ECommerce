using Ecommerce.Shared.Contract.Commons;

namespace Ecommerce.Cart.Application.Constants
{
    /// <summary>
    /// Standardized error codes and messages for the Cart module
    /// </summary>
    public static class CartErrors
    {
        // Customer validation errors
        public static readonly Error CustomerIdRequired = Error.Validation(
            "Cart.CustomerIdRequired", 
            "Customer ID cannot be null or empty.");

        public static readonly Error CustomerIdInvalid = Error.Validation(
            "Cart.CustomerIdInvalid", 
            "Customer ID format is invalid.");

        // Product validation errors
        public static readonly Error ProductIdRequired = Error.Validation(
            "Cart.ProductIdRequired", 
            "Product ID cannot be empty.");

        public static readonly Error ProductIdInvalid = Error.Validation(
            "Cart.ProductIdInvalid", 
            "Product ID format is invalid.");

        // Cart not found errors
        public static Error CartNotFound(string customerId) => Error.NotFound(
            "Cart.NotFound", 
            $"Cart for customer '{customerId}' not found.");

        // Item not found errors
        public static Error ItemNotFound(Guid productId) => Error.NotFound(
            "Cart.ItemNotFound", 
            $"Item with product ID '{productId}' not found in cart.");

        // Quantity validation errors
        public static readonly Error QuantityMustBePositive = Error.Validation(
            "Cart.QuantityMustBePositive", 
            "Quantity must be greater than zero.");

        public static readonly Error QuantityInvalid = Error.Validation(
            "Cart.QuantityInvalid", 
            "Quantity value is invalid.");

        // Price validation errors
        public static readonly Error PriceMustBePositive = Error.Validation(
            "Cart.PriceMustBePositive", 
            "Unit price must be greater than or equal to zero.");

        public static readonly Error PriceInvalid = Error.Validation(
            "Cart.PriceInvalid", 
            "Price value is invalid.");

        // Repository operation errors
        public static readonly Error RepositoryUpdateFailed = Error.Failure(
            "Cart.RepositoryUpdateFailed", 
            "Failed to update cart in the repository.");

        public static readonly Error RepositoryDeleteFailed = Error.Failure(
            "Cart.RepositoryDeleteFailed", 
            "Failed to delete cart from the repository.");

        public static readonly Error RepositoryConnectionFailed = Error.Failure(
            "Cart.RepositoryConnectionFailed", 
            "Failed to connect to the cart repository.");

        // Business logic errors
        public static readonly Error CartIsEmpty = Error.Validation(
            "Cart.IsEmpty", 
            "Cart is empty and cannot be processed.");

        public static Error ItemAlreadyExists(Guid productId) => Error.Conflict(
            "Cart.ItemAlreadyExists", 
            $"Item with product ID '{productId}' already exists in cart.");

        public static readonly Error MaxItemsExceeded = Error.Validation(
            "Cart.MaxItemsExceeded", 
            "Maximum number of items in cart has been exceeded.");

        public static readonly Error MaxQuantityExceeded = Error.Validation(
            "Cart.MaxQuantityExceeded", 
            "Maximum quantity per item has been exceeded.");

        // General operation errors
        public static readonly Error OperationFailed = Error.Failure(
            "Cart.OperationFailed", 
            "Cart operation failed due to an unexpected error.");

        public static Error CustomValidation(string code, string message) => Error.Validation(
            $"Cart.{code}", 
            message);

        public static Error CustomNotFound(string code, string message) => Error.NotFound(
            $"Cart.{code}", 
            message);

        public static Error CustomFailure(string code, string message) => Error.Failure(
            $"Cart.{code}", 
            message);
    }
}