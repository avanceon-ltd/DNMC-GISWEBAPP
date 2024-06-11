<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.ICCSCall"
    Title="ICCS - Call to Any Number" CodeBehind="ICCSCall.aspx.cs" EnableViewState="true" %>

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
            font-size: 13px;
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
            font-size: 13px;
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
            font-size: 13px;
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
            font-size: 13px;
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
             a:link {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }
        .Grid {
            
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
        }

            .Grid td {
                padding: 5px;
               
            }

            .Grid th {
                padding: 5px 2px;
                color: #fff;
                border-left: solid 1px #525252;
                font-size: 1.1em;
                text-align: center;
            }

           .Grid .alt {
                background: #fcfcfc url(Images/grid-alt.png) repeat-x top;
            }

          .Grid .pgr {
				background: #363670 url(Images/grid-pgr.png) repeat-x top;
				
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
					
					text-decoration: none;
				}

					.Grid .pgr a:hover {
						color: #000;
						text-decoration: underline;
					}

        .hiddencol {
            display: none;
        }
    </style>




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
    
    <table>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td style="padding-left: 20px;"><p align="left" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">DNMC Address Book</p></td>
        </tr>
        <tr>
            <td>
                <table>

                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <asp:Label ID="lblField1" runat="server" Width="90" Text="Search Field 1:&nbsp;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlField1" runat="server" CssClass="textfield" Width="150">
                                <asp:ListItem Value="r.REGION_NAME">Region & Network</asp:ListItem>
                                <asp:ListItem Value="c.CATEGORY_NAME">Catchment</asp:ListItem>
                                <asp:ListItem Value="d.TITLE">Designation</asp:ListItem>
                                <asp:ListItem Value="d.NAME">Name</asp:ListItem>
                                <asp:ListItem Value="d.ADDRESS1">Company</asp:ListItem>
                            </asp:DropDownList></td>
                        <td class="requiredvalue">
                            <asp:TextBox ID="txtField1" runat="server" class="textfield" MaxLength="30" Width="150"  />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAndOr" runat="server" CssClass="textfield" Width="60">
                                <asp:ListItem Value="AND">AND</asp:ListItem>
                                <asp:ListItem Value="OR">OR</asp:ListItem>                                
                            </asp:DropDownList></td>
                        <td class="required" style="text-align: right;">
                            <asp:Label ID="Label1" runat="server" Width="90" Text="Search Field 2:&nbsp;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlField2" runat="server" CssClass="textfield" Width="150">
                                <asp:ListItem Value="r.REGION_NAME">Region & Network</asp:ListItem>
                                <asp:ListItem Value="c.CATEGORY_NAME">Catchment</asp:ListItem>
                                <asp:ListItem Value="d.TITLE">Designation</asp:ListItem>
                                <asp:ListItem Value="d.NAME">Name</asp:ListItem>
                                <asp:ListItem Value="d.ADDRESS1">Company</asp:ListItem>
                            </asp:DropDownList></td>
                        <td class="requiredvalue">
                            <asp:TextBox ID="txtField2" runat="server" class="textfield" MaxLength="30" Width="150"  />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" style="padding-left: 20px;"><table>
                            <tr>
                                <td class="required" align="right" style="text-align: left;" >
                                <span class="normal">Region & Network:&nbsp;&nbsp;</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;"">
                                <asp:DropDownList ID="ddlRegionSearch" runat="server" CssClass="textfield" Width="200" AutoPostBack="True" OnSelectedIndexChanged="ddlRegionSearch_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Facility / Catchment Name:&nbsp;&nbsp;</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlCatchmentSearch" runat="server" CssClass="textfield" Width="200" AutoPostBack="True" OnSelectedIndexChanged="ddlCatchmentSearch_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            </tr>
                            </table></td>
                            
                        </tr>
                </table>
            </td>
        </tr>
        <%--<tr>
            <td>&nbsp;</td>
        </tr>--%>
        <tr>
            <td style="padding-left: 20px;">

                <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                    CellPadding="10" CellSpacing="10" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                    AllowCustomPaging="false" AllowSorting="true" CssClass="Grid" OnSelectedIndexChanged="gvCustomers_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-Width="200" />
                        <asp:BoundField DataField="TITLE" HeaderText="Designation" ItemStyle-Width="300" />
                        <asp:BoundField DataField="ADDRESS1" HeaderText="Company" />
                        <asp:BoundField DataField="REGION_NAME" HeaderText="Network" ItemStyle-Width="125" />
                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catchment" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300" />
                        <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Contact 1" ItemStyle-Width="150" />
                        <asp:BoundField DataField="NUMBER1" HeaderText="Contact 2" ItemStyle-Width="150" />
                        <asp:ButtonField Text="SELECT" CommandName="select"  />
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" Text="SELECT" CommandName="Select"
                                    OnClientClick="return GetSelectedRow(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>  --%>                  
                    </Columns>
                   <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                </asp:GridView>
               <%--<span class="normal" style="font-weight: bold;">Total Records: </span> --%>
                <asp:Label runat="server" ID="lblRecordCount" ForeColor="White" Font-Bold="true" Font-Size="10" />
            </td>
        </tr>
    </table>
    <br />
    <table id="table1" style="border-collapse: collapse;" width="100%" border="0">

        <tr>
            <td style="padding-left: 20px;">
                <div id="divone">
                    <table>
                        <tr>
                            <td colspan="2">
                                <p align="left" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">Call Details</p>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="left" style="text-align: left;">
                                <span class="normal">Reported By:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:Label ID="lblReportedBy" runat="server" class="textfield" Width="286" ForeColor="#89969B" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Region & Network:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList Enabled="false" style="-webkit-appearance: none;"  ID="ddlRegionNetwork" runat="server" CssClass="textfield" Width="286px" AutoPostBack="True" OnSelectedIndexChanged="ddlRegionNetwork_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Facility / Catchment Name:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlCatchment" style="-webkit-appearance: none;" Enabled="false" runat="server" CssClass="textfield" Width="286px" AutoPostBack="True" 
                                    OnSelectedIndexChanged="ddlCatchment_SelectedIndexChanged">                                    
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Name:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlName" runat="server" CssClass="textfield" Width="286px" style="-webkit-appearance: none;" Enabled="false"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Company:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:Label ID="lblCompany" runat="server" Text="Metito" class="textfield" Width="286" ForeColor="#89969B"  Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Contact #:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:DropDownList ID="ddlContacts" runat="server" CssClass="textfield" Width="286px"></asp:DropDownList>
                            </td>
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
                        <%--<tr>
                            <td class="required" align="left" style="text-align: left;">
                                <span style="color: red;">* </span><span class="normal">Title:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtTitle" runat="server" class="textfield" MaxLength="30" Width="286" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtTitle" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Title is Required" ForeColor="white" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="required" align="right" style="text-align: left;">
                                <span style="color: red;">* </span><span class="normal">Description:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;" colspan="3">
                                <asp:TextBox ID="txtDetail" runat="server" class="textfield" Width="286" Rows="5" Columns="50" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDetail" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Detail is Required" ForeColor="white" />
                            </td>
                        </tr>

                        <tr style="display: none;">
                            <td class="required" align="right" style="text-align: left;">
                                <span class="normal">Asset:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtAsset" runat="server" class="textfield" Width="286" MaxLength="30"></asp:TextBox>
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
                            <td class="required" align="left" style="text-align: right; vertical-align: middle;">&nbsp;</td>
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
            //document.getElementById("ContentPlaceHolder1_txtWODes").value = "";

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

    <%--    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />--%>



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
