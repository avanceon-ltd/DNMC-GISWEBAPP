using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using System.Windows;

namespace WebAppForm
{
    public partial class AddICCSContact : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AddICCSContact));
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        private static string constr = ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string isEdit = Request.QueryString["isEdit"];
                    if (!string.IsNullOrEmpty(isEdit))
                    {
                        if(isEdit == "1") { FillData(true); }
                    }
                    else { FillData(false); }
                }
            }
            catch (Exception ex)
            {
                FillData(false);
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        private void FillData(bool isEdit)
        {
            try
            {
                SqlConnection myDBCon = new SqlConnection(constr);
                SqlCommand command = null; string sql = string.Empty;
                string REGION_Name = String.Empty; string Category_Name = String.Empty; string NAME = String.Empty;
                string TITLE = String.Empty; string NUMBER_ALIAS = String.Empty; string PHONE_NUMBER = String.Empty; string NUMBER_ALIAS1 = String.Empty;
                string NUMBER1 = String.Empty; string NUMBER_ALIAS2 = String.Empty; string NUMBER2 = String.Empty;

                sql = "SELECT DISTINCT(REGION_NAME) FROM [REGION]";
                command = new SqlCommand(sql, myDBCon);
                myDBCon.Open();
                SqlDataAdapter mySqlAdapter = new SqlDataAdapter();
                mySqlAdapter.SelectCommand = command;
                DataSet ds = new DataSet();
                mySqlAdapter.Fill(ds);
                Regions.DataSource = command.ExecuteReader();
                Regions.DataTextField = "REGION_NAME";
                Regions.DataBind();
                Regions.Items.Insert(0, new ListItem("", ""));
                myDBCon.Close();

                command = new SqlCommand("SELECT DISTINCT(CATEGORY_NAME) FROM [TELEPHONE_CATEGORY] WHERE CATEGORY_NAME <> '' AND CATEGORY_NAME IS NOT NULL", myDBCon);
                myDBCon.Open();
                mySqlAdapter.SelectCommand = command;
                mySqlAdapter.Fill(ds);
                Category.DataSource = command.ExecuteReader();
                Category.DataTextField = "CATEGORY_NAME";
                Category.DataBind();
                Category.Items.Insert(0, new ListItem("", ""));
                myDBCon.Close();

                //command = new SqlCommand("SELECT DISTINCT(TITLE) FROM [TELEPHONE_DETAILS] WHERE TITLE <> '' AND TITLE IS NOT NULL", myDBCon);
                //myDBCon.Open();
                //mySqlAdapter.SelectCommand = command;
                //mySqlAdapter.Fill(ds);
                //Designations.DataSource = command.ExecuteReader();
                //Designations.DataTextField = "TITLE";
                //Designations.DataBind();
                //Designations.Items.Insert(0, new ListItem("", ""));
                //myDBCon.Close();

                //command = new SqlCommand("SELECT DISTINCT(NUMBER_ALIAS1) FROM [TELEPHONE_DETAILS] WHERE NUMBER_ALIAS1 <> '' AND NUMBER_ALIAS1 IS NOT NULL", myDBCon);
                //myDBCon.Open();
                //mySqlAdapter.SelectCommand = command;
                //mySqlAdapter.Fill(ds);
                //CS1.DataSource = command.ExecuteReader();
                //CS1.DataTextField = "NUMBER_ALIAS1";
                //CS1.DataBind();
                //CS1.Items.Insert(0, new ListItem("", ""));
                //myDBCon.Close();

                //command = new SqlCommand("SELECT DISTINCT(NUMBER_ALIAS1) FROM [TELEPHONE_DETAILS] WHERE NUMBER_ALIAS1 <> '' AND NUMBER_ALIAS1 IS NOT NULL", myDBCon);
                //myDBCon.Open();
                //mySqlAdapter.SelectCommand = command;
                //mySqlAdapter.Fill(ds);
                //CS2.DataSource = command.ExecuteReader();
                //CS2.DataTextField = "NUMBER_ALIAS1";
                //CS2.DataBind();
                //CS2.Items.Insert(0, new ListItem("", ""));
                //myDBCon.Close();

                //command = new SqlCommand("SELECT DISTINCT(NUMBER_ALIAS1) FROM [TELEPHONE_DETAILS] WHERE NUMBER_ALIAS1 <> '' AND NUMBER_ALIAS1 IS NOT NULL", myDBCon);
                //myDBCon.Open();
                //mySqlAdapter.SelectCommand = command;
                //mySqlAdapter.Fill(ds);
                //CS3.DataSource = command.ExecuteReader();
                //CS3.DataTextField = "NUMBER_ALIAS1";
                //CS3.DataBind();
                //CS3.Items.Insert(0, new ListItem("", ""));
                //myDBCon.Close();

                if (isEdit)
                {
                    // Change Text
                    btnSubmit.Text = "Update";
                    Heading.Text = "Edit Contact Information";
                    Page.Title = "Edit Contact Information";

                    //Show Cancel Button
                    btncancel.Visible = true;

                    sql = "SELECT R.Region_Name,TC.Category_Name,TD.[PHONE_NUMBER],TD.[NAME],TD.[CATEGORY_ID],TD.[NUMBER_ALIAS],TD.[NUMBER1],TD.[NUMBER_ALIAS1],TD.[NUMBER2]," +
                        "TD.[NUMBER_ALIAS2],TD.[TITLE] FROM[TELEPHONE_DETAILS] TD Join TELEPHONE_CATEGORY_REGION TCR ON TD.[CATEGORY_ID] = TCR.[CATEGORY_ID] " +
                        "Join[dbo].[REGION] R ON R.ID = TCR.Region_ID join[TELEPHONE_CATEGORY] TC ON TC.ID = TD.Category_ID WHERE TD.ID = '" + Request.QueryString["id"] + "'";
                    SqlConnection cnn = new SqlConnection(constr);
                    cnn.Open();
                    DataSet DataSet = SqlHelper.ExecuteDataset(cnn, CommandType.Text, sql);
                    cnn.Close();
                    if (DataSet.Tables.Count > 0)
                    {
                        DataTable dt = DataSet.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            REGION_Name = (dr["REGION_Name"] == DBNull.Value ? "" : Convert.ToString(dr["REGION_Name"])).Trim();
                            Category_Name = (dr["Category_Name"] == DBNull.Value ? "" : Convert.ToString(dr["Category_Name"])).Trim();
                            NAME = (dr["NAME"] == DBNull.Value ? "" : Convert.ToString(dr["NAME"])).Trim();
                            TITLE = (dr["TITLE"] == DBNull.Value ? "" : Convert.ToString(dr["TITLE"])).Trim();
                            NUMBER_ALIAS = (dr["NUMBER_ALIAS"] == DBNull.Value ? "" : Convert.ToString(dr["NUMBER_ALIAS"])).Trim();
                            PHONE_NUMBER = (dr["PHONE_NUMBER"] == DBNull.Value ? "" : Convert.ToString(dr["PHONE_NUMBER"])).Trim();
                            NUMBER_ALIAS1 = (dr["NUMBER_ALIAS1"] == DBNull.Value ? "" : Convert.ToString(dr["NUMBER_ALIAS1"])).Trim();
                            NUMBER1 = (dr["NUMBER1"] == DBNull.Value ? "" : Convert.ToString(dr["NUMBER1"])).Trim();
                            NUMBER_ALIAS2 = (dr["NUMBER_ALIAS2"] == DBNull.Value ? "" : Convert.ToString(dr["NUMBER_ALIAS2"])).Trim();
                            NUMBER2 = (dr["NUMBER2"] == DBNull.Value ? "" : Convert.ToString(dr["NUMBER2"])).Trim();
                        }
                    }
                    if (Regions.Items.FindByText(REGION_Name) != null) {Regions.SelectedIndex = Regions.Items.IndexOf(Regions.Items.FindByValue(REGION_Name)); }
                    if (Category.Items.FindByText(Category_Name) != null) { Category.SelectedIndex = Category.Items.IndexOf(Category.Items.FindByValue(Category_Name)); }
                    if (CS1.Items.FindByText(NUMBER_ALIAS) != null) { CS1.SelectedIndex = CS1.Items.IndexOf(CS1.Items.FindByValue(NUMBER_ALIAS)); }
                    if (CS2.Items.FindByText(NUMBER_ALIAS1) != null) { CS2.SelectedIndex = CS2.Items.IndexOf(CS2.Items.FindByValue(NUMBER_ALIAS1)); }
                    if (CS3.Items.FindByText(NUMBER_ALIAS2) != null) { CS3.SelectedIndex = CS3.Items.IndexOf(CS3.Items.FindByValue(NUMBER_ALIAS2)); }
                    if (!string.IsNullOrEmpty(TITLE)) { Designations.Value = TITLE; }
                    if (!string.IsNullOrEmpty(NAME)) { Persons.Value = NAME; }
                    if (!string.IsNullOrEmpty(PHONE_NUMBER)) { CN1.Value = PHONE_NUMBER; }
                    if (!string.IsNullOrEmpty(NUMBER1)) { CN2.Value = NUMBER1; }
                    if (!string.IsNullOrEmpty(NUMBER2)) { CN3.Value = NUMBER2; }
                }

            }
            catch(Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        private void UpdateData()
        {
            try
            {
                string CategoryID = getCategoryID(Regions.SelectedItem.Text, Category.SelectedItem.Text);
                string sql = "UPDATE [dbo].[TELEPHONE_DETAILS] SET[PHONE_NUMBER] = '" + CN1.Value + "',[NAME] = '" + Persons.Value + "'," +
                    "[CATEGORY_ID] = '" + CategoryID + "',[NUMBER_ALIAS] = '" + CS1.SelectedItem.Value + "',[NUMBER1] = '" + CN2.Value + "'," +
                    "[NUMBER_ALIAS1] = '" + CS2.SelectedItem.Value + "',[NUMBER2] = '" + CN3.Value + "',[NUMBER_ALIAS2] = '" + CS3.SelectedItem.Value + "'," +
                    "[TITLE] = '" + Designations.Value + "' WHERE ID = '" + Request.QueryString["id"] + "'";

                string info = "Update ICCS Contact\nQuery\n" + sql + "\nparams";
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                lblMessage.Text = "Contact Updated successfully.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        private bool CheckContactExist(string region, string name, string categoryID)
        {
            bool result = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM dbo.TELEPHONE_DETAILS WHERE CATEGORY_ID = " + categoryID + " AND NAME = '" + name + "' AND   (SELECT CASE WHEN EXISTS " +
                        "( SELECT ID FROM dbo.REGION WHERE REGION_NAME = '" + region + "') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) = 1";
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace.ToString());
            }

            return result;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // 1 mean valid=true , 0 mean valid = false , 2 mean data already available
                if (string.IsNullOrEmpty(Regions.SelectedItem.Text) || string.IsNullOrEmpty(Category.SelectedItem.Text) || string.IsNullOrEmpty(Persons.Value)
                    || string.IsNullOrEmpty(Designations.Value) || string.IsNullOrEmpty(CS1.SelectedItem.Text) || string.IsNullOrEmpty(CN1.Value))
                {
                    lblMessage.Text = "<b>Required field are missing:</b> Please fill all the Red '*' marked filed.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }
                else
                {
                    string CategoryID = getCategoryID(Regions.SelectedItem.Text, Category.SelectedItem.Text);

                    if (CheckContactExist(Regions.SelectedItem.Text, Persons.Value, CategoryID) == true)
                    {
                        string isEdit = Request.QueryString["isEdit"];
                        if (!string.IsNullOrEmpty(isEdit))
                        {
                            if (isEdit == "1")
                            {
                                UpdateData();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "<b>Data already available:</b> DNMC System will not allow to add the already inserted records.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                    }
                    else
                    {
                        AddData();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        
        protected void contactList_Click(object sender,EventArgs e)
        {
            try
            {
                Response.Redirect("ICCSList.aspx",false);
            }
            catch(Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        private void AddData()
        {
            try
            {
                string query = "INSERT INTO [dbo].[TELEPHONE_DETAILS] ([ID],[PHONE_NUMBER],[NAME],[CATEGORY_ID],[NUMBER_ALIAS],[NUMBER1],[NUMBER_ALIAS1],[NUMBER2],[NUMBER_ALIAS2],[TITLE],[PERSONAL]) " +
                        "VALUES(@ID, @PHONE_NUMBER, @NAME, @CATEGORY_ID,@NUMBER_ALIAS,@NUMBER1,@NUMBER_ALIAS1,@NUMBER2,@NUMBER_ALIAS2,@TITLE,@PERSONAL)";
                string info = "Add DW Site\nQuery\n" + query + "\nparams";
                string CategoryID = getCategoryID(Regions.SelectedItem.Text, Category.SelectedItem.Text);

                string MAXID = getMaxICCSContactID();
                MAXID = (Convert.ToInt32(Convert.ToDouble(MAXID)) + 1).ToString();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@ID", MAXID);
                        cmd.Parameters.AddWithValue("@PHONE_NUMBER", CN1.Value);
                        cmd.Parameters.AddWithValue("@NAME", Persons.Value);
                        cmd.Parameters.AddWithValue("@CATEGORY_ID", CategoryID);
                        cmd.Parameters.AddWithValue("@NUMBER_ALIAS", CS1.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@NUMBER1", CN2.Value);
                        cmd.Parameters.AddWithValue("@NUMBER_ALIAS1", CS2.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@NUMBER2", CN3.Value);
                        cmd.Parameters.AddWithValue("@NUMBER_ALIAS2", CS3.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@TITLE", Designations.Value);
                        cmd.Parameters.AddWithValue("@PERSONAL", "0".ToString());

                        foreach (SqlParameter param in cmd.Parameters)
                        {
                            info += "\n" + param.ParameterName + ": " + param.Value;
                        }

                        log.Info(info);

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                lblMessage.Text = "ICCS data added successfully.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            catch(Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        private string getMaxICCSContactID()
        {
            string result = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT MAX(ID) AS ID FROM dbo.TELEPHONE_DETAILS";
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = string.IsNullOrEmpty(reader["ID"].ToString()) ? "0" : reader["ID"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace.ToString());
            }

            return result;
        }
        protected void Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Sql = string.Empty;
            if (!string.IsNullOrEmpty(Regions.SelectedItem.Text))
            {
                Sql = "SELECT [CATEGORY_NAME] FROM [TELEPHONE_CATEGORY] WHERE ID in (SELECT category_id FROM[TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_CATEGORY_REGION] " +
                        "where region_id in (SELECT[ID] FROM[TELEPHONE_DIRECTORY].[dbo].[REGION] WHERE Region_Name = '" + Regions.SelectedItem.Text + "'))";
            }
            else
            {
                Sql = "SELECT DISTINCT(CATEGORY_NAME) FROM [TELEPHONE_CATEGORY]";
            }
            SqlConnection myDBCon = new SqlConnection(constr);
            SqlCommand command = null;
            command = new SqlCommand(Sql, myDBCon);
            myDBCon.Open();
            SqlDataAdapter mySqlAdapter = new SqlDataAdapter();
            mySqlAdapter.SelectCommand = command;
            DataSet ds = new DataSet();
            mySqlAdapter.Fill(ds);

            Category.DataSource = command.ExecuteReader();
            Category.DataTextField = "CATEGORY_NAME";
            Category.DataBind();
            Category.Items.Insert(0, new ListItem("", ""));

            myDBCon.Close();
        }
        private string getCategoryID(string Region, string category)
        {
            string ID = string.Empty;
            try
            {
                SqlConnection cnn = new SqlConnection(constr);
                string Sql = "SELECT [ID] FROM [TELEPHONE_CATEGORY] WHERE ID in (SELECT category_id FROM[TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_CATEGORY_REGION] " +
                    "where region_id in (SELECT[ID] FROM[TELEPHONE_DIRECTORY].[dbo].[REGION] WHERE Region_Name = '" + Region + "')) and Category_Name = '" + category + "'";

                cnn.Open();
                DataSet dsRegion = SqlHelper.ExecuteDataset(cnn, CommandType.Text, Sql);
                cnn.Close();

                if (dsRegion.Tables.Count > 0)
                {
                    DataTable dt = dsRegion.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        ID = dr["ID"] == DBNull.Value ? "" : Convert.ToString(dr["ID"]);
                    }
                }

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

            return ID;
        }

        
    }
}