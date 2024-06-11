<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.SCADALiveDataSummary"
    Title="SCADA Alarms Report" CodeBehind="SCADALiveDataSummary.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>

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
            height: 28px;
            width: 120px;
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

        .heading {
            font-size: 30px;
            text-align: center;
            padding-left: 10px;
        }

        .GridPager a, .GridPager span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }

        .catchment {
            font-size: 12px;
        }
    </style>

    <div style="padding-left: 10px;">
        <div>
            <h1 class="heading">PS Summary -
                <asp:Label runat="server" ID="lblTitle" ForeColor="White" Font-Bold="true" /></h1>
        </div>

        <table width="100%">

            <tr>
                <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                    <span class="normal" style="font-weight: bold;">Comm Fail:</span>
                </td>
                <td class="requiredvalue" style="text-align: left; width: 32%;">
                    <asp:TextBox ID="lblCommFail" runat="server" class="textfield" BorderStyle="None"
                        Width="200" Text="&nbsp;"></asp:TextBox></td>
                <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                    <span class="normal" style="font-weight: bold;">Open WOs:</span>
                </td>
                <td class="requiredvalue" style="text-align: left; width: 32%;">
                    <asp:Label ID="lblOpenWOs" runat="server" class="textfield" BorderStyle="None"
                        Width="186" Text="&nbsp;" /></td>
            </tr>
            <tr>
                <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                    <span class="normal" style="font-weight: bold;">Status:</span>
                </td>
                <td class="requiredvalue" style="text-align: left; width: 32%;">
                    <asp:Label ID="lblStatus" runat="server" class="textfield" BorderStyle="None"
                        Width="200" Text="&nbsp;" /></td>
                <td class="requiwhite" align="right" style="text-align: right; width: 18%;">&nbsp;</td>
                <td class="requiredvalue" style="text-align: left; width: 32%;">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4"><h3 class="heading" style="font-size: 20px;">Status</h3></></td>
            </tr>
           <tr style="padding:5px;">
               <td colspan="4">
                   <asp:DataList ID="dlStatus" runat="server" EnableViewState="false" RepeatDirection="Horizontal" 
                      Width="100%" RepeatColumns="2" CellPadding="20" CellSpacing="20" OnItemDataBound="dlStatus_ItemDataBound">
                        
                        <ItemTemplate>
                            <td style="padding: 5px;" align="right"><h2 style="font-weight: bold;"><%# GetPumpName(Eval("TagName").ToString())%></h2></td>
                            <td align="left" style="padding: 5px;"><asp:Image ID="imgStatus" runat="server" ImageUrl='<%# GetImageUrl(Eval("Value").ToString())%>' Height="50" Width="80" /></td>
                        </ItemTemplate>
                    </asp:DataList>
               </td>
           </tr>  
            <tr>
                <td colspan="4"><h5 class="heading" style="font-size: 20px;">Wetwell Levels</h5></></td>
            </tr>
           <tr class="textfield">  
                <td style="padding: 5px;" align="right">Wetwell Level 1 - PV1</td>  
                <td align="left" style="padding: 5px;"><asp:Label ID="lblWWLevel1PV1" runat="server" Text=""></asp:Label></td>
               <td  style="padding: 5px;" align="right">Wetwell Level 1 - PV2</td>  
                <td align="left" style="padding: 5px;"><asp:Label ID="lblWWLevel1PV2" runat="server" Text=""></asp:Label></td> 
           </tr> 
            <tr class="textfield">  
                <td style="padding: 5px;" align="right">Wetwell Level 2 - PV1</td>  
                <td align="left" style="padding: 5px;"><asp:Label ID="lblWWLevel2PV1" runat="server" Text=""></asp:Label></td>
               <td  style="padding: 5px;" align="right">Wetwell Level 2 - PV2</td>  
                <td align="left" style="padding: 5px;"><asp:Label ID="lblWWLevel2PV2" runat="server" Text=""></asp:Label></td> 
           </tr>
            <tr>
                <asp:GridView ID="gvWetWellLevel" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="WetWellLevel" HeaderText="WetWellLevel" />
    </Columns>
</asp:GridView>
            </tr>
                
            
        </table>

        

    </div>
    <script type="text/javascript">

        function showModal() {
            $("#myModal").modal('show');

        }


        function SetButton(btn) {

            btn.value = "Wait...";

        }


        function hideModal() {
            $("#myModal").modal('hide');
        }
    </script>

    <link rel="stylesheet" href="Styles/jquery-ui.css">
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        //$('#ContentPlaceHolder1_txtWOReportedDate').datetimepicker({
        //    timeFormat: "hh:mm tt"
        //});
        $('#ContentPlaceHolder1_txtTargetStart').datetimepicker({
            //timeFormat: "hh:mm tt"
            dateFormat: 'yy-mm-dd',
            showOtherMonths: true,
            selectOtherMonths: true,
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtTargetFinish').datetimepicker({
            //timeFormat: "hh:mm tt"
            dateFormat: 'yy-mm-dd',
            showOtherMonths: true,
            selectOtherMonths: true,
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleStart').datetimepicker({
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleFinish').datetimepicker({
            timeFormat: "hh:mm tt"
        });
    </script>

</asp:Content>
