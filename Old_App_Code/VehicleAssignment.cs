using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppForm.App_Code
{
    public class VehicleAssignment
    {
        public int WONUM { get; set; }
        public int VehicleNumber { get; set; }
        public bool IsAssigned { get; set; }
        public String CreatedBy { get; set; }
        public String ModifiedBy { get; set; }


    }
}