<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.CMD" Title="SR Diagnostic Page" CodeBehind="CMD.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />

    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        h1, h2, h3, h4, h5, h6, pre, code, td {
            font-size: 1em;
            font-weight: normal;
        }


        .textfield {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            border: 1px solid #4c636f;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

            .textfield:hover {
                border: 1px solid #4c636f;
            }


        .heading {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 15px;
            font-size-adjust: none;
            font-stretch: normal;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            margin: 0;
            padding: 0px;
            text-align: left;
        }

        .required {
            font-weight: bold;
            padding: 0px;
            text-align: right;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .requiredvalue {
            padding: 0px;
            text-align: left;
        }

        .normal {
            font-size: 12px;
            font-family: "Segoe UI Light",Arial;
            vertical-align: middle;
        }

        .all_required {
            color: Red;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: normal;
            line-height: 30px;
        }

        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 27px;
            width: 90px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
        }

            .simplebutton1:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }
    </style>

    <table id="table1" style="border-collapse: collapse;" width="1600" border="2">


        <tr>
            <td>
                <br />

                <p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    Complaint Management Diagnostic  <input id="btnHide" type="button" Class="simplebutton1" style="float:right;" value="DEBUG"/>
                    <br />
                </p>


            </td>
        </tr>
        <tr>
            <td>

                <table width="100%" border="0">
                    <tr>
                        <td class="required" style="text-align: left; padding-left: 40PX; width: 80PX">
                            <span class="normal">Complaint#:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 0PX; width: 170px;">
                            <asp:TextBox ID="txtcomplaintno" runat="server" class="textfield" MaxLength="30" Width="150" BackColor="White" ForeColor="Black" />
                        </td>
                        <td style="width: 297px">
                            <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" />

                        </td>
                        <td class="heading" style="text-align: right;">&nbsp;&nbsp;CRMS SR Number:

                   <asp:Label ID="lbl_crmsid" runat="server" class="textfield" Font-Size="Small" Font-Bold="true" BorderWidth="0" />
                        </td>

                    </tr>
                </table>


            </td>
        </tr>
        <tr>
            <td>

                <table width="100%" border="2">
                    <tr>
                        <th class="required" style="text-align: center; font-weight: bold; width: 20px;"">SR#</th>
                        <th class="required" style="text-align: center; font-weight: bold; width: 390px;">Task Description</th>
                        <th class="required" style="text-align: center; font-weight: bold; width: 605px;">Reference</th>
                        <th class="required" style="text-align: center; font-weight: bold; width: 105px;">Result  (PASS-FAIL)</th>
                        <th class="required" style="text-align: left; font-weight: bold; width: 150px;">QueryResult</th>
                        <th class="required" style="text-align: left; font-weight: bold; width: 330px;">Query</th>


                    </tr>
                    <tr>

                        <td class="required" style="text-align: center;"><span class="normal">1</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ISD staging DB</span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="ref_lblCRMS_isd_stg" runat="server" Text="[DNMC].[dbo].[DrainageComplaints]" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_isd_stg" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_isd_stgQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_isd_stgQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">2</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ODW Staging DB</span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblCRMS_odw_stg" Text="[ODW-STG].[CRMS].[STG_DrainageComplaints]" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_stg" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_stgQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_stgQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">3</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ODW GIS (history) Complaint Table</span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblCRMS_odw_gis_tblhisty" Text="[ODW].[CRMS].[DrainageComplaints]" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_tblhisty" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_tblhistyQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_tblhistyQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">4</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ODW GIS Complaint Table</span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblCRMS_odw_gis_tbl" Text="[ODW].[GIS].[DrainageComplaints]" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_tbl" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_tblQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_tblQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">5</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ODW GIS Complaint Views (ECC)</span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblCRMS_odw_gis_vw" Text="[vwComplaintsExternalFlooding] -ECC" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_vw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_vwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_vwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">6</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ArcGIS Service [ComplaintsAll] </span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="rreflblCRMS_odw_gis_srvce" Text="UAT_ComplaintsAll" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvceQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvceQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">7</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ODW GIS Complaint Views[RainWater] (BAU)</span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblCRMS_odw_gis_rw" Text="[ODW].[GIS].[vwBAUComplaintsRainWater] -(BAU)" runat="server" class="textfield" BorderWidth="0" />
                        </td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_rw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_rwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_rwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">8</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ArcGIS Service -BAUComplaintsRainWater </span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="Label3" Text="BAUComplaintsRainWater" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce_R" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce_RQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce_RQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">9</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ODW GIS Complaint Views[NonRainWater] (BAU)</span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblCRMS_odw_gis_srvce" Text="[ODW].[GIS].[vwBAUComplaintsNonRainWater] -(BAU)" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_noNrw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_noNrwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_noNrwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">10</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">Complaint Available in ArcGIS Service -BAUComplaintsNonRainWater </span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="Label1" Text="BAUComplaintsNonRainWater" runat="server" class="textfield" BorderWidth="0" />
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce_nR" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>

                         <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce_nRQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblCRMS_odw_gis_srvce_nRQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                   </tr>



                         <%-- ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                       
                       
                       
                       EAMS SERVICE REQUEST
                       
                       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                       --%>
                  

                   <tr>
                        <td colspan="6" style="text-align: right;" class="heading">&nbsp;&nbsp;EAMS SR Number:
                            <asp:Label ID="lblEAMS_SR" runat="server" class="textfield" Font-Size="Small" Font-Bold="true" BorderWidth="0" />
                        </td>
                        <%--    <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="Label5" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="Label6" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>--%>
                    
                   </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">11</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR is available in ISD Actual DB </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_SR_isd_actdb" Text="MAXIMO.SR_DSS" runat="server" class="textfield" BorderWidth="0" />
                        </td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_actdb" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>



                            <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_actdbQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_actdbQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>




                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">12</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR is available in ISD Staging DB </span></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_SR_isd_stgdb" Text="MAXIMO.EXT_DSS_sr" runat="server" class="textfield" BorderWidth="0" />
                        </td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_stgdb" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                    
                    
                    
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_stgdbQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_stgdbQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    
                    
                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">13</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">DNMC ODW Successful Read from ISD Staging DB (Flag Check) </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_SR_isd_stgFC" Text="IFACENAME='EXT_DSS_SR'" runat="server" class="textfield" BorderWidth="0" />
                        </td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_stgFC" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   

                            <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_stgFCQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_isd_stgFCQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                        </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">14</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR is available in ODW Staging DB </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_SR_odw_stgdb" Text="[ODW-STG].[EAMS].[STG_EXT_DSS_SR]" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_stgdb" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_stgdbQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_stgdbQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">15</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR is available in DNMC ODW GIS SR Table </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="Label2" Text="[ODW].[GIS].[FactServiceRequest]" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_gisSR" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_gisSRQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_gisSRQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
    
                    </tr>


                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">16</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR is available in DNMC ODW GIS Complaint  (Table) </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_SR_odw_tbl" Text="CRMS.DrainageComplaints" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_tbl" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_tblQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_tblQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    
                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">17</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR is associated with Complaints (Complaints Views) </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">Result=  
                            <asp:Label ID="reflblEAMS_SR_odw_vw" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_vw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_vwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_odw_vwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">18</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS SR association is available in ArcGIS Service  </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">Result=  
                            <asp:Label ID="reflblEAMS_SR_gis_srvce" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_gis_srvce" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                 
                        
                    
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_gis_srvceQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_SR_gis_srvceQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                   <%-- ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                       
                       
                       
                       EAMS WORK WORDER
                       
                       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                       --%>
                  



                    <tr>
                        <td colspan="6" style="text-align: right;" class="heading">&nbsp;&nbsp;EAMS WO Number:
                            <asp:Label ID="lblEAMS_wo" runat="server" class="textfield" Font-Size="Small" Font-Bold="true" BorderWidth="0" />

                        </td>

                   

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">19</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in ISD Actual DB </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_wo_isd_actdb" Text="MAXIMO.workorder_dss" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_actdb" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_actdbQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_actdbQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">20</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in ISD Staging DB </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_wo_isd_stgdb" Text="MAXIMO.EXT_DSS_wo" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_stgdb" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        
                    
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_stgdbQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_stgdbQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">21</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">DNMC ODW Successful Read from ISD Staging DB (Flag Check) </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_wo_isd_stgdbFC" Text="IFACENAME='EXT_DSS_WO'" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_stgdbFC" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_stgdbFCQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_isd_stgdbFCQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
    
                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">22</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in ODW Staging DB </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_wo_odw_stg" Text="ODW-STG].[EAMS].[STG_EXT_DSS_WO]" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_stg" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_stgQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_stgQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
    
                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">23</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WorkOrder is available in DNMC ODW GIS Table </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="Label4" Text="[ODW].[GIS].[FactWorkOrder]" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lbl_odw_giswotbl" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lbl_odw_giswotblQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lbl_odw_giswotblQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
    
                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">24</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in DNMC ODW GIS Complaint(Table) </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_wo_odw_tbl_gis" Text="CRMS.DrainageComplaints" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_tbl_gis" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_tbl_gisQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_tbl_gisQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">25</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in its associated Complaint view   </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="reflblEAMS_wo_odw_vw" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_vw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_vwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_vwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">26</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in its own views   </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="ref_lblEAMS_wo_odw_own_vw" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_own_vw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_own_vwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_odw_own_vwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>






                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">27</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO association is available in ArcGIS Service  </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">Result=    
                            <asp:Label ID="reflblEAMS_wo_gis_srvce" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_gis_srvce" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                   
                        
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_gis_srvceQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_gis_srvceQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                    </tr>
                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">28</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in its own Views in ArcGIS Service   </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">Result=   
                            <asp:Label ID="reflblEAMS_wo_gis_srvce_vw" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_gis_srvce_vw" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_gis_srvce_vwQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblEAMS_wo_gis_srvce_vwQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>

                        
                    </tr>

                    <tr>
                        <td class="required" style="text-align: center;"><span class="normal">29</span></td>
                        <td class="required" style="text-align: left; padding-left: 10px;"><span class="normal">EAMS WO is available in WorkOrderAll ArcGIS Service   </span></td>

                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">Result=   
                            <asp:Label ID="ref_lblwoall" runat="server" class="textfield" BorderWidth="0" />
                        </td>


                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblwoall" runat="server" class="textfield" Text="N/A" Font-Bold="true" BorderWidth="0" /></td>
                  
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblwoallQR" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
                        <td class="requiredvalue" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblwoallQ" runat="server" class="textfield" Text="" Font-Bold="true" BorderWidth="0" /></td>
    
                    
                    </tr>

                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>


    <script type="text/javascript">

        $(function () {

            var labels = document.getElementsByTagName('span');
            for (i = 0; i < labels.length; i++) {
                if (labels[i].innerHTML.indexOf('FAIL') !== -1) {
                    labels[i].style.color = 'red';
                }
                if (labels[i].innerHTML.indexOf('NO') !== -1) {
                    labels[i].style.color = 'red';
                }
            }

            
        })


        $(document).ready(function () {
            $('td:nth-child(6),th:nth-child(6)').hide();
            $('td:nth-child(5),th:nth-child(5)').hide();

                $('#btnHide').click(function () {
                    $('td:nth-child(6),th:nth-child(6)').toggle();
                    $('td:nth-child(5),th:nth-child(5)').toggle();
                  
                });

               
            });


    </script>

</asp:Content>
