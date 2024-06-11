using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class VehicleDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                    SqlDataSource1.DataBind();
                }
            }
        }

        protected void lnkbtnRefresh_Click(object sender, EventArgs e)
        {
            DetailsView1.DataBind();

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 1300;  //or more time....
        }
    }
}