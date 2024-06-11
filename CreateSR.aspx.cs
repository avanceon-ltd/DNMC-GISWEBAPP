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
using WebAppForm.BusinessEntity;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class CreateSR : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(CreateSR));
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");
            txtReportedBy.Attributes.Add("readonly", "readonly");
            TxtboxSRGrp.Attributes.Add("readonly", "readonly");
            txtFrameZone.Attributes.Add("readonly", "readonly");
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

                    if (!string.IsNullOrEmpty(dr["GISLAT"].ToString()))
                        txtLat.Text = dr["GISLAT"].ToString().Trim();

                    if (!string.IsNullOrEmpty(dr["GISLONG"].ToString()))
                        txtlong.Text = dr["GISLONG"].ToString().Trim();

                    if (!string.IsNullOrEmpty(dr["EXT_FRAMEZONE"].ToString()))
                        txtFrameZone.Text = dr["EXT_FRAMEZONE"].ToString().Trim();

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
                ServiceRequest objSR = new ServiceRequest();
                objSR.Description = txtWODes.Text.Trim();
                objSR.Summary = txtSummary.Text;
                objSR.Location = txtLocation.Text.Trim();
                objSR.Asset = txtAsset.Text.Trim();
                objSR.ReportedDate = Convert.ToDateTime(txtWOReportedDate.Text).ToString("s");
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
                objSR.Longitude = txtlong.Text.Trim();
                objSR.Latitude = txtLat.Text.Trim();
                objSR.TargetStart = Convert.ToDateTime(txtTargetStart.Text).ToString("s");
                objSR.TargetFinish = Convert.ToDateTime(txtTargetFinish.Text).ToString("s");
                objSR.ExternalFrameZone = txtFrameZone.Text.Trim();


                CreateServiceRequest(objSR);
            }
        }

        void CreateServiceRequest(ServiceRequest pServiceRequest)
        {

            string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BAUCreateSRUri"];
            string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);

            try
            {
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();
                pServiceRequest.ExternalSRGroup = TxtboxSRGrp.Text.Trim();
                pServiceRequest.ExternalSRType = DrpDwnLstSrType.SelectedValue;
                pServiceRequest.ReportedBy = txtReportedBy.Text;
                pServiceRequest.InternalPriority = DrpDwnIntrnalPriority.SelectedValue;
                pServiceRequest.IssueLocation = txtLocation.Text.Trim();
                pServiceRequest.PriorityOfIssueReported = drpDwnReprtedPriority.SelectedValue;

                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    result = esbServices.CreateServiceRequest(pServiceRequest);
                }
                else
                {
                    result = redHatervices.CreateServiceRequest(pServiceRequest);
                }
                lblMessage.Text = result.Item2;
                ClearControls();
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
            txtLocation.Text = "";
            txtAsset.Text = "";
            txtTargetStart.Text = "";
            txtTargetFinish.Text = "";
            txtlong.Text = "";
            txtLat.Text = "";
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