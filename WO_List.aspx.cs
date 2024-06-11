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
    public partial class WO_List : System.Web.UI.Page
    {

        string query = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
          
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                query = @"SELECT top(1000)  WONUM,DESCRIPTION,STATUS,WORKTYPE,EXT_DEFECTTYPE,EXT_FRAMEZONE,OWNER,WO_PRIORITY,EXT_CONTRACTID,VENDOR,EXT_CREWLEAD,EXT_ENGINEER,EXT_SCHEDULER,ACTSTART,ACTFINISH,REPORTDATE,REPORTEDBY,TARGSTARTDATE,TARGCOMPDATE,ASSETNUM,LOCATION,EXT_CRMSRID,CHANGEBY,CHANGEDATE FROM ODW.EAMS.FactWorkOrder order by CHANGEDATE desc";
                
                }
                else
                {

                query = @"SELECT top(1000) WONUM,DESCRIPTION,STATUS,WORKTYPE,EXT_DEFECTTYPE,EXT_FRAMEZONE,OWNER,WO_PRIORITY,EXT_CONTRACTID,VENDOR,EXT_CREWLEAD,EXT_ENGINEER,EXT_SCHEDULER,ACTSTART,ACTFINISH,REPORTDATE,REPORTEDBY,TARGSTARTDATE,TARGCOMPDATE,ASSETNUM,LOCATION,EXT_CRMSRID,CHANGEBY,CHANGEDATE FROM ODW.EAMS.FactWorkOrder  where " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'" + " order by[CHANGEDATE] desc";

                }

            if (!Page.IsPostBack)
            {
                BindGridView(1);
            }

        }


        public void BindGridView( int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvCustomers.PageSize = pageSize;
          

           
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;           

            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows.Count);
                gvCustomers.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvCustomers.DataSource = dataSet.Tables[0];
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
            BindGridView( e.NewPageIndex + 1);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView( 1);
        }




    }
}