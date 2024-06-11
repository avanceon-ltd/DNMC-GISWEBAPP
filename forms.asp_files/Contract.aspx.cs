using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Contract : System.Web.UI.Page
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
                SqlDataSource1.SelectCommand = "SELECT  d.[CONTRACTID],d.[CONTRACTNUM],e.COMPANY,d.DESCRIPTION,e.NAME,e.COMPANY_KEY FROM [ODW].[EAMS].[DimPurchView] d, [ODW].[EAMS].[DimCompany] e where d.STATUS ='APPR' and e.COMPANY_KEY=d.VENDOR_KEY";
                SqlDataSource2.SelectCommand = "SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimPurchView] d, [ODW].[EAMS].[DimCompany] e where d.STATUS ='APPR' and e.COMPANY_KEY=d.VENDOR_KEY";
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT d.[CONTRACTID],d.[CONTRACTNUM],e.COMPANY,d.DESCRIPTION,e.NAME,e.COMPANY_KEY FROM [ODW].[EAMS].[DimPurchView] d, [ODW].[EAMS].[DimCompany] e where d.STATUS ='APPR' and e.COMPANY_KEY=d.VENDOR_KEY and e.NAME like '%" + txtSearch.Text.Trim() + "%'";
                SqlDataSource2.SelectCommand = "SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimPurchView] d, [ODW].[EAMS].[DimCompany] e where d.STATUS ='APPR' and e.COMPANY_KEY=d.VENDOR_KEY and e.NAME like '%" + txtSearch.Text.Trim() + "%'";
            }
            gvCustomers.DataBind();
            DataList5.DataBind();



        }
    }
}