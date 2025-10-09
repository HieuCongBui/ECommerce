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
                throw new ArgumentException("Quantity must be greater than zero");
            }

            if (unitPrice < 0)
            {
                throw new ArgumentException("Unit price cannot be negative");
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name is required");
            }

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            OldUnitPrice = oldUnitPrice;
            Quantity = quantity;
            PictureUrl = pictureUrl;
        }

        public void AddUnits(int units)
        {
            if (units <= 0)
            {
                throw new ArgumentException("Units must be greater than zero");
            }
            Quantity += units;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }
            Quantity = newQuantity;
        }

    }
}
