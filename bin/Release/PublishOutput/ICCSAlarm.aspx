<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.ICCSAlarm"
    Title="Send Alarm to ICCS" CodeBehind="ICCSAlarm.aspx.cs" %>

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
            font-size: 11px;
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
            font-size: 11px;
        }

        .requiredvalue {
            padding: 5px 5px 5px;
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
            font-size: 11px;
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
            height: 27px;
            width: 120px;
            border: solid 1px #fff;
            font-size: 12px;
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

        .Grid {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
        }

            .Grid td {
                padding: 5px;
                border: solid 1px #c1c1c1;
            }

            .Grid th {
                padding: 5px 2px;
                color: #fff;
                border-left: solid 1px #525252;
                font-size: 0.9em;
            }

            .Grid .alt {
            }

            .Grid .pgr {
            }

                .Grid .pgr table {
                    margin: 3px 0;
                }

                .Grid .pgr td {
                    border-width: 0;
                    padding: 0 6px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .Grid .pgr a {
                    color: Gray;
                    text-decoration: none;
                }

                    .Grid .pgr a:hover {
                        color: #000;
                        text-decoration: none;
                    }
                    .hiddencol
  {
    display: none;
  }
    </style>



    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>


    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                    <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">ICCS Alarm: MessageBox</h4>
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

    <table id="table1" style="border-collapse: collapse;" width="450" border="0">
        <tr>
            <td style="padding-left: 5px;padding-top: 5px;">
                <p align="left" style="font-weight: bold; font-size: 16px; font-family: 'Segoe UI Light', Arial;">Recent Alarms</p>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 5px;"> 
                <div>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WWAlarmConnectionString %>"
                        SelectCommand="	SELECT Top(10) Source_HierarchicalObject,EventTime,Source_Object,Source_HierarchicalArea, comment,Source_Area 
                        FROM [Events]
                        WHERE  EventTime between  DATEADD(HOUR, -24, GETDATE())  AND GetDate()  
                        AND (Source_HierarchicalObject like 'PS40.%' OR Source_HierarchicalObject = @SiteName)
                        AND Priority < 950 AND Alarm_State = 'UNACK_ALM' ORDER BY EventTime DESC">
                       <SelectParameters>
                            <asp:QueryStringParameter DbType="String" Name="SiteName" QueryStringField="SiteName" DefaultValue="" />
                        </SelectParameters>                        
                    </asp:SqlDataSource>
                    <asp:GridView ID="gvAlarms" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                        CellPadding="10" CellSpacing="10" PageSize="10" AllowPaging="true" Width="450"
                        DataSourceID="SqlDataSource1" OnSelectedIndexChanged="gvAlarms_SelectedIndexChanged">
                         <Columns>
                             <asp:ButtonField Text="Select" CommandName="Select" ItemStyle-Font-Underline="true" />
                             <asp:BoundField HeaderText="Time" DataField="EventTime" ItemStyle-Width="70" DataFormatString = "{0:dd-MM HH:MM}" />
                             <asp:BoundField HeaderText="Asset" DataField="Source_Object" ItemStyle-Width="100" />
                             <asp:BoundField HeaderText="Comment" DataField="Comment" />
                             <asp:BoundField HeaderText="Hierarchical Area" DataField="Source_HierarchicalArea" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                         </Columns>
                        <HeaderStyle BackColor="#3a5570" />
                        <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                        <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <div id="divone">
                    <table width="450px">
                        <tr>
                            <td colspan="2">
                                <p align="left" style="font-weight: bold; font-size: 16px; font-family: 'Segoe UI Light', Arial;">Call Details</p>
                            </td>
                            </tr>
                        <tr>
                            <td class="required" align="left" style="text-align: left;">
                                <span class="normal">Reported By:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:Label ID="lblReportedBy" runat="server" class="textfield" Width="286" />                                
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="left" style="text-align: left;">
                                <span style="color: red;">* </span><span class="normal">Title:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtTitle" runat="server" class="textfield" MaxLength="30" Width="286" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtTitle" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Title is Required" ForeColor="white" />
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span style="color: red;">* </span><span class="normal">Description:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtDetail" runat="server" class="textfield" Width="286" MaxLength="250" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDetail" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Detail is Required" ForeColor="white" />
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span style="color: red;">* </span><span class="normal">Region Name:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtRegionName" runat="server" class="textfield" Width="286" MaxLength="250" ReadOnly="true" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span style="color: red;">* </span><span class="normal">Call Priority:</span>
                            </td>
                           <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="textfield" Width="286px">
                                    <asp:ListItem Value="1" Selected="True"> 1 - HIGH</asp:ListItem>
                                    <asp:ListItem Value="2">2 - MEDIUM</asp:ListItem>
                                    <asp:ListItem Value="3">3 - LOW</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Category:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="textfield" Width="286px" EnableViewState="true" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" OnInit="ddlCategory_Init">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Designation:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="textfield" Width="286px" EnableViewState="true" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Name:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlName" runat="server" CssClass="textfield" Width="286px" EnableViewState="true" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Contact #:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlContacts" runat="server" CssClass="textfield" Width="286px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Network\Framework\Area\Asset:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtHierarchy" runat="server" class="textfield" Width="286" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Network:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtNetwork" runat="server" class="textfield" Width="286" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Framework:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtFramework" runat="server" class="textfield" Width="286" MaxLength="30"></asp:TextBox>
                            </td>
                             
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Area:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtArea" runat="server" class="textfield" Width="286" MaxLength="30" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Asset:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtAsset" runat="server" class="textfield" Width="286" MaxLength="30"  ></asp:TextBox>
                                <asp:HiddenField ID="companyName" runat="server" />
                                <asp:HiddenField ID="contact1" runat="server" />
                                <asp:HiddenField ID="contact2" runat="server" />
                                <asp:HiddenField ID="contact3" runat="server" />
                                <asp:HiddenField ID="contactTel1" runat="server" />
                                <asp:HiddenField ID="contactTel2" runat="server" />
                                <asp:HiddenField ID="contactTel3" runat="server" />
                                <asp:HiddenField ID="region" runat="server" />
                            </td>
                             
                        </tr>
                       
                        <tr>
                            <td class="required" align="left" style="text-align: right; width: 18%; vertical-align: middle;">&nbsp;</td>
                            <td style="padding-top: 10px; padding-left: 7px;">
                                <asp:Button ID="btnSubmit" runat="server" ValidationGroup="test" OnClick="btnSubmit_Click" CssClass="simplebutton1" Text="Make Call!" />
                            </td>
                        </tr>

                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
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
            document.getElementById("ContentPlaceHolder1_txtCrew").value = "";
            document.getElementById("ContentPlaceHolder1_txtScheduler").value = "";
            document.getElementById("ContentPlaceHolder1_txtInspector").value = "";
            document.getElementById("ContentPlaceHolder1_txtContractId").value = "";
            document.getElementById("ContentPlaceHolder1_txtEngineer").value = "";
            document.getElementById("ContentPlaceHolder1_txtReportedBy").value = "";
            document.getElementById("ContentPlaceHolder1_ddlPriority").value = "";
            document.getElementById("ContentPlaceHolder1_ddlOwnership").value = "";
            document.getElementById("ContentPlaceHolder1_txtLat").value = "";
            document.getElementById("ContentPlaceHolder1_txtlong").value = "";
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
            popup = window.open("Asset.aspx?LocationId=" + name, "Popup", "width=800,height=720");
            popup.focus();
            return false
        }


        function SelectContract() {
            document.getElementById("ContentPlaceHolder1_txtScheduler").value = "";
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

    <link rel="stylesheet" href="Styles/jquery-ui.css">
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        $('#ContentPlaceHolder1_txtWOReportedDate').datetimepicker({
            timeFormat: "hh:mm tt"
        });
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

</asp:Content>
