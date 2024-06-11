using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class SCADAEvents : System.Web.UI.Page
    {
        String SearchLoc = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");


            if (!Page.IsPostBack)
            {
                txtTargetStart.Text = DateTime.Now.AddHours(-2).ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(-1).ToString();
                BindGridView(1);
            }
        }
        public void BindGridView(int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["alarmPageSize"]) + 10;

            int startIndex = (pageNo - 1) * pageSize;
            int endIndex = startIndex + pageSize - 1;

            gvCustomers.PageSize = pageSize;
            string searchClause = "", query = "";

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString_Live"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            query = "SELECT  COUNT(*) FROM[ODW].[SCADA].[Events] a WITH(NOLOCK)   WHERE 1 = 1 AND a.EventStamp BETWEEN '" + Convert.ToDateTime(txtTargetStart.Text).ToString("s") + "' AND '" + 
                Convert.ToDateTime(txtTargetFinish.Text).ToString("s") + "' " +
                "SELECT [EventStamp],[Area],[TagName],[Description],[Value]" +
                "FROM[ODW].[SCADA].[Events] a WITH(NOLOCK)" +
                "WHERE 1=1 AND a.EventStamp BETWEEN '" + Convert.ToDateTime(txtTargetStart.Text).ToString("s") + "' AND '" +
                Convert.ToDateTime(txtTargetFinish.Text).ToString("s") + "' " +
                "AND Area != 'WeatherStations' ";

            cmd.CommandText = query;
            cmd.CommandTimeout = 1200;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                conn.Open();
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
                gvCustomers.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvCustomers.DataSource = dataSet.Tables[1];
                gvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGridView(e.NewPageIndex + 1);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(1);
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
        
        private void DownloadDateToDrive()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();
            string query = "";

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString_Live"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            query = "SELECT [TagName],[Date],[Description],[Priority],[Alarm State],[Asset],[Previous State] " +
                    ", CASE WHEN ([Priority] >= 1 AND [Priority] <= 250) THEN 'Critical' " +
                      " WHEN([Priority] > 250 AND [Priority] <= 500) THEN 'High' " +
                      " WHEN([Priority] > 500 AND [Priority] < 950) THEN 'Medium' END AS [Severity] " +
                    " FROM [ODW].[SCADA].[Alarms] a WITH(NOLOCK) " +
                    " WHERE a.Date BETWEEN '" + Convert.ToDateTime(txtTargetStart.Text).ToString("s") + "' AND '" +
                    Convert.ToDateTime(txtTargetFinish.Text).ToString("s") + "' " +
                    " ORDER BY (a.Date) DESC ";

            cmd.CommandText = query;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            sqlDataAdapter.Fill(dataSet);

            string folder = ConfigurationManager.AppSettings["FolderPath"];
            string filename = "SCADA-Alarms-Report_" + DateTime.Now.ToString("yyyy-MM-ddTHH_mm_ss") + ".csv";

            filename = filename.Replace(':', '_');

            string delimiter = ",";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string[] columnNames = dataSet.Tables[0].Columns.Cast<DataColumn>()
                                                .Where(column => column.ColumnName != "Priority")
                                                .Select(column => column.ColumnName == "Previous State" ? "Value" : column.ColumnName)
                                                .ToArray();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, columnNames));

            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                sb.AppendLine(string.Join(delimiter, dr.ItemArray.Cast<object>()
                                                        .Where((value, index) => index != 3)));
            }

            File.WriteAllText(Path.Combine(folder, filename), sb.ToString());

            lblMessage.Text = "File: \"" + Path.Combine(folder, filename) + "\" has been successfully created.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

            conn.Close();
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadDateToDrive();
        }

    }
}