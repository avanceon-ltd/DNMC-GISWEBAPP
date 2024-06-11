using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebAppForm
{
    /// <summary>
    /// Built by great "the yasir miraj"
    /// </summary>

    public partial class AddEditMXLocation : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AddEditMXLocationAsset));
        private string constr = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        private static string Hierarchical_SiteName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblSiteName.Attributes.Add("readonly", "readonly");
                    prepareDropDowns();
                    string sitename = Request.QueryString["SiteName"];
                    string AccessLevel = Request.QueryString["AccessLevel"];
                    if (!string.IsNullOrEmpty(sitename))
                    {
                        ddFacilityName.Visible = true;
                        FacilityNameDDLbl.Visible = true;
                        ddFacility.Enabled = true;
                        GetSetData(sitename);
                        this.Title = "Update Facilty";
                    }
                    else
                    {
                        ddFacility.Enabled = true;
                    }
                    if (!string.IsNullOrEmpty(AccessLevel))
                    {
                        int acclvl = Convert.ToInt32(AccessLevel);
                        if (acclvl >= 7777)
                        {
                            btnSubmit.Enabled = true;
                        }
                        else
                        {
                            btnSubmit.Enabled = false;
                            btnSubmit.CssClass = "btnDisabled";
                        }
                    }
                    else
                    {
                        btnSubmit.Enabled = false;
                        btnSubmit.CssClass = "btnDisabled";
                    }
                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {

                    string Heirarichal = txtSiteName.Text.Trim();
                    string MxLocation = txtMXLocation.Text.Trim();
                    bool exists = checkIfRecordExists(MxLocation, Heirarichal);
                    string Facility = ddFacility.SelectedValue.Trim();

                    string Catchment = txtCatchment.Text.Trim();
                    string Muncipality = txtMuncipality.Text.Trim();
                    string LocationDesc = txtMXLocation.Text.Trim();
                    string Upstream = !String.IsNullOrEmpty(txtUpstream.Text.Trim()) ? txtUpstream.Text.Trim() : null;
                    string ImeUpstream = !String.IsNullOrEmpty(txtImeUpstream.Text.Trim()) ? txtImeUpstream.Text.Trim() : null;
                    string ImeUpstreamAlt = !String.IsNullOrEmpty(txtImeUpstreamAlt.Text.Trim()) ? txtImeUpstreamAlt.Text.Trim() : null;
                    string Downstream = !String.IsNullOrEmpty(txtDownstream.Text.Trim()) ? txtDownstream.Text.Trim() : null;
                    string ImeDownstream = !String.IsNullOrEmpty(txtImeDownstream.Text.Trim()) ? txtImeDownstream.Text.Trim() : null;
                    string ImeDownstreamAlt = !String.IsNullOrEmpty(txtImeDownstreamAlt.Text.Trim()) ? txtImeDownstreamAlt.Text.Trim() : null;
                    string Longitude = txtLong.Text.Trim();
                    string Latitude = txtLat.Text.Trim();
                    string ProcessDest = !String.IsNullOrEmpty(txtProcessDestination.Text.Trim()) ? txtProcessDestination.Text.Trim() : null;
                    string SiteName = lblSiteName.Text;
                    string DesignCapac = !String.IsNullOrEmpty(txtDesignCap.Text.Trim()) ? txtDesignCap.Text.Trim() : null;
                    string HyperLink = txtCDMS.Text.Trim();
                    string Framezone = ddFrameZone.SelectedValue.Trim();
                    string Status = ddSiteStatus.SelectedValue.Trim();
                    if (btnSubmit.Text != "Update")
                    {
                        if (!exists)
                        {
                            string query = "INSERT INTO [SCADA].[MXLocations] VALUES(@MXLocation, @SiteName, @Hierarchical_SiteName, @Facility_Type, @Framework, @Municipality, @Location, @Catchment," +
                                " @Upstream, @Immediate_Upstream, @Immediate_Upstream_Alternative, @Downstream, @Immediate_Downstream, @Immediate_Downstream_Alternative, @Process_Destination, @X, @Y," +
                                " @Design_Capacity_m3_Day, @updateDate, @insertedDate,@STATUS, @IsActive)";

                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@MXLocation", MxLocation);
                                    cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                    cmd.Parameters.AddWithValue("@Facility_Type", Facility == "-1" ? (object)DBNull.Value : Facility);
                                    cmd.Parameters.AddWithValue("@Framework", Framezone == "Not Applicable" ? (object)DBNull.Value : Framezone);
                                    cmd.Parameters.AddWithValue("@Status", Status == "-1" ? (object)DBNull.Value : Status);
                                    cmd.Parameters.AddWithValue("@Municipality", Muncipality);
                                    cmd.Parameters.AddWithValue("@Location", LocationDesc);
                                    cmd.Parameters.AddWithValue("@Catchment", Catchment);
                                    cmd.Parameters.AddWithValue("@Upstream", Upstream != null ? (Upstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : Upstream) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Immediate_Upstream", ImeUpstream != null ? (ImeUpstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeUpstream) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Immediate_Upstream_Alternative", ImeUpstreamAlt != null ? (ImeUpstreamAlt.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeUpstreamAlt) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Downstream", Downstream != null ? (Downstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : Downstream) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Immediate_Downstream", ImeDownstream != null ? (ImeDownstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeDownstream) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Immediate_Downstream_Alternative", ImeDownstreamAlt != null ? (ImeDownstreamAlt.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeDownstreamAlt) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Process_Destination", ProcessDest != null ? ProcessDest : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@X", Longitude);
                                    cmd.Parameters.AddWithValue("@Y", Latitude);
                                    cmd.Parameters.AddWithValue("@Design_Capacity_m3_Day", DesignCapac != null ? DesignCapac : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@insertedDate", DateTime.Now);

                                    cmd.Parameters.AddWithValue("@IsActive", true);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            query = "insert into [CDMS].[SiteCorrelationList]([Facility],[Hyperlink],[MXLocationId],[Hierarchical_SiteName],[TableName]) VALUES (@Facility,@Hyperlink,@MXLocation,@Hierarchical_SiteName,@TableName)";
                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@Facility", Facility);
                                    cmd.Parameters.AddWithValue("@HyperLink", HyperLink);
                                    cmd.Parameters.AddWithValue("@MXLocation", MxLocation);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                    cmd.Parameters.AddWithValue("@TableName", "SCADA.MXLocations");
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            lblMessage.Text = "Record added successfully.";
                            ClearControls();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                        else
                        {
                            lblMessage.Text = "Location already exists.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                    }
                    else
                    {
                        string query = "UPDATE [SCADA].[MXLocations] SET MXLocation = @MXLocation, SiteName= @SiteName, Hierarchical_SiteName= @Hierarchical_SiteName, Facility_Type= @Facility_Type, Municipality= @Municipality," +
                            " Framework =@Framework, Status=@Status, Location= @Location, Catchment= @Catchment, Upstream= @Upstream, Immediate_Upstream= @Immediate_Upstream, Immediate_Upstream_Alternative= @Immediate_Upstream_Alternative, Downstream= @Downstream, " +
                            "Immediate_Downstream= @Immediate_Downstream, Immediate_Downstream_Alternative= @Immediate_Downstream_Alternative, Process_Destination= @Process_Destination, X= @X, Y= @Y," +
                             "Design_Capacity_m3_Day= @Design_Capacity_m3_Day, updateDate= @updateDate WHERE Hierarchical_SiteName = '" + Hierarchical_SiteName + "'";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@MXLocation", MxLocation);
                                cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                cmd.Parameters.AddWithValue("@Facility_Type", Facility == "-1" ? (object)DBNull.Value : Facility);
                                cmd.Parameters.AddWithValue("@Framework", Framezone == "Not Applicable" ? (object)DBNull.Value : Framezone);
                                cmd.Parameters.AddWithValue("@Status", Status == "-1" ? (object)DBNull.Value : Status);
                                cmd.Parameters.AddWithValue("@Municipality", Muncipality);
                                cmd.Parameters.AddWithValue("@Location", LocationDesc);
                                cmd.Parameters.AddWithValue("@Catchment", Catchment);
                                cmd.Parameters.AddWithValue("@Upstream", Upstream != null ? (Upstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : Upstream) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Immediate_Upstream", ImeUpstream != null ? (ImeUpstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeUpstream) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Immediate_Upstream_Alternative", ImeUpstreamAlt != null ? (ImeUpstreamAlt.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeUpstreamAlt) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Downstream", Downstream != null ? (Downstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : Downstream) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Immediate_Downstream", ImeDownstream != null ? (ImeDownstream.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeDownstream) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Immediate_Downstream_Alternative", ImeDownstreamAlt != null ? (ImeDownstreamAlt.ToLower().Equals("n/a") ? "NOT APPLICABLE" : ImeDownstreamAlt) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Process_Destination", ProcessDest != null ? ProcessDest : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@X", Longitude);
                                cmd.Parameters.AddWithValue("@Y", Latitude);
                                cmd.Parameters.AddWithValue("@Design_Capacity_m3_Day", DesignCapac != null ? DesignCapac : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);

                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }


                        query = "UPDATE [SCADA].[MXLocations_Assets] SET MXLocation = @MXLocation, SiteName= @SiteName, " +
                            "Hierarchical_SiteName= '" + txtSiteName.Text.Trim()+ ".' + SUBSTRING(@Hierarchical_SiteName, CHARINDEX('.', @Hierarchical_SiteName) + 1, LEN(@Hierarchical_SiteName)),  " +
                            "WHERE Hierarchical_SiteName = '" + Hierarchical_SiteName + "'";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@MXLocation", txtMXLocation.Text.Trim());
                                cmd.Parameters.AddWithValue("@SiteName", txtSiteName.Text.Trim());
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                               
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);

                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        query = "update CDMS.SiteCorrelationList set Hyperlink=@Hyperlink,MXLocationId=@MXLocationId,updateDate=getdate() where " +
                               "TableName = @TableName and Hierarchical_SiteName = @Hierarchical_SiteName";
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@TableName", "SCADA.MXLocations");
                                cmd.Parameters.AddWithValue("@Hyperlink", HyperLink);
                                cmd.Parameters.AddWithValue("@MXLocationId", MxLocation);
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        lblMessage.Text = "Record updated successfully.";
                        ClearControls();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        private void ClearControls()
        {
            try
            {
                txtCatchment.Text = "";
                txtDesignCap.Text = "";
                txtCDMS.Text = "";
                txtDownstream.Text = "";
                txtFramework.Text = "";
                txtImeDownstream.Text = "";
                txtImeDownstreamAlt.Text = "";
                txtImeUpstream.Text = "";
                txtImeUpstreamAlt.Text = "";
                ddFacility.SelectedIndex = -1;
                txtLat.Text = "";
                txtLocDesc.Text = "";
                txtLong.Text = "";
                txtMuncipality.Text = "";
                txtMXLocation.Text = "";
                txtProcessDestination.Text = "";
                txtSiteName.Text = "";
                txtUpstream.Text = "";
                lblSiteName.Text = "";
                ddFrameZone.SelectedIndex = -1;
                ddFacilityName.SelectedIndex = -1;
                ddSiteStatus.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        private void prepareDropDowns()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                // Facility DropDown
                string com = "Select DISTINCT [Facility_Type] from [SCADA].[MXLocations] Where Facility_Type is not NULL order by [Facility_Type]";
                SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                ddFacility.DataSource = dt;
                ddFacility.DataTextField = "Facility_Type";
                ddFacility.DataValueField = "Facility_Type";
                ddFacility.DataBind();

                com = "Select  Hierarchical_SiteName FROM SCADA.MXLocations  order by Hierarchical_SiteName";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ddFacilityName.DataSource = dt;
                ddFacilityName.DataTextField = "Hierarchical_SiteName";
                ddFacilityName.DataValueField = "Hierarchical_SiteName";
                ddFacilityName.DataBind();

                com = "select EXT_FRAMEZONE from (Select distinct isnull(EXT_FRAMEZONE,'Not Applicable')  as EXT_FRAMEZONE From EAMS.DimLocation) as tbl order by EXT_FRAMEZONE";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ddFrameZone.DataSource = dt;
                ddFrameZone.DataTextField = "EXT_FRAMEZONE";
                ddFrameZone.DataValueField = "EXT_FRAMEZONE";
                ddFrameZone.DataBind();

                com = "select distinct STATUS from eams.DimLocation order by STATUS";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ddSiteStatus.DataSource = dt;
                ddSiteStatus.DataTextField = "STATUS";
                ddSiteStatus.DataValueField = "STATUS";
                ddSiteStatus.DataBind();
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

        }
        protected void OnFacilityNameChanged(object sender, EventArgs e)
        {
            try
            {
                GetSetData(ddFacilityName.SelectedValue);
            }
            catch(Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        private void GetSetData(string siteName)
        {
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "SELECT MXLocation,SiteName,Hierarchical_SiteName,Facility_Type,isnull(Framework,'Not Applicable') as Framework" +
                    ",STATUS,Municipality,Location,Catchment,Upstream,Immediate_Upstream,Immediate_Upstream_Alternative,Downstream,Immediate_Downstream," +
                    "Immediate_Downstream_Alternative,Process_Destination,X,Y,Design_Capacity_m3_Day FROM [SCADA].[MXLocations] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", siteName);
                cmd.CommandType = CommandType.Text;
                // Create a DataAdapter to run the command and fill the DataTable
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMXLocation.Text = row["MXLocation"].ToString();
                    lblSiteName.Text = row["SiteName"].ToString();
                    ddSiteStatus.Text = String.IsNullOrEmpty(row["STATUS"].ToString()) ? "-1" : row["STATUS"].ToString();
                    txtSiteName.Text = row["Hierarchical_SiteName"].ToString();
                    Hierarchical_SiteName = txtSiteName.Text.ToString();
                    ddFacility.Text = String.IsNullOrEmpty(row["Facility_Type"].ToString()) ? "-1" : row["Facility_Type"].ToString();
                    ddFacilityName.Text = row["Hierarchical_SiteName"].ToString();
                    ddFrameZone.Text = row["Framework"].ToString();
                    txtMuncipality.Text = row["Municipality"].ToString();
                    txtLocDesc.Text = row["Location"].ToString();
                    txtCatchment.Text = row["Catchment"].ToString();
                    txtUpstream.Text = row["Upstream"].ToString();
                    txtImeUpstream.Text = row["Immediate_Upstream"].ToString();
                    txtImeUpstreamAlt.Text = row["Immediate_Upstream_Alternative"].ToString();
                    txtDownstream.Text = row["Downstream"].ToString();
                    txtImeDownstream.Text = row["Immediate_Downstream"].ToString();
                    txtImeDownstreamAlt.Text = row["Immediate_Downstream_Alternative"].ToString();
                    txtProcessDestination.Text = row["Process_Destination"].ToString();
                    txtLong.Text = row["X"].ToString();
                    txtLat.Text = row["Y"].ToString();
                    txtDesignCap.Text = row["Design_Capacity_m3_Day"].ToString();

                    cmd = new SqlCommand("Select Status,EXT_FRAMEZONE FROM ODW.EAMS.DimLocation where LOCATION = @MXLocation", conn);
                    cmd.Parameters.AddWithValue("@MXLocation", txtMXLocation.Text);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        txtSiteStatus.Text = row["Status"].ToString();
                        txtFramework.Text = row["EXT_FRAMEZONE"].ToString();
                    }
                    cmd = new SqlCommand("select Hyperlink from CDMS.SiteCorrelationList where TableName = 'SCADA.MXLocations' and Hierarchical_SiteName = @SiteName", conn);
                    cmd.Parameters.AddWithValue("@SiteName", txtSiteName.Text);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        txtCDMS.Text = row["Hyperlink"].ToString();
                    }

                }
                else
                {
                    lblMessage.Text = "No Record Found.";
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                }
                conn.Close();

                btnSubmit.Text = "Update";
                Label2.Text = "Update " + siteName + " Pumping Station";
                lblFrmwrk.Visible = true;
                txtFramework.Visible = true;
                lblSiteStatus.Visible = true;
                txtSiteStatus.Visible = true;
                txtSiteName.Visible = true;
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

        }
        private bool checkIfRecordExists(string MXLocation, string SiteName)
        {
            bool check = false;
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [SCADA].[MXLocations] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName or MXLocation=@MXLocation )", conn);
                check_User_Name.Parameters.AddWithValue("@Hierarchical_SiteName", SiteName);
                check_User_Name.Parameters.AddWithValue("@MXLocation", MXLocation);
                int UserExist = (int)check_User_Name.ExecuteScalar();
                conn.Close();
                if (UserExist > 0)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            return check;
        }

    }
}