using Ecommerce.Shared.Contract.Shared;
using MediatR;

namespace Ecommerce.Shared.Contract.Abtractions.Queries
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    { }
}