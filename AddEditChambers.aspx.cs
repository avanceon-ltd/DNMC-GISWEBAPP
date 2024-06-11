using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebAppForm
{
    public partial class AddEditChambers : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(typeof(AddEditChambers));
        private string constr = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        protected void Page_Load(object sender, EventArgs e)
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
                    GetSetData(sitename);
                    this.Title = "Update Chambers";
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string Heirarichal = txtSiteName.Text.Trim();
                    bool exists = checkIfRecordExists(Heirarichal);
                    //string Framework = !String.IsNullOrEmpty(txtFramework.Text.Trim()) ? txtFramework.Text.Trim() : null;
                    string Framework = FrameworkDD.SelectedValue.Trim();
                    string Status = ChamberStatusDD.SelectedValue.Trim();
                    string Facility = ddFacility.SelectedValue.Trim();
                    string AssetId = !String.IsNullOrEmpty(txtAssetId.Text.Trim()) ? txtAssetId.Text.Trim() : null;
                    string Location = !String.IsNullOrEmpty(txtLocation.Text.Trim()) ? txtLocation.Text.Trim() : null;
                    string Municipality = !String.IsNullOrEmpty(txtMuncipality.Text.Trim()) ? txtMuncipality.Text.Trim() : null;
                    string Longitude = !String.IsNullOrEmpty(txtLong.Text.Trim()) ? txtLong.Text.Trim() : null;
                    string Latitude = !String.IsNullOrEmpty(txtLat.Text.Trim()) ? txtLat.Text.Trim() : null;
                    string ChamberStatus = !String.IsNullOrEmpty(txtChamberStatus.Text.Trim()) ? txtChamberStatus.Text.Trim() : null;
                    string HyperLink = txtCDMS.Text.Trim();
                    string SiteName = lblSiteName.Text;
                    bool Customer = isCustomer.Checked;
                    string GIS_Ref = GISRef.Text;
                    string CRMS_ID = CRMSID.Text;
                    string Customer_Name = CustomerName.Text;
                    string Service_Location = ServiceLocation.Text;


                    if (btnSubmit.Text != "Update")
                    {
                        if (!exists)
                        {

                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    cmd.Connection = con;

                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "[SCADA].[sp_Insert_Update_Chambers]";
                                    cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                    cmd.Parameters.AddWithValue("@Facility_Type", Facility);
                                    cmd.Parameters.AddWithValue("@Asset_ID", AssetId != null ? AssetId : (object)DBNull.Value);
                                    //cmd.Parameters.AddWithValue("@Framework", Framework != null ? Framework : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Framework", Framework);
                                    cmd.Parameters.AddWithValue("@STATUS", Status);
                                    cmd.Parameters.AddWithValue("@Municipality", Municipality != null ? Municipality : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Location", Location != null ? Location : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@X", Longitude != null ? Longitude : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Y", Latitude != null ? Latitude : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Created_Date", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@Updated_Date", DateTime.Now);
                                    //cmd.Parameters.AddWithValue("@STATUS", ChamberStatus != null ? ChamberStatus : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@IsActive", true);
                                    cmd.Parameters.AddWithValue("@IsCustomer", Customer);
                                    cmd.Parameters.AddWithValue("@GISRef", GIS_Ref);
                                    cmd.Parameters.AddWithValue("@CRMSID", CRMS_ID);
                                    cmd.Parameters.AddWithValue("@CustomerName", Customer_Name);
                                    cmd.Parameters.AddWithValue("@ServiceLocation", Service_Location);
                                    cmd.Parameters.AddWithValue("@Param", "Add");

                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }


                            string query = "INSERT INTO [CDMS].[SiteCorrelationList]([Facility],[Hyperlink],[Hierarchical_SiteName],[TableName],[AssetID]) VALUES (@Facility,@Hyperlink,@Hierarchical_SiteName,@TableName,@AssetID)";

                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@Facility", Facility);
                                    cmd.Parameters.AddWithValue("@HyperLink", HyperLink);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                    cmd.Parameters.AddWithValue("@TableName", "SCADA.Chambers");
                                    cmd.Parameters.AddWithValue("@AssetID", AssetId != null ? AssetId : (object)DBNull.Value);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            lblMessage.Text = "Record  added successfully.";
                            ClearControls();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                        else
                        {
                            lblMessage.Text = "Chamber already exists.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = con;

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "[SCADA].[sp_Insert_Update_Chambers]";
                                cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarichal);
                                cmd.Parameters.AddWithValue("@Facility_Type", Facility);
                                cmd.Parameters.AddWithValue("@Asset_ID", AssetId != null ? AssetId : (object)DBNull.Value);
                                //cmd.Parameters.AddWithValue("@Framework", Framework != null ? Framework : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Framework", Framework);
                                cmd.Parameters.AddWithValue("@STATUS", Status);
                                cmd.Parameters.AddWithValue("@Municipality", Municipality != null ? Municipality : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Location", Location != null ? Location : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@X", Longitude != null ? Longitude : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Y", Latitude != null ? Latitude : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Created_Date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Updated_Date", DateTime.Now);
                                //cmd.Parameters.AddWithValue("@STATUS", ChamberStatus != null ? ChamberStatus : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@IsActive", true);
                                cmd.Parameters.AddWithValue("@IsCustomer", Customer);
                                cmd.Parameters.AddWithValue("@GISRef", GIS_Ref);
                                cmd.Parameters.AddWithValue("@CRMSID", CRMS_ID);
                                cmd.Parameters.AddWithValue("@CustomerName", Customer_Name);
                                cmd.Parameters.AddWithValue("@ServiceLocation", Service_Location);
                                cmd.Parameters.AddWithValue("@Param", "Update");

                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        string query = "update CDMS.SiteCorrelationList set Hyperlink=@Hyperlink, AssetID=@AssetID,updateDate=getdate() where " +
                               "TableName = @TableName and Hierarchical_SiteName = @Hierarchical_SiteName";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@TableName", "SCADA.Chambers");
                                cmd.Parameters.AddWithValue("@Hyperlink", HyperLink);
                                cmd.Parameters.AddWithValue("@AssetID", AssetId != null ? AssetId : (object)DBNull.Value);
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
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SCADA].[Chambers] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)", conn);
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
                    //txtFramework.Text = row["Framework"].ToString();
                    FrameworkDD.Text = row["Framework"] == DBNull.Value ? "-1" : row["Framework"].ToString();
                    ChamberStatusDD.Text = row["Status"] == DBNull.Value ? "-1" : row["STATUS"].ToString();
                    txtMuncipality.Text = row["Municipality"].ToString();
                    txtAssetId.Text = row["Asset_ID"].ToString();
                    txtLocation.Text = row["Location"].ToString();
                    txtLong.Text = row["X"].ToString();
                    txtLat.Text = row["Y"].ToString();
                    isCustomer.Checked = row["Customer"].ToString().Equals("True") || row["Customer"].ToString().Equals("true") ? true : false;
                    GISRef.Text = row["GISRef"].ToString();
                    CRMSID.Text = row["CRMSID"].ToString();
                    CustomerName.Text = row["Customer_Name"].ToString();
                    ServiceLocation.Text = row["Service_Location"].ToString();


                    cmd = new SqlCommand("Select Status,EXT_FRAMEZONE From EAMS.DimAsset where ASSETNUM = @AssetId", conn);
                    cmd.Parameters.AddWithValue("@AssetId", txtAssetId.Text);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        txtChamberStatus.Text = row["Status"].ToString();
                        txtFramework.Text = row["EXT_FRAMEZONE"].ToString();
                    }

                    cmd = new SqlCommand("select Hyperlink from CDMS.SiteCorrelationList where TableName = 'SCADA.Chambers' and Hierarchical_SiteName = @SiteName", conn);
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
                Label2.Text = "Update " + siteName + " Chamber";
                lblFrmwrk.Visible = true;
                txtFramework.Visible = true;
                lblChamberStatus.Visible = true;
                txtChamberStatus.Visible = true;
                txtSiteName.ReadOnly = true;

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
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
                string com = "Select DISTINCT [Facility_Type] from [SCADA].[Chambers] Where Facility_Type is not NULL order by Facility_Type";
                SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                ddFacility.DataSource = dt;
                ddFacility.DataTextField = "Facility_Type";
                ddFacility.DataValueField = "Facility_Type";
                ddFacility.DataBind();

                com = "Select Hierarchical_SiteName FROM SCADA.Chambers  order by Hierarchical_SiteName";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ddFacilityName.DataSource = dt;
                ddFacilityName.DataTextField = "Hierarchical_SiteName";
                ddFacilityName.DataValueField = "Hierarchical_SiteName";
                ddFacilityName.DataBind();

                com = "select EXT_FRAMEZONE from (Select distinct EXT_FRAMEZONE as EXT_FRAMEZONE From EAMS.DimAsset where EXT_FRAMEZONE is not null) as tbl order by EXT_FRAMEZONE";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                FrameworkDD.DataSource = dt;
                FrameworkDD.DataTextField = "EXT_FRAMEZONE";
                FrameworkDD.DataValueField = "EXT_FRAMEZONE";
                FrameworkDD.DataBind();

                com = "select distinct STATUS from eams.DimAsset order by STATUS";
                adpt = new SqlDataAdapter(com, con);
                dt = new DataTable();
                adpt.Fill(dt);
                ChamberStatusDD.DataSource = dt;
                ChamberStatusDD.DataTextField = "STATUS";
                ChamberStatusDD.DataValueField = "STATUS";
                ChamberStatusDD.DataBind();
            }
            catch(Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
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
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
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
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [SCADA].[Chambers] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)", conn);
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
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
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
                txtLocation.Text = "";
                txtMuncipality.Text = "";
                txtCDMS.Text = "";
                txtAssetId.Text = "";
                txtFramework.Text = "";
                txtLat.Text = "";
                txtLong.Text = "";
                txtSiteName.Text = "";
                ddFacility.SelectedIndex = -1;
                lblSiteName.Text = "";
                FrameworkDD.SelectedIndex = -1;
                ChamberStatusDD.SelectedIndex = -1;
                ddFacilityName.SelectedIndex = -1;
                GISRef.Text ="";
                CRMSID.Text = "";
                CustomerName.Text = "";
                ServiceLocation.Text = "";
            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }

        protected void isCustomer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isCustomer.Checked)
                {
                    GISRef.ReadOnly = false;
                    CRMSID.ReadOnly = false;
                    CustomerName.ReadOnly = false;
                    ServiceLocation.ReadOnly = false;
                }
                else
                {
                    GISRef.ReadOnly = true;
                    CRMSID.ReadOnly = true;
                    CustomerName.ReadOnly = true;
                    ServiceLocation.ReadOnly = true;
                }

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditChambers).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
    }
}