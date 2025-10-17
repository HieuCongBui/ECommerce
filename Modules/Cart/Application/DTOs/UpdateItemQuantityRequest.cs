using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Cart.Application.DTOs
{
    public record UpdateItemQuantityRequest
    {
        public int Quantity { get; set; }
    }
}