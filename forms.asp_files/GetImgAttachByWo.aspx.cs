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
    public partial class GetImgAttachByWo : System.Web.UI.Page
    {

        List<string> imgname = new List<string>();
        string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                // Repeater1.DataBind();
                id = Convert.ToString(Request.QueryString["ID"]);
                BindDataList();
                
            }
        }

        private void BindDataList()
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {

                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("SELECT [URLNAME] FROM [ODW].[EAMS].[Attachment] where OWNERID IN (SELECT WORKORDERID  FROM [ODW].[GIS].[vwWorkOrder]  where WONUM='" + id + "') AND OWNERTABLE='WORKORDER' AND (URLNAME LIKE '%JPG' OR URLNAME LIKE '%PNG' OR URLNAME LIKE '%GIF' OR URLNAME LIKE '%JPEG')", con);



                SqlDataAdapter sdadpr = new SqlDataAdapter();
                sdadpr.SelectCommand = cmd;
                sdadpr.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    imgname.Add(@"http://mv2eamsuat01.ashghal.gov.qa/ATTACHMENTS/" + dr[0].ToString().Split('\\').Last());
                }


            }

            dlImages.DataSource = imgname;
            dlImages.DataBind();

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 1300;  //or more time....
        }


    }
}