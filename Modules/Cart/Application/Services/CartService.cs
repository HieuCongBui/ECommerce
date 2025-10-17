using Ecommerce.Cart.Domain.Abstractions;
using Ecommerce.Cart.Domain.Entities;
using Ecommerce.Shared.Contract.Commons;
using Microsoft.Extensions.Logging;
using Ecommerce.Shared.Contract.Abtractions.Enums;
using Ecommerce.Cart.Application.Constants;

namespace Ecommerce.Cart.Application.Services
{
    public class CartService
    {
        private readonly ICartRepository _repository;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository repository, ILogger<CartService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResultT<CustomerCart>> GetCartAsync(string customerId)
        {
            var validationResult = ValidateCustomerId(customerId);
            if (!validationResult.IsSuccess)
            {
                return validationResult.Error!;
            }

            var cart = await _repository.GetCartAsync(customerId);

            if (cart == null)
            {
                return CartErrors.CartNotFound(customerId);
            }

            _logger.LogInformation("Retrieved cart for customer {CustomerId}", customerId);

            return ResultT<CustomerCart>.Success(cart);
        }

        public async Task<ResultT<CustomerCart>> AddItemToCartAsync(string customerId, CartItemRequest request)
        {
            var validationResult = ValidateCustomerId(customerId);
            if (!validationResult.IsSuccess)
            {
                return validationResult.Error!;
            }

            var cart = await _repository.GetCartAsync(customerId) ?? new CustomerCart(customerId);

            cart.AddItem(
                request.ProductId,
                request.ProductName ?? string.Empty,
                request.UnitPrice,
                request.OldUnitPrice,
                request.Quantity,
                request.PictureUrl ?? string.Empty);

            var updatedCart = await _repository.UpdateCartAsync(cart);
            if (updatedCart == null)
            {
                return CartErrors.RepositoryUpdateFailed;
            }

            _logger.LogInformation("Added item {ProductId} to cart for customer {CustomerId}",
                request.ProductId, customerId);

            return ResultT<CustomerCart>.Success(updatedCart);
        }

        public async Task<ResultT<CustomerCart>> RemoveItemFromCartAsync(string customerId, Guid productId)
        {
            var customerValidation = ValidateCustomerId(customerId);
            if (!customerValidation.IsSuccess)
            {
                return customerValidation.Error!;
            }

            var productValidation = ValidateProductId(productId);
            if (!productValidation.IsSuccess)
            {
                return productValidation.Error!;
            }

            var cart = await _repository.GetCartAsync(customerId);
            if (cart == null)
            {
                return CartErrors.CartNotFound(customerId);
            }

            cart.RemoveItem(productId);

            var updatedCart = await _repository.UpdateCartAsync(cart);
            if (updatedCart == null)
            {
                return CartErrors.RepositoryUpdateFailed;
            }

            _logger.LogInformation("Removed item {ProductId} from cart for customer {CustomerId}", productId, customerId);

            return ResultT<CustomerCart>.Success(updatedCart);
        }

        public async Task<ResultT<CustomerCart>> UpdateItemQuantityAsync(string customerId, Guid productId, UpdateItemQuantityRequest request)
        {
            var customerValidation = ValidateCustomerId(customerId);
            if (!customerValidation.IsSuccess)
            {
                return customerValidation.Error!;
            }

            var productValidation = ValidateProductId(productId);
            if (!productValidation.IsSuccess)
            {
                return productValidation.Error!;
            }

            var quantityValidation = ValidateQuantity(request.Quantity);
            if (!quantityValidation.IsSuccess)
            {
                return quantityValidation.Error!;
            }

            var cart = await _repository.GetCartAsync(customerId);
            if (cart == null)
            {
                return CartErrors.CartNotFound(customerId);
            }

            cart.UpdateItemQuantity(productId, request.Quantity);

            var updatedCart = await _repository.UpdateCartAsync(cart);
            if (updatedCart == null)
            {
                return CartErrors.RepositoryUpdateFailed;
            }

            _logger.LogInformation("Updated quantity for item {ProductId} in cart for customer {CustomerId}",
                productId, customerId);

            return ResultT<CustomerCart>.Success(updatedCart);
        }

        public async Task<Result> ClearCartAsync(string customerId)
        {
            var validationResult = ValidateCustomerId(customerId);
            if (!validationResult.IsSuccess)
            {
                return validationResult.Error!;
            }

            var result = await _repository.DeleteCartAsync(customerId);
            if (!result)
            {
                return CartErrors.CartNotFound(customerId);
            }

            _logger.LogInformation("Cleared cart for customer {CustomerId}", customerId);

            return Result.Success();
        }

        public async Task<ResultT<decimal>> GetCartTotalAsync(string customerId)
        {
            var validationResult = ValidateCustomerId(customerId);
            if (!validationResult.IsSuccess)
            {
                return validationResult.Error!;
            }

            var cart = await _repository.GetCartAsync(customerId);
            if (cart == null)
            {
                return CartErrors.CartNotFound(customerId);
            }

            var total = cart.GetTotalPrice();

            _logger.LogInformation("Retrieved cart total {Total} for customer {CustomerId}", total, customerId);

            return ResultT<decimal>.Success(total);
        }

        // Private validation helper methods
        private static Result ValidateCustomerId(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return CartErrors.CustomerIdRequired;
            }

            return Result.Success();
        }

        private static Result ValidateProductId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return CartErrors.ProductIdRequired;
            }

            return Result.Success();
        }

        private static Result ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                return CartErrors.QuantityMustBePositive;
            }

            return Result.Success();
        }
    }
}
