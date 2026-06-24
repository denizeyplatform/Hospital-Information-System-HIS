using Identity.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.DTOs
{
    public class UpdateUserDTO
    {
        
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? UserName { get; set; } 
        public string? NormalizedUserName { get; set; }
        public Gender Gender { get; set; } 
        public string? Email { get; set; } 
        public string? NormalizedEmail { get; set; } 
        public string? PasswordHash { get; set; } 
        public string? PhoneNumber { get; set; } 
        public string? Role { get; set; } 
        
    }
}
