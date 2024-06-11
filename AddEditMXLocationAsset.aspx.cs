using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class AddEditMXLocationAsset : System.Web.UI.Page
    {
        // class variables
        ILog log = log4net.LogManager.GetLogger(typeof(AddEditMXLocationAsset));
        private string constr = ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString;
        private static string ErrorMessage = "An error occured while processing your request. Please contact administrator";
        String SearchLoc = string.Empty;

        // Class Function
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblSiteName.Attributes.Add("readonly", "readonly");
                    string SiteName = Request.QueryString["SiteName"];
                    string AccessLevel = Request.QueryString["AccessLevel"];
                    string IsEdit = !string.IsNullOrEmpty(Request.QueryString["isEdit"]) ? Request.QueryString["isEdit"] : "0";
                    prepareDropDowns();
                    if (!string.IsNullOrEmpty(SiteName))
                    {
                        if (IsEdit == "1")
                        {
                            ddAssets.Visible = true;
                            AssetsDDLbl.Visible = true;
                            GetSetData(SiteName);
                            this.Title = "Update MX Location Assets";
                        }
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
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
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
                    string AssetId = txtAssetId.Text.Trim();
                    bool exists = checkIfRecordExists(AssetId);
                    string MxLocation = ddMXLocation.SelectedItem.Text.Split('(', ')')[1].Trim();
                    string Heirarchical = txtSiteName.Text.Trim();
                    string SiteName = lblSiteName.Text.Trim();
                    string AssetTag = txtAssetTag.Text.Trim();
                    string TagName = txtTagName.Text.Trim();
                    string AssetDesc = txtAssetDesc.Text.Trim();

                    if (Request.QueryString["isEdit"] != "1")
                    {
                        if (!exists)
                        {
                            string query = "INSERT INTO [SCADA].[MXLocations_Assets] VALUES(@SiteName, @Hierarchical_SiteName, @Asset_ID, @Asset_Tag, @Asset_ID_Description, @MXLocation, " +
                                "@fileLoadDate, @insertedDate, @updateDate, @IsActive,@IsMainPump,@TagName)";
                            
                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@MXLocation", MxLocation);
                                    cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                    cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarchical);
                                    cmd.Parameters.AddWithValue("@Asset_ID", AssetId);
                                    cmd.Parameters.AddWithValue("@Asset_Tag", AssetTag);
                                    cmd.Parameters.AddWithValue("@Asset_ID_Description", AssetDesc);
                                    cmd.Parameters.AddWithValue("@fileLoadDate", DateTime.Now);   // ???
                                    cmd.Parameters.AddWithValue("@insertedDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);
                                    cmd.Parameters.AddWithValue("@IsMainPump", chkMainPump.Checked);
                                    cmd.Parameters.AddWithValue("@TagName", TagName);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }

                            lblMessage.Text = "Asset added successfully.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                        else
                        {
                            lblMessage.Text = "Asset with Asset ID already exists.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        }
                    }
                    else
                    {
                        string query = "UPDATE [SCADA].[MXLocations_Assets] SET MXLocation = @MXLocation, SiteName= @SiteName, Hierarchical_SiteName= @Hierarchical_SiteName, Asset_ID= @Asset_ID, Asset_Tag= @Asset_Tag," +
                            " Asset_ID_Description= @Asset_ID_Description, updateDate= @updateDate,IsActive=@IsActive,IsMainPump=@IsMainPump,TagName=@TagName WHERE Hierarchical_SiteName = @Hierarchical_SiteName";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@MXLocation", MxLocation);
                                cmd.Parameters.AddWithValue("@SiteName", SiteName);
                                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", Heirarchical);
                                cmd.Parameters.AddWithValue("@Asset_ID", AssetId);
                                cmd.Parameters.AddWithValue("@Asset_Tag", AssetTag);
                                cmd.Parameters.AddWithValue("@Asset_ID_Description", AssetDesc);
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);
                                cmd.Parameters.AddWithValue("@IsMainPump", chkMainPump.Checked);
                                cmd.Parameters.AddWithValue("@TagName", TagName);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        lblMessage.Text = "Asset updated successfully.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                    }
                }
            }

            //catch (SqlException sqlExp)
            //{
            //    if (sqlExp.Number == 2627)
            //    {
            //        string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
            //        log.Error(error, ex);
            //        lblMessage.Text = ErrorMessage;
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            //    }
            //}
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        private void GetSetData(string SiteName)
        {
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "SELECT [SiteName],[Hierarchical_SiteName],[Asset_ID],[Asset_Tag],[Asset_ID_Description],[MXLocation],[fileLoadDate]" +
                    ",[insertedDate],[updateDate],[IsActive],[IsMainPump],[TagName] FROM [ODW].[SCADA].[MXLocations_Assets] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", SiteName);
                cmd.CommandType = CommandType.Text;
                // Create a DataAdapter to run the command and fill the DataTable
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string MXLocation = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    MXLocation = row["MXLocation"].ToString();
                    lblSiteName.Text = row["SiteName"].ToString();
                    txtSiteName.Text = row["Hierarchical_SiteName"].ToString();
                    txtAssetId.Text = row["Asset_ID"].ToString();
                    txtAssetTag.Text = row["Asset_Tag"].ToString();
                    txtTagName.Text = row["TagName"].ToString();
                    txtAssetDesc.Text = row["Asset_ID_Description"].ToString();
                    chkActive.Checked = Convert.ToBoolean(row["IsActive"]);
                    if (!row.IsNull("IsMainPump")) { chkMainPump.Checked = Convert.ToBoolean(row["IsMainPump"]);}

                    cmd = new SqlCommand("Select Status,EXT_FRAMEZONE FROM ODW.EAMS.DimLocation where LOCATION = @MXLocation", conn);
                    cmd.Parameters.AddWithValue("@MXLocation", MXLocation);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        txtAssetStatus.Text = row["Status"].ToString();
                        txtFramework.Text = row["EXT_FRAMEZONE"].ToString();
                    }

                    cmd = new SqlCommand("SELECT * FROM [ODW].[SCADA].[MXLocations] Where MXLocation = " + MXLocation, conn);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        string siteName = row["Hierarchical_SiteName"].ToString();
                        string location = row["MXLocation"].ToString();
                        string SiteNameLocation = siteName + " (" + location + ")";
                        ddMXLocation.SelectedItem.Text = SiteNameLocation;
                    }

                    cmd = new SqlCommand("Select [Hierarchical_SiteName] from [SCADA].[MXLocations_Assets] Where MXLocation = " + MXLocation + "  ORDER BY Hierarchical_SiteName", conn);
                    cmd.CommandType = CommandType.Text;
                    // Create a DataAdapter to run the command and fill the DataTable
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);

                    ddAssets.DataSource = dt;
                    ddAssets.DataTextField = "Hierarchical_SiteName";
                    ddAssets.DataValueField = "Hierarchical_SiteName";
                    ddAssets.DataBind();

                    ddAssets.Items.Insert(0, new ListItem("- Select Asset -", "-1"));
                }
                btnSubmit.Text = "Update Asset";
                Label2.Text = "Edit Asset";
                lblFrmwrk.Visible = true;
                txtFramework.Visible = true;
                lblAssetStatus.Visible = true;
                txtAssetStatus.Visible = true;


            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }


        }
        protected void OnMXLocationIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string MxLocation = ddMXLocation.SelectedItem.Text.Split('(', ')')[1].Trim();
                if (string.IsNullOrEmpty(MxLocation))
                {
                    btnSubmit.Enabled = false;
                    return;
                }
                btnSubmit.Enabled = true;
                string SiteName = ddMXLocation.SelectedValue;
                SqlConnection con = new SqlConnection(constr);
                // Assets DropDown
                string com = "Select [Hierarchical_SiteName] from [SCADA].[MXLocations_Assets] Where MXLocation = (Select MXLocation FROM SCADA.MXLocations WHERE Hierarchical_SiteName = '" + SiteName + "')  ORDER BY Hierarchical_SiteName";
                SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                ddAssets.DataSource = dt;
                ddAssets.DataTextField = "Hierarchical_SiteName";
                ddAssets.DataValueField = "Hierarchical_SiteName";
                ddAssets.DataBind();

                ddAssets.Items.Insert(0, new ListItem("- Select Location -", "-1"));

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }


        }
        protected void onAssetsIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SCADA].[MXLocations_Assets] WHERE ([Hierarchical_SiteName] = @Hierarchical_SiteName)", conn);
                cmd.Parameters.AddWithValue("@Hierarchical_SiteName", ddAssets.SelectedValue);
                cmd.CommandType = CommandType.Text;
                //Create a DataAdapter to run the command and fill the DataTable
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow row = dt.Rows[0];
                //ddMXLocation.Text = row["MXLocation"].ToString();
                lblSiteName.Text = row["SiteName"].ToString();
                txtSiteName.Text = row["Hierarchical_SiteName"].ToString();
                txtAssetId.Text = row["Asset_ID"].ToString();
                txtAssetTag.Text = row["Asset_Tag"].ToString();
                txtTagName.Text = row["TagName"].ToString();
                txtAssetDesc.Text = row["Asset_ID_Description"].ToString();
                chkActive.Checked = Convert.ToBoolean(row["IsActive"]);
                if (!row.IsNull("IsMainPump")) { chkMainPump.Checked = Convert.ToBoolean(row["IsMainPump"]); }

                cmd = new SqlCommand("Select EXT_FRAMEZONE, Status FROM ODW.EAMS.DimLocation where LOCATION = @MXLocation", conn);
                cmd.Parameters.AddWithValue("@MXLocation", row["MXLocation"].ToString());
                cmd.CommandType = CommandType.Text;
                // Create a DataAdapter to run the command and fill the DataTable
                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    txtAssetStatus.Text = row["Status"].ToString();
                    txtFramework.Text = row["EXT_FRAMEZONE"].ToString();
                }

                conn.Close();

                txtSiteName.Attributes.Add("readonly", "readonly");
                Label2.Text = "Edit " + txtSiteName.Text;

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }


        }
        private bool checkIfRecordExists(string AssetId)
        {
            bool check = false;
            try
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [SCADA].[MXLocations_Assets] WHERE ([Asset_ID] = @AssetId)", conn);
                check_User_Name.Parameters.AddWithValue("@AssetId", AssetId);
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
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

            return check;


        }
        private void prepareDropDowns()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                // MX Location DropDown
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM [SCADA].[MXLocations]", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    ddMXLocation.Items.Clear();
                    ddMXLocation.Items.Add(new ListItem("- Select Location -", "-1"));
                    ddMXLocation.Items.Add(new ListItem("Not Available", "Not Available"));
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string siteName = reader["Hierarchical_SiteName"].ToString();
                            string location = reader["MXLocation"].ToString();
                            string SiteNameLocation = siteName + " (" + location + ")";

                            ddMXLocation.Items.Add(new ListItem(SiteNameLocation, siteName));
                        }
                    }
                }
                con.Close();

                // Asset DropDown
                if (!string.IsNullOrEmpty(Request.QueryString["SiteName"]))
                {
                    string com = "Select [Hierarchical_SiteName] from [SCADA].[MXLocations_Assets] ORDER BY Hierarchical_SiteName";
                    SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);

                    ddAssets.DataSource = dt;
                    ddAssets.DataTextField = "Hierarchical_SiteName";
                    ddAssets.DataValueField = "Hierarchical_SiteName";
                    ddAssets.DataBind();

                    ddAssets.Items.Insert(0, new ListItem("- Select Location -", "-1"));
                }

            }
            catch (Exception ex)
            {
                string error = String.Format("{0} Exception\nService \nMessage: {0}\nInner Exception: {1}", typeof(AddEditMXLocationAsset).Name, ex.Message, ex.InnerException);
                log.Error(error, ex);
                lblMessage.Text = ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }

        }
    }
}