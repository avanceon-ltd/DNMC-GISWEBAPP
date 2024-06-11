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
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class CreateWOCustomer : System.Web.UI.Page
    {
       static WorkOrder objWorkOrder;
        protected void Page_Load(object sender, EventArgs e)
        {

            txtVendor.Attributes.Add("readonly", "readonly");
            txtVendorName.Attributes.Add("readonly", "readonly");
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                txtWOReportedDate.Text = DateTime.Now.ToString();
                txtTargetStart.Text = DateTime.Now.ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(4).ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["CRMSId"]))
                {
                    string _ID = Convert.ToString(Request.QueryString["CRMSId"]);
                    lblCRMSId.Text = _ID;
                    FillRainWaterComplaintData(_ID);
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

                if (!String.IsNullOrEmpty(Request.QueryString["WorkType"]))
                {
                    drpdwnWorkType.Text = Convert.ToString(Request.QueryString["WorkType"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Lat"]))
                {
                    txtLat.Text = Convert.ToString(Request.QueryString["Lat"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Long"]))
                {
                    txtlong.Text = Convert.ToString(Request.QueryString["Long"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ECCOwnership"]))
                {
                    ddlOwnership.Text = Convert.ToString(Request.QueryString["ECCOwnership"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ECCPriority"]))
                {
                    ddlPriority.Text = Convert.ToString(Request.QueryString["ECCPriority"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Engineer"]))
                {
                    txtEngineer.Text = Convert.ToString(Request.QueryString["Engineer"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Inspector"]))
                {
                    txtInspector.Text = Convert.ToString(Request.QueryString["Inspector"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ReportedBy"]))
                {
                    lblReportedBy.Text = Convert.ToString(Request.QueryString["ReportedBy"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Scheduler"]))
                {
                    txtScheduler.Text = Convert.ToString(Request.QueryString["Scheduler"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ContractId"]))
                {
                    txtContractId.Text = Convert.ToString(Request.QueryString["ContractId"]); ;
                }

                //if (!String.IsNullOrEmpty(Request.QueryString["CrewLead"]))
                //{
                //    txtCrew.Text = Convert.ToString(Request.QueryString["CrewLead"]); ;
                //}
            }
        }

        private void FillRainWaterComplaintData(string iD)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT [Service_Request_Number], [Contact_Reason], [Details],[Latitude],[Longitude],[Street],[Municipality],[Building_Number],[Zone],[District],[Pin],[EAMS_SR_Number],[TICKETID],[WONUM] FROM [ODW].[GIS].[vwComplaintsExternalFlooding] WHERE Service_Request_Number = @UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", iD);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtWODes.Text = reader["Service_Request_Number"].ToString().Trim() + " – " + reader["Contact_Reason"].ToString().Trim();
                            txtLat.Text = reader["Latitude"].ToString().Trim();
                            txtlong.Text = reader["Longitude"].ToString().Trim();

                            objWorkOrder = new WorkOrder();
                            objWorkOrder.Street = reader["Street"].ToString();
                            //objWorkOrder.Municipality = reader["Municipality"].ToString();
                            objWorkOrder.BuildingNumber = reader["Building_Number"].ToString();
                            objWorkOrder.Zone = reader["Zone"].ToString();
                            objWorkOrder.District = reader["District"].ToString();
                            objWorkOrder.Pin = reader["Pin"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["EAMS_SR_Number"].ToString();
                            objWorkOrder.TICKETID = reader["TICKETID"].ToString();
                            objWorkOrder.CRMSID = reader["Service_Request_Number"].ToString();
                            //objWorkOrder.SADescription = reader["Details"].ToString().Substring(0,40);
                            lblEAMSSRId.Text = reader["EAMS_SR_Number"].ToString();
                            if (String.IsNullOrEmpty(lblEAMSSRId.Text) || !String.IsNullOrEmpty(reader["WONUM"].ToString()))
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //objWorkOrder = new WorkOrder();
                objWorkOrder.Description = txtWODes.Text.Trim();
                objWorkOrder.Location = txtLocation.Text.Trim();
                objWorkOrder.Asset = txtAsset.Text.Trim();
                objWorkOrder.WorkType = drpdwnWorkType.SelectedValue.Trim();
                objWorkOrder.ReportedDate = Convert.ToDateTime(txtWOReportedDate.Text).ToString("s");
                objWorkOrder.Longitude = txtlong.Text.Trim();
                objWorkOrder.Latitude = txtLat.Text.Trim();
                objWorkOrder.TargetStart = Convert.ToDateTime(txtTargetStart.Text).ToString("s");
                objWorkOrder.TargetFinish = Convert.ToDateTime(txtTargetFinish.Text).ToString("s");
                objWorkOrder.ECCOwnership = ddlOwnership.SelectedValue.Trim();
                objWorkOrder.ECCPriority = ddlPriority.SelectedValue.Trim();
                objWorkOrder.ContractId = txtContractId.Text.Trim();
                //objWorkOrder.Vendor = txtVendor.Text.Trim();
                //objWorkOrder.OnBehalfOf = txtOnBehalfOf.Text.Trim();
                objWorkOrder.Engineer = txtEngineer.Text.Trim();
                objWorkOrder.Scheduler = txtScheduler.Text.Trim();
                objWorkOrder.Inspector = txtInspector.Text.Trim();

                CreateWO(objWorkOrder);
            }
        }

        void CreateWO(WorkOrder pWorkOrder)
        {
            try
            {
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCCreateWOUri"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["FAUTH"] = authValue;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    dynamic jsonReqeust = new JObject();
                    jsonReqeust.CreateWorkOrderRequest = new JObject();
                    jsonReqeust.CreateWorkOrderRequest.Header = new JObject();
                    jsonReqeust.CreateWorkOrderRequest.Header.referenceNum = "a";
                    jsonReqeust.CreateWorkOrderRequest.Header.clientChannel = "DSS";
                    jsonReqeust.CreateWorkOrderRequest.Header.requestTime = DateTime.Now.ToString("s");
                    jsonReqeust.CreateWorkOrderRequest.Header.retryFlag = "N";
                    jsonReqeust.CreateWorkOrderRequest.Header.sucessfulReversalFlag = "N";

                    jsonReqeust.CreateWorkOrderRequest.Body = new JObject();
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder = new JObject();
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.AssetNumber = pWorkOrder.Asset;
                    //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ChangeBy = lblReportedBy.Text;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Description = pWorkOrder.Description;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalContractID = pWorkOrder.ContractId;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalEngineer = pWorkOrder.Engineer;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalInspector = pWorkOrder.Inspector;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Location = "QATAR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ReportDate = DateTime.Now.ToString("s");
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ReportedBy = lblReportedBy.Text;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Status = "WAPPR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.TargetCompletionDate = pWorkOrder.TargetFinish;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.TargetStartDate = pWorkOrder.TargetStart;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                    //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OnBehalfOf = pWorkOrder.OnBehalfOf;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = objWorkOrder.CRMSID;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalCSRGroup = "DR_CSRL2";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderPriority = "3";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress = new JObject();
                    //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.AddressLine2 = pWorkOrder;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.AddressLine3 = pWorkOrder.BuildingNumber;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.County = pWorkOrder.Zone;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.Description = pWorkOrder.SADescription;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.LatitudeY = pWorkOrder.Latitude;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.LongitudeX = pWorkOrder.Longitude;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.RegionDistrict = pWorkOrder.District;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.StateProvince = pWorkOrder.Municipality;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.StreetAddress = pWorkOrder.Street;
                    
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec = new JArray() as dynamic;

                    dynamic wospec = new JObject();
                    wospec.ALNValue = "FLOOD";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "1";
                    wospec.AssetAttrId = "PROBTYPE";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "RAINWATER";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "2";
                    wospec.AssetAttrId = "CASPROB";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "YES";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "3";
                    wospec.AssetAttrId = "VISEXFL";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "4";
                    wospec.AssetAttrId = "FLODLOC";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "5";
                    wospec.AssetAttrId = "EXTFLOD";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "6";
                    wospec.AssetAttrId = "FLDFRM";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "7";
                    wospec.AssetAttrId = "NOAFPROP";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "8";
                    wospec.AssetAttrId = "PROBAST";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "9";
                    wospec.AssetAttrId = "CASBLK";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "SUCTION";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "10";
                    wospec.AssetAttrId = "WRKSIT";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "11";
                    wospec.AssetAttrId = "FOWRTYP";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = ddlOwnership.SelectedValue;// "CUSTOMER-SERVICE";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "12";
                    wospec.AssetAttrId = "THRDP";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = ddlPriority.SelectedValue;// "VIP";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "13";
                    wospec.AssetAttrId = "PROPERTY_TYPE";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "14";
                    wospec.AssetAttrId = "WARNINGLETTER";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "15";
                    wospec.AssetAttrId = "DUPLICATE";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "16";
                    wospec.AssetAttrId = "PARENTCRMSSRNO";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    

                    streamWriter.Write(jsonReqeust.ToString());
                }

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    lblMessage.Text = "Work Order created successfully. Your WONUM is: <b>" + jsonResult.SelectToken("UpdateWorkOrderReply.Body.WorkOrderNumber").ToString() + "</b>";
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
                    lblMessage.Text = "<b> WO created successfully. The WONUM is " + jsonResult.SelectToken("Error.Detail.WorkOrderNumber").ToString() + ". While updating got the error: " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
                }
                else
                {
                    lblMessage.Text = "<b> Please rectify the error: " + jsonResult.SelectToken("Error.Detail.ExceptionType").ToString() + "</b>";
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
            drpdwnWorkType.SelectedValue = "FR";
            txtTargetStart.Text = "";
            txtTargetFinish.Text = "";
            txtlong.Text = "";
            txtLat.Text = "";
            ddlOwnership.SelectedIndex = -1;
            ddlPriority.SelectedIndex = -1;
            txtContractId.Text = "";
            txtScheduler.Text = "";
            txtEngineer.Text = "";
            txtInspector.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
        }

        [WebMethod]
        public static string[] GetCustomers(string prefix)
        {
            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT  [ASSETNUM],DESCRIPTION FROM AssetList_PS where ASSETNUM like @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(string.Format("{0}-{1}", sdr["DESCRIPTION"], sdr["ASSETNUM"]));
                        }
                    }
                    conn.Close();
                }
            }
            return customers.ToArray();
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
        protected void ddlOwnership_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOwnership.SelectedIndex == 1)
            {
                txtContractId.Text = "C/2018/41";// "C /2019/63";//C/2018/41
                txtVendor.Text = "101-104142-Doha";// "101 -105790-Doha";//101-104142-Doha
                txtVendorName.Text = "METITO (OVERSEAS) QATAR";// "VELOSI CERTIFICATION LLC";//METITO (OVERSEAS) QATAR
                txtEngineer.Text = "STALREJA";
                txtScheduler.Text = "M_SCH_SOUTH";
                ddlPriority.SelectedValue = "HIGH";
            }
            else if (ddlOwnership.SelectedIndex == 2 || ddlOwnership.SelectedIndex == 3 || ddlOwnership.SelectedIndex == 4
                || ddlOwnership.SelectedIndex == 5 || ddlOwnership.SelectedIndex == 6 || ddlOwnership.SelectedIndex == 0)
            {
                txtContractId.Text = "";
                txtVendor.Text = "";
                txtVendorName.Text = "";
                txtEngineer.Text = "";
                txtScheduler.Text = "";
                ddlPriority.SelectedIndex = 0;
            }

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
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
            else
            {
                args.IsValid = false;
            }
        }
    }
}