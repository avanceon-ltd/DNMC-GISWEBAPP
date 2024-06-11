using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebAppForm
{
    public partial class AddDWSite : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AddDWSite));
        private string constr = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        protected void Page_Load(object sender, EventArgs e)
        {
            string AccessLevel = Request.QueryString["AccessLevel"];
            if (!string.IsNullOrEmpty(AccessLevel))
            {
                int acclvl = Convert.ToInt32(AccessLevel);
                if (acclvl >= 7777)
                {
                    btnSubmit.Enabled = true;
                }
                else
                {
                    btnSubmit.Enabled = false;
                    btnSubmit.CssClass = "btnDisabled";
                }
            }
            else
            {
                btnSubmit.Enabled = false;
                btnSubmit.CssClass = "btnDisabled";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string siteName = txtSiteName.Text.Trim();
                    string permitNo = txtPermitNo.Text.Trim();
                    string ipAddress = txtIPAddress.Text.Trim();
                    bool IsActive = CB_IsActive.Checked;

                    bool exists = checkIfRecordExists(siteName);

                    if (checkIfRecordExists(siteName) == false)
                    {
                        string query = "INSERT INTO [CRMS].[DewateringSitesSCADABridge](SCADA_SiteName,PERMIT_NO,IP_ADDRESS,IsActive) VALUES( @SiteName, @PermitNo, @IPAddress,@IsActive)";
                        string info = "Add DW Site\nQuery\n" + query + "\nparams";
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@SiteName", siteName);
                                cmd.Parameters.AddWithValue("@PermitNo", permitNo);
                                cmd.Parameters.AddWithValue("@IPAddress", ipAddress);
                                cmd.Parameters.AddWithValue("@IsActive", IsActive);

                                foreach (SqlParameter param in cmd.Parameters)
                                {
                                    info += "\n" + param.ParameterName + ": " + param.Value;
                                }

                                log.Info(info);

                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        lblMessage.Text = "Dewatering site added successfully.";
                        ClearControls();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    }
                    else
                    {
                        lblMessage.Text = "Dewatering '" + siteName + "' site already exists.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    }

                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddDWSite).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }


        private bool checkIfRecordExists(string siteName)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [CRMS].[DewateringSitesSCADABridge] WHERE SCADA_SiteName = @SCADA_SiteName", conn);
            check_User_Name.Parameters.AddWithValue("@SCADA_SiteName", siteName);
            int sites = (int)check_User_Name.ExecuteScalar();
            conn.Close();

            return sites > 0 ? true : false;
        }

        private void ClearControls()
        {
            txtSiteName.Text = "";
            txtPermitNo.Text = "";
            txtIPAddress.Text = "";
        }

        protected void btnViewList_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["AccessLevel"]))
            {
                btnViewList.PostBackUrl = "~/DWSitesList.aspx?AccessLevel=" + Request.QueryString["AccessLevel"];
            }
            else
            {
                btnViewList.PostBackUrl = "~/DWSitesList.aspx";
            }

            Response.Redirect(btnViewList.PostBackUrl);
        }
    }
}