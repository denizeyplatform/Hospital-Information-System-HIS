using HIS.Domain.Aggregates.PatientAggregate.DomainEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Events
{
    public class PatientCreatedHandler : INotificationHandler<INotificationWrapper<PatientCreatedDomainEvent>>
    {
        public Task Handle(INotificationWrapper<PatientCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(
            $"Patient Created: {notification.DomainEvent.PatientId}");

            return Task.CompletedTask;
        }
    }


}
