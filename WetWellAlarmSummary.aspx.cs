using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class WetWellAlarmSummary : System.Web.UI.Page
    {
        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString_hammad"].ConnectionString);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateFrameZoneDropdown();
                PopulatFacilityTypeDropdown();
                string selectedFrameZoneValue = ddlFrameZone.SelectedValue;
                string selectedFacilityTypeValue = ddlFacilityType.SelectedValue;
                LoadAlarmData(selectedFrameZoneValue, selectedFacilityTypeValue);
                //BindHighAlarmData();
                //BindHighHighAlarmData();
                //BindOverflowData();
            }           
        }
        protected void PopulateFrameZoneDropdown()
        {
            ddlFrameZone.Items.Insert(0, new ListItem("All", "All"));
            // Define your SQL query to retrieve data from the database
            string sqlQuery = "select distinct(Framework) from SCADA.MXLocations";

          
                using (SqlCommand command = new SqlCommand(sqlQuery, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string itemText = "";
                            string itemValue = "";
                            if (reader["Framework"].ToString() == "")
                            {
                                itemText = "Other";
                                 itemValue = "Other";
                            }
                            else
                            {
                                 itemText = reader["Framework"].ToString();
                                 itemValue = reader["Framework"].ToString();

                            }

                        // Create a new ListItem and add it to the DropDownList
                        ListItem listItem = new ListItem(itemText, itemValue);
                            ddlFrameZone.Items.Add(listItem);
                        }
                    }
                    conn.Close();

                // Add an optional default item
            }
        }
        protected void PopulatFacilityTypeDropdown()
        {
            ddlFacilityType.Items.Insert(0, new ListItem("All", "All"));
            // Define your SQL query to retrieve data from the database
            string sqlQuery = "select distinct(Facility_Type) from SCADA.MXLocations";


            using (SqlCommand command = new SqlCommand(sqlQuery, conn))
            {
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string itemText = "";
                        string itemValue = "";
                        if (reader["Facility_Type"].ToString() == "")
                        {
                            itemText = "Other";
                            itemValue = "Other";
                        }
                        else
                        {
                            itemText = reader["Facility_Type"].ToString();
                            itemValue = reader["Facility_Type"].ToString();

                        }

                        // Create a new ListItem and add it to the DropDownList
                        ListItem listItem = new ListItem(itemText, itemValue);
                        ddlFacilityType.Items.Add(listItem);
                    }
                }
                conn.Close();

                // Add an optional default item
            }
        }
        protected void ddlFacilityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This event handler is called when the user selects an item from the dropdown.
            // You can access the selected value and perform actions based on it.
            string selectedFrameZoneValue = ddlFrameZone.SelectedValue;
            string selectedFacilityTypeValue = ddlFacilityType.SelectedValue;
            LoadAlarmData(selectedFrameZoneValue, selectedFacilityTypeValue);
            // Perform actions based on the selected value.
        }
        protected void ddlFrameZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This event handler is called when the user selects an item from the dropdown.
            // You can access the selected value and perform actions based on it.
            string selectedFrameZoneValue = ddlFrameZone.SelectedValue;
            string selectedFacilityTypeValue = ddlFacilityType.SelectedValue;
            LoadAlarmData(selectedFrameZoneValue, selectedFacilityTypeValue);
            // Perform actions based on the selected value.
        }
        protected void LoadAlarmData(string framework,string facilityType)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("EXEC [SCADA].sp_AlarmSummary @Framework = @Value1, @Facility_Type = @Value2", conn);
            cmd.Parameters.AddWithValue("@Value1", framework);
            cmd.Parameters.AddWithValue("@Value2", facilityType);
            cmd.CommandTimeout = 600;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[12] { new DataColumn("1"), new DataColumn("2"), new DataColumn("3"), new DataColumn("4"),
            //new DataColumn("5"), new DataColumn("6"), new DataColumn("7"),new DataColumn("8"), new DataColumn("9"), new DataColumn("10"), new DataColumn("11"),
            //new DataColumn("12")});
            conn.Close();
            
            if (ds.Tables[0].Rows.Count > 0)
            {

                dlHigh.DataSource = ds.Tables[0];
                dlHigh.DataBind();

            }
            else
            {
                dlHigh.DataSource = null;

                // Clear any existing items
                dlHigh.DataBind();
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                dlHighHigh.DataSource = ds.Tables[1];
                dlHighHigh.DataBind();
            }
            else
            {
                dlHighHigh.DataSource = null;

                // Clear any existing items
                dlHighHigh.DataBind();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                dlOverflow.DataSource = ds.Tables[2];
                dlOverflow.DataBind();
            }
            else
            {
                dlOverflow.DataSource = null;

                // Clear any existing items
                dlOverflow.DataBind();
            }
        }
        protected void BindHighAlarmData()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT StationName,PumpRunning,PV1,PV2 FROM SCADA.vwWetWellPSSummaryForAlarm WHERE Type = 'High Level Alarm'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[12] { new DataColumn("1"), new DataColumn("2"), new DataColumn("3"), new DataColumn("4"),
            //new DataColumn("5"), new DataColumn("6"), new DataColumn("7"),new DataColumn("8"), new DataColumn("9"), new DataColumn("10"), new DataColumn("11"),
            //new DataColumn("12")});
            conn.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dlHigh.DataSource = ds.Tables[0];
                dlHigh.DataBind();
            }
            else
            {
                //ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                //gvDWSites.DataSource = ds;
                //gvDWSites.DataBind();
                //int columncount = gvDWSites.Rows[0].Cells.Count;
                //gvDWSites.Rows[0].Cells.Clear();
                //gvDWSites.Rows[0].Cells.Add(new TableCell());
                //gvDWSites.Rows[0].Cells[0].ColumnSpan = columncount;
                //gvDWSites.Rows[0].Cells[0].Text = "No Records Found";
            }
        }
        protected void BindHighHighAlarmData()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT StationName,PumpRunning,PV1,PV2 FROM SCADA.vwWetWellPSSummaryForAlarm WHERE Type = 'High High Level Alarm'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[12] { new DataColumn("1"), new DataColumn("2"), new DataColumn("3"), new DataColumn("4"),
            //new DataColumn("5"), new DataColumn("6"), new DataColumn("7"),new DataColumn("8"), new DataColumn("9"), new DataColumn("10"), new DataColumn("11"),
            //new DataColumn("12")});
            conn.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dlHighHigh.DataSource = ds.Tables[0];
                dlHighHigh.DataBind();
            }
            else
            {
                //ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                //gvDWSites.DataSource = ds;
                //gvDWSites.DataBind();
                //int columncount = gvDWSites.Rows[0].Cells.Count;
                //gvDWSites.Rows[0].Cells.Clear();
                //gvDWSites.Rows[0].Cells.Add(new TableCell());
                //gvDWSites.Rows[0].Cells[0].ColumnSpan = columncount;
                //gvDWSites.Rows[0].Cells[0].Text = "No Records Found";
            }
        }

        protected void BindOverflowData()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT StationName,PumpRunning,PV1,PV2 FROM SCADA.vwWetWellPSSummaryForAlarm WHERE Type = 'Overflow Level Alarm'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[12] { new DataColumn("1"), new DataColumn("2"), new DataColumn("3"), new DataColumn("4"),
            //new DataColumn("5"), new DataColumn("6"), new DataColumn("7"),new DataColumn("8"), new DataColumn("9"), new DataColumn("10"), new DataColumn("11"),
            //new DataColumn("12")});
            conn.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dlOverflow.DataSource = ds.Tables[0];
                dlOverflow.DataBind();
            }
            else
            {
                //ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                //gvDWSites.DataSource = ds;
                //gvDWSites.DataBind();
                //int columncount = gvDWSites.Rows[0].Cells.Count;
                //gvDWSites.Rows[0].Cells.Clear();
                //gvDWSites.Rows[0].Cells.Add(new TableCell());
                //gvDWSites.Rows[0].Cells[0].ColumnSpan = columncount;
                //gvDWSites.Rows[0].Cells[0].Text = "No Records Found";
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            string selectedFrameZoneValue = ddlFrameZone.SelectedValue;
            string selectedFacilityTypeValue = ddlFacilityType.SelectedValue;
            LoadAlarmData(selectedFrameZoneValue, selectedFacilityTypeValue);
            //BindHighAlarmData();
            //BindHighHighAlarmData();
            //BindOverflowData();
        }

       
    }
}