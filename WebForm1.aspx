<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebAppForm.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 243px;
        }

        .auto-style2 {
            width: 263px;
            height: 44px;
        }

        .auto-style3 {
            width: 344px;
        }

        .auto-style4 {
            width: 148px;
        }

        .auto-style5 {
            width: 100%;
        }

        .auto-style6 {
            text-align: center;
            font-size: large;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="1" class="auto-style5">
                <tr>
                    <td class="auto-style1">WO Description</td>
                    <td class="auto-style3">
                        <textarea id="TextArea1" class="auto-style2" name="S1"></textarea></td>
                    <td class="auto-style4">Location</td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" Width="197px">
                            <asp:ListItem>1000012</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Asset</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList2" runat="server" Width="197px">
                            <asp:ListItem>10014447</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">Department</td>
                    <td>DRAINAGE</td>
                </tr>
                <tr>
                    <td class="auto-style1">Work Type</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList3" runat="server" Width="197px">
                            <asp:ListItem>PM</asp:ListItem>
                            <asp:ListItem Selected="True">CM</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">Status</td>
                    <td>WAPPR</td>
                </tr>
                <tr>
                    <td class="auto-style1">Internal Priority</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList4" runat="server" Width="197px">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6" colspan="4"><strong>Service Address</strong></td>
                </tr>
                <tr>
                    <td class="auto-style1">Service Address</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList5" runat="server" Width="197px">
                            <asp:ListItem>1004521</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">Description</td>
                    <td>AHMED BIN ALI STREET</td>
                </tr>
                <tr>
                    <td class="auto-style1">Street Name</td>
                    <td class="auto-style3">AHMED BIN ALI STREET</td>
                    <td class="auto-style4">Municipality</td>
                    <td>Wakra</td>
                </tr>
                <tr>
                    <td class="auto-style1">Building #/Electric #/Water#</td>
                    <td class="auto-style3">23</td>
                    <td class="auto-style4">Zone</td>
                    <td>
                        <asp:DropDownList ID="DropDownList6" runat="server" Width="197px">
                            <asp:ListItem>28</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">District</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList7" runat="server" Width="197px">
                            <asp:ListItem>Abal Hayyat</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">Latitude (Y)</td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server">22.1200000000</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Longitude (X)</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="TextBox2" runat="server">51.5572793400</asp:TextBox>
                    </td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6" colspan="4"><strong>Scheduling Infomration</strong></td>
                </tr>
                <tr>
                    <td class="auto-style1">Schedule Start</td>
                    <td class="auto-style3">
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Content/calendar.png" />
                    </td>
                    <td class="auto-style4">Schedule Finish</td>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Content/calendar.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Actual Start</td>
                    <td class="auto-style3">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Content/calendar.png" />
                    </td>
                    <td class="auto-style4">Actual Finish</td>
                    <td>
                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Content/calendar.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6" colspan="4"><strong>Responsibility</strong></td>
                </tr>
                <tr>
                    <td class="auto-style1">Reported By</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList8" runat="server" Width="197px">
                            <asp:ListItem>DR01</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">SR Reported Date</td>
                    <td>
                        <asp:Image ID="Image5" runat="server" ImageUrl="~/Content/calendar.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">WO Reported Date</td>
                    <td class="auto-style3">
                        <asp:Image ID="Image7" runat="server" ImageUrl="~/Content/calendar.png" />
                    </td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6" colspan="4"><strong>Workflow Related Fields</strong></td>
                </tr>
                <tr>
                    <td class="auto-style1">Engineer</td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList9" runat="server" Width="197px">
                            <asp:ListItem>SGWINSP3</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">Contract Id</td>
                    <td>
                        <asp:DropDownList ID="DropDownList10" runat="server" Width="197px">
                            <asp:ListItem>C/2017/39</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Section</td>
                    <td class="auto-style3">NETWORKS</td>
                    <td class="auto-style4">Crew Lead</td>
                    <td>
                        <asp:DropDownList ID="DropDownList11" runat="server" Width="230px">
                            <asp:ListItem>DCSGWMACECREW4</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Unit</td>
                    <td class="auto-style3">SGW</td>
                    <td class="auto-style4">Inspector</td>
                    <td>
                        <asp:DropDownList ID="DropDownList12" runat="server" Width="197px">
                            <asp:ListItem>SGWINSP3</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Group/Ares</td>
                    <td class="auto-style3">QNORTH</td>
                    <td class="auto-style4">Scheduler</td>
                    <td>
                        <asp:DropDownList ID="DropDownList13" runat="server" Width="231px">
                            <asp:ListItem>DCSGWMACESCHED</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td colspan="2">
                        <asp:Button ID="Button1" runat="server" Text="Create WO" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
