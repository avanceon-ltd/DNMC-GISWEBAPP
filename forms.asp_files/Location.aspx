﻿<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Location.aspx.cs" Inherits="WebAppForm.Location" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Locations - Work Order</title>
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
    </style>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">
    <form id="form1" runat="server" style="text-align: center;">
        <div>
            <span style="font-size: 16px; margin-right: 10px;">LOCATION ID:</span>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="searchtxt" />
            <asp:Button Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="search" Width="100" Height="35" />
            <br />
            <br />

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>" SelectCommand="SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE
  FROM [ODW].[EAMS].[DimLocation]  WHERE isActive = 1 Order by LOCATION"
                OnSelected="SqlDataSource1_Selected"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>" SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimLocation]
  where isActive = 1"></asp:SqlDataSource>
            <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light"
                DataKeyNames="Location" CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                DataSourceID="SqlDataSource1" OnDataBound="gvCustomers_DataBound">
                <Columns>
                    <asp:BoundField DataField="LOCATION" HeaderText="LOCATION" ItemStyle-Width="100" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ItemStyle-Width="200" />
                    <asp:BoundField DataField="EXT_LOCATIONTAG" HeaderText="LOCATION TAG" ItemStyle-Width="200" />
                    <asp:TemplateField HeaderText="TYPE">
                        <ItemTemplate>OPERATING</ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SELECT">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect" runat="server" Text="SELECT" CommandName="Select"
                                OnClientClick="return GetSelectedRow(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
            </asp:GridView>

            <asp:DataList ID="DataList5" runat="server" DataSourceID="SqlDataSource2">
                <ItemTemplate>
                    Total Records:
                    <asp:Label ID="label1" Font-Bold="true" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
                </ItemTemplate>
            </asp:DataList>

            <script type="text/javascript">
                function GetSelectedRow(lnk) {


                    if (window.opener != null && !window.opener.closed) {

                        var row = lnk.parentNode.parentNode;
                        var rowIndex = row.rowIndex - 1;
                        var name = row.cells[0].innerHTML;
                        //var country = row.cells[2].innerHTML;
                        window.close();
                        //window.opener.document.getElementById("ContentPlaceHolder1_txtWODes").value = country;
                        window.opener.document.getElementById("ContentPlaceHolder1_txtLocation").value = name;
                        return false;
                    }
                    window.close();
                }
            </script>
        </div>
    </form>
</body>
</html>
