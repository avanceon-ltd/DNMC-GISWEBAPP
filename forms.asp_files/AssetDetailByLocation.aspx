<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetDetailByLocation.aspx.cs" Inherits="WebAppForm.AssetDetailByLocation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Details by Location ID</title>
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
    </style>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">

    <form id="form1" runat="server" style="text-align: center;">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT 
      [ASSETNUM]
      ,[CHANGEBY]
      ,[CHANGEDATE]    
      ,[CONDITIONCODE]
      ,[DESCRIPTION]
      ,[EXT_CONTRACTID]    
      ,[PLUSSFEATURECLASS]
      ,[SADDRESSCODE]     
      ,[ADDRESSCODE]
      ,[ADDRESSLINE2]
      ,[ADDRESSLINE3]
      ,[COUNTY]
      ,[DESCRIPTION_SA]
      ,[STATUS]
  FROM [ODW].[EAMS].[DimAsset] where LOCATION_KEY in (select LOCATION_KEY FROM [ODW].[EAMS].[DimLocation] where LOCATION = @ID)">
                  <SelectParameters>
                    <asp:QueryStringParameter DbType="String" DefaultValue="100" Name="ID" QueryStringField="ID" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimAsset] where LOCATION_KEY in (select LOCATION_KEY FROM [ODW].[EAMS].[DimLocation] where LOCATION = @ID)">
                           <SelectParameters>
                    <asp:QueryStringParameter DbType="String" DefaultValue="100" Name="ID" QueryStringField="ID" />
                </SelectParameters>
                

            </asp:SqlDataSource>
            <asp:GridView ID="gvCustomers"      Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="true"
              CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                DataSourceID="SqlDataSource1">
              
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
