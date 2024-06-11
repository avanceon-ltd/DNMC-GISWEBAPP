<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetSubAssemblies.aspx.cs" Inherits="WebAppForm.ChambersAssetInfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Sub-Assemblies</title>
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

		table caption {
			font-weight: bold;
			font-size: 20px;
			font-family: 'Segoe UI Light', Arial;
			margin-bottom:5px;
            text-align:left;
		}
	</style>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">
	<%--<p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    Chamber: Sub-Assemblies <br />
                </p>--%>
	<form id="form1" runat="server" style="text-align: center;">

		<asp:SqlDataSource ID="SqlDataSourceChambersAsset" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
			<SelectParameters>
				<asp:QueryStringParameter DbType="String" Name="AssetId" QueryStringField="AssetId" />
			</SelectParameters>
		</asp:SqlDataSource>

		<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
			<SelectParameters>
				<asp:QueryStringParameter DbType="String" DefaultValue="" Name="AssetId" QueryStringField="AssetId" />
			</SelectParameters>
		</asp:SqlDataSource>

		<asp:GridView ID="gvChambersAsset" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
			CellPadding="10" CellSpacing="10" PageSize="20" AllowPaging="true" 
			DataSourceID="SqlDataSourceChambersAsset">
			<Columns>
				<asp:BoundField DataField="ASSETNUM" HeaderText="ASSET NUM" ItemStyle-Width="70" />
				<asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
				<asp:BoundField DataField="LocationID" HeaderText="LOCATION ID" />
				<asp:BoundField DataField="LocationDescription" HeaderText="LOCATION DESCRIPTION" />
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
		<script type="text/javascript">
</script>

	</form>
</body>
</html>
