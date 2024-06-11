<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.CRMSComplaintDetail"
    Title="CRMS Complaint Detail" CodeBehind="CRMSComplaintDetail.aspx.cs" %>

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
    </style>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
        SelectCommand="SELECT 
      [Service_Request_Number]
        ,[Application_Status]
     ,[Division]
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
  FROM [ODW].[GIS].[vwComplaintsExternalFlooding] where ([Service_Request_Number]= @ID)"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="07102019-000" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <table style="width: 460px; font-family: Segoe UI Light;">

        <tr>
            <td><asp:LinkButton ID="lnkBtnSR" runat="server" Text="Associated SR Details" Font-Names="Segoe UI Light" Font-Size="Small"  ForeColor="White"></asp:LinkButton>
                <asp:LinkButton style="padding-left:15px;" ID="lnkBtnWO" runat="server" Text="Associated WO Details" Font-Names="Segoe UI Light" Font-Size="Small"  ForeColor="White"></asp:LinkButton>
                <asp:HiddenField runat="server" ID="hdnbtn" />
                <asp:LinkButton style="padding-left:15px;" ID="lnkbtnRefresh" runat="server" Text="Refresh" Font-Names="Segoe UI Light" Font-Size="Small" 
                    ForeColor="White" OnClick="lnkbtnRefresh_Click"></asp:LinkButton>
            </td>
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


</asp:Content>
