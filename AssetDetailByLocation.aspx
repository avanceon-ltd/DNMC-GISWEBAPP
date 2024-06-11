<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetDetailByLocation.aspx.cs" Inherits="WebAppForm.AssetDetailByLocation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Details by Location ID</title>
    <style type="text/css">
        .Grid {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
        }

            .Grid td {
                padding: 5px;
                border: solid 1px #c1c1c1;
            }

            .Grid th {
                padding: 5px 2px;
                color: #fff;
                background: #363670 url(Images/grid-header.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: 0.9em;
            }

            .Grid .alt {
                background: #fcfcfc url(Images/grid-alt.png) repeat-x top;
            }

            .Grid .pgr {
                background: #363670 url(Images/grid-pgr.png) repeat-x top;
            }

                .Grid .pgr table {
                    margin: 3px 0;
                }

                .Grid .pgr td {
                    border-width: 0;
                    padding: 0 6px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .Grid .pgr a {
                    color: Gray;
                    text-decoration: none;
                }

                    .Grid .pgr a:hover {
                        color: #000;
                        text-decoration: none;
                    }

        .textfield {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            border: 1px solid #4c636f;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        a:link {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        /* visited link */
        a:visited {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        /* mouse over link */
        a:hover {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        /* selected link */
        a:active {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .required {
            font-weight: bold;
            padding: 10px 0 10px 5px;
            text-align: right;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 27px;
            width: 90px;
            border: solid 1px #fff;
            font-size: 15px;
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
    <script type="text/javascript" src="Scripts/1.12.4_jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>

    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
            </div>
        </div>
    </div>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">

    <form id="form1" runat="server" style="text-align: center;" autocomplete="off">
        <div>
            <p align="left" style="font-weight: bold; padding-left: 20px; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                <asp:Label ID="Label2" runat="server" Text="Asset Details By Location"></asp:Label>
                <p>
                    <table>
                        <tr>
                            <td class="required" style="text-align: right;">
                                <span class="normal">Search Data Field:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlField1" runat="server" CssClass="textfield" Width="150">
                                    <asp:ListItem Value="ASSETNUM">ASSET NUM</asp:ListItem>
                                    <asp:ListItem Value="EXT_ASSETTAG">ASSET TAG</asp:ListItem>
                                    <asp:ListItem Value="DESCRIPTION">DESCRIPTION</asp:ListItem>
                                    <asp:ListItem Value="STATUS">STATUS</asp:ListItem>

                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="required" style="text-align: right;">
                                <span class="normal">Enter Data Value:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtField1" runat="server" class="textfield" MaxLength="30" Width="135" />
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click1" />
                            </td>
                        </tr>

                    </table>

                </p>


                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
                    <SelectParameters>
                        <asp:QueryStringParameter DbType="String" DefaultValue="10000107" Name="ID" QueryStringField="ID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
                    <SelectParameters>
                        <asp:QueryStringParameter DbType="String" DefaultValue="" Name="ID" QueryStringField="ID" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                    CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                    DataSourceID="SqlDataSource1" OnSelectedIndexChanged="gvCustomers_SelectedIndexChanged" OnRowDataBound="gvCustomers_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ASSETNUM" HeaderText="ASSET NUM" ItemStyle-Width="70" />
                        <asp:BoundField DataField="EXT_ASSETTAG" HeaderText="ASSET TAG" />
                        <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                        <asp:BoundField DataField="STATUS" HeaderText="STATUS" />
                        <asp:BoundField DataField="ASSET_TYPE_DESCRIPTION" HeaderText="ASSET TYPE" />
                        <asp:BoundField DataField="Asset_Maintainer" HeaderText="ASSET MAINTAINER" />
                        <asp:BoundField DataField="Asset_Operator" HeaderText="ASSET OPERATORE" />
                        <asp:BoundField DataField="CHANGEDATE" HeaderText="CHANGE DATE" />

                        <asp:TemplateField HeaderText="SUB-ASSEMBLIES">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkSubAssemblies"></asp:LinkButton>
                                <%-- text='<%# String.IsNullOrEmpty(Eval("ASSETNUM").ToString()) ? "No Details" : "Details" %>' 
                                OnClientClick='<%#String.Format("return AssetInfo({0})", Eval("ASSETNUM")) %>'--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:ButtonField Text="Create WO" CommandName="Select" ControlStyle-CssClass="simplebutton1" Visible="false" />

                    </Columns>

                    <HeaderStyle BackColor="#3a5570" />
                    <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                    <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                </asp:GridView>

                <asp:DataList ID="DataList5" runat="server" DataSourceID="SqlDataSource2">
                    <ItemTemplate>
                        Total Records:
                    <asp:Label ID="label1" Font-Bold="true" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
                    </ItemTemplate>
                </asp:DataList>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function showModal() {
        $("#myModal").modal('show');

    }
    function hideModal() {
        $("#myModal").modal('hide');
    }
    var popup;
    function AssetInfo(dataval,description) {
		popup = window.open("AssetSubAssemblies.aspx?AssetId=" + dataval+"&Description="+description, "Popup", "width=800,height=720");
		popup.focus();
		return false
	}
</script>
