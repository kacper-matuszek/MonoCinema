
namespace MonoCinema.Application.CQRS.Events
{
    public interface IEventDispatcher
    {
        Task ExecuteAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
    }
}