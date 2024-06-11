using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppForm.BusinessEntity
{
    public class AuditWORequest : BaseAudit
    {
        public string UniqueId_WONumber { get; set; }
        public string ExternalRefID { get; set; }
        public string ServiceOperation { get; set; }
    }
}