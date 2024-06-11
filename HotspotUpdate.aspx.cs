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
using System.Web.Services;
using WebAppForm.App_Code;

namespace WebAppForm
{


    public partial class HotspotUpdate : System.Web.UI.Page
    {

        string ID = string.Empty;
       

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["User"]))
            {

                userName.Text = Convert.ToString(Request.QueryString["User"]);
            }


            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {

                ID = Convert.ToString(Request.QueryString["ID"]);

                if (!IsPostBack)
                    binddata(ID);
            }


        }

        private void binddata(string id)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT top(1) [Zone],[StreetName],[Text1],[refno],[FM19eccstatus] FROM [ODW].[GIS].[vwHotSpot_update_Mapping] where refno=@ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);


                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtHotspotDes.Text = reader["Text1"].ToString();
                            txtStreet.Text = reader["StreetName"].ToString();
                            txtZone.Text = reader["Zone"].ToString();

                            if (!string.IsNullOrEmpty(reader["FM19eccstatus"].ToString()))
                            {
                                ddlStatus.SelectedValue = reader["FM19eccstatus"].ToString();
                                ViewState["rowExist"] = true;
                            }else
                                ViewState["rowExist"] = false;
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