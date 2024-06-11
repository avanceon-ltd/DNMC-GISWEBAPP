using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebAppForm
{
    public partial class AddEditManholes : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AddEditManholes));
        private string constr = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
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
                        GetSetData(sitename);
                        this.Title = "Update Manholes";
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
                    bool exists = checkIfRecordExists(Heirarichal);
                    string Framework = !String.IsNullOrEmpty(txtFramework.Text.Trim()) ? txtFramework.Text.Trim() : null;
                    string Facility = ddFacility.SelectedValue.Trim();
                    string AssetId = !String.IsNullOrEmpty(txtAssetId.Text.Trim()) ? txtAssetId.Text.Trim() : null;
                    string Catchment = txtCatchment.Text.Trim();
                    string AreaName = txtArea.Text.Trim();
                    string PSName = ddPSName.SelectedValue.Trim();
                    string CoverLvl = !String.IsNullOrEmpty(txtCoverLvl.Text.Trim()) ? txtCoverLvl.Text.Trim() : null;
                    string InvertLvl = !String.IsNullOrEmpty(txtInverLvl.Text.Trim()) ? txtInverLvl.Text.Trim() : null;
                    string ChannelWdt = !String.IsNullOrEmpty(txtChannelWdt.Text.Trim()) ? txtChannelWdt.Text.Trim() : null;
                    string HiSP = !String.IsNullOrEmpty(txtHiSP.Text.Trim()) ? txtHiSP.Text.Trim() : null;
                    string HiHiSP = !String.IsNullOrEmpty(txtHiHiSP.Text.Trim()) ? txtHiHiSP.Text.Trim() : null;
                    string ServiceType = ddServiceType.SelectedValue.Trim();
                    string Longitude = !String.IsNullOrEmpty(txtLong.Text.Trim()) ? txtLong.Text.Trim() : null;
                    string Latitude = !String.IsNullOrEmpty(txtLat.Text.Trim()) ? txtLat.Text.Trim() : null;
                    string PicPath = !String.IsNullOrEmpty(txtPicPAth.Text.Trim()) ? txtPicPAth.Text.Trim() : null;
                    string SiteName = lblSiteName.Text;
                    string ManholeStatus = !String.IsNullOrEmpty(txtManholeStatus.Text.Trim()) ? txtManholeStatus.Text.Trim() : null;
                    if (btnSubmit.Text != "Update Manhole")
                    {
                        if (!exists)
                        {
                            string query = "INSERT INTO [SCADA].[Manholes] VALUES( @SiteName, @Hierarchical_SiteName, @Facility_Type, @Asset_ID, @Framework, @Area_Name, @Catchment_Zone," +
                                " @PS_Name, @X, @Y, @Cover_Level, @Invert_Level, @Channel_Width, @Hi_SetPoint, @HiHiSetPoint, @Service_Type," +
                                " @Picture_Path, @updateDate, @insertedDate,@STATUS, @IsActive)";

                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                    cmd.Parameters.AddWithValue("@Facility_Type", Facility);
                                    cmd.Parameters.AddWithValue("@Asset_ID", AssetId != null ? AssetId : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Framework", Framework != null ? Framework : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Area_Name", AreaName);
                                    cmd.Parameters.AddWithValue("@Catchment_Zone", Catchment != null ? (Catchment.ToLower().Equals("n/a") ? "NOT APPLICABLE" : Catchment) : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@PS_Name", PSName != "-1" ? PSName : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@X", Longitude != null ? Longitude : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Y", Latitude != null ? Latitude : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Cover_Level", CoverLvl != null ? CoverLvl : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Invert_Level", InvertLvl != null ? InvertLvl : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Channel_Width", ChannelWdt != null ? ChannelWdt : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Hi_SetPoint", HiSP != null ? HiSP : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@HiHiSetPoint", HiHiSP != null ? HiHiSP : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Service_Type", ServiceType);
                                    cmd.Parameters.AddWithValue("@Picture_Path", PicPath != null ? PicPath : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@insertedDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@STATUS", ManholeStatus != null ? ManholeStatus : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@IsActive", true);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            query = "update SCADA.Manholes set Framework = ( select EXT_FRAMEZONE FROM ODW.EAMS.DimAsset " +
                                "where ASSETNUM = @AssetId ) " +
                                "WHERE Hierarchical_SiteName = @Hierarchical_SiteName";

                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@AssetId", AssetId);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            lblMessage.Text = "Manhole added successfully.";
                            ClearControls();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                        else
                        {
                            lblMessage.Text = "Manhole already exists.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                    }
                    else
                    {
                        string query = "UPDATE [SCADA].[Manholes] SET Site_Name= @SiteName, Hierarchical_SiteName= @Hierarchical_SiteName, Facility_Type= @Facility_Type, Asset_ID= @Asset_ID," +
                            " Framework= @Framework, Area_Name= @Area_Name, Catchment_Zone= @Catchment_Zone, PS_Name= @PS_Name, X= @X, Y= @Y, " +
                            "Cover_Level= @Cover_Level, Invert_Level= @Invert_Level, Channel_Width= @Channel_Width, Hi_SetPoint= @Hi_SetPoint, HiHi_SetPoint= @HiHi_SetPoint," +
                             "Service_Type= @Service_Type, Pictures_Path = @Pictures_Path,  updateDate= @updateDate,STATUS=@STATUS WHERE Hierarchical_SiteName = @Hierarchical_SiteName";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                cmd.Parameters.AddWithValue("@Facility_Type", Facility);
                                cmd.Parameters.AddWithValue("@Asset_ID", AssetId != null ? AssetId : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Framework", Framework != null ? Framework : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Area_Name", AreaName);
                                cmd.Parameters.AddWithValue("@Catchment_Zone", Catchment);
                                cmd.Parameters.AddWithValue("@PS_Name", PSName != "-1" ? PSName : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@X", Longitude != null ? Longitude : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Y", Latitude != null ? Latitude : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Cover_Level", CoverLvl != null ? CoverLvl : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Invert_Level", InvertLvl != null ? InvertLvl : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Channel_Width", ChannelWdt != null ? ChannelWdt : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Hi_SetPoint", HiSP != null ? HiSP : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@HiHi_SetPoint", HiHiSP != null ? HiHiSP : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Service_Type", ServiceType);
                                cmd.Parameters.AddWithValue("@Pictures_Path", PicPath != null ? PicPath : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@STATUS", ManholeStatus != null ? ManholeStatus : (object)DBNull.Value);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        query = "update SCADA.Manholes set Framework = ( select EXT_FRAMEZONE FROM ODW.EAMS.DimAsset " +
                                "where ASSETNUM = @AssetId ) " +
                                "WHERE Hierarchical_SiteName = @Hierarchical_SiteName";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@AssetId", AssetId);
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        lblMessage.Text = "Manhole updated successfully.";
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

        private void GetSetData(string siteName)
        {
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SCADA].[Manholes] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)", conn);
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
                    lblSiteName.Text = row["Site_Name"].ToString();
                    txtSiteName.Text = row["Hierarchical_SiteName"].ToString();
                    ddFacility.Text = row["Facility_Type"].ToString();
                    ddPSName.Text = row["PS_Name"].ToString();
                    ddServiceType.Text = row["Service_Type"].ToString();
                    txtFramework.Text = row["Framework"].ToString();
                    txtArea.Text = row["Area_Name"].ToString();
                    txtAssetId.Text = row["Asset_ID"].ToString();
                    txtCatchment.Text = row["Catchment_Zone"].ToString();
                    txtChannelWdt.Text = row["Channel_Width"].ToString();
                    txtCoverLvl.Text = row["Cover_Level"].ToString();
                    txtHiHiSP.Text = row["HiHi_SetPoint"].ToString();
                    txtHiSP.Text = row["Hi_SetPoint"].ToString();
                    txtInverLvl.Text = row["Invert_Level"].ToString();
                    txtPicPAth.Text = row["Pictures_Path"].ToString();
                    txtLong.Text = row["X"].ToString();
                    txtLat.Text = row["Y"].ToString();

                    cmd = new SqlCommand("Select Status From EAMS.DimAsset where ASSETNUM = @AssetId", conn);
                    cmd.Parameters.AddWithValue("@AssetId", txtAssetId.Text);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0) { row = dt.Rows[0]; }
                    txtManholeStatus.Text = row["Status"].ToString();
                }
                else
                {
                    lblMessage.Text = "No Record Found.";
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                }
                conn.Close();

                btnSubmit.Text = "Update Manhole";
                Label2.Text = "Edit " + siteName;
                lblFrmwrk.Visible = true;
                txtFramework.Visible = true;
                lblManholeStatus.Visible = true;
                txtManholeStatus.Visible = true;
                txtSiteName.ReadOnly = true;
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
                string com = "Select DISTINCT [Facility_Type] from [SCADA].[Manholes] Where Facility_Type is not NULL";
                SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                ddFacility.DataSource = dt;
                ddFacility.DataTextField = "Facility_Type";
                ddFacility.DataValueField = "Facility_Type";
                ddFacility.DataBind();

                // Service Type
                com = "Select DISTINCT [Service_Type] from [SCADA].[Manholes] Where [Service_Type] is not NULL";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ddServiceType.DataSource = dt;
                ddServiceType.DataTextField = "Service_Type";
                ddServiceType.DataValueField = "Service_Type";
                ddServiceType.DataBind();

                // PS Name
                com = "Select DISTINCT [Hierarchical_SiteName] from [SCADA].[MXLocations] Where [Hierarchical_SiteName] is not NULL";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ddPSName.DataSource = dt;
                ddPSName.DataTextField = "Hierarchical_SiteName";
                ddPSName.DataValueField = "Hierarchical_SiteName";
                ddPSName.DataBind();
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            
        }

        private bool checkIfRecordExists(string SiteName)
        {
            bool check = false;
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [SCADA].[Manholes] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)", conn);
                check_User_Name.Parameters.AddWithValue("@Hierarchical_SiteName", SiteName);
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
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddICCSContact).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
            return check;
        }

        private void ClearControls()
        {
            try
            {
                txtCatchment.Text = "";
                txtArea.Text = "";
                txtAssetId.Text = "";
                txtFramework.Text = "";
                txtCatchment.Text = "";
                txtChannelWdt.Text = "";
                txtCoverLvl.Text = "";
                txtHiHiSP.Text = "";
                txtHiSP.Text = "";
                txtInverLvl.Text = "";
                txtLat.Text = "";
                txtLong.Text = "";
                txtPicPAth.Text = "";
                txtSiteName.Text = "";
                ddFacility.SelectedIndex = -1;
                ddPSName.SelectedIndex = -1;
                ddServiceType.SelectedIndex = -1;
                lblSiteName.Text = "";
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditManholes).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
    }
}