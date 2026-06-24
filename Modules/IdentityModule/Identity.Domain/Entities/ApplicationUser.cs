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

        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TempPassword { get; set; } = string.Empty;
        public string ConfirmationLink { get; set; }

        public void registerDomainEvent()
        {
            RaiseDomainEvent(new UserRegisteredDomainEvent(DateTime.UtcNow, Id));
        }

        public void loginDomainEvent()
        {
            RaiseDomainEvent(new UserLoggedInDomainEvent(DateTime.UtcNow, Email));
        }

        public static ApplicationUser create(string userId, string email, string firstName, string lastName, string tempPassword, string confirmationLink)
        {
            var user = new ApplicationUser();
            user.Id = userId;
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.TempPassword = tempPassword;
            user.ConfirmationLink = confirmationLink;

            user.RaiseDomainEvent(
                new UserCreatedDomainEvent(Guid.Parse(user.Id),user.Email,user.FirstName,user.LastName,
                user.TempPassword, user.ConfirmationLink,DateTime.UtcNow ));

            return user;
        }
    }
}
