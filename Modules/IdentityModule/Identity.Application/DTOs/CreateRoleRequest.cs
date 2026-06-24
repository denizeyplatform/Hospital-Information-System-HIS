using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.DTOs
{
    public class CreateRoleRequest
    {
        public string? RoleName { get; set; } 

        public string? Description { get; set; } 
    }
}
