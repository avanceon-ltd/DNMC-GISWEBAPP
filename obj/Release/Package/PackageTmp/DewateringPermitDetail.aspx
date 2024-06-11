<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.DewateringPermitDetail"
    Title="Work Order Detail" CodeBehind="DewateringPermitDetail.aspx.cs" %>

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
      [Service_Request_Id]
      ,[Service_Request_Number]
      ,[Division]
       ,[DW_Contractor]
      ,[DWC_Engineer]
      ,[Application_Stage]
      ,[Application_Status]
      ,[Application_Type]
      ,[Created_On]
      ,[Current_Dewatering_Permit]
      ,[Customer_Requires_Updates]
      ,[Dewatering_Related]
      ,[District]
      ,[Duration_days]
      ,[EAMS_SR_Number]
      ,[Electricity_No]
      ,[Email]
      ,[Engineer]
      ,[Engineer_Comments]
      ,[Permit_SLA_Follow_up_Date]
      ,[Latitude]
      ,[Longitude]
      ,[Mobile_Number]
      ,[Municipality]
      ,[Project_Coordinator_Name]
      ,[Project_Owner_Name]
      ,[Consultant_Approval_Name]
      ,[Main_Contractor_Name]
      ,[Nearest_Land_Mark]
      ,[Outfall]
      ,[Owner]
      ,[Permit_Collected_By]
      ,[Permit_Collected_Date]
      ,[Permit_End_Date]
      ,[Permit_Number]
      ,[Permit_Start_Date]
      ,[PIN]
      ,[Preferred_Method_of_Contact]
      ,[Pro_Dis_MH_No]
      ,[Project_End_Date]
      ,[Project_Name]
      ,[Project_Start_Date]
      ,[Pumping_Zone_Station]
      ,[Quantity_L_S]
      ,[Project_Coordinator_Signature_Date]
      ,[Project_Owner_Signature_Date]
      ,[Consultant_Approval_Signature_Date]
      ,[Engineer_Customer_Service_Signature_Date]
      ,[Main_Contractor_Signature_Date]
      ,[Permit_SLA_Status]
      ,[Street]
      ,[Temp_Permit_Number]
      ,[Type_of_Line]
      ,[Zone]
      ,[Modified_On] FROM [ODW].[CRMS].[DrainageDeWateringPermit] WHERE Service_Request_Number = @ID"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="10032016-0096" Name="ID" QueryStringField="ID"  />
        </SelectParameters>
    </asp:SqlDataSource>
      
    <asp:DetailsView ID="DetailsView1" Caption="Dewatering Permit Detail" CaptionAlign="Top" Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" BorderColor="#4c636f" runat="server" Height="50px" Width="100%" DataSourceID="SqlDataSource1"  >
       
     <RowStyle CssClass="textfield"  />
        <AlternatingRowStyle CssClass="textfieldalt" />
    </asp:DetailsView>

</asp:Content>
