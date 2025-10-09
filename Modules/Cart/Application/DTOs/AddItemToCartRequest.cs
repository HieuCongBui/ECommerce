using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Cart.Application.DTOs
{
    public class AddItemToCartRequest
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public Guid ProductId { get; set; }

        public string? ProductName { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal? OldUnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? PictureUrl { get; set; }

        public decimal GetOldUnitPrice() => OldUnitPrice ?? UnitPrice;
    }
}