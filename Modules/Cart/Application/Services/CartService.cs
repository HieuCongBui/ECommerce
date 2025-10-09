using Ecommerce.Cart.Domain.Abstractions;
using Ecommerce.Cart.Domain.Entities;
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

        public async Task<CustomerCart?> GetCartAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            try
            {
                var cart = await _repository.GetCartAsync(customerId);
                _logger.LogInformation("Retrieved cart for customer {CustomerId}", customerId);
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart for customer {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<CustomerCart> AddItemToCartAsync(string customerId, Guid productId, string productName, decimal unitPrice, int quantity, string pictureUrl)
        {
            try
            {
                var cart = await _repository.GetCartAsync(customerId) ?? new CustomerCart(customerId);

                cart.AddItem(productId, productName, unitPrice, quantity, pictureUrl ?? string.Empty);
                
                var updatedCart = await _repository.UpdateCartAsync(cart);
                _logger.LogInformation("Added item {ProductId} to cart for customer {CustomerId}", productId, customerId);
                
                return updatedCart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item {ProductId} to cart for customer {CustomerId}", productId, customerId);
                throw;
            }
        }

        public async Task<CustomerCart?> RemoveItemFromCartAsync(string customerId, Guid productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            try
            {
                var cart = await _repository.GetCartAsync(customerId);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found for customer {CustomerId}", customerId);
                    return null;
                }

                cart.RemoveItem(productId);
                
                var updatedCart = await _repository.UpdateCartAsync(cart);
                _logger.LogInformation("Removed item {ProductId} from cart for customer {CustomerId}", productId, customerId);
                
                return updatedCart;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Item {ProductId} not found in cart for customer {CustomerId}: {Message}", productId, customerId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ProductId} from cart for customer {CustomerId}", productId, customerId);
                throw;
            }
        }

        public async Task<CustomerCart?> UpdateItemQuantityAsync(string customerId, Guid productId, int newQuantity)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

            try
            {
                var cart = await _repository.GetCartAsync(customerId);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found for customer {CustomerId}", customerId);
                    return null;
                }

                cart.UpdateItemQuantity(productId, newQuantity);
                
                var updatedCart = await _repository.UpdateCartAsync(cart);
                _logger.LogInformation("Updated quantity for item {ProductId} in cart for customer {CustomerId}", productId, customerId);
                
                return updatedCart;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Item {ProductId} not found in cart for customer {CustomerId}: {Message}", productId, customerId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item {ProductId} quantity in cart for customer {CustomerId}", productId, customerId);
                throw;
            }
        }

        public async Task<bool> ClearCartAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            try
            {
                var result = await _repository.DeleteCartAsync(customerId);
                _logger.LogInformation("Cleared cart for customer {CustomerId}", customerId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for customer {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<decimal> GetCartTotalAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            try
            {
                var cart = await _repository.GetCartAsync(customerId);
                var total = cart?.GetTotalPrice() ?? 0;
                _logger.LogInformation("Retrieved cart total {Total} for customer {CustomerId}", total, customerId);
                return total;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart total for customer {CustomerId}", customerId);
                throw;
            }
        }
    }
}
