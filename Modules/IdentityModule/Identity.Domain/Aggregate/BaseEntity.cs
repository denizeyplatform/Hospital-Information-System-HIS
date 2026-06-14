using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Aggregate
{
    public class BaseEntity<TId>
    {
        public TId Id { get; set; } = default!;
        public DateTime CreatedAt { get; protected set; }
       = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; protected set; }

    }
}
