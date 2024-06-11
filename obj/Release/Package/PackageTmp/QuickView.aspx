<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.QuickView"
    Title="Quick View Base" CodeBehind="QuickView.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />

    <style type="text/css">
        .fieldlabel {
            text-align: right;
            width: 50%;
        }

        .fieldvalue {
            text-align: left;
        }

        * {
            margin: 0;
            padding: 0;
        }

        h1, h2, h3, h4, h5, h6, pre, code, td {
            font-size: 1em;
            font-weight: normal;
        }

        ul, ol {
            list-style: none;
        }

        a img, :link img, :visited img, fieldset {
            border: 0;
        }

        a {
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }

        td {
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        h2, .h2 {
            font-family: "Segoe UI Light",Arial;
            font-size: 15px;
        }

        h3 {
            font-family: "Segoe UI Light",Arial;
            font-size: 15px;
            color: #154670;
        }

        h1 {
            font-family: "Segoe UI Light",Arial;
            font-size: 19px;
            color: #154670;
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



        .CustomValidatorCalloutStyle div, .CustomValidatorCalloutStyle td {
            border: solid 1px blue;
            background-color: Black;
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
            padding: 3px 5px 3px 0;
            text-align: left;
        }

        .required {
            font-weight: bold;
            padding: 10px 0 10px 5px;
            text-align: right;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .requiredvalue {
            padding: 10px 10px 5px;
            text-align: left;
        }

        .chkrequired {
            font-weight: bold;
            padding: 10px 0 3px 5px;
            text-align: left;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .text_padding {
            padding-top: 20px;
        }

        .normal {
            font-size: 12px;
            font-family: "Segoe UI Light",Arial;
            vertical-align: middle;
        }

        .chknormal {
            line-height: 23px;
        }

        .all_required {
            color: Red;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: normal;
            line-height: 30px;
        }

        .style3 {
            font-weight: bold;
            padding: 10px 0 3px 5px;
            text-align: left;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            width: 205px;
        }



        ul li {
            list-style-type: square;
            margin-left: 40px;
        }

        .validationsummary {
            border: 1px solid #fff;
            background-color: #001119;
            padding: 0px 0px 13px 0px;
            font-size: 12px;
            width: 99%;
            color: white;
        }

            .validationsummary ul {
                padding-top: 5px;
                padding-left: 45px;
            }

                .validationsummary ul li {
                    padding: 2px 0px 0px 15px;
                }

        .wo {
            color: white;
            padding: 1em 1.5em;
            text-decoration: none;
            text-transform: uppercase;
        }

            .wo a:hover {
                background-color: #555;
            }

            .wo a:active {
                background-color: black;
            }

            .wo a:visited {
                background-color: #ccc;
            }

        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 35px;
            width: 140px;
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




        .popupbtn {
            color: #fff;
            background-color: #154670;
            height: 23px;
            width: 62px;
            border: solid 1px #fff;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            border: 1px solid #4c636f;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
            cursor: pointer;
        }


            .popupbtn:hover {
                background-color: #3498db;
                border: solid 1px #4c636f;
            }
    </style>

    <br />
    <table id="table1" style="width: 350px; border: 2px dashed" cellpadding="10" cellspacing="10">

        <tr>
            <td>
                <p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    <asp:Image ImageUrl="images/search.png" runat="server" Width="20" Height="20" ID="Image" Style="vertical-align: middle;" />uickView - Express Form
                </p>
                <br />
            </td>
        </tr>


        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 125px; text-align: right">CRMS SR:</td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txt_CRMSComplaints" runat="server" CssClass="textfield" Width="140" placeholder="Service_Request_Number" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">
                            <a href='javascript:void(0);' id='LnkCRMSComplaintDetails' title="See CRMSComplaint Details" style="font-family: Segoe UI Light; color: white;">Details</a>


                              <asp:LinkButton Style="padding-left: 5px;" ToolTip="See CRMS SR List" ID="LnkBtnCRMSSR" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectCRMSSRList()" ></asp:LinkButton>

                        </td>
                    </tr>

                </table>
            </td>

        </tr>


        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 140px; text-align: right">EAMS SR:</td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txtEAMSSR" runat="server" CssClass="textfield" Width="140" placeholder="EAMS_SR_NUM" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">

                            <a href='javascript:void(0);' id='LnkEAMSSRDetails' title="See EAMS-SR Details" style="font-family: Segoe UI Light; color: white;">Details</a>

                            <asp:LinkButton Style="padding-left: 5px;" ToolTip="See EAMS SR List" ID="LnkBtnEamsSR" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectEAMSSRList()" ></asp:LinkButton>

                        </td>
                    </tr>

                </table>
            </td>

        </tr>


        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 125px; text-align: right">EAMS WorkOrder:       </td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txt_WONUM" runat="server" CssClass="textfield" Width="140" placeholder="WO_NUM" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">
                            <a href='javascript:void(0);' id='LnkWODetails' title="See WorkOrder Details" style="font-family: Segoe UI Light; color: white;">Details</a>

                            <asp:LinkButton Style="padding-left: 5px;" ToolTip="See WorkOrder List" ID="LinkButton2" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectWoList()"></asp:LinkButton>

                        </td>
                    </tr>

                </table>
            </td>

        </tr>

        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 135px; text-align: right">AVLS Vehicle:</td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txt_AVLSVehicle" runat="server" CssClass="textfield" Width="140" placeholder="VEHICLE_NUM" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">

                            <a href='javascript:void(0);' id='LnkAVLSVehicleDetails' title="See Vehicle Details" style="font-family: Segoe UI Light; color: white;">Details</a>

                             <asp:LinkButton Style="padding-left: 5px;" ToolTip="See Vehicle Information List" ID="LnkBtnVL" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectVehList()" ></asp:LinkButton>
                        </td>
                    </tr>

                </table>
            </td>

        </tr>


        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 136px; text-align: right">EAMS Assets:

                        </td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txt_Asset" runat="server" CssClass="textfield" Width="140" Text="" placeholder="ASSET_NUM" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">
                            <a href='javascript:void(0);' id='LnkAssetDetails' title="See Asset Details" style="font-family: Segoe UI Light; color: white;">Details</a>

                            <asp:LinkButton Style="padding-left: 5px;" ToolTip="See Asset List" ID="lnkBtnAssetList" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectAsset()"></asp:LinkButton>

                        </td>
                    </tr>

                </table>
            </td>

        </tr>


        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 136px; text-align: right">Assets Failure:
                        </td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txt_AF" runat="server" CssClass="textfield" Width="140" placeholder="MANUAL_ASSET_NUM" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">
                            <a href='javascript:void(0);' id='LnkAFDetails' title="See Asset_Failure Details" style="font-family: Segoe UI Light; color: white;">Details</a>

                            <asp:LinkButton Style="padding-left: 5px;" ToolTip="See Asset_Failure List" ID="LnkbtAF" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectAFList()" ></asp:LinkButton>


                        </td>
                    </tr>

                </table>
            </td>

        </tr>

        <tr>
            <td style="padding-bottom: 10px;">
                <table width="100%">
                    <tr>
                        <td style="width: 132px; text-align: right">Facility Location:</td>
                        <td style="padding-left: 10px; width: 150px; text-align: left;">

                            <asp:TextBox ID="txt_fl" runat="server" CssClass="textfield" Width="140" placeholder="LOCATION_NUM" />
                        </td>

                        <td align="left" style="width: 23%; text-align: left; padding-left: 10px;">

                            <a href='javascript:void(0);' id='LnkFLDetails' title="See Location Details" style="font-family: Segoe UI Light; color: white;">Details</a>


                             <asp:LinkButton Style="padding-left: 5px;" ToolTip="See Facility Location List" ID="lnkbtnFLL" runat="server" Text="List" Font-Names="Segoe UI Light"
                                ForeColor="White" OnClientClick="SelectFLList()" ></asp:LinkButton>


                        </td>
                    </tr>

                </table>
            </td>
        </tr>

    </table>




    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.js"></script>

    <script type="text/javascript">

        function SelectWoList() {
            popup = window.open("WO_List.aspx", "Popup", "width=1080,height=920");
            popup.focus();
            return false
        }

        function SelectAsset() {
            popup = window.open("Asset.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectAFList() {
            popup = window.open("AssetFailureList.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }





        function SelectFLList() {
            popup = window.open("Location_List.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }
        
        function SelectVehList() {
            popup = window.open("Vehicle_List.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectEAMSSRList() {
            popup = window.open("EAMS_SRList.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectCRMSSRList() {
            popup = window.open("CRMS_SRList.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }





        $(function () {

            $("#ContentPlaceHolder1_txt_CRMSComplaints").keypress(function () {
                if (event.which == 13) {
                    $('#LnkCRMSComplaintDetails').click();
                }
            });

            $("#ContentPlaceHolder1_txtEAMSSR").keypress(function () {
                if (event.which == 13) {
                    $('#LnkEAMSSRDetails').click();
                }
            });

            $("#ContentPlaceHolder1_txt_WONUM").keypress(function () {
                if (event.which == 13) {
                    $('#LnkWODetails').click();
                }
            });


            $("#ContentPlaceHolder1_txt_AVLSVehicle").keypress(function () {
                if (event.which == 13) {
                    $('#LnkAVLSVehicleDetails').click();
                }
            });

            $("#ContentPlaceHolder1_txt_Asset").keypress(function () {
                if (event.which == 13) {
                    $('#LnkAssetDetails').click();
                }
            });


            $("#ContentPlaceHolder1_txt_AF").keypress(function () {
                if (event.which == 13) {
                    $('#LnkAFDetails').click(); 
                }
            });

            $("#ContentPlaceHolder1_txt_fl").keypress(function () {
                if (event.which == 13) {
                    $('#LnkFLDetails').click();
                }
            });


            $('#LnkWODetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txt_WONUM").val();

                //  alert(str);
                window.open("WorkOrderDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });
            

            $('#LnkCRMSComplaintDetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txt_CRMSComplaints").val();

                //  alert(str);
                window.open("CRMSComplaintDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });


            $('#LnkEAMSSRDetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txtEAMSSR").val();

                //  alert(str);
                window.open("SRDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });


            $('#LnkAVLSVehicleDetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txt_AVLSVehicle").val();

                //  alert(str);
                window.open("VehicleDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });


            $('#LnkAssetDetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txt_Asset").val();
                //   alert(strWO1);

                window.open("AssetDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });


            $('#LnkAFDetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txt_AF").val();

                //  alert(str);
                window.open("AssetFailureDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });


            $('#LnkFLDetails').click(function () {
                var strWO = $("#ContentPlaceHolder1_txt_fl").val();

                window.open("LocationsDetail.aspx?ID=" + strWO, "Popup", "width=690,height=400");
            });


        });

    </script>


</asp:Content>
