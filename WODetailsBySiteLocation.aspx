<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WODetailsBySiteLocation.aspx.cs" Inherits="WebAppForm.WODetailsBySiteLocation" %>

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
            SelectCommand="SELECT wo.location,wo.EXT_FRAMEZONE,loc.Hierarchical_SiteName, wo.DESCRIPTION,wo.STATUS,wo.WORKTYPE,wo.ASSETNUM, wo.TARGSTARTDATE,wo.TARGCOMPDATE,wo.REPORTEDBY,wo.REPORTDATE,wo.EXT_ENGINEER,wo.CHANGEDATE  
            from [SCADA].[MXLocations] loc  with (NOLOCK)  INNER JOIN
[EAMS].[FactWorkOrder] wo WITH (NOLOCK) ON loc.MXLocation =  wo.LOCATION
WHERE loc.[Facility_Type]=@Facility_Type  AND wo.WORKTYPE = @WORKTYPE and loc.MXLocation IS NOT NULL AND wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC')
 order by wo.CHANGEDATE desc">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="" Name="Facility_Type" QueryStringField="FacilityType" />
                <%--<asp:QueryStringParameter DbType="String" DefaultValue="" Name="WorkType" QueryStringField="WorkType" />--%>
                <asp:ControlParameter ControlID="ddlWorkType" DbType="String" Name="WORKTYPE" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT count(*) AS TotalRows  from [SCADA].[MXLocations] loc  with (NOLOCK)  INNER JOIN [EAMS].[FactWorkOrder] wo WITH (NOLOCK) ON loc.MXLocation =  wo.LOCATION 
            WHERE loc.[Facility_Type]=@Facility_Type and wo.WorkType=@WORKTYPE AND loc.MXLocation IS NOT NULL AND wo.[Status] NOT IN ('CAN','CLOSE','COMP','HISTEDIT','SRVPARRST','THDPTY','WRKCOMP','QC')">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="" Name="Facility_Type" QueryStringField="FacilityType" />
                <%--<asp:QueryStringParameter DbType="String" DefaultValue="" Name="WorkType" QueryStringField="WorkType" />--%>
                <asp:ControlParameter ControlID="ddlWorkType" DbType="String" Name="WORKTYPE" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <table>

            <tr>
                <td style="text-align: left;" class="required">
                    <span>Work Type Filter: </span>
                    <asp:DropDownList ID="ddlWorkType" runat="server" AutoPostBack="true" CssClass="textfield" Width="186px" OnSelectedIndexChanged="ddlWorkType_SelectedIndexChanged">
                        <asp:ListItem Value="All">1- All</asp:ListItem>
                        <asp:ListItem Value="FR">2- FR</asp:ListItem>
                        <asp:ListItem Value="CM" Selected="True">3- CM</asp:ListItem>
                        <asp:ListItem Value="PM">4- PM</asp:ListItem>
                         <asp:ListItem Value="PM,CM">5- PM, CM</asp:ListItem>
                    </asp:DropDownList>

                </td>
            </tr>
            <%--<tr>
                <td>
                    <table>
                        <tr>
                            <td class="required" style="text-align: right;">
                                <span class="normal">Search Data Field:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="textfield" Width="150">
                                    <asp:ListItem Value="WONUM">WO NUM</asp:ListItem>
                                    <asp:ListItem Value="WODescription">WODescription</asp:ListItem>
                                    <asp:ListItem Value="Status_Description">Status_Description</asp:ListItem>
                                    <asp:ListItem Value="ASSETNUM">ASSET NUM</asp:ListItem>
                                    <asp:ListItem Value="Asset_Description">Asset Description</asp:ListItem>
                   
                                </asp:DropDownList></td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtSearch" runat="server" class="textfield" MaxLength="30" Width="150" BackColor="White" ForeColor="Black" />
                            </td>

                            <td>
                                <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" />
                            </td>

                        </tr>
                    </table>

                    <br />
                </td>
            </tr>--%>

            <tr>
                <td>


                    <asp:GridView ID="gvWO" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                        CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvWO_PageIndexChanging"
                        DataSourceID="SqlDataSource1" ViewStateMode="Enabled">
                        <Columns>
                            <asp:BoundField DataField="Location" HeaderText="MX Location" ItemStyle-Width="70" />
                            <asp:BoundField DataField="EXT_FRAMEZONE" HeaderText="Framework Zone" />
                            <asp:BoundField DataField="Hierarchical_SiteName" HeaderText="Site Name" />
                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                            <asp:BoundField DataField="STATUS" HeaderText="Status" />
                            <asp:BoundField DataField="WORKTYPE" HeaderText="WO Type" ItemStyle-Width="70" />
                            <asp:BoundField DataField="ASSETNUM" HeaderText="Asset No." />
                            <asp:BoundField DataField="TARGSTARTDATE" HeaderText="Target Start Date" />
                            <asp:BoundField DataField="TARGCOMPDATE" HeaderText="Target End Date" />
                            <asp:BoundField DataField="REPORTEDBY" HeaderText="Reported By" />
                            <asp:BoundField DataField="REPORTDATE" HeaderText="Date Reported" />
                            <asp:BoundField DataField="EXT_ENGINEER" HeaderText="Engineer Name" />
                            <asp:BoundField DataField="CHANGEDATE" HeaderText="Date Changed" />
                            
                        </Columns>
                        <HeaderStyle BackColor="#3a5570" />
                        <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                        <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                    </asp:GridView>

                    <asp:DataList ID="dlPager" runat="server" DataSourceID="SqlDataSource2">
                        <ItemTemplate>
                            Total Records:
                    <asp:Label ID="lblRecordCount" Font-Bold="true" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
                        </ItemTemplate>
                    </asp:DataList>

                </td>
            </tr>
        </table>

    </form>
</body>
</html>
