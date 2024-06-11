using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class SRDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {

                if (Request.QueryString["ID"].ToLower().Contains("qu"))
                {
                    lblMessage.Text = "ERROR! EAMS SR Number is QUEUED. Contact ISD to resolve this issue.";
                   

                }
                else
                {
                  SqlDataSource1.SelectParameters["ID"].DefaultValue = Convert.ToString(Request.QueryString["ID"]);
                    SqlDataSource1.SelectCommand = @"SELECT 

        [DESCRIPTION]
      ,[STATUS]
      ,[EXT_DEFECTTYPE]
        ,[AFFECTEDPERSON]
      ,[REPORTEDBY]
        ,[AFFECTEDPHONE]
      ,[CHANGEBY]
      ,[CHANGEDATE]
      ,[CLASS]
      
      ,[EXTERNALRECID]
      ,[EXT_CRMSRID]
      ,[EXT_DEPARTMENT]
      ,[ORGID]
      ,[ORIGRECORDCLASS]
      ,[ORIGRECORDID]
      ,[ORIGRECORGID]
      ,[ORIGRECSITEID]
      ,[PLUSSFEATURECLASS]
      ,[REPORTEDPHONE]
      ,[SITEID]
      ,[TICKETID]
      ,[TICKETUID]
      ,[URGENCY]
      ,[ADDRESSLINE2]
      ,[ADDRESSLINE3]
      ,[COUNTY]
      ,[DESCRIPTION_TK]
      ,[LATITUDEY]
      ,[LONGITUDEX]
      ,[ORGID_TK]
      ,[REFERENCEPOINT]
      ,[REGIONDISTRICT]
      ,[SADDRESSCODE]
      ,[SITEID_TK]
      ,[STATEPROVINCE]
      ,[STREETADDRESS]
      ,[TRANSID]
      ,[TRANSSEQ]
      ,[DESCRIPTION_LONGDESCRIPTION]
      ,[ACTUALFINISH]
      ,[ACTUALSTART]
      ,[REPORTDATE]
      ,[STATUSDATE]
      ,[TARGETFINISH]
      ,[TARGETSTART]
      ,[ASSETNUM]      
      ,[EXT_SRGROUP]
      ,[EXT_SRMEMBER]
      ,[EXT_SRTYPE]
      ,[LOCATION]
      
      ,[SOURCE]

        FROM[ODW].[EAMS].[FactServiceRequest] where([TICKETID]=" + Convert.ToString(Request.QueryString["ID"]) +  ")";
                   SqlDataSource1.DataBind();
               
                }

             
                if (!String.IsNullOrEmpty(Request.QueryString["CRMSID"]))
                {
                    lnkbtnback.Visible = true;
                    lnkbtnback.PostBackUrl = "CRMSComplaintDetail.aspx?ID=" + Convert.ToString(Request.QueryString["CRMSID"]);
                //    Session["CRMSID"] = null;
                }

            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 130;  //or more time....
        }


    }
}