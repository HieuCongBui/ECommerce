using Ecommerce.Cart.Application.Services;
using Ecommerce.Cart.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

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
        try
        {
            var cart = await _cartService.AddItemToCartAsync(
                customerId,
                request.ProductId,
                request.ProductName,
                request.UnitPrice,
                request.Quantity,
                request.PictureUrl);

            return Ok(cart);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
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
        var cart = await _cartService.UpdateItemQuantityAsync(customerId, productId, request.Quantity);
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
        try
        {
            var total = await _cartService.GetCartTotalAsync(customerId);
            return Ok(new { Total = total });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record AddItemRequest
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? PictureUrl { get; set; }
}

public record UpdateQuantityRequest
{
    public int Quantity { get; set; }
}