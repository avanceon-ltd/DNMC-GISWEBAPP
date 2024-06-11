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
    public partial class DeleteICCSContacts : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AddICCSContact));
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        private static string constr = ConfigurationManager.ConnectionStrings["CapitaConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillRegions();
            }
        }

        private void FillRegions()
        {
            try
            {
                SqlConnection myDBCon = new SqlConnection(constr);
                SqlCommand command = null; string sql = string.Empty;
                string REGION_Name = String.Empty; string Category_Name = String.Empty; string NAME = String.Empty;
                string TITLE = String.Empty; string NUMBER_ALIAS = String.Empty; string PHONE_NUMBER = String.Empty; string NUMBER_ALIAS1 = String.Empty;
                string NUMBER1 = String.Empty; string NUMBER_ALIAS2 = String.Empty; string NUMBER2 = String.Empty;

                sql = "SELECT DISTINCT(REGION_NAME),ID FROM[REGION]";
                command = new SqlCommand(sql, myDBCon);
                myDBCon.Open();
                SqlDataAdapter mySqlAdapter = new SqlDataAdapter();
                mySqlAdapter.SelectCommand = command;
                DataSet ds = new DataSet();
                mySqlAdapter.Fill(ds);
                Regions.DataSource = command.ExecuteReader();
                Regions.DataTextField = "REGION_NAME";
                Regions.DataValueField = "ID";
                Regions.DataBind();
                Regions.Items.Insert(0, new ListItem("", ""));
                myDBCon.Close();
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM dbo.TELEPHONE_DETAILS WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM dbo.TELEPHONE_CATEGORY_REGION WHERE REGION_ID = " + Regions.SelectedValue + ")", conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                lblMessage.Text = "Records deleted successfully.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                lblRecords.Text = "";
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        protected void contactList_Click(object sender, EventArgs e)
        {
            Response.Redirect("ICCSList.aspx", false);
        }

        protected void Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Sql = string.Empty;

            Sql = "SELECT COUNT(*) FROM dbo.TELEPHONE_DETAILS WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM dbo.TELEPHONE_CATEGORY_REGION WHERE REGION_ID = " + Regions.SelectedValue + ")";

            SqlConnection myDBCon = new SqlConnection(constr);
            SqlCommand command = null;
            command = new SqlCommand(Sql, myDBCon);
            myDBCon.Open();
            SqlDataAdapter mySqlAdapter = new SqlDataAdapter();
            mySqlAdapter.SelectCommand = command;
            DataSet ds = new DataSet();
            mySqlAdapter.Fill(ds);

            lblRecords.Text =  ds.Tables[0].Rows[0][0].ToString();
            

            myDBCon.Close();
        }
        
    }
}