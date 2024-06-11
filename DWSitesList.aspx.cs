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
    public partial class DWSitesList : System.Web.UI.Page
    {
        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }


            
        }
        protected void BindData()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Complaint");
            dt.Columns.Add("Description");
            dt.Columns.Add("AffectedPerson");
            dt.Columns.Add(new DataColumn("Select",typeof(bool)));
            DataRow row = dt.NewRow();
            row["Complaint"] = "21052023-0005";
            row["Description"] = "21052023-0005 – Rainwater Flooding";
            dt.Rows.Add(row);
           
              
            
        }
       
      
      

       
    }
}