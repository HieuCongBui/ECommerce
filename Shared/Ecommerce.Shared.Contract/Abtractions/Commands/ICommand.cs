using Ecommerce.Shared.Contract.Commons;
using MediatR;

namespace Ecommerce.Shared.Contract.Abtractions.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

