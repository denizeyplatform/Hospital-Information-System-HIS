using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Models
{
    public class RolePermissionsModel : RolePermission
    {
        public string RoleId { get; set; }
        public Role roles { get; set; }
    }
}
