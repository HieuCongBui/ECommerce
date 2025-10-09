using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Cart.Application.DTOs
{
    public class UpdateItemQuantityRequest
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}