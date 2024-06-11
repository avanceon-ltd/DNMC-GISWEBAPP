<%@ Page Title="SCADA Bridge Site Table" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WetWellAlarmSummary.aspx.cs" Inherits="WebAppForm.WetWellAlarmSummary" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="refresh" content="10;" />
    <style type="text/css">
        .Grid {
            /*background-color: #fff;*/
            margin: 5px 0 10px 0;
            border: solid 1px #e3d9d969;
            border-collapse: collapse;
            font-family: Calibri;
            /*color: #474747;*/
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

            .Grid td {
                padding: 3px;
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
            width: 110px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
        }

        .btnDisabled {
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
    <div style="width: 1024px">
        <p align="center" style="font-weight: bold; padding-left: 20px; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
            <asp:Label ID="Label2" runat="server" Text="Pumping Stations Alarm Summary"></asp:Label>
        </p>
        <span class="normal" style="font-weight: bold;">FrameZone:</span>
        <asp:DropDownList ID="ddlFrameZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFrameZone_SelectedIndexChanged" CssClass="textfield">
            <%--<asp:ListItem Text="All" Value="All"></asp:ListItem>            
            <asp:ListItem Text="Qatar North" Value="Qatar North"></asp:ListItem>
            <asp:ListItem Text="Qatar South" Value="Qatar South"></asp:ListItem>
            <asp:ListItem Text="Qatar West" Value="Qatar West"></asp:ListItem>
            <asp:ListItem Text="NA" Value="NA"></asp:ListItem>
            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>--%>
        </asp:DropDownList>
        <span class="normal" style="font-weight: bold; margin-left:20px;">Facility Type:</span>
        <asp:DropDownList ID="ddlFacilityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFacilityType_SelectedIndexChanged" CssClass="textfield">
            <%--<asp:ListItem Text="All" Value="All"></asp:ListItem>            
            <asp:ListItem Text="FW-PS" Value="FW-PS"></asp:ListItem>
            <asp:ListItem Text="SGW-PS" Value="SGW-PS"></asp:ListItem>
            <asp:ListItem Text="SGW-UP-PS" Value="SGW-UP-PS"></asp:ListItem>
            <asp:ListItem Text="TWN-PS" Value="TWN-PS"></asp:ListItem>
            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>--%>
        </asp:DropDownList>
        <div style="padding-left: 1px;">
            <div style="display: flex; justify-content: space-between;">
                <h3 style="font-size:14px">Date & Time: <% =DateTime.Now.ToString() %></h3>
                <asp:Button ID="btnRefresh" runat="server" CssClass="simplebutton1" Enabled="true" Text="Refresh" OnClick="btnRefresh_Click" />                
            </div>    
            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:DataList ID="dlHigh" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" CssClass="Grid">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <h3 style="margin: 0px;">Wetwell High Level Alarm</h3>                                           
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; width: 100px;">Station Name</td>
                                        <td style="border: 1px solid; width: 100px; text-align: center;">Pump Running</td>
                                        <td style="border: 1px solid; width: 50px; text-align: center;">PV1</td>
                                        <td style="border: 1px solid; width: 50px; text-align: center;">PV2</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                            <AlternatingItemStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light " />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 100px;"><%# Eval("StationName") %></td>
                                        <td style="width: 100px; text-align: center;"><%# Eval("PumpRunning") %></td>
                                        <td style="width: 50px; text-align: center;"><%# string.Format("{0:0.00}", Eval("PV1")) %></td>
                                        <td style="width: 50px; text-align: center;"><%# string.Format("{0:0.00}", Eval("PV2")) %></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle BackColor="#3a5570" />
                        </asp:DataList>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DataList ID="dlHighHigh" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" CssClass="Grid">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <h3 style="margin: 0px;">Wetwell High High Level Alarm</h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; width: 100px;">Station Name</td>
                                        <td style="border: 1px solid; width: 100px; text-align: center;">Pump Running</td>
                                        <td style="border: 1px solid; width: 50px; text-align: center;">PV1</td>
                                        <td style="border: 1px solid; width: 50px; text-align: center;">PV2</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                            <AlternatingItemStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light " />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 100px;"><%# Eval("StationName") %></td>
                                        <td style="width: 100px; text-align: center;"><%# Eval("PumpRunning") %></td>
                                        <td style="width: 50px; text-align: center;"><%# string.Format("{0:0.00}", Eval("PV1")) %></td>
                                        <td style="width: 50px; text-align: center;"><%# string.Format("{0:0.00}", Eval("PV2")) %></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle BackColor="#3a5570" />
                        </asp:DataList>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:DataList ID="dlOverflow" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" CssClass="Grid">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <h3 style="margin: 0px;">Wetwell Overflow Level Alarm</h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; width: 100px;">Station Name</td>
                                        <td style="border: 1px solid; width: 100px; text-align: center;">Pump Running</td>
                                        <td style="border: 1px solid; width: 50px; text-align: center;">PV1</td>
                                        <td style="border: 1px solid; width: 50px; text-align: center;">PV2</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                            <AlternatingItemStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light " />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 100px;"><%# Eval("StationName") %></td>
                                        <td style="width: 100px; text-align: center;"><%# Eval("PumpRunning") %></td>
                                        <td style="width: 50px; text-align: center;"><%# string.Format("{0:0.00}", Eval("PV1")) %></td>
                                        <td style="width: 50px; text-align: center;"><%# string.Format("{0:0.00}", Eval("PV2")) %></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle BackColor="#3a5570" />
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Label ID="lblresult" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
