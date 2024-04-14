using BoxCommerce.Orders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Orders.Domain.Entities
{
    public class AuditTrail
    {
        public Guid AuditId { get; set; }
        public AuditType AuditType { get; set; }
        public string AffectedEntity { get; set; }
        public Guid AffectedEntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTimeOffset AuditDate { get; set; }
        public string CreatedBy { get; set; }
    }

}
