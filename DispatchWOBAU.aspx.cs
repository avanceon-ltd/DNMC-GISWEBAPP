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
using System.Xml;
using WebAppForm.App_Code;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class DispatchWOBAU : System.Web.UI.Page
    {
        WorkOrder objWorkOrder;
        bool isRainWater = false;

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
                txtScheduleStart.Text = DateTime.Now.ToString();
                txtScheduleFinish.Text = DateTime.Now.AddHours(4).ToString();
                if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
                {
                    lblWONUM.Text = Request.QueryString["WONUM"];
                    isRainWater = !String.IsNullOrEmpty(Request.QueryString["israinwater"]) && Request.QueryString["israinwater"] == "1";
                    txtPageTitle.InnerText = isRainWater ? "BAU Express Form - Dispatch Rain WO" : "BAU Express Form - Dispatch Non-Rain WO";
                    FillWODetail(Request.QueryString["WONUM"]);
                    _scheduler = txtScheduler.Text.Trim();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //WorkOrder objWorkOrder = new WorkOrder();
                objWorkOrder = new WorkOrder();
                if (ViewState["DispatchWOBAUObj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["DispatchWOBAUObj"];
                }
                objWorkOrder.WONUM = lblWONUM.Text;
                objWorkOrder.ContractId = txtContractId.Text;
                objWorkOrder.CrewLead = txt_Crew.Text.Trim();//Drpdwn_Crew.SelectedValue.Trim();

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
                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();

                pWorkOrder.EAMSSRNumber = objWorkOrder.EAMSSRNumber;
                pWorkOrder.ExternalCRMSRID = objWorkOrder.CRMSID;
                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    result = esbServices.DispatchWorkOrder(pWorkOrder, isRainWater);
                }
                else
                {
                    result = redHatervices.DispatchWorkOrder(pWorkOrder);
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
            lblWONUM.Text = "";
            txtContractId.Text = "";
            // txtCrew.Text = "";
            txtScheduleFinish.Text = "";
            txtScheduler.Text = "";
            txtScheduleStart.Text = "";
            txtVendor.Text = "";
            txtVendorName.Text = "";
            txt_Crew.Text = "";//Drpdwn_Crew.SelectedIndex = -1;
        }
        private void FillWODetail(string wonum)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT top 50 dp.DISPLAYNAME as CrewLeadName,w.LONGITUDEX,w.LATITUDEY,w.OWNER,w.PRIORITY,w.VENDOR,w.ONBEHALFOF,w.EXT_SCHEDULER,w.EXT_ENGINEER,w.EXT_INSPECTOR,
                    w.EXT_CREWLEAD,w.EXT_CONTRACTID,w.ORIGRECORDID,w.ORIGRECORDCLASS,w.EXT_CRMSRID,w.STATUS, 
                    (Select [NAME] FROM EAMS.DimCompany WHERE Company = w.VENDOR) AS VENDORNAME,
                    (Select DESCRIPTION FROM EAMS.DimPurchView WHERE PURCHVIEW_KEY = w.PURCHVIEW_KEY) AS ContractDesc,
                    (Select DISPLAYNAME FROM EAMS.DimPerson WHERE PERSON_KEY = w.SCHEDULER_KEY) AS SchedulerDesc 
                    FROM [ODW].[EAMS].vwFactWorkOrder w
					left join [ODW].[EAMS].DimPerson dp on w.CREWLEAD_KEY= dp.PERSON_KEY WHERE w.WONUM = @WONUM", con))
                {
                    cmd.Parameters.AddWithValue("@WONUM", wonum);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objWorkOrder = new WorkOrder();
                            //string owner;
                            //if (String.IsNullOrEmpty(reader["OWNER"].ToString()))
                            //{
                            //    owner = "- Select Ownership -";
                            //}
                            //else
                            //{
                            //    owner = reader["OWNER"].ToString();
                            //}

                            txtScheduler.Text = reader["EXT_SCHEDULER"].ToString();
                            txtScheduler.ToolTip = reader["SchedulerDesc"].ToString();

                            string contractId = reader["EXT_CONTRACTID"].ToString();
                            //FillCrewBySchedulers(contractId);

                            //if (String.IsNullOrEmpty(reader["EXT_CREWLEAD"].ToString()))
                            //{
                            //    Drpdwn_Crew.SelectedValue = "-1";
                            //}
                            //else
                            //{
                            //    Drpdwn_Crew.SelectedValue = reader["EXT_CREWLEAD"].ToString();
                            //}
                            txt_Crew.Text = reader["EXT_CREWLEAD"].ToString();
                            txt_Crew.Attributes.Add("title", reader["CrewLeadName"].ToString());
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

                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["ORIGRECORDID"].ToString();
                            ViewState["DispatchWOBAUObj"] = objWorkOrder;


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
              " WHERE CHARINDEX('\"Status\":\"DISPATCH\"', requestPayload) > 100  AND CHARINDEX('\"returnCodeDesc\":\"Service Success\"',responsePayload) > 0 AND uniqueId_WONumber = '" + wonum + "' AND serviceOperation IN ('ECC Update WorkOrder','BAU_Dispatch_Rain_WO','BAU_Dispatch_Non_Rain_WO') ";
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

        private void FillCrewBySchedulers(string contractId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT PERSONID,TITLE,JOBCODE,JOBCODE_DESCRIPTION,DISPLAYNAME,EXT_CONTRACTNUM FROM [ODW].[EAMS].[vwDimPerson] WHERE JOBCODE = 'CREWLEAD' AND EXT_CONTRACTNUM ='" + contractId + "'", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        int counter = 0;
                        while (reader.Read())
                        {
                            string displayName = reader["DISPLAYNAME"].ToString();
                            string personId = reader["PERSONID"].ToString();

                            //Drpdwn_Crew.Items.Add(new ListItem(displayName, personId));
                            //Drpdwn_Crew.Items[counter++].Attributes.Add("title", personId);
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

        
    }
}