using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppForm.App_Code;

namespace WebAppForm
{
    public partial class AssignVehicle : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AssignVehicle));
        public String VEHNUM { get; set; }
        public String WONUM { get; set; }
        public String USER { get; set; }
        private string Result { get; set; }
        private string Status { get; set; }

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

        //void VehicleAssignement2()
        //{
        //    try
        //    {
        //        ESBServices esbServices = new ESBServices();
        //        RedHatService redHatervices = new RedHatService();
        //        var result = new Tuple<bool, string,string>(false, "","");
        //        if (Convert.ToInt32(ConfigurationManager.AppSettings["ESBFlag"]) == 1)
        //        {
        //            result = esbServices.VehicleAssignement(WONUM,VEHNUM,USER);
        //        }
        //        else
        //        {
        //            result = redHatervices.VehicleAssignement(WONUM, VEHNUM, USER);
        //        }

        //        lblMessage.Text = "<b>" + result.Item2 + "</b>";
        //        if (result.Item3 == "success")
        //        {
        //            tblConfirm.Visible = false;
        //            btnOK.Visible = false;
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        //    /*
        //    try
        //    {
        //        string assignUri = System.Web.Configuration.WebConfigurationManager.AppSettings["AssignVehicle"];
        //        assignUri += "?wo=" + this.WONUM + "&vehicle=" + this.VEHNUM + "&user=" + this.USER;
        //        string authValue = System.Web.Configuration.WebConfigurationManager.AppSettings["FAUTH"];
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(assignUri);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "GET";
        //        httpWebRequest.Headers["FAUTH"] = authValue;

        //        using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
        //        {
        //            JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
        //            var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
        //            var jsonResult = JObject.Parse(jsonObject.ToString());
        //            lblMessage.Text = "<b>" + jsonResult.SelectToken("Message.Description").ToString() + "</b>";

        //            if (jsonResult.SelectToken("Message.Status").ToString().ToLower() == "success")
        //            {
        //                tblConfirm.Visible = false;
        //                btnOK.Visible = false;
        //            }
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        //        }
        //    }
        //    catch (WebException wex)
        //    {
        //        JsonTextReader jsonReader = new JsonTextReader(new StreamReader(wex.Response.GetResponseStream()));
        //        var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
        //        var jsonResult = JObject.Parse(jsonObject.ToString());
        //        lblMessage.Text = "<b>" + jsonResult.SelectToken("Error.Description").ToString() + "</b>";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        //    }
        //    catch (Exception exp)
        //    {
        //        lblMessage.Text = exp.Message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        //    }
        //    */
        //}
        void VehicleAssignement()
        {
            ILog log = LogManager.GetLogger("AssignVehicle");

            try
            {
                int PlateNumber = VEHNUM.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                int WorkOrder = WONUM.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ODW_STG_ConnectionString"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        if (PlateNumber > 1 && WorkOrder == 1 && PlateNumber <= 10)
                        {
                            cmd.CommandText = "AddVehicles_WO";
                        }
                        else if (PlateNumber == 1 && WorkOrder > 1 && WorkOrder <= 3)
                        {
                            cmd.CommandText = "AddWorkorders_Vehicle";
                        }
                        else if (PlateNumber == 1 && WorkOrder == 1)
                        {
                            cmd.CommandText = "AddWO_Vehicle";
                        }

                        cmd.Parameters.Add("@PlateNumber", SqlDbType.VarChar, 500).Value = VEHNUM;
                        cmd.Parameters.Add("@WONUM", SqlDbType.VarChar, 500).Value = WONUM;
                        cmd.Parameters.Add("@USER", SqlDbType.VarChar, 500).Value = USER;
                        cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        Result = Convert.ToString(cmd.Parameters["@RESULT"].Value);
                        Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                        cmd.Dispose();
                    }
                    con.Close();
                    con.Dispose();
                }
                if (Result.Contains("Violation of PRIMARY KEY"))
                {
                    log.Info("Primary key Violation" + Result);
                    Result = "Already " + VEHNUM + " vehicles Assigned to " + WONUM + " workorder";
                }

                lblMessage.Text = "<b>" + Result + "</b>";
                if (Status.ToLower() == "success")
                {
                    tblConfirm.Visible = false;
                    btnOK.Visible = false;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                log.Info("Succesfully inserted Vehicle assignment record.");
            }
            catch (WebException wex)
            {
                log.Error("Error Message: " + wex.Message);
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(wex.Response.GetResponseStream()));
                var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                var jsonResult = JObject.Parse(jsonObject.ToString());
                lblMessage.Text = "<b>" + jsonResult.SelectToken("Error.Description").ToString() + "</b>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            catch (Exception exp)
            {
                log.Error("Error Message: " + exp.Message + "\n Error Inner Exception: " + exp.InnerException);
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