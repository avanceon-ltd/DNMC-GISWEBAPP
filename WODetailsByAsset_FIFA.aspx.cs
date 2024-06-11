using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class WODetailsByAsset_FIFA : System.Web.UI.Page
    {
        private string siteName;
        private string assetType;
        private int WONum;
        private string status = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (drpDwnStatus.Text.Contains("OPEN"))
            {
                status = "'WAPPR', 'APPR', 'RETURNED', 'RTNSCHED','DISPATCH', 'RTNCRWLD'";
            }
            else if (drpDwnStatus.Text.Contains("CLOSE"))
            {
                status = "'WRKCOMP', 'SRVPARRST', 'THDPTY', 'HZMITG', 'QC', 'AUDIT','CLOSE', 'COMP'";
            }
            else if (drpDwnStatus.Text.Contains("INPRG"))
            {
                status = "'INPRG'";
            }

            if (!String.IsNullOrEmpty(Request.QueryString["hSiteName"]) && !String.IsNullOrEmpty(Request.QueryString["AssetType"]) && String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                siteName = Request.QueryString["hSiteName"];
                assetType = Request.QueryString["AssetType"];
                BindGridData(siteName, assetType, 1);
            }
            else if (String.IsNullOrEmpty(Request.QueryString["hSiteName"]) && String.IsNullOrEmpty(Request.QueryString["AssetType"]) && !String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                WONum = Convert.ToInt32(Request.QueryString["ID"]);
                BindGridDataWo(WONum, 1);
            }
        }
        void BindGridData(string siteName, string assetType, int PageIndex)
        {
            string[] commands = new string[2];
            switch (assetType)
            {
                case "Manhole":
                    assetType = "[Manholes]";
                    break;
                case "Chambers":
                    assetType = "[Chambers]";
                    break;
                case "Asset":
                    assetType = "[MXLocations_Assets]";
                    break;
                default:
                    assetType = "[MXLocations_Assets]";
                    break;
            }
            
            if (drpDwnWType.Text.Contains("ALL"))
            {
                if (drpDwnStatus.Text.Contains("ALL"))
                {

                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName  = '" + siteName + "') ");
                    }
                    else
                    {
                        commands = CreateQueryWo(" ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName  = '" +
                            siteName + "') AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" STATUS IN ("+ status +") AND ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName  = '" + siteName + "') ");
                    }
                    else
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName  = '" +
                            siteName + "') AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }

                }
            }
            else
            {
                if (drpDwnStatus.Text.Contains("ALL"))
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName = '" + siteName + "') ");
                    }
                    else
                    {
                        commands = CreateQueryWo(" WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName = '" + siteName + "')  AND " +
                            ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName = '" + siteName + "') ");
                    }
                    else
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM = (SELECT TOP(1) Asset_ID FROM [SCADA]." + assetType + " WHERE Hierarchical_SiteName = '" + siteName + "')  AND " +
                            ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                var sql = "Select * from (" + commands[0] + ") a where ROWNUMBER BETWEEN(" + PageIndex + " -1) * 10 + 1 AND(((" + PageIndex + " -1) * 10 + 1) + 10) - 1";
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();

                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    gvWO.DataSource = ds.Tables[0];
                    gvWO.DataBind();
                }
                using (SqlCommand cmd2 = new SqlCommand(commands[1], con))
                {
                    cmd2.CommandType = CommandType.Text;
                    Int32 recordCount = (Int32)cmd2.ExecuteScalar();
                    this.PopulatePager(recordCount, PageIndex);
                    lblRecordCount.Text = recordCount.ToString();
                }

                con.Close();
            }
        }
        protected void gvWO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWO.PageIndex = e.NewPageIndex;

            if (WONum > 0)
            {
                BindGridDataWo(WONum, e.NewPageIndex + 1);
            }
            else
            {
                BindGridData(siteName, assetType, e.NewPageIndex + 1);
            }
        }
        protected void gvWO_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)((decimal)recordCount / decimal.Parse("10"));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                pages.Add(new ListItem("First", "1", currentPage > 1));
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }
        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);

            if (WONum > 0)
            {
                BindGridDataWo(WONum, pageIndex);
            }
            else
            {
                BindGridData(siteName, assetType, pageIndex);
            }

        }
        void BindGridDataWo(int WONum, int PageIndex)
        {
            string[] commands = new string[2];

            if (drpDwnWType.Text.Contains("ALL"))
            {
                if (drpDwnStatus.Text.Contains("ALL"))
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" ASSETNUM = '" + WONum + "'");
                    }
                    else
                    {
                        commands = CreateQueryWo(" ASSETNUM = '" + WONum + "' AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND ASSETNUM = '" + WONum + "'");
                    }
                    else
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND ASSETNUM = '" + WONum + "' AND " + ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
            }
            else
            {
                if (drpDwnStatus.Text.Contains("ALL"))
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM =  '" + WONum + "'");
                    }
                    else
                    {
                        commands = CreateQueryWo(" WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM = '" + WONum + "'  AND " +
                            ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtField1.Text))
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM =  '" + WONum + "'");
                    }
                    else
                    {
                        commands = CreateQueryWo(" STATUS IN (" + status + ") AND WORKTYPE='" + drpDwnWType.SelectedValue.Trim() +
                            "' AND ASSETNUM = '" + WONum + "'  AND " +
                            ddlField1.SelectedValue + " like '%" + txtField1.Text + "%' ");
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString))
            {
                var sql = commands[0] + ") As tbl ORDER BY CASE WHEN tbl.ACTFINISH IS NULL THEN [StatusOrder] ELSE tbl.ACTFINISH END  DESC " +
                    "SELECT* FROM #TEMP ORDER BY CASE WHEN ACTFINISH IS NULL THEN [StatusOrder] ELSE ACTFINISH END DESC OFFSET " + (PageIndex - 1) * 10 + " ROWS FETCH NEXT 10 ROW ONLY; ";
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();

                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    gvWO.DataSource = ds.Tables[0];
                    gvWO.DataBind();
                }

                using (SqlCommand cmd2 = new SqlCommand(commands[1], con))
                {
                    cmd2.CommandType = CommandType.Text;
                    Int32 recordCount = (Int32)cmd2.ExecuteScalar();
                    this.PopulatePager(recordCount, PageIndex);
                    lblRecordCount.Text = recordCount.ToString();
                }

                con.Close();
            }



        }
        string[] CreateQueryWo(string addWhereClause)
        {
            string[] queries = new string[2];
            queries[0] = @"DROP TABLE IF EXISTS #TEMP SELECT * INTO #TEMP FROM ( SELECT CASE WHEN STATUS = 'WAPPR' THEN 0 WHEN STATUS = 'APPR' THEN 1 WHEN STATUS = 'RETURNED' THEN 2 WHEN STATUS = 'RTNSCHED' THEN 3
                           WHEN STATUS = 'DISPATCH' THEN 4 WHEN STATUS = 'RTNCRWLD' THEN 5 WHEN STATUS = 'INPRG'  THEN 6 WHEN STATUS = 'WRKCOMP' THEN 7 WHEN STATUS = 'SRVPARRST' THEN 8 WHEN STATUS = 'THDPTY' THEN 9
                           WHEN STATUS = 'HZMITG' THEN 10 WHEN STATUS = 'QC'THEN 11 WHEN STATUS = 'AUDIT' THEN 12 WHEN STATUS = 'CLOSE' THEN 13 WHEN STATUS = 'COMP' THEN 14 ELSE 15 END AS [StatusOrder],
                           ROW_NUMBER() OVER (ORDER BY CHANGEDATE DESC) ROWNUMBER,wo.WONUM,wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,ACTSTART,ACTFINISH,wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTDATE,wo.CHANGEDATE
                           FROM [EAMS].[FactWorkOrder] wo WHERE  " + addWhereClause;
            queries[1] = @"SELECT Count(*) AS TotalRows FROM [EAMS].[FactWorkOrder] wo
                        WHERE " + addWhereClause;

            return queries;
        }
        //string[] CreateQuery(string addWhereClause)
        //{
        //    string[] queries = new string[2];
        //    queries[0] = @"SELECT ROW_NUMBER() OVER (ORDER BY CHANGEDATE DESC) ROWNUMBER,wo.WONUM,wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,ACTSTART,ACTFINISH,wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTDATE,,wo.CHANGEDATE
        //                 FROM [EAMS].[FactWorkOrder] wo WHERE " + addWhereClause;

        //    queries[1] = @"SELECT Count(*) AS TotalRows FROM [EAMS].[FactWorkOrder] wo
        //                WHERE " + addWhereClause;

        //    return queries;
        //}
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}