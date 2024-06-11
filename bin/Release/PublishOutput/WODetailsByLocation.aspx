<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WODetailsByLocation.aspx.cs" Inherits="WebAppForm.WODetailsByLocation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Get Work Orders Details By Location</title>
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
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">

    <form id="form1" runat="server" style="text-align: center;">

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT wo.WONUM,wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,wo.ASSETNUM,wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTEDBY,wo.REPORTDATE,wo.EXT_ENGINEER,wo.CHANGEDATE
                FROM [EAMS].[FactWorkOrder] wo where WORKTYPE in (@WType) and wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC') 
                AND location=(SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName = @hSiteName) order by changedate desc">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="" Name="hSiteName" QueryStringField="hSiteName" />
                <asp:ControlParameter ControlID="drpDwnWType" DbType="String" Name="WType" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT Count(*) AS TotalRows FROM [EAMS].[FactWorkOrder] wo where WORKTYPE in (@WType) and 
            wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC') and location=(SELECT TOP(1) MXLocation FROM [SCADA].[MXLocations] WHERE Hierarchical_SiteName = @hSiteName)">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="" Name="hSiteName" QueryStringField="hSiteName" />
                <asp:ControlParameter ControlID="drpDwnWType" DbType="String" Name="WType" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <table>

            <tr>
                <td style="text-align: left;" class="required">
                    <span>Work Type Filter: </span>
                    <asp:DropDownList ID="drpDwnWType" runat="server" AutoPostBack="true" CssClass="textfield" Width="186px">
                        <asp:ListItem Value="ALL" Selected="True">1- All</asp:ListItem>
                        <asp:ListItem Value="FR,CM">2- FR,CM</asp:ListItem>
                        <asp:ListItem Value="FR">3- FR</asp:ListItem>
                        <asp:ListItem Value="CM">4- CM</asp:ListItem>
                    </asp:DropDownList>
                    <span>&nbsp;&nbsp;&nbsp;WO Status: </span>
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" CssClass="textfield" Width="186px">
                        <%--<asp:ListItem Value="ALL" Selected="True">1- All</asp:ListItem>--%>
                        <%--<asp:ListItem Value="DISPATCH">DISPATCH</asp:ListItem>
                        <asp:ListItem Value="INPRG">INPRG</asp:ListItem>
                        <asp:ListItem Value="NOTCOMP">NOTCOMP</asp:ListItem>
                        <asp:ListItem Value="RETURNED">RETURNED</asp:ListItem>
                        <asp:ListItem Value="RTNCRWLD">RTNCRWLD</asp:ListItem>
                        <asp:ListItem Value="WAPPR">WAPPR</asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="required" style="text-align: right;">
                                <span class="normal">Search Data Field:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlField1" runat="server" CssClass="textfield" Width="150">
                                    <asp:ListItem Value="WONUM">WO Number</asp:ListItem>
                                    <asp:ListItem Value="DESCRIPTION">Description</asp:ListItem>
                                    <%--<asp:ListItem Value="STATUS">Status</asp:ListItem>--%>
                                    <asp:ListItem Value="ASSETNUM">Asset Number</asp:ListItem>
                                    <asp:ListItem Value="EXT_ENGINEER">Engineer Name</asp:ListItem>                   
                                </asp:DropDownList></td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtField1" runat="server" class="textfield" MaxLength="30" Width="150"  />
                            </td>

                            <td>
                                <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" />
                            </td>

                        </tr>
                    </table>

                    <br />
                </td>
            </tr>

            <tr>
                <td>


                    <asp:GridView ID="gvWO" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                        CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnRowDataBound="gvWO_RowDataBound" OnPageIndexChanging="gvWO_PageIndexChanging"
                        DataSourceID="SqlDataSource1">
                        <Columns>
                            <asp:BoundField DataField="WONUM" HeaderText="WO NUM" ItemStyle-Width="70" />
                            <asp:BoundField DataField="DESCRIPTION" HeaderText="WO Description" />
                            <%--<asp:BoundField DataField="Status_Description" HeaderText="Status Description" />--%>
                            <asp:BoundField DataField="STATUS" HeaderText="WO Status" />
                            <asp:BoundField DataField="WORKTYPE" HeaderText="WORK TYPE" />
                            <asp:BoundField DataField="ASSETNUM" HeaderText="ASSET NUM" ItemStyle-Width="70" />
                            <%--<asp:BoundField DataField="Asset_Description" HeaderText="Asset Description" />--%>
                            <asp:BoundField DataField="TARGSTARTDATE" HeaderText="Target Start Date" />
                            <asp:BoundField DataField="TARGCOMPDATE" HeaderText="Target End Date" />
                            <asp:BoundField DataField="REPORTEDBY" HeaderText="REPORTED BY" />
                            <asp:BoundField DataField="REPORTDATE" HeaderText="Report Date" />
                            <asp:BoundField DataField="EXT_ENGINEER" HeaderText="Engineer Name" />
                            <asp:BoundField DataField="CHANGEDATE" HeaderText="CHANGE DATE" />
                            <asp:TemplateField HeaderText="Attachments" ItemStyle-Width="70">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="WOAssetHyp" Text=""></asp:HyperLink>
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
    </form>
</body>
</html>
