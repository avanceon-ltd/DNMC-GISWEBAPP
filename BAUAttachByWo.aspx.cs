using Microsoft.ApplicationBlocks.Data;
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
    public partial class BAUAttachByWo : System.Web.UI.Page
    {

        List<string> imgname = new List<string>();
        string id = string.Empty;
        bool IsSR = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["IsSR"]))
            {
                IsSR = true;
            }
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
                string query = ConfigurationManager.AppSettings["GetAttachURL"];
                if (IsSR)
                {
                    query = "SELECT [URLNAME] FROM [ODW].[EAMS].[Attachment] where OWNERID IN (SELECT TICKETUID FROM [ODW].EAMS.FactServiceRequest where TICKETID='" + id + "') and OWNERTABLE='SR' AND (URLNAME LIKE '%JPG' OR URLNAME LIKE '%PNG' OR URLNAME LIKE '%GIF' OR URLNAME LIKE '%JPEG')";
                }
                query = query.Replace("#id", id);
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, query);
                con.Close();

                string attachmentLink = ConfigurationManager.AppSettings["WOAttachmentUri"];

                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        string url = dr[0].ToString();
                        int startIndex = url.ToUpper().IndexOf("DOCLINKS", 0) + "doclinks\\".Length;
                        string filePath = url.Replace(url.Substring(0, startIndex), "");
                        string fullPath = attachmentLink + filePath.Replace('\\', '/');
                        imgname.Add(fullPath);

                    }
                }
            }

            dlImages.DataSource = imgname;
            dlImages.DataBind();

        }
    }
}