using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Location : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //DataTable dt = new DataTable();
                //dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
                //        new DataColumn("Name", typeof(string)),
                //        new DataColumn("Country",typeof(string)) });
                //dt.Rows.Add(1, "John Hammond", "United States");
                //dt.Rows.Add(2, "Mudassar Khan", "India");
                //dt.Rows.Add(3, "Suzanne Mathews", "France");
                //dt.Rows.Add(4, "Robert Schidner", "Russia");
                //gvCustomers.DataSource = SqlDataSource1;
                //gvCustomers.DataBind();
            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void gvCustomers_DataBound(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            SqlDataSource1.SelectCommand = "SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1 AND Location LIKE '" + txtSearch.Text.Trim() + "'";
           




            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {


                SqlDataSource1.SelectCommand = "SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1";
                SqlDataSource2.SelectCommand = "SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1";
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1 AND Location LIKE '%" + txtSearch.Text.Trim() + "%'";
                SqlDataSource2.SelectCommand = "SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1 AND Location LIKE '%" + txtSearch.Text.Trim() + "%'";
            }
            gvCustomers.DataBind();
            DataList5.DataBind();




        }
    }
}