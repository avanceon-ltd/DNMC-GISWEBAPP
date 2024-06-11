using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppForm.App_Code
{
    public class WorkOrder
    {
        public String WONUM { get; set; }
        public String Description { get; set; }
        public String Location { get; set; }
        public String Asset { get; set; }
        public String WorkType { get; set; }
        public String InternalPriority { get; set; }
        public String ReportedDate { get; set; }
        public String Longitude { get; set; }
        public String Latitude { get; set; }
        public String TargetStart { get; set; }
        public String TargetFinish { get; set; }
        public String ScheduleStart { get; set; }
        public String ScheduleFinish { get; set; }
        public String ECCOwnership { get; set; }
        public String ECCPriority { get; set; }
        public String Engineer { get; set; }
        public String Section { get; set; }
        public String Unit { get; set; }
        public String Area { get; set; }
        public String ContractId { get; set; }
        public String Vendor { get; set; }
        public String OnBehalfOf { get; set; }
        public String CrewLead { get; set; }
        public String Inspector { get; set; }
        public String Scheduler { get; set; }
        public String Zone { get; set; }
        public String Street { get; set; }
        public String Municipality { get; set; }
        public String BuildingNumber { get; set; }
        public String District { get; set; }
        public String Pin { get; set; }
        public String SADescription { get; set; }
        public String EAMSSRNumber { get; set; }
        public String TICKETID { get; set; }
        public String CRMSID { get; set; }
    }



    //public class UpdateWO
    //{
    //    public String WONUM { get; set; }
    //    public String ECCOwnership { get; set; }
    //    public String ECCPriority { get; set; }
    //    public String ScheduleStart { get; set; }
    //    public String ScheduleFinish { get; set; }
    //    public String ContractId { get; set; }
    //    public String CrewLead { get; set; }
    //    public String Scheduler { get; set; }
    //}
}