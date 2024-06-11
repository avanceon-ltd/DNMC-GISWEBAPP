using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class WODetailsBySiteLocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["WorkType"]) && !Page.IsPostBack &&
                !String.IsNullOrEmpty(Request.QueryString["FacilityType"]))
            {
                /*
                string facilityType = Request.QueryString["FacilityType"];
                if (facilityType.ToUpper().Equals("SGW-PS"))
                {
                    facilityType = "loc.[Facility_Type] = 'SGW-PS' OR loc.[Facility_Type] = 'SGW-UP-PS'";
                }
                else
                {
                    facilityType = "loc.[Facility_Type] = @Facility_Type";
                }

                switch (Request.QueryString["WorkType"])
                {
                    case "PM":
                        ddlWorkType.SelectedValue = "PM";
                        SqlDataSource1.SelectParameters["WORKTYPE"].DefaultValue = Request.QueryString["WorkType"];
                        SqlDataSource2.SelectParameters["WORKTYPE"].DefaultValue = Request.QueryString["WorkType"];
                        break;
                    case "Äll":
                    default:
                        ddlWorkType.SelectedValue = "All";
                        SqlDataSource1.SelectCommand = "SELECT wo.location,loc.Hierarchical_SiteName, wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,wo.ASSETNUM, wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTEDBY,wo.REPORTDATE,wo.EXT_ENGINEER,wo.CHANGEDATE  " +
                        " from [SCADA].[MXLocations1] loc with(NOLOCK)  INNER JOIN [GIS].[FactWorkOrder] wo WITH(NOLOCK) ON loc.MXLocationId =  wo.LOCATION WHERE (" + facilityType + ") AND loc.MXLocationId<> 'Not Available' ORDER BY wo.CHANGEDATE desc";
                        SqlDataSource2.SelectCommand = "SELECT COUNT(*) AS TotalRows  " +
                        " from [SCADA].[MXLocations1] loc with(NOLOCK)  INNER JOIN [GIS].[FactWorkOrder] wo WITH(NOLOCK) ON loc.MXLocationId =  wo.LOCATION WHERE (" + facilityType + ")  AND loc.MXLocationId<> 'Not Available'";
                        break;
                    
                }
                
                gvWO.DataBind();
                dlPager.DataBind();
                */

                switch (Request.QueryString["WorkType"].ToUpper())
                {
                    case "PM":
                        ddlWorkType.SelectedValue = "PM";
                        break;
                    case "PM,CM":
                    case "CM,PM":
                        ddlWorkType.SelectedValue = "PM,CM";
                        break;
                    case "CM":
                        ddlWorkType.SelectedValue = "CM";
                        break;
                    case "Äll":
                    default:
                        ddlWorkType.SelectedValue = "All";
                        break;
                }
                BindDataToGrid(ddlWorkType.SelectedValue);
            }
        }


        protected void gvWO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWO.PageIndex = e.NewPageIndex;
            BindDataToGrid(ddlWorkType.SelectedValue);
        }

        protected void gvWO_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the HyperLink control.
                HyperLink myHyperLink = (e.Row.FindControl("WOAssetHyp") as HyperLink);
                string _WONum = e.Row.Cells[0].Text;
                if (!string.IsNullOrEmpty(_WONum))
                {
                    if (IsAttachments(_WONum))
                    {
                        myHyperLink.NavigateUrl = "javascript:window.open('GetImgAttachByWo.aspx?id=" + _WONum + "','PopupWindow','width=400,height=200'); ";
                        myHyperLink.Text = "Click Here";
                    }
                }
            }
        }

        private bool IsAttachments(string wONUM)
        {
            bool Attachments = false;

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("SELECT [URLNAME] FROM [ODW].[EAMS].[Attachment] where OWNERID IN (SELECT WORKORDERID  FROM [ODW].EAMS.FactWorkOrder  where WONUM='" + wONUM + "') and OWNERTABLE='WorkOrder'", con);
                SqlDataAdapter sdadpr = new SqlDataAdapter();
                sdadpr.SelectCommand = cmd;
                sdadpr.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Attachments = true;
                }
            }
            return Attachments;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void BindDataToGrid(string workType)
        {
            string whereClause = "";
            string facilityType = Request.QueryString["FacilityType"];
            string framezone = "";

            if (facilityType.ToUpper().Equals("SGW-PS"))
            {
                facilityType = "loc.[Facility_Type] = 'SGW-PS' OR loc.[Facility_Type] = 'SGW-UP-PS'";
            }
            else
            {
                facilityType = "loc.[Facility_Type] = @Facility_Type";
            }

            switch (workType)
            {
                case "All":
                    whereClause += " WHERE (" + facilityType + ") AND loc.MXLocation IS NOT NULL AND wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC')";
                    break;
                case "FR":
                    whereClause += " WHERE (" + facilityType + ") AND wo.WORKTYPE = 'FR' and loc.MXLocation IS NOT NULL AND wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC')";
                    break;
                case "CM":
                    whereClause += " WHERE (" + facilityType + ") AND wo.WORKTYPE = 'CM' and loc.MXLocation IS NOT NULL AND wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC')";
                    break;
                case "PM":
                    whereClause += " WHERE (" + facilityType + ") AND wo.WORKTYPE = 'PM' AND  wo.[Status] NOT IN ('CAN','CLOSE','COMP','WRKCOMP') and loc.MXLocation IS NOT NULL ";
                    break;
                case "PM,CM":
                    whereClause += " WHERE (" + facilityType + ") AND wo.WORKTYPE IN ('PM','CM')  AND  wo.[Status] NOT IN ('CAN','CLOSE','COMP','WRKCOMP') and loc.MXLocation IS NOT NULL ";
                    break;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["Framezone"]))
            {
                framezone = Request.QueryString["Framezone"];
                whereClause += " AND wo.EXT_FRAMEZONE = '" + framezone + "' ";
            }

            if (!String.IsNullOrEmpty(Request.QueryString["Status"]))
            {                
                whereClause += " AND wo.Status = '" + Request.QueryString["Status"] + "' ";
            }

            SqlDataSource1.SelectCommand = @"SELECT wo.location,wo.EXT_FRAMEZONE,loc.Hierarchical_SiteName, wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,wo.ASSETNUM, wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTEDBY,wo.REPORTDATE,wo.EXT_ENGINEER,wo.CHANGEDATE  
            FROM [SCADA].[MXLocations] loc  with (NOLOCK)  INNER JOIN [EAMS].[FactWorkOrder] wo WITH (NOLOCK) ON loc.MXLocation =  wo.LOCATION " + whereClause + " ORDER BY wo.CHANGEDATE desc ";
            SqlDataSource2.SelectCommand = @"SELECT count(*) AS TotalRows  from [SCADA].[MXLocations] loc  with (NOLOCK)  INNER JOIN [EAMS].[FactWorkOrder] wo WITH (NOLOCK) ON loc.MXLocation =  wo.LOCATION " + whereClause ;
            gvWO.DataBind();
            dlPager.DataBind();
        }

        protected void ddlWorkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataToGrid(ddlWorkType.SelectedValue);
        }
    }
}