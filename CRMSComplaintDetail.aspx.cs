using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class CRMSComplaintDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkBtnSR.Enabled = false;
            lnkBtnWO.Enabled = false;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                sdsCRMS.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                sdsCRMS.DataBind();

                sdsEAMS.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                sdsEAMS.DataBind();
                //Repeater1.DataBind();

                SetAssociatedSRWO(Convert.ToString(Request.QueryString["ID"]));
                //  Session["CRMSID"] = Convert.ToString(Request.QueryString["ID"]);
                BindEAMSData();


            }
        }

        private void SetAssociatedSRWO(string ID)
        {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(@"SELECT [TICKETID],[WONUM] FROM [ODW].[GIS].[ECCComplaints] where [EXT_CRMSRID]=@UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", ID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader["TICKETID"].ToString()))
                            {
                                lnkBtnSR.PostBackUrl = "SRDetail.aspx?ID=" + reader["TICKETID"].ToString() + "&CRMSID=" + ID;
                                lnkBtnSR.Enabled = true;
                            }
                            else
                                lnkBtnSR.Enabled = false;


                            if (!string.IsNullOrEmpty(reader["WONUM"].ToString()))
                            {
                                lnkBtnWO.PostBackUrl = "WorkOrderDetail.aspx?ID=" + reader["WONUM"].ToString() + "&CRMSID=" + ID + "&USER=" + Request.QueryString["USER"];
                                lnkBtnWO.Enabled = true;
                            }
                            else
                                lnkBtnWO.Enabled = false;
                        }
                    }
                }
            }
        }

        

        protected void sdsEAMS_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 130;  //or more time....
        }

        protected void sdsCRMS_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 130;  //or more time....
        }
        protected void lnkbtnRefresh_Click(object sender, EventArgs e)
        {
            dvCRMS.DataBind();
            //dvEAMS.DataBind();
        }

        private void BindEAMSData()
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            string query = @"SELECT 
	   sr.[EXT_CRMSRID]
	  ,sr.[DESCRIPTION]
      ,sr.[REPORTDATE]
      ,sr.[STATUS]
      ,sr.[WONUM]
      ,sr.[WO_Status]
      ,sr.[OWNER]
      ,sr.[PRIORITY]
      ,sr.[KPI_Status] 
      ,sr.[LATITUDEY]
      ,sr.[LONGITUDEX]
      ,sr.[TICKETID]
      
      ,sr.[AFFECTEDPERSON]
      ,sr.[AFFECTEDPHONE]
      ,sr.[CHANGEDATE]      
      ,sr.[EXT_FRAMEZONE]
      ,sr.ParentCRMSRID,sr.IsDuplicate,sr.HaveDuplicates, '' AS ChildCRMS
  FROM  [GIS].[ECCComplaints] sr LEFT JOIN [CRMS].[DrainageComplaints] crms 
  ON crms.Service_Request_Number = sr.EXT_CRMSRID
  WHERE sr.EXT_CRMSRID = '" + Request.QueryString["ID"] + "'";
            cmd.CommandText = query;
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            string childQuery  = " select EXT_CRMSRID from GIS.ECCComplaints where ParentCRMSRID ='" + Request.QueryString["ID"] + "'";
            string childCrms = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                cmd = new SqlCommand(childQuery, con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        
                        while (reader.Read())
                        {
                            childCrms += reader["EXT_CRMSRID"].ToString() + ", ";
                        }
                    }
                }
            }

            dt.Rows[0]["ChildCRMS"] = childCrms;

            Repeater1.DataSource = dt;
            Repeater1.DataBind();

        }

        public string ConvertNullableBoolToYesNo(object pBool)
        {
            if (!String.IsNullOrEmpty(pBool.ToString()) && pBool != null)
            {
                return (bool)pBool ? "Yes" : "No";
            }
            else
            {
                return "";
            }
        }

        protected void lnkbtnCreateWO_Click(object sender, EventArgs e)
        {
            Response.Write("<script type='text/javascript'>");
            Response.Write("window.open('CreateWOCustomer.aspx?CRMSId=" + Request.QueryString["ID"] + "','_blank');");
            Response.Write("</script>");
        }

        protected void llnkbtnCRMSLink(object sender, EventArgs e)
        {
            Response.Write("<script type='text/javascript'>");
            Response.Write("window.open('CreateWOCustomer.aspx?CRMSId=" + Request.QueryString["ID"] + "','_blank');");
            Response.Write("</script>");
        }
        

        protected void lnkbtnCRMSLink_Click(object sender, EventArgs e)
        {
          
            string CRMS_ID = Request.QueryString["ID"];

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(@"SELECT Service_Request_Id from CRMS.DrainageComplaints Where Service_Request_Number =@CRMSID", con))
                {
                    cmd.Parameters.AddWithValue("@CRMSID", CRMS_ID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string CRMSSerReqID = reader["Service_Request_Id"].ToString();
                            if (!string.IsNullOrEmpty(CRMSSerReqID))
                            {
                                
                                //string crmsid = "your_dynamic_crmsid_here"; // This should be dynamically retrieved, perhaps from session or query string
                                string popupUrl = "https://crmsuat2.crm4.dynamics.com/main.aspx?appid=12d0a13f-63a4-e911-a2cc-00155dc60a3b&forceUCI=1&pagetype=entityrecord&etn=incident&id="+ CRMSSerReqID;

                                // Script to open a new popup window
                                string script = $"window.open('{popupUrl}', 'Popup', 'width=800,height=720,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes');";

                                // Register the script to run on the client side
                                ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                            }
                        }
                    }
                }
            }
  

        }
    }
}