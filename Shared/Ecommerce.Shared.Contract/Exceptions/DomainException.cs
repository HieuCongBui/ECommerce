namespace Ecommerce.Shared.Contract.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string title, string message) : base(message)
            => Title = title;
        protected DomainException(string title, string message, Exception innerException)
        : base(message, innerException)
        => Title = title;

        public string Title { get; }
    }
}
