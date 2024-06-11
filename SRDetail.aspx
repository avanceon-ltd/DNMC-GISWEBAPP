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
       
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="10083111" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>



    <table style="width: 460px; font-family: Segoe UI Light;">
        <tr>
            <td><div style="text-align: left;float:left; font-family: Segoe UI Light; font-size: Small;">Service Request Details
                <div style="width: 100%; padding: 5px; padding-left: 0px;">
            
            <a href='javascript:void(0);' id='lnkimgattch' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px; padding-left: 10px;">Images</a>
        </div>

                </div>

      
                <div style="padding-right:50px; float:right;">
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
    <asp:Label ID="lblMessage" Text="" runat="server" CssClass="textfield" />

    
    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.js"></script>

    <script type="text/javascript">

        $(function () {
             $('#lnkimgattch').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                //  alert(str);
                window.open("GetImgAttachByWo.aspx?IsSR=1&&ID=" + str, "_blank", "width=760,height=500");
            });
            });

    </script>
</asp:Content>
