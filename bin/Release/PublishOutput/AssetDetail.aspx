<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.AssetDetail"
	Title="Asset Detail" CodeBehind="AssetDetail.aspx.cs" %>

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

		table caption {
			font-weight: bold;
			font-size: 20px;
			font-family: 'Segoe UI Light', Arial;
			margin-bottom: 5px;
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
		/* DivTable.com */
		.divTable {
			display: table;
			width: 100%;
		}

		.divTableRow {
			display: table-row;
		}

		.divTableHeading {
			background-color: #EEE;
			display: table-header-group;
		}

		.divTableCell, .divTableHead {
			display: table-cell;
			padding: 3px 10px;
		}

		.divTableHeading {
			background-color: #EEE;
			display: table-header-group;
			font-weight: bold;
		}

		.divTableFoot {
			background-color: #EEE;
			display: table-footer-group;
			font-weight: bold;
		}

		.divTableBody {
			display: table-row-group;
		}
	</style>
	<div class="divTable">
		<div class="divTableBody">
			<div class="divTableRow">
				<div class="divTableCell">
					<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
						SelectCommand="SELECT 
								   [ASSETNUM]   as 'ASSETNUM'
								  ,[EXT_ASSETTAG]  as 'ASSET TAG' 	  
								  ,[DESCRIPTION] 	
								  ,[STATUS]
                                  ,[EXT_FRAMEZONE]
								  ,[ASSET_TYPE_DESCRIPTION]  as 'ASSET TYPE DESCRIPTION' 
									,[ASSET_CONDITION_DESCRIPTION] as 'CONDITION'
								  ,(SELECT [NAME] FROM [ODW].[EAMS].[DimCompany] where COMPANY_KEY=[MAINTAINER_KEY]) As 'Asset Maintainer'
								  ,(SELECT [NAME] FROM [ODW].[EAMS].[DimCompany] where COMPANY_KEY=[OPERATOR_KEY]) As 'Asset Operator'
									,AREA_DESCRIPTION
								  ,[SECTION_DESCRIPTION]
								  ,[UNIT_DESCRIPTION]
									,CHANGEBY
								  ,[CHANGEDATE]     
							  FROM [ODW].[EAMS].[vwDimAsset] where ASSETNUM =@ID"
						OnSelecting="SqlDataSource1_Selecting">
						<SelectParameters>
							<asp:QueryStringParameter DefaultValue="" Name="ID" QueryStringField="ID" Type="Object" />
						</SelectParameters>
					</asp:SqlDataSource>

					<asp:DetailsView ID="DetailsView1" Caption="Asset Detail" CaptionAlign="Top" Width="350px"
						Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3"
						CellSpacing="3" BorderColor="#4c636f" runat="server" DataSourceID="SqlDataSource1">

						<RowStyle CssClass="textfield" />
						<AlternatingRowStyle CssClass="textfieldalt" />
					</asp:DetailsView>

					<asp:SqlDataSource ID="SqlDataSourceAssetSpecification" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
						<SelectParameters>
							<asp:QueryStringParameter DbType="String" DefaultValue="10000107" Name="ID" QueryStringField="ID" />
						</SelectParameters>
					</asp:SqlDataSource>

					<asp:GridView ID="gvAssetSpecification" Font-Size="12px" Font-Names="Segoe UI Light" Font-Bold="true" runat="server" AutoGenerateColumns="false" AllowSorting="false"
						CellPadding="4" CellSpacing="5" PageSize="20" Caption="Asset Specification" CaptionAlign="Top"
						DataSourceID="SqlDataSourceAssetSpecification" Width="350px">
						<Columns>
							<asp:BoundField DataField="ATTDESC" HeaderText="Description" />
							<asp:BoundField DataField="AssetValue" HeaderText="Value" />
						</Columns>
						<%--<EmptyDataTemplate>No Specification Available</EmptyDataTemplate>--%>
						<HeaderStyle BackColor="#3a5570" />
						<RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
						<AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
					</asp:GridView>

				</div>
				<div class="divTableCell">
					<asp:SqlDataSource ID="SqlDataSourceChambersAsset" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
						<SelectParameters>
							<asp:QueryStringParameter DbType="String" Name="ID" QueryStringField="ID" />
						</SelectParameters>
					</asp:SqlDataSource>

					<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>">
						<SelectParameters>
							<asp:QueryStringParameter DbType="String" DefaultValue="" Name="ID" QueryStringField="ID" />
						</SelectParameters>
					</asp:SqlDataSource>

					<asp:GridView ID="gvChambersAsset" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
						CellPadding="3" CellSpacing="3" PageSize="30" AllowPaging="true" Caption="Sub-Assemblies" CaptionAlign="Top" Width="600px"
						DataSourceID="SqlDataSourceChambersAsset" OnDataBound="gvChambersAsset_DataBound">
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

					<asp:DataList ID="DataList5" runat="server" DataSourceID="SqlDataSource2" Visible="false">
						<ItemTemplate>
							Total Records:
							<asp:Label ID="label1" Font-Bold="true" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
						</ItemTemplate>
					</asp:DataList>
				</div>
			</div>
		</div>
	</div>


</asp:Content>
