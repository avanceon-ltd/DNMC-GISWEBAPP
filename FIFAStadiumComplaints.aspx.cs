using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebAppForm
{
    public partial class FIFAStadiumComplaints : System.Web.UI.Page
    {
        private string Stadium;
        private int Count;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Stadium"]))
            {
                Stadium = Convert.ToString(Request.QueryString["Stadium"]);
            }
            lblStadium.InnerText = Stadium;
            if (!this.IsPostBack)
            {
                txtLabel.InnerText = "Rain Water Complaints";
                gvOwner.DataSource = FormStadiumRainComplaintsView();
                gvOwner.DataBind();
                txtCount.InnerText = "(" + Count + ")";
            }
        }
        protected void btn_RainWater(object sender, EventArgs e)
        {
            txtLabel.InnerText = "Rain Water Complaints";
            gvOwner.DataSource = FormStadiumRainComplaintsView();
            gvOwner.DataBind();
            txtCount.InnerText = "(" + Count + ")";
        }
        protected void btn_NonRainWater(object sender, EventArgs e)
        {
            txtLabel.InnerText = "Non-Rain Water Complaints";
            gvOwner.DataSource = FormStadiumNonRainComplaintsView();
            gvOwner.DataBind();
            txtCount.InnerText = "(" + Count + ")";
        }
        private void FormStadiumRainandNonRainComplaints()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[GIS].[spStadiumComplaintsReview]";
                        command.Parameters.Add("@Stadium_Name", SqlDbType.VarChar, 200).Value = Stadium;
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataSet ds = new DataSet();

                        adapter.SelectCommand = command;
                        adapter.Fill(ds);

                        //txtRainUnAssigned.Text = Convert.ToString(ds.Tables[0].Rows[0]["UnAssigned"]);
                        //txtRainInPregress.Text = Convert.ToString(ds.Tables[0].Rows[0]["InProg"]);
                        //txtNonRainUnAssigned.Text = Convert.ToString(ds.Tables[1].Rows[0]["UnAssigned"]);
                        //txtNonRainInPregress.Text = Convert.ToString(ds.Tables[1].Rows[0]["InProg"]);
                        command.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        private DataTable FormStadiumRainComplaintsView()
        {
            DataTable dt = new DataTable("Stadium-complaints");

            dt.Columns.Add("Stadium_Name", typeof(string));
            dt.Columns.Add("UnAssigned", typeof(string));
            dt.Columns.Add("Assigned", typeof(string));
            dt.Columns.Add("UnAttended", typeof(string));
            dt.Columns.Add("InProg", typeof(string));
            dt.Columns.Add("Complete", typeof(string));
            dt.Columns.Add("Resolved", typeof(string));


            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[GIS].[spStadiumRainComplaintsReview]";
                        command.Parameters.Add("@Stadium_Name", SqlDbType.VarChar, 200).Value = Stadium;
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataSet ds = new DataSet();

                        adapter.SelectCommand = command;
                        adapter.Fill(ds);

                        DataRow newRow;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            newRow = dt.NewRow();
                            newRow["Stadium_Name"] = Convert.ToString(row["Stadium_Name"]);
                            newRow["UnAssigned"] = Convert.ToString(row["UnAssigned"]);
                            newRow["Assigned"] = Convert.ToString(row["Assigned"]);
                            newRow["UnAttended"] = Convert.ToString(row["UnAttended"]);
                            newRow["InProg"] = Convert.ToString(row["InProg"]);
                            newRow["Complete"] = Convert.ToString(row["Complete"]);
                            newRow["Resolved"] = Convert.ToString(row["Resolved"]);
                            Count = Convert.ToInt32(row["UnAssigned"]) + Convert.ToInt32(row["Assigned"]) + Convert.ToInt32(row["UnAttended"]) + Convert.ToInt32(row["InProg"]);
                            dt.Rows.Add(newRow);
                        }
                        command.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return dt;
        }

        private DataTable FormStadiumNonRainComplaintsView()
        {
            DataTable dt = new DataTable("Stadium-complaints");

            dt.Columns.Add("Stadium_Name", typeof(string));
            dt.Columns.Add("UnAssigned", typeof(string));
            dt.Columns.Add("Assigned", typeof(string));
            dt.Columns.Add("UnAttended", typeof(string));
            dt.Columns.Add("InProg", typeof(string));
            dt.Columns.Add("Complete", typeof(string));
            dt.Columns.Add("Resolved", typeof(string));


            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[GIS].[spStadiumNonRainComplaintsReview]";
                        command.Parameters.Add("@Stadium_Name", SqlDbType.VarChar, 200).Value = Stadium;
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataSet ds = new DataSet();

                        adapter.SelectCommand = command;
                        adapter.Fill(ds);

                        DataRow newRow;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            newRow = dt.NewRow();
                            newRow["Stadium_Name"] = Convert.ToString(row["Stadium_Name"]);
                            newRow["UnAssigned"] = Convert.ToString(row["UnAssigned"]);
                            newRow["Assigned"] = Convert.ToString(row["Assigned"]);
                            newRow["UnAttended"] = Convert.ToString(row["UnAttended"]);
                            newRow["InProg"] = Convert.ToString(row["InProg"]);
                            newRow["Complete"] = Convert.ToString(row["Complete"]);
                            newRow["Resolved"] = Convert.ToString(row["Resolved"]);
                            Count = Convert.ToInt32(row["UnAssigned"]) + Convert.ToInt32(row["Assigned"]) + Convert.ToInt32(row["UnAttended"]) + Convert.ToInt32(row["InProg"]);
                            dt.Rows.Add(newRow);
                        }
                        command.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return dt;
        }
    }
}