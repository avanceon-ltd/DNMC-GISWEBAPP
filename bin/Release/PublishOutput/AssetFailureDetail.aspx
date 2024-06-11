<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.AssetFailureDetail" Title="AssetFailure Detail" CodeBehind="AssetFailureDetail.aspx.cs" %>

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

        .popupbtn {
            color: #fff;
            background-color: #154670;
            height: 23px;
            width: 62px;
            border: solid 1px #fff;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            border: 1px solid #4c636f;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
            cursor: pointer;
        }


            .popupbtn:hover {
                background-color: #3498db;
                border: solid 1px #4c636f;
            }
    </style>

    <div style="width: 460px; font-family: Segoe UI Light;">

        <div style="text-align: left; font-family: Segoe UI Light; font-size: Small;">AssetFailure Detail</div>


        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT [UID]
      ,[Description]
      ,[Lat]
      ,[Long]
      ,[Status]
      ,[Remarks]
      ,[ModifiedDate]
      ,[CreatedDate]
      ,[MAst_UID]
      ,[loggedinUser]
  FROM [ODW].[GIS].[AssetFailure] where MAst_UID=@ID"
            UpdateCommand="UPDATE [GIS].[AssetFailure]  SET  [Status] = @Status ,[ModifiedDate] =@ModifiedDate,loggedinUser=@loggedinUser  WHERE MAst_UID=@MAst_UID"
            OnUpdating="SqlDataSource1_Updating"
            >
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="1" Name="ID" QueryStringField="ID" />
            
            </SelectParameters>

            <UpdateParameters>
                <asp:Parameter Name="MAst_UID" DbType="String" />
                <asp:Parameter Name="Status" Type="String"  />
                <asp:Parameter Name="ModifiedDate" Type="DateTime" />
                <asp:Parameter Name="loggedinUser" Type="String" />

            </UpdateParameters>
        </asp:SqlDataSource>

        <asp:DetailsView ID="DetailsView1" CaptionAlign="Top" AutoGenerateRows="False" Font-Name="Segoe UI Light" Style="min-width: 200px;" Font-Bold="true" Font-Size="18px" CellPadding="3"
            CellSpacing="3" BorderColor="#4c636f" DataKeyNames="MAst_UID" runat="server" Height="50px" Width="450px" DataSourceID="SqlDataSource1">


            <Fields>
         
                 <asp:BoundField DataField="MAst_UID" HeaderText="UID" ReadOnly="true" />
                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true" />
                <asp:BoundField DataField="Lat" HeaderText="Lat" ReadOnly="true" />
                <asp:BoundField DataField="Long" HeaderText="Long " ReadOnly="true" />
                <asp:BoundField DataField="loggedinUser" HeaderText="UserName " ReadOnly="true" />
                <asp:BoundField DataField="ModifiedDate" HeaderText="Modified Date" ReadOnly="true" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ReadOnly="true" />
                <asp:TemplateField HeaderText="Status">



                    <EditItemTemplate>
                        <asp:DropDownList ID="IdDDL" runat="server" CssClass="textvalue" SelectedValue='<%# Bind("Status") %>'>

                           
                            <asp:ListItem Text="Unattended" Value="Unattended"></asp:ListItem>
                            <asp:ListItem Text="InProgress" Value="InProgress"></asp:ListItem>
                            <asp:ListItem Text="Resolved" Value="Resolved"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>



                    <ItemTemplate>
                        <asp:Label ID="LblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" CssClass="popupbtn" />
                        &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" CssClass="popupbtn" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" CssClass="popupbtn" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>



            <RowStyle CssClass="textfield" />
            <AlternatingRowStyle CssClass="textfieldalt" />
        </asp:DetailsView>
    </div>




</asp:Content>
