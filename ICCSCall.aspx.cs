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

    public partial class ICCSCall : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                FillRegions("", ddlRegionNetwork);
                FillRegions("", ddlRegionSearch);
                if (!String.IsNullOrEmpty(Request.QueryString["ReportedBy"]))
                {
                    lblReportedBy.Text = Convert.ToString(Request.QueryString["ReportedBy"]); ;
                }

                BindGridView("", "", "", "","", 1);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GenerateAlarm();
            }
        }

        //        {
        //  "AlarmID": 0,
        //  "AlarmTitle": "string",
        //  "Priority": 0,
        //  "Status": 0,
        //  "UnitID": "string",
        //  "FunctionArea": "string",
        //  "AlarmDate": "2020-01-21T11:47:55.974Z",
        //  "OwnerName": "string",
        //  "Address1": "string",
        //  "Address2": "string",
        //  "Address3": "string",
        //  "Address4": "string",
        //  "PostCode": "string",
        //  "AlarmContactNo": "string",
        //  "Region": "string",
        //  "Contact1": "string",
        //  "ContactTel1": "string",
        //  "Contact2": "string",
        //  "ContactTel2": "string",
        //  "Contact3": "string",
        //  "ContactTel3": "string",
        //  "AlarmDetails": "string"
        //}
        void GenerateAlarm()
        {
            string reqPayload = "";
            DateTime reqTime = DateTime.Now;
            string alarmId = "0";
            string respPayload = "";
            DateTime respTime;
            string operation = "";

            try
            {
                string createUri = WebConfigurationManager.AppSettings["AlarmUri"]; //"http://10.154.70.25/DTCApi/api/Alarms"; //"http://iccsintsrv01/DTCApi/api/Alarms"; //10.154.70.25
                string authValue = "c2NhZGRhOmFuZ2Vs";// System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(createUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["Authorization"] = authValue;
                string unitId = "DSS";
                if (!String.IsNullOrEmpty(Request.QueryString["Source"]))
                {
                    unitId = Request.QueryString["Source"];
                }

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    dynamic jsonReqeust = new JObject();
                    string[] contactName = ddlName.SelectedItem.Text.Split(':');
                    string[] contactTel = ddlContacts.SelectedItem.Text.Split('-');
                    string catchment = "";
                    if (ddlCatchment.SelectedItem.Text.Length > 10)
                        catchment = ddlCatchment.SelectedItem.Text.Substring(0, 10) + ".";
                    else
                        catchment = ddlCatchment.SelectedItem.Text;
                    jsonReqeust.AlarmTitle = "From DNMC: " + catchment + " - " + GetTitleId();
                    jsonReqeust.Priority = ddlPriority.SelectedValue;
                    jsonReqeust.UnitID = unitId;
                    jsonReqeust.AlarmDate = DateTime.Now.ToString("s");
                    jsonReqeust.OwnerName = lblReportedBy.Text;
                    jsonReqeust.Address1 = contactName[0];
                    if (contactName.Length > 1)
                        jsonReqeust.Address2 = contactName[1];
                    jsonReqeust.Address3 = companyName.Value;
                    jsonReqeust.AlarmContactNo = contactTel[1];
                    jsonReqeust.Region = ddlRegionNetwork.SelectedItem.Text;
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
                    operation = "DASHBOARD";
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
                //con.Open();
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
                    int i = cmd.ExecuteNonQuery();
                    if (i == -1)
                    {
                        //txtTitle.Text = GetTitleId();
                    }
                }
            }
        }

        string GetTitleId()
        {
            string title = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FuseConnectionString"].ConnectionString))
            {
                //con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Max(id) + 1 AS TitleID FROM [auditAlarms]", con))
                {
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    object titleId = cmd.ExecuteScalar();
                    title = titleId.ToString(); //"SCADA-Alarm-" + titleId.ToString();
                }
            }

            return title;
        }


        private void ClearControls()
        {
            txtDetail.Text = "";
            txtAsset.Text = "";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //    Response.Redirect("~/CreateWorkOrder.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModal();", true);
        }
        void FillContacts()
        {
            if (ddlName.SelectedValue != "-1")
            {

                ddlContacts.Items.Clear();
                int id = Convert.ToInt32(ddlName.SelectedValue);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                {
                    //con.Open();
                    using (SqlCommand cmd = new SqlCommand())// "auditAlarmSP ", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT [ID],[PHONE_NUMBER],[NAME],[CATEGORY_ID],[ADDRESS1],[ADDRESS2],[ADDRESS3],[ADDRESS4],[COMMENT1],[COMMENT2] " +
                        ",[PERSONAL],[NUMBER_ALIAS],[NUMBER1],[NUMBER_ALIAS1],[NUMBER2],[NUMBER_ALIAS2],[NUMBER3],[NUMBER_ALIAS3],[NUMBER4],[NUMBER_ALIAS4],[TITLE] " +
                        " FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] " +
                        " WHERE ID = " + id;
                        cmd.Connection = con;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string contactSource1 = reader["NUMBER_ALIAS"].ToString();
                            string contactNumber1 = reader["PHONE_NUMBER"].ToString();
                            string fullContact = contactSource1 + " - " + contactNumber1;
                            if (!String.IsNullOrEmpty(contactSource1) || !String.IsNullOrEmpty(contactNumber1))
                                ddlContacts.Items.Add(new ListItem(fullContact, fullContact));

                            contactSource1 = reader["NUMBER_ALIAS1"].ToString();
                            contactNumber1 = reader["NUMBER1"].ToString();
                            fullContact = contactSource1 + " - " + contactNumber1;
                            if (!String.IsNullOrEmpty(contactSource1) || !String.IsNullOrEmpty(contactNumber1))
                                ddlContacts.Items.Add(new ListItem(fullContact, fullContact));

                            contactSource1 = reader["NUMBER_ALIAS2"].ToString();
                            contactNumber1 = reader["NUMBER2"].ToString();
                            fullContact = contactSource1 + " - " + contactNumber1;
                            if (!String.IsNullOrEmpty(contactSource1) || !String.IsNullOrEmpty(contactNumber1))
                                ddlContacts.Items.Add(new ListItem(fullContact, fullContact));

                            companyName.Value = reader["ADDRESS1"].ToString();
                            contact1.Value = reader["NUMBER_ALIAS"].ToString();
                            contactTel1.Value = reader["PHONE_NUMBER"].ToString(); ;
                            contact2.Value = reader["NUMBER_ALIAS1"].ToString();
                            contactTel2.Value = reader["NUMBER1"].ToString();
                            contact3.Value = reader["NUMBER_ALIAS2"].ToString();
                            contactTel3.Value = reader["NUMBER2"].ToString();

                            lblCompany.Text = reader["ADDRESS1"].ToString();
                        }
                    }
                }
            }
        }
        protected void ddlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillContacts();
        }

        void FillRegions(string region, DropDownList ddlR)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT [ID],[REGION_NAME],[NATIONAL_REGION]
                    FROM[TELEPHONE_DIRECTORY].[dbo].[REGION] 
                    WHERE REGION_NAME NOT IN('National')";
                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlR.Items.Clear();
                    ddlR.Items.Add(new ListItem("-- Select Region --", "-1"));

                    while (reader.Read())
                    {
                        string regionName = reader["REGION_NAME"].ToString();
                        string id = reader["ID"].ToString();
                        ddlR.Items.Add(new ListItem(regionName, id));
                    }

                    if (!String.IsNullOrEmpty(region))
                    {
                        string text = WebUtility.HtmlDecode(region);
                        ListItem item = ddlR.Items.FindByText(text);
                        if (item != null)
                            ddlR.Items.FindByText(text).Selected = true;
                    }
                }
            }
        }

        void FillCatchment(string regionId, string catchment, DropDownList ddlC)
        {
            if (String.IsNullOrEmpty(regionId)) return;
            int id = Convert.ToInt32(regionId);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT [ID],[CATEGORY_NAME]
                    FROM [dbo].[TELEPHONE_CATEGORY]
                    WHERE ID IN (SELECT [CATEGORY_ID]  FROM [dbo].[TELEPHONE_CATEGORY_REGION] WHERE REGION_ID = " + id + " )";
                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlC.Items.Clear();
                    ddlC.Items.Add(new ListItem("-- Select Catchment --", "-1"));

                    while (reader.Read())
                    {
                        string catName = reader["CATEGORY_NAME"].ToString();
                        string catId = reader["ID"].ToString();
                        ddlC.Items.Add(new ListItem(catName, catId));
                    }

                    if (!String.IsNullOrEmpty(catchment))
                    {
                        string text = WebUtility.HtmlDecode(catchment);
                        ListItem item = ddlC.Items.FindByText(text);

                        if (item != null)
                            ddlC.Items.FindByText(text).Selected = true;
                    }
                }
            }
        }

        void FillNames(string catchment, string selectedName, string selectedDesignation, string region)
        {
            if (String.IsNullOrEmpty(catchment)) return;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    //cmd.CommandText = "SELECT [ID],[PHONE_NUMBER],[NAME],[CATEGORY_ID],[ADDRESS1],[ADDRESS2],[ADDRESS3],[ADDRESS4],[COMMENT1],[COMMENT2] " +
                    //",[PERSONAL],[NUMBER_ALIAS],[NUMBER1],[NUMBER_ALIAS1],[NUMBER2],[NUMBER_ALIAS2],[NUMBER3],[NUMBER_ALIAS3],[NUMBER4],[NUMBER_ALIAS4],[TITLE] " +
                    //" FROM [dbo].[TELEPHONE_DETAILS] " +
                    //" WHERE CATEGORY_ID = " + Convert.ToInt32(catId);
                    cmd.CommandText = GetGeneralContactsQuery(catchment, region);
                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlName.Items.Clear();
                    ddlName.Items.Add(new ListItem("-- Select Name --", "-1"));

                    while (reader.Read())
                    {
                        string name = reader["NAME"].ToString();
                        string designation = reader["TITLE"].ToString();
                        string fullName = designation + name;
                        string id = reader["ID"].ToString();
                        ddlName.Items.Add(new ListItem(fullName, id));
                    }

                    if (!String.IsNullOrEmpty(selectedName) && !String.IsNullOrEmpty(selectedDesignation))
                    {
                        string fullName = selectedDesignation + selectedName;
                        string text = WebUtility.HtmlDecode(fullName);
                        ListItem item = ddlName.Items.FindByText(text);
                        if (item != null)
                            ddlName.Items.FindByText(text).Selected = true;
                    }
                }
            }
        }

        string GetGeneralContactsQuery(string catchmentName, string regionName)
        {
            string[] generalContacts = WebConfigurationManager.AppSettings["GeneralContacts"].Split(',');
            bool isGeneralSelected = false;
            string generalContactsQuery = "";
            foreach (string general in generalContacts)
            {
                if (general.ToLower().Contains(catchmentName.ToLower()))
                    isGeneralSelected = true;
                generalContactsQuery += " OR CATEGORY_NAME LIKE '%" + general + "%'";
            }

            string query = "";
            if (isGeneralSelected == true)
                query = @"  SELECT [ID],[PHONE_NUMBER],[NAME],[CATEGORY_ID],[ADDRESS1],[ADDRESS2],[ADDRESS3],[ADDRESS4],[COMMENT1],[COMMENT2] 
                    ,[PERSONAL],[NUMBER_ALIAS],[NUMBER1],[NUMBER_ALIAS1],[NUMBER2],[NUMBER_ALIAS2],[NUMBER3],[NUMBER_ALIAS3],[NUMBER4],[NUMBER_ALIAS4],[TITLE] FROM TELEPHONE_DETAILS
              WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM TELEPHONE_CATEGORY_REGION
              WHERE REGION_ID = (SELECT ID FROM REGION  WHERE REGION_NAME LIKE '%" + regionName + "%') AND CATEGORY_ID IN (SELECT ID FROM TELEPHONE_CATEGORY " +
              " WHERE CATEGORY_NAME like '%" + catchmentName + "%'))";
            else
                query = @"SELECT [ID],[PHONE_NUMBER],[NAME],[CATEGORY_ID],[ADDRESS1],[ADDRESS2],[ADDRESS3],[ADDRESS4],[COMMENT1],[COMMENT2] 
                    ,[PERSONAL],[NUMBER_ALIAS],[NUMBER1],[NUMBER_ALIAS1],[NUMBER2],[NUMBER_ALIAS2],[NUMBER3],[NUMBER_ALIAS3],[NUMBER4],[NUMBER_ALIAS4],[TITLE] FROM TELEPHONE_DETAILS
              WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM TELEPHONE_CATEGORY_REGION
              WHERE REGION_ID = (SELECT REGION_ID FROM TELEPHONE_CATEGORY_REGION
              WHERE CATEGORY_ID = (SELECT ID FROM TELEPHONE_CATEGORY
              WHERE CATEGORY_NAME = '" + catchmentName + "')) AND CATEGORY_ID IN (SELECT ID FROM TELEPHONE_CATEGORY " +
              " WHERE CATEGORY_NAME = '" + catchmentName + "' " + generalContactsQuery + "))";
            //OR CATEGORY_NAME like '%customer service%' OR CATEGORY_NAME like '%General%' OR CATEGORY_NAME like '%Operations%' OR CATEGORY_NAME like '%Maintenance%'))";

            return query;
        }

        protected void ddlRegionNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegionNetwork.SelectedValue != "-1")
            {
                ddlName.Items.Clear();
                lblCompany.Text = "";
                ddlContacts.Items.Clear();
                FillCatchment(ddlRegionNetwork.SelectedValue, "", ddlCatchment);
            }
        }

        protected void ddlCatchment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegionNetwork.SelectedValue != "-1")
            {
                lblCompany.Text = "";
                ddlContacts.Items.Clear();
                FillNames(ddlCatchment.SelectedItem.Text, "", "", ddlRegionNetwork.SelectedItem.Text);
            }
        }

        #region " Address Book Search Code "

        public void BindGridView(string text1, string column1, string text2, string column2, string Opr, int pageNo)
        {
            
            int pageSize = 15;//Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvCustomers.PageSize = pageSize;
            string searchClause = "", query = "";

            if (!String.IsNullOrEmpty(text1))
            {
                searchClause += column1 + " LIKE '%" + text1 + "%'";
            }

            if (!String.IsNullOrEmpty(text1) && !String.IsNullOrEmpty(text2))
            {
                //searchClause += ddlAndOr.SelectedValue;
                searchClause += Opr;
            }

            if (!String.IsNullOrEmpty(text2))
            {
                searchClause += " " + column2 + " LIKE '%" + text2 + "%'";
            }

            if (!String.IsNullOrEmpty(searchClause))
            {
                searchClause = " WHERE " + searchClause;
            }

            if (text1.Contains("Select Region"))
            {
                searchClause = "";
            }

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            query = @"SELECT d.ID,r.REGION_NAME,c.CATEGORY_NAME,d.TITLE,d.NAME,d.ADDRESS1,d.NUMBER_ALIAS,d.PHONE_NUMBER,d.NUMBER_ALIAS1,d.NUMBER1 
                    FROM TELEPHONE_DETAILS d LEFT JOIN TELEPHONE_CATEGORY c ON d.CATEGORY_ID = c.ID 
                    LEFT JOIN TELEPHONE_CATEGORY_REGION rc ON rc.CATEGORY_ID = c.ID 
                    LEFT JOIN REGION r ON r.ID = rc.REGION_ID " +
                    searchClause + " ORDER BY d.NAME ASC";

            cmd.CommandText = query;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = dataSet.Tables[0].Rows.Count;
                gvCustomers.VirtualItemCount = totalRows;
                lblRecordCount.Text = "Total Records: " + totalRows.ToString();
                gvCustomers.DataSource = dataSet.Tables[0];
                gvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            if (String.IsNullOrEmpty(txtField1.Text) && String.IsNullOrEmpty(txtField2.Text))
            {
                if (ddlCatchmentSearch.SelectedValue != "-1" && !String.IsNullOrEmpty(ddlCatchmentSearch.SelectedValue))
                    BindGridView(ddlRegionSearch.SelectedItem.Text.Trim(), "r.REGION_NAME", ddlCatchmentSearch.SelectedItem.Text.Trim(), "c.CATEGORY_NAME", " AND ", e.NewPageIndex + 1);
                else
                    BindGridView(ddlRegionSearch.SelectedItem.Text.Trim(), "r.REGION_NAME", "", "", " OR ", e.NewPageIndex + 1);                
            }
            else
            {
                BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, ddlAndOr.SelectedValue, e.NewPageIndex + 1);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, ddlAndOr.SelectedValue, 1);
            ddlRegionSearch.SelectedIndex = 0;
            ddlCatchmentSearch.Items.Clear();
        }

        #endregion

        protected void gvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Accessing BoundField Column.
            string name = gvCustomers.SelectedRow.Cells[0].Text;
            string designation = gvCustomers.SelectedRow.Cells[1].Text;
            string company = gvCustomers.SelectedRow.Cells[2].Text;
            string network = gvCustomers.SelectedRow.Cells[3].Text;
            string catchment = gvCustomers.SelectedRow.Cells[4].Text;
            string contact1 = gvCustomers.SelectedRow.Cells[5].Text;
            string contact2 = gvCustomers.SelectedRow.Cells[6].Text;


            FillRegions(network, ddlRegionNetwork);
            FillCatchment(ddlRegionNetwork.SelectedValue, catchment, ddlCatchment);
            FillNames(catchment, name, designation, network);
            lblCompany.Text = company;
            FillContacts();
            ////Accessing TemplateField Column controls.
            //string country = (GridView1.SelectedRow.FindControl("lblCountry") as Label).Text;

            //lblValues.Text = "<b>Name:</b> " + name + " <b>Country:</b> " + country;
        }

        protected void ddlRegionSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegionSearch.SelectedValue != "-1")
            {
                FillCatchment(ddlRegionSearch.SelectedValue, "", ddlCatchmentSearch);
                BindGridView(ddlRegionSearch.SelectedItem.Text.Trim(), "r.REGION_NAME", "", "", " OR ", 1);
            }
            else
            {
                FillCatchment(ddlRegionSearch.SelectedValue, "", ddlCatchmentSearch);
                BindGridView("", "", "", "", " OR ", 1);
            }

            ResetSearch();
        }

        void ResetSearch()
        {
            ddlField1.SelectedIndex = 0;
            txtField1.Text = "";
            ddlField2.SelectedIndex = 0;
            txtField2.Text = "";
            ddlAndOr.SelectedIndex = 0;
        }
        protected void ddlCatchmentSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCatchmentSearch.SelectedValue != "-1")
            {
                //FillCatchment(ddlRegionSearch.SelectedValue, "", ddlCatchmentSearch);
                BindGridView(ddlRegionSearch.SelectedItem.Text.Trim(), "r.REGION_NAME", ddlCatchmentSearch.SelectedItem.Text.Trim(), "c.CATEGORY_NAME", " AND ", 1);
                ResetSearch();
            }
        }
    }
}