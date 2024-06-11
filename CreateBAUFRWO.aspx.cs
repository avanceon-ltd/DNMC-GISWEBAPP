using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WebAppForm.App_Code;

namespace WebAppForm
{

    public partial class CreateBAUFRWO : System.Web.UI.Page
    {
        WorkOrder objWorkOrder;
        bool isRainWater = false;
        int rainWaterInt = 0;
        ILog log = log4net.LogManager.GetLogger(typeof(CreateBAUFRWO));
        protected void Page_Load(object sender, EventArgs e)
        {
            txtVendor.Attributes.Add("readonly", "readonly");
            txtVendorName.Attributes.Add("readonly", "readonly");
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");
            txtReportedBy.Attributes.Add("readonly", "readonly");
            txtEngineer.Attributes.Add("readonly", "readonly");
            txtContractId.Attributes.Add("readonly", "readonly");
            txtScheduler.Attributes.Add("readonly", "readonly");
            txtFrameZone.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                txtScheduler.Text = "";
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                txtWOReportedDate.Text = DateTime.Now.ToString();
                txtTargetStart.Text = DateTime.Now.ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(4).ToString();

                isRainWater = !String.IsNullOrEmpty(Request.QueryString["israinwater"]) && Request.QueryString["israinwater"] == "1";
                txtPageTitle.InnerText = isRainWater ? "BAU Express Form - Create Rain FR WO" : "BAU Express Form - Create Non-Rain FR WO";
                FillDefectType(isRainWater);

                if (!String.IsNullOrEmpty(Request.QueryString["CRMSId"]))
                {
                    string _ID = Convert.ToString(Request.QueryString["CRMSId"]);
                    lblCRMSId.Text = _ID;
                    FillRainWaterComplaintData(_ID);
                    FillFrameworkZone();
                    if (isRainWater == true)
                    {
                        search.Visible = false;
                        txtLocation.Text = "QATAR";
                    }
                }


                if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
                {
                    lblWONUM.Text = Request.QueryString["WONUM"];
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

                if (!String.IsNullOrEmpty(Request.QueryString["ContractId"]))
                {
                    txtContractId.Text = Convert.ToString(Request.QueryString["ContractId"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ReportedBy"]))
                {
                    txtReportedBy.Text = Convert.ToString(Request.QueryString["ReportedBy"]); 
                }
                else
                {
                    txtReportedBy.Text = Convert.ToString(Request.QueryString["ReportedBy"]);
                 //   txtReportedBy.Text = WebConfigurationManager.AppSettings["ReportedBy"];
                }
            }

            //if (ddlFrameZone.SelectedIndex == -1)
            //    Drpdwn_Scheduler.SelectedIndex = -1;
        }

        private void FillDefectType(bool isRainWater)
        {
            if (isRainWater == true)
            {
                ddlDefectType.Items.Add(new ListItem { Value = "RNWFLOOD", Text = "RNWFLOOD - Rainwater Flooding" });
            }
            else
            {

                ddlDefectType.Items.Add(new ListItem { Value = "SWRBKUP", Text = "SWRBKUP - Sewer Back Up (No flooding/spillage)" });
                ddlDefectType.Items.Add(new ListItem { Value = "XTFLD", Text = "External Flooding", Selected = true });
                ddlDefectType.Items.Add(new ListItem { Value = "XTFLDTSE", Text = "External Flooding TSE/Recycled Water" });
                ddlDefectType.Items.Add(new ListItem { Value = "MNHLCVR", Text = "MNHLCVR - Manhole Cover" });
                ddlDefectType.Items.Add(new ListItem { Value = "ODOUR", Text = "Odour Complaint" });
                ddlDefectType.Items.Add(new ListItem { Value = "INTFLOOD", Text = "Internal Flooding" });
                ddlDefectType.Items.Add(new ListItem { Value = "NOWATER", Text = "No Water TSE/Recycled Water" });
                ddlDefectType.Items.Add(new ListItem { Value = "LOWPRESS", Text = "Low Pressure TSE/Recycled Water" });
                ddlDefectType.Items.Add(new ListItem { Value = "NOISE", Text = "Noise Complaint" });
                ddlDefectType.Items.Add(new ListItem { Value = "IGWFLOOD", Text = "Internal groundwater Flooding" });
                ddlDefectType.Items.Add(new ListItem { Value = "GRSTRAP", Text = "Grease Trap Request" });
                ddlDefectType.Items.Add(new ListItem { Value = "CLAIMS", Text = "CLAIMS - Claims" });
                ddlDefectType.Items.Add(new ListItem { Value = "DEWATER", Text = "DEWATER - Dewatering Permit Request" });
                ddlDefectType.Items.Add(new ListItem { Value = "TSECON", Text = "TSECON - TSE Connection Request" });

            }
        }

        void FillFrameworkZone()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();

                if (!String.IsNullOrEmpty(lblEAMSSRId.Text))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT EXT_FRAMEZONE FROM EAMS.vwFactServiceRequest WHERE TICKETID ='" + lblEAMSSRId.Text + "'", con);
                    {
                        using (SqlDataReader reader2 = cmd.ExecuteReader())
                        {
                            if (reader2.HasRows)
                            {
                                while (reader2.Read())
                                {
                                    string framezone = reader2["EXT_FRAMEZONE"].ToString();
                                    if (!String.IsNullOrEmpty(framezone))
                                    {
                                        txtFrameZone.Text = reader2["EXT_FRAMEZONE"].ToString();
                                    }
                                    else
                                    {
                                        string longitude = "", latitude = "";
                                        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GISPRODConnectionString"].ConnectionString))
                                        {
                                            longitude = txtlong.Text.Trim();
                                            latitude = txtLat.Text.Trim();
                                            connection.Open();
                                            using (SqlCommand command1 = new SqlCommand())
                                            {
                                                command1.Connection = connection;

                                                command1.CommandType = CommandType.StoredProcedure;
                                                command1.CommandText = "[dbo].[spFindFrameZoneByPoint]";
                                                command1.Parameters.Add("@long", SqlDbType.VarChar, 30).Value = longitude;
                                                command1.Parameters.Add("@lat", SqlDbType.VarChar, 30).Value = latitude;

                                                using (SqlDataReader reader1 = command1.ExecuteReader())
                                                {
                                                    if (reader1.HasRows)
                                                    {
                                                        while (reader1.Read())
                                                        {
                                                            string catchment = reader1["CATCHMENT"].ToString();
                                                            txtFrameZone.Text = catchment;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            FrameZoneSelected(txtFrameZone.Text);
        }


        private void FillRainWaterComplaintData(string iD)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT [Service_Request_Number], [Contact_Reason], [Details],[Latitude],[Longitude],[Street],[Municipality],[Building_Number],[Zone],[District],[Pin],[EAMS_SR_Number],[TICKETID],[WONUM],[Takamul_Catchment_Zone],[Takamul_Catchment_Contractor_Id], (SELECT EXT_DEFECTTYPE FROM EAMS.FactServiceRequest WHERE TICKETID = EAMS_SR_Number) AS DEFECTTYPE, (SELECT CLASSSTRUCTUREID FROM EAMS.FactServiceRequest WHERE TICKETID = EAMS_SR_Number) AS CLASSSTRUCTUREID FROM [ODW].CRMS.DrainageComplaints WHERE Service_Request_Number = @UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", iD);

                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlDefectType.SelectedValue = reader["DEFECTTYPE"].ToString();
                            txtWODes.Text = reader["Service_Request_Number"].ToString().Trim() + " – " + reader["Contact_Reason"].ToString().Trim();
                            txtLat.Text = reader["Latitude"].ToString().Trim();
                            txtlong.Text = reader["Longitude"].ToString().Trim();
                            txtFrameZone.Text = reader["Takamul_Catchment_Zone"].ToString().Trim();
                            //if (!String.IsNullOrEmpty(txtFrameZone.Text)) { FrameZoneSelected(txtFrameZone.Text.Trim()); }
                            txtContractId.Text = reader["Takamul_Catchment_Contractor_Id"].ToString().Trim();
                            if (!String.IsNullOrEmpty(txtContractId.Text)) { FillContractValues(txtContractId.Text.Trim()); }
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
                            objWorkOrder.ClassStructID = reader["CLASSSTRUCTUREID"].ToString();
                            //objWorkOrder.SADescription = reader["Details"].ToString().Substring(0,40);
                            lblEAMSSRId.Text = reader["EAMS_SR_Number"].ToString();
                            // lblClassStructId.Text = reader["CLASSSTRUCTUREID"].ToString();

                            ViewState["CreateBAUFRWOObj"] = objWorkOrder;
                            string DType = "";
                            if (String.IsNullOrEmpty(lblEAMSSRId.Text) || !String.IsNullOrEmpty(reader["WONUM"].ToString()) || IsWOCreated(iD) == true || IsSameDefectType(lblEAMSSRId.Text, ddlDefectType.SelectedValue, out DType) == true)
                            {
                                btnSubmit.Enabled = false;
                                btnSubmit.ForeColor = System.Drawing.Color.Gray;
                                btnSubmit.BorderColor = System.Drawing.Color.Gray;
                            }

                            if (lblEAMSSRId.Text.ToLower().Contains("qu"))
                            {
                                lblMessage.Text = "ERROR! EAMS SR Number is QUEUED. Contact ISD to resolve this issue.";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

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
                objWorkOrder = new WorkOrder();
                if (ViewState["CreateBAUFRWOObj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["CreateBAUFRWOObj"];
                }
                objWorkOrder.Description = txtWODes.Text.Trim();
                objWorkOrder.Location = txtLocation.Text.Trim();
                objWorkOrder.Asset = txtAsset.Text.Trim();
                //objWorkOrder.WorkType = drpdwnWorkType.SelectedValue.Trim();
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
                objWorkOrder.ContractId = txtContractId.Text.Trim();
                objWorkOrder.OnBehalfOf = txtOnBehalfOf.Text.Trim();
                objWorkOrder.Engineer = txtEngineer.Text.Trim();
                objWorkOrder.Scheduler = txtScheduler.Text;
                objWorkOrder.Location = isRainWater ? "QATAR" : objWorkOrder.Location;

                CreateWO(objWorkOrder);
            }
        }

        void CreateWO(WorkOrder pWorkOrder)
        {
            try
            {
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();
                pWorkOrder.ExternalFrameZone = txtFrameZone.Text.Trim();
                pWorkOrder.ReportedBy = txtReportedBy.Text;
                pWorkOrder.ExternalDefectType = ddlDefectType.SelectedValue;
                pWorkOrder.ExternalCRMSRID = objWorkOrder.CRMSID;
                pWorkOrder.ClassStructID = objWorkOrder.ClassStructID;
                var result = new Tuple<bool, string>(false, "");

                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    //result = esbServices.CreateWorkOrder(pWorkOrder, isRainWater);
                    string DType = "";
                    bool defect = IsSameDefectType(lblEAMSSRId.Text, pWorkOrder.ExternalDefectType, out DType);
                    rainWaterInt = DType == "RNWFLOOD" ? 1 : 0;
                    result = esbServices.CreateWorkOrder(pWorkOrder, true, false, rainWaterInt);
                }
                else
                {
                    result = redHatervices.CreateWorkOrder(pWorkOrder);
                }

                lblMessage.Text = result.Item2;
                btnSubmit.Enabled = false;
                btnSubmit.ForeColor = System.Drawing.Color.Gray;
                btnSubmit.BorderColor = System.Drawing.Color.Gray;
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
            txtLocation.Text = "QATAR";
            txtAsset.Text = "";

            txtTargetStart.Text = "";
            txtTargetFinish.Text = "";
            txtlong.Text = "";
            txtLat.Text = "";
            txtScheduler.Text = "";
            txtContractId.Text = "";
            txtEngineer.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
        }

        private bool IsWOCreated(string crmsId)
        {
            string query = "SELECT CHARINDEX('\"ExternalCRMSRID\":\"" + crmsId + "\"', requestPayload) AS position " +
            " ,[requestPayload] ,[uniqueId_WONumber] ,[serviceOperation] FROM [dbo].[auditWorkorder] " +
            " WHERE CHARINDEX('\"ExternalCRMSRID\":\"" + crmsId + "\"', requestPayload) > 100 " +
            " AND uniqueId_WONumber IS NOT NULL  AND LEN(uniqueId_WONumber) >0  AND serviceOperation  IN ('ECC-Composite Create WorkOrder','BAU_Comp_Update_Rain_WO','BAU_Comp_Create_Non_Rain_WO') ";
            bool isWOCreated = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FuseConnectionString"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandType = CommandType.Text;
                    command.CommandText = query;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            isWOCreated = true;
                        }
                    }
                }
            }

            return isWOCreated;
        }

        private bool IsSameDefectType(string TicketID, string DefectType, out string DType)
        {
            string query = "select EXT_DEFECTTYPE from EAMS.FactServiceRequest where TICKETID = '" + TicketID + "'";
            bool isWOCreated = false;
            DType = "";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandType = CommandType.Text;
                    command.CommandText = query;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            string ServiceDefectType = reader["EXT_DEFECTTYPE"].ToString();
                            DType = ServiceDefectType;
                            if (ServiceDefectType != DefectType)
                                isWOCreated = true;
                        }
                    }
                }
            }

            return isWOCreated;
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


        private void DisableWOFields()
        {
            spanRed.Visible = false;
            imgbtnContract.Visible = false;
            imgbtnEngineer.Visible = false;
            txtContractId.ReadOnly = true;
            txtContractId.BorderWidth = 0;
        }

        private void EnableWOFields()
        {
            spanRed.Visible = true;
            imgbtnContract.Visible = true;
            imgbtnEngineer.Visible = true;
            txtContractId.Enabled = true;
            txtContractId.ReadOnly = false;
            txtContractId.BorderWidth = 1;

        }

        private void InitializeWorkFlowFields()
        {

            txtContractId.Text = string.Empty;
            txtVendor.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            txtEngineer.Text = string.Empty;
            //    txtScheduler.Text = string.Empty;

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
                cmd.CommandText = @"SELECT pv.[CONTRACTID],pv.[CONTRACTNUM],c.COMPANY,pv.DESCRIPTION,c.NAME,c.COMPANY_KEY, (SELECT SUPERVISOR FROM EAMS.vwDimPerson WHERE PERSONID = '" + txtScheduler.Text + "') AS SUPERVISOR" +
                        " FROM [ODW].[EAMS].[vwDimPurchView] pv, [ODW].[EAMS].[vwDimCompany] c " +
                        " WHERE pv.STATUS ='APPR' and c.COMPANY_KEY=pv.VENDOR_KEY AND pv.CONTRACTNUM IN (SELECT TOP(1) EXT_CONTRACTNUM FROM EAMS.vwDimPerson WHERE PERSONID = '" + txtScheduler.Text + "')";
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
        protected void txtFrameZone_Changed(object sender, EventArgs e)
        {
            FrameZoneSelected(txtFrameZone.Text.Trim());
        }
        protected void ddlFrameZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FrameZoneSelected(ddlFrameZone.SelectedValue);
        }

        private void FrameZoneSelected(string framezone)
        {
            switch (framezone)
            {
                case "Qatar North":
                    SelectedOwner("CFZ North");
                    break;
                case "Qatar South":
                    SelectedOwner("CFZ South");
                    break;
                case "Qatar West":
                    SelectedOwner("CFZ West");
                    break;
                default:
                    break;
            }
        }

        private void SelectedOwner(string owner)
        {
            txtScheduler.Text = "";
            //txtContractId.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                bool isFirst = false;
                string contractId = "";
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT PERSONID,TITLE,JOBCODE,JOBCODE_DESCRIPTION,DISPLAYNAME,EXT_CONTRACTNUM,SUPERVISOR,(SELECT TOP(1) t.DISPLAYNAME  FROM EAMS.vwDimPerson t WHERE t.PERSONID = p.SUPERVISOR) AS Engineer_DisplayName FROM [EAMS].[vwDimPerson] p WHERE JOBCODE = 'SCHEDULER' AND TITLE = '" + owner + "'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string displayName = reader["DISPLAYNAME"].ToString();
                            string personId = reader["PERSONID"].ToString();
                            string engTooltip = reader["Engineer_DisplayName"].ToString();

                            if (isFirst == false) //Only selected the first scheduler
                            {
                                contractId = reader["EXT_CONTRACTNUM"].ToString();
                                //txtContractId.Text = contractId;
                                txtEngineer.Text = reader["SUPERVISOR"].ToString();
                                txtEngineer.ToolTip = engTooltip;
                                //txtScheduler.Text = personId;
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
                                txtContractId.Text = contractId;
                            }
                        }
                    }
                }
            }
        }

        private void FillContractValues(string contractId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT pv.[CONTRACTID],pv.[CONTRACTNUM],c.COMPANY,pv.DESCRIPTION,c.NAME,c.COMPANY_KEY 
                        FROM [ODW].[EAMS].[vwDimPurchView] pv, [ODW].[EAMS].[vwDimCompany] c 
                        where pv.STATUS ='APPR' and c.COMPANY_KEY=pv.VENDOR_KEY AND pv.CONTRACTNUM = '" + contractId + "'", con);
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
}