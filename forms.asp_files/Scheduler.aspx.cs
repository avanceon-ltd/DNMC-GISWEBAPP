using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Scheduler : System.Web.UI.Page
    {
        string _companyKey = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CompanyKey"]))
            {
                _companyKey = Request.QueryString["CompanyKey"].ToString();
                //   SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["VandorID"]);
                SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where (COMPANY_KEY <> -1 and COMPANY_KEY = " + _companyKey +  " and STATUS = 'Active' and  personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";

                SqlDataSource1.DataBind();


                SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where ( COMPANY_KEY <> -1 and COMPANY_KEY =" + _companyKey + " and STATUS = 'Active' and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";

                SqlDataSource2.DataBind();


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

                if (!String.IsNullOrEmpty(_companyKey))
                {
                    SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where ( COMPANY_KEY <> -1  and COMPANY_KEY = " + _companyKey + " and STATUS = 'Active' and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where (COMPANY_KEY <> -1  and COMPANY_KEY = " + _companyKey + " and STATUS = 'Active' and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";

                }
                else
                {

                    SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and PERSONID in (select respparty from [EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";
                }
            }
            else
            {

                if (!String.IsNullOrEmpty(_companyKey))
                {
                    SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where (COMPANY_KEY <> -1  and COMPANY_KEY =" + _companyKey + " and STATUS = 'Active' and " + " PERSONID LIKE '" + txtSearch.Text.Trim() + "'" + "and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where (COMPANY_KEY <> -1  and COMPANY_KEY = " + _companyKey + " and STATUS = 'Active' and " + " PERSONID LIKE '" + txtSearch.Text.Trim() + "'" + "and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";

                }
                else

                {
                    SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and" + " PERSONID LIKE '" + txtSearch.Text.Trim() + "'" + " and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and" + " PERSONID LIKE '" + txtSearch.Text.Trim() + "'" + " and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";

                }
            }
            gvCustomers.DataBind();
            DataList5.DataBind();



        }
    }
}