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
    public partial class EAMSComplaintDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkBtnSR.Enabled = false;
            lnkBtnWO.Enabled = false;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                SqlDataSource1.DataBind();
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

       
    }
}