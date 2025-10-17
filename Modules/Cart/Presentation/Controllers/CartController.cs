using Ecommerce.Cart.Application.DTOs;
using Ecommerce.Cart.Application.Services;
using Ecommerce.Cart.Domain.Entities;
using Ecommerce.Cart.Application.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ecommerce.Shared.Contract.Abtractions.Enums;
using Ecommerce.Shared.Contract.Extensions;
using Ecommerce.Shared.Contract.Commons;

namespace Ecommerce.Cart.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController(CartService cartService, IValidationService validationService) : ControllerBase
{
    private readonly CartService _cartService = cartService;
    private readonly IValidationService _validationService = validationService;

    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(CustomerCart), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCart(string customerId)
    {
        var result = await _cartService.GetCartAsync(customerId);
        
        return result.Match(
            onSuccess: value => Ok(result),
            onFailure: error => MapErrorToActionResult(error)
        );
    }

    [HttpPost("{customerId}/items")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> AddItemToCart(string customerId, [FromBody] CartItemRequest request)
    {
        var validationResult = await _validationService.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorsMessages = string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage));
            var validationError = CartErrors.CustomValidation("RequestValidation", errorsMessages);
            return MapErrorToActionResult(validationError);
        }

        var result = await _cartService.AddItemToCartAsync(customerId, request);
        
        return result.Match(
            onSuccess: value => Ok(result),
            onFailure: error => MapErrorToActionResult(error)
        );
    }

    [HttpDelete("{customerId}/items/{productId}")]
    [ProducesResponseType(typeof(CustomerCart),(int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RemoveItemFromCart(string customerId, Guid productId)
    {
        var result = await _cartService.RemoveItemFromCartAsync(customerId, productId);
        
        return result.Match(
            onSuccess : value => Ok(result),
            onFailure: error => MapErrorToActionResult(error)
        );
    }

    [HttpPut("{customerId}/items/{productId}/quantity")]
    [ProducesResponseType(typeof(CustomerCart),(int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateItemQuantity(string customerId, Guid productId, [FromBody] UpdateItemQuantityRequest request)
    {
        var validationResult = await _validationService.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorsMessages = string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage));
            var validationError = CartErrors.CustomValidation("RequestValidation", errorsMessages);
            return MapErrorToActionResult(validationError);
        }

        var result = await _cartService.UpdateItemQuantityAsync(customerId, productId, request);
        
        return result.Match(
            onSuccess: value => Ok(result),
            onFailure: error => MapErrorToActionResult(error)
        );
    }

    [HttpDelete("{customerId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> ClearCart(string customerId)
    {
        var result = await _cartService.ClearCartAsync(customerId);
        
        return result.Match(
            onSuccess: () => NoContent(),
            onFailure: error => MapErrorToActionResult(error)
        );
    }

    [HttpGet("{customerId}/total")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetCartTotal(string customerId)
    {
        var result = await _cartService.GetCartTotalAsync(customerId);
        
        return result.Match(
            onSuccess: total => Ok(new { Total = total }),
            onFailure: error => MapErrorToActionResult(error)
        );
    }

    #region Wrapper method
    private IActionResult MapErrorToActionResult(Error error)
    {
        var errorResponse = new { error = error.Description, code = error.Code };

        return error.ErrorType switch
        {
            ErrorType.Validation => BadRequest(errorResponse),
            ErrorType.NotFound => NotFound(errorResponse),
            ErrorType.Conflict => Conflict(errorResponse),
            ErrorType.AccessUnAuthorized => Unauthorized(errorResponse),
            ErrorType.AccessForbidden => StatusCode(403, errorResponse),
            ErrorType.Failure => StatusCode(500, errorResponse),
            _ => StatusCode(500, errorResponse)
        };
    }
    #endregion
}
