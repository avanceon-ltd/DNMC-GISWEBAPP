using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class EccOwnerDetails : System.Web.UI.Page
    {
        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        protected void BindData()
        {
            conn.Open();
            string query = "SELECT ALNDOMAINID,DESCRIPTION,VALUE,ISNULL(owners.IsECC,0) AS  IsEccOwner,ISNULL(owners.SequenceId,'') as Sequence  FROM EAMS.vwAlnDomain alnval";
            query += " LEFT JOIN Eams.WOOwners owners On owners.DomainID = alnval.ALNDOMAINID";
            query += " WHERE alnval.DOMAINID = 'THRDP' AND alnval.EXT_DEPARTMENT = 'DRAINAGE' ORDER BY Sequence";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEccOwners.DataSource = ds;
                gvEccOwners.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvEccOwners.DataSource = ds;
                gvEccOwners.DataBind();
                int columncount = gvEccOwners.Rows[0].Cells.Count;
                gvEccOwners.Rows[0].Cells.Clear();
                gvEccOwners.Rows[0].Cells.Add(new TableCell());
                gvEccOwners.Rows[0].Cells[0].ColumnSpan = columncount;
                gvEccOwners.Rows[0].Cells[0].Text = "No Records Found";
            }
        }

        protected void gvEccOwners_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEccOwners.EditIndex = e.NewEditIndex;
            BindData();
        }
        protected void gvEccOwners_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int domainId =Convert.ToInt32(gvEccOwners.DataKeys[e.RowIndex].Value);
            GridViewRow row = (GridViewRow)gvEccOwners.Rows[e.RowIndex];
            TextBox OwnerVal = (TextBox)row.Cells[1].Controls[0];
            TextBox OwnerDescription = (TextBox)row.Cells[2].Controls[0];
            CheckBox IsEcc = (CheckBox)row.Cells[3].Controls[0];
            TextBox Sequence = (TextBox)row.Cells[4].Controls[0];
            gvEccOwners.EditIndex = -1;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[EAMS].[Insert_Update_EccOwners]";
                    cmd.Parameters.AddWithValue("@DomainId", domainId);
                    cmd.Parameters.AddWithValue("@OwnerValue", OwnerVal.Text.Trim());
                    cmd.Parameters.AddWithValue("@OwnerDescription", OwnerDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@IsECC", Convert.ToInt32(IsEcc.Checked));
                    cmd.Parameters.AddWithValue("@Sequence", Sequence.Text.Trim());
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
                BindData();
        }
        protected void gvEccOwners_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEccOwners.PageIndex = e.NewPageIndex;
            BindData();
        }
        protected void gvEccOwners_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEccOwners.EditIndex = -1;
            BindData();
        }

        protected void gvEccOwners_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Enabled = false;
                e.Row.Cells[1].Enabled = false;
                e.Row.Cells[2].Enabled = false;
                string item = e.Row.Cells[0].Text;
                int accessLevel = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["AccessLevel"]))
                {
                    accessLevel = Convert.ToInt32(Request.QueryString["AccessLevel"]);
                }

                if (accessLevel >= 7777)
                {
                    e.Row.Cells[5].Visible = true;
                }
                else
                {
                    e.Row.Cells[5].Visible = false;
                }

            }
        }
    }
}