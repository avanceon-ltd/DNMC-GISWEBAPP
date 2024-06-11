using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class HotspotRadius : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindGridView(ddlDistance.SelectedValue, 1);
            }
        }

        protected void gvHotspot_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHotspot.PageIndex = e.NewPageIndex;
            //BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, e.NewPageIndex + 1);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
        }

        public void BindGridView(string distance, int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;
            string serviceRequest = "";
            string wonum = "";
            serviceRequest = Request.QueryString["ServiceRequest"];
            wonum = Request.QueryString["WONUM"];

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvHotspot.PageSize = pageSize;
            string searchClause = "", query = "";
            //if (!String.IsNullOrEmpty(text1))
            //    searchClause += " " + column1 + " LIKE '%" + text1 + "%'";
            //if (!String.IsNullOrEmpty(text1) && !String.IsNullOrEmpty(text2))
            //    searchClause += ddlAndOr.SelectedValue;
            //if (!String.IsNullOrEmpty(text2))
            //    searchClause += " " + column2 + " LIKE '%" + text2 + "%'";

            //if (!String.IsNullOrEmpty(searchClause))
            //    searchClause = " AND " + searchClause;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            if (!String.IsNullOrEmpty(serviceRequest))
            {
                query = @"SELECT refno,[Municipality],[Zone],[District],[StreetNumber],[location],FM19ECCSTATUSValue," +
            "h.Shape.STDistance(geography::STGeomFromText((Select Shape from CRMS.DrainageComplaints Where Service_Request_Number ='" + serviceRequest + "').ToString(), 4326)) AS Distance" +
            " FROM GIS.vwHotSpot h " +
            " WHERE h.Shape.STBuffer(" + Convert.ToInt32(ddlDistance.SelectedValue) + ").STContains(geography::STGeomFromText((Select Shape from CRMS.DrainageComplaints Where Service_Request_Number ='" + serviceRequest + "').ToString(), 4326)) = 1 " +
            " ORDER BY Distance ASC ";
            }
            if (!String.IsNullOrEmpty(wonum))
            {
                query = @"SELECT refno,[Municipality],[Zone],[District],[StreetNumber],[location],FM19ECCSTATUSValue," +
            "h.Shape.STDistance(geography::STGeomFromText((SELECT Shape FROM GIS.FactWorkOrder WHERE WONUM ='" + wonum + "').ToString(), 4326)) AS Distance" +
            " FROM GIS.vwHotSpot h " +
            " WHERE h.Shape.STBuffer(" + Convert.ToInt32(ddlDistance.SelectedValue) + ").STContains(geography::STGeomFromText((SELECT Shape FROM GIS.FactWorkOrder WHERE WONUM ='" + wonum + "').ToString(), 4326)) = 1 " +
            " ORDER BY Distance ASC ";
            }
            cmd.CommandText = query;
            /*
             SELECT 
refno,[Municipality],[Zone],[District],[StreetNumber],[location],FM19ECCSTATUSValue
  FROM [ODW].[SP].[HotSpot]
  WHERE [FM19_PlanRefValue] = '040. Flooding Mitigation Plan - DNOM SGW'
             */

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows.Count);
                gvHotspot.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvHotspot.DataSource = dataSet.Tables[0];
                gvHotspot.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ddlDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(ddlDistance.SelectedValue, 1);
        }
    }
}