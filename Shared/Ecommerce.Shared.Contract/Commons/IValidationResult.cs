namespace Ecommerce.Shared.Contract.Commons;

public interface IValidationResult
{
    static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}
