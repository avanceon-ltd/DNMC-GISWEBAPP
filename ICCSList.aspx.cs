using ClosedXML.Excel;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Input;

namespace WebAppForm
{
    public partial class ICCSList : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(ICCSList));
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString);
        private static string constr = ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSearch.UniqueID;

            if (!IsPostBack)
            {
                BindData(false);
            }
        }
        private void BindData(bool search)
        {
            try
            {
                conn.Open();
                string sql = "SELECT TD.[ID],TD.[PHONE_NUMBER],TD.[NAME],TD.[CATEGORY_ID],(SELECT CATEGORY_NAME fROM[TELEPHONE_CATEGORY] WHERE ID IN(TD.CATEGORY_ID)) AS 'Category_Name'," +
                    "TD.[NUMBER_ALIAS],TD.[NUMBER1],TD.[NUMBER_ALIAS1],TD.[NUMBER2],TD.[NUMBER_ALIAS2],TD.[TITLE]," +
                    "(seLECT REGION_NAME FROM REGION WHERE ID IN(SELECT TOP(1) REGION_ID FROM TELEPHONE_CATEGORY_REGION WHERE CATEGORY_ID = TD.CATEGORY_ID) anD NATIONAL_REGION = 0) AS 'Region_Name'" +
                    "FROM[TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] TD";
                string Region_Name_Query = "(seLECT REGION_NAME FROM REGION WHERE ID IN(SELECT TOP(1) REGION_ID FROM TELEPHONE_CATEGORY_REGION WHERE CATEGORY_ID = TD.CATEGORY_ID) anD NATIONAL_REGION = 0) ";
                if (search)
                {
                    if (!string.IsNullOrEmpty(txtField1.Text.Trim()))
                    {
                        if (!string.IsNullOrEmpty(txtField2.Text.Trim()))
                        {
                            switch (SearchType1.SelectedItem.Text.Trim())
                            {
                                case "Region Name":
                                    sql += " WHERE ( " + Region_Name_Query + " Like '%" + txtField1.Text.Trim() + "%')";
                                    break;
                                case "Name":
                                    sql += " WHERE ([NAME] Like '%" + txtField1.Text.Trim() + "%')";
                                    break;
                                case "Contact Number":
                                    sql += " WHERE [PHONE_NUMBER] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER1] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER2] Like '%" + txtField1.Text.Trim() + "%')";
                                    break;
                                case "Category Name":
                                    sql += " WHERE ([Category_Name] Like '%" + txtField1.Text.Trim() + "%')";
                                    break;
                                case "Contact Source":
                                    sql += " WHERE ([NUMBER_ALIAS] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER_ALIAS1] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER_ALIAS2] Like '%" + txtField1.Text.Trim() + "%')";
                                    break;
                            }

                            switch (SearchType2.SelectedItem.Text.Trim())
                            {
                                case "Region Name":
                                    sql += " " + Type.SelectedItem.Text.Trim() + " ( " + Region_Name_Query + " Like '%" + txtField2.Text.Trim() + "%')";
                                    break;
                                case "Name":
                                    sql += " " + Type.SelectedItem.Text.Trim() + " ([NAME] Like '%" + txtField2.Text.Trim() + "%')";
                                    break;
                                case "Contact Number":
                                    sql += " " + Type.SelectedItem.Text.Trim() + " ([PHONE_NUMBER] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER1] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER2] Like '%" + txtField2.Text.Trim() + "%')";
                                    break;
                                case "Category Name":
                                    sql += " " + Type.SelectedItem.Text.Trim() + " ([Category_Name] Like '%" + txtField2.Text.Trim() + "%')";
                                    break;
                                case "Contact Source":
                                    sql += " " + Type.SelectedItem.Text.Trim() + " ([NUMBER_ALIAS] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER_ALIAS1] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER_ALIAS2] Like '%" + txtField2.Text.Trim() + "%')";
                                    break;
                            }
                        }
                        else
                        {
                            switch (SearchType1.SelectedItem.Text.Trim())
                            {
                                case "Region Name":
                                    sql += " WHERE ( " + Region_Name_Query + "  Like '%" + txtField1.Text.Trim() + "%')";
                                    break;
                                case "Name":
                                    sql += " WHERE [NAME] Like '%" + txtField1.Text.Trim() + "%'";
                                    break;
                                case "Contact Number":
                                    sql += " WHERE [PHONE_NUMBER] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER1] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER2] Like '%" + txtField1.Text.Trim() + "%'";
                                    break;
                                case "Category Name":
                                    sql += " WHERE [Category_Name] Like '%" + txtField1.Text.Trim() + "%'";
                                    break;
                                case "Contact Source":
                                    sql += " WHERE [NUMBER_ALIAS] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER_ALIAS1] Like '%" + txtField1.Text.Trim() + "%' OR [NUMBER_ALIAS2] Like '%" + txtField1.Text.Trim() + "%'";
                                    break;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(txtField2.Text.Trim()))
                    {
                        switch (SearchType2.SelectedItem.Text.Trim())
                        {
                            case "Region Name":
                                sql += " WHERE ( " + Region_Name_Query + "  Like '%" + txtField2.Text.Trim() + "%')";
                                break;
                            case "Name":
                                sql += " WHERE [NAME] Like '%" + txtField2.Text.Trim() + "%'";
                                break;
                            case "Contact Number":
                                sql += " WHERE [PHONE_NUMBER] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER1] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER2] Like '%" + txtField2.Text.Trim() + "%'";
                                break;
                            case "Category Name":
                                sql += " WHERE [Category_Name] Like '%" + txtField2.Text.Trim() + "%'";
                                break;
                            case "Contact Source":
                                sql += " WHERE [NUMBER_ALIAS] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER_ALIAS1] Like '%" + txtField2.Text.Trim() + "%' OR [NUMBER_ALIAS2] Like '%" + txtField2.Text.Trim() + "%'";
                                break;
                        }
                    }
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                conn.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ICCSGrid.DataSource = ds;
                    ICCSGrid.DataBind();
                    Total_Records.Text = "Total Records: " + ds.Tables[0].Rows.Count.ToString();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    ICCSGrid.DataSource = ds;
                    ICCSGrid.DataBind();
                    int columncount = ICCSGrid.Rows[0].Cells.Count;
                    ICCSGrid.Rows[0].Cells.Clear();
                    ICCSGrid.Rows[0].Cells.Add(new TableCell());
                    ICCSGrid.Rows[0].Cells[0].ColumnSpan = columncount;
                    ICCSGrid.Rows[0].Cells[0].Text = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AddICCSContact.aspx", false);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void btnImport_Click(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showUpload();", true);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void Upload_Click(object sender, EventArgs e)
        {
            try
            {
                //Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(FileUpload.PostedFile.InputStream))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    int lastrow = workSheet.LastRowUsed().RowNumber();
                    var rows = workSheet.Rows(1, lastrow);


                    foreach (IXLRow row in rows)
                    {
                        if (row.IsEmpty())
                            row.Delete();

                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }


                    }
                    int count = 0;
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        count++;

                        string Region = dataRow["Region & Network"].ToString();
                        string Category = dataRow["SCADA and Generalized Names"].ToString();

                        string CategoryID = getCategoryID(Region, Category);
                        SaveData(Region, dataRow, CategoryID,count);
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideUpload();", true);
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "Your file not uploaded";
                lblMessage.ForeColor = System.Drawing.Color.Red;
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
        private void SaveData(string region, DataRow dataRow, string categoryID,int count)
        {
            try
            {
                string query = string.Empty;
                string name = dataRow["Person NAME"].ToString();
                if (CheckContactExist(region, name, categoryID) == false)
                {
                    query = "INSERT INTO [dbo].[TELEPHONE_DETAILS] ([ID],[PHONE_NUMBER],[NAME],[CATEGORY_ID],[NUMBER_ALIAS],[NUMBER1],[NUMBER_ALIAS1],[NUMBER2],[NUMBER_ALIAS2],[TITLE],[PERSONAL]) " +
                        "VALUES(@ID, @PHONE_NUMBER, @NAME, @CATEGORY_ID,@NUMBER_ALIAS,@NUMBER1,@NUMBER_ALIAS1,@NUMBER2,@NUMBER_ALIAS2,@TITLE,@PERSONAL)";


                    string info = "Add ICCS Contact\nQuery\n" + query + "\nparams";

                    string MAXID = String.IsNullOrEmpty(getMaxICCSContactID())? "1": getMaxICCSContactID();
                    MAXID = (Convert.ToInt32(Convert.ToDouble(MAXID)) + 1).ToString();
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Parameters.AddWithValue("@ID", MAXID);
                            cmd.Parameters.AddWithValue("@PHONE_NUMBER", dataRow["Contact Number1"].ToString());
                            cmd.Parameters.AddWithValue("@NAME", dataRow["Person NAME"].ToString());
                            cmd.Parameters.AddWithValue("@CATEGORY_ID", categoryID);
                            cmd.Parameters.AddWithValue("@NUMBER_ALIAS", dataRow["Contact Source 1"].ToString());
                            cmd.Parameters.AddWithValue("@NUMBER1", dataRow["Contact Number2"].ToString());
                            cmd.Parameters.AddWithValue("@NUMBER_ALIAS1", dataRow["Contact Source 2"].ToString());
                            cmd.Parameters.AddWithValue("@NUMBER2", dataRow["Contact Number3"].ToString());
                            cmd.Parameters.AddWithValue("@NUMBER_ALIAS2", dataRow["Contact Source 3"].ToString());
                            cmd.Parameters.AddWithValue("@TITLE", dataRow["Designation"].ToString());
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

                    lblMessage.Text = count + " - Contact Uploaded successfully.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }
                else
                {
                    lblMessage.Text = "<b>Data already available:</b> DNMC System will not allow to add the already inserted records.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
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
                                result = reader["ID"].ToString();
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
        private string getCategoryID(string Region, string category)
        {
            string ID = string.Empty;
            try
            {
                SqlConnection cnn = new SqlConnection(constr);
                string Sql = "SELECT [ID] FROM [TELEPHONE_CATEGORY] WHERE ID in (SELECT category_id FROM[TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_CATEGORY_REGION] " +
                    "where region_id in (SELECT[ID] FROM[TELEPHONE_DIRECTORY].[dbo].[REGION] WHERE Region_Name = '" + Region + "')) and Category_Name LIKE '" + category + "%'";

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
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

            return ID;
        }
        protected void ICCSGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ICCSGrid.PageIndex = e.NewPageIndex;
                BindData(false);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void ICCSGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                GridViewRow row = (GridViewRow)ICCSGrid.Rows[e.RowIndex];
                Label lbldeleteid = (Label)row.FindControl("lblID");
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM [TELEPHONE_DETAILS] WHERE ID='" + ICCSGrid.DataKeys[e.RowIndex].Value.ToString() + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                BindData(false);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void ICCSGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string item = e.Row.Cells[0].Text;
                    foreach (Button button in e.Row.Cells[13].Controls.OfType<Button>())
                    {
                        if (button.CommandName == "Delete")
                        {
                            button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindData(true);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void ICCSGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                string ID = ICCSGrid.DataKeys[e.NewEditIndex].Value.ToString();
                Response.Redirect("AddICCSContact.aspx?isEdit=1&id=" + ID, false);
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(ICCSList).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string ICCSList_sql = "SELECT (seLECT REGION_NAME FROM REGION WHERE ID IN(SELECT TOP(1) REGION_ID FROM TELEPHONE_CATEGORY_REGION WHERE CATEGORY_ID = d.CATEGORY_ID) anD NATIONAL_REGION = 0) AS 'Region & Network'," +
                    "(SELECT CATEGORY_NAME fROM[TELEPHONE_CATEGORY] WHERE ID IN(d.CATEGORY_ID)) AS 'SCADA and Generalized',[NAME],[TITLE],[NUMBER_ALIAS] AS 'Contact Source 1',[PHONE_NUMBER] AS 'Contact Number 1'," +
                    "[NUMBER_ALIAS1] AS 'Contact Source 2',[NUMBER1] AS 'Contact Number 2',d.NUMBER_ALIAS2 AS 'Contact Source 3',d.NUMBER2 AS 'Contact Number 3' FROM[TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS] d";

                string Telephone_Details_sql = "SELECT *  FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_DETAILS]";
                string Region_sql = "SELECT [ID],[REGION_NAME]  FROM [TELEPHONE_DIRECTORY].[dbo].[REGION]";
                string Category_sql = "SELECT [ID],[CATEGORY_NAME]  FROM [TELEPHONE_DIRECTORY].[dbo].[TELEPHONE_CATEGORY]";

                DataTable ICCSList = GetExportData(ICCSList_sql);
                DataTable Telephone_Details = GetExportData(Telephone_Details_sql);
                DataTable Region = GetExportData(Region_sql);
                DataTable Category = GetExportData(Category_sql);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ICCSList, "ICCSList");
                    wb.Worksheets.Add(Telephone_Details, "Telephone_Details");
                    wb.Worksheets.Add(Region, "Region");
                    wb.Worksheets.Add(Category, "Category");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ICCSContactData.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace.ToString());

            }
        }
        private DataTable GetExportData(string Query)
        {
            string constr = ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(Query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;

                        }
                    }
                }
            }
        }
    }
}