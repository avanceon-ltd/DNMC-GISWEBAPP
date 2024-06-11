<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.SRDetail"
    Title="SR Detail" CodeBehind="SRDetail.aspx.cs" %>

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
     
        [DESCRIPTION]
        ,[AFFECTEDPERSON]
      ,[AFFECTEDPHONE]
      ,[CHANGEBY]
      ,[CHANGEDATE]
      ,[CLASS]
      
      ,[EXTERNALRECID]
      ,[EXT_CRMSRID]
      ,[EXT_DEPARTMENT]
      ,[ORGID]
      ,[ORIGRECORDCLASS]
      ,[ORIGRECORDID]
      ,[ORIGRECORGID]
      ,[ORIGRECSITEID]
      ,[PLUSSFEATURECLASS]
      ,[REPORTEDPHONE]
      ,[SITEID]
      ,[TICKETID]
      ,[TICKETUID]
      ,[URGENCY]
      ,[ADDRESSLINE2]
      ,[ADDRESSLINE3]
      ,[COUNTY]
      ,[DESCRIPTION_TK]
      ,[LATITUDEY]
      ,[LONGITUDEX]
      ,[ORGID_TK]
      ,[REFERENCEPOINT]
      ,[REGIONDISTRICT]
      ,[SADDRESSCODE]
      ,[SITEID_TK]
      ,[STATEPROVINCE]
      ,[STREETADDRESS]
      ,[TRANSID]
      ,[TRANSSEQ]
      ,[DESCRIPTION_LONGDESCRIPTION]
      ,[ACTUALFINISH]
      ,[ACTUALSTART]
      ,[REPORTDATE]
      ,[STATUSDATE]
      ,[TARGETFINISH]
      ,[TARGETSTART]
      ,[ASSETNUM]
      ,[EXT_DEFECTTYPE]
      ,[EXT_SRGROUP]
      ,[EXT_SRMEMBER]
      ,[EXT_SRTYPE]
      ,[LOCATION]
      ,[REPORTEDBY]
      ,[SOURCE]
      ,[STATUS]
  FROM [ODW].[EAMS].[FactServiceRequest] where ([TICKETID]= @ID)"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="10083111" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>



    <table style="width: 460px; font-family: Segoe UI Light;">
        <tr>
            <td><div style="text-align: left;float:left; font-family: Segoe UI Light; font-size: Small;">Service Request Details</div>

      
                <div style="padding-right:10px;       float:right;">
                <asp:LinkButton ID="lnkbtnback" runat="server" Text="Back" Visible="false" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" /></div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DetailsView ID="DetailsView1" Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" BorderColor="#4c636f" runat="server" Width="450px" Height="50px" DataSourceID="SqlDataSource1">

                    <RowStyle CssClass="textfield" />
                    <AlternatingRowStyle CssClass="textfieldalt" />
                </asp:DetailsView>

            </td>
        </tr>
    </table>


</asp:Content>
