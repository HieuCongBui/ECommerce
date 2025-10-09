using Ecommerce.Shared.Contract.Shared;
using MediatR;

namespace Ecommerce.Shared.Contract.Abtractions.Queries
{
    public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
    { }
}
