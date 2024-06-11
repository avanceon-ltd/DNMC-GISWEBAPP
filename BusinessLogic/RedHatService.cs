using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using WebAppForm.BusinessEntity;
using log4net;

namespace WebAppForm.App_Code
{
    public class RedHatService
    {
        ILog log = log4net.LogManager.GetLogger(typeof(RedHatService));
        string IP = new EventLogging().GetIP();
        string request = "";
        public enum ServiceOperations
        {
            Comp_Create_WO, Update_WO, Dispatch_WO,CM_Create_WO, SR_Create,Assign_Vehicle
        }
        internal Tuple<bool, string> CreateServiceRequest(ServiceRequest serviceRequest)
        {
            bool isSuccess = false;
            string res = "";

            string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BAUCreateSRUri"];
            string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];

            System.Net.ServicePointManager.Expect100Continue = false;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);



            //specify to use TLS 1.2 as default connection
            //     System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            //   System.Net.ServicePointManager.Expect100Continue = false;

            //  httpWebRequest.KeepAlive = false;
            //  httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            //  httpWebRequest.ServicePoint.ConnectionLimit = 24;
            //   httpWebRequest.Timeout = 10000;

            ////   httpWebRequest.TransferEncoding = "chunked";

            httpWebRequest.ContentType = "text/xml;charset=utf-8";
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

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.AssetNumber = serviceRequest.Asset;

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ShortDescripton = serviceRequest.Description;

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.LongDescription = serviceRequest.Description;
                //jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalRecId = "123";
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalDepartment = "DRAINAGE";
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalSoruce = "CRM";
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalSRGroup = serviceRequest.ExternalSRGroup;
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ExternalSRType = serviceRequest.ExternalSRType;

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ReportDate = serviceRequest.ReportedDate;
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.ReportedBy = serviceRequest.ReportedBy;

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.InternalPriority = serviceRequest.InternalPriority;
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.IssueLocation = serviceRequest.IssueLocation;

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.PriorityOfIssueReported = serviceRequest.PriorityOfIssueReported;


                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.SiteId = "101";
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.Status = "QUEUED";
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.Source = "DSS";

                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TargetFinish = serviceRequest.TargetFinish;
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TargetStart = serviceRequest.TargetStart;


                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TKServiceAddress = new JObject();
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TKServiceAddress.LatitudeY = serviceRequest.Latitude;
                jsonReqeust.CreateServiceRequestRequest.Body.ServiceRequest.ServiceRequestCore.TKServiceAddress.LongitudeX = serviceRequest.Longitude;

                var s = jsonReqeust.ToString();
                //   s = s.Replace("{{", "}");

