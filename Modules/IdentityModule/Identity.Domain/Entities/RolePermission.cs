using Identity.Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class RolePermission : AggregateRoot<string>
    {

        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
