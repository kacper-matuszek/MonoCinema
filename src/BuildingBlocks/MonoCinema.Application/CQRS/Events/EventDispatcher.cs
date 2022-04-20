using Microsoft.Extensions.DependencyInjection;

namespace MonoCinema.Application.CQRS.Events
{
    internal sealed class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task ExecuteAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            if (@event == null)
            {
                return;
            }

            var subscribers = _serviceProvider.GetServices<ISubscriber<TEvent>>();
            if (subscribers == null || !subscribers.Any())
            {
                return;
            }
            await Task.WhenAll(subscribers.Select(subscriber => subscriber.HandleAsync(@event, cancellationToken)));
        }
    }
}
