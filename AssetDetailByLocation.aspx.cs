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
    public partial class AssetDetailByLocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                    SqlDataSource1.SelectCommand = @"SELECT [ASSETNUM],[EXT_ASSETTAG],[DESCRIPTION],[STATUS],[ASSET_TYPE_DESCRIPTION],(SELECT[NAME] FROM [ODW].[EAMS].[DimCompany] where COMPANY_KEY =[MAINTAINER_KEY]) As 'Asset_Maintainer',(SELECT[NAME] FROM [ODW].[EAMS].[DimCompany] where COMPANY_KEY =[OPERATOR_KEY]) As 'Asset_Operator',[CHANGEDATE],(SELECT COUNT(*) FROM EAMS.DimAsset a WHERE a.PARENT=Assetvw.ASSETNUM)  as CountVal FROM [ODW].[EAMS].[vwDimAsset] Assetvw where LOCATION_KEY in (select LOCATION_KEY FROM[ODW].[EAMS].[DimLocation] where LOCATION ='" + Convert.ToString(Request.QueryString["ID"]) + "') ORDER BY  CountVal desc";

                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[vwDimAsset] where LOCATION_KEY in (select LOCATION_KEY FROM[ODW].[EAMS].[DimLocation] where LOCATION ='" + Convert.ToString(Request.QueryString["ID"]) + "')";

                }
                else
                {
                    SqlDataSource1.SelectCommand = @"SELECT [ASSETNUM],[EXT_ASSETTAG],[DESCRIPTION],[STATUS],[ASSET_TYPE_DESCRIPTION],(SELECT[NAME] FROM [ODW].[EAMS].[DimCompany] where COMPANY_KEY =[MAINTAINER_KEY]) As 'Asset_Maintainer',(SELECT[NAME] FROM [ODW].[EAMS].[DimCompany] where COMPANY_KEY =[OPERATOR_KEY]) As 'Asset_Operator',[CHANGEDATE],(SELECT COUNT(*) FROM EAMS.DimAsset a WHERE a.PARENT=Assetvw.ASSETNUM)  as CountVal FROM [ODW].[EAMS].[vwDimAsset] Assetvw where LOCATION_KEY in (select LOCATION_KEY FROM[ODW].[EAMS].[DimLocation] where LOCATION ='" + Convert.ToString(Request.QueryString["ID"]) + "') AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%' ORDER BY  CountVal desc";

                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[vwDimAsset] where LOCATION_KEY in (select LOCATION_KEY FROM[ODW].[EAMS].[DimLocation] where LOCATION ='" + Convert.ToString(Request.QueryString["ID"]) + "') AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'";
                }

                gvCustomers.DataBind();
                DataList5.DataBind();
            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void btnCreateWO_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/CreateBAUWorkOrder.aspx?Asset=" + e.)
            string assetnum = gvCustomers.SelectedRow.Cells[0].Text;
        }

        protected void gvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Accessing BoundField Column.
            string assetnum = gvCustomers.SelectedRow.Cells[0].Text;
            Response.Redirect("~/CreateBAUWorkOrder.aspx?SCADAAssetID=" + assetnum);

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click1(object sender, EventArgs e)
        {

        }

        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString);
        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkSubAssemblies = e.Row.FindControl("lnkSubAssemblies") as LinkButton;
                string assetnum = e.Row.Cells[0].Text;
                string description = e.Row.Cells[2].Text;
                string query = @"SELECT COUNT(*) FROM EAMS.DimAsset a WHERE PARENT =  '" + assetnum + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object rows = cmd.ExecuteScalar();
                if (Convert.ToInt32(rows) > 0)
                {
                    lnkSubAssemblies.Text = "Details";
                    lnkSubAssemblies.OnClientClick = string.Format("return AssetInfo({0},'{1}')", assetnum, description);
                }

            }
            conn.Close();
        }
    }
}