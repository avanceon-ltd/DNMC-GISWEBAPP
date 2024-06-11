using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace WebAppForm
{
    public partial class STWUploadFile : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                string SaveLocation = ConfigurationManager.AppSettings["STWUploadFile-Path"] + fn;

                try
                {
                    FileUpload1.PostedFile.SaveAs(SaveLocation);
                    lblMessage.Text = "The file has been uploaded.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }
            }
            else
            {
                lblMessage.Text = "Please select a file to upload.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }       
        
    }
}