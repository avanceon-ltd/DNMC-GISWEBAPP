using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Asset : System.Web.UI.Page
    {

        String SearchLoc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["LocationId"]))
            {
                SearchLoc = Request.QueryString["LocationId"];

            }
            if (!string.IsNullOrEmpty(Request.QueryString["IsFrameWorkZone"]))
            {
                hdnFrameZone.Value = Request.QueryString["IsFrameWorkZone"];
            }
            if (!Page.IsPostBack)
            {
                BindGridView("", "", "", "", 1);
            }
        }

        public void BindGridView(string text1, string column1, string text2, string column2, int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvCustomers.PageSize = pageSize;
            string searchClause = "", query = "";

            if (!string.IsNullOrEmpty(SearchLoc))
            {
                searchClause = " location='" + SearchLoc + "' ";
            }

            if (!String.IsNullOrEmpty(text1) && String.IsNullOrEmpty(SearchLoc))
            {
                searchClause += " " + column1 + " LIKE '%" + text1 + "%'";
            }
            if (!String.IsNullOrEmpty(text1))
            {
                searchClause += " AND " + column1 + " LIKE '%" + text1 + "%'";
            }

            if (!String.IsNullOrEmpty(text1) && !String.IsNullOrEmpty(text2))
            {
                searchClause += ddlAndOr.SelectedValue;
            }

            if (!String.IsNullOrEmpty(text2))
            {
                searchClause += " " + column2 + " LIKE '%" + text2 + "%'";
            }

            if (!String.IsNullOrEmpty(searchClause))
            {
                searchClause = " AND " + searchClause;
            }

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            query = @"SELECT COUNT(*) AS TotalRows FROM[ODW].[EAMS].[vwDimAsset] a left join EAMS.DimLocation l on a.LOCATION_KEY = l.LOCATION_KEY WHERE a.STATUS = 'OPERATING'  " + searchClause +
                              " SELECT a.ASSET_KEY ,a.ASSETNUM,a.CHANGEBY,a.[DESCRIPTION],a.[HIERARCHYPATH],a.[STATUS],a.[LATITUDEY],a.[LONGITUDEX],a.[GISLONG] " +
                              " ,a.[GISLAT],'Qatar' as Location,a.Ext_FrameZone FROM [ODW].[EAMS].[vwDimAsset] a left join EAMS.DimLocation l on a.LOCATION_KEY = l.LOCATION_KEY " +
                                " WHERE a.STATUS = 'OPERATING' " + searchClause +
                                " ORDER BY (a.ASSET_KEY) OFFSET " + (pageNo - 1) * pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";

            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[0]);
                gvCustomers.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvCustomers.DataSource = dataSet.Tables[1];
                gvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, e.NewPageIndex + 1);
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, 1);
        }


    }
}