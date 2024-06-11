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
using System.Xml;
using WebAppForm.App_Code;

namespace WebAppForm
{
    public partial class UpdateWorkOrder : System.Web.UI.Page
    {
        String _WONUM = String.Empty;
        WorkOrder objWorkOrder;
        string ERROR_MESSAGE = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            //txtEngineer.Attributes.Add("readonly", "readonly");
            txtContractId.Attributes.Add("readonly", "readonly");
            txtVendor.Attributes.Add("readonly", "readonly");
            txtVendorName.Attributes.Add("readonly", "readonly");

            Page.DataBind();

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
                {
                    _WONUM = Convert.ToString(Request.QueryString["WONUM"]);
                    lblWONUM.Text = _WONUM;
                    FillOwners();
                    FillWODetail();
                    FillFrameworkZone();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ESBServices esbServices = new ESBServices();
                string wostatus = esbServices.GetWOStatus(Request.QueryString["WONUM"]);

                if (!(wostatus.ToLower().Equals("appr") || wostatus.ToLower().Equals("wappr")))
                {
                    btnSubmit.Enabled = false;
                    btnSubmit.ForeColor = System.Drawing.Color.Gray;
                    btnSubmit.BorderColor = System.Drawing.Color.Gray;

                    lblMessage.Text = "Cannot update the WO because WO is already DISPATCHED.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    return;
                }

                objWorkOrder = new WorkOrder();
                if (ViewState["UpdateWorkOrderObj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["UpdateWorkOrderObj"];
                }
                objWorkOrder.WONUM = lblWONUM.Text;
                objWorkOrder.Scheduler = Drpdwn_Scheduler.SelectedValue.Trim();
                objWorkOrder.ContractId = txtContractId.Text.Trim();
                objWorkOrder.Vendor = txtVendor.Text.Trim();
                objWorkOrder.InternalPriority = Convert.ToInt32(Convert.ToDouble(ddlPriority.SelectedValue));
                objWorkOrder.RESTRICTEDAREA = ddlRestrictedArea.SelectedValue;
                objWorkOrder.Zone = ddlFrameZone.SelectedValue;

                objWorkOrder.EAMSSRNumber = objWorkOrder.EAMSSRNumber;
                objWorkOrder.ExternalCRMSRID = objWorkOrder.CRMSID;
                objWorkOrder.Status = lblWOStatus.Text;
                objWorkOrder.ECCOwnership = ddlOwnership.SelectedValue;
                

                UpdateWO(objWorkOrder);
            }
        }

        void UpdateWO(WorkOrder pWorkOrder)
        {
            try
            {
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();

                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    //result = esbServices.UpdateWorkOrder(pWorkOrder, "", true, true);
                    result = esbServices.Update_ECC_WorkOrder(pWorkOrder, "");
                }
                else
                {
                    result = redHatervices.UpdateWorkOrderECC(pWorkOrder);
                }
                if (!string.IsNullOrEmpty(result.Item2))
                {
                    if (result.Item2.Contains("Error Detail:"))
                    {
                        ERROR_MESSAGE = result.Item2.Replace(" - ", "!").Split('!')[1];
                        if (!string.IsNullOrEmpty(ERROR_MESSAGE))
                        {
                            lblMessage.Text = ERROR_MESSAGE;
                        }
                        else
                        {
                            lblMessage.Text = result.Item2;
                        }
                    }
                    else
                        lblMessage.Text = result.Item2;
                }
                else
                    lblMessage.Text = result.Item2;
                //lblMessage.Text = result.Item2;
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
            Drpdwn_Scheduler.SelectedIndex = 0;
            txtVendor.Text = "";
            txtVendorName.Text = "";
            ddlOwnership.SelectedIndex = 0;
            ddlPriority.SelectedIndex = 0;
          //  txtEngineer.Text = "";

        }

