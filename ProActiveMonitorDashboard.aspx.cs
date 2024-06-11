using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using System.Web.Services;
using System.Security.Cryptography;

namespace WebAppForm
{
    public partial class ProActiveMonitorDashboard : System.Web.UI.Page
    {
       // ILog log = log4net.LogManager.GetLogger(typeof(ProActiveMonitorDashboard));
        public DateTime reportTime;
        public int screenWidth;
        public int screenHeight;
        public static int buttonClick = 1;
        static bool IsISDErrorExist = false;
        static bool IsODWErrorExist = false;
        static bool IsESRIErrorExist = false;
        static bool IsGISIZEErrorExist = false;
        public string currentSystem = "ISD System";
        private string constr = ConfigurationManager.ConnectionStrings["MonitoringConnectionString"].ConnectionString;
        private String Auth;
        private static readonly string EncryptionKey = "check";
        private string sourceString="isChecked";
        private bool isAuth=false;

        protected void Page_Unload(object sender, EventArgs e)
        {
            //CaptureScreen();
            //if (buttonClick==1)
            //{
            //    SendEmail(null, "ISD System", "Screenshot", CaptureScreen());
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //string encryptedString = Encrypt(sourceString);//ZK01wUVwctgsbPpdhB2yTlciX3OXt1KBJQcgz2l5vVw=

            Auth = Request.QueryString["auth"];           
            if (!string.IsNullOrEmpty(Auth))
            {
                string decryptedString = Decrypt(Auth);
                // Use the decrypted key for authentication or authorization
                if (decryptedString == "isChecked")
                {
                    isAuth = true;
                }
                else
                {
                    isAuth = false;
                }
            }
            //Menu1.StaticSelectedStyle.BackColor = System.Drawing.Color.DarkGreen;
            reportTime = DateTime.Now;
            btnButton1.BackColor = System.Drawing.Color.Green;
            btnButton2.BackColor = System.Drawing.Color.Green;
            btnButton3.BackColor = System.Drawing.Color.Green;
            btnButton4.BackColor = System.Drawing.Color.Green;
            btnButton5.BackColor = System.Drawing.Color.Green;
            if (!IsPostBack)
            {
                // Menu1.Items[0].Text = "<span style='background-color:Blue;'>"+Menu1.Items[0].Text+"</span>";
               
                buttonClick = 1;
                btnButton1.BackColor = System.Drawing.Color.Blue;
                btnButton2.BackColor = System.Drawing.Color.Green;
                btnButton3.BackColor = System.Drawing.Color.Green;
                btnButton4.BackColor = System.Drawing.Color.Green;
                //LoadISDData();
                //LoadODWData();
                //LoadESRIData();
                //LoadGISIZEData();
                LoadAllSystemData();

            }
            
        }


