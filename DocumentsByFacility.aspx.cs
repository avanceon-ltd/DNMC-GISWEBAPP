using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class DocumentsByFacility : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void gvDocLinks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocLinks.PageIndex = e.NewPageIndex;
            DataBind();
        }

    }
}