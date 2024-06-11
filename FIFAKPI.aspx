<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.FIFAKPI"
    Title="Ashgal FIFAKPI" CodeBehind="FIFAKPI.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <link href="Styles/App.css" rel="stylesheet" />
    <style>
        
        table{
            width: 630px;
            height: 90px;
            border: 0.01em solid #4c636f !important;
            border-collapse: collapse;
        }

        .KPI {
            width: 16.6%;
        }

        .KPIValue {
            font-weight: bold;
            font-size: 30px;
            margin-left: 12px;
            margin-bottom: 0px;
        }

        .KPIName {
            font-weight: bold;
            font-size: 16px;
            margin-left: 10px;
            margin-bottom: 0px;
        }

        .Main-div {
            background-color: black;
        }
        .lblTitle{
            font-weight: bold; 
            font-size: 18px; 
            font-family: 'Segoe UI Light', Arial; 
            margin-bottom: 0; 
            display: flow-root;
        }
        
    </style>
    <script type="text/javascript" src="Scripts/1.12.4_jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <meta http-equiv="Refresh" content="30" />

    <div class="Main-div">
        <table>
            <tr>
                <td colspan="6">
                    <asp:Label ID="lblTitle" class="lblTitle" align="center" runat="server" />
                </td>
            </tr>
            <tr style="background-color: #022130">

                <td class="KPI" style="border-right: 0.01em solid black;">
                    <asp:Label class="KPIValue" ID="Unassigned" runat="server" />
                    <p class="KPIName">Unassigned</p>
                </td>
                <td class="KPI" style="border-right: 0.01em solid black;">
                    <asp:Label class="KPIValue" ID="Assigned" runat="server" />
                    <p class="KPIName">Assigned </p>
                </td>
                <td class="KPI" style="border-right: 0.01em solid black;">
                    <asp:Label class="KPIValue" ID="Unattended" runat="server" />
                    <p class="KPIName">Unattended </p>
                </td>
                <td class="KPI" style="border-right: 0.01em solid black;">
                    <asp:Label class="KPIValue" ID="InProgress" runat="server" />
                    <p class="KPIName">In Progress </p>
                </td>
                <td class="KPI" style="border-right: 0.01em solid black;">
                    <asp:Label class="KPIValue" ID="WorkComp" runat="server" />
                    <p class="KPIName">Work Comp. </p>
                </td>
                <td class="KPI" style="border-left: 0.01em solid black;">
                    <asp:Label class="KPIValue" ID="Resolved" runat="server" />
                    <p class="KPIName">Resolved </p>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="height: 3px"></td>
            </tr>
        </table>
    </div>

</asp:Content>
