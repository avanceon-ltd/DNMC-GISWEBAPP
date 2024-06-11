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
    public partial class CreateWOCustomer : System.Web.UI.Page
    {
        WorkOrder objWorkOrder;
        string ERROR_MESSAGE = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            txtVendor.Attributes.Add("readonly", "readonly");
            txtVendorName.Attributes.Add("readonly", "readonly");
            txtTargetStart.Attributes.Add("readonly", "readonly");
            txtTargetFinish.Attributes.Add("readonly", "readonly");
            txtReportedBy.Attributes.Add("readonly", "readonly");
            txtLocation.Attributes.Add("readonly", "readonly");
            txtContractId.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                txtWOReportedDate.Text = DateTime.Now.ToString();
                txtTargetStart.Text = DateTime.Now.ToString();
                txtTargetFinish.Text = DateTime.Now.AddHours(4).ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["CRMSId"]))
                {
                    string _ID = Convert.ToString(Request.QueryString["CRMSId"]);
                    lblCRMSId.Text = _ID;
                    FillRainWaterComplaintData(_ID);
                    FillOwners();
                    FillFrameworkZone();
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

                if (!String.IsNullOrEmpty(Request.QueryString["ECCOwnership"]))
                {
                    ddlOwnership.Text = Convert.ToString(Request.QueryString["ECCOwnership"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ECCPriority"]))
                {
                    ddlPriority.Text = Convert.ToString(Request.QueryString["ECCPriority"]); ;
                }

                //if (!String.IsNullOrEmpty(Request.QueryString["Engineer"]))
                //{
                //    txtEngineer.Text = Convert.ToString(Request.QueryString["Engineer"]); ;
                //}

                if (!String.IsNullOrEmpty(Request.QueryString["ContractId"]))
                {
                    txtContractId.Text = Convert.ToString(Request.QueryString["ContractId"]); ;
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

            if (ddlOwnership.SelectedIndex == -1)
                Drpdwn_Scheduler.SelectedIndex = -1;
        }

        // Submit Function
        #region Submit-Function
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                objWorkOrder = new WorkOrder();
                if (ViewState["CreateWOCustomerObj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["CreateWOCustomerObj"];
                }

                objWorkOrder.Description = txtWODes.Text.Trim();
                objWorkOrder.Location = txtLocation.Text.Trim();
                objWorkOrder.Asset = txtAsset.Text.Trim();
                objWorkOrder.WorkType = "FR";
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
                if (latitude < latStart || latitude > latEnd)
                {
                    lblMessage.Text = "Latitude must be between " + latStart.ToString() + " to " + latEnd.ToString() + ".";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    return;
                }
                objWorkOrder.Longitude = txtlong.Text.Trim();
                objWorkOrder.Latitude = txtLat.Text.Trim();
                objWorkOrder.TargetStart = Convert.ToDateTime(txtTargetStart.Text).ToString("s");
                objWorkOrder.TargetFinish = Convert.ToDateTime(txtTargetFinish.Text).ToString("s");
                objWorkOrder.ECCOwnership = ddlOwnership.SelectedValue.Trim();
                objWorkOrder.InternalPriority = Convert.ToInt32(Convert.ToDouble(ddlPriority.SelectedValue));
                objWorkOrder.RESTRICTEDAREA = ddlRestrictedArea.SelectedValue;
                objWorkOrder.ContractId = txtContractId.Text.Trim();
                objWorkOrder.OnBehalfOf = txtOnBehalfOf.Text.Trim();
                objWorkOrder.Scheduler = Drpdwn_Scheduler.SelectedValue;
                objWorkOrder.SGWAvailability = Network_Availability.SelectedValue;
                objWorkOrder.SGWType = Network_Type.SelectedValue;
                objWorkOrder.RoadClass = Road_Class.SelectedValue;
                objWorkOrder.Event = Event.SelectedValue;
                objWorkOrder.FloodLoc = ddlFloodLoc.SelectedValue;
                objWorkOrder.Project = txtProject.Text.Trim();
                
                CreateWO(objWorkOrder);
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModal();", true);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WorkOrderList.aspx");
        }
        #endregion

        // Unused Function
        #region Unused-Function
        private void DisableWOFields()
        {
            spanRed.Visible = false;
            imgbtnContract.Visible = false;
            //imgbtnEngineer.Visible = false;
            txtContractId.ReadOnly = true;
            txtContractId.BorderWidth = 0;
        }
        private void EnableWOFields()
        {
            spanRed.Visible = true;
            imgbtnContract.Visible = true;
            //imgbtnEngineer.Visible = true;
            txtContractId.Enabled = true;
            txtContractId.ReadOnly = false;
            txtContractId.BorderWidth = 1;

        }
        #endregion

        // Form Filling Function
        #region Form-Filling-Function
        void FillOwners()
        {
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
                    ddlPriority.Items.Add(new ListItem("- Select Item -", "-1"));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlPriority.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
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
                        ddlRestrictedArea.SelectedIndex = 1;
                    }
                }
            }
            #endregion
            #region "Flood Location"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.ALNDomain WHERE DOMAINID = 'RNFLOODLOC'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlFloodLoc.Items.Clear();
                    ddlFloodLoc.Items.Add(new ListItem("- Select Item -", "-1"));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlFloodLoc.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }
                    }
                }
            }
            #endregion

            #region "SGW Network Availability"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.ALNDomain WHERE DOMAINID = 'EXT_YESNO'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Network_Availability.Items.Clear();
                    Network_Availability.Items.Add(new ListItem("- Select Item -", "-1"));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Network_Availability.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }
                    }
                }
            }
            #endregion

            #region "SGW Network Type"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.ALNDomain WHERE DOMAINID = 'SGW_NETWRK_TYPE'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Network_Type.Items.Clear();
                    Network_Type.Items.Add(new ListItem("- Select Item -", "-1"));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Network_Type.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }
                    }
                }
            }
            #endregion

            #region "Road Class"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.ALNDomain WHERE DOMAINID = 'ROOD_CLASS'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Road_Class.Items.Clear();
                    Road_Class.Items.Add(new ListItem("- Select Item -", "-1"));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Road_Class.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }
                    }
                }
            }
            #endregion

            #region "Event"

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DESCRIPTION,VALUE FROM EAMS.ALNDomain WHERE DOMAINID = 'EVENT'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Event.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Event.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }

                        Event.SelectedValue = "NA";
                    }
                }
            }
            #endregion

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


                if (!String.IsNullOrEmpty(lblEAMSSRId.Text))
                {
                    cmd.CommandText = @"SELECT EXT_FRAMEZONE FROM EAMS.FRFactServiceRequest WHERE TICKETID ='" + lblEAMSSRId.Text + "'";
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
                                        ddlFrameZone.SelectedValue = reader2["EXT_FRAMEZONE"].ToString();
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
                                                            ddlFrameZone.SelectedValue = catchment;
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

            FrameZoneSelected(ddlFrameZone.SelectedValue);
        }
        private void FillRainWaterComplaintData(string iD)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                //using (SqlCommand cmd = new SqlCommand(@"SELECT [Service_Request_Number], [Contact_Reason], [Details],[Latitude],[Longitude],[Street],[Municipality],[Building_Number],[Zone],[District],[Pin],[EAMS_SR_Number],[TICKETID],[WONUM] FROM [ODW].CRMS.DrainageComplaints WHERE Service_Request_Number = @UID", con))
                using (SqlCommand cmd = new SqlCommand(@"SELECT  EXT_CRMSRID,DESCRIPTION,LATITUDEY,LONGITUDEX,TICKETID,WONUM FROM GIS.ECCComplaints WHERE EXT_CRMSRID = @UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", iD);

                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtWODes.Text = reader["DESCRIPTION"].ToString().Trim() ;
                            txtLat.Text = reader["LATITUDEY"].ToString().Trim();
                            txtlong.Text = reader["LONGITUDEX"].ToString().Trim();

                            objWorkOrder = new WorkOrder();
                            //objWorkOrder.Street = reader["Street"].ToString();
                            //objWorkOrder.BuildingNumber = reader["Building_Number"].ToString();
                            //objWorkOrder.Zone = reader["Zone"].ToString();
                            //objWorkOrder.District = reader["District"].ToString();
                            //objWorkOrder.Pin = reader["Pin"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["TICKETID"].ToString();
                            objWorkOrder.TICKETID = reader["TICKETID"].ToString();
                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            lblEAMSSRId.Text = reader["TICKETID"].ToString();
                            ViewState["CreateWOCustomerObj"] = objWorkOrder;
                            txtHotspot.Text = GetHotspotID(iD);
                            if (String.IsNullOrEmpty(lblEAMSSRId.Text) || !String.IsNullOrEmpty(reader["WONUM"].ToString()) || IsWOCreated(iD) == true)
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

        #endregion

        // Dropdown Filling
        #region Dropdown-Filling
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
                SqlCommand cmd = new SqlCommand(@"SELECT PERSONID,TITLE,JOBCODE,JOBCODE_DESCRIPTION,DISPLAYNAME,EXT_CONTRACTNUM,SUPERVISOR,(SELECT TOP(1) t.DISPLAYNAME  FROM EAMS.vwDimPerson t WHERE t.PERSONID = p.SUPERVISOR) AS Engineer_DisplayName FROM [EAMS].[vwDimPerson] p WHERE  STATUS = 'ACTIVE' AND JOBCODE = 'SCHEDULER' AND TITLE = '" + owner + "'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string displayName = reader["DISPLAYNAME"].ToString();
                            string personId = reader["PERSONID"].ToString();
                            string engTooltip = reader["Engineer_DisplayName"].ToString();

                            Drpdwn_Scheduler.Items.Add(new ListItem(displayName, personId));
                            if (isFirst == false) //Only selected the first scheduler
                            {
                                Drpdwn_Scheduler.SelectedIndex = 0;
                                contractId = reader["EXT_CONTRACTNUM"].ToString();
                                txtContractId.Text = contractId;
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
        #endregion

        // Supporting Function
        #region Supporting-Function
        void CreateWO(WorkOrder pWorkOrder)
        {
            try
            {
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();
                pWorkOrder.ExternalFrameZone = ddlFrameZone.SelectedValue;
                pWorkOrder.ReportedDate = DateTime.Now.ToString("s");
                pWorkOrder.ReportedBy = txtReportedBy.Text;
                pWorkOrder.Hotspot = txtHotspot.Text;
                pWorkOrder.ExternalDefectType = "RNWFLOOD";
                pWorkOrder.ExternalCRMSRID = objWorkOrder.CRMSID;

                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    //result = esbServices.CreateWorkOrder(pWorkOrder, true, true);
                    result = esbServices.Create_ECC_WorkOrder(pWorkOrder);
                }
                else
                {
                    result = redHatervices.CreateWorkOrder(pWorkOrder);
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
            ddlOwnership.SelectedIndex = -1;
            ddlPriority.SelectedIndex = -1;
            ddlRestrictedArea.SelectedIndex = -1;
            Drpdwn_Scheduler.SelectedIndex = -1;
            txtContractId.Text = "";            
            txtVendor.Text = "";
            txtVendorName.Text = "";
        }
        private bool IsWOCreated(string crmsId)
        {
            string query = "SELECT CHARINDEX('\"ExternalCRMSRID\":\"" + crmsId + "\"', requestPayload) AS position " +
            " ,[requestPayload] ,[uniqueId_WONumber] ,[serviceOperation] FROM [dbo].[auditWorkorder] " +
            " WHERE CHARINDEX('\"ExternalCRMSRID\":\"" + crmsId + "\"', requestPayload) > 100 " +
            " AND uniqueId_WONumber IS NOT NULL  AND LEN(uniqueId_WONumber) >0  AND serviceOperation IN ('ECC-Composite Create WorkOrder','ECC_Comp_Create_WO') ";
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
        private string GetHotspotID(string serviceRequest)
        {
            string query = "", hotspotId = "";
            query = @"SELECT refno,[Municipality],[Zone],[District],[StreetNumber],[location],FM19ECCSTATUSValue," +
            "h.Shape.STDistance(geography::STGeomFromText((Select Shape from GIS.ECCComplaints Where EXT_CRMSRID ='" + serviceRequest + "').ToString(), 4326)) AS Distance" +
            " FROM GIS.vwHotSpot h " +
            " WHERE h.Shape.STBuffer(" + Convert.ToInt32(50) + ").STContains(geography::STGeomFromText((Select Shape from GIS.ECCComplaints Where EXT_CRMSRID ='" + serviceRequest + "').ToString(), 4326)) = 1 " +
            " ORDER BY Distance ASC ";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();

                    adapter.SelectCommand = command;
                    if (ds != null)
                    {
                        adapter.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            hotspotId = ds.Tables[0].Rows[0]["refno"].ToString();
                        }

                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            lblHotspotId.Visible = true;
                            lblHotspotId.Text = "Multiple Hotspots exist within 50 meters of this point. You may press the Map button to select the desired Hotspot.";
                        }
                    }

                }
            }
            return hotspotId;

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
        private void InitializeWorkFlowFields()
        {

            txtContractId.Text = string.Empty;
            txtVendor.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            //txtEngineer.Text = string.Empty;

        }
        #endregion

        // SelectedIndexChanged
        #region SelectedIndexChanged
        protected void Drpdwn_Network_Availability_SelectedIndexChanged(object sender, EventArgs e)
        { }
        protected void Drpdwn_Network_Type_SelectedIndexChanged(object sender, EventArgs e)
        { }
        protected void Drpdwn_Road_Class_SelectedIndexChanged(object sender, EventArgs e)
        { }
        protected void Drpdwn_Event_SelectedIndexChanged(object sender, EventArgs e)
        { }
        protected void ddlOwnership_SelectedIndexChanged(object sender, EventArgs e)
        {

            SelectedOwner(ddlOwnership.SelectedValue);
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
                cmd.CommandText = @"SELECT pv.[CONTRACTID],pv.[CONTRACTNUM],c.COMPANY,pv.DESCRIPTION,c.NAME,c.COMPANY_KEY, (SELECT SUPERVISOR FROM EAMS.vwDimPerson WHERE PERSONID = '" + Drpdwn_Scheduler.SelectedValue + "') AS SUPERVISOR" +
                        " FROM [ODW].[EAMS].[vwDimPurchView] pv, [ODW].[EAMS].[vwDimCompany] c " +
                        " WHERE pv.STATUS ='APPR' and c.COMPANY_KEY=pv.VENDOR_KEY AND pv.CONTRACTNUM IN (SELECT TOP(1) EXT_CONTRACTNUM FROM EAMS.vwDimPerson WHERE PERSONID = '" + Drpdwn_Scheduler.SelectedValue + "')";
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
        
        #endregion
    }
}