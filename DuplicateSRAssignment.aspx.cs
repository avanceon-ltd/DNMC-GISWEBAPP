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
    public partial class DuplicateSRAssignment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(CheckParentComplaint()))
                {
                    lblConfirm.Text = "Are you sure you want to assign duplicate complaint(s) " + Request.QueryString["cCRMSID"] + " to Compliant# " + Request.QueryString["pCRMSID"];
                }
                else
                {
                    lblMessage.Text = "There is error while assignment of duplicate complaints. Please conact application administrator.";
                }
            }
        }

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


        private List<String> CheckChildComplaints()
        {
            string query = "SELECT EXT_CRMSRID,KPI_Status,TICKETID,STATUS FROM GIS.ECCComplaints WHERE EXT_CRMSRID IN (" + AddCRMSClause() + ")";
            List<String> UnassResCrmsids = new List<string>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == true)
                    {
                        string status = "";
                        int i = 0;
                        while (reader.Read())
                        {
                            status = reader["KPI_Status"].ToString();

                            if (status.Equals("Unassigned"))
                            {
                                UnassResCrmsids.Add(reader["TICKETID"].ToString());

                            }
                        }
                    }

                }
            }
            return UnassResCrmsids;
        }

        private string CheckParentComplaint()
        {
            string query = "SELECT EXT_CRMSRID,KPI_Status,TICKETID,STATUS FROM GIS.ECCComplaints WHERE EXT_CRMSRID = '" + Request.QueryString["pCRMSID"].Trim() + "'";
            string ticketid = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == true)
                    {
                        string status = "";
                        int i = 0;
                        while (reader.Read())
                        {
                            status = reader["KPI_Status"].ToString();
                            if (!status.Equals("Unassigned"))
                            {
                                ticketid = reader["TICKETID"].ToString();
                            }
                        }
                    }

                }
            }
            return ticketid;
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

        public string AddCRMSClause()
        {
            string[] crmsids = Request.QueryString["cCRMSID"].Trim().Split(',');
            string result = "";
            foreach (string crmsid in crmsids)
            {
                result += "'" + crmsid + "',";
            }

            result = result.Substring(0, result.Length - 1);
            return result;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DuplicateAssignment();
        }
        protected void DuplicateAssignment()
        {
            bool isSuccess = false;
            List<String> childComplaints = CheckChildComplaints();

            foreach (String childComplaint in childComplaints)
            {
                ServiceRequest pServiceRequest = new ServiceRequest();
                pServiceRequest.ParentTicketID = CheckParentComplaint();


                pServiceRequest.ChildTicketID = childComplaint;

                ESBServices esbServices = new ESBServices();
                RedHatService redHatervices = new RedHatService();

                var result = new Tuple<bool, string>(false, "");
                result = esbServices.ECCDDuplicateSR(pServiceRequest);
                if (result.Item1 == true)
                {
                    isSuccess = true;
                }
            }

            if (isSuccess == true)
            {
                lblMessage.Text = "Assignment of duplicate complaints " + string.Join(",", childComplaints) + " has been completed successfuly";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

            }
            else
            {
                lblMessage.Text = "There is error while assignment of duplicate complaints. Please conact application administrator.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        string RemoveEmptyValues(String queryString)
        {
            String[] values = queryString.Split(',');
            String result = "";
            foreach (string value in values)
            {
                if (!String.IsNullOrEmpty(value.Trim()))
                    result += value + ",";
            }

            return result.Remove(result.Length - 1);
        }
    }
}