using Ecommerce.Cart.Domain.Exceptions;

namespace Ecommerce.Cart.Domain.Entities
{
    public class CartItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal OldUnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string PictureUrl { get; private set; }

        public CartItem(Guid productId, string productName, decimal unitPrice, decimal oldUnitPrice, int quantity, string pictureUrl)
        {
            if (quantity <= 0)
            {
                throw new InvalidCartItemQuantityException(productId, quantity);
            }

            if (unitPrice < 0)
            {
                throw new InvalidCartItemPriceException(productId, unitPrice);
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name is required", nameof(productName));
            }

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            OldUnitPrice = oldUnitPrice;
            Quantity = quantity;
            PictureUrl = pictureUrl ?? string.Empty;
        }

        public void AddUnits(int units)
        {
            if (units <= 0)
            {
                throw new InvalidCartItemQuantityException($"Units to add must be greater than zero. Attempted to add: {units}");
            }

            Quantity += units;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new InvalidCartItemQuantityException(ProductId, newQuantity);
            }

            Quantity = newQuantity;
        }

    }
}