        public string Encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected void chkBoxGISIZE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxGISIZE.Checked)
            {
                SetGISIZELogToResolved();

            }
            currentSystem = "GISIZE";
           // btnButton1.BackColor = System.Drawing.Color.Green;
           // btnButton2.BackColor = System.Drawing.Color.Green;
           // btnButton3.BackColor = System.Drawing.Color.Green;
            btnButton4.BackColor = System.Drawing.Color.Blue;
            //LoadISDData();
            //LoadODWData();
            //LoadESRIData();
            //LoadGISIZEData();
            LoadAllSystemData();

        }
        protected void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox.Checked)
            {
                SetESRILogToResolved();

            }
            currentSystem = "ESRI System";
            //btnButton1.BackColor = System.Drawing.Color.Green;
            //btnButton2.BackColor = System.Drawing.Color.Green;
            btnButton3.BackColor = System.Drawing.Color.Blue;
            //btnButton4.BackColor = System.Drawing.Color.Green;
            LoadAllSystemData();

        }
        protected void LoadAllSystemData()
        {
            LoadISDData();
            LoadODWData();
            LoadESRIData();
            LoadGISIZEData();
        }
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            if (buttonClick == 1 && IsISDErrorExist)
            {
                SendEmail(null, "ISD System", "Screenshot", CaptureScreen());
            }
            else if (buttonClick == 2 && IsODWErrorExist)
            {
                SendEmail(null, "ODW System", "Screenshot", CaptureScreen());
            }
            else if (buttonClick == 3 && IsESRIErrorExist)
            {
                SendEmail(null, "ESRI System", "Screenshot", CaptureScreen());
            }
            else if (buttonClick == 4 && IsGISIZEErrorExist)
            {
                SendEmail(null, "GISIZE System", "Screenshot", CaptureScreen());
            }
            if (buttonClick == 1)
            {
                currentSystem = "ISD System";
                btnButton1.BackColor = System.Drawing.Color.Blue;
            }
            else if (buttonClick == 2)
            {
                currentSystem = "ODW";
                btnButton2.BackColor = System.Drawing.Color.Blue;
            }
            else if (buttonClick == 3)
            {
                currentSystem = "ESRI System";
                btnButton3.BackColor = System.Drawing.Color.Blue;
            }
            else if (buttonClick == 4)
            {
                currentSystem = "GISIZE";
                btnButton4.BackColor = System.Drawing.Color.Blue;
            }
            else if (buttonClick == 5)
            {
                currentSystem = "Data Count";
                btnButton5.BackColor = System.Drawing.Color.Blue;
            }
            LoadAllSystemData();
            LoadAllDataCount();

        }

        protected void btnButton1_Click(object sender, EventArgs e)
        {
            currentSystem = "ISD System";
            buttonClick = 1;
            btnButton1.BackColor = System.Drawing.Color.Blue;
            //btnButton2.BackColor = System.Drawing.Color.Green;
            //btnButton3.BackColor = System.Drawing.Color.Green;
            //btnButton4.BackColor = System.Drawing.Color.Green;

            //LoadISDData();
            //LoadODWData();
            //LoadESRIData();
            //LoadGISIZEData();
            LoadAllSystemData();
            
        }

        protected void btnButton2_Click(object sender, EventArgs e)
        {
            //if (IsISDScreen && buttonClick==1)
            //{
            //    SendEmail(null, "ISD System", "Screenshot", CaptureScreen());
            //}
            currentSystem = "ODW";
            buttonClick = 2;
           // btnButton1.BackColor = System.Drawing.Color.Green;
            btnButton2.BackColor = System.Drawing.Color.Blue;
            // btnButton3.BackColor = System.Drawing.Color.Green;
            // btnButton4.BackColor = System.Drawing.Color.Green;

            LoadAllSystemData();
        }

        protected void btnButton3_Click(object sender, EventArgs e)
        {
            buttonClick = 3;
            currentSystem = "ESRI System";
            //btnButton1.BackColor = System.Drawing.Color.Green;
            //btnButton2.BackColor = System.Drawing.Color.Green;
            btnButton3.BackColor = System.Drawing.Color.Blue;
            //btnButton4.BackColor = System.Drawing.Color.Green;
            LoadAllSystemData();
        }

        protected void btnButton4_Click(object sender, EventArgs e)
        {
            buttonClick = 4;
            currentSystem = "GISIZE";
            //btnButton1.BackColor = System.Drawing.Color.Green;
           // btnButton2.BackColor = System.Drawing.Color.Green;
           // btnButton3.BackColor = System.Drawing.Color.Green;
            btnButton4.BackColor = System.Drawing.Color.Blue;
            //btnButton5.BackColor = System.Drawing.Color.Green;
            LoadAllSystemData();
        }

        protected void btnButton5_Click(object sender, EventArgs e)
        {
            buttonClick = 5;
            currentSystem = "Data Count";
           // btnButton1.BackColor = System.Drawing.Color.Green;
           // btnButton2.BackColor = System.Drawing.Color.Green;
           // btnButton3.BackColor = System.Drawing.Color.Green;
           // btnButton4.BackColor = System.Drawing.Color.Green;
            btnButton5.BackColor = System.Drawing.Color.Blue;

            //LoadISDData();
            //LoadODWData();
            //LoadESRIData();
            //LoadGISIZEData();
            LoadAllSystemData();
            LoadAllDataCount();
        }


        //protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        //{
        //    Menu1.StaticSelectedStyle.BackColor = System.Drawing.Color.DarkGreen;
        //    int index = Int32.Parse(e.Item.Value);
        //    if (Int32.Parse(e.Item.Value) == 0)
        //    {
        //        LoadISDData();
        //    }
        //    if (Int32.Parse(e.Item.Value) == 1)
        //    {
        //        LoadODWData();
        //    }
        //    if (Int32.Parse(e.Item.Value) == 2)
        //    {
        //        LoadESRIData();
        //    }
        //    if (Int32.Parse(e.Item.Value) == 3)
        //    {
        //        LoadGISIZEData();
        //    }
        //    MultiView1.ActiveViewIndex = index;


        //}
        private void LoadISDData()
        {
            bool isISDError = false;
            #region ISDSystemStatus
            DataTable dt4 = this.GetISDSystemStatus();

            //Building an HTML string.
            StringBuilder html4 = new StringBuilder();

            //Table start.
            html4.Append("<table border = '1'>");

            //Building the Header row.
            html4.Append("<tr>");
            foreach (DataColumn column in dt4.Columns)
            {
                html4.Append("<th>");
                html4.Append(column.ColumnName);
                html4.Append("</th>");
            }
            html4.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt4.Rows)
            {
                string rowClass = row["Status"].ToString() != "Success" ? "error-row" : "";
                if (rowClass == "error-row")
                {
                    html4.Append("<tr style='background-color:#FFC7CE'>");
                }
                else
                {
                    html4.Append("<tr>");
                }

                foreach (DataColumn column in dt4.Columns)
                {
                    html4.Append("<td>");
                    if (column.ColumnName == "Status" )
                    {
                        if (row[column.ColumnName].ToString() != "Success")
                        {

                            btnButton1.BackColor = System.Drawing.Color.Red;
                            isISDError = true;
                            IsISDErrorExist = true;
                        }
                    }

                    html4.Append(row[column.ColumnName]);
                    html4.Append("</td>");
                }
                html4.Append("</tr>");
            }

            //Table end.
            html4.Append("</table>");

            //Append the HTML string to Placeholder.
            tab1Table1.Controls.Add(new Literal { Text = html4.ToString() });
            if (isISDError)
            {

                SendEmail(html4, "ISD Integration System Status", "ISD System");
                isISDError = false;
            }
            #endregion

            #region ISDATACount
            DataTable dt = this.GetISDDataCount();

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column.ColumnName].ToString() == "Service Request" || row[column.ColumnName].ToString() == "Complaint" ||
                        row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                    {
                        html.Append("<td style='font-weight:bold'>");
                    }
                    else
                    {
                        html.Append("<td>");
                    }
                   // html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            tab1Table2.Controls.Add(new Literal { Text = html.ToString() });
            #endregion

            #region ISDRecentData
            DataTable dt5 = this.GetISDRecentData();

            //Building an HTML string.
            StringBuilder html5 = new StringBuilder();

            //Table start.
            html5.Append("<table border = '1'>");

            //Building the Header row.
            html5.Append("<tr>");
            foreach (DataColumn column in dt5.Columns)
            {
                html5.Append("<th>");
                html5.Append(column.ColumnName);
                html5.Append("</th>");
            }
            html5.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt5.Rows)
            {
                html5.Append("<tr>");
                foreach (DataColumn column in dt5.Columns)
                {
                    html5.Append("<td>");
                    html5.Append(row[column.ColumnName]);
                    html5.Append("</td>");
                }
                html5.Append("</tr>");
            }

            //Table end.
            html5.Append("</table>");

            //Append the HTML string to Placeholder.
            tab1Table3.Controls.Add(new Literal { Text = html5.ToString() });
            #endregion

            //#region ISD_StatusMismatchData
            //DataTable dt3 = this.GetISDStatusMismatchData();

            ////Building an HTML string.
            //StringBuilder html3 = new StringBuilder();

            ////Table start.
            //html3.Append("<table border = '1'>");

            ////Building the Header row.
            //html3.Append("<tr>");
            //foreach (DataColumn column in dt3.Columns)
            //{
            //    html3.Append("<th>");
            //    html3.Append(column.ColumnName);
            //    html3.Append("</th>");
            //}
            //html3.Append("</tr>");

            ////Building the Data rows.
            //foreach (DataRow row in dt3.Rows)
            //{
            //    html3.Append("<tr>");
            //    foreach (DataColumn column in dt3.Columns)
            //    {
            //        html3.Append("<td>");
            //        html3.Append(row[column.ColumnName]);
            //        html3.Append("</td>");
            //    }
            //    html3.Append("</tr>");
            //}

            ////Table end.
            //html3.Append("</table>");

            ////Append the HTML string to Placeholder.
            //tab1Table4.Controls.Add(new Literal { Text = html3.ToString() });
            //#endregion
        }
        private void LoadODWData()
        {
            bool isODWError = false;

            #region OWDDataCount
            DataTable dt = this.GetODWDataCount();

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column.ColumnName].ToString() == "Service Request" || row[column.ColumnName].ToString() == "Complaint" ||
                        row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                    {
                        html.Append("<td style='font-weight:bold'>");
                    }
                    else
                    {
                        html.Append("<td>");
                    }
                        
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            tab2Table1.Controls.Add(new Literal { Text = html.ToString() });
            #endregion

            #region ODWRecentData
            DataTable dt5 = this.GetODWRecentData();

            //Building an HTML string.
            StringBuilder html5 = new StringBuilder();

            //Table start.
            html5.Append("<table border = '1'>");

            //Building the Header row.
            html5.Append("<tr>");
            foreach (DataColumn column in dt5.Columns)
            {
                html5.Append("<th>");
                html5.Append(column.ColumnName);
                html5.Append("</th>");
            }
            html5.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt5.Rows)
            {
                html5.Append("<tr>");
                foreach (DataColumn column in dt5.Columns)
                {
                    html5.Append("<td>");
                    html5.Append(row[column.ColumnName]);
                    html5.Append("</td>");
                }
                html5.Append("</tr>");
            }

            //Table end.
            html5.Append("</table>");

            //Append the HTML string to Placeholder.
            tab2Table2.Controls.Add(new Literal { Text = html5.ToString() });
            #endregion

            #region ODWWindows_Services
            DataTable dt4 = this.GetODWWindowsServices();

            //Building an HTML string.
            StringBuilder html4 = new StringBuilder();

            //Table start.
            html4.Append("<table border = '1'>");

            //Building the Header row.
            html4.Append("<tr>");
            foreach (DataColumn column in dt4.Columns)
            {
                html4.Append("<th>");
                html4.Append(column.ColumnName);
                html4.Append("</th>");
            }
            html4.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt4.Rows)
            {
                string rowClass = row["Status"].ToString() != "Running" ? "error-row" : "";
                if (rowClass == "error-row")
                {
                    html4.Append("<tr style='background-color:#FFC7CE'>");
                }
                else
                {
                    html4.Append("<tr>");
                }
                //html4.Append("<tr>");
                foreach (DataColumn column in dt4.Columns)
                {
                    html4.Append("<td>");
                    if (column.ColumnName == "Status")
                    {
                        if (row[column.ColumnName].ToString() != "Running")
                        {
                            // Menu1.StaticSelectedStyle.CssClass = string.Empty;
                            btnButton2.BackColor = System.Drawing.Color.Red;
                            
                            isODWError = true;
                            IsODWErrorExist = true;
                        }
                    }
                    html4.Append(row[column.ColumnName]);
                    html4.Append("</td>");
                }
                html4.Append("</tr>");
            }

            //Table end.
            html4.Append("</table>");

            //Append the HTML string to Placeholder.
            tab2Table4.Controls.Add(new Literal { Text = html4.ToString() });
            if (isODWError)
            {
                SendEmail(html4, "ODW Windows Services Status", "ODW System");
                isODWError = false;
            }
            #endregion

            #region OWD_JobStatus
            DataTable dt3 = this.GetODWJobStatus();

            //Building an HTML string.
            StringBuilder html3 = new StringBuilder();

            //Table start.
            html3.Append("<table border = '1'>");

            //Building the Header row.
            html3.Append("<tr>");
            foreach (DataColumn column in dt3.Columns)
            {
                html3.Append("<th>");
                html3.Append(column.ColumnName);
                html3.Append("</th>");
            }
            html3.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt3.Rows)
            {
                string rowClass = row["LastRunOutcome"].ToString().ToUpper() != "SUCCESS" ? "error-row" : "";
                if (rowClass == "error-row")
                {
                    html3.Append("<tr style='background-color:#FFC7CE'>");
                }
                else
                {
                    html3.Append("<tr>");
                }
                //html3.Append("<tr>");
                foreach (DataColumn column in dt3.Columns)
                {
                    html3.Append("<td>");
                    if (column.ColumnName == "LastRunOutcome")
                    {
                        if (row[column.ColumnName].ToString().ToUpper() != "SUCCESS")
                        {
                            // Menu1.StaticSelectedStyle.CssClass = string.Empty;
                            btnButton2.BackColor = System.Drawing.Color.Red;

                            isODWError = true;
                            IsODWErrorExist = true;
                        }
                    }
                    html3.Append(row[column.ColumnName]);
                    html3.Append("</td>");
                }
                html3.Append("</tr>");
            }

            //Table end.
            html3.Append("</table>");

            //Append the HTML string to Placeholder.
            tab2Table3.Controls.Add(new Literal { Text = html3.ToString() });
            if (isODWError)
            {
                SendEmail(html3, "ODW JOB Status", "ODW System");
                isODWError = false;
            }
            #endregion

        }
        private void LoadESRIData()
        {
            bool isESRIError = false;
            #region ESRI_Layer_Status
            DataTable dt2 = this.GetESRILayerServiceStatus();

            //Building an HTML string.
            StringBuilder html2 = new StringBuilder();

            //Table start.
            html2.Append("<table border = '1'>");

            //Building the Header row.
            html2.Append("<tr>");
            foreach (DataColumn column in dt2.Columns)
            {
                html2.Append("<th>");
                html2.Append(column.ColumnName);
                html2.Append("</th>");
            }
            html2.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt2.Rows)
            {
                string rowClass = row["Status"].ToString() != "Running" ? "error-row" : "";
                if (rowClass == "error-row")
                {
                    html2.Append("<tr style='background-color:#FFC7CE'>");
                }
                else
                {
                    html2.Append("<tr>");
                }
               // html2.Append("<tr>");
                foreach (DataColumn column in dt2.Columns)
                {
                    html2.Append("<td>");
                    if (column.ColumnName == "Status")
                    {
                        if (row[column.ColumnName].ToString() != "Running")
                        {
                            // Menu1.StaticSelectedStyle.CssClass = string.Empty;
                            btnButton3.BackColor = System.Drawing.Color.Red;

                            isESRIError = true;
                            IsESRIErrorExist = true;
                        }
                    }
                    html2.Append(row[column.ColumnName]);
                    html2.Append("</td>");
                }
                html2.Append("</tr>");
            }

            //Table end.
            html2.Append("</table>");

            //Append the HTML string to Placeholder.
            tab3Table1.Controls.Add(new Literal { Text = html2.ToString() });
            if (isESRIError)
            {
                SendEmail(html2, "ESRI Layer Status", "ESRI System");
                isESRIError = false;
            }
            #endregion

            #region ESRI_DataCount
            DataTable dt3 = this.GetESRIDataCount();

            //Building an HTML string.
            StringBuilder html3 = new StringBuilder();

            //Table start.
            html3.Append("<table border = '1'>");

            //Building the Header row.
            html3.Append("<tr>");
            foreach (DataColumn column in dt3.Columns)
            {
                html3.Append("<th>");
                html3.Append(column.ColumnName);
                html3.Append("</th>");
            }
            html3.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt3.Rows)
            {
                html3.Append("<tr>");
                foreach (DataColumn column in dt3.Columns)
                {
                    if (row[column.ColumnName].ToString() == "EAMS Complaints" ||row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                    {
                        html3.Append("<td style='font-weight:bold'>");
                    }
                    else
                    {
                        html3.Append("<td>");
                    }
                    //html3.Append("<td>");
                    html3.Append(row[column.ColumnName]);
                    html3.Append("</td>");
                }
                html3.Append("</tr>");
            }

            //Table end.
            html3.Append("</table>");

            //Append the HTML string to Placeholder.
            tab3Table2.Controls.Add(new Literal { Text = html3.ToString() });
            #endregion

            #region ESRI_Web_Services_Layers_Logs_LiveData
            DataTable dt = this.GetESRIServicesLayersLogs();

            chkBox.Enabled = false;
            if (dt.Rows.Count > 0 )
            {
                
                chkBox.Checked = false;
                if (isAuth)
                {
                    chkBox.Enabled = true;
                }

            }
            else
            {
                chkBox.Checked = true;
               // chkBox.Enabled = false;
            }
            

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                //html.Append("<tr>");
                html.Append("<tr style='background-color:#FFC7CE'>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    btnButton3.BackColor = System.Drawing.Color.Red;
                    isESRIError = true;
                    IsESRIErrorExist = true;
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            tab3Table4.Controls.Add(new Literal { Text = html.ToString() });
            if (isESRIError)
            {
                SendEmail(html, "ESRI Services Log", "ESRI System");
                isESRIError = false;
            }
            #endregion

            #region ESRI_Windows_Services
            DataTable dt4 = this.GetESRIWindowsServices();

            //Building an HTML string.
            StringBuilder html4 = new StringBuilder();

            //Table start.
            html4.Append("<table border = '1'>");

            //Building the Header row.
            html4.Append("<tr>");
            foreach (DataColumn column in dt4.Columns)
            {
                html4.Append("<th>");
                html4.Append(column.ColumnName);
                html4.Append("</th>");
            }
            html4.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt4.Rows)
            {
                string rowClass = row["Status"].ToString() != "Running" ? "error-row" : "";
                if (rowClass == "error-row")
                {
                    html4.Append("<tr style='background-color:#FFC7CE'>");
                }
                else
                {
                    html4.Append("<tr>");
                }
                //html4.Append("<tr>");
                foreach (DataColumn column in dt4.Columns)
                {
                    html4.Append("<td>");
                    if (column.ColumnName == "Status")
                    {
                        if (row[column.ColumnName].ToString() != "Running")
                        {
                            // Menu1.StaticSelectedStyle.CssClass = string.Empty;
                            btnButton3.BackColor = System.Drawing.Color.Red;

                            isESRIError = true;
                            IsESRIErrorExist = true;
                        }
                    }
                    html4.Append(row[column.ColumnName]);
                    html4.Append("</td>");
                }
                html4.Append("</tr>");
            }

            //Table end.
            html4.Append("</table>");

            //Append the HTML string to Placeholder.
            tab3Table3.Controls.Add(new Literal { Text = html4.ToString() });
            if (isESRIError)
            {
                SendEmail(html4, "ESRI Window Service Status", "ESRI System");
                isESRIError = false;
            }
            #endregion

            #region ESRIRecentData
            DataTable dt5 = this.GetESRIRecentData();

            //Building an HTML string.
            StringBuilder html5 = new StringBuilder();

            //Table start.
            html5.Append("<table border = '1'>");

            //Building the Header row.
            html5.Append("<tr>");
            foreach (DataColumn column in dt5.Columns)
            {
                html5.Append("<th>");
                html5.Append(column.ColumnName);
                html5.Append("</th>");
            }
            html5.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt5.Rows)
            {
                html5.Append("<tr>");
                foreach (DataColumn column in dt5.Columns)
                {
                    html5.Append("<td>");
                    html5.Append(row[column.ColumnName]);
                    html5.Append("</td>");
                }
                html5.Append("</tr>");
            }

            //Table end.
            html5.Append("</table>");

            //Append the HTML string to Placeholder.
            tab3Table5.Controls.Add(new Literal { Text = html5.ToString() });
            #endregion

            
        }
        private void LoadGISIZEData()
        {
            bool isGISIZEError = false;
            #region GISIZE_DataCount
            DataTable dt2 = this.GetGISIZEDataCount();

            //Building an HTML string.
            StringBuilder html2 = new StringBuilder();

            //Table start.
            html2.Append("<table border = '1'>");

            //Building the Header row.
            html2.Append("<tr>");
            foreach (DataColumn column in dt2.Columns)
            {
                html2.Append("<th>");
                html2.Append(column.ColumnName);
                html2.Append("</th>");
            }
            html2.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt2.Rows)
            {
                html2.Append("<tr>");
                foreach (DataColumn column in dt2.Columns)
                {
                    if (row[column.ColumnName].ToString() == "Complaint" || row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                    {
                        html2.Append("<td style='font-weight:bold'>");
                    }
                    else
                    {
                        html2.Append("<td>");
                    }
                    //html2.Append("<td>");
                    html2.Append(row[column.ColumnName]);
                    html2.Append("</td>");
                }
                html2.Append("</tr>");
            }

            //Table end.
            html2.Append("</table>");

            //Append the HTML string to Placeholder.
            tab4Table1.Controls.Add(new Literal { Text = html2.ToString() });
            #endregion

            #region GISIZE_Windows_Services
            DataTable dt = this.GetGISIZEWindowsServices();

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                //html.Append("<tr>");
                string rowClass = row["Status"].ToString() != "Running" ? "error-row" : "";
                if (rowClass == "error-row")
                {
                    html.Append("<tr style='background-color:#FFC7CE'>");
                }
                else
                {
                    html.Append("<tr>");
                }
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    if (column.ColumnName == "Status")
                    {
                        if (row[column.ColumnName].ToString() != "Running")
                        {
                            // Menu1.StaticSelectedStyle.CssClass = string.Empty;
                            btnButton4.BackColor = System.Drawing.Color.Red;

                            isGISIZEError = true;
                            IsGISIZEErrorExist = true;
                        }
                    }
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            tab4Table2.Controls.Add(new Literal { Text = html.ToString() });

            if (isGISIZEError)
            {
                SendEmail(html, "GISIZE Window Service Status", "GISIZE");
                isGISIZEError = false;
            }
            #endregion

            #region GISIZERecentData
            DataTable dt5 = this.GetGISIZERecentData();

            //Building an HTML string.
            StringBuilder html5 = new StringBuilder();

            //Table start.
            html5.Append("<table border = '1'>");

            //Building the Header row.
            html5.Append("<tr>");
            foreach (DataColumn column in dt5.Columns)
            {
                html5.Append("<th>");
                html5.Append(column.ColumnName);
                html5.Append("</th>");
            }
            html5.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt5.Rows)
            {
                html5.Append("<tr>");
                foreach (DataColumn column in dt5.Columns)
                {
                    html5.Append("<td>");
                    html5.Append(row[column.ColumnName]);
                    html5.Append("</td>");
                }
                html5.Append("</tr>");
            }

            //Table end.
            html5.Append("</table>");

            //Append the HTML string to Placeholder.
            tab4Table3.Controls.Add(new Literal { Text = html5.ToString() });
            #endregion

            #region GISIZE_Web_Services_Layers_Logs
            DataTable dt4 = this.GETGISIZEServicesLogs();
            chkBoxGISIZE.Enabled = false;
            if (dt4.Rows.Count > 0)
            {
                chkBoxGISIZE.Checked = false;
                if (isAuth)
                {
                    chkBoxGISIZE.Enabled = true;
                }

            }
            else
            {
                chkBoxGISIZE.Checked = true;
                //chkBoxGISIZE.Enabled = false;
            }
            //Building an HTML string.
            StringBuilder html4 = new StringBuilder();

            //Table start.
            html4.Append("<table border = '1'>");

            //Building the Header row.
            html4.Append("<tr>");
            foreach (DataColumn column in dt4.Columns)
            {
                html4.Append("<th>");
                html4.Append(column.ColumnName);
                html4.Append("</th>");
            }
            html4.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt4.Rows)
            {
                //html4.Append("<tr>");
                html4.Append("<tr style='background-color:#FFC7CE'>");
                foreach (DataColumn column in dt4.Columns)
                {
                    html4.Append("<td>");
                    btnButton4.BackColor = System.Drawing.Color.Red;
                    isGISIZEError = true;
                    IsGISIZEErrorExist = true;
                    html4.Append(row[column.ColumnName]);
                    html4.Append("</td>");
                }
                html4.Append("</tr>");
            }

            //Table end.
            html4.Append("</table>");

            //Append the HTML string to Placeholder.
            tab4Table4.Controls.Add(new Literal { Text = html4.ToString() });
            if (isGISIZEError)
            {
                SendEmail(html4, "GISIZE Layer Services Log", "GISIZE");
                isGISIZEError = false;
            }
            #endregion
        }

        private void LoadAllDataCount()
        {
            #region getAllDataCount
            DataTable dt2 = this.GetAllDataCount();
            int count = 0;
            int count2 = 0;
            //Building an HTML string.
            StringBuilder html2 = new StringBuilder();

            //Table start.
            html2.Append("<table border = '1'>");

            //Building the Header row.
            html2.Append("<tr>");
            foreach (DataColumn column in dt2.Columns)
            {
                if(column.ColumnName == "ISD QueryDate" || column.ColumnName == "ODW QueryDate" || column.ColumnName == "ESRI QueryDate" || column.ColumnName == "GISIZE QueryDate")
                {

                    if (column.ColumnName == "ISD QueryDate")
                    {
                        html2.Append("<th style='background-color:#b4eeb4' >");
                        html2.Append("ISD");
                    }
                    else if (column.ColumnName == "ODW QueryDate")
                    {

                        html2.Append("<th style='background-color:#adfff3' >");
                        html2.Append("ODW");
                    }
                    else if (column.ColumnName == "ESRI QueryDate")
                    {
                        html2.Append("<th style='background-color:#ffb2d7' >");
                        html2.Append("ESRI");
                    }
                    else if (column.ColumnName == "GISIZE QueryDate")
                    {
                        html2.Append("<th style='background-color:#cbb2ff' >");
                        html2.Append("GISIZE");

                    }

                    
                    html2.Append("</th>");
                }
                else
                {
                    
                    if (column.ColumnName == "ISD Data Count" || column.ColumnName == "ODW Data Count" || column.ColumnName == "ESRI Data Count" || column.ColumnName == "GISIZE Data Count")
                    {
                        count++;
                        if (count == 1)
                        {
                            html2.Append("<th style='background-color:#ddfcf7' colspan='4'> Data Count </th>");
                            html2.Append("<th style='background-color:#b2a8a8' colspan='4'> QueryDate </th>");
                            html2.Append("</tr>");
                        }
                        if (column.ColumnName == "ISD Data Count" )
                        {
                            
                            html2.Append("<tr>");
                            html2.Append("<th style='background-color:#b4eeb4' >");
                            html2.Append("ISD");
                        }
                        else if (column.ColumnName == "ODW Data Count")
                        {

                            html2.Append("<th style='background-color:#adfff3' >");
                            html2.Append("ODW");
                        }
                        else if (column.ColumnName == "ESRI Data Count")
                        {
                            html2.Append("<th style='background-color:#ffb2d7' >");
                            html2.Append("ESRI");
                        }
                        else if (column.ColumnName == "GISIZE Data Count")
                        {
                            html2.Append("<th style='background-color:#cbb2ff' >");
                            html2.Append("GISIZE");

                        }
                        
                        //html2.Append(column.ColumnName);
                        html2.Append("</th>");
                    }
                    else
                    {
                        html2.Append("<th rowspan='2'>");
                        html2.Append(column.ColumnName);
                        html2.Append("</th>");
                    }




                }
               
            }
            html2.Append("</tr>");
            int isUnassigned = 0;
            int check = 4;
            //Building the Data rows.
            foreach (DataRow row in dt2.Rows)
            {
                html2.Append("<tr>");
                foreach (DataColumn column in dt2.Columns)
                {
                    //if (column.ColumnName == "ISD Data Count" || column.ColumnName == "ODW Data Count" || column.ColumnName == "ESRI Data Count" || column.ColumnName == "GISIZE Data Count")
                    //{
                    //    html2.Append("<td style='font-weight:bold' >");
                    //}
                    //else
                    //{
                        
                    //}
                    if (check > 0 && isUnassigned == 1)
                    {
                        if (check == 1)
                        {
                            isUnassigned = 0;
                        }
                        check--;

                        if (!(row[column.ColumnName] is DBNull))
                        {
                            if (Convert.ToInt32(row[column.ColumnName]) > 10)
                            {
                                if(row[column.ColumnName].ToString()== "Service Request" || row[column.ColumnName].ToString() == "Complaint" || 
                                    row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                                {
                                    html2.Append("<td style='background-color:#FFC7CE; font-weight:bold'>");
                                }
                                else
                                {
                                    html2.Append("<td style='background-color:#FFC7CE'>");
                                }
                                
                                SendEmail(null, "Unassigned is more than 10 in "+column.ColumnName, "UnAssigned KPI more than 10");
                            }
                            else
                            {
                                if (row[column.ColumnName].ToString() == "Service Request" || row[column.ColumnName].ToString() == "Complaint" ||
                                    row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                                {
                                    html2.Append("<td style='font-weight:bold'>");
                                }
                                else
                                {
                                    html2.Append("<td>");
                                }
                               
                            }
                        }
                        else
                        {
                            if (row[column.ColumnName].ToString() == "Service Request" || row[column.ColumnName].ToString() == "Complaint" ||
                                    row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                            {
                                html2.Append("<td style='font-weight:bold'>");
                            }
                            else
                            {
                                html2.Append("<td>");
                            }
                            
                        }
                       
                    }
                    else
                    {
                        if (row[column.ColumnName].ToString() == "Service Request" || row[column.ColumnName].ToString() == "Complaint" ||
                                    row[column.ColumnName].ToString() == "Total WorkOrder" || row[column.ColumnName].ToString() == "Total KPI")
                        {
                            html2.Append("<td style='font-weight:bold'>");
                        }
                        else
                        {
                            html2.Append("<td>");
                        }
                        
                    }

                    if (row[column.ColumnName].ToString().ToLower()== "unassigned (wo)")
                    {
                        isUnassigned = 1;
                    }
                   
                    if (row[column.ColumnName].ToString() == null || row[column.ColumnName].ToString() == "")
                    {
                        html2.Append("N/A");
                    }
                    else
                    {
                        html2.Append(row[column.ColumnName]);
                    }  
                    html2.Append("</td>");
                }
                html2.Append("</tr>");
            }

            //Table end.
            html2.Append("</table>");

            //Append the HTML string to Placeholder.
            tab5Table1.Controls.Add(new Literal { Text = html2.ToString() });
#endregion
        }


        private DataTable GetODWDataCount()
        {
            //string constr ="";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT [TYPE] as Name,Count,GETDATE() as FetchDate, UpdatedOn FROM [ODW].[DataCount]"))
                using (SqlCommand cmd = new SqlCommand("EXEC Monitoring.[dbo].[GetODWDataCount]"))
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
        private DataTable GetAllDataCount()
        {
            //string constr ="";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC Monitoring.[dbo].[getAllData]"))
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

        private DataTable GetISDDataCount()
        {
            //string constr ="";
            using (SqlConnection con = new SqlConnection(constr))
            {
               // using (SqlCommand cmd = new SqlCommand(" SELECT [TYPE] as Name,Count,GETDATE() as FetchDate, UpdatedOn FROM [ISD].[DataCount] ORDER BY SortOrder ASC"))
                using (SqlCommand cmd = new SqlCommand(" EXEC Monitoring.[dbo].[GetISDDataCount]"))
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

        private DataTable GetODWWindowsServices()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select FetchTime, MachineName,IP, ServiceName, [Status] from [ODW].[WindowsServicesStatus]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DataRow[] dtrow = dt.Select($"{"Status"}<> '{"Running"}'");
                            if (dtrow.Length > 0)
                            {
                                return dt.AsEnumerable().OrderByDescending(row => row.Field<string>("Status")).CopyToDataTable();
                            }
                            return dt;
                            
                        }
                    }
                }
            }
        }
        private DataTable GetESRIServicesLayersLogs()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select FileType as [Service Name],MethodName as [Feature Class],[Status],ErrorType as [Error Type],[Description],ErrorTime as [Error Time] from [ESRI].[WebServicesLogs] where Status='Unresolved'"))
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

        private DataTable GetESRIWindowsServices()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ServiceName AS [Service Name], MachineName AS [Machine Name], [Status],FetchTime AS [Last Updated Time] FROM [ESRI].[WindowsServicesStatus]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DataRow[] dtrow = dt.Select($"{"Status"}<> '{"Running"}'");
                            if (dtrow.Length > 0)
                            {
                                return dt.AsEnumerable().OrderByDescending(row => row.Field<string>("Status")).CopyToDataTable();
                            }
                            return dt;
                        }
                    }
                }
            }
        }
        private DataTable GetGISIZEDataCount()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT [Type] AS [Layer Name], 'RT Service' AS [Service Name], [Count], UpdatedOn as [Updated On] FROM [GISIZE].[DataCount]"))
                using (SqlCommand cmd = new SqlCommand("EXEC Monitoring.[dbo].[GetGISIZEDataCount]"))
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


        private DataTable GETGISIZEServicesLogs()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("  select MachineName,[FileName] as [Service Name],[Status],[Description],ErrorTime as [Error Time] from [GISIZE].[Web_Services_Layers_Logs_LiveData] where Status='Unresolved'"))
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

        private DataTable GetGISIZEWindowsServices()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ServiceName AS [Service Name], [Status], FetchTime AS [Last Updated Time] from [GISIZE].[Windows_Services_LiveData]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DataRow[] dtrow = dt.Select($"{"Status"}<> '{"Running"}'");
                            if (dtrow.Length > 0)
                            {
                                return dt.AsEnumerable().OrderByDescending(row => row.Field<string>("Status")).CopyToDataTable();
                            }
                            return dt;
                        }
                    }
                }
            }
        }

        private DataTable GetESRILayerServiceStatus()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT LayerName AS [Layer Name], ServiceName AS [Service Name], [Status], UpdatedOn AS [Last Updated Time] FROM [ESRI].[LayerServiceStatus]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DataRow[] dtrow = dt.Select($"{"Status"}<> '{"Running"}'");
                            if (dtrow.Length > 0)
                            {
                                return dt.AsEnumerable().OrderByDescending(row => row.Field<string>("Status")).CopyToDataTable();
                            }
                            return dt;
                           
                        }
                    }
                }
            }
        }

        private DataTable GetODWJobStatus()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT JobName AS [Job Name],LastRunOutcome,LastRunOutcomeMessage,LastRunDuration AS [Run Duration], LastRunDatetime AS [Last Run Time] FROM [ODW].[JobStatus]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DataRow[] dtrow = dt.Select($"{"LastRunOutcome"}<> '{"SUCCESS"}'");
                            if (dtrow.Length > 0)
                            {
                                return dt.AsEnumerable().OrderBy(row => row.Field<string>("LastRunOutcome")).CopyToDataTable();
                            }
                            return dt;
                            
                        }
                    }
                }
            }
        }

        private DataTable GetESRIDataCount()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT [Description],[Count],[UpdatedOn] AS [Last Updated Time] FROM [ESRI].[ServicesDataCount] "))
                using (SqlCommand cmd = new SqlCommand("EXEC Monitoring.[dbo].[GetESRIDataCount] "))
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

        private DataTable GetISDSystemStatus()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Select  Name, [Description],IpAddress AS [Ip Address],[Status],FetchDate AS [Fetch Date] from [ISD].[ServerStatus]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            DataRow[] dtrow = dt.Select($"{"Status"}= '{"TimedOut"}'");
                            if (dtrow.Length > 0)
                            {
                                return dt.AsEnumerable().OrderByDescending(row => row.Field<string>("Status")).CopyToDataTable();
                            }
                            return dt;
                        }
                    }
                }
            }
        }
        private DataTable GetISDStatusMismatchData()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Select EAMS_STG_STATUS AS [Stagging Status],EAMS_STG_STATUSDATE AS [Stagging Status Date], EAMS_PROD_STATUS AS [Production Status], EAMS_PROD_STATUSDATE AS [Production Status Date], FetchDate AS [Fetch Date] from [ISD].[Audit_WO_Status_Missmatch_LiveData]"))
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

        private DataTable GetESRIRecentData()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT [LayerName],[Service_Request_Number],[WONUM],[EntryType],[WO_STATUS],[DNMC_Fetch_Date] FROM [ESRI].[ServicesData]"))
                //using (SqlCommand cmd = new SqlCommand("SELECT [LayerName],[EntryType],[Service_Request_Number],[Application_Status],[WONUM],[WO_STATUS],[DNMC_Fetch_Date] FROM[Monitoring].[ESRI].[Rest_Service_LiveData]"))
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
        private DataTable GetISDRecentData()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT (CASE when type= 'Work Order' THEN OWNER + '(WO)'ELSE TYPE  end) AS Name,EXT_CRMSRID,TICKETID,WONUM,REPORTDATE,DESCRIPTION,STATUS,UpdatedOn FROM ISD.RecentData"))
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
        private DataTable GetODWRecentData()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT (CASE when type= 'Work Order' THEN OWNER + '(WO)'ELSE TYPE  end) AS Name,EXT_CRMSRID,TICKETID,WONUM,REPORTDATE,DESCRIPTION,STATUS,UpdatedOn FROM ODW.RecentData"))
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
        private DataTable GetGISIZERecentData()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT (CASE when type= 'Work Order' THEN OWNER + '(WO)'ELSE TYPE  end) AS Name,EXT_CRMSRID,TICKETID,WONUM,REPORTDATE,DESCRIPTION,STATUS,UpdatedOn FROM GISIZE.RecentData "))
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

        private void SetESRILogToResolved()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("TRUNCATE TABLE [ESRI].[WebServicesLogs] "))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
        }
        private void SetESRILogToUnresolved()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE [ESRI].[WebServicesLogs] SET [Status]='Unresolved'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
        }

        private void SetGISIZELogToResolved()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("TRUNCATE TABLE [GISIZE].[Web_Services_Layers_Logs_LiveData]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
        }
        private void SetGISIZELogToUnresolved()
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE [GISIZE].[Web_Services_Layers_Logs_LiveData] SET [Status]='Unresolved'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
        }

        public void SendEmail(string Name, string Description, string IPAddress, string status)
        {
            try
            {
                //log.Info("--------------- SEND EMAIL PROCESS STARTED ---------------");
                //log.Info("\n Name: " + Name + " \n Description: " + Description + " \n IPAddress: " + IPAddress + " \n Status: " + status);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["Password"]);
                smtp.EnableSsl = false;

                MailMessage msg = new MailMessage();
                //msg.Subject = "PROD : " + new IPAddress().GetLocalIPAddress(true) + " - ISD Server " + status + " Error";
                msg.Subject = "Pro-Active Monitor Dashboard ISD Server" + status + "Error";
                //msg.Body = "<b>Machine Name : </b>" + new IPAddress().GetLocalIPAddress(true);
                //msg.Body += "<br/>";

                msg.Body += "<b>Server Name : </b>" + Name;
                msg.Body += "<br/>";

                msg.Body += "<b>Description : </b>" + Description;
                msg.Body += "<br/>";

                msg.Body += "<b>IPAddress : </b>" + IPAddress;
                msg.Body += "<br/>";
                msg.Body += "<font color='red'><b>Status : </b>" + status + " </font>";
                msg.Body += "<br/>";

                msg.Body += "<b>Error Time : </b>" + DateTime.Now;
                msg.Body += "<br/>";

                msg.Priority = MailPriority.High;
                msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
                string ToEmail = ConfigurationManager.AppSettings["ReceiverEmailGroup"];
                string[] Multi = ToEmail.Split(',');
                foreach (string Multiemailid in Multi)
                {
                    msg.To.Add(new MailAddress(Multiemailid));
                }
                msg.IsBodyHtml = true;
                smtp.Send(msg);
                //log.Info("Email Send Successfully!");
            }
            catch (Exception ex)
            {
               // log.Error(ex.ToString());
            }
        }

        public void SendEmail(StringBuilder table, string tableName, string module, Byte[] screenshot=null)
        {
            try
            {
                //log.Info("--------------- SEND EMAIL PROCESS STARTED ---------------");
                //log.Info("\n Table: " + table);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["Password"]);
                smtp.EnableSsl = false;

                MailMessage msg = new MailMessage();
                //msg.Subject = "PROD : " + new IPAddress().GetLocalIPAddress(true) + " - ISD Server " + status + " Error";
                msg.Subject = "Pro-Active Monitor Dashboard("+ module + ")" ;
                //msg.Body = "<b>Machine Name : </b>" + new IPAddress().GetLocalIPAddress(true);
                //msg.Body += "<br/>";
                msg.Body += "<b>" + tableName + " </b>";
                msg.Body += "<br/>";
                msg.Body += "<b>Error Time : </b>" + DateTime.Now;
                msg.Body += "<br/>";

                msg.Body += table;
                msg.Body += "<br/>";


                //msg.Body += "<b>IPAddress : </b>" + IPAddress;
                //msg.Body += "<br/>";
                //msg.Body += "<font color='red'><b>Status : </b>" + status + " </font>";
                //msg.Body += "<br/>";
                MemoryStream attachmentStream=null;
                Attachment screenshotAttachment=null;
                if (screenshot != null)
                {
                    attachmentStream = new MemoryStream(screenshot);
                    screenshotAttachment = new Attachment(attachmentStream, "screenshot.png", "image/png");
                    msg.Attachments.Add(screenshotAttachment);
                }
                

                msg.Priority = MailPriority.High;
                msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
                string ToEmail = ConfigurationManager.AppSettings["ReceiverEmailGroup"];
                string[] Multi = ToEmail.Split(',');
                foreach (string Multiemailid in Multi)
                {
                    msg.To.Add(new MailAddress(Multiemailid));
                }
                msg.IsBodyHtml = true;
                smtp.Send(msg);
                //log.Info("Email Send Successfully!");
                if (screenshot != null)
                {
                    screenshotAttachment.Dispose();
                    attachmentStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.ToString());
            }
        }

        protected Byte[] CaptureScreen()
        {
            double maxWidthDouble = System.Windows.SystemParameters.FullPrimaryScreenWidth;
            double maxHeightDouble = System.Windows.SystemParameters.FullPrimaryScreenHeight;
            int maxWidth = (int)Math.Round(maxWidthDouble);
            int maxHeight = (int)Math.Round(maxHeightDouble);

            //int screenWidth = SystemInformation.PrimaryMonitorSize.Width;
            //int screenHeight = SystemInformation.PrimaryMonitorSize.Height;

            // Capture the screenshot
            using (Bitmap screenshot = new Bitmap(1920, 1080))
            {
                using (Graphics graphics = Graphics.FromImage(screenshot))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
                }

                byte[] screenshotBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    screenshot.Save(ms, ImageFormat.Png);
                    screenshotBytes = ms.ToArray();
                }
                return screenshotBytes;
            }
            
        }

       

    }
}