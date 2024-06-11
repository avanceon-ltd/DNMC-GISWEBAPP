using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class ChambersAssetInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string AssetId = Request.QueryString["AssetId"];
            string Description = Request.QueryString["Description"];
            if (!String.IsNullOrEmpty(AssetId))
            {
                gvChambersAsset.Caption = "Sub-Assemblies: " + AssetId + " ( " + Description + " )";
                gvChambersAsset.CaptionAlign = TableCaptionAlign.Left;
                SqlDataSourceChambersAsset.SelectCommand = @"SELECT ASSETNUM,DESCRIPTION,(SELECT LOCATION FROM EAMS.DimLocation where LOCATION_KEY  = a.LOCATION_KEY)as  LocationID,(SELECT DESCRIPTION from EAMS.DimLocation WHERE LOCATION_KEY  = a.LOCATION_KEY) as  LocationDescription from EAMS.DimAsset a WHERE PARENT =  '" + Convert.ToString(Request.QueryString["AssetId"]) + "'";
                SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows FROM EAMS.DimAsset a WHERE PARENT = '" + Convert.ToString(Request.QueryString["AssetId"]) + "'";
                gvChambersAsset.DataBind();
                DataList5.DataBind();
            }
        }
    }
}