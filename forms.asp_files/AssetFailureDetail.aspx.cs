﻿using System;
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
    public partial class AssetFailureDetail : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                SqlDataSource1.DataBind();
                // Repeater1.DataBind();


            }
        }
        protected void SqlDataSource1_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@ModifiedDate"].Value = DateTime.Now;
        }
    }
}