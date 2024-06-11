using LCSDMS_Images_Migration;
using System;
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
    public partial class SCADALiveDataSummary : System.Web.UI.Page
    {
        String SearchLoc = string.Empty;
        ILog log = log4net.LogManager.GetLogger(typeof(SCADAAlarms));
        string sitename = "", siteId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                sitename = Request.QueryString["hSiteName"];
                FilleSiteID();
                FillDetails();
                FillStatus();
                FillWetWellLevel();
            }



        }

        private void FillStatus()
        {
            #region "Fill PS Data"
            string query = @"select MXLocation,TagName,DATETIME,Value,Hierarchical_SiteName from SCADA.LiveTagsData where Hierarchical_SiteName = '" + sitename + "' AND  TagName like '%_P0%.RunS'";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                dlStatus.DataSource = dt;
                dlStatus.DataBind();
            }
            #endregion
        }

        private void FillDetails()
        {
            #region "Fill PS Data"
            string StatusQuery = "SELECT STATUS FROM EAMS.DimLocation WHERE LOCATION = '" + siteId + "'";
            string WOCountQuery = "SELECT COUNT(*)  AS WOCCOUNT from EAMS.FactWorkOrder where LOCATION = '" + siteId + "' and STATUS not in ('CLOSE', 'CAN', 'WRKCOMP')";
            String CommQuery = "select TOP 1 VALUE FROM SCADA.LiveTagsData where Hierarchical_SiteName = '" + sitename + "' and TagName LIKE '%COMMFAILS%'";

            lblTitle.Text = sitename;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(StatusQuery, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lblStatus.Text = reader["STATUS"].ToString();
                        }
                    }
                }

                cmd.CommandText = WOCountQuery;
                using (SqlDataReader reader1 = cmd.ExecuteReader())
                {

                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            lblOpenWOs.Text = reader1["WOCCOUNT"].ToString();
                        }
                    }
                }

                cmd.CommandText = CommQuery;
                using (SqlDataReader reader2 = cmd.ExecuteReader())
                {

                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            string commValue = reader2["VALUE"].ToString();
                            if (!String.IsNullOrEmpty(commValue))
                            {
                                if (commValue == "1")
                                {
                                    lblCommFail.Text = "ALARM";
                                }
                                else if (commValue == "0")
                                {
                                    lblCommFail.Text = "NORMAL";
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        protected void dlStatus_ItemDataBound(object sender, DataListItemEventArgs e)
        {

        }

        protected string GetImageUrl(object val)
        {
            switch (val.ToString())
            {
                case "0":
                    return "~/Images/cpump-off.png";

                case "1":
                    return "~/Images/cpump-on.jpeg";

                default:
                    return "~/Images/cpump-off.png";

            }

        }

        protected string GetPumpName(object val)
        {
            string assetname = "";
            if (!string.IsNullOrEmpty(val.ToString()))
            {
                assetname = val.ToString();
                string[] list = assetname.Split('_');
                assetname = list[list.Length - 1].Replace(".RunS", "");
            }
            return assetname;
        }

        private void FilleSiteID()
        {
            string siteIdQuery = "select top 1 MXLocation from SCADA.LiveTagsData where Hierarchical_SiteName = '" + sitename + "'";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(siteIdQuery, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            siteId = reader["MXLocation"].ToString();
                        }
                    }
                }
            }
        }

        private void FillWetWellLevel()
        {
            string siteIdQuery = "select MXLocation,TagName,DATETIME,Value,Hierarchical_SiteName from SCADA.LiveTagsData where  TagName like '%LT0%.PV%' AND Hierarchical_SiteName = '" + sitename + "'";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(siteIdQuery, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string tagName = reader["TagName"].ToString();
                            string value = reader["Value"].ToString();
                            if (tagName.Contains("LT01.PV1"))
                            {
                                lblWWLevel1PV1.Text = value;
                            }
                            else if (tagName.Contains("LT01.PV2"))
                            {
                                lblWWLevel1PV2.Text = value;
                            }
                            else if (tagName.Contains("LT02.PV1"))
                            {
                                lblWWLevel2PV1.Text = value;
                            }
                            else if (tagName.Contains("LT02.PV2"))
                            {
                                lblWWLevel2PV2.Text = value;
                            }
                        }
                    }
                }
            }
        }
    }
}