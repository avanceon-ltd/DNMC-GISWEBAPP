using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppForm.BusinessEntity
{
    public class AuditServiceRequest : BaseAudit
    {
        public string ExternalCRMsrId { get; set; }
        public string IncidentUID { get; set; }
        public string ServiceOperation { get; set; }
    }
}