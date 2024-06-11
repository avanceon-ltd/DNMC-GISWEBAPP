<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.LocationsDetail"
    Title="Locations Detail" CodeBehind="LocationsDetail.aspx.cs" %>

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
        SelectCommand="SELECT [CHANGEDATE]
      ,[CLASSSTRUCTUREID]
      ,[DESCRIPTION]
      ,[EXT_CONTRACTID]
      ,[EXT_DATALOADID]
      ,[EXT_DEPARTMENT]
      ,[EXT_LOCATIONTAG]
      ,[LOCATION]
      ,[LOCATIONSID]
      ,[SADDRESSCODE]
      ,[SITEID]
      ,[STATUS]
      ,[TYPE]
      ,[LOC_TYPE_DESCRIPTION]
      ,[ADDRESSCODE]
      ,[CITY]
      ,[DESCRIPTION_SA]
      ,[FORMATTEDADDRESS]
      ,[ORGID]
      ,[DESCRIPTION_LS]
      ,[SYSTEMID_LS]
      ,[TRANSID]
      ,[TRANSSEQ]
      ,[isActive]
      ,[LOC_STATUS_DESCRIPTION]
      ,[LOCPRIORITY_KEY]
      ,[STREETADDRESS]
      ,[STATEPROVINCE]
      ,[HIERARCHYPATH]
      ,[STATEPROVINCE_DESCRIPTION]
  FROM [ODW].[EAMS].[DimLocation] WHERE ([LOCATION] = @ID)"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="DRTOOLS" Name="ID" QueryStringField="ID"  />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:DetailsView ID="DetailsView1" Caption="Locations Detail" CaptionAlign="Top" Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" BorderColor="#4c636f" runat="server" Height="50px"  DataSourceID="SqlDataSource1"  >
       
     <RowStyle CssClass="textfield"  />
        <AlternatingRowStyle CssClass="textfieldalt" />
    </asp:DetailsView>

</asp:Content>
