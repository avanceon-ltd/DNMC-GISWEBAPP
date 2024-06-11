using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class ReportedBy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvCustomers_DataBound(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                SqlDataSource1.SelectCommand = @"SELECT PERSONID,DISPLAYNAME,TITLE,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA FROM EAMS.DIMPERSON WHERE PERSONID IS NOT NULL and STATUS='ACTIVE' ORDER BY PERSONID";
                SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows FROM EAMS.DIMPERSON WHERE DISPLAYNAME IS NOT NULL and STATUS='ACTIVE'";
            }
            else
            {
                SqlDataSource1.SelectCommand = @"SELECT PERSONID,DISPLAYNAME,TITLE,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA FROM EAMS.DIMPERSON WHERE PERSONID IS NOT NULL and STATUS='ACTIVE' and" + " DISPLAYNAME LIKE '%" + txtSearch.Text.Trim() +  "%' ORDER BY PERSONID";
                SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows FROM EAMS.DIMPERSON WHERE PERSONID IS NOT NULL and STATUS='ACTIVE' and" + " DISPLAYNAME LIKE '%" + txtSearch.Text.Trim() + "%'";

            }
            gvCustomers.DataBind();
            DataList5.DataBind();



        }

    }
}