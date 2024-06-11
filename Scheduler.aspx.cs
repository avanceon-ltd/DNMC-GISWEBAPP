using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Scheduler : System.Web.UI.Page
    {
        string _company = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Company"]))
            {
                _company = Request.QueryString["Company"].ToString();
            }

            if (!IsPostBack)
            {
                BindGridView("", "", "", "", 1, _company);
                //    BindGrid(_company);
            }
        }


        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, e.NewPageIndex + 1, _company);

        }

        protected void gvCustomers_DataBound(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, 1, _company);

        }

        //private void BindGrid(string CompanyName)
        //{
        //    if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
        //    {

        //        if (!String.IsNullOrEmpty(CompanyName))
        //        {
        //            //Search string null and QS not null

        //            SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where ([STATUS] = 'Active' AND COMPANY_KEY <> -1 and COMPANY_KEY = (Select  Company_Key From EAMS.DimCompany where COMPANY = '" + CompanyName + "') and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";
        //            SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where ([STATUS] = 'Active' AND COMPANY_KEY <> -1 and COMPANY_KEY = (Select  Company_Key From EAMS.DimCompany where COMPANY = '" + CompanyName + "') and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";

        //        }
        //        else
        //        {
        //            //Search string is null and QS is null
        //            SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";
        //            SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and PERSONID in (select respparty from [EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";
        //        }
        //    }
        //    else
        //    {

        //        if (!String.IsNullOrEmpty(CompanyName))
        //        {


        //            //search string is not null and QS not null
        //            SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where (COMPANY_KEY <> -1 and  COMPANY_KEY = (Select  Company_Key From EAMS.DimCompany where COMPANY = '" + CompanyName + "') and STATUS = 'Active' and " + " PERSONID LIKE '%" + txtSearch.Text.Trim() + "%'" + "and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";
        //            SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where (COMPANY_KEY <> -1 and  COMPANY_KEY = (Select  Company_Key From EAMS.DimCompany where COMPANY = '" + CompanyName + "') and STATUS = 'Active' and " + " PERSONID LIKE '%" + txtSearch.Text.Trim() + "%' and personid in (select respparty from [EAMS].[DimPersonGroup] where persongroup ='SCHEDULR'))";

        //        }
        //        else

        //        {
        //            //search string is not null and QS is null
        //            SqlDataSource1.SelectCommand = @"select PERSON_KEY,PERSONID,EXT_RESPSECTION,EXT_UNIT,EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and" + " PERSONID LIKE '%" + txtSearch.Text.Trim() + "%'" + " and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";
        //            SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows from [EAMS].[DimPerson] where COMPANY_KEY <> -1 and STATUS = 'Active' and PERSONID LIKE '%" + txtSearch.Text.Trim() + "%'" + " and PERSONID in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR')";

        //        }
        //    }
        //    gvCustomers.DataBind();
        //    DataList5.DataBind();



        //}


        public void BindGridView(string text1, string column1, string text2, string column2, int pageNo, string company)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvCustomers.PageSize = pageSize;
            string searchClause = "", query = "";
            if (!String.IsNullOrEmpty(text1))
                searchClause += " " + column1 + " LIKE '%" + text1 + "%'";
            if (!String.IsNullOrEmpty(text1) && !String.IsNullOrEmpty(text2))
                searchClause += ddlAndOr.SelectedValue;
            if (!String.IsNullOrEmpty(text2))
                searchClause += " " + column2 + " LIKE '%" + text2 + "%'";

            if (!String.IsNullOrEmpty(searchClause))
                searchClause = " AND (" + searchClause + ")";

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            //query1 = @"SELECT pv.[CONTRACTID],pv.[CONTRACTNUM],c.COMPANY,pv.DESCRIPTION,c.NAME,c.COMPANY_KEY 
            //        FROM [ODW].[EAMS].[DimPurchView] pv, [ODW].[EAMS].[DimCompany] c 
            //        where pv.STATUS ='APPR' and c.COMPANY_KEY=pv.VENDOR_KEY " + searchClause;

            query = @"select PERSON_KEY, PERSONID, EXT_RESPSECTION, EXT_UNIT, EXT_RESPAREA,[SUPERVISOR] from [EAMS].[DimPerson] where (COMPANY_KEY <> -1 and  COMPANY_KEY = (Select  Company_Key From EAMS.DimCompany where COMPANY = '" + company + "') " +
                      "and STATUS = 'Active' and personid in (select respparty from[EAMS].[DimPersonGroup] where persongroup = 'SCHEDULR') " + searchClause + ")";


            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows.Count);
                gvCustomers.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvCustomers.DataSource = dataSet.Tables[0];
                gvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }


    }
}