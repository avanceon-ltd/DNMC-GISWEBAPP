using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class PABoundaryByWO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindGridView(1);
            }
        }

        protected void gvHotspot_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHotspot.PageIndex = e.NewPageIndex;
            BindGridView(e.NewPageIndex + 1);
        }

        public void BindGridView(int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;
            string longitude = "0", latitude = "0";
            if (!String.IsNullOrEmpty(Request.QueryString["long"]) && !String.IsNullOrEmpty(Request.QueryString["lat"]))
            {
                longitude = Request.QueryString["long"].Trim();
                latitude = Request.QueryString["lat"].Trim();
                if (String.IsNullOrEmpty(longitude) || String.IsNullOrEmpty(latitude))
                {
                    longitude = "0";
                    latitude = "0";
                }

            }

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvHotspot.PageSize = pageSize;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GISPRODConnectionString"].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[spFindPABoundaryByPoint]";
                    command.Parameters.Add("@long", SqlDbType.VarChar, 30).Value = longitude;
                    command.Parameters.Add("@lat", SqlDbType.VarChar, 30).Value = latitude;


                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();

                    adapter.SelectCommand = command;
                    if (ds != null)
                        adapter.Fill(ds);

                    int totalRows = Convert.ToInt32(ds.Tables[0].Rows.Count);
                    gvHotspot.VirtualItemCount = totalRows;
                    lblRecordCount.Text = totalRows.ToString();
                    gvHotspot.DataSource = ds.Tables[0];
                    gvHotspot.DataBind();
                }
            }
        }

        //protected void ddlPABoundary_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindGridView(1);
        //}
    }
}