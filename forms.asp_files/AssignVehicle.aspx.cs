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
    public partial class AssignVehicle : System.Web.UI.Page
    {
        public String VEHNUM { get; set; }
        public String WONUM { get; set; }
        public String USER { get; set; }

        protected void Page_Load(object sender, EventArgs e)
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

            lblConfirm.Text = "Are you sure you want to assign vehicle(s)# " + this.VEHNUM + " to Work Order(s)# " + this.WONUM;
        }

        void VehicleAssignement()
        {
            try
            {
                string assignUri = System.Web.Configuration.WebConfigurationManager.AppSettings["AssignVehicle"];
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

        protected void btnInformation_Click(object sender, EventArgs e)
        {

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            VehicleAssignement();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "closePage", "window.close();",true);
            //Response.Write("<script>window.close();</script>");
            //Response.Write("<script language='javascript'> { self.close();return false; }</script>");
            //ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/JavaScript'>window.close();return true;</script>");            

        }
    }
}