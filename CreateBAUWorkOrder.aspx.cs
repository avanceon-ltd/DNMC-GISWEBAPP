using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WebAppForm.App_Code;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class CreateBAUWorkOrder : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(CreateBAUFRWO));
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");
            txtReportedBy.Attributes.Add("readonly", "readonly");
            //  txtLocation.Attributes.Add("readonly", "readonly");
            //    drpdwnWorkType.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                txtWOReportedDate.Text = DateTime.Now.ToString();
                txtTargetStart.Text = DateTime.Now.ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(4).ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["Description"]))
                {
                    txtWODes.Text = Convert.ToString(Request.QueryString["Description"]); ;
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Location"]))
                {
                    // Query string value is there so now use it
                    txtLocation.Text = Convert.ToString(Request.QueryString["Location"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Asset"]))
                {
                    txtAsset.Text = Convert.ToString(Request.QueryString["Asset"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Lat"]))
                {
                    txtLat.Text = Convert.ToString(Request.QueryString["Lat"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Long"]))
                {
                    txtlong.Text = Convert.ToString(Request.QueryString["Long"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Engineer"]))
                {
                    txtEngineer.Text = Convert.ToString(Request.QueryString["Engineer"]); ;
                }


                //if (!String.IsNullOrEmpty(Request.QueryString["Scheduler"]))
                //{
                //    txtScheduler.Text = Convert.ToString(Request.QueryString["Scheduler"]); ;
                //}



                if (!String.IsNullOrEmpty(Request.QueryString["ReportedBy"]))
                {
                    txtReportedBy.Text = Convert.ToString(Request.QueryString["ReportedBy"]); ;
                }
                else
                {
                    txtReportedBy.Text = WebConfigurationManager.AppSettings["ReportedBy"];
                }


                if (!String.IsNullOrEmpty(Request.QueryString["SCADAAssetID"]))
                {
                    txtAsset.Text = Convert.ToString(Request.QueryString["SCADAAssetID"]);
                    Fill_Loc_LongLat_bay_AssetID(txtAsset.Text.Trim());
                }
            }
        }




        private void Fill_Loc_LongLat_bay_AssetID(string v)
        {
            GetData(v);
        }

        public void GetData(string searchTerm)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataTable DTable = new DataTable();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[GetAssets_Pager]";

            cmd.Parameters.AddWithValue("@PageIndex", 1);
            cmd.Parameters.AddWithValue("@pageSize", 5);
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
            cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(DTable);


                foreach (DataRow dr in DTable.Rows)
                {

                    if (dr["LOCATION"] != null && !dr["LOCATION"].ToString().Contains("&nbsp;"))
                        txtLocation.Text = dr["LOCATION"].ToString().Trim();

                    if (!string.IsNullOrEmpty(dr["LATITUDEY"].ToString()))
                        txtLat.Text = dr["LATITUDEY"].ToString().Trim();

                    if (!string.IsNullOrEmpty(dr["LONGITUDEX"].ToString()))
                        txtlong.Text = dr["LONGITUDEX"].ToString().Trim();
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                WorkOrder objWorkOrder = new WorkOrder();
                objWorkOrder.Description = txtWODes.Text.Trim();
                objWorkOrder.Location = txtLocation.Text.Trim();
                objWorkOrder.Asset = txtAsset.Text.Trim();
                objWorkOrder.ReportedDate = Convert.ToDateTime(txtWOReportedDate.Text).ToString("s");
                decimal longitude = Math.Round(Convert.ToDecimal(txtlong.Text.Trim()), 10);
                decimal latitude = Math.Round(Convert.ToDecimal(txtLat.Text.Trim()), 10);

                Decimal latStart = Convert.ToDecimal(ConfigurationManager.AppSettings["latStart"]);
                Decimal latEnd = Convert.ToDecimal(ConfigurationManager.AppSettings["latEnd"]);
                Decimal longStart = Convert.ToDecimal(ConfigurationManager.AppSettings["longStart"]);
                Decimal longEnd = Convert.ToDecimal(ConfigurationManager.AppSettings["longEnd"]);

                if (longitude < longStart || longitude > longEnd)
                {
                    lblMessage.Text = "Longitude must be between " + longStart.ToString() + " to " + longEnd.ToString() + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    return;
                }
                if (latitude < latStart || latitude > latStart)
                {
                    lblMessage.Text = "Latitude must be between " + latStart.ToString() + " to " + latEnd.ToString() + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    return;
                }
                objWorkOrder.Longitude = txtlong.Text.Trim();
                objWorkOrder.Latitude = txtLat.Text.Trim();
                objWorkOrder.TargetStart = Convert.ToDateTime(txtTargetStart.Text).ToString("s");
                objWorkOrder.TargetFinish = Convert.ToDateTime(txtTargetFinish.Text).ToString("s");
                objWorkOrder.Engineer = txtEngineer.Text.Trim();
                //  objWorkOrder.Scheduler = txtScheduler.Text.Trim();

                CreateWO(objWorkOrder);
            }
        }

        void CreateWO(WorkOrder pWorkOrder)
        {

            string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BAUCreateWOUri"];
            string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);

            try
            {
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();
                pWorkOrder.Location = txtLocation.Text.Trim();
                pWorkOrder.ReportedBy = txtReportedBy.Text;
                pWorkOrder.ReportedDate = DateTime.Now.ToString("s");
                pWorkOrder.ECCPriority = DrpDwnPriority.SelectedValue.Trim();

                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    result = esbServices.CreateCMWorkOrder(pWorkOrder);
                }
                else
                {
                    result = redHatervices.CreateCMWorkOrder(pWorkOrder);
                }
                lblMessage.Text = result.Item2;
                ClearControls();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            catch (WebException wex)
            {
                if (wex.Message.Contains("Unable to connect"))
                {
                    lblMessage.Text = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                }
                else
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resp);
                    XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                    lblMessage.Text = "<b> CM WO creation error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                    ClearControls();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);               
            }
            catch (Exception exp)
            {
                lblMessage.Text = exp.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

        }
        private void ClearControls()
        {
            txtWODes.Text = "";
            txtLocation.Text = "QATAR";
            txtAsset.Text = "";
            txtTargetStart.Text = "";
            txtTargetFinish.Text = "";
            txtlong.Text = "";
            txtLat.Text = "";
            //   txtScheduler.Text = "";
            txtEngineer.Text = "";

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //    Response.Redirect("~/CreateWorkOrder.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModal();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WorkOrderList.aspx");

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












    }
}