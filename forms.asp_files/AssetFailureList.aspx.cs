using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class AssetFailureList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    DataTable dt = new DataTable();
            //    dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
            //            new DataColumn("Name", typeof(string)),
            //            new DataColumn("Country",typeof(string)) });
            //    dt.Rows.Add(1, "John Hammond", "United States");
            //    dt.Rows.Add(2, "Mudassar Khan", "India");
            //    dt.Rows.Add(3, "Suzanne Mathews", "France");
            //    dt.Rows.Add(4, "Robert Schidner", "Russia");
            //    gvCustomers.DataSource = dt;
            //    gvCustomers.DataBind();
            //}

            String location = Request.QueryString["LocationId"];
            String location2 = location;
        }
        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            DataBind();
        }
    }
}