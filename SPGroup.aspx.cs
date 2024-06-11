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
    public partial class SPGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindGridView("", "", 1);
            }
        }

        protected void gvGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGroups.PageIndex = e.NewPageIndex;
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, e.NewPageIndex + 1);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, 1);
        }

        public void BindGridView(string text1, string column1, int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvGroups.PageSize = pageSize;
            string searchClause = "", query = "";
            if (!String.IsNullOrEmpty(text1))
                searchClause += " " + column1 + " LIKE '%" + text1 + "%'";

            if (!String.IsNullOrEmpty(searchClause))
                searchClause = " WHERE " + searchClause;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            query = @"Select Distinct PERSONGROUP,DESCRIPTION from  [EAMS].[DimPersonGroup] " + searchClause;
            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows.Count);
                gvGroups.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvGroups.DataSource = dataSet.Tables[0];
                gvGroups.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }


        //private void BindGrid()
        //{
        //    if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
        //    {
        //        SqlDataSource1.SelectCommand = @"Select Distinct PERSONGROUP,DESCRIPTION from  [EAMS].[DimPersonGroup]";
        //        SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows from (Select Distinct PERSONGROUP,DESCRIPTION from  [EAMS].[DimPersonGroup] ) as p";
        //    }
        //    else
        //    {
        //        SqlDataSource1.SelectCommand = @"Select Distinct PERSONGROUP,DESCRIPTION from  [EAMS].[DimPersonGroup] where DESCRIPTION  Like '%" + txtSearch.Text.Trim() + "%'";
        //        SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows from (Select Distinct PERSONGROUP,DESCRIPTION from  [EAMS].[DimPersonGroup] where DESCRIPTION Like '%" + txtSearch.Text.Trim() + "%' ) as p";

        //    }
        //}

    }
}