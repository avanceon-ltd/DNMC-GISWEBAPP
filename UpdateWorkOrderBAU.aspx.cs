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
using WebAppForm.App_Code;

namespace WebAppForm
{
    public partial class UpdateWorkOrderBAU : System.Web.UI.Page
    {
        String _WONUM = String.Empty;
        WorkOrder objWorkOrder;
        bool isRainWater = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtEngineer.Attributes.Add("readonly", "readonly");
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
                    isRainWater = !String.IsNullOrEmpty(Request.QueryString["israinwater"]) && Request.QueryString["israinwater"] == "1";
                    txtPageTitle.InnerText = isRainWater ? "BAU Express Form - Update Rain WO" : "BAU Express Form - Update Non-Rain WO";

                    txtModel.InnerText = isRainWater ? "Update Rain WO: MessageBox" : "Update Non-Rain WO: MessageBox";
                    FillWODetail();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                objWorkOrder = new WorkOrder();
                if (ViewState["UpdateWorkOrderBAUObj"] != null)
                {
                    objWorkOrder = (WorkOrder)ViewState["UpdateWorkOrderBAUObj"];
                }
                objWorkOrder.WONUM = lblWONUM.Text;
                objWorkOrder.Scheduler = txtScheduler.Text.Trim();
                objWorkOrder.ContractId = txtContractId.Text.Trim();
                objWorkOrder.Vendor = txtVendor.Text.Trim();
                objWorkOrder.Engineer = txtEngineer.Text.Trim();

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
                pWorkOrder.Status = lblWOStatus.Text;
                pWorkOrder.ExternalFrameZone = ddlFrameWrk.SelectedValue.Trim();
                pWorkOrder.ExternalCRMSRID = objWorkOrder.CRMSID;
                var result = new Tuple<bool, string>(false, "");
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
                {
                    result = esbServices.UpdateWorkOrder(pWorkOrder, "", isRainWater);
                }
                else
                {
                    result = redHatervices.UpdateWorkOrder(pWorkOrder);
                }
                lblMessage.Text = result.Item2;
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
            ddlFrameWrk.SelectedIndex = 0;

            txtEngineer.Text = "";

        }
        private void FillWODetail()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT VENDOR,ONBEHALFOF,EXT_SCHEDULER,EXT_ENGINEER,EXT_INSPECTOR,EXT_DEFECTTYPE,EXT_FRAMEZONE,ORIGRECORDID,
                                    EXT_CREWLEAD,EXT_CONTRACTID,EXT_CRMSRID,STATUS, 
                                    (Select [NAME] FROM EAMS.DimCompany WHERE Company = w.VENDOR) AS VENDORNAME,
                                    (Select DESCRIPTION FROM EAMS.DimPurchView WHERE PURCHVIEW_KEY = w.PURCHVIEW_KEY) AS ContractDesc,
                                    (Select DISPLAYNAME FROM EAMS.DimPerson WHERE PERSON_KEY = w.ENGINEER_KEY) AS EngineerDesc 
                                    FROM [ODW].[EAMS].vwFactWorkOrder w WHERE WONUM = @WONUM", con))
                {
                    cmd.Parameters.AddWithValue("@WONUM", _WONUM);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objWorkOrder = new WorkOrder();
                            ddlFrameWrk.SelectedValue = reader["EXT_FRAMEZONE"].ToString();

                            txtContractId.Text = reader["EXT_CONTRACTID"].ToString();
                            txtContractId.ToolTip = reader["ContractDesc"].ToString();
                            txtVendor.Text = reader["VENDOR"].ToString();
                            txtVendorName.Text = reader["VENDORNAME"].ToString();

                            txtScheduler.Text = reader["EXT_SCHEDULER"].ToString();

                            txtEngineer.Text = reader["EXT_ENGINEER"].ToString();
                            txtEngineer.ToolTip = reader["EngineerDesc"].ToString();


                            lblEAMSSR.Text = reader["ORIGRECORDID"].ToString();
                            lblCRMSSR.Text = reader["EXT_CRMSRID"].ToString();
                            lblWOStatus.Text = reader["STATUS"].ToString();
                            //if (reader["STATUS"].Equals("WAPPR"))
                            //{
                            //txtContractId.ReadOnly = false;
                            //ImageButton1.Visible = true;
                            //}

                            objWorkOrder.CRMSID = reader["EXT_CRMSRID"].ToString();
                            objWorkOrder.EAMSSRNumber = reader["ORIGRECORDID"].ToString();
                            ViewState["UpdateWorkOrderBAUObj"] = objWorkOrder;
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



    }
}