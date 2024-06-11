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
    public partial class Location : System.Web.UI.Page
    {

        string AssetNum = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["AssetNum"]))
            {
                AssetNum = Request.QueryString["AssetNum"];

            }
            if (!this.IsPostBack)
            {
                BindGridView("", "", "", "", 1);
            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLocations.PageIndex = e.NewPageIndex;
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, e.NewPageIndex + 1);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, 1);
        }

        public void BindGridView(string text1, string column1, string text2, string column2, int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvLocations.PageSize = pageSize;
            string searchClause = "", query = "";
            if (!String.IsNullOrEmpty(text1))
                searchClause += " " + column1 + " LIKE '%" + text1 + "%'";
            if (!String.IsNullOrEmpty(text1) && !String.IsNullOrEmpty(text2))
                searchClause += ddlAndOr.SelectedValue;
            if (!String.IsNullOrEmpty(text2))
                searchClause += " " + column2 + " LIKE '%" + text2 + "%'";

            if (!String.IsNullOrEmpty(searchClause))
                searchClause = " AND " + searchClause;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            //query = @"SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE,EXT_FRAMEZONE FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1  " + searchClause;
            if (string.IsNullOrEmpty(AssetNum) || AssetNum.Length == 0)
            {
                query = @"SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE,EXT_FRAMEZONE FROM [ODW].[EAMS].[DimLocation] WHERE isActive = 1  " + searchClause;
            }
            else
            {
                query = @"select l.Location,l.DESCRIPTION,l.EXT_LOCATIONTAG, l.TYPE,l.EXT_FRAMEZONE from [ODW].[EAMS].[vwDimAsset] a join
                      EAMS.DimLocation l on a.LOCATION_KEY = l.LOCATION_KEY WHERE a.STATUS = 'OPERATING' and a.ASSETNUM = '"+AssetNum+"'" + searchClause;
            }
                      
            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows.Count);
                gvLocations.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvLocations.DataSource = dataSet.Tables[0];
                gvLocations.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }


    }
}