using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web.UI;

namespace WebAppForm
{
    public partial class GetImgAttachByWo : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(GetImgAttachByWo));
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
            List<string> files = new List<string>();
            string AttachmentsPATH = ConfigurationManager.AppSettings["AttachmentsPATH"];
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
            Request.ApplicationPath.TrimEnd('/') + "/";

            if (Directory.Exists(AttachmentsPATH + id))
            {
                string[] filePaths = Directory.GetFiles(AttachmentsPATH + id, "*.JPG");
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    if (fileName.Contains("thumb"))
                        files.Add(baseUrl + "/Attachments/" + id + "/" + fileName );
                }
            }
            else
            {
                Tuple<bool, List<string>> response = CheckAttachment();
                if (response.Item1)
                {
                    files = response.Item2;
                }
            }

            dlImages.DataSource = files;
            dlImages.DataBind();

        }

        private Tuple<bool, List<string>> CheckAttachment()
        {
            Tuple<bool, List<string>> result = null;
            try
            {
                string attachmentLink = ConfigurationManager.AppSettings["WOAttachmentUri"];
                List<string> itemlist = new List<string>();
                bool HasRecord = false;
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    string query = ConfigurationManager.AppSettings["GetAttachURL"];
                    query = query.Replace("#id", id);

                    con.Open();
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, query);
                    con.Close();

                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                            HasRecord = true;
                        foreach (DataRow dr in dt.Rows)
                        {
                            string url = dr[0].ToString();
                            int startIndex = url.ToUpper().IndexOf("DOCLINKS", 0) + "doclinks\\".Length;
                            string filePath = url.Replace(url.Substring(0, startIndex), "");
                            string fullPath = attachmentLink + filePath.Replace('\\', '/');

                            itemlist.Add(fullPath);
                        }
                    }
                }
                result = new Tuple<bool, List<string>>(HasRecord, itemlist);
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace.ToString());
            }

            return result;
        }


    }
}