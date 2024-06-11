using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class ECCWOOwnersKPIs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindGridData();              
            }
        }

        private void BindGridData()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[GIS].[spOwnerAndPriorityWiseKPIs]";
                    command.CommandTimeout = 600;

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    
                    adapter.SelectCommand = command;
                    adapter.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("SortOrder", typeof(System.Int32));
                    dt.Columns["SortOrder"].DefaultValue = 10;

                    int count = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["OwnerName"].ToString().ToLower().Contains("north"))
                        {
                            dt.Rows[count]["SortOrder"] = 1;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("south"))
                        {
                            dt.Rows[count]["SortOrder"] = 2;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("west"))
                        {
                            dt.Rows[count]["SortOrder"] = 3;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("bala"))
                        {
                            dt.Rows[count]["SortOrder"] = 4;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("hpd"))
                        {
                            dt.Rows[count]["SortOrder"] = 5;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("rpd"))
                        {
                            dt.Rows[count]["SortOrder"] = 6;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("fps"))
                        {
                            dt.Rows[count]["SortOrder"] = 7;
                        }
                        if (row["OwnerName"].ToString().ToLower().Equals("bc"))
                        {
                            dt.Rows[count]["SortOrder"] = 8;
                        }
                        if (row["OwnerName"].ToString().ToLower().Contains("dnpd"))
                        {
                            dt.Rows[count]["SortOrder"] = 9;
                        }
                        count++;
                    }

                    dt.DefaultView.Sort = "SortOrder ASC";

                    gvOwner.DataSource = dt;
                    gvOwner.DataBind();
                }
            }            
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGridData();
        }        
    }
}