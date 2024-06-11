<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.VehicleDetail"
    Title="Vehicle Detail" CodeBehind="VehicleDetail.aspx.cs" %>

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
        SelectCommand="SELECT [Title]
      ,[Plate_Number]
        ,[Movement_Status]
         ,[driverMobile]
        ,[IMEI_Number]
      ,[Vehicle_Details]
      ,[Brand]
      ,[Model]
      ,[Latitude]
      ,[Longitude]
      ,[Digital_Input_1]
      ,[Speed]
      ,[Driver_Name]
      ,[Angle]
      ,[LastUpdateTimeStamp]
      ,[Last_Update_Timestamp]
      ,[Last_Update]
      ,[LastUpdate]
      ,[Odometer]
      ,[Initial_Odometer]
      ,[Duration_Since_Last_Record]
      ,[Php_Timezone]
      ,[Avls_Status]
      
      ,[Movement_Status_Code]
      ,[Offset]
      ,[PlateNumber]
      ,[Lat]
      ,[Lng]
      ,[Last_Location]
      ,[LastRecordGMT]
      ,[Last_Record_GMT]
      ,[Driver]
      ,[Department]
      ,[Zone]
      ,[Section]
      ,[Contractor]
      ,[VehicleType]
      ,[Date_Created]     
  FROM [ODW].[GIS].[vwVehicleInfo] WHERE (Title = @ID)"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="8737290" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <div style="width: 460px; font-family: Segoe UI Light;">

        <div style="text-align: left; font-family: Segoe UI Light; font-size: Small;">Vehicle Details</div>
        <div style="width: 100%; padding: 3px; padding-left: 0px;">
            <a href='javascript:void(0);' id='AssoWOPopUp' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Associated WorkOrders</a>
            <asp:LinkButton style="padding-left:15px;" ID="lnkbtnRefresh" runat="server" Text="Refresh" Font-Names="Segoe UI Light" Font-Size="Small" 
                    ForeColor="White" OnClick="lnkbtnRefresh_Click"></asp:LinkButton>
        </div>

        <asp:DetailsView ID="DetailsView1"  CellPadding="3" CellSpacing="3"
            BorderColor="#4c636f" runat="server" Height="50px" DataSourceID="SqlDataSource1">
            <RowStyle CssClass="textfield" />
            <AlternatingRowStyle CssClass="textfieldalt" />
        </asp:DetailsView>
    </div>

        <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
     <script type="text/javascript">

        $(function () {

            $('#AssoWOPopUp').click(function () {
                var strWO = '<%= Request.QueryString["ID"] %>';
                var strUser = '<%= Request.QueryString["USER"] %>';
                    //  alert(str);
                window.open("AssociatedWorkOrders.aspx?Title=" + strWO + "&USER=" + strUser , "Popup", "width=800,height=200");
                });

        });

    </script>
</asp:Content>
