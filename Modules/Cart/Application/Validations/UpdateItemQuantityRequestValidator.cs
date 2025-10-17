namespace Ecommerce.Cart.Application.Validations
{
    public class UpdateItemQuantityRequestValidator : AbstractValidator<UpdateItemQuantityRequest>
    {
        public UpdateItemQuantityRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");
        }
    }
}
