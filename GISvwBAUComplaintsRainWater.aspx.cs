using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class GISvwBAUComplaintsRainWater : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkBtnSR.Enabled = false;
            lnkBtnWO.Enabled = false;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                SqlDataSource1.DataBind();
                //Repeater1.DataBind();

                SetAssociatedSRWO(Convert.ToString(Request.QueryString["ID"]));
                //  Session["CRMSID"] = Convert.ToString(Request.QueryString["ID"]);



            }
        }

        private void SetAssociatedSRWO(string ID)
        {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(@"SELECT [EAMS_SR_Number],[WONUM] FROM [ODW].[GIS].[vwDrainageComplaints] where [Service_Request_Number]=@UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", ID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader["EAMS_SR_Number"].ToString()))
                            {
                                lnkBtnSR.PostBackUrl = "SRDetail.aspx?ID=" + reader["EAMS_SR_Number"].ToString() + "&CRMSID=" + ID;
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

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 130;  //or more time....
        }
        protected void lnkbtnRefresh_Click(object sender, EventArgs e)
        {
            DetailsView1.DataBind();

        }

        protected void lnkbtnCreateWO_Click(object sender, EventArgs e)
        {
            Response.Write("<script type='text/javascript'>");
            Response.Write("window.open('CreateWOCustomer.aspx?CRMSId=" + Request.QueryString["ID"] + "','_blank');");
            Response.Write("</script>");
        }

    }
}