                streamWriter.Write(s);
            }

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                isSuccess = true;
                res = "SR created successfully. Your SR# is: <b>" + jsonResult.SelectToken("CreateServiceRequestReply.Body.IncidentId").ToString() + "</b>";
                //lblMessage.Text = "SR created successfully. Your SR# is: <b>" + jsonResult.SelectToken("CreateServiceRequestReply.Body.IncidentId").ToString() + "</b>";
                //ClearControls();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

            }

            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }

        internal Tuple<bool, string,string> VehicleAssignement(string WONUM, string VEHNUM, string USER)
        {
            bool isSuccess = false;
            string messageStatus = string.Empty;
            string res = "";
            try
            {
                string assignUri = System.Web.Configuration.WebConfigurationManager.AppSettings["AssignVehicle"];
                assignUri += "?wo=" + WONUM + "&vehicle=" + VEHNUM + "&user=" + USER;
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(assignUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers["FAUTH"] = authValue;

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());

                    isSuccess = true;
                    string Description = jsonResult.SelectToken("Message.Description").ToString();
                    UpdateLogRequest(jsonObject.ToString(), Description, ServiceOperations.Assign_Vehicle.ToString());
                    res = Description;

                    if (jsonResult.SelectToken("Message.Status").ToString().ToLower() == "success")
                        messageStatus = jsonResult.SelectToken("Message.Status").ToString().ToLower();
                }
            }
            catch (WebException wex)
            {
                log.Info("Fuse Error Assign Vehicle:\n" + wex.Message);
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(wex.Response.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                res = "<b>" + jsonResult.SelectToken("Error.Description").ToString() + "</b>";
            }
            catch (Exception exp)
            {
                res = exp.Message;
            }
            var result = new Tuple<bool, string,string>(isSuccess, res,messageStatus);
            return result;
        }

        internal Tuple<bool, string> CreateWorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";
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
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalFrameZone = pWorkOrder.ExternalFrameZone;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalInspector = pWorkOrder.Inspector;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Location = "QATAR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ReportDate = DateTime.Now.ToString("s");
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ReportedBy = pWorkOrder.ReportedBy;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Status = "WAPPR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.TargetCompletionDate = pWorkOrder.TargetFinish;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.TargetStartDate = pWorkOrder.TargetStart;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OnBehalfOf = pWorkOrder.OnBehalfOf;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = pWorkOrder.ExternalDefectType;
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.ExternalCRMSRID;
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
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROBTYPE_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "1";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROBTYPE"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "RAINWATER";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["CASPROB_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "2";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["CASPROB"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "YES";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["VISEXFL_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "3";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["VISEXFL"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["FLODLOC_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "4";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["FLODLOC"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["EXTFLOD_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "5";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["EXTFLOD"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["FLDFRM_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "6";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["FLDFRM"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["NOAFPROP_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "7";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["NOAFPROP"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROBAST_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "8";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROBAST"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["CASBLK_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "9";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["CASBLK"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "SUCTION";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["WRKSIT_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "10";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["WRKSIT"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["FOWRTYP_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "11";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["FOWRTYP"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = pWorkOrder.ECCOwnership;
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["THRDP_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "12";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["THRDP"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = pWorkOrder.ECCPriority;
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROPERTY_TYPE_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "13";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROPERTY_TYPE"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);


                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["WARNINGLETTER_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "14";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["WARNINGLETTER"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["DUPLICATE_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "15";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["DUPLICATE"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["PARENTCRMSSRNO_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "16";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["PARENTCRMSSRNO"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["HOTSPOT_NUMBER_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "17";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["HOTSPOT_NUMBER"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROJECT_ID_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "18";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROJECT_ID"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = "";
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["RELATED_TO_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "19";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["RELATED_TO"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = pWorkOrder.UnderpassRelated.ToLower() == "null" ? "" : pWorkOrder.UnderpassRelated;
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["ISUPRELATED_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "20";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["ISUPRELATED"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    wospec = new JObject();
                    wospec.ALNValue = pWorkOrder.Severity.ToLower() == "null" ? "" : pWorkOrder.Severity;
                    wospec.ClassSpecID = WebConfigurationManager.AppSettings["SEVERITY_ID"];
                    wospec.ClassStructureId = "20089";
                    wospec.DisplaySequence = "21";
                    wospec.AssetAttrId = WebConfigurationManager.AppSettings["SEVERITY"];
                    jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                    LogRequest(jsonReqeust.ToString(), ServiceOperations.Comp_Create_WO.ToString());
                    request = jsonReqeust.ToString();
                    streamWriter.Write(jsonReqeust.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    isSuccess = true;
                    string wonum = jsonResult.SelectToken("UpdateWorkOrderReply.Body.WorkOrderNumber").ToString();
                    UpdateLogRequest(jsonObject.ToString(), wonum, ServiceOperations.Comp_Create_WO.ToString());
                    res = "Work Order created successfully. Your WONUM is: <b>" + wonum + "</b>";
                }
            }
            catch (Exception ex)
            {
                res = CatchException(ex,ServiceOperations.Comp_Create_WO.ToString());
            }
            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }

        internal Tuple<bool, string> DispatchWorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";
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
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCrewLead = pWorkOrder.CrewLead;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledFinish = pWorkOrder.ScheduleFinish;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledStart = pWorkOrder.ScheduleStart;
                    if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = "DISPATCH";
                    if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                    {
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                    }
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                    if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.CRMSID;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";

                    LogRequest(jsonReqeust.ToString(), ServiceOperations.Dispatch_WO.ToString());
                    request = jsonReqeust.ToString();
                    streamWriter.Write(jsonReqeust.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    isSuccess = true;
                    UpdateLogRequest(jsonObject.ToString(), pWorkOrder.WONUM, ServiceOperations.Dispatch_WO.ToString());
                    res = "Work Order: " + pWorkOrder.WONUM + " dispatched successfully.";// + jsonResult.SelectToken("UpdateWorkOrderReply.Body.Success").ToString() + "</b>";
                }
            }
            catch (Exception ex)
            {
                res = CatchException(ex, ServiceOperations.Dispatch_WO.ToString());
            }
            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }


        internal Tuple<bool, string> UpdateWorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";

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


                    if (string.IsNullOrEmpty(pWorkOrder.ContractId))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalContractID = " ";
                    else
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalContractID = pWorkOrder.ContractId;

                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";

                    if (string.IsNullOrEmpty(pWorkOrder.Engineer))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalEngineer = " ";
                    else
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalEngineer = pWorkOrder.Engineer;

                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;

                    //if (!String.IsNullOrEmpty(objWorkOrder.EAMSSRNumber))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = lblWOStatus.Text; //"APPR";
                    if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                    {
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                    }
                    else
                    {
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = null;
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = null;
                    }
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                    if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                        jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.CRMSID;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCSRGroup = "DR_CSRL2";
                    //jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalFrameZone = pWorkOrder.ExternalFrameZone;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = "APPR";

                    LogRequest(jsonReqeust.ToString(), ServiceOperations.Update_WO.ToString());
                    request = jsonReqeust.ToString();
                    streamWriter.Write(jsonReqeust.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    isSuccess = true;
                    UpdateLogRequest(jsonObject.ToString(), pWorkOrder.WONUM, ServiceOperations.Update_WO.ToString());
                    res = "Work Order: " + pWorkOrder.WONUM + " updated successfully.";// + jsonResult.SelectToken("UpdateWorkOrderReply.Body.Success").ToString() + "</b>";
                }
            }
            catch (Exception ex)
            {
                res = CatchException(ex, ServiceOperations.Update_WO.ToString());
            }
            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }
        internal Tuple<bool, string> CreateCMWorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";

            string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BAUCreateWOUri"];
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
                //   jsonReqeust.CreateWorkOrderRequest.Header.sucessfulReversalFlag = "N";

                jsonReqeust.CreateWorkOrderRequest.Body = new JObject();
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder = new JObject();
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.AssetNumber = pWorkOrder.Asset;
                //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ChangeBy = lblReportedBy.Text;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Description = pWorkOrder.Description;
                //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalContractID = pWorkOrder.ContractId;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalEngineer = pWorkOrder.Engineer;
                //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalInspector = pWorkOrder.Inspector;
                //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Location = pWorkOrder.Location;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ReportDate = pWorkOrder.ReportedDate;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ReportedBy = pWorkOrder.ReportedBy;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.Status = "WAPPR";
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.TargetCompletionDate = pWorkOrder.TargetFinish;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.TargetStartDate = pWorkOrder.TargetStart;
                //jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.OnBehalfOf = pWorkOrder.OnBehalfOf;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ExternalCSRGroup = "DR_CSRL2";
                // jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";  removed by raj and zahir in testing 30/12/19
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderPriority = pWorkOrder.ECCPriority;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkType = "CM";
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress = new JObject();
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.LatitudeY = pWorkOrder.Latitude;
                jsonReqeust.CreateWorkOrderRequest.Body.WorkOrder.WorkOrderServiceAddress.LongitudeX = pWorkOrder.Longitude;

                var jsonBracketSplit = jsonReqeust.ToString();
                jsonBracketSplit = jsonBracketSplit.Replace("{{", "}");

                streamWriter.Write(jsonBracketSplit);
            }

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                isSuccess = true;
                res = "Work Order created successfully. Your WONUM is: <b>" + jsonResult.SelectToken("CreateWorkOrderReply.Body.WorkOrderNumber").ToString() + "</b>";

            }
            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }


        internal Tuple<bool, string> UpdateWorkOrderECC(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";

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
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";

                if (string.IsNullOrEmpty(pWorkOrder.ContractId))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalContractID = " ";
                else
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalContractID = pWorkOrder.ContractId;

                if (string.IsNullOrEmpty(pWorkOrder.Engineer))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalEngineer = " ";
                else
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalEngineer = pWorkOrder.Engineer;

                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalFrameZone = pWorkOrder.Zone;
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;

                if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = pWorkOrder.Status; //"APPR";
                if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                {
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                }
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.CRMSID;
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCSRGroup = "DR_CSRL2";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";

                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec = new JArray() as dynamic;

                dynamic wospec = new JObject();
                wospec.ALNValue = pWorkOrder.ECCOwnership;// "CUSTOMER-SERVICE";
                wospec.ClassSpecID = WebConfigurationManager.AppSettings["THRDP_ID"];
                wospec.ClassStructureId = "20089";
                wospec.AssetAttrId = WebConfigurationManager.AppSettings["THRDP"];
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                wospec = new JObject();
                wospec.ALNValue = pWorkOrder.ECCPriority;// "VIP";
                wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROPERTY_TYPE_ID"];
                wospec.ClassStructureId = "20089";
                wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROPERTY_TYPE"];
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                streamWriter.Write(jsonReqeust.ToString());
            }
            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                isSuccess = true;
                res = "Work Order: " + pWorkOrder.WONUM + " updated successfully.";// + jsonResult.SelectToken("UpdateWorkOrderReply.Body.Success").ToString() + "</b>";
            }

            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }
        internal Tuple<bool, string> DispatchWorkOrderECC(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";

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
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCrewLead = pWorkOrder.CrewLead;
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDepartment = "DRAINAGE";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalScheduler = pWorkOrder.Scheduler;
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledFinish = pWorkOrder.ScheduleFinish;
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ScheduledStart = pWorkOrder.ScheduleStart;
                if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalSource = "CRM";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.SiteID = "101";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.Status = "DISPATCH";
                if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                {
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordID = pWorkOrder.EAMSSRNumber;
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigRecordClass = "SR";
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.OrigTKID = pWorkOrder.EAMSSRNumber;
                }
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDNMC = "DNMC";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalDefectType = "RNWFLOOD";
                if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
                    jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ExternalCRMSRID = pWorkOrder.CRMSID;
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.ClassStructureID = "20089";
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkType = "FR";

                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec = new JArray() as dynamic;

                dynamic wospec = new JObject();
                wospec.ALNValue = pWorkOrder.ECCOwnership;// "CUSTOMER-SERVICE";
                wospec.ClassSpecID = WebConfigurationManager.AppSettings["THRDP_ID"];
                wospec.ClassStructureId = "20089";
                wospec.AssetAttrId = WebConfigurationManager.AppSettings["THRDP"];
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                wospec = new JObject();
                wospec.ALNValue = pWorkOrder.ECCPriority;// "VIP";
                wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROPERTY_TYPE_ID"];
                wospec.ClassStructureId = "20089";
                wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROPERTY_TYPE"];
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                wospec = new JObject();
                wospec.ALNValue = pWorkOrder.Hotspot; // "Hotspot ID";
                wospec.ClassSpecID = WebConfigurationManager.AppSettings["HOTSPOT_NUMBER_ID"];
                wospec.ClassStructureId = "20089";
                wospec.AssetAttrId = WebConfigurationManager.AppSettings["HOTSPOT_NUMBER"];
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                wospec = new JObject();
                wospec.ALNValue = pWorkOrder.Project;// "Project ID";
                wospec.ClassSpecID = WebConfigurationManager.AppSettings["PROJECT_ID_ID"];
                wospec.ClassStructureId = "20089";
                wospec.AssetAttrId = WebConfigurationManager.AppSettings["PROJECT_ID"];
                jsonReqeust.UpdateWorkOrderRequest.Body.WorkOrder.WorkOrderSpec.Add(wospec);

                streamWriter.Write(jsonReqeust.ToString());
            }
            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                isSuccess = true;
                res = "Work Order: " + pWorkOrder.WONUM + " dispatched successfully.";// + jsonResult.SelectToken("UpdateWorkOrderReply.Body.Success").ToString() + "</b>";
            }

            var result = new Tuple<bool, string>(isSuccess, res);
            return result;
        }

        #region Logging
        private void LogRequest(string json, string ServiceOperation = "", string WONUM = null)
        {
            log.Info("Fuse Request:\n IP: " + IP + " request: \n" + json);
        }
        private void UpdateLogRequest(string jsonText, string WONUM, string ServiceOperation)
        {
            log.Info("IP: " + IP + "\n response: \n" + jsonText + "\nWONUM: " + WONUM);
        }
            #endregion

            #region "Catch Exception"
            /*private string CatchException(Exception wex, string oprs, string woSrId, RequestType requestType, string externalRefID = "")*/
            private string CatchException(Exception wex, string oprs)
        {
            string res = "";
            try
            {
                log.Info("Fuse Error:\n" + wex.Message);
                if (wex is WebException)
                {
                    WebException webexp = (WebException)wex;
                    if (webexp.Message.Contains("Unable to connect"))
                    {
                        res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                    }
                    else
                    {
                        var resp = new StreamReader(webexp.Response.GetResponseStream()).ReadToEnd();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(resp);
                        XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");
                        string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);

                        jsonText = JSONStringReplace(jsonText);
                        res = "Error Detail: " + error[0].InnerText;
                    }
                }
                else
                {
                    res = wex.Message;
                }
            }
            catch (Exception exp)
            {
                res = exp.Message;
            }

            log.Error("Fuse Error:\n" + oprs.ToString() + " response: \n" + res + "\nrequest: \n" + request);
            return res;
        }

        public string JSONStringReplace(string jsonText)
        {
            return jsonText.Replace("NS1:", "").Replace(@"""?xml"":{""@version"":""1.0"",""@encoding"":""UTF-8""},", "")
                                   .Replace(@"""@xmlns:NS1"":""esb.ashghal.gov.qa"",", "").Replace("eAI_Header", "Header").Replace("eAI_STATUS", "STATUS");
        }
        #endregion
    }
}