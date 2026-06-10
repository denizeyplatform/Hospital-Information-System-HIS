using Identity.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Events
{
    public record SessionRevokedDomainEvent(DateTime OccurredOnUtc) : IDomainEvent
    {
    }
}
