using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppForm.BusinessEntity
{
    public class BaseAudit
    {
        public string RequestPayLoad { get; set; }

        public string ResponsePayload { get; set; }
        public string XmlRequest { get; set; }
        public string XmlResponse { get; set; }
    }
}