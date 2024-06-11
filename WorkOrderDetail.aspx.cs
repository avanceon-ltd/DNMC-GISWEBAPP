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
    public partial class WorkOrderDetail : System.Web.UI.Page
    {
        string _user, _crmsid = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["USER"]))
                _user = Request.QueryString["USER"];
            if (!String.IsNullOrEmpty(Request.QueryString["CRMSID"]))
                _crmsid = Request.QueryString["CRMSID"];

            if (!Page.IsPostBack)
            {
                string wonum = Request.QueryString["ID"];
                if (!String.IsNullOrEmpty(wonum))
                {
                    string[] wonums = wonum.Split(',');
                    ddlWONUM.Items.Clear();

                    foreach (string item in wonums)
                    {
                        ddlWONUM.Items.Add(new ListItem(item.Trim(), item.Trim()));
                    }

                    if (!String.IsNullOrEmpty(Request.QueryString["CRMSID"]))
                    {
                        lnkbtnback.Visible = true;
                        lnkbtnback.PostBackUrl = "CRMSComplaintDetail.aspx?ID=" + Convert.ToString(Request.QueryString["CRMSID"]);
                    }

                    SqlDataSource1.SelectParameters["WONUM"].DefaultValue = wonums[0].Trim();
                    DetailsView1.DataBind();

                    SetdispatchWOlink(wonums[0].Trim(), _user, _crmsid);
                    lnkWorkLog.PostBackUrl = "WorkLogs.aspx?id=" + ddlWONUM.SelectedValue + "&backlink=true" + "&CRMSID=" + _crmsid + "&USER=" + _user;
                }
            }

        }

        private void SetdispatchWOlink(string _wonum, string user, string crmsid)
        {
            //    linkDispatchWO

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(@"SELECT [EXT_FRAMEZONE] FROM [ODW].[EAMS].[FRFactWorkOrder] where WONUM=@UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", _wonum);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader["EXT_FRAMEZONE"].ToString()))
                            {
                                linkDispatchWO.PostBackUrl = "DispatchWO460px.aspx?wonum=" + _wonum + "&backlink=true" + "&CRMSID=" + _crmsid + "&USER=" + _user;
                            }
                            else
                                linkDispatchWO.PostBackUrl = "DispatchWOBAU460px.aspx?wonum=" + _wonum + "&backlink=true" + "&CRMSID=" + _crmsid + "&USER=" + _user;

                        }
                    }
                }
            }

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 1300;  //or more time....
        }

        protected void lnkbtnRefresh_Click(object sender, EventArgs e)
        {
            DetailsView1.DataBind();

        }

        protected void lnkWorkLog_Click(object sender, EventArgs e)
        {
            lnkWorkLog.PostBackUrl = "WorkLogs.aspx?id=" + ddlWONUM.SelectedValue + "&backlink=true" + "&CRMSID=" + _crmsid + "&USER=" + _user;

        }

        protected void ddlWONUM_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataSource1.SelectParameters["WONUM"].DefaultValue = ddlWONUM.SelectedValue.Trim();
            DetailsView1.DataBind();
            SetdispatchWOlink(ddlWONUM.SelectedValue.Trim(), _user, _crmsid);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}