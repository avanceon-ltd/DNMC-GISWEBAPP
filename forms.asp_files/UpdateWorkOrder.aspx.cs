using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppForm.App_Code;

namespace WebAppForm
{
    public partial class UpdateWorkOrder : System.Web.UI.Page
    {
        String _WONUM = String.Empty;
        static WorkOrder objWorkOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.DataBind();

            if (!Page.IsPostBack)
            {
                //txtScheduleStart.Text = DateTime.Now.ToString();
                //txtScheduleFinish.Text = DateTime.Now.AddHours(4).ToString();
                if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
                {
                    _WONUM = Convert.ToString(Request.QueryString["WONUM"]);
                    lblWONUM.Text = _WONUM;
                    FillWODetail();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //WorkOrder objWorkOrder = new WorkOrder();

                objWorkOrder.WONUM = lblWONUM.Text;
                objWorkOrder.ContractId = txtContractId.Text;
                //objWorkOrder.CrewLead = txtCrew.Text; ;
                //objWorkOrder.ScheduleStart = Convert.ToDateTime(txtScheduleStart.Text).ToString("s");
                //objWorkOrder.ScheduleFinish = Convert.ToDateTime(txtScheduleFinish.Text).ToString("s");
                objWorkOrder.Scheduler = txtScheduler.Text;

                UpdateWO(objWorkOrder);
            }
        }

        void UpdateWO(WorkOrder pWorkOrder)
        {
            try
            {
                string updateUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCUpdateWOUri"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(updateUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["FAUTH"] = authValue;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    dynamic jsonReqeust = new JObject();
                    jsonReqeust.UpdateWorkOrderRequest = new JObject();
                    jsonReqeust.UpdateWorkOrderRequest.Header = new JObject();
                    jsonReqeust.UpdateWorkOrderRequest.Header.referenceNum = "a";
                    jsonReqeust.UpdateWorkOrderRequest.Header.clientChannel = "DSS";
                    jsonReqeust.UpdateWorkOrderRequest.Header.requestTime = DateTime.Now.ToString("s");
                    jsonReqeust.UpdateWorkOrderRequest.Header.retryFlag = "N";
                    jsonReqeust.UpdateWorkOrderRequest.Header.sucessfulReversalFlag = "N";

                    jsonReqeust.UpdateWorkOrderRequest.Body = new JObject();
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrderNumber = pWorkOrder.WONUM;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder = new JObject();
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalContractID = pWorkOrder.ContractId;
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCrewLead = pWorkOrder.CrewLead;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledFinish = pWorkOrder.ScheduleFinish;
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledStart = pWorkOrder.ScheduleStart;
                    if (!String.IsNullOrEmpty(objWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = "APPR";
                    if (!String.IsNullOrEmpty(objWorkOrder.EAMSSRNumber))
                    {
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                    }
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                    if (!String.IsNullOrEmpty(objWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.CRMSID;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCSRGroup = "DR_CSRL2";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";

                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec = new JArray() as dynamic;
                    dynamic wospec = new JObject();
                    wospec.ALNValue = ddlOwnership.SelectedValue;// "CUSTOMER-SERVICE";
                    wospec.ClassStructureId = "20089";
                    wospec.AssetAttrId = "THRDP";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = ddlPriority.SelectedValue;// "VIP";
                    wospec.ClassStructureId = "20089";
                    wospec.AssetAttrId = "PROPERTY_TYPE";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    streamWriter.Write(jsonReqeust.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    lblMessage.Text = "Work Order updated successfully.";// + jsonResult.SelectToken("UpdateWorkOrderReply.Body.Success").ToString() + "</b>";
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }
            }


            catch (WebException wex)
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(wex.Response.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());

                if (jsonResult.SelectToken("Error.FaultActor").ToString() == "UpdateWorkOrderServiceApp")
                {
                    lblMessage.Text = "<b> Work Order cannot be updated. " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
                }
                else
                {
                    lblMessage.Text = "<b> Work Order cannot be updated. Please rectify the error: " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
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
            lblWONUM.Text = "";
            txtContractId.Text = "";
            //txtCrew.Text = "";
            //txtScheduleFinish.Text = "";
            txtScheduler.Text = "";
            //txtScheduleStart.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
            ddlOwnership.SelectedIndex = 0;
            ddlPriority.SelectedIndex = 0;
        }
        private void FillWODetail()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT OWNER,PRIORITY,VENDOR,EXT_SCHEDULER,EXT_ENGINEER,EXT_INSPECTOR,EXT_CREWLEAD,EXT_CONTRACTID,ORIGRECORDID,ORIGRECORDCLASS,EXT_CRMSRID,STATUS, (Select [NAME] FROM EAMS.DimCompany WHERE Company = w.VENDOR) AS VENDORNAME FROM [ODW].[GIS].[vwWorkOrder] w WHERE WONUM = @WONUM", con))
                {
                    cmd.Parameters.AddWithValue("@WONUM", _WONUM);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objWorkOrder = new WorkOrder();
                            ddlOwnership.SelectedValue = reader["OWNER"].ToString();
                            ddlPriority.SelectedValue = reader["PRIORITY"].ToString();
                            txtContractId.Text = reader["EXT_CONTRACTID"].ToString();
                            txtVendor.Text = reader["VENDOR"].ToString();
                            txtVendorName.Text = reader["VENDORNAME"].ToString();
                            txtScheduler.Text = reader["EXT_SCHEDULER"].ToString();
                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();

                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["ORIGRECORDID"].ToString();

                            if (reader["STATUS"].ToString().Equals("DISPATCH"))
                            {
                                btnSubmit.Enabled = false;
                                btnSubmit.ForeColor = System.Drawing.Color.Gray;
                                btnSubmit.BorderColor = System.Drawing.Color.Gray;
                            }
                        }
                    }
                }
            }
        }
    }
}