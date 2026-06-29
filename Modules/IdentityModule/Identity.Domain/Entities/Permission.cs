using Identity.Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class Permission : AggregateRoot<Guid>
    {

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
