using Identity.Domain.Aggregate;
using Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Models
{
    public class User : IdentityUser
    {
        // Additional properties can be added here as needed
        public int Gender { get; set; }
        public DateOnly BOD { get; set; }

        public Email _email { get; set; }
        public HashedPassword hashedPassword { get; set; }


        public static void register()
        {
            // UserRegisteredDomainEvent(datetime.now);
            // raise domain event for user registration
        }

        public static void login()
        {
            // raise domain event for user login
        }
    }
}
