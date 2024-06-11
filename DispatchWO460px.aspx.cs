using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

    public partial class DispatchWO460px : System.Web.UI.Page
    {
        WorkOrder objWorkOrder;
        string ERROR_MESSAGE = string.Empty;

        static string _scheduler = string.Empty;
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
                if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
                {
                    lblWONUM.Text = Request.QueryString["WONUM"];
                    FillWODetail(Request.QueryString["WONUM"]);
                    _scheduler = txtScheduler.Text.Trim();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                objWorkOrder = new WorkOrder();
                if (ViewState["DispatchWOObj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["DispatchWOObj"];
                }
                objWorkOrder.WONUM = lblWONUM.Text;
                objWorkOrder.ContractId = txtContractId.Text;
                objWorkOrder.CrewLead = Drpdwn_Crew.SelectedValue.Trim();
                objWorkOrder.InternalPriority = Convert.ToInt32(Convert.ToDouble(ddlPriority.SelectedValue));
                objWorkOrder.RESTRICTEDAREA = ddlRestrictedArea.SelectedValue;
                objWorkOrder.Scheduler = txtScheduler.Text;
                objWorkOrder.FloodLoc = ddlFloodLoc.SelectedValue;
                objWorkOrder.SGWAvailability = Network_Availability.SelectedValue;
                objWorkOrder.SGWType = Network_Type.SelectedValue;
                objWorkOrder.RoadClass = Road_Class.SelectedValue;
                objWorkOrder.Event = Event.SelectedValue;

                UpdateWO(objWorkOrder);
            }
        }

        void UpdateWO(WorkOrder pWorkOrder)
        {
            try
            {
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();

                pWorkOrder.EAMSSRNumber = objWorkOrder.EAMSSRNumber;
                pWorkOrder.ECCOwnership = ddlOwnership.SelectedItem.Text;
                pWorkOrder.Hotspot = txtHotspot.Text;
                pWorkOrder.Project = txtProject.Text;
                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    result = esbServices.ECCDispatchWorkOrder(pWorkOrder);
                }
                else
                {
                    result = redHatervices.DispatchWorkOrderECC(pWorkOrder);
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
            lblWONUM.Text = "";
            txtContractId.Text = "";
            txtScheduler.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
            ddlOwnership.SelectedIndex = 0;
            ddlPriority.SelectedIndex = 0;
            Drpdwn_Crew.SelectedIndex = -1;
        }
        private void FillWODetail(string wonum)
        {
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
                    Event.Items.Add(new ListItem("- Select Item -", "-1"));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Event.Items.Add(new ListItem(reader["DESCRIPTION"].ToString(), reader["VALUE"].ToString()));
                        }
                    }
                }
            }
            #endregion

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
                    ddlRestrictedArea.Items.Add(new ListItem("- Select Item -", "-1"));

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

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT LONGITUDEX,LATITUDEY,OWNER,WOPRIORITY,VENDOR,ONBEHALFOF,EXT_SCHEDULER,EXT_ENGINEER,EXT_INSPECTOR,
                    EXT_CREWLEAD,EXT_CONTRACTID,ORIGRECORDID,ORIGRECORDCLASS,EXT_CRMSRID,STATUS, 
                    (Select [NAME] FROM EAMS.DimCompany WHERE Company = w.VENDOR) AS VENDORNAME,
                    (Select DESCRIPTION FROM EAMS.DimPurchView WHERE PURCHVIEW_KEY = w.PURCHVIEW_KEY) AS ContractDesc,
                    (Select DISPLAYNAME FROM EAMS.DimPerson WHERE PERSON_KEY = w.SCHEDULER_KEY) AS SchedulerDesc,
                    (Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'HOTSPOT NUMBER') HOTSPOT,
					(Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'PROJECT ID') PROJECT,  
					(Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'EVENT') [EVENT],  
					(Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'ROOD_CLASS') ROOD_CLASS,  
					(Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'SGW_NETWRK_TYPE') SGW_NETWRK_TYPE,  
					(Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'SGW_NETWRK_AVL') SGW_NETWRK_AVL,  
					(Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'FLODLOC') FLODLOC,
                    (Select ALNVALUE from EAMS.WorkOrderSpecification (nolock) where WONUM=@WONUM and ASSETATTRID = 'RESTRICTEDAREA') RESTRICTEDAREA   
                    FROM [ODW].[EAMS].vwFactWorkOrder w WHERE WONUM = @WONUM", con))
                {
                    cmd.Parameters.AddWithValue("@WONUM", wonum);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objWorkOrder = new WorkOrder();
                            string owner;
                            if (String.IsNullOrEmpty(reader["OWNER"].ToString()))
                            {
                                owner = "- Select Ownership -";
                            }
                            else
                            {
                                owner = reader["OWNER"].ToString();
                            }

                            ddlOwnership.SelectedItem.Text = owner;
                            ddlPriority.SelectedValue = reader["WOPRIORITY"].ToString();
                            ddlRestrictedArea.SelectedValue = reader["RESTRICTEDAREA"].ToString();
                            Road_Class.SelectedValue = reader["ROOD_CLASS"].ToString();
                            Network_Type.SelectedValue = reader["SGW_NETWRK_TYPE"].ToString();
                            Network_Availability.SelectedValue = reader["SGW_NETWRK_AVL"].ToString();
                            ddlFloodLoc.SelectedValue = reader["FLODLOC"].ToString();
                            Event.SelectedValue = reader["EVENT"].ToString();

                            txtScheduler.Text = reader["EXT_SCHEDULER"].ToString();
                            txtScheduler.ToolTip = reader["SchedulerDesc"].ToString();

                            FillCrewBySchedulers(owner);


                            if (String.IsNullOrEmpty(reader["EXT_CREWLEAD"].ToString()))
                            {
                                Drpdwn_Crew.SelectedValue = "-1";
                            }
                            else
                            {
                                Drpdwn_Crew.SelectedValue = reader["EXT_CREWLEAD"].ToString();
                            }

                            txtContractId.Text = reader["EXT_CONTRACTID"].ToString();
                            txtContractId.ToolTip = reader["ContractDesc"].ToString();
                            txtVendor.Text = reader["VENDOR"].ToString();
                            txtVendorName.Text = reader["VENDORNAME"].ToString();

                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();
                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();
                            lblCRMSSR.Text = reader["EXT_CRMSRID"].ToString();
                            lblWOStatus.Text = reader["STATUS"].ToString();
                            lblLong.Text = reader["LONGITUDEX"].ToString();
                            lblLat.Text = reader["LATITUDEY"].ToString();

                            txtHotspot.Text = GetHotspotID(wonum);
                            if (String.IsNullOrEmpty(txtHotspot.Text))
                            {
                                txtHotspot.Text = reader["HOTSPOT"].ToString();
                            }
                            if (!string.IsNullOrEmpty(lblLong.Text) && !string.IsNullOrEmpty(lblLat.Text))
                            {
                                txtProject.Text = GetProjectID(lblLong.Text, lblLat.Text);
                                if (String.IsNullOrEmpty(txtProject.Text))
                                {
                                    txtProject.Text = reader["PROJECT"].ToString();
                                }
                            }

                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["ORIGRECORDID"].ToString();

                            ViewState["DispatchWOObj"] = objWorkOrder;

                            if (reader["STATUS"].ToString().Equals("APPR"))
                            {
                                if (IsWODispatched(wonum) == false)
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

        private bool IsWODispatched(string wonum)
        {
            string query = "SELECT CHARINDEX('\"Status\":\"DISPATCH\"', requestPayload) AS position  ,[requestPayload] ,[uniqueId_WONumber] ,[serviceOperation] " +
              " FROM[dbo].[auditWorkorder] " +
              " WHERE CHARINDEX('\"Status\":\"DISPATCH\"', requestPayload) > 100  AND CHARINDEX('\"returnCodeDesc\":\"Service Success\"',responsePayload) > 0 AND uniqueId_WONumber = '" + wonum + "' AND serviceOperation IN ('ECC Update WorkOrder','ECC_Dispatch_WO') ";
            bool isDispatched = false;

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
                            isDispatched = true;
                        }
                    }
                }
            }

            return isDispatched;
        }

        private string GetHotspotID(string wonum)
        {
            string query = "", hotspotId = "";
            query = @"SELECT refno,[Municipality],[Zone],[District],[StreetNumber],[location],FM19ECCSTATUSValue," +
            "h.Shape.STDistance(geography::STGeomFromText((SELECT Shape FROM GIS.FactWorkOrder WHERE WONUM ='" + wonum + "').ToString(), 4326)) AS Distance" +
            " FROM GIS.vwHotSpot h " +
            " WHERE h.Shape.STBuffer(" + Convert.ToInt32(50) + ").STContains(geography::STGeomFromText((SELECT Shape FROM GIS.FactWorkOrder WHERE WONUM ='" + wonum + "').ToString(), 4326)) = 1 " +
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
        private string GetProjectID(string longitude, string latitude)
        {
            string projectId = "";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GISPRODConnectionString"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[spFindPABoundaryByPoint]";
                    command.Parameters.Add("@long", SqlDbType.VarChar, 30).Value = longitude;
                    command.Parameters.Add("@lat", SqlDbType.VarChar, 30).Value = latitude;

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();

                    adapter.SelectCommand = command;
                    if (ds != null)
                    {
                        adapter.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            projectId = ds.Tables[0].Rows[0]["Project_Code"].ToString();
                        }

                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            lblProjectId.Visible = true;
                            lblProjectId.Text = "This point lies in multiple PA boundaries. You may press the Map button to select the desired project from the list.";
                        }
                    }

                }
            }
            return projectId;
        }


        private void FillCrewBySchedulers(string owner)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                Drpdwn_Crew.Items.Clear();
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT PERSONID,TITLE,JOBCODE,JOBCODE_DESCRIPTION,DISPLAYNAME,EXT_CONTRACTNUM FROM [ODW].[EAMS].[vwDimPerson] WHERE STATUS = 'ACTIVE' AND JOBCODE = 'CREWLEAD' AND TITLE = '" + owner + "'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        int counter = 0;
                        while (reader.Read())
                        {
                            string displayName = reader["DISPLAYNAME"].ToString();
                            string personId = reader["PERSONID"].ToString();

                            Drpdwn_Crew.Items.Add(new ListItem(displayName, personId));
                            Drpdwn_Crew.Items[counter++].Attributes.Add("title", personId);
                        }
                    }
                }
            }
        }

        private void FillCrewByOwnerScheduler(string ddlOwner, string _Schdulertext)
        {
            if (ddlOwner.StartsWith("C"))
            {
                Drpdwn_Crew.Items.Clear();
                Drpdwn_Crew.Items.Add(new ListItem { Text = "- Select CrewLead -", Value = "-1", Enabled = true });
                //    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Scheduler"], Value = WebConfigurationManager.AppSettings["CS-Scheduler"], Enabled = true, Selected = true });

                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description1"], Value = WebConfigurationManager.AppSettings["CS-Crew1"], Enabled = true, Selected = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description2"], Value = WebConfigurationManager.AppSettings["CS-Crew2"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description3"], Value = WebConfigurationManager.AppSettings["CS-Crew3"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description4"], Value = WebConfigurationManager.AppSettings["CS-Crew4"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description5"], Value = WebConfigurationManager.AppSettings["CS-Crew5"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description6"], Value = WebConfigurationManager.AppSettings["CS-Crew6"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description7"], Value = WebConfigurationManager.AppSettings["CS-Crew7"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description8"], Value = WebConfigurationManager.AppSettings["CS-Crew8"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description9"], Value = WebConfigurationManager.AppSettings["CS-Crew9"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["CS-Description10"], Value = WebConfigurationManager.AppSettings["CS-Crew10"], Enabled = true });

            }
            else if (ddlOwner.StartsWith("F"))
            {
                Drpdwn_Crew.Items.Clear();
                Drpdwn_Crew.Items.Add(new ListItem { Text = "- Select CrewLead -", Value = "-1", Enabled = true });

                if (_Schdulertext.Contains("FMP_S"))
                {
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Scheduler"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler"], Enabled = true, Selected = true });
                    Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["FMP-S-Scheduler"] + "-Tooltip"]);
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description1"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler1"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description2"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler2"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description3"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler3"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description4"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler4"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description5"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler5"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description6"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler6"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description7"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler7"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description8"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler8"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description9"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler9"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-S-Description10"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler10"], Enabled = true });

                }
                else if (_Schdulertext.Contains("FMP_N"))
                {
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Scheduler"], Value = WebConfigurationManager.AppSettings["FMP-S-Scheduler"], Enabled = true, Selected = true });
                    Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["FMP-N-Scheduler"] + "-Tooltip"]);
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description1"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler1"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description2"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler2"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description3"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler3"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description4"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler4"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description5"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler5"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description6"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler6"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description7"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler7"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description8"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler8"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description9"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler9"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-N-Description10"], Value = WebConfigurationManager.AppSettings["FMP-N-Scheduler10"], Enabled = true });


                }
                else if (_Schdulertext.Contains("FMP_W"))
                {
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Scheduler"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler"], Enabled = true, Selected = true });
                    Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["FMP-W-Scheduler"] + "-Tooltip"]);
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description1"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler1"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description2"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler2"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description3"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler3"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description4"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler4"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description5"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler5"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description6"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler6"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description7"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler7"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description8"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler8"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description9"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler9"], Enabled = true });
                    Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["FMP-W-Description10"], Value = WebConfigurationManager.AppSettings["FMP-W-Scheduler10"], Enabled = true });
                }


            }
            else if (ddlOwner.StartsWith("W"))
            {

                Drpdwn_Crew.Items.Clear();
                Drpdwn_Crew.Items.Add(new ListItem { Text = "- Select CrewLead -", Value = "-1", Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Scheduler"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["Workshop-Scheduler"] + "-Tooltip"]);
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description1"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler1"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description2"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler2"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description3"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler3"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description4"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler4"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description5"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler5"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description6"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler6"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description7"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler7"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description8"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler8"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description9"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler9"], Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Workshop-Description10"], Value = WebConfigurationManager.AppSettings["Workshop-Scheduler10"], Enabled = true });



            }
            else if (ddlOwner.StartsWith("B"))
            {

                Drpdwn_Crew.Items.Clear();
                Drpdwn_Crew.Items.Add(new ListItem { Text = "- Select CrewLead -", Value = "-1", Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Baladiya-Scheduler"], Value = WebConfigurationManager.AppSettings["Baladiya-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["Baladiya-Scheduler"] + "-Tooltip"]);
            }
            else if (ddlOwner.StartsWith("R"))
            {

                Drpdwn_Crew.Items.Clear();
                Drpdwn_Crew.Items.Add(new ListItem { Text = "- Select CrewLead -", Value = "-1", Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["Roads-Scheduler"], Value = WebConfigurationManager.AppSettings["Roads-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["Roads-Scheduler"] + "-Tooltip"]);
            }
            else if (ddlOwner.StartsWith("I"))
            {

                Drpdwn_Crew.Items.Clear();
                Drpdwn_Crew.Items.Add(new ListItem { Text = "- Select CrewLead -", Value = "-1", Enabled = true });
                Drpdwn_Crew.Items.Add(new ListItem { Text = WebConfigurationManager.AppSettings["IAProject-Scheduler"], Value = WebConfigurationManager.AppSettings["IAProject-Scheduler"], Enabled = true, Selected = true });
                Drpdwn_Crew.Items[1].Attributes.Add("title", WebConfigurationManager.AppSettings[WebConfigurationManager.AppSettings["IAProject-Scheduler"] + "-Tooltip"]);
            }



        }

        protected void lnkReload_Click(object sender, EventArgs e)
        {
            //     string wonum = Request.QueryString["WONUM"];
            //   FillWODetail(wonum);

        }


        private void DisableWOFields()
        {
            //      ReqFldContract.Enabled = false;
            //     spanRed.Visible = false;
            //     imgbtnContract.Visible = false;
            txtContractId.ReadOnly = true;
            txtContractId.BorderWidth = 0;

        }

        private void EnableWOFields()
        {
            //  ReqFldContract.Enabled = true;
            //  spanRed.Visible = true;
            //  imgbtnContract.Visible = true;
            txtContractId.Enabled = true;
            txtContractId.ReadOnly = false;
            txtContractId.BorderWidth = 1;

        }

        private void InitializeWorkFlowFields()
        {

            txtContractId.Text = string.Empty;
            txtVendor.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            //  txtCrew.Text = string.Empty;
            txtScheduler.Text = string.Empty;

        }



        [WebMethod]
        public static List<string> GetCrewDefaultNames(string term)
        {
            List<string> listName = new List<string>();
            if (term.StartsWith("C"))
            {
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew1"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew2"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew3"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew4"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew5"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew6"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew7"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew8"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew9"]);
                listName.Add(WebConfigurationManager.AppSettings["CustomeService-Crew10"]);

            }

            if (term.Contains("F"))
            {
                if (_scheduler.Contains("FMP_W"))
                {
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler1"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler2"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler3"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler4"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler5"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler6"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler7"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler8"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler9"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-W-Scheduler10"]);
                }
                else if (_scheduler.Contains("FMP_S"))
                {
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler1"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler2"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler3"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler4"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler5"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler6"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler7"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler8"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler9"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-S-Scheduler10"]);
                }
                else if (_scheduler.Contains("FMP_N"))
                {
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler1"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler2"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler3"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler4"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler5"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler6"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler7"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler8"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler9"]);
                    listName.Add(WebConfigurationManager.AppSettings["FMP-N-Scheduler10"]);
                }

            }



            return listName;

        }


    }
}