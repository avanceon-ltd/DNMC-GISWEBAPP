using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class Vehicle_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
                if (string.IsNullOrEmpty(txtField1.Text))
                {
                    SqlDataSource1.SelectCommand = @"SELECT Title as Vehicle_No,Movement_Status,Vehicle_Details,Section,Contractor,VehicleType,Driver_Name,Zone,Last_Update_Timestamp,Date_Created FROM ODW.AVLS.VehicleInfo order by Last_Update_Timestamp desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM ODW.AVLS.VehicleInfo";
                }
                else
                {

                    SqlDataSource1.SelectCommand = @"SELECT Title as Vehicle_No,Movement_Status,Vehicle_Details,Section,Contractor,VehicleType,Driver_Name,Zone,Last_Update_Timestamp,Date_Created FROM ODW.AVLS.VehicleInfo where " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'" + " order by Last_Update_Timestamp desc";
                    SqlDataSource2.SelectCommand = @"SELECT Count(*) AS TotalRows FROM ODW.AVLS.VehicleInfo where " + ddlField1.SelectedValue + " like'%" + txtField1.Text + "%'";
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