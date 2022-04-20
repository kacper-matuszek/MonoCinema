
namespace MonoCinema.Application.CQRS.Commands
{
    public interface ICommandDispatcher
    {
        Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
    }
}