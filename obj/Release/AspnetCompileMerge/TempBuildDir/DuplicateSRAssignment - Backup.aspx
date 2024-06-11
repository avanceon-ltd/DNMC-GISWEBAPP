<%@ Page Title="Duplicate Complaints" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DuplicateSRAssignment - Backup.aspx.cs" 
    Inherits="WebAppForm.DuplicateSRAssignmentBK" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <link href="Styles/App.css" rel="stylesheet" />
     <style type="text/css">
        .Grid {
			/*background-color: #fff;*/
			margin: 5px 0 10px 0;
			border: solid 1px #e3d9d969;
			border-collapse: collapse;
			font-family: Calibri;
			/*color: #474747;*/
		}

            .Grid td {
                padding: 5px;
                /*border: solid 1px #c1c1c1;*/
            }

            .Grid th {
                padding: 5px 2px;
                color: #fff;
                background: #3a5570 url(Images/grid-header.png) repeat-x top;
                /*border-left: solid 1px #525252;*/
                font-size: 0.9em;
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
                    /*border-left: solid 1px #666;*/
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
						a:link {
			color: white;
			font-family: "Segoe UI Light",Arial;
			font-size: 12px;
		}

		/* visited link */
		a:visited {
			color: white;
			font-family: "Segoe UI Light",Arial;
			font-size: 12px;
		}

		/* mouse over link */
		a:hover {
			color: white;
			font-family: "Segoe UI Light",Arial;
			font-size: 12px;
		}

		/* selected link */
		a:active {
			color: white;
			font-family: "Segoe UI Light",Arial;
			font-size: 12px;
		}

                    .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 35px;
            width: 150px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
        }
        .btnDisabled{
            color: grey;
            background-color: #154670;
            height: 35px;
            width: 110px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
            cursor: not-allowed;
        }
            .simplebutton1:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }
    </style>
    
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                    <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">Assign Duplicate SR: MessageBox</h4>
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
    <div>
         <br /><p align="center" style="font-weight: bold; padding-left:20px; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    Assignment of Duplicate Complaints</p>
        
        <div style="padding-left: 20px;">
               <table id="table1" style="border-collapse: collapse;" width="100%" border="0"> 
                <tr><td colspan="4" class="heading">&nbsp;&nbsp; Parent Complaint:</td></tr>
                <tr><td colspan="4" ><hr style="border-top:1px solid #4c636f;margin-top:0px;"/> </td></tr>
                <tr>
                    <td class="required" colspan="2" style="text-align:left"><span style="color: red;">* </span>Parent Complaint:</td>
                    <td style="padding-left: 10px;" colspan="2">
                        <%--<asp:DropDownList ID="ddlParentSR" runat="server" CssClass="textfield" Width="186px" ></asp:DropDownList>--%>
                        <asp:Label ID="ddlParentSR" runat="server" CssClass="textfield" BorderWidth="0"></asp:Label>
                    </td>
                </tr>
                   <tr><td colspan="4" >&nbsp; </td></tr>
                   <tr><td colspan="4" class="heading">&nbsp;&nbsp; Select Duplicate Child Complaints:</td></tr>
                <tr><td colspan="4" ><hr style="border-top:1px solid #4c636f;margin-top:0px;"/> </td></tr>
                  <tr><td colspan="4" >
             <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"></asp:SqlDataSource>
            <asp:GridView ID="gvDuplicateSR" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                CellPadding="15" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvDuplicateSR_PageIndexChanging"
                DataSourceID="SqlDataSource1">
                <Columns>
                   
                   <asp:BoundField DataField="EXT_CRMSRID" HeaderText="Complaint#" ReadOnly="true" ItemStyle-Width="100" />
                    <asp:BoundField DataField="TICKETID" HeaderText="Ticket ID" ItemStyle-Width="80" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-Width="250" />
                    <asp:BoundField DataField="STATUS" HeaderText="Status" ItemStyle-Width="100" />
                    <asp:BoundField DataField="AFFECTEDPERSON" HeaderText="Affected Person" ItemStyle-Width="100" />
                    <asp:BoundField DataField="CHANGEDATE" HeaderText="CHANGEDATE" ItemStyle-Width="150" />
                    
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                </Columns>

                <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
            </asp:GridView>

            <asp:DataList ID="dlRowCount" runat="server" DataSourceID="SqlDataSource2">
                <ItemTemplate>
                    Total Records:
                    <asp:Label ID="label1" Font-Bold="true" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
                </ItemTemplate>
            </asp:DataList>
                      <div class="modal-body"><asp:Label ID="lblEmptyMessage" runat="server"></asp:Label></div>
                      </td>
                      </tr>

        
        <tr>
            <td class="required"></td>
            <td style="padding-left: 10px;"></td>
            <td class="required" align="left" style="text-align: right; width: 18%; vertical-align: middle;">&nbsp;
            </td>
            <td style="padding-top: 20px; padding-left: 7px;text-align:right;">
                
                <asp:Button ID="btnDuplicateSR" runat="server" ValidationGroup="test" OnClick="btnDuplicateSR_Click" 
                    CssClass="simplebutton1" Text="Assign Duplicate SR" OnClientClick="return confirm('Are you sure you want to assign dupliate complaints?');" />
            </td>
        </tr>
                   </table>
        </div><%--<div>
            <asp:Label ID="lblresult" runat="server"></asp:Label>
        </div>--%>
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
            //document.getElementById("ContentPlaceHolder1_txtWODes").value = "";

            document.getElementById("ContentPlaceHolder1_txtReportedBy").value = "";
            document.getElementById("ContentPlaceHolder1_txtLat").value = "";
            document.getElementById("ContentPlaceHolder1_txtlong").value = "";

            document.getElementById("ContentPlaceHolder1_txtAsset").value = "";
            document.getElementById("ContentPlaceHolder1_txtLocation").value = "";
            document.getElementById("ContentPlaceHolder1_txtVendorName").value = "";
            document.getElementById("ContentPlaceHolder1_txtVendor").value = "";

        }

    </script>

    <link rel="stylesheet" href="Styles/jquery-ui.css">
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
</asp:Content>
