using Ecommerce.Shared.Contract.Shared;
using MediatR;

namespace Ecommerce.Shared.Contract.Abtractions.Commands
{
    public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
    { }

    public interface ICommandHandler<in TCommand, TResponse> 
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
    { }
}
