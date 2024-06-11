using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class ExternalFloodComplaint : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the HyperLink control.
                HyperLink myHyperLink = (e.Row.FindControl("Hyp_detail") as HyperLink);
                HyperLink myHyperwOLink = (e.Row.FindControl("Hyp_WO") as HyperLink);
                HyperLink myHyperWoDetailLink = (e.Row.FindControl("Hyp_WOdetail") as HyperLink);
                HyperLink myHyperDispatchLink = (e.Row.FindControl("Hyp_dispatch") as HyperLink);

                //Label eams_sr_no = (e.Row.FindControl("Hidden_EAMS_SR_Number") as Label);
                Label eams_wo = (e.Row.FindControl("Hidden_WONUM") as Label);

                string _SR_No = e.Row.Cells[0].Text;

                if (!string.IsNullOrEmpty(_SR_No))
                {
                    myHyperLink.NavigateUrl = "javascript:window.open('CRMSComplaintDetail.aspx?id=" + _SR_No + "','PopupWindow','width=800,height=900'); ";
                    myHyperLink.Text = "Click Here";

                    if (string.IsNullOrEmpty(eams_wo.Text.Trim()))
                    {

                        myHyperwOLink.NavigateUrl = "javascript:window.open('CreateWOCustomer.aspx?CRMSId=" + _SR_No + "','PopupWindow','width=800,height=800'); ";
                        myHyperwOLink.Text = "Click Here";

                    }
                }

                if (!string.IsNullOrEmpty(eams_wo.Text.Trim()))
                {
                    myHyperWoDetailLink.NavigateUrl = "javascript:window.open('WorkOrderDetail.aspx?id=" + eams_wo.Text.Trim() + "&CRMSId=" + _SR_No + "','PopupWindow','width=800,height=950'); ";
                    myHyperWoDetailLink.Text = "Click Here";

                    myHyperDispatchLink.NavigateUrl = "javascript:window.open('DispatchWO.aspx?WONUM=" + eams_wo.Text.Trim() + "','PopupWindow','width=750,height=600'); ";
                    myHyperDispatchLink.Text = "Click Here";
                }

            }
        }




    }
}