using Ecommerce.Shared.Contract.Commons;
using MediatR;

namespace Ecommerce.Shared.Contract.Abtractions.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}