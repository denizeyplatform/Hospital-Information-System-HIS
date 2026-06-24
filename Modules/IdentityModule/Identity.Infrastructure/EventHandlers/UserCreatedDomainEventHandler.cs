using Identity.Application.Contracts.Interface;
using Identity.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.EventHandlers
{
    public class UserCreatedDomainEventHandler : INotificationHandler<INotificationWrapper<UserCreatedDomainEvent>>
    {
        private readonly IEmailService _emailService;

        public UserCreatedDomainEventHandler(
            IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(INotificationWrapper<UserCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var body = $@"
                        Welcome {notification.DomainEvent.firstName}

                        Your account has been created.

                        Email:
                        {notification.DomainEvent.email}

                        Temporary Password:
                        {notification.DomainEvent.temporaryPassword}

                        Please confirm your email:

                        {notification.DomainEvent.confirmationLink}
                        ";

            await _emailService.SendAsync(notification.DomainEvent.email, "Welcome to the Platform", body);
        }

        
    }
}
