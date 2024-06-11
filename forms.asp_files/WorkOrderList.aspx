<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderList.aspx.cs" Inherits="WebAppForm.WorkOrderList" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Work Orders - List</title>

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

        .search {
            background-color: #0090CB;
            border-color: #0090CB;
            color: #fff;
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
    </style>


</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">
    <form id="form1" runat="server" style="text-align: center;">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT TOP (100) WONUM,DESCRIPTION  FROM [ODW].[EAMS].[FactWorkOrder]  Order by CHANGEDATE desc"></asp:SqlDataSource>
            <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>" 
            SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimLocation]
  where isActive = 1"></asp:SqlDataSource>--%>
            <table>
                <tr>
                    <td style="text-align: left;">
                        <asp:HyperLink ID="linkWO" runat="server" NavigateUrl="~/CreateWorkOrder.aspx" Text="Create WO" Font-Bold="true"  Font-Names="Segoe UI Light" Font-Size="12px" ForeColor="White" ></asp:HyperLink></td>
                </tr>
               

                <tr>
                    <td>
                        <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light"
                            DataKeyNames="WONUM" CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                            DataSourceID="SqlDataSource1">
                            <Columns>
                                <asp:BoundField DataField="WONUM" HeaderText="WONUM" ItemStyle-Width="100" />
                                <%--  <asp:HyperLinkField DataTextField="WONUM" DataNavigateUrlFields="WONUM" ItemStyle-Width="100" />--%>
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ItemStyle-Width="600" ItemStyle-HorizontalAlign="Left" />
                                <%-- <asp:BoundField DataField="ASSETNUM" HeaderText="ASSETNUM" ItemStyle-Width="100" />--%>
                                <%--    <asp:BoundField DataField="LOCATION" HeaderText="LOCATION" ItemStyle-Width="100" />--%>
                                <%--       <asp:BoundField DataField="STATUS" HeaderText="STATUS" ItemStyle-Width="70" />--%>
                                <%--<asp:BoundField DataField="WORKTYPE" HeaderText="WORKTYPE" ItemStyle-Width="70" />--%>
                            </Columns>
                            <HeaderStyle BackColor="#3a5570" />
                            <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                            <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                        </asp:GridView>

                        <%--        <asp:DataList ID="DataList5" runat="server" DataSourceID="SqlDataSource2">
            <ItemTemplate>
                Total Records: <asp:Label ID="label1" Font-Bold="true" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
            </ItemTemplate>
        </asp:DataList>--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>

</html>
