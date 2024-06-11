<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.ICCSForm"
    Title="Address Book - Online Form" CodeBehind="ICCSForm.aspx.cs" %>

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


        .ui-autocomplete {
            cursor: pointer;
            height: 200px;
            overflow-y: scroll;
            -moz-background-clip: border !important;
            -moz-background-inline-policy: continuous !important;
            -moz-background-origin: padding !important;
            background: #001119 !important;
            border: 1px solid #4c636f !important;
            padding: 0.4em !important;
            color: white !important;
            font-family: "Segoe UI Light",Arial !important;
            font-size: 12px !important;
        }

            .ui-autocomplete:hover {
                border: 1px solid #4c636f !important;
            }
    </style>



    <script type="text/javascript" src="Scripts/1.12.4_jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>


    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                    <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">Add Contact: MessageBox</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                    <%--<asp:Button ID="Button2" CssClass="popupbtn" runat="server" OnClick="Button2_Click" Text="Create WO" />--%>
                    <asp:Button ID="Button1" CssClass="popupbtn" runat="server" Text="OK" OnClientClick="return;" />
                </div>
            </div>
        </div>
    </div>



    <table id="table1" style="border-collapse: collapse;" width="100%" border="0">
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    Add Contact - Express Form<br />
                </p>

                <br />
                <asp:ValidationSummary
                    ID="valSum"
                    DisplayMode="BulletList"
                    runat="server" CssClass="validationsummary"
                    ValidationGroup="test"
                    EnableClientScript="true"
                    HeaderText="&nbsp;Please correct the below errors:" ForeColor="white" ShowSummary="true" />

                <br />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td colspan="4" class="heading">&nbsp;&nbsp;Site Hierarchy Information:</td>
                    </tr>

                </table>
            </td>

        </tr>
        <tr>
            <td>
                <div id="divone">
                    <table width="100%">
                        <tr>
                            <td class="required"><span class="normal"><span style="color: red;">* </span>Network:</span></td>
                             <td>
                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="textfield" Width="186px" Style="margin-left: 8px; " >
                                    <asp:ListItem Selected="True" Value="PM">FWN - Foul Water Network</asp:ListItem>
                                    <asp:ListItem Value="IN">TWN - Treated Water Network</asp:ListItem>
                                    <asp:ListItem Value="CM">SGW - Surface Ground Water</asp:ListItem>
                                    <asp:ListItem Value="EM">PTP - Package Treatment Plant</asp:ListItem>
                                    <asp:ListItem Value="FR">STP - STP</asp:ListItem>
                                </asp:DropDownList></td>
                            <td class="required"><span style="color: red;">* </span>Framework:</td>
                            <td class="requiredvalue">
                                <asp:DropDownList ID="DropDownList2" runat="server" CssClass="textfield" Width="186px" Style="margin-left: 8px;">
                                    <asp:ListItem Value="PM">North</asp:ListItem>
                                    <asp:ListItem Value="IN" Selected="True">South</asp:ListItem>
                                    <asp:ListItem Value="CM">West</asp:ListItem>                                    
                                </asp:DropDownList></td>
                        </tr>
                         <tr>
                            <td class="required"><span class="normal"><span style="color: red;">* </span>Catchment:</span></td>
                             <td>
                                <asp:DropDownList ID="DropDownList3" runat="server" CssClass="textfield" Width="186px" Style="margin-left: 8px;" >
                                    <asp:ListItem Value="PM">PS44</asp:ListItem>
                                    <asp:ListItem Value="IN">PS1</asp:ListItem>
                                    <asp:ListItem Value="CM">PS10</asp:ListItem>
                                    <asp:ListItem Value="EM">PS11</asp:ListItem>
                                    <asp:ListItem Value="FR">PS16</asp:ListItem>
                                    <asp:ListItem Value="PJ" Selected="True">PS40</asp:ListItem>
                                    <asp:ListItem Value="RI">PS40/2</asp:ListItem>
                                    <asp:ListItem Value="AI">PS40A</asp:ListItem>
                                    <asp:ListItem Value="HI">PS3N</asp:ListItem>
                                </asp:DropDownList></td>
                            <td class="required"><span style="color: red;">* </span>Facility/Asset:</td>
                            <td class="requiredvalue">
                                <asp:DropDownList ID="DropDownList4" runat="server" CssClass="textfield" Width="186px" Style="margin-left: 8px; ">
                                   <asp:ListItem>10007292 - MCC PANEL</asp:ListItem>
                                    <asp:ListItem>10007295 - UPS PANEL</asp:ListItem>
                                    <asp:ListItem>10007296 - VENTILATION FAN 4 - BELT DRIVE TYPE</asp:ListItem>
                                    <asp:ListItem Selected="True">10007320 - FLOW METER 1</asp:ListItem>
                                    
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="4" class="heading">&nbsp;&nbsp;Personal Information:</td>

                        </tr>

                       
                        <tr>
                            <td class="required"><span style="color: red;">* </span>Name:</td>
                            <td style="padding-left: 10px;">
                                <asp:TextBox ID="TextBox1" runat="server" class="textfield" Width="186" />
                            </td>
                            <td class="required" align="right" style="text-align: right;">
                                <span id="spanRed" runat="server" style="color: red;">* </span>Address :
                            </td>
                            <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                                <asp:TextBox ID="txtWODes" runat="server" class="textfield" TextMode="MultiLine" Height="50" Width="185" Rows="30" Columns="20"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="required">Landline:</td>
                            <td style="padding-left: 10px;">
                                <asp:TextBox ID="txtEngineer" runat="server" class="textfield" Width="186" />
                                
                            </td>
                            <td class="required" align="right" style="text-align: right;">
                                <span class="normal">Mobile#:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                                <asp:TextBox ID="TextBox2" runat="server" class="textfield" Width="186" />

                            </td>
                        </tr>
                         <tr>
                            <td class="required"><span style="color: red;">* </span>Contact Name 1:</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="textfield" Width="186px" AutoPostBack="true">
                                    <asp:ListItem>1 - Manager</asp:ListItem>
                                    <asp:ListItem Value="CUSTOMER-SERVICE" Selected="True">2 - Engineer</asp:ListItem>
                                    <asp:ListItem Value="WORKSHOPS">3 - Supervisor</asp:ListItem>                                   
                                </asp:DropDownList>

                            </td>
                            <td class="required"><span style="color: red;">* </span>Contact# 1:</td>
                            <td style="padding-left: 10px;">
                               <%-- <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="186px">
                                    <asp:ListItem Value="-1">1 - 2264</asp:ListItem>
                                    <asp:ListItem Value="-1">2 - 2265</asp:ListItem>
                                    <asp:ListItem Value="-1">3 - 2266</asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="TextBox3" runat="server" class="textfield" Width="186" />

                            </td>
                        </tr>
                        <tr>
                            <td class="required"><span style="color: red;">* </span>Contact Name 2:</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="DropDownList5" runat="server" CssClass="textfield" Width="186px" AutoPostBack="true">
                                    <asp:ListItem>1 - Manager</asp:ListItem>
                                    <asp:ListItem Value="CUSTOMER-SERVICE" Selected="True">2 - Engineer</asp:ListItem>
                                    <asp:ListItem Value="WORKSHOPS">3 - Supervisor</asp:ListItem>                                   
                                </asp:DropDownList>

                            </td>
                            <td class="required"><span style="color: red;">* </span>Contact# 2:</td>
                            <td style="padding-left: 10px;">
                               <%-- <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="186px">
                                    <asp:ListItem Value="-1">1 - 2264</asp:ListItem>
                                    <asp:ListItem Value="-1">2 - 2265</asp:ListItem>
                                    <asp:ListItem Value="-1">3 - 2266</asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="TextBox4" runat="server" class="textfield" Width="186" />

                            </td>
                        </tr>
                        <tr>
                            <td class="required"><span style="color: red;">* </span>Contact Name 3:</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="DropDownList6" runat="server" CssClass="textfield" Width="186px" AutoPostBack="true">
                                    <asp:ListItem>1 - Manager</asp:ListItem>
                                    <asp:ListItem Value="CUSTOMER-SERVICE" Selected="True">2 - Engineer</asp:ListItem>
                                    <asp:ListItem Value="WORKSHOPS">3 - Supervisor</asp:ListItem>                                   
                                </asp:DropDownList>

                            </td>
                            <td class="required"><span style="color: red;">* </span>Contact# 3:</td>
                            <td style="padding-left: 10px;">
                               <%-- <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="186px">
                                    <asp:ListItem Value="-1">1 - 2264</asp:ListItem>
                                    <asp:ListItem Value="-1">2 - 2265</asp:ListItem>
                                    <asp:ListItem Value="-1">3 - 2266</asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="TextBox5" runat="server" class="textfield" Width="186" />

                            </td>
                        </tr>

                                              <tr>
                            <td class="required">&nbsp;</td>
                            <td style="padding-left: 10px;">&nbsp;
                            </td>
                            <td class="required" align="left" style="text-align: right; width: 18%; vertical-align: middle;">&nbsp;
                            </td>
                            <td style="padding-top: 20px; padding-left: 7px;">
                                <asp:Button ID="btnSubmit" runat="server" ValidationGroup="test" CssClass="simplebutton1" Enabled="true" Text="Add Contact" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>


        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>

    </table>

    <link rel="stylesheet" href="Styles/1.12.1-jquery-ui.css">
    <script type="text/javascript" src="Scripts/1.12.1-jquery-ui.min.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />

</asp:Content>
