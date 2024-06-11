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


    public partial class DevTblToGrd : System.Web.UI.Page
    {


        string QueryTablename = string.Empty;
        string QueryColNames = string.Empty;
        string _connStr = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            // Query string value is there so now use it
            QueryTablename = Convert.ToString(Request.QueryString["TableName"]);

            if (!string.IsNullOrEmpty(Request.QueryString["col"]))
            QueryColNames = Convert.ToString(Request.QueryString["col"]);


            if (!this.IsPostBack)
            {
                SqlDataSource2.SelectCommand = "SELECT Count(*) AS TotalRows FROM [ODW]."+ QueryTablename;
                this.BindGrid(QueryTablename, QueryColNames);
            }

        }




        private void BindGrid(string parm, string col)
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_connStr))
            {

                con.Open();
                SqlCommand cmd;
                if (String.IsNullOrEmpty(col))
                    cmd = new SqlCommand("SELECT * FROM [ODW]." + parm, con);
                else
                    cmd = new SqlCommand("SELECT " + col + " FROM [ODW]." + parm, con);

                SqlDataAdapter sdadpr = new SqlDataAdapter();
                sdadpr.SelectCommand = cmd;
                sdadpr.Fill(dt);

            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }



        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.BindGrid(QueryTablename, QueryColNames);
        }
    }
}


