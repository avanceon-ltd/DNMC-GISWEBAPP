<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asset.aspx.cs" Inherits="WebAppForm.Asset" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assets - Work Order</title>
    <style type="text/css">
        .hiddencol {
            display: none;
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
                background: #363670 url(Images/grid-header.png) repeat-x top;
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
            <span style="font-size: 16px; margin-right: 10px;">Asset Num:</span>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="searchtxt" />
            <asp:Button Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="search" Width="100" Height="35" />
            <br />
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT ASSETNUM,DESCRIPTION,HIERARCHYPATH,STATUS,[LATITUDEY],[LONGITUDEX],[GISLONG],[GISLAT]  FROM [ODW].[EAMS].[DimAsset] where STATUS = 'OPERATING' AND LOCATION_KEY = 1271 Order by ASSETNUM"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimAsset] where STATUS = 'OPERATING' AND LOCATION_KEY = 1271"></asp:SqlDataSource>
            <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="ASSETNUM" HeaderText="ASSETNUM" ItemStyle-Width="100" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="400" />
                    <asp:BoundField DataField="HIERARCHYPATH" HeaderText="HIERARCHY PATH" ItemStyle-Width="100" />
                    <asp:BoundField DataField="STATUS" HeaderText="STATUS" ItemStyle-Width="100" />

                    <%--          ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>

                    <asp:BoundField DataField="LATITUDEY" HeaderText="LATITUDEY" />
                    <asp:BoundField DataField="LONGITUDEX" HeaderText="LONGITUDEX" />
                    <%--                 <asp:BoundField DataField="GISLONG" HeaderText="GISLONG"  />
                    <asp:BoundField DataField="GISLAT" HeaderText="GISLAT"  />--%>


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
                        debugger;
                        var row = lnk.parentNode.parentNode;
                        var rowIndex = row.rowIndex - 1;
                        var name = row.cells[0].innerHTML;
                        var lang = row.cells[4].innerHTML;
                        var lat = row.cells[5].innerHTML;
                        window.close();
                        window.opener.document.getElementById("ContentPlaceHolder1_txtAsset").value = name;

                        window.opener.document.getElementById("ContentPlaceHolder1_txtlong").value = lang;
                        window.opener.document.getElementById("ContentPlaceHolder1_txtLat").value = lat;

                        return false;
                    }
                    window.close();
                }
            </script>
        </div>
    </form>
</body>
</html>
