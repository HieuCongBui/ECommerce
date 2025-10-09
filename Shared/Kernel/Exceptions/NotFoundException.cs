namespace Ecommerce.Shared.Contract.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string message)
            : base("Not found", message) { }
        public NotFoundException(string message, Exception innerException)
        : base("Not found", message, innerException) { }
    }
}
