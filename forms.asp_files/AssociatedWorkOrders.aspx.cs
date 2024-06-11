using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class AssociatedWorkOrders : System.Web.UI.Page
    {

        public String VEHNUM { get; set; }
        public String WONUM { get; set; }
        public String USER { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
           
                if (!String.IsNullOrEmpty(Request.QueryString["USER"]))
                {
                    this.USER = Request.QueryString["USER"].Trim();
                }

                if (!String.IsNullOrEmpty(Request.QueryString["PlateNumber"]))
                {
                    this.VEHNUM = Request.QueryString["PlateNumber"].Trim();
                }
            
        }



        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvCustomers.Rows[e.RowIndex];
            this.WONUM = row.Cells[0].Text.Trim();
            this.VEHNUM = row.Cells[1].Text.Trim();
            e.Cancel = true;
           WOUnAssignement();
            gvCustomers.DataBind();
            DataList5.DataBind();


        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string WONumber = e.Row.Cells[0].Text;
                foreach (LinkButton button in e.Row.Cells[6].Controls.OfType<LinkButton>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to Unassign WorkOrder=" + WONumber + "?')){ return false; };";
                    }
                }
            }
        }



        void WOUnAssignement()
        {

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
                        lblMessage.Text = "Work Order un-assigned successfully.";
                       
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

    }
}