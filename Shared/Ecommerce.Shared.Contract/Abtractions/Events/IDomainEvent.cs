namespace Ecommerce.Shared.Contract.Abtractions.Events
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        public DateTime CreationDate { get; }
    }
}
