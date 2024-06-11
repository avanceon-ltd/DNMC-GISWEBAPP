using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Asset : System.Web.UI.Page
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


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                SqlDataSource1.SelectCommand = @"SELECT ASSETNUM,DESCRIPTION,HIERARCHYPATH,STATUS,[LATITUDEY],[LONGITUDEX],[GISLONG],[GISLAT]  FROM [ODW].[EAMS].[DimAsset] where STATUS = 'OPERATING' AND LOCATION_KEY = 1271 Order by ASSETNUM";
                SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimAsset] where STATUS = 'OPERATING' AND LOCATION_KEY = 1271";
            }
            else
            {
                SqlDataSource1.SelectCommand = @"SELECT ASSETNUM,DESCRIPTION,HIERARCHYPATH,STATUS,[LATITUDEY],[LONGITUDEX],[GISLONG],[GISLAT] FROM [ODW].[EAMS].[DimAsset] where STATUS = 'OPERATING' AND LOCATION_KEY = 1271 and ASSETNUM LIKE '%" + txtSearch.Text.Trim() + "%'" + " Order by ASSETNUM";
                SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimAsset] where STATUS = 'OPERATING' AND LOCATION_KEY = 1271 and ASSETNUM LIKE '%" + txtSearch.Text.Trim() + "%'";

            }
            gvCustomers.DataBind();
            DataList5.DataBind();



        }
    }
}