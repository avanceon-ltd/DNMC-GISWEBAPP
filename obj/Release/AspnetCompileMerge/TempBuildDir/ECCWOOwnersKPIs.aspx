
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.ECCWOOwnersKPIs"
    Title="KPI Details - Owner Wise" CodeBehind="ECCWOOwnersKPIs.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   
    <style type="text/css">
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
                background: #1773D8 url(Images/grid-header.png) repeat-x top;
                border-left: solid 1px #525252;
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

        .search {
            background-color: #0090CB;
            border-color: #0090CB;
            color: #fff;
        }

            .search:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }

        .searchtxt {
            display: inline-block;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #555;
            vertical-align: middle;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc
        }
           .textfield {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
        }
         
        .textvalue {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .textfieldalt {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #022130;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
        }

        .textvaluealt {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #022130;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }
              .required {
            font-weight: bold;
            padding: 10px 0 10px 5px;
            text-align: right;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
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
                
    
    <table width="80%">
        <tr>
            <td colspan="6" align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">            
                    KPIs Count - Owner-Wise</td>
        </tr>
        <tr>
            <td colspan="6" align="right" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;padding-right:60px;">
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh"  CssClass="simplebutton1"  OnClick="btnRefresh_Click" />
            </td>
        </tr>
        <tr><td colspan="6">&nbsp;</td></tr>
    </table>
    <asp:GridView ID="gvOwner" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light"
        CellPadding="10" CellSpacing="10" AllowPaging="true">
        <Columns>
                <asp:BoundField DataField="OwnerName" HeaderText="Owner Name" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
            <%--<asp:BoundField DataField="UnAssigned" HeaderText="UnAssigned" ItemStyle-Width="50" />--%>
            <asp:BoundField DataField="Assigned" HeaderText="Assigned" ItemStyle-Width="50"  ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="Unattended" HeaderText="Dispatched" ItemStyle-Width="50"  ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="InProgress" HeaderText="In-Progress" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="Work Complete" HeaderText="Work-Complete" ItemStyle-Width="80"  ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="Resolved" HeaderText="Resolved" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" />
            <%--<asp:BoundField DataField="UnMapped" HeaderText="UnMapped" ItemStyle-Width="70" />--%>
        </Columns>

        <HeaderStyle BackColor="#3a5570" />
        <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
        <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
    </asp:GridView>
            
 </asp:Content>