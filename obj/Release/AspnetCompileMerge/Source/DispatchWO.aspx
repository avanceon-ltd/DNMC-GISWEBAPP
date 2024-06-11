<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.DispatchWO"
    Title="Dispatch Work Order (O/P)" CodeBehind="DispatchWO.aspx.cs" %>

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



        .accordionHeader {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }

        #master_content .accordionHeader a {
            color: #FFFFFF;
            background: none;
            text-decoration: none;
        }

            #master_content .accordionHeader a:hover {
                background: none;
                text-decoration: underline;
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

        .accordionHeaderSelected {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
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


        p, div, input {
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


        .ui-widget.ui-widget-content {
        }

        .ui-widget-content {
        }
    </style>

    <script type="text/javascript" src="Scripts/1.12.4_jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>



    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                    <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">WO Dispatched: MessageBox</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                    <asp:Button ID="Button1" CssClass="popupbtn" runat="server" Text="OK" OnClientClick="return;" />
                </div>
            </div>
        </div>
    </div>

    <table id="table1" style="border-collapse: collapse;" width="100%" border="0">
        <tr>
            <td style="text-align: right;">
                <asp:HyperLink ID="linkWO" runat="server" NavigateUrl="~/WorkOrderList.aspx" Text="List View" Font-Bold="true" CssClass="wo"></asp:HyperLink></td>
        </tr>
        <tr>
            <td>
                <p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    ECC Express Form - Dispatch WO<br />
                </p>

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
                    <tr style="display: none">
                        <td colspan="4" class="requiredvalue" style="text-align: right; width: 32%;">
                            <asp:LinkButton ID="lnkReload" runat="server" Text="Reload Data" OnClick="lnkReload_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span class="normal" style="font-weight: bold;">Work Order#:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; width: 32%;">
                            <asp:TextBox ID="lblWONUM" runat="server" class="textfield" BorderStyle="None"
                                Width="200" Text="&nbsp;"></asp:TextBox></td>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span class="normal" style="font-weight: bold;">CRMS SR#:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; width: 32%;">
                            <asp:Label ID="lblCRMSSR" runat="server" class="textfield" BorderStyle="None"
                                Width="186" Text="&nbsp;" /></td>
                    </tr>
                    <tr>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span class="normal" style="font-weight: bold;">Work Order Status:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; width: 32%;">
                            <asp:Label ID="lblWOStatus" runat="server" class="textfield" BorderStyle="None"
                                Width="200" Text="&nbsp;" /></td>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span class="normal" style="font-weight: bold;">EAMS SR#:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; width: 32%;">
                            <asp:Label ID="lblEAMSSR" runat="server" class="textfield" BorderStyle="None"
                                Width="186" Text="&nbsp;" /></td>
                    </tr>
                    <tr>
                        <td class="required" align="right" style="text-align: right;">ECC Ownership:</td>
                        <td class="requiredvalue" style="text-align: left;">
                            <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="textfield" Width="200px" Enabled="false" Style="border: none; border-radius: 5px; -webkit-appearance: none;">
                                <asp:ListItem Value="-1" Selected="True">- Select Ownership -</asp:ListItem>
                                <asp:ListItem Value="CUSTOMER-SERVICE">1 - Customer Service</asp:ListItem>
                                <asp:ListItem Value="WORKSHOPS">2 - Workshops</asp:ListItem>
                                <asp:ListItem Value="FMP-WORK">3 - FMP Work</asp:ListItem>
                                <asp:ListItem Value="ROADS">4 - Roads Work</asp:ListItem>
                                <asp:ListItem Value="IA-PROJECTS">5 - IA Project</asp:ListItem>
                                <asp:ListItem Value="BALADIYA">6 - Baladiya</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="required">Scheduler:</td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtScheduler" runat="server" class="textfield" Width="186" ReadOnly="true" BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="required"><span style="color: red;">* </span>Crew Lead:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Drpdwn_Crew" runat="server" CssClass="textfield" Width="200px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="Drpdwn_Crew" ID="RequiredFieldValidator1"
                                ValidationGroup="test" ErrorMessage="CrewLead is Required."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </td>

                        <td class="required" align="right" style="text-align: right;">Contract Id: </td>
                        <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                            <asp:TextBox ID="txtContractId" runat="server" class="textfield" Width="186" BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal">Vendor:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                            <asp:TextBox ID="txtVendor" runat="server" class="textfield" Width="200" BorderWidth="0" />

                        </td>
                        <td style="padding-left: 46px;" class="required">Vendor Name:</td>
                        <td style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                            <asp:TextBox ID="txtVendorName" runat="server" class="textfield" Width="186" BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal">Longitude:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                            <asp:TextBox ID="lblLong" runat="server" class="textfield" Width="200" BorderWidth="0" />
                        </td>
                        <td style="padding-left: 46px;" class="required">Latitude:</td>
                        <td style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                            <asp:TextBox ID="lblLat" runat="server" class="textfield" Width="186" BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="heading">&nbsp;&nbsp; WO Specs Information:</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr style="border-top: 1px solid #4c636f; margin-top: 0px;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="required"><span style="color: red;">* </span>Flooding Location:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlFloodLoc" runat="server" CssClass="textfield" Width="186px"></asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlFloodLoc" ID="RequiredFieldValidatorddlFloodLoc"
                                ValidationGroup="test" ErrorMessage="Flood Location is Required."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="required"><span style="color: red;">* </span>Is SGW network available?:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Network_Availability" runat="server" CssClass="textfield" Width="186px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="Network_Availability" ID="RequiredFieldValidatorNetwork_Availability"
                                ValidationGroup="test" ErrorMessage="Network Availability is Required."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td class="required"><span style="color: red;">* </span>SGW Network Type?:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Network_Type" runat="server" CssClass="textfield" Width="186px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="Network_Availability" ID="RequiredFieldValidatorNetwork_Type"
                                ValidationGroup="test" ErrorMessage="Network Type is Required."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="required"><span style="color: red;">* </span>Road Class?:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Road_Class" runat="server" CssClass="textfield" Width="186px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="Road_Class" ID="RequiredFieldValidatorRoad_Class"
                                ValidationGroup="test" ErrorMessage="Road Class is Required."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td style="padding-left: 46px;" class="required"><span style="color: red;">* </span>Event:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Event" runat="server" CssClass="textfield" Width="186px">
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td class="required"><span style="color: red;">* </span>Restricted Area:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlRestrictedArea" runat="server" CssClass="textfield" Width="186px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="Drpdwn_Crew" ID="ddlRestrictedAreaRequiredFieldValidator"
                                ValidationGroup="test" ErrorMessage="Restricted Area is Required."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>


                        </td>
                        <td class="required" align="left" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>ECC Priority:</span>
                        </td>
                        <td class="requiredvalue">

                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="186">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlPriority" ID="RequiredFieldValidator7"
                                ValidationGroup="test" ErrorMessage="Please select Priority."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>

                        </td>


                    </tr>
                    <tr>
                        <td class="required">Hotspot ID:</td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtHotspot" runat="server" class="textfield" Width="165" MaxLength="250" />
                            <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectHotspot();" Width="20" Height="20" ID="imgbtnHotspot" Style="vertical-align: middle;" />
                            <asp:ImageButton runat="server" ImageUrl="images/distance-logo.png" OnClientClick="SelectHotspotRadius();" ID="ImageButton1" Style="vertical-align: middle;" />
                            <asp:Label ID="lblHotspotId" runat="server" Visible="false"></asp:Label>
                        </td>
                        <td class="required" align="right" style="text-align: right;">Project ID:</td>
                        <td class="requiredvalue" style="text-align: left; padding-right: 0px; padding-left: 7px; padding-top: 10px;">
                            <asp:TextBox ID="txtProject" runat="server" class="textfield" Width="165" MaxLength="250" />
                            <asp:ImageButton runat="server" ImageUrl="images/search.png" Width="20" Height="20" OnClientClick="SelectPABoundary();" ID="imgbtnProject" Style="vertical-align: middle;" />
                            <asp:ImageButton runat="server" ImageUrl="images/distance-logo.png" OnClientClick="SelectPAByWO();" ID="ImageButton2" Style="vertical-align: middle;" />
                            <asp:Label ID="lblProjectId" runat="server" Visible="false"></asp:Label>
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
                            <asp:Button ID="btnSubmit" runat="server" ValidationGroup="test" OnClick="btnSubmit_Click" CssClass="simplebutton1" Text="Dispatch WO" />
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        function showModal() {
            $("#myModal").modal('show');

        }
        function hideModal() {
            $("#myModal").modal('hide');
        }
        var popup;

        function SelectHotspot() {
            popup = window.open("Hotspot.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false;
        }

        function SelectHotspotRadius() {
            var wonum = document.getElementById("ContentPlaceHolder1_lblWONUM").value;
            //"alert('" + Request.QueryString("WONUM") + "');";
            popup = window.open("HotspotRadius.aspx?WONUM=" + wonum, "Popup", "width=800,height=720");
            popup.focus();
            return false;
        }

        function SelectPABoundary() {
            popup = window.open("PABoundaries.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false;
        }
        //SelectPAByWO
        function SelectPAByWO() {
            var lat = document.getElementById("ContentPlaceHolder1_lblLat").value;
            var long = document.getElementById("ContentPlaceHolder1_lblLong").value;
            popup = window.open("PABoundaryByWO.aspx?long=" + long + "&lat=" + lat, "Popup", "width=800,height=720");
            popup.focus();
            return false;
        }

        function SelectCrew() {

            var companyKey = document.getElementById("ContentPlaceHolder1_txtVendor").value;
            debugger;
            if (companyKey != "") {
                popup = window.open("Crew.aspx?Company=" + companyKey, "Popup", "width=800,height=720");
                popup.focus();
            } else {
                popup = window.open("Crew.aspx", "Popup", "width=800,height=720");
                popup.focus();
            }
            return false;
        }


        function Select_ECC_Crew() {
            var txtOwner = $('#ContentPlaceHolder1_ddlOwnership :selected').val().charAt("0");
            debugger;
            if (txtOwner != "") {
                popup = window.open("ECCCrew.aspx?ID=" + txtOwner, "Popup", "width=600,height=620");
                popup.focus();
            }
            return false;

        }

        function SelectScheduler() {

            var companyKey = document.getElementById("ContentPlaceHolder1_txtVendor").value;
            debugger;
            if (companyKey != "") {
                popup = window.open("Scheduler.aspx?Company=" + companyKey, "Popup", "width=800,height=720");
                popup.focus();
            } else {
                popup = window.open("Scheduler.aspx", "Popup", "width=800,height=720");
                popup.focus();
            }
            return false;
        }

    </script>




    <link rel="stylesheet" href="Styles/1.12.1-jquery-ui.css">
    <script type="text/javascript" src="Scripts/1.12.1-jquery-ui.min.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript">

        $('#ContentPlaceHolder1_txtScheduleStart').datetimepicker({
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleFinish').datetimepicker({
            timeFormat: "hh:mm tt"
        });

    </script>



    <%--    <script type="text/javascript">
        $(document).ready(function () {
            BindControls();
        });

        function BindControls() {
            var Countries = ['CSE_ECC_CORD_1',
                'CSE_ECC_CORD_2',
                'CSE_ECC_CORD_3',
                'CSE_ECC_CORD_4',
                'CSE_ECC_CORD_5',
                'CSE_ECC_CORD_6',
                'CSE_ECC_CORD_7',
                'CSE_ECC_CORD_8',
                'CSE_ECC_CORD_9',
                'CSE_ECC_CORD_10'];

            $('#ContentPlaceHolder1_txtCrew').autocomplete({
                source: Countries,
                minLength: 0,
                scroll: true
            }).focus(function () {
                $(this).autocomplete("search", "");
            });
        }
    </script>--%>



    <script type="text/javascript">

        $(document).ready(function () {

            $('#ContentPlaceHolder1_txtCrew').autocomplete({
                minLength: 0,
                scroll: true,
                source: function (request, responce) {

                    $.ajax({
                        url: "DispatchWO.aspx/GetCrewDefaultNames",
                        method: "post",
                        contentType: "application/json;charset=utf-8",
                        data: JSON.stringify({ term: $('#ContentPlaceHolder1_ddlOwnership :selected').val().charAt("0") }),
                        dataType: 'json',
                        success: function (data) {
                            console.log(JSON.stringify(data));
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


    </script>



</asp:Content>
