using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Engineer : System.Web.UI.Page
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
                SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where COMPANY_KEY = -1  and STATUS = 'Active' and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'ENGINEER') ORDER BY PERSONID";
                SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where COMPANY_KEY = -1 and STATUS = 'Active' and PERSONID in (select respparty from [EAMS].[DimPersonGroup] where persongroup = 'ENGINEER')";
            }
            else
            {
                SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where COMPANY_KEY = -1 and STATUS = 'Active' and" + " PERSONID LIKE '%" + txtSearch.Text.Trim() + "%'" + " and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'ENGINEER') ORDER BY PERSONID";
                SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where COMPANY_KEY = -1 and STATUS = 'Active' and" + " PERSONID LIKE '%" + txtSearch.Text.Trim() + "%'" + " and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'ENGINEER')";

            }
            gvCustomers.DataBind();
            DataList5.DataBind();



        }

    }
}