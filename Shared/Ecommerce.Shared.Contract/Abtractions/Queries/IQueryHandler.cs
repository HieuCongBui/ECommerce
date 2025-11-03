using Ecommerce.Shared.Contract.Commons;
using MediatR;

namespace Ecommerce.Shared.Contract.Abtractions.Queries;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
