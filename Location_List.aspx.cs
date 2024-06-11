using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Location_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                    SqlDataSource1.SelectCommand = @"SELECT LOCATION,DESCRIPTION,STATUS,LOC_TYPE_DESCRIPTION,LOC_STATUS_DESCRIPTION,ADDRESSCODE,EXT_DATALOADID,EXT_LOCATIONTAG,CHANGEDATE FROM ODW.EAMS.DimLocation order by CHANGEDATE desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM ODW.EAMS.DimLocation";
                }
                else
                {

                    SqlDataSource1.SelectCommand = @"SELECT LOCATION,DESCRIPTION,STATUS,LOC_TYPE_DESCRIPTION,LOC_STATUS_DESCRIPTION,ADDRESSCODE,EXT_DATALOADID,EXT_LOCATIONTAG,CHANGEDATE FROM ODW.EAMS.DimLocation where " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'" + " order by[CHANGEDATE] desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM ODW.EAMS.DimLocation where " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'";
                }

                gvCustomers.DataBind();
                DataList5.DataBind();
            
        }
        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}