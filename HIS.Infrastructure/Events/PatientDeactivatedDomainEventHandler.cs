using HIS.Domain.Aggregates.PatientAggregate.DomainEvents;
using HIS.Infrastructure.Presestance;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Events
{
    public class PatientDeactivatedDomainEventHandler : INotificationHandler<INotificationWrapper<PatientDeactivatedDomainEvent>>
    {
        public ILogger<PatientDeactivatedDomainEventHandler> Logger { get; }
        public PatientDeactivatedDomainEventHandler(ILogger<PatientDeactivatedDomainEventHandler> logger)
        {
            Logger = logger;
        }
        public async Task Handle(INotificationWrapper<PatientDeactivatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                Logger.LogInformation("Patient with ID {PatientId} has been deactivated.", notification.DomainEvent.PatientId);
            });
        }
    }
}
