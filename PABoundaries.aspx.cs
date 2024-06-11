﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class PABoundaries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindGridView("", "", "", "", 1);
            }
        }

        protected void gvEngineers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEngineers.PageIndex = e.NewPageIndex;
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, e.NewPageIndex + 1);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, 1);
        }

        public void BindGridView(string text1, string column1, string text2, string column2, int pageNo)
        {
            int pageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["AssetPagingSize"]);
            int startIndex, endIndex;

            startIndex = (pageNo - 1) * (pageSize + 1);
            endIndex = (((pageNo - 1) * pageSize + 1) + pageSize) - 1;
            gvEngineers.PageSize = pageSize;
            string searchClause = "", query = "";
            if (!String.IsNullOrEmpty(text1))
                searchClause += " " + column1 + " LIKE '%" + text1 + "%'";
            if (!String.IsNullOrEmpty(text1) && !String.IsNullOrEmpty(text2))
                searchClause += ddlAndOr.SelectedValue;
            if (!String.IsNullOrEmpty(text2))
                searchClause += " " + column2 + " LIKE '%" + text2 + "%'";

            if (!String.IsNullOrEmpty(searchClause))
                searchClause = " AND " + searchClause;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet dataSet = new DataSet();

            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["GISPRODConnectionString"].ConnectionString;

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            query = @"SELECT Project_Name,Project_Code,ECC FROM " + ddlPA.SelectedValue + " WHERE ECC = 'Active' " + searchClause + " ORDER BY Project_Code ";
            cmd.CommandText = query;
            
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;

            try
            {
                sqlDataAdapter.Fill(dataSet);
                int totalRows = Convert.ToInt32(dataSet.Tables[0].Rows.Count);
                gvEngineers.VirtualItemCount = totalRows;
                lblRecordCount.Text = totalRows.ToString();
                gvEngineers.DataSource = dataSet.Tables[0];
                gvEngineers.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ddlPA_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtField1.Text = "";
            txtField2.Text = "";
            ddlField1.SelectedIndex = 0;
            ddlField2.SelectedIndex = 0;
            BindGridView(txtField1.Text.Trim(), ddlField1.SelectedValue, txtField2.Text.Trim(), ddlField2.SelectedValue, 1);
        }
    }
}