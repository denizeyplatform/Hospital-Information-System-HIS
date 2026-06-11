using Identity.Domain.Aggregate;
using Identity.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class ApplicationUser : AggregateRoot<string>
    {

        public string Email { get; set; }
        
        public void registerDomainEvent()
        {
            RaiseDomainEvent(new UserRegisteredDomainEvent(DateTime.UtcNow, Id));
        }

        public void loginDomainEvent()
        {
            RaiseDomainEvent(new UserLoggedInDomainEvent(DateTime.UtcNow, Email));
        }
    }
}
