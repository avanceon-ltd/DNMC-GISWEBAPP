using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using WebAppForm.App_Code;
using WebAppForm.BusinessEntity;
using WebAppForm.DAL;
using WebAppForm.ExtensionEntity;

namespace WebAppForm.BusinessLogic
{
    public class GenerateXML
    {
        string xmlDeclarationString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        public String THRDP { get { return ConfigurationManager.AppSettings["THRDP"]; } }
        public String PROP_TYPE { get { return ConfigurationManager.AppSettings["PROPERTY_TYPE"]; } }
        public String HOTSPOT { get { return ConfigurationManager.AppSettings["HOTSPOT_NUMBER"]; } }
        public String PROJ_ID { get { return ConfigurationManager.AppSettings["PROJECT_ID"]; } }
        public String ISUPRELATED { get { return ConfigurationManager.AppSettings["ISUPRELATED"]; } }
        public String SEVERITY { get { return ConfigurationManager.AppSettings["SEVERITY"]; } }
        internal string GenerateWOXML(WorkOrder pWorkOrder, string ExternalRefID, bool withSpecification, int isRainWater)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "CreateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("AssetNumber", pWorkOrder.Asset);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("RNWFLOOD", "5101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalContractID", pWorkOrder.ContractId);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalEngineer", pWorkOrder.Engineer);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalFrameZone", pWorkOrder.ExternalFrameZone);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Location", pWorkOrder.Location);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportDate", pWorkOrder.ReportedDate);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportedBy", pWorkOrder.ReportedBy);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "WAPPR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetCompletionDate", pWorkOrder.TargetFinish);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetStartDate", pWorkOrder.TargetStart);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OnBehalfOf", pWorkOrder.OnBehalfOf);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCSRGroup", "DR_CSRL2");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDefectType", pWorkOrder.ExternalDefectType);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.ExternalCRMSRID);
            eleWorkOrder.AppendChild(eleBody);

            if (string.IsNullOrEmpty(pWorkOrder.ClassStructID))
            {
                eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", "20089");
                eleWorkOrder.AppendChild(eleBody);
            }
            else
            {
                eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", pWorkOrder.ClassStructID);
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", "3");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            XmlElement eleServiceAddress = xmlRequest.CreateElement(prefix, "WorkOrderServiceAddress", ns);
            eleWorkOrder.AppendChild(eleServiceAddress);

            eleBody = xmlRequest.AddNodeWithValue("AddressLine3", pWorkOrder.BuildingNumber);
            eleServiceAddress.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("County", pWorkOrder.Zone);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("LatitudeY", pWorkOrder.Latitude);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("LongitudeX", pWorkOrder.Longitude);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("RegionDistrict", pWorkOrder.District);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("StreetAddress", pWorkOrder.Street);
            
            eleServiceAddress.AppendChild(eleBody);

            if (withSpecification)
            {
                ServicesLogs servicesLogs = new ServicesLogs();
                List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification();

                foreach (ClassSpecification classSpecification in classSpecificationLst)
                {
                    XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                    eleWorkOrder.AppendChild(eleSpec);
                    XmlElement spec;
                    if (classSpecification.ASSETATTRID.Equals(this.THRDP) && !String.IsNullOrEmpty(pWorkOrder.ECCOwnership))
                        classSpecification.ALNValue = pWorkOrder.ECCOwnership;

                    if (classSpecification.ASSETATTRID.Equals(this.PROP_TYPE) && !String.IsNullOrEmpty(pWorkOrder.ECCPriority))
                        classSpecification.ALNValue = pWorkOrder.ECCPriority;

                    if (classSpecification.ASSETATTRID.Equals(this.ISUPRELATED))
                        classSpecification.ALNValue = pWorkOrder.UnderpassRelated;

                    if (classSpecification.ASSETATTRID.Equals(this.HOTSPOT))
                        classSpecification.ALNValue = pWorkOrder.Hotspot;

                    if (classSpecification.ASSETATTRID.Equals(this.SEVERITY))
                        classSpecification.ALNValue = pWorkOrder.Severity;

                    classSpecification.ALNValue = isRainWater == 0 ? "NULL" : classSpecification.ALNValue;
                    spec = xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("DisplaySequence", classSpecification.SEQ);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                    eleSpec.AppendChild(spec);
                }

            }

            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }
        internal string Generate_ECC_WOXML(WorkOrder pWorkOrder, string ExternalRefID)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "CreateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("AssetNumber", pWorkOrder.Asset);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalContractID", pWorkOrder.ContractId);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalEngineer", pWorkOrder.Engineer);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalFrameZone", pWorkOrder.ExternalFrameZone);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Location", pWorkOrder.Location);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportDate", pWorkOrder.ReportedDate);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportedBy", pWorkOrder.ReportedBy);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "WAPPR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetCompletionDate", pWorkOrder.TargetFinish);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetStartDate", pWorkOrder.TargetStart);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OnBehalfOf", pWorkOrder.OnBehalfOf);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCSRGroup", "DR_CSRL2");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDefectType", pWorkOrder.ExternalDefectType);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.ExternalCRMSRID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", ConfigurationManager.AppSettings["ClassStructureID"]);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", pWorkOrder.InternalPriority.ToString());
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            XmlElement eleServiceAddress = xmlRequest.CreateElement(prefix, "WorkOrderServiceAddress", ns);
            eleWorkOrder.AppendChild(eleServiceAddress);

            eleBody = xmlRequest.AddNodeWithValue("AddressLine3", pWorkOrder.BuildingNumber);
            eleServiceAddress.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("County", pWorkOrder.Zone);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("LatitudeY", pWorkOrder.Latitude);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("LongitudeX", pWorkOrder.Longitude);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("RegionDistrict", pWorkOrder.District);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("StreetAddress", pWorkOrder.Street);
            eleServiceAddress.AppendChild(eleBody);

            ServicesLogs servicesLogs = new ServicesLogs();
            List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification();

            foreach (ClassSpecification classSpecification in classSpecificationLst)
            {
                XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                eleWorkOrder.AppendChild(eleSpec);
                XmlElement spec;

                if (classSpecification.ASSETATTRIDODW.ToString().Equals("SGW_NETWRK_AVL"))
                {

                    spec = pWorkOrder.SGWAvailability.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.SGWAvailability);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("SGW_NETWRK_TYPE"))
                {
                    spec = pWorkOrder.SGWType.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.SGWType);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("ROOD_CLASS"))
                {
                    spec = pWorkOrder.RoadClass.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RoadClass);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("EVENT"))
                {
                    spec = pWorkOrder.Event.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Event);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("THRDP"))
                {
                    spec = pWorkOrder.ECCOwnership.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.ECCOwnership);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("PROJECT ID"))
                {
                    spec = pWorkOrder.Project.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Project);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("HOTSPOT NUMBER"))
                {
                    spec = pWorkOrder.Hotspot.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Hotspot);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("FLODLOC"))
                {
                    spec = pWorkOrder.FloodLoc.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.FloodLoc);
                    eleSpec.AppendChild(spec);
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("RESTRICTEDAREA"))
                {
                    spec = pWorkOrder.RESTRICTEDAREA.Equals("-1") ? xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue) :
                        xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RESTRICTEDAREA);
                    eleSpec.AppendChild(spec);
                }
                else
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue);
                    eleSpec.AppendChild(spec);
                }

                spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                eleSpec.AppendChild(spec);

                spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                eleSpec.AppendChild(spec);

                spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                eleSpec.AppendChild(spec);

            }

            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }
        internal string GenerateReturnWOXML(WorkOrder pWorkOrder, string ExternalRefID, string WONUM, bool isECC)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "UpdateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrderNumber = xmlRequest.CreateElement(prefix, "WorkOrderNumber", ns);
            eleWorkOrderNumber.InnerText = WONUM;
            eleHeader.AppendChild(eleWorkOrderNumber);


            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            if (pWorkOrder.ECCOwnership != "CFZ North" && pWorkOrder.ECCOwnership != "CFZ South" && pWorkOrder.ECCOwnership != "CFZ West")
            {
                if (string.IsNullOrEmpty(pWorkOrder.ContractId) || pWorkOrder.ContractId.Length == 0)
                {
                    pWorkOrder.ContractId = "NULL";
                }
            }

            eleBody = xmlRequest.AddNodeWithValue("ExternalContractID", pWorkOrder.ContractId);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCrewLead", pWorkOrder.CrewLead);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
            eleWorkOrder.AppendChild(eleBody);



            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "DISPATCH");
            eleWorkOrder.AppendChild(eleBody);



            eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);



            eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.ExternalCRMSRID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", "20089");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("RNWFLOOD", "5101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            string assetattrid = this.THRDP + "," + this.PROP_TYPE;
            ServicesLogs servicesLogs = new ServicesLogs();
            List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification(assetattrid);
            foreach (ClassSpecification classSpecification in classSpecificationLst)
            {
                XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                eleWorkOrder.AppendChild(eleSpec);
                XmlElement spec;


                if (classSpecification.ASSETATTRID.Equals(this.THRDP) && !String.IsNullOrEmpty(pWorkOrder.ECCOwnership))
                    classSpecification.ALNValue = pWorkOrder.ECCOwnership;

                if (classSpecification.ASSETATTRID.Equals(this.PROP_TYPE) && !String.IsNullOrEmpty(pWorkOrder.ECCPriority))
                    classSpecification.ALNValue = pWorkOrder.ECCPriority;


                spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                eleSpec.AppendChild(spec);

                spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                eleSpec.AppendChild(spec);

                spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                eleSpec.AppendChild(spec);

            }

            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();

        }
        internal string Generate_ECC_ReturnWOXML(WorkOrder pWorkOrder, string ExternalRefID, string WONUM)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "UpdateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrderNumber = xmlRequest.CreateElement(prefix, "WorkOrderNumber", ns);
            eleWorkOrderNumber.InnerText = WONUM;
            eleHeader.AppendChild(eleWorkOrderNumber);


            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            if (pWorkOrder.ECCOwnership != "CFZ North" && pWorkOrder.ECCOwnership != "CFZ South" && pWorkOrder.ECCOwnership != "CFZ West")
            {
                if (string.IsNullOrEmpty(pWorkOrder.ContractId) || pWorkOrder.ContractId.Length == 0)
                {
                    pWorkOrder.ContractId = "NULL";
                }
            }

            eleBody = xmlRequest.AddNodeWithValue("ExternalContractID", pWorkOrder.ContractId);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCrewLead", pWorkOrder.CrewLead);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
            eleWorkOrder.AppendChild(eleBody);



            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "DISPATCH");
            eleWorkOrder.AppendChild(eleBody);



            eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);



            eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.ExternalCRMSRID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", ConfigurationManager.AppSettings["ClassStructureID"]);
            eleWorkOrder.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", pWorkOrder.InternalPriority.ToString());
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            ServicesLogs servicesLogs = new ServicesLogs();
            List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification("");
            foreach (ClassSpecification classSpecification in classSpecificationLst)
            {
                XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                eleWorkOrder.AppendChild(eleSpec);
                XmlElement spec;
                bool isSpecification = false;

                if (classSpecification.ASSETATTRIDODW.ToString().Equals("THRDP"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.ECCOwnership);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("SGW_NETWRK_AVL"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.SGWAvailability);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("SGW_NETWRK_TYPE"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.SGWType);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("ROOD_CLASS"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RoadClass);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("EVENT"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Event);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("FLODLOC"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.FloodLoc);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("RESTRICTEDAREA"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RESTRICTEDAREA);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (isSpecification)
                {
                    spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                    eleSpec.AppendChild(spec);
                }
            }

            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();

        }
        internal string GenerateWOUpdateXML(WorkOrder pWorkOrder, string ExternalRefID, string WONUM, bool isECC)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "UpdateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrderNumber = xmlRequest.CreateElement(prefix, "WorkOrderNumber", ns);
            eleWorkOrderNumber.InnerText = WONUM;
            eleHeader.AppendChild(eleWorkOrderNumber);


            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);


            if (pWorkOrder.ECCOwnership != "CFZ North" && pWorkOrder.ECCOwnership != "CFZ South" && pWorkOrder.ECCOwnership != "CFZ West")
            {
                if (string.IsNullOrEmpty(pWorkOrder.ContractId) || pWorkOrder.ContractId.Length == 0)
                {
                    pWorkOrder.ContractId = "NULL";
                }
            }

            eleBody = xmlRequest.AddNodeWithValue("ExternalContractID", pWorkOrder.ContractId);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalEngineer", pWorkOrder.Engineer);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalFrameZone", pWorkOrder.ExternalFrameZone);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Location", pWorkOrder.Location); // "QATAR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportDate", pWorkOrder.ReportedDate);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportedBy", pWorkOrder.ReportedBy);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "APPR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetCompletionDate", pWorkOrder.TargetFinish);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetStartDate", pWorkOrder.TargetStart);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OnBehalfOf", pWorkOrder.OnBehalfOf);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCSRGroup", "DR_CSRL2");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDefectType", pWorkOrder.ExternalDefectType);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.ExternalCRMSRID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", "20089");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", "3");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            if (isECC)
            {
                string assetattrid = this.THRDP + "," + this.PROP_TYPE + "," + this.ISUPRELATED + "," + this.SEVERITY;
                ServicesLogs servicesLogs = new ServicesLogs();
                List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification(assetattrid);
                foreach (ClassSpecification classSpecification in classSpecificationLst)
                {
                    XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                    eleWorkOrder.AppendChild(eleSpec);
                    XmlElement spec;

                    if (classSpecification.ASSETATTRID.Equals(this.THRDP) && !String.IsNullOrEmpty(pWorkOrder.ECCOwnership))
                        classSpecification.ALNValue = pWorkOrder.ECCOwnership;

                    if (classSpecification.ASSETATTRID.Equals(this.PROP_TYPE) && !String.IsNullOrEmpty(pWorkOrder.ECCPriority))
                        classSpecification.ALNValue = pWorkOrder.ECCPriority;

                    if (classSpecification.ASSETATTRID.Equals(this.ISUPRELATED))
                        classSpecification.ALNValue = pWorkOrder.UnderpassRelated;

                    if (classSpecification.ASSETATTRID.Equals(this.SEVERITY))
                        classSpecification.ALNValue = pWorkOrder.Severity;

                    spec = xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("DisplaySequence", classSpecification.SEQ);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                    eleSpec.AppendChild(spec);
                }
            }

            xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
            return xml;
        }
        internal string Generate_ECC_WOUpdateXML(WorkOrder pWorkOrder, string ExternalRefID, string WONUM)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "UpdateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrderNumber = xmlRequest.CreateElement(prefix, "WorkOrderNumber", ns);
            eleWorkOrderNumber.InnerText = WONUM;
            eleHeader.AppendChild(eleWorkOrderNumber);


            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);


            if (pWorkOrder.ECCOwnership != "CFZ North" && pWorkOrder.ECCOwnership != "CFZ South" && pWorkOrder.ECCOwnership != "CFZ West")
            {
                if (string.IsNullOrEmpty(pWorkOrder.ContractId) || pWorkOrder.ContractId.Length == 0)
                {
                    pWorkOrder.ContractId = "NULL";
                }
            }

            eleBody = xmlRequest.AddNodeWithValue("ExternalContractID", pWorkOrder.ContractId);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalEngineer", pWorkOrder.Engineer);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalFrameZone", pWorkOrder.ExternalFrameZone);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Location", pWorkOrder.Location); // "QATAR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportDate", pWorkOrder.ReportedDate);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportedBy", pWorkOrder.ReportedBy);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "APPR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetCompletionDate", pWorkOrder.TargetFinish);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetStartDate", pWorkOrder.TargetStart);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("OnBehalfOf", pWorkOrder.OnBehalfOf);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCSRGroup", "DR_CSRL2");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDefectType", pWorkOrder.ExternalDefectType);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.ExternalCRMSRID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", ConfigurationManager.AppSettings["ClassStructureID"]);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", pWorkOrder.InternalPriority.ToString());
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            ServicesLogs servicesLogs = new ServicesLogs();
            List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification();

            foreach (ClassSpecification classSpecification in classSpecificationLst)
            {
                XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                eleWorkOrder.AppendChild(eleSpec);
                bool isSpecification = false;
                XmlElement spec;

                if (classSpecification.ASSETATTRIDODW.ToString().Equals("THRDP"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.ECCOwnership);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;
                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("RESTRICTEDAREA"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RESTRICTEDAREA);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (isSpecification)
                {
                    spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                    eleSpec.AppendChild(spec);

                    //spec = xmlRequest.AddNodeWithValue("DisplaySequence", classSpecification.SEQ);
                    //eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                    eleSpec.AppendChild(spec);
                }
            }

            xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
            return xml;
        }

        internal string GenerateWODispatchXML(WorkOrder pWorkOrder, string ExternalRefID, bool isECC)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "UpdateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrderNumber = xmlRequest.CreateElement(prefix, "WorkOrderNumber", ns);
            eleWorkOrderNumber.InnerText = pWorkOrder.WONUM;
            eleHeader.AppendChild(eleWorkOrderNumber);


            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCrewLead", pWorkOrder.CrewLead);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
            {
                eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("ScheduledFinish", pWorkOrder.ScheduleFinish);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ScheduledStart", pWorkOrder.ScheduleStart);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "DISPATCH");
            eleWorkOrder.AppendChild(eleBody);

            if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
            {
                eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
                eleWorkOrder.AppendChild(eleBody);

                eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
                eleWorkOrder.AppendChild(eleBody);

                eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
            {
                eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.CRMSID);
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", "20089");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            if (isECC)
            {
                string assetattrid = this.THRDP + "," + this.PROP_TYPE + "," + this.HOTSPOT + "," + this.PROJ_ID;

                ServicesLogs servicesLogs = new ServicesLogs();
                List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification(assetattrid);
                foreach (ClassSpecification classSpecification in classSpecificationLst)
                {
                    XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                    eleWorkOrder.AppendChild(eleSpec);
                    XmlElement spec;

                    if (classSpecification.ASSETATTRID.Equals(this.THRDP) && !String.IsNullOrEmpty(pWorkOrder.ECCOwnership))
                        classSpecification.ALNValue = pWorkOrder.ECCOwnership;

                    if (classSpecification.ASSETATTRID.Equals(this.PROP_TYPE) && !String.IsNullOrEmpty(pWorkOrder.ECCPriority))
                        classSpecification.ALNValue = pWorkOrder.ECCPriority;

                    if (classSpecification.ASSETATTRID.Equals(this.HOTSPOT))
                        classSpecification.ALNValue = pWorkOrder.Hotspot;

                    if (classSpecification.ASSETATTRID.Equals(this.PROJ_ID))
                        classSpecification.ALNValue = pWorkOrder.Project;

                    spec = xmlRequest.AddNodeWithValue("ALNValue", classSpecification.ALNValue);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("DisplaySequence", classSpecification.SEQ);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                    eleSpec.AppendChild(spec);
                }
            }


            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }
        internal string Generate_ECC_WODispatchXML(WorkOrder pWorkOrder, string ExternalRefID)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "UpdateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrderNumber = xmlRequest.CreateElement(prefix, "WorkOrderNumber", ns);
            eleWorkOrderNumber.InnerText = pWorkOrder.WONUM;
            eleHeader.AppendChild(eleWorkOrderNumber);


            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCrewLead", pWorkOrder.CrewLead);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalScheduler", pWorkOrder.Scheduler);
            eleWorkOrder.AppendChild(eleBody);

            if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
            {
                eleBody = xmlRequest.AddNodeWithValue("ExternalSource", "CRM");
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "DISPATCH");
            eleWorkOrder.AppendChild(eleBody);

            if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
            {
                eleBody = xmlRequest.AddNodeWithValue("OrigRecordID", pWorkOrder.EAMSSRNumber);
                eleWorkOrder.AppendChild(eleBody);

                eleBody = xmlRequest.AddNodeWithValue("OrigRecordClass", "SR");
                eleWorkOrder.AppendChild(eleBody);

                eleBody = xmlRequest.AddNodeWithValue("OrigTKID", pWorkOrder.EAMSSRNumber);
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            if (!String.IsNullOrEmpty(pWorkOrder.EAMSSRNumber))
            {
                eleBody = xmlRequest.AddNodeWithValue("ExternalCRMSRID", pWorkOrder.CRMSID);
                eleWorkOrder.AppendChild(eleBody);
            }

            eleBody = xmlRequest.AddNodeWithValue("ClassStructureID", ConfigurationManager.AppSettings["ClassStructureID"]);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", pWorkOrder.InternalPriority.ToString());
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "FR");
            eleWorkOrder.AppendChild(eleBody);

            ServicesLogs servicesLogs = new ServicesLogs();
            List<ClassSpecification> classSpecificationLst = servicesLogs.GetClassSpecification();

            foreach (ClassSpecification classSpecification in classSpecificationLst)
            {
                XmlElement eleSpec = xmlRequest.CreateElement(prefix, "WorkOrderSpec", ns);
                eleWorkOrder.AppendChild(eleSpec);
                bool isSpecification = false;
                XmlElement spec;

                if (classSpecification.ASSETATTRIDODW.ToString().Equals("HOTSPOT NUMBER"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Hotspot);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("PROJECT ID"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Project);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("SGW_NETWRK_AVL"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.SGWAvailability);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("SGW_NETWRK_TYPE"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.SGWType);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("ROOD_CLASS"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RoadClass);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("EVENT"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.Event);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("FLODLOC"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.FloodLoc);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (classSpecification.ASSETATTRIDODW.ToString().Equals("RESTRICTEDAREA"))
                {
                    spec = xmlRequest.AddNodeWithValue("ALNValue", pWorkOrder.RESTRICTEDAREA);
                    eleSpec.AppendChild(spec);
                    isSpecification = true;

                }
                if (isSpecification)
                {
                    spec = xmlRequest.AddNodeWithValue("ClassSpecID", classSpecification.CLASSSPECID);
                    eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("ClassStructureId", classSpecification.ClassStructureId);
                    eleSpec.AppendChild(spec);

                    //spec = xmlRequest.AddNodeWithValue("DisplaySequence", classSpecification.SEQ);
                    //eleSpec.AppendChild(spec);

                    spec = xmlRequest.AddNodeWithValue("AssetAttrId", classSpecification.ASSETATTRID);
                    eleSpec.AppendChild(spec);
                }

            }

            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }
        internal string GenerateCMWOXML(WorkOrder pWorkOrder, string ExternalRefID)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "CreateWorkOrderRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_HEADER", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_BODY", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleWorkOrder = xmlRequest.CreateElement(prefix, "WorkOrder", ns);
            eleHeader.AppendChild(eleWorkOrder);

            XmlElement eleBody = xmlRequest.AddNodeWithValue("AssetNumber", pWorkOrder.Asset);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Description", pWorkOrder.Description);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalRefID", ExternalRefID);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDepartment", "DRAINAGE");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalEngineer", pWorkOrder.Engineer);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Location", pWorkOrder.Location);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportDate", pWorkOrder.ReportedDate);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ReportedBy", pWorkOrder.ReportedBy);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("SiteID", "101");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("Status", "WAPPR");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetCompletionDate", pWorkOrder.TargetFinish);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("TargetStartDate", pWorkOrder.TargetStart);
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalCSRGroup", "DR_CSRL2");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("ExternalDNMC", "DNMC");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkOrderPriority", "3");
            eleWorkOrder.AppendChild(eleBody);

            eleBody = xmlRequest.AddNodeWithValue("WorkType", "CM");
            eleWorkOrder.AppendChild(eleBody);

            XmlElement eleServiceAddress = xmlRequest.CreateElement(prefix, "WorkOrderServiceAddress", ns);
            eleWorkOrder.AppendChild(eleServiceAddress);

            eleBody = xmlRequest.AddNodeWithValue("LatitudeY", pWorkOrder.Latitude);
            eleServiceAddress.AppendChild(eleBody);


            eleBody = xmlRequest.AddNodeWithValue("LongitudeX", pWorkOrder.Longitude);
            eleServiceAddress.AppendChild(eleBody);

            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }

        internal string GenerateSRXML(ServiceRequest serviceRequest, string ExternalRecId)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "esb.ashghal.gov.qa";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "CreateServiceRequestRequest", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);
            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "eAI_Header", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleReferenceNum = xmlRequest.AddNodeWithValue("referenceNum", "a");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("clientChannel", "DSS");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("requestTime", DateTime.Now.ToString("s") + "Z");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("retryFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.AddNodeWithValue("sucessfulReversalFlag", "N");
            eleHeader.AppendChild(eleReferenceNum);

            eleHeader = xmlRequest.CreateElement(prefix, "eAI_Body", ns);
            rootElement.AppendChild(eleHeader);

            XmlElement eleServiceRequest = xmlRequest.CreateElement(prefix, "ServiceRequest", ns);
            eleHeader.AppendChild(eleServiceRequest);

            XmlElement eleServiceRequestCore = xmlRequest.CreateElement(prefix, "ServiceRequestCore", ns);
            eleServiceRequest.AppendChild(eleServiceRequestCore);

            XmlElement eleBody = xmlRequest.CreateElement(prefix, "AssetNumber", ns);
            eleBody.InnerText = serviceRequest.Asset;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ShortDescripton", ns);
            eleBody.InnerText = serviceRequest.Summary;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "LongDescription", ns);
            eleBody.InnerText = serviceRequest.Description;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalRecId", ns);
            eleBody.InnerText = ExternalRecId;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalDepartment", ns);
            eleBody.InnerText = "DRAINAGE";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalFrameZone", ns);
            eleBody.InnerText = serviceRequest.ExternalFrameZone;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalLattitudeX", ns);
            eleBody.InnerText = "0.0";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalLongittudeY", ns);
            eleBody.InnerText = "0.0";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalSoruce", ns);
            eleBody.InnerText = "CRM";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalSRGroup", ns);
            eleBody.InnerText = serviceRequest.ExternalSRGroup;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ExternalSRType", ns);
            eleBody.InnerText = serviceRequest.ExternalSRType;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "GlAccount", ns);
            eleBody.InnerText = "";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "InternalPriority", ns);
            eleBody.InnerText = serviceRequest.InternalPriority;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "IssueLocation", ns);
            eleBody.InnerText = serviceRequest.IssueLocation;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ReportDate", ns);
            eleBody.InnerText = serviceRequest.ReportedDate;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "ReportedBy", ns);
            eleBody.InnerText = serviceRequest.ReportedBy;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "PriorityOfIssueReported", ns);
            eleBody.InnerText = serviceRequest.PriorityOfIssueReported;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "SiteId", ns);
            eleBody.InnerText = "101";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "Source", ns);
            eleBody.InnerText = "DSS";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "Status", ns);
            eleBody.InnerText = "QUEUED";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "TargetFinish", ns);
            eleBody.InnerText = serviceRequest.TargetFinish;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "TargetStart", ns);
            eleBody.InnerText = serviceRequest.TargetStart;
            eleServiceRequestCore.AppendChild(eleBody);

            XmlElement eleServiceAddress = xmlRequest.CreateElement(prefix, "TKServiceAddress", ns);
            eleServiceRequestCore.AppendChild(eleServiceAddress);

            eleBody = xmlRequest.CreateElement(prefix, "LatitudeY", ns);
            eleBody.InnerText = serviceRequest.Latitude;
            eleServiceAddress.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "LongitudeX", ns);
            eleBody.InnerText = serviceRequest.Longitude;
            eleServiceAddress.AppendChild(eleBody);


            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }


        internal string GenerateDuplicateSRXML(ServiceRequest serviceRequest, string ExternalRecId)
        {
            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "http://www.ibm.com/maximo";
            string prefix = "esb";

            XmlElement rootElement = xmlRequest.CreateElement(prefix, "SyncEXT_SRDUP", ns);
            XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
            nsAttribute.Value = ns;
            rootElement.Attributes.Append(nsAttribute);

            nsAttribute = xmlRequest.CreateAttribute("baseLanguage");
            nsAttribute.Value = "EN";
            rootElement.Attributes.Append(nsAttribute);

            nsAttribute = xmlRequest.CreateAttribute("creationDateTime");
            nsAttribute.Value = DateTime.Now.ToString("s");
            rootElement.Attributes.Append(nsAttribute);

            nsAttribute = xmlRequest.CreateAttribute("maximoVersion");
            nsAttribute.Value = "7 6 20141117-2230 V7600-218";
            rootElement.Attributes.Append(nsAttribute);

            nsAttribute = xmlRequest.CreateAttribute("messageID");
            nsAttribute.Value = "0";
            rootElement.Attributes.Append(nsAttribute);

            nsAttribute = xmlRequest.CreateAttribute("transLanguage");
            nsAttribute.Value = "EN";
            rootElement.Attributes.Append(nsAttribute);

            xmlRequest.AppendChild(rootElement);

            XmlElement eleHeader = xmlRequest.CreateElement(prefix, "EXT_SRDUPSet", ns);
            rootElement.AppendChild(eleHeader);



            XmlElement eleServiceRequestCore = xmlRequest.CreateElement(prefix, "SR", ns);
            eleHeader.AppendChild(eleServiceRequestCore);

            XmlElement eleBody = xmlRequest.CreateElement(prefix, "TICKETID", ns);
            eleBody.InnerText = serviceRequest.ChildTicketID;
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "EXT_DEPARTMENT", ns);
            eleBody.InnerText = "DRAINAGE";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "STATUS", ns);
            eleBody.InnerText = "DUPLICATE";
            eleServiceRequestCore.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "CLASS", ns);
            eleBody.InnerText = "SR";
            eleServiceRequestCore.AppendChild(eleBody);

            XmlElement eleServiceAddress = xmlRequest.CreateElement(prefix, "RELATEDRECORD", ns);
            eleServiceRequestCore.AppendChild(eleServiceAddress);

            eleBody = xmlRequest.CreateElement(prefix, "RELATEDRECCLASS", ns);
            eleBody.InnerText = "SR";
            eleServiceAddress.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "RELATEDRECKEY", ns);
            eleBody.InnerText = serviceRequest.ParentTicketID;
            eleServiceAddress.AppendChild(eleBody);

            eleServiceAddress = xmlRequest.CreateElement(prefix, "TKSTATUS", ns);
            eleServiceRequestCore.AppendChild(eleServiceAddress);

            eleBody = xmlRequest.CreateElement(prefix, "STATUS", ns);
            eleBody.InnerText = "DUPLICATE";
            eleServiceAddress.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "CHANGEBY", ns);
            eleBody.InnerText = "DNMC";
            eleServiceAddress.AppendChild(eleBody);

            eleBody = xmlRequest.CreateElement(prefix, "TKSTATUSID", ns);
            eleBody.InnerText = "-1";
            eleServiceAddress.AppendChild(eleBody);


            return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();
        }

        /// <summary>
        /// Created By: Yasir Miraj
        /// JIRA Ticket:  https://avanceon.atlassian.net/browse/ASM-1119 / https://avanceon.atlassian.net/browse/ASM-1052
        /// This method will generate the request of the WO XML to fetch the details of a WO
        /// </summary>
        /// <param name="wonum">Pass WONUM</param>
        /// <returns></returns>
        internal string GenerateWOFetchXML(string wonum)
        {
            //string xml = string.Empty;

            //XmlDocument xmlRequest = new XmlDocument();
            //XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            //xmlRequest.AppendChild(xmlDeclaration);

            //string ns = "http://pwa-crm.ashghal.gov.qa";

            //XmlElement rootElement = xmlRequest.CreateElement("GetWorkOrdersRequest");
            //XmlAttribute nsAttribute = xmlRequest.CreateAttribute("xmlns");
            //nsAttribute.Value = ns;
            //rootElement.Attributes.Append(nsAttribute);
            //xmlRequest.AppendChild(rootElement);

            //XmlElement eleReferenceNum = xmlRequest.CreateElement("", "Wonum", "");
            //eleReferenceNum.InnerText = wonum;
            //rootElement.AppendChild(eleReferenceNum);

            //eleReferenceNum = xmlRequest.CreateElement("", "SiteId", "");
            //eleReferenceNum.InnerText = "101";
            //rootElement.AppendChild(eleReferenceNum);


            //return xml = xmlDeclarationString + xmlRequest.RemoveEmptyNodes();

            string xml = string.Empty;

            XmlDocument xmlRequest = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlRequest.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlRequest.AppendChild(xmlDeclaration);

            string ns = "http://pwa-crm.ashghal.gov.qa";

            XmlElement rootElement = xmlRequest.CreateElement("GetWorkOrdersRequest", ns); // Use the constructor to specify the namespace
            xmlRequest.AppendChild(rootElement);

            XmlElement eleReferenceNum = xmlRequest.CreateElement("Wonum", ns); // Specify the namespace here too
            eleReferenceNum.InnerText = wonum;
            rootElement.AppendChild(eleReferenceNum);

            eleReferenceNum = xmlRequest.CreateElement("SiteId", ns); // Specify the namespace here too
            eleReferenceNum.InnerText = "101";
            rootElement.AppendChild(eleReferenceNum);

            // Convert the XmlDocument to a string
            xml = xmlRequest.OuterXml;

            return xml;
        }
    }
}