namespace Ecommerce.Shared.Contract.Shared
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(bool isSuccess, TValue? value, string? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access the value of a failed result.");

        public static Result<TValue> Success(TValue value) => new(true, value, null);

        public static new Result<TValue> Failure(string error) => new(false, default, error);
    }
}
