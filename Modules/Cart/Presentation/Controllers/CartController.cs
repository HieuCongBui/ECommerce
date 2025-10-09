using Ecommerce.Cart.Application.DTOs;
using Ecommerce.Cart.Application.Services;
using Ecommerce.Cart.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Cart.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;
    private readonly ILogger<CartController> _logger;

    public CartController(CartService cartService, ILogger<CartController> logger)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(CustomerCart), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CustomerCart>> GetCart(string customerId)
    {
        var cart = await _cartService.GetCartAsync(customerId);
        if (cart == null)
        {
            return NotFound($"Cart not found for customer {customerId}");
        }
        return Ok(cart);
    }

    [HttpPost("{customerId}/items")]
    [ProducesResponseType(typeof(CustomerCart), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CustomerCart>> AddItemToCart(string customerId, [FromBody] AddItemRequest request)
    {
        var addItemRequest = new AddItemToCartRequest
        {
            CustomerId = customerId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            UnitPrice = request.UnitPrice,
            OldUnitPrice = request.OldUnitPrice > 0 ? request.OldUnitPrice : null,
            Quantity = request.Quantity,
            PictureUrl = request.PictureUrl
        };

        var cart = await _cartService.AddItemToCartAsync(addItemRequest);
        return Ok(cart);
    }

    [HttpDelete("{customerId}/items/{productId}")]
    [ProducesResponseType(typeof(CustomerCart), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CustomerCart>> RemoveItemFromCart(string customerId, Guid productId)
    {
        var cart = await _cartService.RemoveItemFromCartAsync(customerId, productId);
        if (cart == null)
        {
            return NotFound($"Cart not found for customer {customerId}");
        }
        return Ok(cart);
    }

    [HttpPut("{customerId}/items/{productId}/quantity")]
    [ProducesResponseType(typeof(CustomerCart), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CustomerCart>> UpdateItemQuantity(string customerId, Guid productId, [FromBody] UpdateQuantityRequest request)
    {
        var updateRequest = new UpdateItemQuantityRequest
        {
            CustomerId = customerId,
            ProductId = productId,
            Quantity = request.Quantity
        };

        var cart = await _cartService.UpdateItemQuantityAsync(updateRequest);
        if (cart == null)
        {
            return NotFound($"Cart not found for customer {customerId}");
        }
        return Ok(cart);
    }

    [HttpDelete("{customerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> ClearCart(string customerId)
    {
        var result = await _cartService.ClearCartAsync(customerId);
        if (!result)
        {
            return NotFound($"Cart not found for customer {customerId}");
        }
        return NoContent();
    }

    [HttpGet("{customerId}/total")]
    [ProducesResponseType(typeof(decimal), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<decimal>> GetCartTotal(string customerId)
    {
        var total = await _cartService.GetCartTotalAsync(customerId);
        return Ok(new { Total = total });
    }
}

#region Helpers
public record AddItemRequest
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal OldUnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? PictureUrl { get; set; }
}

public record UpdateQuantityRequest
{
    public int Quantity { get; set; }
}
#endregion