using HIS.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Events
{
    public interface INotificationWrapper<out T> : INotification
    {
        T DomainEvent { get; }
    }

    public class NotificationWrapper<T> : INotificationWrapper<T>
    {
        public NotificationWrapper(T notification)
        {
            DomainEvent = notification;
        }

        public T DomainEvent { get; }
    }
}
