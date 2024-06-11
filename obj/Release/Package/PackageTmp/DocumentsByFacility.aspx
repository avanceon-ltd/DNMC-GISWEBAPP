<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentsByFacility.aspx.cs" Inherits="WebAppForm.DocumentsByFacility" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Documents Links by Facility</title>
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
        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 27px;
            width: 84px;
            border: solid 1px #fff;
            font-size: 12px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            text-decoration: none;
            padding: 5px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
        }

            .simplebutton1:hover {
                background-color: #3498db;
                border: solid 1px #fff;
                cursor: pointer;
            }
    </style>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">

    <form id="form1" runat="server" style="text-align: center;">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT [Facility] ,[Hyperlink] ,[AssetID]  FROM [ODW].[CDMS].[SiteCorrelationList] WHERE Facility =@Facility">
                <SelectParameters>
                    <asp:QueryStringParameter DbType="String" DefaultValue="FW-PS" Name="Facility" QueryStringField="Facility" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT COUNT(*) AS TotalRows  FROM [ODW].[CDMS].[SiteCorrelationList] WHERE Facility =@Facility">
                <SelectParameters>
                    <asp:QueryStringParameter DbType="String" DefaultValue="FW-PS" Name="Facility" QueryStringField="Facility" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="gvDocLinks" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvDocLinks_PageIndexChanging"
                DataSourceID="SqlDataSource1">
                <Columns>
                      <asp:TemplateField HeaderText="Sr#"> <ItemTemplate><%#Container.DataItemIndex+1 %></ItemTemplate></asp:TemplateField>
                      <asp:BoundField DataField="Facility" HeaderText="Facility Name" ItemStyle-Width="70" />
                      <asp:HyperLinkField DataTextField="Hyperlink" DataNavigateUrlFields="Hyperlink" HeaderText="Document Link" Target="_blank"  />                     
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
        </div>
    </form>
</body>
</html>