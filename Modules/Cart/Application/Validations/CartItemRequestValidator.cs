namespace Ecommerce.Cart.Application.Validations
{
    public class CartItemRequestValidator : AbstractValidator<CartItemRequest>
    {
        public CartItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative.");   
        }
    }
}
