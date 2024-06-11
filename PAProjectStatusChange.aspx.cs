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
using WebAppForm.App_Code;

namespace WebAppForm
{


    public partial class PAProjectStatusChange : System.Web.UI.Page
    {

        string code, tbl_type = string.Empty;



        protected void Page_Load(object sender, EventArgs e)
        {



            if (!String.IsNullOrEmpty(Request.QueryString["Type"]))
            {

                tbl_type = Convert.ToString(Request.QueryString["Type"]);

                if (!String.IsNullOrEmpty(Request.QueryString["User"]))
                {

                    userName.Text = Convert.ToString(Request.QueryString["User"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ProjCode"]))
                {
                    code = Convert.ToString(Request.QueryString["ProjCode"]);

                    //if (!IsPostBack)
                    //    binddata(ID);
                }

                switch (tbl_type)
                {
                    case "BC":
                        binddata(code, WebConfigurationManager.AppSettings[tbl_type]);
                        break;
                    case "DNPD":

                        break;
                    case "FPS":

                        break;
                    case "FRP":

                        break;
                    case "HPD":

                        break;
                    case "RPD":

                        break;
                    default:
                        break;

                }

            }
        }

        private void binddata(string code, string tablename)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GISUATConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT TOP (1) [OBJECTID],[Project_Name],[Project_Code],[ECC_Status],[ECC] FROM " + tablename + " where Project_Code='" + code + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtprojcode.Text = reader["Project_Code"].ToString();
                            txtprojname.Text = reader["Project_Name"].ToString();
                       
                            if (!string.IsNullOrEmpty(reader["ECC_Status"].ToString()))
                            {
                                ddlStatus.SelectedValue = reader["ECC_Status"].ToString();
                            
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

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    con.Open();
                    if (bool.Parse(ViewState["rowExist"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"UPDATE [GIS].[HotSpot_Status] SET [FM19eccstatus] =@FM19eccstatus,[Modified_Date]=@Modified_Date  WHERE [refno]=@UID", con))
                        {

                            cmd.Parameters.AddWithValue("@UID", ID);
                            cmd.Parameters.AddWithValue("@FM19eccstatus", ddlStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@Modified_Date", DateTime.Now.ToString());
                            //cmd.Parameters.AddWithValue("@loggedinUser", (!string.IsNullOrEmpty(userName.Text.Trim())) ? userName.Text.Trim() : (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                            lblMessage.Text = "Your Hotspot data with refno:" + ID + " updated successfully";

                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [GIS].[HotSpot_Status]([refno],[FM19eccstatus],Modified_Date) VALUES (@UID,@FM19eccstatus,@Modified_Date)", con))
                        {
                            cmd.Parameters.AddWithValue("@UID", ID);
                            cmd.Parameters.AddWithValue("@FM19eccstatus", ddlStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@Modified_Date", DateTime.Now.ToString());
                            //cmd.Parameters.AddWithValue("@loggedinUser", (!string.IsNullOrEmpty(userName.Text.Trim())) ? userName.Text.Trim() : (object)DBNull.Value);



                            cmd.ExecuteNonQuery();

                            lblMessage.Text = "Your data inserted successfully";

                        }
                    }
                }
            }
        }




    }


}