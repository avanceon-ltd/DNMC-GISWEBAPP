using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppForm.App_Code;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class CreateBAUSRCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");
            txtReportedBy.Attributes.Add("readonly", "readonly");
            //    txtLocation.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                txtWOReportedDate.Text = DateTime.Now.ToString();
                txtTargetStart.Text = DateTime.Now.ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(4).ToString();


                if (!String.IsNullOrEmpty(Request.QueryString["CRMSId"]))
                {
                    string _ID = Convert.ToString(Request.QueryString["CRMSId"]);
                    lblCRMSId.Text = _ID;

                }

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
                objWorkOrder.Longitude = txtlong.Text.Trim();
                objWorkOrder.Latitude = txtLat.Text.Trim();
                objWorkOrder.TargetStart = Convert.ToDateTime(txtTargetStart.Text).ToString("s");
                objWorkOrder.TargetFinish = Convert.ToDateTime(txtTargetFinish.Text).ToString("s");


                CreateSR(objWorkOrder);
            }
        }

        void CreateSR(WorkOrder pWorkOrder)
        {
            try
            {
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BAUCreateSRUri"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["FAUTH"] = authValue;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    dynamic jsonReqeust = new JObject();
                    jsonReqeust.CreateServiceRequestRequest = new JObject();
                    jsonReqeust.CreateServiceRequestRequest.Header = new JObject();
                    jsonReqeust.CreateServiceRequestRequest.Header.referenceNum = "a";
                    jsonReqeust.CreateServiceRequestRequest.Header.clientChannel = "DSS";
                    jsonReqeust.CreateServiceRequestRequest.Header.requestTime = DateTime.Now.ToString("s") + "Z";
                    jsonReqeust.CreateServiceRequestRequest.Header.retryFlag = "N";
                    jsonReqeust.CreateServiceRequestRequest.Header.sucessfulReversalFlag = "N";

                    jsonReqeust.CreateServiceRequestRequest.Body = new JObject();
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest = new JObject();
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore = new JObject();

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.AssetNumber = pWorkOrder.Asset;

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ShortDescripton = pWorkOrder.Description;

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.LongDescription = pWorkOrder.Description;
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalRecId = "123";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalCRMsrId = lblCRMSId.Text.Trim();

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalDepartment = "DRAINAGE";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalSoruce = "CRM";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalSRGroup = TxtboxSRGrp.Text.Trim();
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalSRType = DrpDwnLstSrType.SelectedValue;

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ReportDate = DateTime.Now.ToString("s") + "Z";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ReportedBy = txtReportedBy.Text;

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.InternalPriority = DrpDwnIntrnalPriority.SelectedValue;
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.IssueLocation = txtLocation.Text.Trim();

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.PriorityOfIssueReported = drpDwnReprtedPriority.SelectedValue;
                    
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.SiteId = "101";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.Status = "NEW";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.Source = "DSS";

                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TargetFinish = pWorkOrder.TargetFinish + "Z";
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TargetStart = pWorkOrder.TargetStart + "Z";


                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TKServiceAddress = new JObject();
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TKServiceAddress.LatitudeY = pWorkOrder.Latitude;
                    jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TKServiceAddress.LongitudeX = pWorkOrder.Longitude;

                    var s = jsonReqeust.ToString();
                    //   s = s.Replace("{{", "}");

                    streamWriter.Write(s);
                }

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    lblMessage.Text = "BAU SR created successfully. Your SRNUM is: <b>" + jsonResult.SelectToken("CreateServiceRequestReply.Body.IncidentId").ToString() + "</b>";
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                }
            }
            catch (WebException wex)
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(wex.Response.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());

                lblMessage.Text = "<b> BAU SR didn't created. Error: " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
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
            Response.Redirect("");

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