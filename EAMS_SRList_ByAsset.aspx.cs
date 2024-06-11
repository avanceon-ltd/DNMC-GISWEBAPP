using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class EAMS_SRList_ByAsset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                    SqlDataSource1.SelectCommand = @"SELECT [TICKETID] as EAMS_SR_NUM,[DESCRIPTION] AS SR_Details,[STATUS],[EXT_SRGROUP] aS Service_Provider_Team,[LOCATION],(select DESCRIPTION from [EAMS].DimLocation where LOCATION = s.LOCATION) as Location_Description,[REPORTEDBY],[TARGETFINISH],[TARGETSTART],[CHANGEDATE] FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED')  and [ASSETNUM]='" + Convert.ToString(Request.QueryString["ID"]) + "'";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED')  and [ASSETNUM]= '" + Convert.ToString(Request.QueryString["ID"]) + "'";
                }
                else
                {

                    SqlDataSource1.SelectCommand = @"SELECT [TICKETID] as EAMS_SR_NUM,[DESCRIPTION] AS SR_Details,[STATUS],[EXT_SRGROUP] aS Service_Provider_Team,[LOCATION],(select DESCRIPTION from [EAMS].DimLocation where LOCATION = s.LOCATION) as Location_Description,[REPORTEDBY],[TARGETFINISH],[TARGETSTART],[CHANGEDATE] FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED') and [ASSETNUM]='" + Convert.ToString(Request.QueryString["ID"]) + "' AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'" + " order by[CHANGEDATE] desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[FactServiceRequest] s where STATUS not in ('CLOSED','RESOLVED','CANCELLED')  and [ASSETNUM]='" + Convert.ToString(Request.QueryString["ID"]) + "' AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'";
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the HyperLink control.
                LinkButton lnkWorkLog = (e.Row.FindControl("lnkWorkLog") as LinkButton);
                LinkButton lnkImages = (e.Row.FindControl("lnkImages") as LinkButton);
                string sr = e.Row.Cells[0].Text;
                if (!string.IsNullOrEmpty(sr))
                {
                    lnkWorkLog.PostBackUrl = "WorkLogs.aspx?isSR=1&id=" + sr + "&backlink=true" + "&AssetId=" + Request.QueryString["ID"];// + "&USER=" + _user;
                    //lnkWorkLog.PostBackUrl = "WorkLogs.aspx?isSR=1&id=" + sr + "&backlink=true" + "&AssetId=" + Request.QueryString["ID"];// + "&USER=" + _user;
                    lnkImages.Attributes.Add("onclick", "window.open('GetImgAttachByWo.aspx?IsSR=1&ID=" + sr + "','_blank', 'width=760,height=500')");
                }
            }
        }
    }
}