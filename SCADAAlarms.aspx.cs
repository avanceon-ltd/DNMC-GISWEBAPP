using LCSDMS_Images_Migration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace WebAppForm
{
    public partial class SCADAAlarms : System.Web.UI.Page
    {
        String SearchLoc = string.Empty;
        ILog log = log4net.LogManager.GetLogger(typeof(SCADAAlarms));
        //private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                txtTargetStart.Text = DateTime.Now.AddHours(-2).ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(-1).ToString();
                sdate.Text = txtTargetStart.Text;
                edate.Text = txtTargetFinish.Text;
                BindGridView(1);
            }
        }

        public void BindGridView(int pageNo)
        {
            sdate.Text = txtTargetStart.Text;
            edate.Text = txtTargetFinish.Text;
            gvCustomers.DataSource = null;
            gvCustomers.DataBind();
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["alarmPageSize"]);
            pageSize += 10;
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvCustomers.PageSize = pageSize;
            string searchClause = "", query = "";

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString_Live"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 1200;
            cmd.Parameters.Clear();
            query = @"SELECT COUNT(*) FROM (
                    SELECT *, 
                    CASE WHEN (a.[Priority] >= 1 AND a.[priority] <= 250) THEN 'Critical'
                    WHEN (a.[Priority] > 250 AND a.[priority] <= 500) THEN 'High'
                    WHEN (a.[Priority] > 500 AND a.[priority] < 950) THEN 'Medium' END AS [Severity]
                    FROM [ODW].[SCADA].[vwAlarms] a WITH(NOLOCK)
                    INNER JOIN dbo.WebAppTestView w ON a.Asset = w.Asset_OdW
                    ) b
                    WHERE cast(b.Date as datetime) BETWEEN @StartDate AND @EndDate ";
            if (PriorityList.SelectedValue != "All")
            {
                query += " AND Severity = @selectedSeverity ";
                cmd.Parameters.AddWithValue("@selectedSeverity", PriorityList.SelectedValue);
            }
            if (ddlField1.SelectedValue != "All")
            {
                query += " AND CZF_ODW = @SelectedCatchment ";
                cmd.Parameters.AddWithValue("@SelectedCatchment", ddlField1.SelectedValue);
            }
            if (NetworkTypelist.SelectedValue != "All")
            {
                query += " AND Network_ODW = @SelectedNetwork ";
                cmd.Parameters.AddWithValue("@SelectedNetwork", NetworkTypelist.SelectedValue);
            }
            if (AlarmStatelist.SelectedValue != "All")
            {
                query += " AND [Alarm State] = @selectedAlarm ";
                cmd.Parameters.AddWithValue("@selectedAlarm", AlarmStatelist.SelectedValue);
            }
            if (TagNameList.SelectedValue != "All")
            {
                query += " AND [TagName] = @selectedTag ";
                cmd.Parameters.AddWithValue("@selectedTag", TagNameList.SelectedValue);
            }
            if (AssetTypeList.SelectedValue != "All")
            {
                query += " AND [Asset Group_ODW] = @selectedAssetType ";
                cmd.Parameters.AddWithValue("@selectedAssetType", AssetTypeList.SelectedValue);
            }
            if (AssetList.SelectedValue != "All")
            {
                query += " AND [Asset] = @selectedAsset ";
                cmd.Parameters.AddWithValue("@selectedAsset", AssetList.SelectedValue);
            }
            if (IsSilencedList.SelectedValue == "False")
            {
                query += " AND [IsSilenced?] = 0";
            }
            else if (IsSilencedList.SelectedValue == "True")
            {
                query += " AND [IsSilenced?] = 1";
            }
            if (AttributeList.SelectedValue != "All")
            {
                query += " AND Attribute = @selectedAttribute ";
                cmd.Parameters.AddWithValue("@selectedAttribute", AttributeList.SelectedValue);
            }

            query += @"select * from (
                SELECT [TagName],[Date],[Description],[Priority],[Alarm State],[Asset],[Alarm_DurationMs],[User],[Current State],[Previous State], w.[CZF_ODW], w.[Network_ODW], [IsSilenced?], w.[Asset Group_ODW] 
            , CASE WHEN ([Priority] >= 1 AND [priority] <= 250) THEN 'Critical'
                WHEN ([Priority] > 250 AND [priority] <= 500) THEN 'High'
                WHEN ([Priority] > 500 AND [priority] < 950) THEN 'Medium' END AS [Severity], [Attribute]
            FROM [ODW].[SCADA].[vwAlarms] a WITH(NOLOCK)
            INNER JOIN dbo.WebAppTestView w ON a.Asset = w.Asset_OdW) k where cast(Date as datetime) between @StartDate AND @EndDate ";
            if (PriorityList.SelectedValue != "All")
            {
                query += " AND Severity = @selectedSeverity1 ";
                cmd.Parameters.AddWithValue("@selectedSeverity1", PriorityList.SelectedValue);
            }
            if (AttributeList.SelectedValue != "All")
            {
                query += " AND Attribute = @selectedAttribute1 ";
                cmd.Parameters.AddWithValue("@selectedAttribute1", AttributeList.SelectedValue);
            }
            if (ddlField1.SelectedValue != "All")
            {
                query += " AND CZF_ODW = @SelectedCatchment1 ";
                cmd.Parameters.AddWithValue("@SelectedCatchment1", ddlField1.SelectedValue);
            }
            if (NetworkTypelist.SelectedValue != "All")
            {
                query += " AND Network_ODW = @SelectedNetwork1 ";
                cmd.Parameters.AddWithValue("@SelectedNetwork1", NetworkTypelist.SelectedValue);
            }
            if (AlarmStatelist.SelectedValue != "All")
            {
                query += " AND [Alarm State] = @selectedAlarm1 ";
                cmd.Parameters.AddWithValue("@selectedAlarm1", AlarmStatelist.SelectedValue);
            }
            if (TagNameList.SelectedValue != "All")
            {
                query += " AND [TagName] = @selectedTag1 ";
                cmd.Parameters.AddWithValue("@selectedTag1", TagNameList.SelectedValue);
            }
            if (AssetTypeList.SelectedValue != "All")
            {
                query += " AND [Asset Group_ODW] = @selectedAssetType1 ";
                cmd.Parameters.AddWithValue("@selectedAssetType1", AssetTypeList.SelectedValue);
            }
            if (AssetList.SelectedValue != "All")
            {
                query += " AND [Asset] = @selectedAsset1 ";
                cmd.Parameters.AddWithValue("@selectedAsset1", AssetList.SelectedValue);
            }
            if (IsSilencedList.SelectedValue == "False")
            {
                query += " AND [IsSilenced?] = 0";
            }
            else if (IsSilencedList.SelectedValue == "True")
            {
                query += " AND [IsSilenced?] = 1";
            }
            query += " ORDER BY Date OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(txtTargetStart.Text).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(txtTargetFinish.Text).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@Offset", (pageNo - 1) * pageSize);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[0]);
                gvCustomers.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                FilterData(dataSet);


                gvCustomers.DataSource = dataSet.Tables[1];
                gvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void FilterData(DataSet dataSet)
        {


            //List<Dictionary<string, object>> obj = ToCollection(dataSet);


            // foreach (DataTable table in dataSet.Tables)
            //{
            //    // Use LINQ to check if the value exists in any cell of the DataTable
            //    if (table.AsEnumerable().Any(row => row.ItemArray.Contains(PriorityList.SelectedValue)))
            //    {


            //    }
            //}
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGridView(e.NewPageIndex + 1);
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime startDate = Convert.ToDateTime(txtTargetStart.Text);
            DateTime endDate = Convert.ToDateTime(txtTargetFinish.Text);

            if ((endDate - startDate).TotalDays > 90)
            {
                lblMessage.Text = "Please select a date range within the last 90 days.";
                string script = "showModal('SCADA Alarm Page', false); hideSpinner();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", script, true);
                return;
            }

            BindGridView(1);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideSpinner", "hideSpinner();", true);
        }

        protected void StartEndDiffValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime startDate;
            DateTime endDate;
            bool isStartDate = DateTime.TryParse(txtTargetStart.Text, out startDate);
            bool isEndDate = DateTime.TryParse(txtTargetFinish.Text, out endDate);
            if (isStartDate && isEndDate)
            {
                if (startDate < endDate)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = false;
            }
        }
        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                if (e.Row.Cells[5].Text.Equals("UNACK_RTN"))
                {
                    e.Row.Cells[6].BackColor = Color.LightCyan;
                }
                else
                {
                    switch (e.Row.Cells[6].Text)
                    {
                        case "Critical":
                            e.Row.Cells[6].BackColor = Color.Red;
                            break;
                        case "Medium":
                            e.Row.Cells[6].BackColor = Color.Yellow;
                            break;
                        case "High":
                            e.Row.Cells[6].BackColor = Color.Orange;
                            break;
                        default:
                            e.Row.Cells[6].BackColor = Color.LightCyan;
                            break;
                    }
                }
            }
        }

        public DataSet GetDownloadData()
        {
            string query = "";

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString_Live"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 1200;
            cmd.Parameters.Clear();

            //query += @"select * from (
            //    SELECT [TagName],[Date],[Description],[Priority],[Alarm State],[Asset],[Alarm_DurationMs],[User],[Current State],[Previous State], w.[CZF_ODW], w.[Network_ODW], [IsSilenced?], w.[Asset Group_ODW] 
            //, CASE WHEN ([Priority] >= 1 AND [priority] <= 250) THEN 'Critical'
            //    WHEN ([Priority] > 250 AND [priority] <= 500) THEN 'High'
            //    WHEN ([Priority] > 500 AND [priority] < 950) THEN 'Medium' END AS [Severity], [Attribute]
            //FROM [ODW].[SCADA].[vwAlarms] a WITH(NOLOCK)
            //INNER JOIN dbo.WebAppTestView w ON a.Asset = w.Asset_OdW) k where cast(Date as datetime) between @StartDate AND @EndDate ";
            query = @"SELECT [Date], [Asset], [TagName], [Description], [Previous State], [Alarm State], [Severity]
                    FROM (
                    SELECT a.[Date], a.[Asset], a.[TagName], a.[Description], a.[Previous State], a.[Alarm State], w.[CZF_ODW], w.[Network_ODW], a.[IsSilenced?], w.[Asset Group_ODW],
                         CASE 
                            WHEN (a.[Priority] >= 1 AND a.[Priority] <= 250) THEN 'Critical'
                            WHEN (a.[Priority] > 250 AND a.[Priority] <= 500) THEN 'High'
                            WHEN (a.[Priority] > 500 AND a.[Priority] < 950) THEN 'Medium'
                            END AS [Severity], a.[Attribute]
                            FROM [ODW].[SCADA].[vwAlarms] a WITH (NOLOCK)
                            INNER JOIN dbo.WebAppTestView w ON a.Asset = w.Asset_OdW
                                ) k
                            WHERE CAST([Date] AS DATETIME) BETWEEN @StartDate AND @EndDate";
            if (PriorityList.SelectedValue != "All")
            {
                query += " AND Severity = @selectedSeverity1 ";
                cmd.Parameters.AddWithValue("@selectedSeverity1", PriorityList.SelectedValue);
            }
            if (AttributeList.SelectedValue != "All")
            {
                query += " AND Attribute = @selectedAttribute1 ";
                cmd.Parameters.AddWithValue("@selectedAttribute1", AttributeList.SelectedValue);
            }
            if (ddlField1.SelectedValue != "All")
            {
                query += " AND CZF_ODW = @SelectedCatchment1 ";
                cmd.Parameters.AddWithValue("@SelectedCatchment1", ddlField1.SelectedValue);
            }
            if (NetworkTypelist.SelectedValue != "All")
            {
                query += " AND Network_ODW = @SelectedNetwork1 ";
                cmd.Parameters.AddWithValue("@SelectedNetwork1", NetworkTypelist.SelectedValue);
            }
            if (AlarmStatelist.SelectedValue != "All")
            {
                query += " AND [Alarm State] = @selectedAlarm1 ";
                cmd.Parameters.AddWithValue("@selectedAlarm1", AlarmStatelist.SelectedValue);
            }
            if (TagNameList.SelectedValue != "All")
            {
                query += " AND [TagName] = @selectedTag1 ";
                cmd.Parameters.AddWithValue("@selectedTag1", TagNameList.SelectedValue);
            }
            if (AssetTypeList.SelectedValue != "All")
            {
                query += " AND [Asset Group_ODW] = @selectedAssetType1 ";
                cmd.Parameters.AddWithValue("@selectedAssetType1", AssetTypeList.SelectedValue);
            }
            if (AssetList.SelectedValue != "All")
            {
                query += " AND [Asset] = @selectedAsset1 ";
                cmd.Parameters.AddWithValue("@selectedAsset1", AssetList.SelectedValue);
            }
            if (IsSilencedList.SelectedValue == "False")
            {
                query += " AND [IsSilenced?] = 0";
            }
            else if (IsSilencedList.SelectedValue == "True")
            {
                query += " AND [IsSilenced?] = 1";
            }
            query += " ORDER BY Date DESC";
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(txtTargetStart.Text).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(txtTargetFinish.Text).ToString("yyyy-MM-dd HH:mm:ss.fff"));

            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(dataSet);

            return dataSet;
        }

        private void DownloadDataToDrive()
        {
            string sharedFolderPath = ConfigurationManager.AppSettings["SharedFolderPath"];
            string username = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];
            string filename = "";
            try
            {
                DataSet dataSet = GetDownloadData();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    DateTime dateValue = Convert.ToDateTime(row[0]);
                    row[0] = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dataSet.Tables[0].Columns["Previous State"].ColumnName = "Value";
                //Network Authentication //
                using (new NetworkConnection(sharedFolderPath, new NetworkCredential(username, password)))
                {
                    filename = "SCADA-Alarms-Report_" + DateTime.Now.ToString("yyyy-MM-ddTHH_mm_ss") + ".csv";
                    filename = filename.Replace(':', '_');

                    string localFilePath = Path.Combine(sharedFolderPath, filename);

                    using (StreamWriter writer = new StreamWriter(localFilePath))
                    {
                        foreach (DataTable table in dataSet.Tables)
                        {
                            writer.WriteLine(string.Join(",", table.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));

                            foreach (DataRow row in table.Rows)
                            {
                                writer.WriteLine(string.Join(",", row.ItemArray));
                            }
                        }
                    }
                }
                lblMessage.Text = "File downloaded successfully on " + sharedFolderPath;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal('" + filename + "');", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Error("An error occurred during file copy operation.", ex);
                lblMessage.Text = "An error occurred during file copy operation: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }


        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DateTime startDate = Convert.ToDateTime(txtTargetStart.Text);
            DateTime endDate = Convert.ToDateTime(txtTargetFinish.Text);

            if ((endDate - startDate).TotalDays > 90)
            {
                lblMessage.Text = "Please select a date range within the last 90 days.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal(); hideSpinner();", true);
                return;
            }

            DownloadDataToDrive();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideSpinner", "hideSpinner();", true);
        }

        protected void ddlField1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(1);
        }

        protected void NetworkTypelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(1);
        }


    }
}