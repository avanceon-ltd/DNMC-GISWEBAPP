using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QRCoder;

namespace WebAppForm
{
    public partial class MapsLinkView : System.Web.UI.Page
    {
        string isWorkOrder = string.Empty;
        string Id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            notifyLabel.Visible = false;
            string lat = Request.QueryString["lat"];
            string lon = Request.QueryString["long"];
            string phone = Request.QueryString["ph"];
            Id = Request.QueryString["Id"];
            isWorkOrder = Request.QueryString["isWorkOrder"];
            string mapUrl = "http://maps.google.com/?q="+lat+","+lon;
            if (!IsPostBack)
            {
                labelQRCode.Text = mapUrl;
                phNumbers.Text = phone ;
            }
            // Create QR Code on Page Load
            string code = mapUrl;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 150;
            imgBarCode.Width = 150;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                PlaceHolder1.Controls.Add(imgBarCode);
            }
        }

        protected void sendMapLinkViaSMS_Click(object sender, EventArgs e)
        {
            var PhoneNumbers = phNumbers.Text.Split(',');
            sendMapLinkViaSMS.Enabled = false;
            sendMapLinkViaSMS.Text = "Sending...";
            var message = (isWorkOrder == "1" ?  "WorkOrder Number: " : "Complain Number: ") + Id + ", Link: " + labelQRCode.Text;
            foreach (var number in PhoneNumbers)
            {
                if (!string.IsNullOrEmpty(number))
                {
                    var num = number.Trim();
                    var result = GetData(number, message);
                    if(result.Contains("OK"))
                    {
                        notifyLabel.Text = "Message sent successfully";
                        notifyLabel.ForeColor = Color.Green;
                        notifyLabel.Visible = true;
                    }
                    else
                    {
                        notifyLabel.Text = result;
                        notifyLabel.ForeColor = Color.Red;
                        notifyLabel.Visible = true;
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    sendMapLinkViaSMS.Enabled = true;
                    sendMapLinkViaSMS.Text = "Send";
                }
            }
        }
        public string GetData(string SmsNumber, string message)
        {
            try
            {
                WebClient client = new WebClient();
                string address = @"http://pwaesms.ashghal.gov.qa/EWAPI/mysms.ashx?userName=ashgal.svc_es_dnmc&password=ES$Pwa2022&smsText=" + message + "&mobile=" + SmsNumber + "&recipientName=97433885208&senderID=Ashghal&taskName=Broadcast1&schedule=&priority=3";
                return string.Format("Returned Status: {0}", client.DownloadString(address));
            }
            catch (Exception ex)
            {
                notifyLabel.Text = ex.Message;
                //log.Error($"Error on SMS Serice, GetData(){ex}");
                return string.Empty;
            }

        }

        
    }
}