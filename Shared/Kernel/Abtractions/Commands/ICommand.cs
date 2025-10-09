using Ecommerce.Shared.Contract.Shared;
using MediatR;
namespace Ecommerce.Shared.Contract.Abtractions.Commands
{
    public interface ICommand : IRequest<Result>
    { }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    { }
}
