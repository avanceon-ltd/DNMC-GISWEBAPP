using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class FIFAKPI : System.Web.UI.Page
    {
        string Stadium = string.Empty;
        string Type = string.Empty;
        ILog log = log4net.LogManager.GetLogger(typeof(FIFAKPI));
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Stadium = Request.QueryString["Stadium"];
                Type = Request.QueryString["Type"];
                if (!string.IsNullOrEmpty(Stadium) && !string.IsNullOrEmpty(Type))
                {
                    switch (Type)
                    {
                        case "Rain":
                            lblTitle.Text = "Complaint (Rainwater)";
                            break;
                        case "NR":
                            lblTitle.Text = "Complaint (Non Rainwater)";
                            break;
                        case "Total":
                            lblTitle.Text = "Complaint (Total)";
                            break;

                    }
                    getKPI();
                }
                else
                {
                    lblTitle.Text = "Complaint (Rainwater)";

                    Unassigned.Text = "0";
                    Assigned.Text = "0";
                    Unattended.Text = "0";
                    InProgress.Text = "0";
                    WorkComp.Text = "0";
                    Resolved.Text = "0";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace.ToString());
            }


        }

        private void getKPI()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("[FIFA].[FIFAKPI]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Stadium", Stadium));
                    cmd.Parameters.Add(new SqlParameter("@Type", Type));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Unassigned.Text = reader["Unassigned"].ToString();
                            Assigned.Text = reader["Assigned"].ToString();
                            Unattended.Text = reader["Unattended"].ToString();
                            InProgress.Text = reader["InProgress"].ToString();
                            WorkComp.Text = reader["WorkComp"].ToString();
                            Resolved.Text = reader["Resolved"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace.ToString());
            }
        }

    }
}