        void FillOwners()
        {
            //string[] owners = ConfigurationManager.AppSettings["ECC-Owners"].Split(',');
            //ddlOwnership.Items.Clear();
            //ddlOwnership.Items.Add(new ListItem("- Select Ownership -", "-1"));
            //foreach (string owner in owners)
            //{
            //    ddlOwnership.Items.Add(new ListItem(owner, owner));
            //}
            ddlOwnership.Items.Clear();
            ddlOwnership.Items.Add(new ListItem("- Select Ownership -", "-1"));
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"Select OwnerValue  from Eams.WOOwners where IsECC = 1 order by SequenceId", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    ddlOwnership.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string owner = reader["OwnerValue"].ToString();
                            ddlOwnership.Items.Add(new ListItem(owner, owner));
                        }
                    }
                }
            }

            #region "Internal Priority"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.NumericDomain WHERE DOMAINID = 'EXT_PRIORITY'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlPriority.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlPriority.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), Convert.ToInt32(Convert.ToDouble(reader["VALUE"])).ToString()));
                        }
                    }
                }
            }
            #endregion
            #region "Restricted Area"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.ALNDomain WHERE DOMAINID = 'RESTRICTEDAREA'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlRestrictedArea.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlRestrictedArea.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }
                    }
                }
            }
            #endregion
        }

        private void SelectedOwner(string owner)
        {
            if (ddlOwnership.SelectedIndex == 0)
            {
                InitializeWorkFlowFields();
            }

            Drpdwn_Scheduler.Items.Clear();
            txtContractId.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                bool isFirst = false;
                string contractId = "";
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT PERSONID,TITLE,JOBCODE,JOBCODE_DESCRIPTION,DISPLAYNAME,EXT_CONTRACTNUM,SUPERVISOR FROM [EAMS].[vwDimPerson] WHERE STATUS = 'ACTIVE' AND JOBCODE = 'SCHEDULER' AND TITLE = '" + owner + "'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string displayName = reader["DISPLAYNAME"].ToString();
                            string personId = reader["PERSONID"].ToString();

                            Drpdwn_Scheduler.Items.Add(new ListItem(displayName, personId));
                            if (isFirst == false) //Only selected the first scheduler
                            {
                                Drpdwn_Scheduler.SelectedIndex = 0;
                                contractId = reader["EXT_CONTRACTNUM"].ToString();
                                txtContractId.Text = contractId;
                                //txtEngineer.Text = reader["SUPERVISOR"].ToString();
                                isFirst = true;
                            }
                        }
                    }
                }
                if (isFirst == true && !String.IsNullOrEmpty(contractId))
                {
                    cmd.CommandText = @"SELECT pv.[CONTRACTID],pv.[CONTRACTNUM],c.COMPANY,pv.DESCRIPTION,c.NAME,c.COMPANY_KEY 
                        FROM [ODW].[EAMS].[vwDimPurchView] pv, [ODW].[EAMS].[vwDimCompany] c 
                        where pv.STATUS ='APPR' and c.COMPANY_KEY=pv.VENDOR_KEY AND pv.CONTRACTNUM = '" + contractId + "'";
                    //and CONTRACTNUM in ('C/2020/47','C/2020/28','C/2020/50')"
                    using (SqlDataReader reader2 = cmd.ExecuteReader())
                    {
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                txtVendor.Text = reader2["COMPANY"].ToString();
                                txtVendorName.Text = reader2["NAME"].ToString();
                            }
                        }
                    }
                }
            }
        }


        void FillFrameworkZone()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM EAMS.vwAlnDomain WHERE DOMAINID = 'EXT_FRAMEZONE' AND EXT_DEPARTMENT = 'DRAINAGE'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    ddlFrameZone.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string frameZoneVal = reader["VALUE"].ToString();
                            ddlFrameZone.Items.Add(new ListItem(frameZoneVal, frameZoneVal));
                        }
                    }
                }


                if (!String.IsNullOrEmpty(lblEAMSSR.Text))
                {
                    cmd.CommandText = @"SELECT EXT_FRAMEZONE FROM EAMS.vwFactServiceRequest WHERE TICKETID ='" + lblEAMSSR.Text + "'";
                    //using (SqlCommand cmd2 = new SqlCommand(, con))
                    {
                        using (SqlDataReader reader2 = cmd.ExecuteReader())
                        {
                            if (reader2.HasRows)
                            {
                                while (reader2.Read())
                                {
                                    ddlFrameZone.SelectedValue = reader2["EXT_FRAMEZONE"].ToString();
                                }
                            }
                        }
                    }
                }
            }

            //FrameZoneSelected(ddlFrameZone.SelectedValue);
        }

        private void FillWODetail()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT OWNER,WOPRIORITY,VENDOR,ONBEHALFOF,EXT_SCHEDULER,EXT_ENGINEER,EXT_INSPECTOR,
                                    EXT_CREWLEAD,EXT_CONTRACTID,ORIGRECORDID,ORIGRECORDCLASS,EXT_CRMSRID,STATUS,EXT_FRAMEZONE, 
                                    (Select [NAME] FROM EAMS.DimCompany WHERE Company = w.VENDOR) AS VENDORNAME,
                                    (Select DESCRIPTION FROM EAMS.DimPurchView WHERE PURCHVIEW_KEY = w.PURCHVIEW_KEY) AS ContractDesc,
                                    (Select DISPLAYNAME FROM EAMS.DimPerson WHERE PERSON_KEY = w.ENGINEER_KEY) AS EngineerDesc,
									(Select ALNVALUE from EAMS.FRWorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'ISUPRELATED') ISUPRELATED,
									(Select ALNVALUE from EAMS.FRWorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'SEVERITY') SEVERITY,
                                    (Select ALNVALUE from EAMS.FRWorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'RESTRICTEDAREA') RESTRICTEDAREA  
                                    FROM [ODW].[EAMS].FRFactWorkOrder w WHERE WONUM = @WONUM", con))
                {
                    cmd.Parameters.AddWithValue("@WONUM", _WONUM);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objWorkOrder = new WorkOrder();
                            ddlOwnership.SelectedValue = reader["OWNER"].ToString();
                            ddlPriority.SelectedValue = reader["WOPRIORITY"].ToString();
                            ddlRestrictedArea.SelectedValue = reader["RESTRICTEDAREA"].ToString();
                            ddlUnderpassRelated.SelectedValue = reader["ISUPRELATED"].ToString();
                            ddlSeverity.SelectedValue = reader["SEVERITY"].ToString();

                            txtContractId.Text = reader["EXT_CONTRACTID"].ToString();
                            txtContractId.ToolTip = reader["ContractDesc"].ToString();
                            txtVendor.Text = reader["VENDOR"].ToString();
                            txtVendorName.Text = reader["VENDORNAME"].ToString();

                            //FillSchedulers(true);
                            Drpdwn_Scheduler.SelectedValue = reader["EXT_SCHEDULER"].ToString();
                            ddlFrameZone.SelectedValue = reader["EXT_FRAMEZONE"].ToString();
                            

                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();
                            lblCRMSSR.Text = reader["EXT_CRMSRID"].ToString();
                            lblWOStatus.Text = reader["STATUS"].ToString();
                            
                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["ORIGRECORDID"].ToString();
                            
                            SelectedOwner(reader["OWNER"].ToString());
                            ViewState["UpdateWorkOrderObj"] = objWorkOrder;
                            if (reader["STATUS"].ToString().ToLower().Equals("appr") || reader["STATUS"].ToString().ToLower().Equals("wappr"))
                            {
                                btnSubmit.Enabled = true;
                                btnSubmit.ForeColor = System.Drawing.Color.White;
                                btnSubmit.BorderColor = System.Drawing.Color.White;
                            }
                            else
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

        private void FillScheduler(string selectedValue)
        {
            if (selectedValue.StartsWith("C"))
            {
                //EnableWOFields();
                txtContractId.Text = WebConfigurationManager.AppSettings["CS-ContractId"]; //"C/2018/41";// "C /2019/63";//C/2018/41
                txtContractId.ToolTip = WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["CS-ContractId"] + "-Tooltip"];
                txtVendor.Text = WebConfigurationManager.AppSettings["CS-VendorId"]; //"101-104142-Doha";// "101 -105790-Doha";//101-104142-Doha
                txtVendorName.Text = WebConfigurationManager.AppSettings["CS-VendorName"]; //"METITO (OVERSEAS) QATAR";// "VELOSI CERTIFICATION LLC";//METITO (OVERSEAS) QATAR
                //txtEngineer.Text = WebConfigurationManager.AppSettings["CS-Engineer"]; //"STALREJA";
                //txtEngineer.ToolTip = WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["CS-Engineer"] + "-Tooltip"];
                //     txtScheduler.Text = WebConfigurationManager.AppSettings["CS-Scheduler"]; //"M_SCH_SOUTH";
                ddlPriority.SelectedValue = WebConfigurationManager.AppSettings["CS-Priority"]; //"HIGH";

                Drpdwn_Scheduler.Items.Clear();
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = "- Select Scheduler -", Value = "-1", Enabled = true, });
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Scheduler"], Value = WebConfigurationManager.AppSettings["CS-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Scheduler.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["CS-Scheduler"] + "-Tooltip"]);
            }
            else if (selectedValue.StartsWith("F"))
            {
                //DisableWOFields();
                Drpdwn_Scheduler.Items.Clear();
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = "- Select Scheduler -", Value = "-1", Enabled = true, });
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Scheduler"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler"], Enabled = true });
                Drpdwn_Scheduler.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["FMP-N-Scheduler"] + "-Tooltip"]); //adding tooltip.

                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Scheduler"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler"], Enabled = true, });
                Drpdwn_Scheduler.Items[2].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["FMP-W-Scheduler"] + "-Tooltip"]); //adding tooltip.

                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Scheduler"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler"], Enabled = true, });
                Drpdwn_Scheduler.Items[3].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["FMP-S-Scheduler"] + "-Tooltip"]); //adding tooltip.
            }
            else if (selectedValue.StartsWith("W"))
            {
                //DisableWOFields();
                Drpdwn_Scheduler.Items.Clear();
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = "- Select Scheduler -", Value = "-1", Enabled = true, });
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Scheduler"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Scheduler.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["Workshop-Scheduler"] + "-Tooltip"]);
            }
            else if (selectedValue.StartsWith("B"))
            {
                //DisableWOFields();
                Drpdwn_Scheduler.Items.Clear();
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = "- Select Scheduler -", Value = "-1", Enabled = true, });
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Baladiya-Scheduler"], Value = WebConfigurationManager.AppSettings["Baladiya-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Scheduler.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["Baladiya-Scheduler"] + "-Tooltip"]);
            }
            else if (selectedValue.StartsWith("R"))
            {
                //DisableWOFields();
                Drpdwn_Scheduler.Items.Clear();
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = "- Select Scheduler -", Value = "-1", Enabled = true, });
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Roads-Scheduler"], Value = WebConfigurationManager.AppSettings["Roads-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Scheduler.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["Roads-Scheduler"] + "-Tooltip"]);

            }
            else if (selectedValue.StartsWith("I"))
            {
                //DisableWOFields();
                Drpdwn_Scheduler.Items.Clear();
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = "- Select Scheduler -", Value = "-1", Enabled = true, });
                Drpdwn_Scheduler.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["IAProject-Scheduler"], Value = WebConfigurationManager.AppSettings["IAProject-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Scheduler.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["IAProject-Scheduler"] + "-Tooltip"]);
            }
        }

        private void InitializeWorkFlowFields()
        {
            txtContractId.Text = string.Empty;
            txtVendor.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            //txtEngineer.Text = string.Empty;
            Drpdwn_Scheduler.SelectedIndex = -1;
        }

        protected void ddlOwnership_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedOwner(ddlOwnership.SelectedValue);
        }

        private void DisableWOFields()
        {
            //imgbtnEngineer.Visible = false;
        }

        private void EnableWOFields()
        {
            //imgbtnEngineer.Visible = true;
        }

        protected void Drpdwn_Scheduler_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtContractId.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
					SELECT pv.[CONTRACTID],pv.[CONTRACTNUM],c.COMPANY,pv.DESCRIPTION,c.NAME,c.COMPANY_KEY 
                        FROM [ODW].[EAMS].[vwDimPurchView] pv, [ODW].[EAMS].[vwDimCompany] c 
                        where pv.STATUS ='APPR' and c.COMPANY_KEY=pv.VENDOR_KEY AND pv.CONTRACTNUM IN (SELECT TOP(1) EXT_CONTRACTNUM FROM EAMS.vwDimPerson WHERE PERSONID = '" + Drpdwn_Scheduler.SelectedValue + "')";
                cmd.Connection = con;
                using (SqlDataReader reader2 = cmd.ExecuteReader())
                {
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            txtContractId.Text = reader2["CONTRACTNUM"].ToString();
                            txtVendor.Text = reader2["COMPANY"].ToString();
                            txtVendorName.Text = reader2["NAME"].ToString();
                        }
                    }
                }

            }
        }

        protected void ddlFrameZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            FrameZoneSelected(ddlFrameZone.SelectedValue);
        }

        private void FrameZoneSelected(string framezone)
        {
            switch (framezone)
            {
                case "Qatar North":
                    ddlOwnership.SelectedValue = "CFZ North";
                    SelectedOwner("CFZ North");
                    break;
                case "Qatar South":
                    ddlOwnership.SelectedValue = "CFZ South";
                    SelectedOwner("CFZ South");
                    break;
                case "Qatar West":
                    ddlOwnership.SelectedValue = "CFZ West";
                    SelectedOwner("CFZ West");
                    break;
                default:
                    break;
            }
        }


    }
}