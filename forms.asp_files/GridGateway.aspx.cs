using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class GridGateway : System.Web.UI.Page
    {

        string Id, page_name,user = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["PageName"]))
            {
                 page_name = Request.QueryString["PageName"];

                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                    Id = Convert.ToString(Request.QueryString["ID"]);
                if (!string.IsNullOrEmpty(Request.QueryString["USER"]))
                    user = Convert.ToString(Request.QueryString["USER"]);
                switch (page_name)
                {
                    case "ASST":
                        Response.Redirect("~/AssetDetail.aspx?ID="+ Id);
                        break;
                    case "AVLS":
                        Response.Redirect("~/VehicleDetail.aspx?ID=" + Id + "&User=" + user);
                        break;
                    case "WORD":
                        Response.Redirect("~/WorkOrderDetail.aspx?ID=" + Id + "&User=" + user);
                        break;
                    case "CSSR":
                        Response.Redirect("~/CRMSComplaintDetail.aspx?ID=" + Id);
                        break;
                    case "LOCS":
                        Response.Redirect("~/LocationsDetail.aspx?ID=" + Id);
                        break;
                    case "DWPM":
                        Response.Redirect("~/DewateringPermitDetail.aspx?ID=" + Id);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}