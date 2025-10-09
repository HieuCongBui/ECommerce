using Ecommerce.Cart.Application.DTOs;
using Ecommerce.Cart.Domain.Abstractions;
using Ecommerce.Cart.Domain.Entities;
using Ecommerce.Cart.Domain.Exceptions;
using Microsoft.Extensions.Logging;

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

        public async Task<CustomerCart> GetCartAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new EmptyCustomerIdException();
            }

            var cart = await _repository.GetCartAsync(customerId);

            if (cart == null)
            {
                throw new CartNotFoundException(customerId);
            }

            _logger.LogInformation("Retrieved cart for customer {CustomerId}", customerId);

            return cart;
        }

        public async Task<CustomerCart?> AddItemToCartAsync(AddItemToCartRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new EmptyCustomerIdException();
            }

            if (request.Quantity <= 0)
            {
                throw new InvalidCartItemQuantityException(request.Quantity);
            }

            if (request.UnitPrice < 0)
            {
                throw new InvalidCartItemPriceException(request.ProductId, request.UnitPrice);
            }

            var cart = await _repository.GetCartAsync(request.CustomerId) ?? new CustomerCart(request.CustomerId);

            cart.AddItem(
                request.ProductId,
                request.ProductName,
                request.UnitPrice,
                request.GetOldUnitPrice(),
                request.Quantity,
                request.PictureUrl ?? string.Empty);

            var updatedCart = await _repository.UpdateCartAsync(cart);
            _logger.LogInformation("Added item {ProductId} to cart for customer {CustomerId}",
                request.ProductId, request.CustomerId);

            return updatedCart;
        }

        public async Task<CustomerCart?> RemoveItemFromCartAsync(string customerId, Guid productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new EmptyCustomerIdException();
            }

            var cart = await _repository.GetCartAsync(customerId);
            if (cart == null)
            {
                throw new CartNotFoundException(customerId);
            }

            cart.RemoveItem(productId);

            var updatedCart = await _repository.UpdateCartAsync(cart);

            _logger.LogInformation("Removed item {ProductId} from cart for customer {CustomerId}", productId, customerId);

            return updatedCart;
        }

        public async Task<CustomerCart?> UpdateItemQuantityAsync(UpdateItemQuantityRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new EmptyCustomerIdException();
            }

            if (request.Quantity <= 0)
            {
                throw new InvalidCartItemQuantityException(request.ProductId, request.Quantity);
            }

            var cart = await _repository.GetCartAsync(request.CustomerId);
            if (cart == null)
            {
                throw new CartNotFoundException(request.CustomerId);
            }

            cart.UpdateItemQuantity(request.ProductId, request.Quantity);

            var updatedCart = await _repository.UpdateCartAsync(cart);

            _logger.LogInformation("Updated quantity for item {ProductId} in cart for customer {CustomerId}",
                request.ProductId, request.CustomerId);

            return updatedCart;
        }

        public async Task<bool> ClearCartAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new EmptyCustomerIdException();
            }

            var result = await _repository.DeleteCartAsync(customerId);
            if (!result)
            {
                throw new CartNotFoundException(customerId);
            }

            _logger.LogInformation("Cleared cart for customer {CustomerId}", customerId);
            return result;
        }

        public async Task<decimal> GetCartTotalAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new EmptyCustomerIdException();
            }

            var cart = await _repository.GetCartAsync(customerId);
            if (cart == null)
            {
                throw new CartNotFoundException(customerId);
            }

            var total = cart.GetTotalPrice();
            _logger.LogInformation("Retrieved cart total {Total} for customer {CustomerId}", total, customerId);
            return total;
        }
    }
}
