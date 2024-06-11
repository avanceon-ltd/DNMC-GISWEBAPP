using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppForm.App_Code;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class ICCSAlarm : System.Web.UI.Page
    {
        string siteName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtArea.Attributes.Add("readonly", "readonly");
            if (!String.IsNullOrEmpty(Request.QueryString["SiteName"]))
                siteName = Request.QueryString["SiteName"];
            if (!Page.IsPostBack && !string.IsNullOrEmpty(siteName))
            {
                FillICCSContacts(siteName);
                FillAlarmGrid();
                if (!String.IsNullOrEmpty(Request.QueryString["ReportedBy"]))
                {
                    lblReportedBy.Text = Convert.ToString(Request.QueryString["ReportedBy"]); ;
                }
            }
        }
        void FillAlarmGrid()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["SiteName"]))
            {
                string siteName = Request.QueryString["SiteName"];

                string query = @" SELECT Top(10) Source_HierarchicalObject,EventTime,Source_Object,Source_HierarchicalArea, comment,Source_Area 
                        FROM [Events]
                        WHERE  EventTime between  DATEADD(HOUR, -24, GETDATE())  AND GetDate()  
                        AND (Source_HierarchicalObject like '" + siteName + ".%' OR Source_HierarchicalObject = '" + siteName +
                        "') AND Priority < 950 AND Alarm_State = 'UNACK_ALM' ORDER BY EventTime DESC ";
                SqlDataSource1.SelectCommand = query;
                gvAlarms.DataBind();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GenerateAlarm();
            }
        }
        void GenerateAlarm()
        {
            string reqPayload = "";
            DateTime reqTime = DateTime.Now;
            string alarmId = "0";
            string respPayload = "";
            DateTime respTime;
            string operation = "";
            string RESULT;

            try
            {
                string createUri = WebConfigurationManager.AppSettings["AlarmUri"]; //"http://10.154.70.25/DTCApi/api/Alarms"; //"http://iccsintsrv01/DTCApi/api/Alarms"; //10.154.70.25
                string authValue = "c2NhZGRhOmFuZ2Vs";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["Authorization"] = authValue;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    dynamic jsonReqeust = new JObject();
                    string[] contactName = ddlName.SelectedItem.Text.Split(':');
                    string[] contactTel = ddlContacts.SelectedItem.Text.Split('-');

                    jsonReqeust.AlarmTitle = txtTitle.Text + " - " + GetTitleId();
                    jsonReqeust.Priority = ddlPriority.SelectedValue;
                    jsonReqeust.UnitID = "SCADA";
                    jsonReqeust.AlarmDate = DateTime.Now.ToString("s");
                    jsonReqeust.OwnerName = lblReportedBy.Text;
                    jsonReqeust.Address1 = ddlDesignation.SelectedItem.Text.ToString().Trim();// txtNetwork.Text;
                    jsonReqeust.Address2 = ddlName.SelectedItem.Text.ToString().Trim();
                    jsonReqeust.Address3 = companyName.Value;
                    //jsonReqeust.Address4 = txtAsset.Text;
                    jsonReqeust.AlarmContactNo = ddlContacts.SelectedItem.Text.ToString().Trim();
                    jsonReqeust.Region = region.Value;
                    jsonReqeust.Contact1 = contact1.Value;// "Section Head";
                    jsonReqeust.ContactTel1 = contactTel1.Value; // "1234";
                    jsonReqeust.Contact2 = contact2.Value;// "Manager";
                    jsonReqeust.ContactTel2 = contactTel2.Value; // "4567";
                    jsonReqeust.Contact3 = contact3.Value; // "Supervisor";
                    jsonReqeust.ContactTel3 = contactTel3.Value;// "2233";
                    jsonReqeust.AlarmDetails = txtDetail.Text;

                    streamWriter.Write(jsonReqeust.ToString());

                    // set db parameters
                    reqTime = DateTime.Now;
                    reqPayload = jsonReqeust.ToString();
                    operation = "SCADA";
                }

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    if (jsonResult.SelectToken("ResponseCode").ToString().Equals("1"))
                    {
                        lblMessage.Text = "<b>" + jsonResult.SelectToken("ResponseMessage").ToString() + "</b>";
                        ClearControls();
                    }
                    else
                    {
                        lblMessage.Text = "<b>Error: </b>" + jsonResult.SelectToken("ResponseMessage").ToString() + " with Response Code: " + jsonResult.SelectToken("ResponseCode").ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                    //set db parameters
                    respPayload = jsonResult.ToString();
                    respTime = DateTime.Now;
                    alarmId = jsonResult.SelectToken("AlarmID").ToString();

                }
            }
            catch (WebException wex)
            {
                EventLogging.LogEvents(wex.StackTrace);
                lblMessage.Text = "<b> Problem with the alarm generation. Please contact system administrator";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                //set db parameters
                respPayload = wex.Message;
                respTime = DateTime.Now;

            }
            catch (Exception exp)
            {
                lblMessage.Text = exp.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                respPayload = exp.Message;
                respTime = DateTime.Now;
            }

            LogEvent(reqPayload, reqTime, alarmId, respPayload, respTime, operation);

        }
        private void LogEvent(string reqPayload, DateTime reqTime, string alarmId, string respPayload, DateTime respTime, string operation)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FuseConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("auditAlarmSP ", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@reqPayload", SqlDbType.Text).Value = reqPayload;
                    cmd.Parameters.AddWithValue("@reqTime", SqlDbType.DateTime).Value = reqTime;
                    cmd.Parameters.AddWithValue("@alarmId", SqlDbType.VarChar).Value = alarmId;
                    cmd.Parameters.AddWithValue("@respPayload", SqlDbType.Text).Value = respPayload;
                    cmd.Parameters.AddWithValue("@respTime", SqlDbType.DateTime).Value = respTime;
                    cmd.Parameters.AddWithValue("@operation", SqlDbType.VarChar).Value = operation;

                    cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 500);
                    cmd.Parameters["@RESULT"].Direction = ParameterDirection.Output;

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        string GetTitleId()
        {
            string title = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FuseConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Max(id) + 1 AS TitleID FROM [auditAlarms]", con))
                {
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    object titleId = cmd.ExecuteScalar();
                    title = titleId.ToString();
                }
            }

            return title;
        }
        private void ClearControls()
        {
            txtDetail.Text = "";
            txtTitle.Text = "";
            txtNetwork.Text = "";
            txtFramework.Text = "";
            txtArea.Text = "";
            txtAsset.Text = "";
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModal();", true);
        }

        void FillICCSContacts(string siteName)
        {
            string Facility_Type = String.Empty;
            string Framework = String.Empty;
            String RegionNetwork = string.Empty;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Facility_Type,Framework FROM [ODW].[SCADA].[MXLocations] where SiteName = '" + siteName + "'" ;
                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Facility_Type = string.IsNullOrEmpty(reader["Facility_Type"].ToString()) ? null : reader["Facility_Type"].ToString().Split('-')[0];
                        Framework = string.IsNullOrEmpty(reader["Framework"].ToString()) ? null : reader["Framework"].ToString().Split(' ')[1];
                    }
                }
            }
            if (!string.IsNullOrEmpty(Facility_Type) && !string.IsNullOrEmpty(Framework))
            {
                RegionNetwork = Framework + " " + Facility_Type;
                txtRegionName.Text = RegionNetwork;
                if (!string.IsNullOrEmpty(RegionNetwork))
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_CATEGORY] where CATEGORY_NAME LIKE '%" + RegionNetwork + "'";
                            cmd.Connection = con;

                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            ddlCategory.Items.Clear();
                            ddlCategory.Items.Add(new ListItem("-- Select Category --", "-1"));
                            while (reader.Read())
                            {
                                string ID = reader["ID"].ToString();
                                string CATEGORY_NAME = reader["CATEGORY_NAME"].ToString();
                                ddlCategory.Items.Add(new ListItem(CATEGORY_NAME, ID));
                            }
                        }
                    }
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] WHERE CATEGORY_ID IN (SELECT ID FROM [dbo].[TELEPHONE_CATEGORY] WHERE CATEGORY_NAME LIKE '%" + RegionNetwork + "')";
                            cmd.Connection = con;

                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            ddlDesignation.Items.Clear(); ddlName.Items.Clear(); ddlContacts.Items.Clear();
                            ddlDesignation.Items.Add(new ListItem("-- Select Designation --", "-1")); ddlName.Items.Add(new ListItem("-- Select Name --", "-1"));
                            ddlContacts.Items.Add(new ListItem("-- Select Contact --", "-1"));
                            while (reader.Read())
                            {
                                string ID = reader["ID"].ToString();
                                string Title = reader["Title"].ToString();
                                string Name = reader["Name"].ToString();
                                string Number1, Number2, Number3;

                                ddlDesignation.Items.Add(new ListItem(Title.Trim(), ID));
                                ddlName.Items.Add(new ListItem(Name.Trim(), ID));

                                if (!string.IsNullOrEmpty(reader["Phone_Number"].ToString()))
                                {
                                    Number1 = reader["Phone_Number"].ToString() + "  (" + reader["Number_Alias"].ToString() + ")";
                                    ddlContacts.Items.Add(new ListItem(Number1.Trim(), ID));
                                }
                                if (!string.IsNullOrEmpty(reader["Number1"].ToString()))
                                {
                                    Number2 = reader["Number1"].ToString() + "  (" + reader["Number_Alias1"].ToString() + ")";
                                    ddlContacts.Items.Add(new ListItem(Number2.Trim(), ID));
                                }
                                if (!string.IsNullOrEmpty(reader["Number2"].ToString()))
                                {
                                    Number3 = reader["Number2"].ToString() + "  (" + reader["Number_Alias2"].ToString() + ")";
                                    ddlContacts.Items.Add(new ListItem(Number3.Trim(), ID));
                                }
                            }
                        }
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " SELECT TOP(1) [ID],[REGION_NAME],[NATIONAL_REGION] FROM [TELEPHONE_DIRECTORY].[dbo].[REGION] " +
                    " WHERE ID IN(SELECT[REGION_ID] FROM[TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_CATEGORY_REGION] " +
                    " WHERE CATEGORY_ID IN(SELECT ID FROM[dbo].[TELEPHONE_CATEGORY] WHERE CATEGORY_NAME = '" + siteName + "'))";
                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        region.Value = reader["REGION_NAME"].ToString();
                    }
                }
            }
        }

        protected void gvAlarms_SelectedIndexChanged(object sender, EventArgs e)
        {
            string alarmDate = gvAlarms.SelectedRow.Cells[1].Text;
            string title = gvAlarms.SelectedRow.Cells[2].Text;
            string description = gvAlarms.SelectedRow.Cells[3].Text;

            txtTitle.Text = title;
            txtDetail.Text = description;
            string[] hierarchy = gvAlarms.SelectedRow.Cells[4].Text.Split('.');
            txtNetwork.Text = hierarchy[1];
            txtFramework.Text = hierarchy[2];
            txtArea.Text = hierarchy[3];
            txtAsset.Text = hierarchy[4];

            txtHierarchy.Text = txtNetwork.Text + "\\" + txtFramework.Text + "\\" + txtArea.Text + "\\" + txtAsset.Text;
        }

        protected void ddlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlName.SelectedValue != "-1")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] WHERE NAME = '" + ddlName.SelectedItem + "' AND CATEGORY_ID = '" + ddlCategory.SelectedValue + "'";
                        cmd.Connection = con;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlContacts.Items.Clear();
                        ddlContacts.Items.Add(new ListItem("-- Select Contact --", "-1"));
                        while (reader.Read())
                        {
                            string ID = reader["ID"].ToString();
                            string Number1, Number2, Number3;

                            if (!string.IsNullOrEmpty(reader["Phone_Number"].ToString()))
                            {
                                Number1 = reader["Phone_Number"].ToString() + "  (" + reader["Number_Alias"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number1.Trim(), ID));
                            }
                            if (!string.IsNullOrEmpty(reader["Number1"].ToString()))
                            {
                                Number2 = reader["Number1"].ToString() + "  (" + reader["Number_Alias1"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number2.Trim(), ID));
                            }
                            if (!string.IsNullOrEmpty(reader["Number2"].ToString()))
                            {
                                Number3 = reader["Number2"].ToString() + "  (" + reader["Number_Alias2"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number3.Trim(), ID));
                            }
                        }
                    }
                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue != "-1")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] WHERE CATEGORY_ID = " + ddlCategory.SelectedValue;
                        cmd.Connection = con;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlDesignation.Items.Clear(); ddlName.Items.Clear(); ddlContacts.Items.Clear();
                        ddlDesignation.Items.Add(new ListItem("-- Select Designation --", "-1")); ddlName.Items.Add(new ListItem("-- Select Name --", "-1"));
                        ddlContacts.Items.Add(new ListItem("-- Select Contact --", "-1"));
                        while (reader.Read())
                        {
                            string ID = reader["ID"].ToString();
                            string Title = reader["Title"].ToString();
                            string Name = reader["Name"].ToString();
                            string Number1, Number2, Number3;

                            ddlDesignation.Items.Add(new ListItem(Title.Trim(), ID));
                            ddlName.Items.Add(new ListItem(Name.Trim(), ID));

                            if (!string.IsNullOrEmpty(reader["Phone_Number"].ToString()))
                            {
                                Number1 = reader["Phone_Number"].ToString() + "  (" + reader["Number_Alias"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number1.Trim(), ID));
                            }
                            if (!string.IsNullOrEmpty(reader["Number1"].ToString()))
                            {
                                Number2 = reader["Number1"].ToString() + "  (" + reader["Number_Alias1"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number2.Trim(), ID));
                            }
                            if (!string.IsNullOrEmpty(reader["Number2"].ToString()))
                            {
                                Number3 = reader["Number2"].ToString() + "  (" + reader["Number_Alias2"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number3.Trim(), ID));
                            }
                        }
                    }
                }
            }
        }
        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDesignation.SelectedValue != "-1")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] WHERE TITLE = '" + ddlDesignation.SelectedItem + "' AND CATEGORY_ID = '" + ddlCategory.SelectedValue + "'";
                        cmd.Connection = con;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlName.Items.Clear(); ddlContacts.Items.Clear();
                        ddlName.Items.Add(new ListItem("-- Select Name --", "-1")); ddlContacts.Items.Add(new ListItem("-- Select Contact --", "-1"));
                        while (reader.Read())
                        {
                            string ID = reader["ID"].ToString();
                            string Name = reader["Name"].ToString();
                            string Number1, Number2, Number3;

                            ddlName.Items.Add(new ListItem(Name.Trim(), ID));

                            if (!string.IsNullOrEmpty(reader["Phone_Number"].ToString()))
                            {
                                Number1 = reader["Phone_Number"].ToString() + "  (" + reader["Number_Alias"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number1.Trim(), ID));
                            }
                            if (!string.IsNullOrEmpty(reader["Number1"].ToString()))
                            {
                                Number2 = reader["Number1"].ToString() + "  (" + reader["Number_Alias1"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number2.Trim(), ID));
                            }
                            if (!string.IsNullOrEmpty(reader["Number2"].ToString()))
                            {
                                Number3 = reader["Number2"].ToString() + "  (" + reader["Number_Alias2"].ToString() + ")";
                                ddlContacts.Items.Add(new ListItem(Number3.Trim(), ID));
                            }
                        }
                    }
                }

            }
        }

        protected void ddlCategory_Init(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(siteName))
                FillICCSContacts(siteName);
        }
    }
}