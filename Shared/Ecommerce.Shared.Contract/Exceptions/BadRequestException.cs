namespace Ecommerce.Shared.Contract.Exceptions
{
    public class BadRequestException : DomainException
    {
        public BadRequestException(string message)
            : base("Bad request", message) { }

        public BadRequestException(string message, Exception innerException)
        : base("Bad request", message, innerException) { }
    }
}
