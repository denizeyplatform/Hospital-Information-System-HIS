using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.DTOs
{
    public class RoleClaimRequest
    {
       
        public string roleName { get; set; }
        public string ClaimType { get; set; } = string.Empty;

        public string ClaimValue { get; set; } = string.Empty;
    }
}
