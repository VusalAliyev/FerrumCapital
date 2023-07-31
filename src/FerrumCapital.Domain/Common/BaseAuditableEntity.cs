using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Domain.Common
{
    public class BaseAuditableEntity:BaseEntity
    {
            public DateTime CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
    }
}
