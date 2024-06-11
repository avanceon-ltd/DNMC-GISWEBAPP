<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.DispatchWO"
    Title="Dispatch Work Order" CodeBehind="DispatchWO.aspx.cs" %>

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
    </style>
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
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
                    <%--<asp:Button ID="Button2" CssClass="popupbtn" runat="server" OnClick="Button2_Click" Text="Create WO" />--%>
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
                    ECC Dispatch WO - Express Form<br />
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
                    <tr>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span class="normal" style="font-weight: bold;">WorkOrder No:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; width: 32%;">
                            <asp:Label ID="lblWONUM" runat="server" class="textfield" BorderStyle="None"
                                Width="186" Text="&nbsp;" /></td>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span class="normal" style="font-weight: bold;">EAMS SR No:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left; width: 32%;">
                            <asp:Label ID="lblEAMSSR" runat="server" class="textfield" BorderStyle="None"
                                Width="186" Text="&nbsp;" /></td>
                    </tr>
                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>ECC Ownership:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left;">
                            <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="textfield" Width="197">
                                <asp:ListItem Value="-1" Selected="True">- Select Ownership -</asp:ListItem>
                                    <asp:ListItem Value="CUSTOMER-SERVICE">1 - Customer Service</asp:ListItem>
                                    <asp:ListItem Value="WORKSHOPS">2 - Workshops</asp:ListItem>
                                    <asp:ListItem Value="FMP-WORK">3 - FMP Work</asp:ListItem>
                                    <asp:ListItem Value="ROADS">4 - Roads Work</asp:ListItem>
                                    <asp:ListItem Value="IA-PROJECTS">5 - IA Project</asp:ListItem>
                                    <asp:ListItem Value="BALADIYA">6 - Baladiya</asp:ListItem>

                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlOwnership" ID="RequiredFieldValidator6"
                                ValidationGroup="test" ErrorMessage="Please select Ownership."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>
                        </td>

                        <td class="required" align="left" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>ECC Priority:</span>
                        </td>
                        <td class="requiredvalue">

                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="197">
                                <asp:ListItem Value="-1" Selected="True">- Select Priority -</asp:ListItem>
                                    <asp:ListItem Value="VIP">1 - VIP</asp:ListItem>
                                    <asp:ListItem Value="SCHOOL">2 - School</asp:ListItem>
                                    <asp:ListItem Value="HOSPITAL">3 - Hospital</asp:ListItem>
                                    <asp:ListItem Value="UNDERPASS">4 - Underpass</asp:ListItem>
                                    <asp:ListItem Value="CRITICAL-ASSET">5 - Critical Asset</asp:ListItem>
                                    <asp:ListItem Value="HIGH">6 - High</asp:ListItem>
                                    <asp:ListItem Value="MEDIUM">7 - Medium</asp:ListItem>
                                    <asp:ListItem Value="LOW">8 - Low</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlPriority" ID="RequiredFieldValidator7"
                                ValidationGroup="test" ErrorMessage="Please select Priority."
                                InitialValue="-1" runat="server" Display="None" Operator="NotEqual" ValueToCompare="-1" Type="Integer">
                            </asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>Schedule Start:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left;">
                            <asp:TextBox ID="txtScheduleStart" runat="server" class="textfield" Width="186"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtScheduleStart" Display="None"
                                ValidationGroup="test" Text="*" ErrorMessage="Schedule Start is Required" ForeColor="white" />
                        </td>

                        <td class="required" align="left" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>Schedule Finish:</span>
                        </td>
                        <td class="requiredvalue">
                            <asp:TextBox ID="txtScheduleFinish" runat="server" class="textfield" Width="186" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtScheduleFinish" Display="None"
                                ValidationGroup="test" Text="*" ErrorMessage="Schedule Finish is Required" ForeColor="white" />

                            <asp:CustomValidator ID="StartEndDiffValidator"
                                ValidateEmptyText="true"
                                ValidationGroup="test"
                                OnServerValidate="StartEndDiffValidator_ServerValidate"
                                Display="None"
                                ErrorMessage="Schedule Start must be earlier than Schedule finish"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="required">
                            <span style="color: red;">* </span><span class="normal" style="font-weight: bold;">Contract Id:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left;padding-right: 0px;padding-left: 7px;padding-top: 5px;">
                            <asp:TextBox ID="txtContractId" runat="server" class="textfield" MaxLength="20" Width="186" ReadOnly="true" BorderWidth="0" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContractId" Display="None" EnableClientScript="true"
                                ValidationGroup="test" Text="*" ErrorMessage="Contract Id is Required" ForeColor="white" />
                            <%--<asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectContract()" Width="20" Height="20" ID="ImageButton2" Style="vertical-align: middle;" />--%>
                        </td>

                        <td class="required" align="left" style="text-align: right; padding-left: 49px;">
                            <span class="normal"><span style="color: red;">* </span>Scheduler:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left;padding-right: 0px;padding-left: 7px;padding-top: 5px;">
                            <asp:TextBox ID="txtScheduler" runat="server" class="textfield" MaxLength="30" Text="" Width="186" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtScheduler" Display="None" EnableClientScript="true"
                                ValidationGroup="test" Text="*" ErrorMessage="Schedule is Required" ForeColor="white" />
                            <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectScheduler()" Width="20" Height="20" ID="ImageButton5" Style="vertical-align: middle;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="required">Vendor:</td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtVendor" runat="server" class="textfield" Width="186" ReadOnly="true" BorderWidth="0"/>
                        </td>
                        <td class="requiwhite" align="right" style="text-align: right; width: 18%;">
                            <span style="color: red;">* </span><span class="normal" style="font-weight: bold;">Crew Lead:</span>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtCrew" runat="server" class="textfield" MaxLength="20" Width="186" />                            
                            <asp:ImageButton runat="server" ImageUrl="images/search.png" OnClientClick="SelectCrew()" Width="20" Height="20" ID="ImageButton1" Style="vertical-align: middle;" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCrew" Display="None" EnableClientScript="true"
                                ValidationGroup="test" Text="*" ErrorMessage="Crew Lead is Required" ForeColor="white" />

                        </td>
                    </tr>
                    <tr>
                        <td class="required">Vendor Name:</td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtVendorName" runat="server" class="textfield" Width="186" ReadOnly="true" BorderWidth="0"/>
                        </td>
                        <td class="required" align="left" style="text-align: right; padding-left: 49px;">
                            <span class="normal"></span>
                        </td>
                        <td class="requiredvalue"></td>
                    </tr>
                    <tr>
                            <td class="required">&nbsp;</td>
                        <td style="padding-left: 10px;">
                            &nbsp;
                        </td>
                        <td class="required" align="left" style="text-align: right; width: 18%; vertical-align: middle;">
                            &nbsp;
                        </td>
                        <td style="padding-top: 20px;padding-left: 7px;">                               
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

        function SelectContract() {
            popup = window.open("Contract.aspx", "Popup", "width=800,height=720");
            popup.focus();
            return false
        }

        function SelectCrew() {

            var companyKey = document.getElementById("ContentPlaceHolder1_txtVendor").value;
            if (companyKey != "") {
                popup = window.open("Crew.aspx?Company=" + companyKey, "Popup", "width=800,height=720");
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

    </script>

    <%--    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />--%>


    <link rel="stylesheet" href="Styles/jquery-ui.css">
    <script src="Scripts/jquery-ui.js"></script>
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


</asp:Content>
