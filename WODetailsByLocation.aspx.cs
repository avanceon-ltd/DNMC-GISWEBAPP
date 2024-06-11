using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class WODetailsByLocation : System.Web.UI.Page
    {
        public String Status
        {
            get
            {
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    return Request.QueryString["status"].ToLower();
                }

                return "";
            }
            set { }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["hSiteName"]))
            {
                string siteName = Request.QueryString["hSiteName"];
                
                string[] commands = new string[2];
                
               

                if (drpDwnWType.Text.Contains(","))
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        if (ddlStatus.SelectedValue == "All")
                        {
                            commands = CreateQuery(" AND WORKTYPE IN ('FR','CM') AND  LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName = '" +
                            siteName + "') ");
                        }
                        else
                        {
                            commands = CreateQuery(" AND WORKTYPE IN ('FR','CM') AND  LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName = '" +
                            siteName + "') AND STATUS  like '%" + (ddlStatus.SelectedValue != "All" ? ddlStatus.SelectedValue : this.Status) + "%' ");
                        }
                            
                    }
                    else
                    {
                        if (ddlStatus.SelectedValue == "All")
                        {
                            commands = CreateQuery(" AND WORKTYPE IN ('FR','CM') AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName = '" +
                            siteName + "') AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                        }
                        else
                        {
                            commands = CreateQuery(" AND WORKTYPE IN ('FR','CM') AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName = '" +
                            siteName + "') AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%'  AND STATUS  like '%" + (ddlStatus.SelectedValue != "All" ? ddlStatus.SelectedValue : this.Status) + "%' ");
                        }
                        
                    }
                }
                else if (drpDwnWType.Text.Contains("ALL"))

                {
                    if (string.IsNullOrEmpty(txtField1.Text) )
                    {
                        if (!String.IsNullOrEmpty(this.Status))
                        {
                            if (ddlStatus.SelectedValue == "All")
                            {
                                commands = CreateQuery(" AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" + siteName + "') ");
                            }
                            else
                            {
                                commands = CreateQuery(" AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" +
                            siteName + "') AND STATUS  like '%" + (ddlStatus.SelectedValue != "All" && ddlStatus.SelectedValue != "" ? ddlStatus.SelectedValue : this.Status) + "%' ");
                            }
                            
                        }else if (ddlStatus.SelectedValue != "All" && ddlStatus.SelectedValue != "")
                        {
                            commands = CreateQuery(" AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" +
                            siteName + "') AND STATUS  like '%" + ddlStatus.SelectedValue + "%' ");
                        }
                        else
                        {
                            commands = CreateQuery(" AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" + siteName + "') ");
                        }
                    }
                    else
                    {
                        if (ddlStatus.SelectedValue == "All" && ddlStatus.SelectedValue != "")
                        {
                            commands = CreateQuery(" AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" +
                                siteName + "') AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%'");
                        }
                        else
                        {
                            commands = CreateQuery(" AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" +
                                siteName + "') AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' AND STATUS  like '%" + (ddlStatus.SelectedValue != "All" && ddlStatus.SelectedValue != "" ? ddlStatus.SelectedValue : this.Status) + "%'");
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        if (ddlStatus.SelectedValue == "All")
                        {
                            commands = CreateQuery(" AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" + siteName + "')");
                        }
                        else
                        {
                            commands = CreateQuery(" AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" + siteName + "')" +
                            "AND STATUS  like '%" + (ddlStatus.SelectedValue != "All" ? ddlStatus.SelectedValue : this.Status) + "%' ");
                        }
                            
                    }
                    else
                    {
                        if (ddlStatus.SelectedValue == "All")
                        {
                            commands = CreateQuery(" AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                           "' AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" + siteName + "')  AND " +
                           ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                        }
                        else
                        {
                            commands = CreateQuery(" AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                           "' AND LOCATION = (SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName ='" + siteName + "')  AND " +
                           ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' AND STATUS  like '%" + (ddlStatus.SelectedValue != "All" ? ddlStatus.SelectedValue : this.Status) + "%' ");
                        }
                           
                    }
                }

                

                SqlDataSource1.SelectCommand = commands[0];
                SqlDataSource2.SelectCommand = commands[1];

                gvWO.DataBind();
                DataList5.DataBind();

                //DataSourceSelectArguments args = new DataSourceSelectArguments();
                //DataView view = (DataView)SqlDataSource1.Select(args);
                //DataTable dt = view.ToTable();
                //DataTable distinctDT = SelectDistinct(dt, "STATUS");

                
                //foreach (DataRow row in distinctDT.Rows)
                //{
                //    ddlStatus.Items.Add(new ListItem(row["STATUS"].ToString(), row["STATUS"].ToString()));
                //}
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
                {
                    string sql = @"select distinct(wo.STATUS) from EAMS.FactWorkOrder wo WHERE wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC')AND LOCATION = (SELECT TOP(1) MXLocation FROM[SCADA].[MXLocations]
                              WHERE Hierarchical_SiteName = 'ps31') ";
                    con.Open();
                    
                    using (SqlCommand cmd = new SqlCommand(sql,con))
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);
                    }
                    con.Close();
                    con.Dispose();
                }

                

                if (!Page.IsPostBack)
                {
                    ddlStatus.Items.Clear();
                    ddlStatus.Items.Add(new ListItem("All","All"));
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ddlStatus.Items.Add(new ListItem(row["STATUS"].ToString(), row["STATUS"].ToString()));
                    }

                    if (!String.IsNullOrEmpty(this.Status))
                    {
                        ddlStatus.SelectedValue = Request.QueryString["status"].ToUpper();

                    }
                }
                
            }

        }
        
       
        

        public DataTable SelectDistinct(DataTable SourceTable, string FieldName)
        {
            // Create a Datatable – datatype same as FieldName  
            DataTable dt = new DataTable(SourceTable.TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);
            // Loop each row & compare each value with one another  
            // Add it to datatable if the values are mismatch  
            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            return dt;
        }

        private bool ColumnEqual(object A, object B)
        {
            // Compares two values to see if they are equal. Also compares DBNULL.Value.             
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value  
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is BNull.Value  
                return false;
            return (A.Equals(B)); // value type standard comparison  
        }

        string[] CreateQuery(string addWhereClause)
        {
            string[] queries = new string[2];
            queries[0] = @"SELECT wo.WONUM,wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,wo.ASSETNUM,wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTEDBY,wo.REPORTDATE,wo.EXT_ENGINEER,wo.CHANGEDATE
                         FROM [EAMS].[FactWorkOrder] wo WHERE wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC') " + addWhereClause +
                         " ORDER BY  CHANGEDATE DESC";
            queries[1] = @"SELECT Count(*) AS TotalRows FROM [EAMS].[FactWorkOrder] wo
                        WHERE wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC') " + addWhereClause;

            return queries;
        }


        protected void gvWO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWO.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void gvWO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the HyperLink control.
                HyperLink myHyperLink = (e.Row.FindControl("WOAssetHyp") as HyperLink);
                string _WONum = e.Row.Cells[0].Text;
                if (!string.IsNullOrEmpty(_WONum))
                {
                    if (IsAttachments(_WONum))
                    {
                        myHyperLink.NavigateUrl = "javascript:window.open('GetImgAttachByWo.aspx?id=" + _WONum + "','PopupWindow','width=400,height=200'); ";
                        myHyperLink.Text = "Click Here";
                    }
                }
            }
        }

        private bool IsAttachments(string wONUM)
        {
            bool Attachments = false;

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("SELECT [URLNAME] FROM [ODW].[EAMS].[Attachment] where OWNERID IN (SELECT WORKORDERID  FROM [ODW].EAMS.FactWorkOrder  where WONUM='" + wONUM + "') and OWNERTABLE='WorkOrder'", con);
                SqlDataAdapter sdadpr = new SqlDataAdapter();
                sdadpr.SelectCommand = cmd;
                sdadpr.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Attachments = true;
                }
            }
            return Attachments;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

    }
}