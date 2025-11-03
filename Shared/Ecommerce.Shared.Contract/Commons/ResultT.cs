namespace Ecommerce.Shared.Contract.Commons;

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    private readonly string _errorMessage;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
        _errorMessage = error.Message;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(_errorMessage ?? "The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
