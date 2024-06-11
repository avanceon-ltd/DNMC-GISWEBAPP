using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class EAMS_SRList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                    SqlDataSource1.SelectCommand = @"SELECT [TICKETID] as EAMS_SR_NUM,[DESCRIPTION] AS SR_Details,[STATUS], [EXT_DEFECTTYPE] as DEFECT_TYPE,[EXT_SRGROUP] aS Service_Provider_Team,[REPORTEDBY],[TARGETFINISH],[TARGETSTART],[CHANGEDATE] FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED') and [EXT_SRTYPE]='Request' order by [CHANGEDATE] desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED') and[EXT_SRTYPE]='Request' ";
                }
                else
                {

                    SqlDataSource1.SelectCommand = @"SELECT [TICKETID] as EAMS_SR_NUM,[DESCRIPTION] AS SR_Details,[STATUS], [EXT_DEFECTTYPE] as DEFECT_TYPE,[EXT_SRGROUP] aS Service_Provider_Team,[REPORTEDBY],[TARGETFINISH],[TARGETSTART],[CHANGEDATE]  FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED') and [EXT_SRTYPE]='Request' AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'" + " order by[CHANGEDATE] desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED') and [EXT_SRTYPE]='Request'  AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'";
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