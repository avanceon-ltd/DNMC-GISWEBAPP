using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class CRMS_SRList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                    SqlDataSource1.SelectCommand = @"SELECT Service_Request_Number,Application_Status AS CRMS_Status,Application_Stage,EAMS_SR_Number,Contact_Reason,Customer,Created_By,Details,Modified_On FROM ODW.crms.DrainageComplaints wHeRE Application_Status nOt in  ('CLOSED','CANCELLED')  OrdER bY Modified_On DesC";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM ODW.crms.DrainageComplaints wHeRE Application_Status nOt in  ('CLOSED','CANCELLED') ";
                }
                else
                {

                    SqlDataSource1.SelectCommand = @"SELECT Service_Request_Number,Application_Status AS CRMS_Status,Application_Stage,EAMS_SR_Number,Contact_Reason,Customer,Created_By,Details,Modified_On FROM ODW.crms.DrainageComplaints wHeRE Application_Status nOt in  ('CLOSED','CANCELLED') AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'" + " order by Modified_On desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM ODW.crms.DrainageComplaints wHeRE Application_Status nOt in  ('CLOSED','CANCELLED')  AND " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'";
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