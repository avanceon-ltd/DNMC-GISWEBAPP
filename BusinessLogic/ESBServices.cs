using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using WebAppForm.BusinessEntity;
using WebAppForm.BusinessLogic;
using WebAppForm.DAL;

namespace WebAppForm.App_Code
{
    public class ESBServices
    {
        ILog log = log4net.LogManager.GetLogger(typeof(ESBServices));
        int recID = 0;
        string ExternalRefID = string.Empty;
        string IP = new EventLogging().GetIP();
        public enum ServiceOperations
        {
            ECC_Comp_Create_WO, ECC_Comp_Update_WO, ECC_Update_WO, ECC_Dispatch_WO,
            BAU_Comp_Create_Rain_WO, BAU_Comp_Update_Rain_WO, BAU_Update_Rain_WO, BAU_Dispatch_Rain_WO,
            BAU_Comp_Create_Non_Rain_WO, BAU_Comp_Update_Non_Rain_WO, BAU_Update_Non_Rain_WO, BAU_Dispatch_Non_Rain_WO,
            CM_Create_WO, SR_Create, Fetch_WO,ECC_Duplicate_SR
        }
        #region SR
        internal Tuple<bool, string> CreateServiceRequest(ServiceRequest serviceRequest)
        {
            bool isSuccess = false;
            string res = "";
            string xml = string.Empty;

            string SRNo = String.Empty;
            try
            {
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BAUCreateSRESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.ServiceRequest.ToString());
                xml = new GenerateXML().GenerateSRXML(serviceRequest, ExternalRefID);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.ServiceRequest, ExternalRefID, ServiceOperations.SR_Create.ToString());
                recID = insert.Item2;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);


                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider").Count > 0)
                    {
                        if (xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText.Contains("Success"))
                        {
                            XmlNodeList node = xmlDoc.GetElementsByTagName("NS1:IncidentId");
                            if (node.Count > 0)
                            {
                                isSuccess = true;
                                SRNo = node[0].InnerText;
                                res = "SR created successfully. Your SR# is: <b>" + SRNo + "</b>";
                            }
                            else
                            {
                                res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                            }
                        }
                        else
                        {
                            res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                        }
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, SRNo, insert.Item2, RequestType.ServiceRequest, ExternalRefID, ServiceOperations.SR_Create.ToString());
                    }
                    //Label3.Text = "Work Order created successfully. Your WONUM is: <b>" + WONUM + "</b><br />" + lbl;
                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, ServiceOperations.SR_Create.ToString(), SRNo, RequestType.ServiceRequest, ExternalRefID, xml);
            }
            /*catch (WebException wex)
            {
                if (wex.Message.Contains("Unable to connect"))
                {
                    res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                    if (recID != 0)
                        UpdateLogRequest(res, "", SRNo, recID, RequestType.ServiceRequest, ExternalRefID, ServiceOperations.SR_Create.ToString());
                    log.Info(ServiceOperations.SR_Create.ToString() + " error: \n" + res + "\nExternal Ref ID: " + ExternalRefID);

                }
                else
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resp);
                    XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");


                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    res = "<b> SR creation error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";

                    if (recID != 0)
                        UpdateLogRequest(resp, jsonText, SRNo, recID, RequestType.ServiceRequest, ExternalRefID, ServiceOperations.SR_Create.ToString());
                    log.Info(ServiceOperations.SR_Create.ToString() + " error: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);

                }

            }
            catch (Exception exp)
            {
                res = exp.Message;
                UpdateLogRequest(res, "", SRNo, recID, RequestType.ServiceRequest, ExternalRefID, ServiceOperations.SR_Create.ToString());
                log.Info(ServiceOperations.SR_Create.ToString() + " error: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
            }*/
            var result = new Tuple<bool, string>(isSuccess, "Create SR: " + res);
            return result;
        }

        internal Tuple<bool, string, string> VehicleAssignement(string WONUM, string VEHNUM, string USER)
        {
            bool isSuccess = false;
            string messageStatus = string.Empty;
            string res = "";
            try
            {
                int WOCount = WONUM.Split(',').Length;
                int VehCount = VEHNUM.Split(',').Length;

                string SPName = "";
                if (WOCount == 1 && VehCount == 1)
                    SPName = "AddWO_Vehicle";
                else if (WOCount > 1 && VehCount == 1)
                    SPName = "AddWorkorders_Vehicle";
                else if (WOCount == 1 && VehCount > 1)
                    SPName = "AddVehicles_WO";

                ServicesLogs logs = new ServicesLogs();
                Tuple<bool, string, string> dbResult = logs.AssignVehicle(WONUM, VEHNUM, USER, SPName);

                isSuccess = dbResult.Item1;
                res = dbResult.Item2;
                messageStatus = dbResult.Item3;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            var result = new Tuple<bool, string, string>(isSuccess, res, messageStatus);
            return result;
        }

        private string ResponseTestXML()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?><esb:CreateServiceRequestReply xmlns:esb=""esb.ashghal.gov.qa""><esb:eAI_HEADER><esb:referenceNum>a</esb:referenceNum>
                     <esb:clientChannel>DSS</esb:clientChannel><esb:requestTime>2021-01-18T14:53:28Z</esb:requestTime><esb:retryFlag>N</esb:retryFlag><esb:sucessfulReversalFlag>N</esb:sucessfulReversalFlag>
                     </esb:eAI_HEADER><esb:eAI_STATUS><esb:returnStatus><esb:returnCode>ESB1009901E</esb:returnCode>
                     <esb:returnCodeDesc>General System Error:Error 500: BMXAA6713E - The MBO fetch operation failed in the mboset with the SQL error code 17006. The record could not be retrieved from the database. See the log file for more details about the error.
                       :https://eamsuat.ashghal.gov.qa:9443/meaweb/os/EXT_SR_SYNC</esb:returnCodeDesc>
                     </esb:returnStatus></esb:eAI_STATUS></esb:CreateServiceRequestReply>";
        }

        #endregion SR

        #region CreateWO
        internal Tuple<bool, string> CreateWorkOrder(WorkOrder pWorkOrder, bool withSpecification, bool isECC = false, int isRainWater = 1)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationCreate = "";
            string WONUM = String.Empty;
            string xml = string.Empty;

            try
            {
                if (isECC == true)
                {
                    serviceOperationCreate = ServiceOperations.ECC_Comp_Create_WO.ToString();
                }
                else
                {
                    if (withSpecification == true)
                    {
                        serviceOperationCreate = ServiceOperations.BAU_Comp_Create_Rain_WO.ToString();
                    }
                    else
                    {
                        serviceOperationCreate = ServiceOperations.BAU_Comp_Create_Non_Rain_WO.ToString();
                    }
                }

                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCCreateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().GenerateWOXML(pWorkOrder, ExternalRefID, withSpecification, isRainWater);

                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);

                recID = insert.Item2;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);
                    var updateResult = new Tuple<bool, string>(false, "");

                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        XmlNodeList node = xmlDoc.GetElementsByTagName("NS1:WorkOrderNumber");
                        if (node.Count > 0)
                        {
                            isSuccess = true;
                            WONUM = node[0].InnerText;
                            updateResult = UpdateWorkOrder(pWorkOrder, WONUM, withSpecification, isECC, true);
                            if (updateResult.Item1)
                            {
                                res = "Work Order created successfully. Your WONUM is: <b>" + WONUM + "</b>";
                            }
                            else
                            {
                                res = updateResult.Item2;
                            }
                        }
                        else
                        {
                            res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                        }
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);
                    }
                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationCreate, WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }
            /* catch (WebException wex)
             {
                 if (wex.Message.Contains("Unable to connect"))
                 {
                     res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                     log.Info(serviceOperationCreate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                     UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);
                 }
                 else
                 {
                     var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                     XmlDocument xmlDoc = new XmlDocument();
                     xmlDoc.LoadXml(resp);
                     XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                     string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                     jsonText = JSONStringReplace(jsonText);

                     res = "<b> WO creation error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                     //log.Info(serviceOperationCreate + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                     log.Error(serviceOperationCreate + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                     UpdateLogRequest(resp, jsonText, WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);
                 }
             }
             catch (Exception exp)
             {
                 res = exp.Message;
                 log.Info(serviceOperationCreate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                 UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);
             }*/
            var result = new Tuple<bool, string>(isSuccess, "Create WO: " + res);
            return result;
        }

        internal Tuple<bool, string> Create_ECC_WorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationCreate = "";
            string WONUM = String.Empty;
            string xml = string.Empty;

            try
            {
                #region New-Function-Code

                serviceOperationCreate = ServiceOperations.ECC_Comp_Create_WO.ToString();
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCCreateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().Generate_ECC_WOXML(pWorkOrder, ExternalRefID);

                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);

                recID = insert.Item2;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);
                    var updateResult = new Tuple<bool, string>(false, "");

                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        XmlNodeList node = xmlDoc.GetElementsByTagName("NS1:WorkOrderNumber");
                        if (node.Count > 0)
                        {
                            isSuccess = true;
                            WONUM = node[0].InnerText;
                            updateResult = Update_ECC_WorkOrder(pWorkOrder, WONUM);
                            if (updateResult.Item1)
                            {
                                res = "Work Order created successfully. Your WONUM is: <b>" + WONUM + "</b>";
                            }
                            else
                            {
                                res = updateResult.Item2;
                            }
                        }
                        else
                        {
                            res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                        }
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationCreate);
                    }
                }
                #endregion
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationCreate, WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }

            var result = new Tuple<bool, string>(isSuccess, "Create WO: " + res);
            return result;
        }

        #endregion CreateWO

        #region UpdateWO
        internal Tuple<bool, string> UpdateWorkOrder(WorkOrder pWorkOrder, string WONUM, bool withSpecification, bool isECC = false, bool isComposite = false, bool ReturnToSch = false)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationUpdate = "";
            string xml = string.Empty;
            try
            {
                if (isComposite == true)
                {
                    if (isECC == true)
                    {
                        serviceOperationUpdate = ServiceOperations.ECC_Comp_Update_WO.ToString();
                    }
                    else
                    {
                        if (withSpecification == true)
                        {
                            serviceOperationUpdate = ServiceOperations.BAU_Comp_Update_Rain_WO.ToString();
                        }
                        else
                        {
                            serviceOperationUpdate = ServiceOperations.BAU_Comp_Update_Non_Rain_WO.ToString();
                        }
                    }
                }
                else
                {
                    if (isECC == true)
                    {
                        serviceOperationUpdate = ServiceOperations.ECC_Update_WO.ToString();
                    }
                    else
                    {
                        if (withSpecification == true)
                        {
                            serviceOperationUpdate = ServiceOperations.BAU_Update_Rain_WO.ToString();
                        }
                        else
                        {
                            serviceOperationUpdate = ServiceOperations.BAU_Update_Non_Rain_WO.ToString();
                        }
                    }
                }

                WONUM = String.IsNullOrEmpty(WONUM) ? pWorkOrder.WONUM : WONUM;
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCUpdateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                if (!ReturnToSch)
                    xml = new GenerateXML().GenerateWOUpdateXML(pWorkOrder, ExternalRefID, WONUM, isECC);
                else
                    xml = new GenerateXML().GenerateReturnWOXML(pWorkOrder, ExternalRefID, WONUM, isECC);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate, WONUM);
                recID = insert.Item2;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        isSuccess = true;
                        res = "Work Order: " + WONUM + " updated successfully.";
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                    }

                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationUpdate, pWorkOrder.WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }
            /*catch (WebException wex)
            {
                if (wex.Message.Contains("Unable to connect"))
                {
                    res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                    log.Info(serviceOperationUpdate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                }
                else
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resp);
                    XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    res = "<b> WO update error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                    log.Info(serviceOperationUpdate + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(resp, jsonText, WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                }
            }
            catch (Exception exp)
            {
                res = exp.Message;
                log.Info(serviceOperationUpdate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
            }*/
            var result = new Tuple<bool, string>(isSuccess, "Update WO: " + res);
            return result;
        }

        internal Tuple<bool, string> Update_ECC_WorkOrder(WorkOrder pWorkOrder, string WONUM)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationUpdate = "";
            string xml = string.Empty;
            try
            {
                serviceOperationUpdate = ServiceOperations.ECC_Comp_Update_WO.ToString();

                WONUM = String.IsNullOrEmpty(WONUM) ? pWorkOrder.WONUM : WONUM;
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCUpdateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().Generate_ECC_WOUpdateXML(pWorkOrder, ExternalRefID, WONUM);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate, WONUM);
                recID = insert.Item2;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        isSuccess = true;
                        res = "Work Order: " + WONUM + " updated successfully.";
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                    }

                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationUpdate, pWorkOrder.WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }
            /*catch (WebException wex)
            {
                if (wex.Message.Contains("Unable to connect"))
                {
                    res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                    log.Info(serviceOperationUpdate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                }
                else
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resp);
                    XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    res = "<b> WO update error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                    log.Info(serviceOperationUpdate + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(resp, jsonText, WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                }
            }
            catch (Exception exp)
            {
                res = exp.Message;
                log.Info(serviceOperationUpdate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
            }*/
            var result = new Tuple<bool, string>(isSuccess, "Update WO: " + res);
            return result;
        }
        #endregion UpdateWO

        #region Return Scheduler
        internal Tuple<bool, string> Return_Update_ECC_WorkOrder(WorkOrder pWorkOrder, string WONUM)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationUpdate = "";
            string xml = string.Empty;
            try
            {
                serviceOperationUpdate = ServiceOperations.ECC_Comp_Update_WO.ToString();

                WONUM = String.IsNullOrEmpty(WONUM) ? pWorkOrder.WONUM : WONUM;
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCUpdateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().Generate_ECC_ReturnWOXML(pWorkOrder, ExternalRefID, WONUM);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate, WONUM);
                recID = insert.Item2;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        isSuccess = true;
                        res = "Work Order: " + WONUM + " updated successfully.";
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                    }

                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationUpdate, pWorkOrder.WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }
            /*catch (WebException wex)
            {
                if (wex.Message.Contains("Unable to connect"))
                {
                    res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                    log.Info(serviceOperationUpdate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                }
                else
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resp);
                    XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    res = "<b> WO update error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                    log.Info(serviceOperationUpdate + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(resp, jsonText, WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
                }
            }
            catch (Exception exp)
            {
                res = exp.Message;
                log.Info(serviceOperationUpdate + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationUpdate);
            }*/
            var result = new Tuple<bool, string>(isSuccess, "Update WO: " + res);
            return result;
        }

        #endregion
        #region DispatchWO
        internal Tuple<bool, string> DispatchWorkOrder(WorkOrder pWorkOrder, bool withSpecification = false, bool isECC = false)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationDispatch = "";
            string xml = string.Empty;

            try
            {
                if (isECC == true)
                {
                    serviceOperationDispatch = ServiceOperations.ECC_Dispatch_WO.ToString();
                }
                else
                {
                    if (withSpecification == true)
                    {
                        serviceOperationDispatch = ServiceOperations.BAU_Dispatch_Rain_WO.ToString();
                    }
                    else
                    {
                        serviceOperationDispatch = ServiceOperations.BAU_Dispatch_Non_Rain_WO.ToString();
                    }
                }
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCUpdateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().GenerateWODispatchXML(pWorkOrder, ExternalRefID, isECC);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
                recID = insert.Item2;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);


                    isSuccess = true;
                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        res = "Work Order <b>" + pWorkOrder.WONUM + "</b> dispatched successfully.";
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }
                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, pWorkOrder.WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
                    }

                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationDispatch, pWorkOrder.WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }
            /* catch (WebException wex)
             {
                 if (wex.Message.Contains("Unable to connect"))
                 {
                     res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                     log.Info(serviceOperationDispatch + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                     UpdateLogRequest(res, "", pWorkOrder.WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
                 }
                 else
                 {
                     var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                     XmlDocument xmlDoc = new XmlDocument();
                     xmlDoc.LoadXml(resp);
                     XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                     string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                     jsonText = JSONStringReplace(jsonText);

                     res = "<b> WO Dispatch error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                     log.Info(serviceOperationDispatch + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                     UpdateLogRequest(resp, jsonText, pWorkOrder.WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
                 }
             }
             catch (Exception exp)
             {
                 res = exp.Message;
                 log.Info(serviceOperationDispatch + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                 UpdateLogRequest(res, "", pWorkOrder.WONUM, recID, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
             }*/
            var result = new Tuple<bool, string>(isSuccess, "Dispatch WO: " + res);
            return result;
        }
        #endregion DispatchWO

        #region DispatchWO
        internal Tuple<bool, string> ECCDispatchWorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationDispatch = "";
            string xml = string.Empty;

            try
            {
                serviceOperationDispatch = ServiceOperations.ECC_Dispatch_WO.ToString();
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCUpdateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().Generate_ECC_WODispatchXML(pWorkOrder, ExternalRefID);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
                recID = insert.Item2;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);


                    isSuccess = true;
                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        res = "Work Order <b>" + pWorkOrder.WONUM + "</b> dispatched successfully.";
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }
                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, pWorkOrder.WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, serviceOperationDispatch);
                    }

                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationDispatch, pWorkOrder.WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }

            var result = new Tuple<bool, string>(isSuccess, "Dispatch WO: " + res);
            return result;
        }
        #endregion DispatchWO

        #region Duplicate Complaints
        internal Tuple<bool, string> ECCDDuplicateSR(ServiceRequest serviceRequest)
        {
            bool isSuccess = false;
            string res = "";
            string serviceOperationDispatch = "";
            string xml = string.Empty;

            try
            {
                serviceOperationDispatch = ServiceOperations.ECC_Duplicate_SR.ToString();
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCDuplicateSR"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.DuplicateSR.ToString());
                xml = new GenerateXML().GenerateDuplicateSRXML(serviceRequest, ExternalRefID);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.DuplicateSR, ExternalRefID, serviceOperationDispatch);
                recID = insert.Item2;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);


                    isSuccess = true;
                   
                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, serviceOperationDispatch, serviceRequest.EAMSSRNumber, RequestType.WorkOrder, ExternalRefID, xml);
            }

            var result = new Tuple<bool, string>(isSuccess, "Duplicate Assignment of SR: " + res);
            return result;
        }
        #endregion "Duplicat Complaints"

        #region CreateCMWO
        internal Tuple<bool, string> CreateCMWorkOrder(WorkOrder pWorkOrder)
        {
            bool isSuccess = false;
            string res = "";
            string xml = string.Empty;

            string WONUM = String.Empty;
            try
            {
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ECCCreateWOESB"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                ExternalRefID = new ServicesLogs().GetUniqueExternalRefID(RequestType.WorkOrder.ToString());
                xml = new GenerateXML().GenerateCMWOXML(pWorkOrder, ExternalRefID);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);


                var insert = LogRequest(xml, RequestType.WorkOrder, ExternalRefID, ServiceOperations.CM_Create_WO.ToString());
                recID = insert.Item2;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);
                    var updateResult = new Tuple<bool, string>(false, "");

                    if (xmlDoc.GetElementsByTagName("NS1:returnCodeDesc")[0].InnerText.Contains("Success"))
                    {
                        XmlNodeList node = xmlDoc.GetElementsByTagName("NS1:WorkOrderNumber");
                        if (node.Count > 0)
                        {
                            isSuccess = true;
                            WONUM = node[0].InnerText;
                            res = "Work Order created successfully. Your WONUM is: <b>" + WONUM + "</b>";
                        }
                        else
                        {
                            res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                        }
                    }
                    else
                    {
                        res = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider")[0].InnerText;
                    }

                    if (insert.Item2 > 0)
                    {
                        UpdateLogRequest(response, jsonText, WONUM, insert.Item2, RequestType.WorkOrder, ExternalRefID, ServiceOperations.CM_Create_WO.ToString());
                    }
                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, ServiceOperations.CM_Create_WO.ToString(), WONUM, RequestType.WorkOrder, ExternalRefID, xml);
            }
            /*{
                if (wex.Message.Contains("Unable to connect"))
                {
                    res = "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                    log.Error(ServiceOperations.CM_Create_WO.ToString() + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(res, "", WONUM, recID, RequestType.WorkOrder, ExternalRefID, ServiceOperations.CM_Create_WO.ToString());
                }
                else
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resp);
                    XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);

                    res = "<b> CM WO creation error: " + wex.Message + "<br/>Detail:" + error[0].InnerText + "</b>";
                    log.Error(ServiceOperations.CM_Create_WO.ToString() + " response: \n" + resp + "\nExternal Ref ID: " + ExternalRefID);
                    UpdateLogRequest(resp, jsonText, pWorkOrder.WONUM, recID, RequestType.WorkOrder, ExternalRefID, ServiceOperations.CM_Create_WO.ToString());

                }
            }
            catch (Exception exp)
            {
                res = exp.Message;
                log.Info(ServiceOperations.CM_Create_WO.ToString() + " response: \n" + res + "\nExternal Ref ID: " + ExternalRefID);
                UpdateLogRequest(res, "", pWorkOrder.WONUM, recID, RequestType.WorkOrder, ExternalRefID, ServiceOperations.CM_Create_WO.ToString());
            }*/
            var result = new Tuple<bool, string>(isSuccess, "Create CM WO: " + res);
            return result;
        }
        #endregion CreateCMWO

        #region Fetch WO Details
        /// <summary>
        /// Get the details of the WO
        /// </summary>
        /// <param name="wonum"></param>
        /// <returns></returns>
        internal string GetWOStatus(string wonum)
        {
            string res = "";
            string xml = string.Empty;


            try
            {
                string createUri = System.Web.Configuration.WebConfigurationManager.AppSettings["WODetail"];
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTHESB"];

                xml = new GenerateXML().GenerateWOFetchXML(wonum);

                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindBySubjectName, "DNMC Root CA", false)[0];
                certStore.Close();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["MAXAUTH"] = authValue;
                httpWebRequest.ClientCertificates.Add(cert);

                log.Info("IP: " + IP + "\n" + RequestType.WorkOrder.ToString() + " request: \n" + xml);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(xml.ToString());
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var response = readStream.ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);

                    string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                    jsonText = JSONStringReplace(jsonText);
                    var updateResult = new Tuple<bool, string>(false, "");

                    res = xmlDoc.GetElementsByTagName("NS1:STATUS")[0].InnerText;
                }
            }
            catch (Exception exp)
            {
                res = CatchException(exp, ServiceOperations.Fetch_WO.ToString(), wonum, RequestType.WorkOrder, ExternalRefID, xml);
            }

            return res;
        }
        #endregion Fetch WO Details

        #region "Catch Exception"
        private string CatchException(Exception wex, string oprs, string woSrId, RequestType requestType, string externalRefID = "", string xml = "")
        {
            string res = "IP: " + IP + "\nURL: " + System.Web.HttpContext.Current.Request.Url + "\n";
            try
            {
                log.Info("IP: " + IP + "\n" + wex.Message + "\nExternal Ref ID: " + ExternalRefID);
                if (wex is WebException)
                {
                    WebException webexp = (WebException)wex;
                    if (webexp.Message.Contains("Unable to connect"))
                    {
                        res += "<b> Communication Error: The connection failed to communicate with ESB server. </b>";
                        UpdateLogRequest(res, "", woSrId, recID, requestType, externalRefID, oprs);
                    }
                    else
                    {
                        var resp = new StreamReader(webexp.Response.GetResponseStream()).ReadToEnd();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(resp);
                        XmlNodeList error = xmlDoc.GetElementsByTagName("NS1:returnCodeDescProvider");
                        string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);

                        jsonText = JSONStringReplace(jsonText);
                        res += "Error Detail: " + error[0].InnerText;
                        UpdateLogRequest(resp, jsonText, woSrId, recID, requestType, externalRefID, oprs);
                    }
                }
                else
                {
                    res += wex.Message;
                    UpdateLogRequest(res, "", woSrId, recID, requestType, externalRefID, oprs);
                }
            }
            catch (Exception exp)
            {
                res += exp.Message;
                UpdateLogRequest(res, "", woSrId, recID, requestType, externalRefID, oprs);
            }

            log.Error("IP: " + IP + "\n" + oprs.ToString() + " response: \n" + res + "\nExternal Ref ID: " + externalRefID + "\nXML:\n" + xml);
            return res;
        }

        #endregion

        #region Logging
        private Tuple<bool, int> LogRequest(string xml, RequestType requestType, string ExternalRefID = "", string ServiceOperation = "", string WONUM = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            jsonText = jsonText.Replace("esb:", "").Replace(@"""?xml"":{""@version"":""1.0"",""@encoding"":""UTF-8""},", "")
                               .Replace(@"""@xmlns:esb"":""esb.ashghal.gov.qa"",", "").Replace("eAI_Header", "Header").Replace("eAI_Body", "Body");
            log.Info("IP: " + IP + "\n" + requestType.ToString() + " request: \n" + xml + "\nExternal Ref ID: " + ExternalRefID);
            ServicesLogs services = new ServicesLogs();
            if (requestType == RequestType.ServiceRequest || requestType == RequestType.DuplicateSR)
            {
                AuditServiceRequest auditServiceRequest = new AuditServiceRequest()
                {
                    RequestPayLoad = jsonText,
                    XmlRequest = xml,
                    ExternalCRMsrId = ExternalRefID,
                    ServiceOperation = ServiceOperation,
                };
                return services.InsertServiceRequestAudit(auditServiceRequest);
            }
            else
            {
                AuditWORequest auditWORequest = new AuditWORequest()
                {
                    RequestPayLoad = jsonText,
                    XmlRequest = xml,
                    ExternalRefID = ExternalRefID,
                    ServiceOperation = ServiceOperation,
                    UniqueId_WONumber = WONUM
                };
                return services.InsertWORequestAudit(auditWORequest);
            }
        }
        private Tuple<bool, int> UpdateLogRequest(string xml, string jsonText, string WONUM, int ID, RequestType requestType, string ExternalRefID, string ServiceOperation)
        {
            ServicesLogs services = new ServicesLogs();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            log.Info("IP: " + IP + "\n" + requestType.ToString() + " response: \n" + doc.InnerXml + "\nExternal Ref ID: " + ExternalRefID);

            if (requestType == RequestType.ServiceRequest)
            {
                AuditServiceRequest auditServiceRequest = new AuditServiceRequest()
                {
                    ResponsePayload = jsonText,
                    XmlResponse = xml,
                    IncidentUID = WONUM,
                    ExternalCRMsrId = ExternalRefID,
                    ServiceOperation = ServiceOperation
                };
                return services.InsertServiceRequestAudit(auditServiceRequest, ID);
            }
            else
            {
                AuditWORequest auditWORequest = new AuditWORequest()
                {
                    ResponsePayload = jsonText,
                    XmlResponse = xml,
                    UniqueId_WONumber = WONUM,
                    ExternalRefID = ExternalRefID,
                    ServiceOperation = ServiceOperation
                };
                return services.InsertWORequestAudit(auditWORequest, ID);
            }
        }
        #endregion Logging

        public string JSONStringReplace(string jsonText)
        {
            return jsonText.Replace("NS1:", "").Replace(@"""?xml"":{""@version"":""1.0"",""@encoding"":""UTF-8""},", "")
                                   .Replace(@"""@xmlns:NS1"":""esb.ashghal.gov.qa"",", "").Replace("eAI_Header", "Header").Replace("eAI_STATUS", "STATUS");
        }

        enum RequestType
        {
            WorkOrder,
            ServiceRequest,
            DuplicateSR
        }
    }


}