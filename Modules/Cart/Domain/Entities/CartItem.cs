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

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            OldUnitPrice = oldUnitPrice;
            Quantity = quantity;
            PictureUrl = pictureUrl ?? string.Empty;
        }

        public void AddUnits(int units)
        {
            Quantity += units;
        }

        public void UpdateQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }

    }
}
