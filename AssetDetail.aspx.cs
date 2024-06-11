using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class AssetDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                SqlDataSource1.DataBind();
                //Repeater1.DataBind();
                SqlDataSourceAssetSpecification.SelectCommand = @"select a.ATTDESC,a.AssetValue from (SELECT ATTDESC,
                                                                    CASE WHEN tbl.Dtype = 'NUMERIC' THEN cast(cast(NUMVALUE as decimal(15,5)) as nvarchar(50))+ ' '+ISNULL(MEASUREUNITID,'') ELSE ALNVALUE + ' '+ ISNULL(MEASUREUNITID,'') END AS AssetValue,DESCRIPTION
                                                                    FROM (SELECT (SELECT ab.DESCRIPTION FROM eams.AssetAttribute ab WHERE ab.ASSETATTRID = asp.ASSETATTRID) ATTDESC,
				                                                                    (SELECT ab.DATATYPE FROM eams.AssetAttribute ab WHERE ab.ASSETATTRID = asp.ASSETATTRID) Dtype,
				                                                                    (SELECT ab.MEASUREUNITID FROM eams.AssetAttribute ab WHERE ab.ASSETATTRID = asp.ASSETATTRID) MEASUREUNITID 
		                                                                    ,* FROM eams.AssetSpecification asp WHERE ASSETNUM = '" + Convert.ToString(Request.QueryString["ID"]) + "' )  AS tbl ) a where a.AssetValue is not null order by ATTDESC";
                gvAssetSpecification.DataBind();

                SqlDataSourceChambersAsset.SelectCommand = @"SELECT ASSETNUM,DESCRIPTION,(SELECT LOCATION FROM EAMS.DimLocation where LOCATION_KEY  = a.LOCATION_KEY)as  LocationID,(SELECT DESCRIPTION from EAMS.DimLocation WHERE LOCATION_KEY  = a.LOCATION_KEY) as  LocationDescription from EAMS.DimAsset a WHERE PARENT =  '" + Convert.ToString(Request.QueryString["ID"]) + "' ORDER BY DESCRIPTION";
                gvChambersAsset.DataBind();
            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 130;  //or more time....
        }

        protected void gvChambersAsset_DataBound(object sender, EventArgs e)
        {
            if (gvChambersAsset.Rows.Count > 0)
            {
                SqlDataSource2.SelectCommand = @"SELECT COUNT(*) AS TotalRows FROM EAMS.DimAsset a WHERE PARENT = '" + Convert.ToString(Request.QueryString["ID"]) + "'";
                DataList5.DataBind();
                DataList5.Visible = true;
            }
        }
    }
}