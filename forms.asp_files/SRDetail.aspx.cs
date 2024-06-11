using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class SRDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                SqlDataSource1.DataBind();
                //Repeater1.DataBind();

                if (!String.IsNullOrEmpty(Request.QueryString["CRMSID"]))
                {
                    lnkbtnback.Visible = true;
                    lnkbtnback.PostBackUrl = "CRMSComplaintDetail.aspx?ID=" + Convert.ToString(Request.QueryString["CRMSID"]);
                //    Session["CRMSID"] = null;
                }

            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 130;  //or more time....
        }


    }
}