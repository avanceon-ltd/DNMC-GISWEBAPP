using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class WorkLogs : System.Web.UI.Page
    {
        string WOSRId = string.Empty;
        bool IsSR = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["IsSR"]) && Request.QueryString["IsSR"] == "1")
                {
                    IsSR = true;
                }
                lblHeading.Text = IsSR ? "Service Request - Work Logs" : "Work Order - Work Logs";
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    WOSRId = Request.QueryString["ID"];
                    BindGridView(1);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["backlink"]))
                {
                    lnkbtnback.Visible = true;
                    if (IsSR == true)
                    {
                        lnkbtnback.PostBackUrl = "EAMS_SRList_ByAsset.aspx?ID=" + Request.QueryString["AssetId"];// "&CRMSID=" + Request.QueryString["CRMSID"] + "&USER=" + Request.QueryString["USER"];
                    }
                    else
                    {
                        lnkbtnback.PostBackUrl = "WorkOrderDetail.aspx?ID=" + WOSRId.Trim() + "&CRMSID=" + Request.QueryString["CRMSID"] + "&USER=" + Request.QueryString["USER"];
                    }
                }
            }
        }
        public void BindGridView(int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]); //Replace by Log Page Size
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvCustomers.PageSize = pageSize;
            string query = "";

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            query = @"SELECT ROW_NUMBER() OVER (ORDER BY RECORDKEY ASC) AS RowNumber, RECORDKEY
                              ,CLASS,CREATEBY,CREATEDATE,MODIFYBY,MODIFYDATE,LOGTYPE,DESCRIPTION,'' as Viewable
	                          INTO #Results from eams.WorkLog where RECORDKEY = '" + WOSRId + "' and CLASS = 'WORKORDER'" +
                              @" SELECT COUNT(*) AS TotalRows  FROM #Results SELECT * FROM #Results
                              WHERE RowNumber BETWEEN " + startIndex + " AND " + endIndex +
                              " DROP TABLE #Results ";
            if (IsSR)
            {
                query = @"SELECT ROW_NUMBER() OVER (ORDER BY RECORDKEY ASC) AS RowNumber, RECORDKEY
                              ,CLASS,CREATEBY,CREATEDATE,MODIFYBY,MODIFYDATE,LOGTYPE,DESCRIPTION,'' as Viewable
	                          INTO #Results from eams.WorkLog where RECORDKEY = '" + WOSRId + "' and CLASS = 'SR'" +
                              @" SELECT COUNT(*) AS TotalRows  FROM #Results SELECT * FROM #Results
                              WHERE RowNumber BETWEEN " + startIndex + " AND " + endIndex +
                              " DROP TABLE #Results ";
            }
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
            if (!string.IsNullOrEmpty(Request.QueryString["IsSR"]))
            {
                IsSR = true;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                WOSRId = Request.QueryString["ID"];
            }
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGridView(e.NewPageIndex + 1);
        }
    }
}