<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.GISvwBAUComplaintsRainWater"
    Title="CRMS Complaint Detail" CodeBehind="GISvwBAUComplaintsRainWater.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .textfield {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
        }

        .textvalue {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .textfieldalt {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #022130;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
        }

        .textvaluealt {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #022130;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 27px;
            width: 85px;
            border: solid 1px #fff;
            font-size: 11px;
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
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
        SelectCommand="SELECT 
      [Service_Request_Number]
        ,[Application_Status] AS [CRMS_Status]
    
      ,[Service_Request_Type]
      ,[Contact_Reason]
      ,[Customer]
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
      
      ,[EAMS_SR_Number]
      ,[Created_On]
      ,[Modified_On]
      ,[DNMC_Fetch_Date]
      ,[DNMC_Fetch_Flag]    
  FROM [ODW].[GIS].[vwBAUComplaintsRainWater]  where ([Service_Request_Number]= @ID)"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="07102019-000" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <table style="width: 460px; font-family: Segoe UI Light;">
        <tr>
            <td>
                <asp:LinkButton ID="lnkBtnSR" runat="server" Text="Associated SR Details" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White"></asp:LinkButton>
                <asp:LinkButton Style="padding-left: 15px;" ID="lnkBtnWO" runat="server" Text="Associated WO Details" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White"></asp:LinkButton>
                <asp:HiddenField runat="server" ID="hdnbtn" />
                <asp:LinkButton Style="padding-left: 15px;" ID="lnkbtnRefresh" runat="server" Text="Refresh" Font-Names="Segoe UI Light" Font-Size="Small"
                    ForeColor="White" OnClick="lnkbtnRefresh_Click"></asp:LinkButton>
                <asp:LinkButton Style="padding-left: 5px;" ID="lnkbtnCreateWO" runat="server" Text="Create WO" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" OnClick="lnkbtnCreateWO_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="smsBtn" class="simplebutton1" value="MapLink/SMS" /></td>
        </tr>
        <tr>
            <td>
                <asp:DetailsView ID="DetailsView1" Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" BorderColor="#4c636f" runat="server" Height="50px" DataSourceID="SqlDataSource1">

                    <RowStyle CssClass="textfield" />
                    <AlternatingRowStyle CssClass="textfieldalt" />
                </asp:DetailsView>

            </td>
        </tr>
    </table>

    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.js"></script>

    <script type="text/javascript">

        $(function () {

            var popup;
            $('#smsBtn').click(function () {
                var Id = '<%= Request.QueryString["ID"] %>';
                var lat = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(43)").text();
                var lon = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(45)").text();
                var ph = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(11)").text();
                if (ph != "" && !ph.startsWith("974")) {
                    ph = "974" + ph;
                }
                var isWorkOrder = "0";
                debugger;
                popup = window.open("MapsLinkView.aspx?lat=" + lat + "&long=" + lon + "&ph=" + ph + "&Id=" + Id + "&isWorkOrder=" + isWorkOrder, + "Popup", "width=600,height=500");
                popup.focus();
            });

        });

    </script>


</asp:Content>
