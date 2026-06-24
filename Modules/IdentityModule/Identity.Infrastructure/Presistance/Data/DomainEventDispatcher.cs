using Identity.Domain.Interface;
using MediatR;


namespace Identity.Infrastructure.Presistance.Data
{
    public sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IPublisher _publisher;


        public DomainEventDispatcher(IPublisher publisher)
        {
            _publisher = publisher;

        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent as INotification);
            }
        }
    }
}
