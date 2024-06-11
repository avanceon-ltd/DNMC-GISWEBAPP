using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class CMD : System.Web.UI.Page
    {

        List<string> tansids;
        string crmsid, eamssr, eamswo, stringtransid, _wostatus, queryresult, countresult = string.Empty;
        bool israinwater, isnonrainwater, allcompaints = false;



        protected void Page_Load(object sender, EventArgs e)
        {


            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !IsPostBack)
            {
                lbl_crmsid.Text = Request.QueryString["id"];
                crmsid = lbl_crmsid.Text.Trim();
            }

            if (IsPostBack)
            {
                countresult = string.Empty;
                queryresult = string.Empty;

                crmsid = string.Empty;
                eamssr = string.Empty;
                eamswo = string.Empty;
                stringtransid = string.Empty;
                _wostatus = string.Empty;
                reflblEAMS_SR_odw_vw.Text = "";
                reflblEAMS_SR_gis_srvce.Text = "";
                reflblEAMS_wo_odw_vw.Text = "";
                reflblEAMS_wo_gis_srvce.Text = "";



                lblCRMS_isd_stgQR.Text = string.Empty;
                lblCRMS_isd_stgQ.Text = string.Empty;

                lblCRMS_odw_stgQR.Text = string.Empty;
                lblCRMS_odw_stgQ.Text = string.Empty;

                lblwoall.Text = string.Empty;
                ref_lblwoall.Text = string.Empty;

                if (!String.IsNullOrEmpty(txtcomplaintno.Text))
                {
                    lbl_crmsid.Text = txtcomplaintno.Text.Trim();
                    crmsid = lbl_crmsid.Text;
                }
            }

            //Complaint Available in ISD staging DB
            lblCRMS_isd_stg.Text = IsCheckData_Count("SELECT COUNT(*) FROM [DNMC].[dbo].[DrainageComplaints] where DRG_Comp_Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["CRMS_STG_ConnectionString"].ConnectionString, 0);
            lblCRMS_isd_stgQR.Text = "Count=" + countresult;
            lblCRMS_isd_stgQ.Text = queryresult;

            //Complaint Available in ODW Staging DB
            lblCRMS_odw_stg.Text = IsCheckData_Count("SELECT count(*) FROM [ODW-STG].[CRMS].[STG_DrainageComplaints] where DRG_Comp_Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODW_STG_ConnectionString"].ConnectionString, 0);
            lblCRMS_odw_stgQR.Text = "Count=" + countresult;
            lblCRMS_odw_stgQ.Text = queryresult;


            //Complaint Available in ODW GIS history Complaint Table
            lblCRMS_odw_gis_tblhisty.Text = IsCheckData_Count("SELECT count(*) from [ODW].[CRMS].[DrainageComplaints] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            lblCRMS_odw_gis_tblhistyQ.Text = queryresult;
            eamssr = OUTData("SELECT EAMS_SR_Number from [ODW].[CRMS].[DrainageComplaints] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString);
            lblCRMS_odw_gis_tblhistyQR.Text = "Count=" + countresult + "  EAMSSR:" + eamssr;

            //Complaint Available in ODW GIS Complaint Table
            lblCRMS_odw_gis_tbl.Text = IsCheckData_Count("SELECT count(*) from [ODW].[GIS].[DrainageComplaints] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            lblCRMS_odw_gis_tblQ.Text = queryresult;
            eamssr = OUTData("SELECT EAMS_SR_Number from [ODW].[CRMS].[DrainageComplaints] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString);
            lblCRMS_odw_gis_tblQR.Text = "Count=" + countresult + "  EAMSSR:" + eamssr;

            //Complaint Available in ODW GIS Complaint Views ECC
            lblCRMS_odw_gis_vw.Text = IsCheckData_Count("SELECT count(*) from [ODW].[GIS].[vwComplaintsExternalFlooding] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);//0 mean pass fail
            lblCRMS_odw_gis_vwQR.Text = "Count=" + countresult;
            lblCRMS_odw_gis_vwQ.Text = queryresult;



            //Complaint Available in ODW GIS Complaint Views rainwater
            lblCRMS_odw_gis_rw.Text = IsCheckData_Count("SELECT count(*) from [ODW].[GIS].[vwBAUComplaintsRainWater] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 1); //1 mean yes/no 
            lblCRMS_odw_gis_rwQR.Text = "Count=" + countresult;
            lblCRMS_odw_gis_rwQ.Text = queryresult;



            //Complaint Available in ODW GIS Complaint Views non rainwater
            lblCRMS_odw_gis_noNrw.Text = IsCheckData_Count("SELECT count(*) from [ODW].[GIS].[vwBAUComplaintsNonRainWater] where Service_Request_Number='" + crmsid + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 1);
            lblCRMS_odw_gis_noNrwQR.Text = "Count=" + countresult;
            lblCRMS_odw_gis_noNrwQ.Text = queryresult;


            //Complaint Available in ArcGIS Service
            lblCRMS_odw_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_ComplaintsAll"] + crmsid + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblCRMS_odw_gis_srvce.Text.ToLower().Contains("p"))
                allcompaints = true;
            else
                allcompaints = false;

            lblCRMS_odw_gis_srvceQR.Text = "Count=" + countresult;
            lblCRMS_odw_gis_srvceQ.Text = queryresult;


            lblCRMS_odw_gis_srvce_nR.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUComplaintsNonRainWater"] + crmsid + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblCRMS_odw_gis_srvce_nR.Text.ToLower().Contains("p"))
            {
                lblCRMS_odw_gis_srvce_nR.Text = "YES";
                isnonrainwater = true;
            }
            else
            {
                lblCRMS_odw_gis_srvce_nR.Text = "NO";
                isnonrainwater = false;
            }
            lblCRMS_odw_gis_srvce_nRQR.Text = "Count=" + countresult;
            lblCRMS_odw_gis_srvce_nRQ.Text = queryresult;


            lblCRMS_odw_gis_srvce_R.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUComplaintsRainWater"] + crmsid + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblCRMS_odw_gis_srvce_R.Text.ToLower().Contains("p"))
            {
                lblCRMS_odw_gis_srvce_R.Text = "YES";
                israinwater = true;
            }
            else
            {
                lblCRMS_odw_gis_srvce_R.Text = "NO";
                israinwater = false;
            }
            lblCRMS_odw_gis_srvce_RQR.Text = "Count=" + countresult;
            lblCRMS_odw_gis_srvce_RQ.Text = queryresult;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //   EAMS SR

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //lblEAMS_SR.Text = eamssr;

            //if (!eamssr.ToLower().Contains("q") && !string.IsNullOrEmpty(eamssr))
            //{
            //    //EAMS SR is available in ISD Actual DB
            //    lblEAMS_SR_isd_actdb.Text = CheckDataOracle("sElECT count(*) as total fROM MAXIMO.SR_DSS wHErE  ticketid='" + eamssr + "'", ConfigurationManager.AppSettings["MAXACTUAL"], 1);
            //}
            //else
            //    lblEAMS_SR_isd_actdb.Text = "FAIL";

            //lblEAMS_SR_isd_actdbQR.Text = "Count=" + countresult;
            //lblEAMS_SR_isd_actdbQ.Text = queryresult;

            lblEAMS_SR_isd_actdb.Text = "N/A";
            lblEAMS_SR_isd_actdbQR.Text = "N/A";
            lblEAMS_SR_isd_actdbQ.Text = "N/A";


            //EAMS SR is available in ISD Staging DB
            lblEAMS_SR_isd_stgdb.Text = CheckDataOracle("select ticketid as EAMS_SR,transid from MAXIMO.EXT_DSS_sr where ticketid='" + eamssr + "'", ConfigurationManager.AppSettings["MAXPROD"], 0);

            lblEAMS_SR_isd_stgdbQ.Text = queryresult;
            if (lblEAMS_SR_isd_stgdb.Text.ToLower().Contains("p"))
                lblEAMS_SR_isd_stgdbQR.Text = "Count=1; transIDs: " + sorttransid(tansids);
            else
                lblEAMS_SR_isd_stgdbQR.Text = "Count=0; transIDs: " + sorttransid(tansids);

            //DNMC ODW Successful Read from ISD Staging DB(Flag Check)
            if (tansids.Count > 0)
            {
                stringtransid = sorttransid(tansids);
                lblEAMS_SR_isd_stgFC.Text = CheckDataOracle("select count(*) from MAXIMO.MXOUT_INTER_TRANS where IFACENAME='EXT_DSS_SR' and transid in (" + stringtransid + ") and importMessage = 'Y' and action='Replace' order by transid desc", ConfigurationManager.AppSettings["MAXPROD"], 1);
            }
            else
                lblEAMS_SR_isd_stgFC.Text = "FAIL";

            lblEAMS_SR_isd_stgFCQR.Text = "Count=" + countresult;
            lblEAMS_SR_isd_stgFCQ.Text = queryresult;


            //EAMS SR is available in ODW Staging DB
            lblEAMS_SR_odw_stgdb.Text = IsCheckData_Count("SELECT count(*) FROM [ODW-STG].[EAMS].[STG_EXT_DSS_SR] where TICKETID='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODW_STG_ConnectionString"].ConnectionString, 0);
            lblEAMS_SR_odw_stgdbQR.Text = "Count=" + countresult;
            lblEAMS_SR_odw_stgdbQ.Text = queryresult;

            //EAMS SR is available in DNMC ODW GIS SR Table
            lblEAMS_SR_odw_gisSR.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[FactServiceRequest] where TICKETID='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            lblEAMS_SR_odw_gisSRQR.Text = "Count=" + countresult;
            lblEAMS_SR_odw_gisSRQ.Text = queryresult;


            //EAMS SR is available in DNMC ODW GIS Complaint(Table)
            lblEAMS_SR_odw_tbl.Text = CheckData("select WONUM from CRMS.DrainageComplaints where EAMS_SR_Number='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 2);
            lblEAMS_SR_odw_tblQ.Text = queryresult;

            if (lblEAMS_SR_odw_tbl.Text.ToLower().Contains("p"))
                lblEAMS_SR_odw_tblQR.Text = "Count=1 WoNum:" + countresult;
            else
                lblEAMS_SR_odw_tblQR.Text = "Count=0";



            //EAMS SR is associated with Complaints(Complaints Views)

            if (!string.IsNullOrEmpty(eamssr) && !eamssr.ToLower().Contains("q"))
            {

                if (allcompaints)
                {
                    lblEAMS_SR_odw_vw.Text = IsCheckData_Count("SELECT count(*) FROM [ODW].[GIS].[vwComplaintsExternalFlooding] where EAMS_SR_Number='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                    reflblEAMS_SR_odw_vw.Text = "[ODW].[GIS].[vwComplaintsExternalFlooding] ECC";
                    lblEAMS_SR_odw_vwQR.Text = "Count=" + countresult;
                    lblEAMS_SR_odw_vwQ.Text = queryresult;
                }

                if (isnonrainwater)
                {
                    lblEAMS_SR_odw_vw.Text = IsCheckData_Count("SELECT count(*) FROM [ODW].[GIS].[vwBAUComplaintsNonRainWater] where EAMS_SR_Number='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                    reflblEAMS_SR_odw_vw.Text += "||[ODW].[GIS].[vwBAUComplaintsNonRainWater]";
                    lblEAMS_SR_odw_vwQR.Text += " || " + "Count=" + countresult;
                    lblEAMS_SR_odw_vwQ.Text += " || " + queryresult;
                }
                if (israinwater)
                {
                    lblEAMS_SR_odw_vw.Text = IsCheckData_Count("SELECT count(*) FROM [ODW].[GIS].[vwBAUComplaintsRainWater] where EAMS_SR_Number='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                    reflblEAMS_SR_odw_vw.Text += "||[ODW].[GIS].[vwBAUComplaintsRainWater]";
                    lblEAMS_SR_odw_vwQR.Text += " || " + "Count=" + countresult;
                    lblEAMS_SR_odw_vwQ.Text += " || " + queryresult;
                }
                if (!isnonrainwater && !israinwater)
                {
                    lblEAMS_SR_odw_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwComplaintsExternalFlooding] where EAMS_SR_Number='" + eamssr + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                    reflblEAMS_SR_odw_vw.Text += "||[ODW].[GIS].[vwComplaintsExternalFlooding] -ECC";
                    lblEAMS_SR_odw_vwQR.Text += " || " + "Count=" + countresult;
                    lblEAMS_SR_odw_vwQ.Text += " || " + queryresult;
                }
            }
            else
                lblEAMS_SR_odw_vw.Text = "FAIL";

            //EAMS SR association is available in ArcGIS Service

            if (!string.IsNullOrEmpty(eamssr) && !eamssr.ToLower().Contains("q"))
            {

                lblEAMS_SR_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_ComplaintsAll"] + crmsid + "' and EAMS_SR_Number='" + eamssr + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
                if (lblEAMS_SR_gis_srvce.Text.Trim().ToLower().Contains("p"))
                {
                    reflblEAMS_SR_gis_srvce.Text = "PROD_ComplaintsAll";
                    lblEAMS_SR_gis_srvceQR.Text = "Count=" + countresult;
                    lblEAMS_SR_gis_srvceQ.Text = queryresult;
                }

                if (isnonrainwater)
                {
                    lblEAMS_SR_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUComplaintsNonRainWater"] + crmsid + "' and EAMS_SR_Number='" + eamssr + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
                    reflblEAMS_SR_gis_srvce.Text = reflblEAMS_SR_gis_srvce.Text + "||ComplaintsNonRainWater";
                    lblEAMS_SR_gis_srvceQR.Text += " || " + "Count=" + countresult;
                    lblEAMS_SR_gis_srvceQ.Text += " || " + queryresult;
                }

                if (israinwater)
                {
                    lblEAMS_SR_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUComplaintsRainWater"] + crmsid + "' and EAMS_SR_Number='" + eamssr + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
                    reflblEAMS_SR_gis_srvce.Text = reflblEAMS_SR_gis_srvce.Text + "||ComplaintsRainWater";
                    lblEAMS_SR_gis_srvceQR.Text += " || " + "Count=" + countresult;
                    lblEAMS_SR_gis_srvceQ.Text += " || " + queryresult;
                }


            }
            else
                lblEAMS_SR_gis_srvce.Text = "FAIL";

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //   EAMS WORK ORDER

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




            lblEAMS_wo.Text = eamswo;

            //EAMS WO is available in Actual DB
            //lblEAMS_wo_isd_actdb.Text = CheckDataOracle("select count(*) as total from MAXIMO.workorder_dss where wonum='" + eamswo + "'", ConfigurationManager.AppSettings["MAXACTUAL"], 1);

            //lblEAMS_wo_isd_actdbQR.Text = "Count=" + countresult;
            //lblEAMS_wo_isd_actdbQ.Text = queryresult;

            lblEAMS_wo_isd_actdb.Text = "N/A";
            lblEAMS_wo_isd_actdbQR.Text = "N/A";
            lblEAMS_wo_isd_actdbQ.Text = "N/A";



            //EAMS WO is avaialble in ISD Staging DB
            lblEAMS_wo_isd_stgdb.Text = CheckDataOracle("select transid,status from MAXIMO.EXT_DSS_wo where wonum='" + eamswo + "'", ConfigurationManager.AppSettings["MAXPROD"], 2);
            lblEAMS_wo_isd_stgdbQ.Text = queryresult;
            if (lblEAMS_wo_isd_stgdb.Text.ToLower().Contains("p"))
                lblEAMS_wo_isd_stgdbQR.Text = "Count=1; transIDs: " + sorttransid(tansids);
            else
                lblEAMS_wo_isd_stgdbQR.Text = "Count=0; transIDs: " + sorttransid(tansids);



            //DNMC ODW Successful Read from ISD Staging DB(Flag Check)

            if (tansids.Count > 0)
            {
                stringtransid = sorttransid(tansids);
                lblEAMS_wo_isd_stgdbFC.Text = CheckDataOracle("select count(*) from MAXIMO.MXOUT_INTER_TRANS where IFACENAME='EXT_DSS_WO' and transid in (" + stringtransid + ") and importMessage = 'Y' and action='Replace' order by transid desc", ConfigurationManager.AppSettings["MAXPROD"], 1);
            }
            else
                lblEAMS_wo_isd_stgdbFC.Text = "FAIL";


            lblEAMS_wo_isd_stgdbFCQR.Text = "Count=" + countresult;
            lblEAMS_wo_isd_stgdbFCQ.Text = queryresult;


            //EAMS WO is available in ODW Staging DB 
            lblEAMS_wo_odw_stg.Text = IsCheckData_Count("SELECT count(*) FROM [ODW-STG].[EAMS].[STG_EXT_DSS_WO] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODW_STG_ConnectionString"].ConnectionString, 0);

            lblEAMS_wo_odw_stgQR.Text = "Count=" + countresult;
            lblEAMS_wo_odw_stgQ.Text = queryresult;


            //EAMS WorkOrder is available in DNMC ODW GIS WO Table
            lbl_odw_giswotbl.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[FactWorkOrder] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);

            lbl_odw_giswotblQR.Text = "Count=" + countresult;
            lbl_odw_giswotblQ.Text = queryresult;

            //EAMS WO is available in DNMC ODW GIS Complaint(Table)
            lblEAMS_wo_odw_tbl_gis.Text = CheckData("select WO_STATUS from CRMS.DrainageComplaints where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 3);
            lblEAMS_wo.Text = eamswo + " STATUS: " + _wostatus;

            lblEAMS_wo_odw_tbl_gisQR.Text = countresult;
            lblEAMS_wo_odw_tbl_gisQ.Text = queryresult;

            //EAMS WO is associated with Complaints(Complaints View)

            if (allcompaints)
            {
                lblEAMS_wo_odw_vw.Text = IsCheckData_Count("SELECT count(*) FROM [ODW].[GIS].[vwComplaintsExternalFlooding] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                reflblEAMS_wo_odw_vw.Text = "[ODW].[GIS].[vwComplaintsExternalFlooding] ECC";

                lblEAMS_wo_odw_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_vwQ.Text = queryresult;
            }

            if (isnonrainwater)
            {
                lblEAMS_wo_odw_vw.Text = IsCheckData_Count("SELECT count(*) FROM [ODW].[GIS].[vwBAUComplaintsNonRainWater] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                reflblEAMS_wo_odw_vw.Text += "||[ODW].[GIS].[vwBAUComplaintsNonRainWater]";

                lblEAMS_wo_odw_vwQR.Text += "|| " + "Count =" + countresult;
                lblEAMS_wo_odw_vwQ.Text += "|| " + queryresult;
            }
            if (israinwater)
            {
                lblEAMS_wo_odw_vw.Text = IsCheckData_Count("SELECT count(*) FROM [ODW].[GIS].[vwBAUComplaintsRainWater] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                reflblEAMS_wo_odw_vw.Text += "||[ODW].[GIS].[vwBAUComplaintsRainWater]";

                lblEAMS_wo_odw_vwQR.Text += "|| " + "Count=" + countresult;
                lblEAMS_wo_odw_vwQ.Text += "|| " + queryresult;
            }
            if (!isnonrainwater && !israinwater)
            {
                lblEAMS_wo_odw_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwComplaintsExternalFlooding] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
                reflblEAMS_wo_odw_vw.Text += "||[ODW].[GIS].[vwComplaintsExternalFlooding] -ECC";

                lblEAMS_wo_odw_vwQR.Text += "|| " + "Count=" + countresult;
                lblEAMS_wo_odw_vwQ.Text += "|| " + queryresult;
            }


            //EAMS WO is available in its own view  
            ref_lblEAMS_wo_odw_own_vw.Text = "";
            lblEAMS_wo_odw_own_vw.Text = "";

            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWise] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWise";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }

            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWiseWorkshop] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWiseWorkshop";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }
            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWiseRoads] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWiseRoads";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }
            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWiseIAProjects] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWiseIAProjects";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }
            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWiseCustomerService] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWiseCustomerService";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }
            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWiseBaladiya] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWiseBaladiya";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }
            lblEAMS_wo_odw_own_vw.Text = IsCheckData_Count("SELECT COUNT(*) FROM [ODW].[GIS].[vwWorkOrderOwnerWiseFMP] where WONUM='" + eamswo + "'", ConfigurationManager.ConnectionStrings["ODWConnectionString"].ConnectionString, 0);
            if (lblEAMS_wo_odw_own_vw.Text.Contains("P"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "vwWorkOrderOwnerWiseFMP";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_odw_own_vwQ.Text = queryresult;
            }

            if (lblEAMS_wo_odw_own_vw.Text.Contains("F"))
            {
                ref_lblEAMS_wo_odw_own_vw.Text = "All Views Failed";
                lblEAMS_wo_odw_own_vwQR.Text = "Count=0";
                lblEAMS_wo_odw_own_vwQ.Text = "N/A";
            }

            //EAMS WO is association is available in ArcGIS Service
            lblEAMS_wo_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_ComplaintsAll"] + crmsid + "' and EAMS_SR_Number='" + eamssr + "' and WO_STATUS='" + _wostatus + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblEAMS_wo_gis_srvce.Text.Trim().ToLower().Contains("p"))
            {
                reflblEAMS_wo_gis_srvce.Text = "PROD_ComplaintsAll";

                lblEAMS_wo_gis_srvceQR.Text = "Count=" + countresult;
                lblEAMS_wo_gis_srvceQ.Text = queryresult;
            }

            if (isnonrainwater)
            {
                lblEAMS_wo_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUComplaintsNonRainWater"] + crmsid + "' and EAMS_SR_Number='" + eamssr + "' and WO_STATUS='" + _wostatus + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
                reflblEAMS_wo_gis_srvce.Text = reflblEAMS_wo_gis_srvce.Text + "||ComplaintsNonRainWater";

                lblEAMS_wo_gis_srvceQR.Text += " || " + "Count=" + countresult;
                lblEAMS_wo_gis_srvceQ.Text += " || " + queryresult;

            }

            if (israinwater)
            {
                lblEAMS_wo_gis_srvce.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUComplaintsRainWater"] + crmsid + "' and EAMS_SR_Number='" + eamssr + "' and WO_STATUS='" + _wostatus + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
                reflblEAMS_wo_gis_srvce.Text = reflblEAMS_wo_gis_srvce.Text + "||ComplaintsRainWater";

                lblEAMS_wo_gis_srvceQR.Text += " || " + "Count=" + countresult;
                lblEAMS_wo_gis_srvceQ.Text += " || " + queryresult;

            }





            //EAMS WO is available in its own Views in ArcGIS Service

            if (isnonrainwater)
            {
                lblEAMS_wo_gis_srvce_vw.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUWorkordersNonRainWaterFR"] + eamswo + "' and STATUS='" + _wostatus + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
                reflblEAMS_wo_gis_srvce_vw.Text = "ArcGIS_Service_PROD_BAUWorkordersNonRainWaterFR";

                lblEAMS_wo_gis_srvce_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_gis_srvce_vwQ.Text = queryresult;
            }

            if (israinwater)
            {
                lblEAMS_wo_gis_srvce_vw.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGIS_Service_PROD_BAUWorkordersRainWaterFR"] + eamswo + "' and STATUS='" + _wostatus + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");

                reflblEAMS_wo_gis_srvce_vw.Text = "ArcGIS_Service_PROD_BAUWorkordersRainWaterFR";

                lblEAMS_wo_gis_srvce_vwQR.Text = "Count=" + countresult;
                lblEAMS_wo_gis_srvce_vwQ.Text = queryresult;
            }


            //EAMS WO is available in workorderall ArcGIS Service



            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }
            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["WORKSHOPS"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }
            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["ROADS"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }
            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["IA-PROJECTS"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }
            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["CUSTOMER-SERVICE"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }
            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["BALADIYA"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }
            lblwoall.Text = CheckGisArcService(ConfigurationManager.AppSettings["ArcGisBase"] + ConfigurationManager.AppSettings["FMP-WORK"].Split(',')[1].ToString() + "/query?where=WONUM='" + eamswo + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&having=&returnIdsOnly=false&returnCountOnly=true&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&queryByDistance=&returnExtentOnly=false&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=json");
            if (lblwoall.Text.Trim().ToLower().Contains("p"))
            {
                ref_lblwoall.Text = ConfigurationManager.AppSettings["WO_VW_ALL"].Split(',')[2].ToString();
                lblwoallQR.Text = "Count=" + countresult;
                lblwoallQ.Text = queryresult;
            }

            if (lblwoall.Text.Contains("F"))
            {
                ref_lblwoall.Text = "All Views Failed";
                lblwoallQR.Text = "Count=0";
                lblwoallQ.Text = "N/A";
            }

        }




        private string IsCheckData_Count(string query, string connectionString, int type)
        {
            Int32 ignoreMe;
            string result = "FAIL";

            if (type == 0)
                result = "FAIL";
            else if (type == 1)
                result = "NO";

            countresult = string.Empty;
            queryresult = string.Empty;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            countresult = reader[0].ToString();
                            queryresult = query;



                            bool trypasre = Int32.TryParse(reader[0].ToString(), out ignoreMe);

                            if (trypasre)
                            {

                                if (Convert.ToInt32(reader[0]) > 0)
                                {

                                    if (type == 0)
                                        result = "PASS";
                                    else if (type == 1)
                                        result = "YES";
                                }
                                else
                                {
                                    if (type == 0)
                                        result = "FAIL";
                                    else if (type == 1)
                                        result = "NO";
                                }
                            }
                            else
                            {
                                if (type == 0)
                                    result = "FAIL";
                                else if (type == 1)
                                    result = "NO";
                            }
                        }
                    }
                }
            }
            return result;


        }

        private string CheckData(string query, string connectionString, int type)
        {
            Int32 ignoreMe;
            string resut = string.Empty;
            resut = "FAIL";
            if (type == 0)
                resut = "FAIL";
            else if (type == 1)
                resut = "NO";

            countresult = string.Empty;
            queryresult = string.Empty;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    queryresult = query;
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            bool trypasre = Int32.TryParse(reader[0].ToString(), out ignoreMe);

                            if (trypasre)
                            {

                                if (Convert.ToInt32(reader[0]) > 0)
                                {

                                    if (type == 0)
                                        resut = "PASS";
                                    else if (type == 1)
                                        resut = "YES";
                                    else if (type == 2)
                                    {
                                        resut = "PASS";
                                        eamswo = reader[0].ToString();
                                        countresult = eamswo;
                                    }
                                    else if (type == 3)
                                    {
                                        resut = "PASS";
                                        _wostatus = reader[0].ToString();
                                        countresult = "Count:" + reader[0].ToString() + " WOStatus:" + _wostatus;
                                    }
                                }
                            }
                            else
                            {
                                if (type == 3)
                                {
                                    resut = "PASS";
                                    _wostatus = reader[0].ToString();
                                    countresult = "Count:1;  WOStatus:" + _wostatus;
                                }
                            }

                        }
                    }

                }
            }
            return resut;
        }


        private string OUTData(string query, string connectionString)
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = reader[0].ToString();

                        }
                    }

                }
            }
            return result;
        }



        private string sorttransid(List<string> tansids)
        {
            string temparray = string.Join(",", tansids.Select(x => string.Format("'{0}'", x)));
            return temparray;
        }

        private string CheckDataOracle(string query, string connectionString, int actiontype)
        {

            tansids = new List<string>();
            string resut = "FAIL";
            countresult = string.Empty;
            queryresult = string.Empty;
            Int32 ignoreMe;

            using (OracleConnection oc = new OracleConnection())
            {

                oc.ConnectionString = connectionString;
                oc.Open();
                queryresult = query;
                OracleDataAdapter oda = new OracleDataAdapter(query, oc);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (actiontype == 0)
                        {
                            resut = "PASS";
                            tansids.Add(dr["transid"].ToString().Trim());

                        }
                        else if (actiontype == 1)
                        {
                            bool trypasre = Int32.TryParse(dr[0].ToString(), out ignoreMe);
                            if (trypasre)
                            {
                                if (Convert.ToInt32(dr[0].ToString()) > 0)
                                    resut = "PASS";

                                countresult = dr[0].ToString().ToString();
                            }

                        }
                        else if (actiontype == 2)
                        {
                            resut = "PASS";
                            countresult = dt.Rows.Count.ToString();
                            tansids.Add(dr["transid"].ToString().Trim());
                        }
                    }
                }
                else
                    countresult = dt.Rows.Count.ToString();

            }
            return resut;
        }

        private string CheckGisArcService(string query)
        {
            string result = "FAIL";

            countresult = string.Empty;
            queryresult = string.Empty;



            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(query);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    JsonTextReader jsonReader = new JsonTextReader(new StreamReader(httpResponse.GetResponseStream()));
                    var jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    var jsonResult = JObject.Parse(jsonObject.ToString());

                    countresult = jsonResult.SelectToken("count").ToString();
                    queryresult = query;


                    if (Convert.ToInt32(countresult) > 0)
                        result = "PASS";


                }
            }
            catch (WebException wex)
            {
                result = "FAIL";


            }
            catch (Exception exp)
            {
                result = "FAIL";

            }
            return result;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }










    }

}