using Identity.Domain.Aggregate;
using Identity.Domain.Enums;
using Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Models
{
    public class User : IdentityUser
    {
        // Additional properties can be added here as needed
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public Gender? Gender { get; set; }
        public DateOnly? BOD { get; set; }

        //[NotMapped]
        //public Email UserEmail { get; set; } = new Email("");
        
        //[NotMapped]
        //public HashedPassword HashedPassword { get; set; } = new HashedPassword("");
        public string? token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }


       
    }
}
