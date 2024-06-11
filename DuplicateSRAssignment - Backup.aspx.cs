using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppForm.App_Code;
using WebAppForm.BusinessEntity;

namespace WebAppForm
{
    public partial class DuplicateSRAssignmentBK : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //FillParentDropDown();
                if (!String.IsNullOrEmpty(Request.QueryString["CRMSId"]))
                {
                    ddlParentSR.Text = Request.QueryString["CRMSID"].ToString();
                    bool complaintstatus = CheckIfComplaintIsNotQueuedOrResolved(Request.QueryString["CRMSID"].ToString());
                    if (complaintstatus == true)
                    {
                        BindData();

                    }
                    else
                    {
                        btnDuplicateSR.Enabled = false;
                        btnDuplicateSR.ForeColor = System.Drawing.Color.Gray;
                        btnDuplicateSR.BorderColor = System.Drawing.Color.Gray;
                    }
                }

            }
        }
        protected void BindData()
        {
            //string query = "SELECT EXT_CRMSRID,STATUS,TICKETID,DESCRIPTION,AFFECTEDPERSON,CHANGEDATE FROM GIS.vwECCComplaints WHERE KPI_Status = 'Unassigned' AND WONUM IS NULL AND STATUS = 'QUEUED' ORDER BY EXT_CRMSRID";

            SqlDataSource1.SelectCommand = @"SELECT EXT_CRMSRID,STATUS,TICKETID,DESCRIPTION,AFFECTEDPERSON,CHANGEDATE FROM GIS.vwECCComplaints WHERE KPI_Status = 'Unassigned' AND WONUM IS NULL AND STATUS = 'QUEUED' ORDER BY REPORTDATE DESC";
            SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows FROM GIS.vwECCComplaints WHERE KPI_Status = 'Unassigned' AND WONUM IS NULL AND STATUS = 'QUEUED'";


            gvDuplicateSR.DataBind();
            dlRowCount.DataBind();
        }

        protected void gvDuplicateSR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDuplicateSR.PageIndex = e.NewPageIndex;
            BindData();
        }

        //private void FillParentDropDown()
        //{
        //    string query = "SELECT EXT_CRMSRID,TICKETID FROM GIS.vwECCComplaints  WHERE KPI_Status NOT IN ('Unassigned','Resolved') AND WONUM IS NOT NULL ORDER BY REPORTDATE DESC";

        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(query, con);

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {

        //            ddlParentSR.Items.Clear();
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    string crmsid = reader["EXT_CRMSRID"].ToString();
        //                    string ticketid = reader["TICKETID"].ToString();
        //                    ddlParentSR.Items.Add(new ListItem(crmsid, ticketid));
        //                }
        //            }
        //        }
        //    }
        //}

        private bool IsDuplicateAssignmentDone(string crmsId)
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


        private bool CheckIfComplaintIsNotQueuedOrResolved(string crmsid)
        {
            string query = "SELECT EXT_CRMSRID,KPI_Status,TICKETID,STATUS FROM GIS.vwECCComplaints WHERE EXT_CRMSRID = '" + crmsid + "'";
            bool isResolvedOrNotExists = true;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == true)
                    {
                        string status = "";
                        while (reader.Read())
                        {
                            status = reader["KPI_Status"].ToString();
                            ViewState["TicketId"] = reader["TICKETID"].ToString();
                            
                            if (status.Equals("Unassigned") || status.Equals("Resolved"))
                            {
                                isResolvedOrNotExists = false;
                                lblEmptyMessage.Text = "The status of the complaint " + crmsid + " is " + reader["STATUS"].ToString() + ". You cannot assign dupliate complaint to this complaint";
                            }
                        }                        
                    }
                    else
                    {
                        isResolvedOrNotExists = false;
                    }
                }
            }
            return isResolvedOrNotExists;
        }
        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        protected void btnDuplicateSR_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            List<String> duplicateSRs = new List<String>();
            foreach (GridViewRow row in gvDuplicateSR.Rows)
            {
                string ticketid = ViewState["TicketId"].ToString();
                ServiceRequest pServiceRequest = new ServiceRequest();
                pServiceRequest.ParentTicketID = RemoveSpecialCharacters(ticketid.Trim());

                CheckBox isSelected = row.Cells[6].FindControl("chk") as CheckBox;
                if (isSelected.Checked == true)
                {
                    String childticketid = row.Cells[1].Text;
                    pServiceRequest.ChildTicketID = childticketid;

                    ESBServices esbServices = new ESBServices();
                    RedHatService redHatervices = new RedHatService();

                    var result = new Tuple<bool, string>(false, "");
                    result = esbServices.ECCDDuplicateSR(pServiceRequest);
                    if (result.Item1 == true)
                    {
                        isSuccess = true;
                        duplicateSRs.Add(row.Cells[0].Text);
                    }

                }
            }

            if (isSuccess == true)
            {
                lblMessage.Text = "Assignment of duplicate complaints " + string.Join(",", duplicateSRs) + " has been completed successfuly";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                BindData();
            }
            else
            {
                lblMessage.Text = "There is error while assignment of duplicate complaints. Please conact application administrator.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }           
        }
    }
}