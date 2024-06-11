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
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class AssetFailure : System.Web.UI.Page
    {

        int UID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ClearControls();
                if (!String.IsNullOrEmpty(Request.QueryString["Lat"]))
                {

                    txtLat.Text = Convert.ToString(Request.QueryString["Lat"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Long"]))
                {
                    txtlong.Text = Convert.ToString(Request.QueryString["Long"]); ;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["UID"]))
                {
                    UID = Convert.ToInt32(Request.QueryString["UID"]);
                    hid_UID.Value = UID.ToString();
                    ViewState["Updaterow"] = true;
                    binddata(UID);
                    btnSubmit.Text = "Update Asset";
                    lbl_label.Text = "Update Asset Failure - Express Form";
                    txtAssetDes.ReadOnly = true;
                    txtlong.ReadOnly = true;
                    txtLat.ReadOnly = true;
                    btnDel.Visible = true;
                    btnDel.Enabled = true;

                }
                else
                {
                    btnSubmit.Text = "Create Asset";
                    txtAssetDes.ReadOnly = false;
                    txtlong.ReadOnly = false;
                    txtLat.ReadOnly = false;
                    btnDel.Visible = false;
                    btnDel.Enabled = false;
                    lbl_label.Text = "Create Asset Failure - Express Form";
                    ViewState["Updaterow"] = false;
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(hid_UID.Value))
                { UID = Convert.ToInt32(hid_UID.Value); }

            }
        }

        private void binddata(int uid)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select * from [ODW].[GIS].[AssetFailure] where UID=@UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", uid);


                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtAssetDes.Text = reader["Description"].ToString();
                            txtLat.Text = reader["Lat"].ToString();
                            txtlong.Text = reader["Long"].ToString();
                            txtRemarks.Text = reader["Remarks"].ToString();
                            ddlStatus.SelectedValue = reader["Status"].ToString();
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
                    if (bool.Parse(ViewState["Updaterow"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"UPDATE [GIS].[AssetFailure] SET [Status]=@Status ,[Remarks]=@Remarks,[ModifiedDate]=@ModifiedDate WHERE UID=@UID", con))
                        {

                            cmd.Parameters.AddWithValue("@UID", UID);
                            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
                            cmd.ExecuteNonQuery();
                            lblMessage.Text = "Your Asset data with UID:" + UID + " updated successfully";
                            ClearControls();
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand(@"insert into [ODW].[GIS].[AssetFailure] ([Description],[Lat],[Long],[Status],[Remarks]) values(@AD, @lat, @long,@Status,@Remarks)", con))
                        {
                            cmd.Parameters.AddWithValue("@AD", txtAssetDes.Text.Trim());
                            cmd.Parameters.AddWithValue("@lat", (!string.IsNullOrEmpty(txtLat.Text.Trim())) ? txtLat.Text.Trim() : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@long", (!string.IsNullOrEmpty(txtlong.Text.Trim())) ? txtlong.Text.Trim() : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue.Trim());
                            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());


                            cmd.ExecuteNonQuery();

                            lblMessage.Text = "Your data inserted successfully";
                            ClearControls();
                        }
                    }
                }
            }
        }



        void ClearControls()
        {
            txtAssetDes.Text = "";
            txtLat.Text = "";
            //  ddlStatus.SelectedValue = "CM";
            txtlong.Text = "";
            txtRemarks.Text = "";
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"delete from [ODW].[GIS].[AssetFailure] where UID=@UID", con))
                    {
                        cmd.Parameters.AddWithValue("@UID", UID);
                        con.Open();
                        cmd.ExecuteNonQuery();

                        lblMessage.Text = "Your Asset data with UID:" + UID + " deleted successfully";
                        ClearControls();

                        btnSubmit.Text = "Create Asset";
                        txtAssetDes.ReadOnly = false;
                        txtlong.ReadOnly = false;
                        txtLat.ReadOnly = false;
                        btnDel.Visible = false;
                        btnDel.Enabled = false;
                        lbl_label.Text = "Create Asset Failure - Express Form";
                        ViewState["Updaterow"] = false;
                    }
                }
            }
        }
    }
}