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
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppForm.App_Code;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class DispatchWOBAU1 : System.Web.UI.Page
    {
        WorkOrder objWorkOrder;

     //   static string _scheduler = string.Empty;
        bool Reloadbtnclicked = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtContractId.Attributes.Add("readonly", "readonly");
            txtScheduler.Attributes.Add("readonly", "readonly");

            txtVendor.Attributes.Add("readonly", "readonly");
            txtVendorName.Attributes.Add("readonly", "readonly");

            Page.DataBind();

            if (!string.IsNullOrEmpty(Request.Params["__EVENTTARGET"]))
            {
                if (Request.Params["__EVENTTARGET"].ToString().Contains("Reload"))
                {
                    Reloadbtnclicked = true;
                }
                else
                    Reloadbtnclicked = false;
            }
            else
                Reloadbtnclicked = false;

            if (!Page.IsPostBack || Reloadbtnclicked)
            {
                txtScheduleStart.Text = DateTime.Now.ToString();
                txtScheduleFinish.Text = DateTime.Now.AddHours(4).ToString();
                if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
                {
                    lblWONUM.Text = Request.QueryString["WONUM"];
                    FillWODetail(Request.QueryString["WONUM"]);
                    
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //WorkOrder objWorkOrder = new WorkOrder();
                objWorkOrder = new WorkOrder();
                if (ViewState["DispatchWOBAU1Obj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["DispatchWOBAU1Obj"];
                }
                objWorkOrder.WONUM = lblWONUM.Text;
                objWorkOrder.ContractId = txtContractId.Text;
                objWorkOrder.CrewLead = txt_Crew.Text;

                objWorkOrder.ScheduleStart = Convert.ToDateTime(txtScheduleStart.Text).ToString("s");
                objWorkOrder.ScheduleFinish = Convert.ToDateTime(txtScheduleFinish.Text).ToString("s");
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

                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalContractID = pWorkOrder.ContractId;

                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCrewLead = pWorkOrder.CrewLead;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledFinish = pWorkOrder.ScheduleFinish;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledStart = pWorkOrder.ScheduleStart;
                  //  if (!String.IsNullOrEmpty(objWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = "DISPATCH";
                  //  if (!String.IsNullOrEmpty(objWorkOrder.EAMSSRNumber))
                    {
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                    }
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                   // jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                    if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.CRMSID;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";

                    
                    streamWriter.Write(jsonReqeust.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    lblMessage.Text = "Work Order: " + Request.QueryString["WONUM"] + " dispatched successfully.";// + jsonResult.SelectToken("UpdateWorkOrderReply.Body.Success").ToString() + "</b>";
                                                                                                                  //   ClearControls();

                    //dispatch no need to clear control just disable the btn
                    btnSubmit.Enabled = false;
                    btnSubmit.ForeColor = System.Drawing.Color.Gray;
                    btnSubmit.BorderColor = System.Drawing.Color.Gray;

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
                    lblMessage.Text = "<b> Work Order cannot be dispatched. " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
                }
                else
                {
                    lblMessage.Text = "<b> Work Order cannot be dispatched. Please rectify the error: " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
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
            txt_Crew.Text = "";
            txtScheduleFinish.Text = "";
            txtScheduler.Text = "";
            txtScheduleStart.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
            ddlFrameWrk.SelectedIndex = 0;
       
        }
        private void FillWODetail(string wonum)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT VENDOR,ONBEHALFOF,EXT_SCHEDULER,EXT_ENGINEER,EXT_INSPECTOR,EXT_FRAMEZONE,
                    EXT_CREWLEAD,EXT_CONTRACTID,ORIGRECORDID,ORIGRECORDCLASS,EXT_CRMSRID,STATUS, 
                    (Select [NAME] FROM EAMS.DimCompany WHERE Company = w.VENDOR) AS VENDORNAME,
                    (Select DESCRIPTION FROM EAMS.DimPurchView WHERE PURCHVIEW_KEY = w.PURCHVIEW_KEY) AS ContractDesc,
                    (Select DISPLAYNAME FROM EAMS.DimPerson WHERE PERSON_KEY = w.SCHEDULER_KEY) AS SchedulerDesc 
                    FROM [ODW].[EAMS].vwFactWorkOrder w WHERE WONUM = @WONUM", con))
                {
                    cmd.Parameters.AddWithValue("@WONUM", wonum);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objWorkOrder = new WorkOrder();
                            if (String.IsNullOrEmpty(reader["EXT_FRAMEZONE"].ToString()))
                            {
                                ddlFrameWrk.SelectedValue = "-1";
                            }
                            else
                            {
                                ddlFrameWrk.SelectedValue = reader["EXT_FRAMEZONE"].ToString();
                            }
                         

                            txtScheduler.Text = reader["EXT_SCHEDULER"].ToString();
                            if (string.IsNullOrEmpty(WebConfigurationManager.AppSettings[reader["EXT_SCHEDULER"].ToString() + "-Tooltip"]))
                            {
                                txtScheduler.ToolTip = reader["SchedulerDesc"].ToString();
                            }
                            else
                            {
                                txtScheduler.ToolTip = WebConfigurationManager.AppSettings[reader["EXT_SCHEDULER"].ToString() + "-Tooltip"];
                            }
                            
                                txt_Crew.Text = reader["EXT_CREWLEAD"].ToString();
                           
                            txtContractId.Text = reader["EXT_CONTRACTID"].ToString();
                            txtContractId.ToolTip = reader["ContractDesc"].ToString();
                            txtVendor.Text = reader["VENDOR"].ToString();
                            txtVendorName.Text = reader["VENDORNAME"].ToString();

                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();
                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();
                            lblCRMSSR.Text = reader["EXT_CRMSRID"].ToString();
                            lblWOStatus.Text = reader["STATUS"].ToString();

                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["ORIGRECORDID"].ToString();
                            ViewState["DispatchWOBAU1Obj"] = objWorkOrder;
                            if (reader["STATUS"].ToString().Equals("DISPATCH") || reader["STATUS"].ToString().Equals("WAPPR"))
                            {
                                btnSubmit.Enabled = false;
                                btnSubmit.ForeColor = System.Drawing.Color.Gray;
                                btnSubmit.BorderColor = System.Drawing.Color.Gray;
                            }
                            else
                            {
                                btnSubmit.Enabled = true;
                                btnSubmit.ForeColor = System.Drawing.Color.White;
                                btnSubmit.BorderColor = System.Drawing.Color.White;
                            }

                        }
                    }
                }
            }
        }

  
        protected void StartEndDiffValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime startDate;
            DateTime endDate;
            bool isStartDate = DateTime.TryParse(txtScheduleStart.Text, out startDate);
            bool isEndDate = DateTime.TryParse(txtScheduleFinish.Text, out endDate);
            if (isStartDate && isEndDate)
            {
                if (startDate < endDate)
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void lnkReload_Click(object sender, EventArgs e)
        {
            //     string wonum = Request.QueryString["WONUM"];
            //   FillWODetail(wonum);

        }
        
    }
}