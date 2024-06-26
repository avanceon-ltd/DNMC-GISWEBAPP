﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.CreateBAUFRWO1"
    Title="Ashgal WorkOrder Creation - Online Form Submission" CodeBehind="CreateBAUFRWO1.aspx.cs" %>

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
                    <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">CreateWo: MessageBox</h4>
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


        <%--<tr>
            <td style="text-align: right;">
                <asp:HyperLink ID="linkWO" runat="server" NavigateUrl="~/WorkOrderList.aspx" Text="List View" Font-Bold="true" CssClass="wo"></asp:HyperLink></td>
        </tr>--%>
        <tr>
            <td>
                <p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">


                    <%if (Request.QueryString["israinwater"] == "1")
                        { %>
                    <asp:Label ID="Label1" runat="server" Text="Create Rainwater FR WO -Express Form"></asp:Label><br />
                    <%} %>
                    <%else
                        { %>
                    <asp:Label ID="Label2" runat="server" Text="Create Non-Rain FR WO -Express Form"></asp:Label>
                    <%} %>


                    <br />
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
                        <td colspan="4" class="heading">&nbsp;&nbsp;WorkOrder Information:</td>
                    </tr>

                </table>
            </td>

        </tr>
        <tr>
            <td>
                <div id="divone">
                    <table width="100%">
                        <tr>
                            <td class="required"><span class="normal">CRMS ID:</span></td>
                            <td class="requiredvalue" style="width: 34%;">
                                <asp:Label ID="lblCRMSId" runat="server" class="textfield" Width="186" Text="&nbsp;" BorderWidth="0" /></td>
                            <td class="required">EAMS SR ID:</td>
                            <td class="requiredvalue">
                                <asp:Label ID="lblEAMSSRId" runat="server" class="textfield" Width="186" Text="&nbsp;" BorderWidth="0" /></td>
                        </tr>
                        <tr>
                            <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                                <span style="color: red;">* </span><span class="normal" style="font-weight: bold;">Defect Type:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left; width: 32%;">
                                <asp:DropDownList ID="ddlDefectType" runat="server" CssClass="textfield" Width="186px">
                                    <%--     <asp:ListItem Value="SWRBKUP">SWRBKUP - Sewer Back Up (No flooding/spillage)</asp:ListItem>
                                    <asp:ListItem Value="XTFLD">External Flooding</asp:ListItem>
                                    <asp:ListItem Value="XTFLDTSE">External Flooding TSE/Recycled Water</asp:ListItem>
                                    <asp:ListItem Value="MNHLCVR" Selected="True">MNHLCVR - Manhole Cover</asp:ListItem>
                                    <asp:ListItem Value="ODOUR">Odour Complaint</asp:ListItem>
                                    <asp:ListItem Value="INTFLOOD">Internal Flooding</asp:ListItem>
                                    <asp:ListItem Value="NOWATER">No Water TSE/Recycled Water</asp:ListItem>
                                    <asp:ListItem Value="LOWPRESS">Low Pressure TSE/Recycled Water</asp:ListItem>
                                    <asp:ListItem Value="NOISE">Noise Complaint</asp:ListItem>
                                    <asp:ListItem Value="IGWFLOOD">Internal groundwater Flooding</asp:ListItem>
                                    <asp:ListItem Value="GRSTRAP">Grease Trap Request</asp:ListItem>
                                    <asp:ListItem Value="CLAIMS">CLAIMS - Claims</asp:ListItem>
                                    <asp:ListItem Value="DEWATER">DEWATER - Dewatering Permit Request</asp:ListItem>
                                    <asp:ListItem Value="RNWFLOOD">RNWFLOOD - Rainwater Flooding</asp:ListItem>
                                    <asp:ListItem Value="TSECON">TSECON - TSE Connection Request</asp:ListItem>--%>
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                                <span style="color: red;">* </span><span class="normal" style="font-weight: bold;">WO Description:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left; width: 32%;">
                                <asp:TextBox ID="txtWODes" runat="server" class="textfield" TextMode="MultiLine" Height="50" Width="185" Rows="30" Columns="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqtxtWodesc" runat="server" ControlToValidate="txtWODes" Display="None" EnableClientScript="true"
                                    ValidationGroup="test" Text="*" ErrorMessage="WO Description is Required" ForeColor="white" />

                            </td>

                            <td class="required" align="left" style="text-align: right; vertical-align: bottom;">
                                <span class="normal">MX Location / :</span><br />
                                <span class="normal" style="padding: 14px;">Facility</span>
                            </td>
                            <td class="requiredvalue" style="padding-right: 0px; padding-left: 7px; padding-top: 0px; padding-bottom: -1px;">
                                <asp:TextBox ID="txtLocation" runat="server" class="textfield" Width="186" Text="" />
                                <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectDetails()" Width="20" Height="20" ID="search" Style="vertical-align: middle;" />
                            </td>
                        </tr>

                        <tr>
                            <td class="required">Asset:</td>
                            <td class="requiredvalue" style="width: 34%;">
                                <asp:TextBox ID="txtAsset" runat="server" CssClass="textfield" Width="185">
                                        
                                </asp:TextBox>
                                <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectAsset()" Width="20" Height="20" ID="ImageButton6" Style="vertical-align: middle;" />


                            </td>
                            <td class="required"><span style="color: red;">* </span>Work Type:</td>
                            <td>
                                <asp:DropDownList ID="drpdwnWorkType" runat="server" CssClass="textfield" Width="186px" Style="margin-left: 8px; border: none;" Enabled="false">
                                    <asp:ListItem Value="PM">PM - Preventive Maintenance</asp:ListItem>
                                    <asp:ListItem Value="IN">IN - Inspection</asp:ListItem>
                                    <asp:ListItem Value="CM">CM - Corrective Maintenance</asp:ListItem>
                                    <asp:ListItem Value="EM">EM - Emergency Maintenance</asp:ListItem>
                                    <asp:ListItem Value="FR" Selected="True">FR - First Response</asp:ListItem>
                                    <asp:ListItem Value="PJ">PJ - Projects</asp:ListItem>
                                    <asp:ListItem Value="RI">RI - Routine Inspection</asp:ListItem>
                                    <asp:ListItem Value="AI">AI - Ad-Hoc Inspection</asp:ListItem>
                                    <asp:ListItem Value="HI">HI - Handover Inspection</asp:ListItem>
                                    <asp:ListItem Value="CI">CI - Customer Inspection</asp:ListItem>
                                    <asp:ListItem Value="SY">SY - Survey</asp:ListItem>
                                    <asp:ListItem Value="SL">SL - Situation Location</asp:ListItem>
                                </asp:DropDownList></td>
                            <asp:RequiredFieldValidator ControlToValidate="drpdwnWorkType" ValidationGroup="test" ID="RequiredFieldValidator1"
                                CssClass="errormesg" ErrorMessage="Please select a WorkType"
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <%--<td class="required"><span style="color: red;">* </span>Internal Priority:</td>--%>
                            <%-- <td style="padding-left: 10px;">
                                <asp:DropDownList ID="drpdwnInternalPriority" runat="server"  Enabled="false"      CssClass="textfield" Width="197px" onfocus="document.getElementById('divone').className='borderfull';"
                                    onblur="document.getElementById('divone').className='bordernone';">
                                    <asp:ListItem Value="1">1 - Urgent</asp:ListItem>
                                    <asp:ListItem Value="2">2 - Very High</asp:ListItem>
                                    <asp:ListItem Value="3">3 - High</asp:ListItem>
                                    <asp:ListItem Value="4">4 - Medium</asp:ListItem>
                                    <asp:ListItem Value="5">5 - Low</asp:ListItem>
                                    <asp:ListItem Value="7">7 - Very Low - Drainage</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="drpdwnInternalPriority" ID="RequiredFieldValidator2"
                                    ValidationGroup="test" ErrorMessage="Please select Internal Priority"
                                    InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                                </asp:RequiredFieldValidator>

                            </td>--%>


                            <td class="required">Reported By:</td>
                            <td style="padding-left: 10px;">
                                <%--     <asp:Label ID="lblReportedBy" Height="17px" runat="server" class="textfield" Style="height: 30px; width: 185px;border:none;" />--%>

                                <asp:TextBox ID="txtReportedBy" runat="server" class="textfield" Style="height: 30px; width: 185px; border: none;" MaxLength="61" />
                                <%--  <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectReportedBy()" Width="20" Height="20" ID="ImgReportedBy" Style="vertical-align: middle;" />--%>

                            </td>
                            <td class="required" align="right" style="text-align: right;">
                                <span class="normal">WO Reported Date:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtWOReportedDate" runat="server" class="textfield" Width="186"
                                    ReadOnly="true"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Longitude (X):</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtlong" runat="server" class="textfield" Width="185" MaxLength="18"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    FilterType="Numbers, Custom" ValidChars="."
                                    TargetControlID="txtlong">
                                </ajax:FilteredTextBoxExtender>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtlong" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Longitude(X) is Required" ForeColor="white" />

                            </td>

                            <td class="required" align="left" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Latitude (Y):</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtLat" runat="server" class="textfield" MaxLength="18" Width="186" />
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                    FilterType="Numbers, Custom" ValidChars="."
                                    TargetControlID="txtLat">
                                </ajax:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtLat" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Latitude(Y) is Required" ForeColor="white" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="required" align="right" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Actual Start:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtActualStart" runat="server" class="textfield" Width="185"
                                    onfocus="document.getElementById('divone').className='borderfull';" 
                                    onblur="document.getElementById('divone').className='bordernone';"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtActualStart" Display="None" 
                                    ValidationGroup="test" Text="*" ErrorMessage="Actual Start is Required" ForeColor="white" EnableClientScript="true" />

                            </td>

                            <td class="required" align="left" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Actual Finish:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtActualFinish" runat="server" class="textfield"
                                    onfocus="document.getElementById('divone').className='borderfull';"
                                    onblur="document.getElementById('divone').className='bordernone';"
                                    Width="182"  />
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtActualFinish" Display="None" 
                                    ValidationGroup="test" Text="*" ErrorMessage="Actual Finish is Required" ForeColor="white" />
                                <%--<asp:CompareValidator id="cvtxtStartDate" runat="server" 
                                     ControlToCompare="txtActualStart" cultureinvariantvalues="true" 
                                     display="None" enableclientscript="true"  
                                     ControlToValidate="txtActualFinish" ValidationGroup="test"
                                     ErrorMessage="Actual Start must be earlier than Actual Finish"
                                     type="Date" Operator="GreaterThanEqual" 
                                     text="Start date must be earlier than finish date"></asp:CompareValidator>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="required" align="right" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Target Start:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtTargetStart" runat="server" class="textfield" Width="185"
                                    onfocus="document.getElementById('divone').className='borderfull';"
                                    onblur="document.getElementById('divone').className='bordernone';"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTargetStart" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Target Start is Required" ForeColor="white" />
                            </td>

                            <td class="required" align="left" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Target Finish:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtTargetFinish" runat="server" class="textfield"
                                    Width="186" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTargetFinish" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Target Finish is Required" ForeColor="white" />
                                <asp:CustomValidator ID="StartEndDiffValidator"
                                    ValidateEmptyText="true"
                                    ValidationGroup="test"
                                    OnServerValidate="StartEndDiffValidator_ServerValidate"
                                    Display="None"
                                    ErrorMessage="Target Start must be earlier than Schedule Target"
                                    runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4" class="heading">&nbsp;&nbsp;Workflow Related Fields:</td>

                        </tr>

                        <tr>
                            <td class="required"><span style="color: red;">*</span>Framework Zone:</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="ddlFrameZone" runat="server" CssClass="textfield" Width="186px" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlFrameWrk" ID="RequiredFieldValidator3"
                                    ValidationGroup="test" ErrorMessage="Framework is Required."
                                    InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                        <%-- <tr>
                            <td class="required"><span style="color: red;">* </span>ECC Ownership:</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="textfield" Width="186px" AutoPostBack="true" OnSelectedIndexChanged="ddlOwnership_SelectedIndexChanged">
                                    <asp:ListItem Value="-1" Selected="True">- Select Ownership -</asp:ListItem>
                                    <asp:ListItem Value="CUSTOMER-SERVICE">1 - Customer Service</asp:ListItem>
                                    <asp:ListItem Value="WORKSHOPS">2 - Workshops</asp:ListItem>
                                    <asp:ListItem Value="FMP-WORK">3 - FMP Work</asp:ListItem>
                                    <asp:ListItem Value="ROADS">4 - Roads Work</asp:ListItem>
                                    <asp:ListItem Value="IA-PROJECTS">5 - IA Project</asp:ListItem>
                                    <asp:ListItem Value="BALADIYA">6 - Baladiya</asp:ListItem>

                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlOwnership" ID="RequiredFieldValidator3"
                                    ValidationGroup="test" ErrorMessage="ECC Ownership is Required."
                                    InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="required"><span style="color: red;">* </span>ECC Priority:</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="186px">
                                    <asp:ListItem Value="-1" >- Select Priority -</asp:ListItem>
                                    <asp:ListItem Value="VIP">1 - VIP</asp:ListItem>
                                    <asp:ListItem Value="SCHOOL">2 - School</asp:ListItem>
                                    <asp:ListItem Value="HOSPITAL">3 - Hospital</asp:ListItem>
                                    <asp:ListItem Value="UNDERPASS">4 - Underpass</asp:ListItem>
                                    <asp:ListItem Value="CRITICAL-ASSET">5 - Critical Asset</asp:ListItem>
                                    <asp:ListItem Value="HIGH" Selected="True">6 - High</asp:ListItem>
                                    <asp:ListItem Value="MEDIUM">7 - Medium</asp:ListItem>
                                    <asp:ListItem Value="LOW">8 - Low</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlPriority" ID="RequiredFieldValidator4"
                                    ValidationGroup="test" ErrorMessage="ECC Priority is Required."
                                    InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>--%>

                        <tr>
                            <td class="required"><span style="color: red;">* </span>
                                Contract Id:
                            </td>
                            <td style="padding-left: 10px;">

                                <asp:TextBox ID="txtContractId" runat="server" class="textfield" Width="186" />
                                <asp:RequiredFieldValidator ID="ReqFldContract" runat="server" ControlToValidate="txtContractId" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Contract Id is Required" ForeColor="white" />
                                <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectContract()" Width="20" Height="20" ID="imgbtnContract" Style="vertical-align: middle;" />

                            </td>
                            <td class="required" align="right" style="text-align: right;">
                                <span id="spanRed" runat="server" style="color: red;">* </span>Scheduler:
                            </td>
                            <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">

                                <asp:TextBox ID="txtScheduler" runat="server" class="textfield" Width="186" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtScheduler" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Scheduler is Required" ForeColor="white" />
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="images/search.png" OnClientClick="SelectScheduler()"
                                    Width="20" Height="20" Style="vertical-align: middle;" />
                            </td>
                        </tr>

                        <tr>
                            <td class="required">Vendor:</td>
                            <td style="padding-left: 10px;">

                                <asp:TextBox ID="txtVendor" runat="server" class="textfield" Width="186" BorderWidth="0" />

                            </td>
                            <td class="required" align="right" style="text-align: right;">
                                <span class="normal">Engineer:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                                <asp:TextBox ID="txtEngineer" runat="server" class="textfield" Width="186" />
                                <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectEngineer()" Width="20" Height="20" ID="imgbtnEngineer" Style="vertical-align: middle;" />



                            </td>
                        </tr>

                        <tr>
                            <td class="required">Vendor Name:</td>
                            <td style="padding-left: 10px;">
                                <asp:TextBox ID="txtVendorName" runat="server" class="textfield" Width="186" BorderWidth="0" />
                            </td>

                            <td style="padding-left: 46px;" class="required">On Behalf Of:</td>
                            <td style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                                <asp:TextBox ID="txtOnBehalfOf" runat="server" class="textfield" Width="186" />
                            </td>
                        </tr>


                        <tr>
                            <td class="required">&nbsp;</td>
                            <td style="padding-left: 10px;">&nbsp;
                            </td>
                            <td class="required" align="left" style="text-align: right; width: 18%; vertical-align: middle;">&nbsp;
                            </td>
                            <td style="padding-top: 20px; padding-left: 7px;">
                                <asp:HiddenField ID="CompanyKey" runat="server" />
                                <asp:Button ID="btnSubmit" runat="server" ValidationGroup="test" OnClick="btnSubmit_Click" CssClass="simplebutton1" Enabled="true" Text="Create WO" />
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
    <script type="text/javascript">

        function showModal() {
            $("#myModal").modal('show');

        }


        function SetButton(btn) {

            btn.value = "Wait...";

        }


        function hideModal() {
            $("#myModal").modal('hide');
            //document.getElementById("ContentPlaceHolder1_txtWODes").value = "";

            document.getElementById("ContentPlaceHolder1_txtCrew").value = "";
            document.getElementById("ContentPlaceHolder1_txtScheduler").value = "";
            document.getElementById("ContentPlaceHolder1_txtInspector").value = "";
            document.getElementById("ContentPlaceHolder1_txtContractId").value = "";
            document.getElementById("ContentPlaceHolder1_txtEngineer").value = "";

            document.getElementById("ContentPlaceHolder1_ddlPriority").value = "";
            document.getElementById("ContentPlaceHolder1_ddlOwnership").value = "";
            document.getElementById("ContentPlaceHolder1_txtLat").value = "";
            document.getElementById("ContentPlaceHolder1_txtlong").value = "";
            //document.getElementById("ContentPlaceHolder1_lblReportedBy").value = "";
            document.getElementById("ContentPlaceHolder1_drpdwnWorkType").value = "";
            document.getElementById("ContentPlaceHolder1_txtAsset").value = "";
            document.getElementById("ContentPlaceHolder1_txtLocation").value = "";
            document.getElementById("ContentPlaceHolder1_txtVendorName").value = "";
            document.getElementById("ContentPlaceHolder1_txtVendor").value = "";

        }

        var popup;
        function SelectDetails() {
            popup = window.open("Location.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectAsset() {
            var name = document.getElementById("ContentPlaceHolder1_txtLocation").value;
            popup = window.open("Asset.aspx?LocationId=&IsLocation=0", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }


        function SelectContract() {

            popup = window.open("Contract.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectCrew() {

            var txtContractId = document.getElementById("ContentPlaceHolder1_txtContractId").value;
            if (txtContractId != "") {
                var VandorID = document.getElementById("ContentPlaceHolder1_VandorID").value;
                popup = window.open("Crew.aspx?VandorID=" + VandorID, "Popup", "width=800,height=720");
                popup.focus();
            } else {
                popup = window.open("Crew.aspx", "Popup", "width=800,height=720");
                popup.focus();
            }
            return false
        }

        function SelectScheduler() {

            var companyKey = document.getElementById("ContentPlaceHolder1_txtVendor").value;
            if (companyKey != "") {
                popup = window.open("Scheduler.aspx?Company=" + companyKey, "Popup", "width=800,height=720");
                popup.focus();
            } else {
                popup = window.open("Scheduler.aspx", "Popup", "width=800,height=720");
                popup.focus();
            }
            return false
        }


        function SelectEngineer() {
            popup = window.open("Engineer.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectInspector() {
            popup = window.open("Inspector.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }


        function SelectReportedBy() {
            popup = window.open("ReportedBy.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }



    </script>

    <link rel="stylesheet" href="Styles/1.12.1-jquery-ui.css">
    <script type="text/javascript" src="Scripts/1.12.1-jquery-ui.min.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        //$('#ContentPlaceHolder1_txtWOReportedDate').datetimepicker({
        //    timeFormat: "hh:mm tt"
        //});
        $('#ContentPlaceHolder1_txtTargetStart').datetimepicker({
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtTargetFinish').datetimepicker({
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleStart').datetimepicker({
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleFinish').datetimepicker({
            timeFormat: "hh:mm tt"
        });

    </script>


    <%--    <script type="text/javascript">

        $(document).ready(function () {

            $('#ContentPlaceHolder1_txtScheduler').autocomplete({
                minLength: 0,
                scroll: true,
                source: function (request, responce) {

                    $.ajax({
                        url: "DataService.asmx/GetOwnerDefaultNames",
                        method: "post",
                        contentType: "application/json;charset=utf-8",
                        data: JSON.stringify({ term: $('#ContentPlaceHolder1_ddlOwnership :selected').val().charAt("0") }),
                        dataType: 'json',
                        success: function (data) {
                            responce(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                }
            }).focus(function () {
                $(this).autocomplete("search", "");
            });
        });


    </script>--%>
</asp:Content>
