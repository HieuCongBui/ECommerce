using Ecommerce.Cart.Domain.Abstractions;
using System.Text.Json.Serialization;

namespace Ecommerce.Cart.Domain.Entities
{
    public class CustomerCart : IAggregateRoot
    {
        private readonly List<CartItem> _items = new();

        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public string CustomerId { get; private set; }

        public CustomerCart(string customerId)
        {
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        }

        [JsonConstructor]
        public CustomerCart(string customerId, IReadOnlyCollection<CartItem> items)
        {
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
            _items = items?.ToList() ?? new List<CartItem>();
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity, string pictureUrl)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem is null)
            {
                var newItem = new CartItem(productId, productName, unitPrice, quantity, pictureUrl);
                _items.Add(newItem);
            }
            else
            {
                existingItem.AddUnits(quantity);
            }
        }

        public void RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);

            if (item is null)
            {
                throw new InvalidOperationException("Item not found in cart");
            }

            _items.Remove(item);
        }

        public void UpdateItemQuantity(Guid productId, int newQuantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);

            if (item is null)
            {
                throw new InvalidOperationException("Item not found in cart");
            }

            item.UpdateQuantity(newQuantity);
        }

        public decimal GetTotalPrice()
        {
            return _items.Sum(i => i.UnitPrice * i.Quantity);
        }
    }
}
