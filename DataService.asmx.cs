using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;

namespace WebAppForm
{
    /// <summary>
    /// Summary description for CountryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        [WebMethod]
        public List<string> GetOwnerDefaultNames(string term)
        {
            List<string> listName = new List<string>();
            if (term.Contains("F"))
            {
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler"]);
            }
            return listName;

        }


        [WebMethod]
        public List<string> GetCrewDefaultNames(string term)
        {
            List<string> listName = new List<string>();
            if (term.StartsWith("C"))
            {
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew1"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew2"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew3"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew4"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew5"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew6"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew7"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew8"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew9"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew10"]);

            }
            
            if (term.Contains("FMP-W"))
            {
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler1"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler2"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler3"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler4"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler5"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler6"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler7"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler8"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler9"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler10"]);
            }

            if (term.Contains("FMP-N"))
            {
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler1"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler2"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler3"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler4"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler5"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler6"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler7"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler8"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler9"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler10"]);
            }

            if (term.Contains("FMP-S"))
            {
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler1"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler2"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler3"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler4"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler5"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler6"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler7"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler8"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler9"]);
                listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler10"]);
            }


            if (term.Contains("Workshop"))
            {
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler1"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler2"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler3"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler4"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler5"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler6"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler7"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler8"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler9"]);
                listName.Add(WebConfigurationManager.AppSettings["Workshop-Scheduler10"]);
            }

            return listName;

        }
    }
}
