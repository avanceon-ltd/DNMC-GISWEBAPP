using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class UnAssignVehicle : System.Web.UI.Page
    {
        public String VEHNUM { get; set; }
        public String WONUM { get; set; }
        public String USER { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //btnCancel.Attributes.Add("onclick", "window.open('', '_self');window.close();");
        }

        void VehicleUnAssignement()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["VEHNUM"]))
            {
                this.VEHNUM = RemoveEmptyValues(Request.QueryString["VEHNUM"].Trim());
            }
            if (!String.IsNullOrEmpty(Request.QueryString["WONUM"]))
            {
                this.WONUM = RemoveEmptyValues(Request.QueryString["WONUM"].Trim());
            }
            if (!String.IsNullOrEmpty(Request.QueryString["USER"]))
            {
                this.USER = Request.QueryString["USER"].Trim();
            }

            try
            {
                string assignUri = System.Web.Configuration.WebConfigurationManager.AppSettings["UnAssignVehicle"];
                assignUri += "?wo=" + this.WONUM + "&vehicle=" + this.VEHNUM + "&user=" + this.USER;
                string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(assignUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers["FAUTH"] = authValue;

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());
                    lblMessage.Text = "<b>" + jsonResult.SelectToken("Message.Description").ToString() + "</b>";

                    if (jsonResult.SelectToken("Message.Status").ToString().ToLower() == "success")
                    {
                        lblMessage.Text = "Vehicle un-assigned successfully.";
                        tblConfirm.Visible = false;
                        btnOK.Visible = false;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                }
            }
            catch (WebException wex)
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(wex.Response.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                lblMessage.Text = "<b>" + jsonResult.SelectToken("Error.Description").ToString() + "</b>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            catch (Exception exp)
            {
                lblMessage.Text = exp.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        string RemoveEmptyValues(String queryString)
        {
            String[] values = queryString.Split(',');
            String result = "";
            foreach (string value in values)
            {
                if (!String.IsNullOrEmpty(value.Trim()))
                    result += value + ",";
            }

            return result.Remove(result.Length - 1);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            VehicleUnAssignement();
        }
    }
}