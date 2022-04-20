namespace MonoCinema.Application.CQRS.Events
{
    public interface ISubscriber<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
