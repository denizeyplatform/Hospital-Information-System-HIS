using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Interface
{
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}
