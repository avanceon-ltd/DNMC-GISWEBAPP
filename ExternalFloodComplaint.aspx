<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalFloodComplaint.aspx.cs" Inherits="WebAppForm.ExternalFloodComplaint" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>External Flood Complaints List</title>
    <meta http-equiv="refresh" content="60">
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
            border: 1px solid #4c636f;
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

        .hiddencol {
            display: none;
            visibility:hidden;
        }
    </style>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">

    <form id="form1" runat="server" style="text-align: center;">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT TOP (1000) [Service_Request_Id]
      ,[Service_Request_Number]
      ,[Owner]
      ,[Division]
      ,[Service_Request_Type]
      ,[Contact_Reason]
      ,[Customer]
      ,[Contact]
      ,[Mobile_Number]
      ,[Email]
      ,[Preferred_Method_Of_Contact]
      ,[Created_By]
      ,[Contact_Source]
      ,[Details]
      ,[Priority]
      ,[Municipality]
      ,[Zone]
      ,[District]
      ,[Street]
      ,[Building_Number]
      ,[Nearest_Land_Mark]
      ,[Pin]
      ,[Electricity_No]
      ,[Water_No]
      ,[Latitude]
      ,[Longitude]
      ,[Attend_SLA_Follow_Up_Date]
      ,[Attend_SLA_Status]
      ,[Complete_Work_SLA_Follow_Up_Date]
      ,[Complete_Work_SLA_Status]
      ,[Application_Stage]
      ,[Application_Status]
      ,[EAMS_SR_Number]
      ,[Created_On]
      ,[Modified_On]
      ,[DNMC_Fetch_Date]
      ,[DNMC_Fetch_Flag]
      ,[GISLONG]
      ,[TICKETID]
      ,[WONUM]
      ,[WO_STATUS]
      ,[WO_EXT_FRAMEZONE]
      ,[WO_OWNER]

      ,[KPI_STATUS]
  FROM [ODW].[GIS].[vwComplaintsExternalFlooding] order by Modified_On desc"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
                SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[GIS].[vwComplaintsExternalFlooding]"></asp:SqlDataSource>


            <table>

                <br />


                <tr>
                    <td>

                        <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                            CellPadding="10" CellSpacing="10" PageSize="50" AllowPaging="true" OnRowDataBound="gvCustomers_RowDataBound" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                            DataSourceID="SqlDataSource1">
                            <Columns>
                                <asp:BoundField DataField="Service_Request_Number" HeaderText="Service Request Number" ItemStyle-Width="150" />
                                <asp:BoundField DataField="KPI_STATUS" HeaderText="KPI_STATUS" />
                                <asp:BoundField DataField="WO_EXT_FRAMEZONE" HeaderText="WO EXT_FRAMEZONE" />
                                <asp:BoundField DataField="WO_OWNER" HeaderText="WO OWNER" />
                                <asp:BoundField DataField="District" HeaderText="District"  />
                                <asp:BoundField DataField="Street" HeaderText="Street" />
                                <asp:BoundField DataField="Building_number" HeaderText="Building Number" />
                                <asp:BoundField DataField="Latitude" HeaderText="Latitude" />
                                <asp:BoundField DataField="Longitude" HeaderText="Longitude" />


                                <asp:TemplateField HeaderText="SR Details" ItemStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="Hyp_detail" Text=""></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Create WO" ItemStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="Hyp_WO" Text=""></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="WO Details" ItemStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="Hyp_WOdetail" Text=""></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dispatch WO">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="Hyp_dispatch" Text=""></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-Width="0" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Hidden_EAMS_SR_Number" Text='<%# Eval("EAMS_SR_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-Width="0"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Hidden_WONUM" Text='<%# Eval("WONUM") %>'></asp:Label>
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


                    </td>
                </tr>
            </table>







        </div>
    </form>
</body>
</html>
