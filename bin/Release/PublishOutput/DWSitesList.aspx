<%@ Page Title="SCADA Bridge Site Table" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DWSitesList.aspx.cs" Inherits="WebAppForm.DWSitesList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <style type="text/css">
        .Grid {
			/*background-color: #fff;*/
			margin: 5px 0 10px 0;
			border: solid 1px #e3d9d969;
			border-collapse: collapse;
			font-family: Calibri;
			/*color: #474747;*/
		}

            .Grid td {
                padding: 5px;
                /*border: solid 1px #c1c1c1;*/
            }

            .Grid th {
                padding: 5px 2px;
                color: #fff;
                background: #3a5570 url(Images/grid-header.png) repeat-x top;
                /*border-left: solid 1px #525252;*/
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
                    /*border-left: solid 1px #666;*/
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

                    .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 35px;
            width: 110px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
        }
        .btnDisabled{
            color: grey;
            background-color: #154670;
            height: 35px;
            width: 110px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
            cursor: not-allowed;
        }
            .simplebutton1:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }
    </style>
    <div>
         <p align="left" style="font-weight: bold; padding-left:20px; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                    <asp:Label ID="Label2" runat="server" Text="List of Dewatering Sites in SCADA"></asp:Label>                    
        </p>
        
        <div style="padding-left: 20px;">
            <asp:Button ID="btnAdd" runat="server" CssClass="simplebutton1" Enabled="true" Text="Add Site" PostBackUrl="" OnClick="btnAdd_Click" />
            <asp:GridView ID="gvDWSites" runat="server" AutoGenerateColumns="false" PageSize="15" CssClass="Grid" AllowPaging="true"
                DataKeyNames="SCADA_SiteName" OnPageIndexChanging="gvDWSites_PageIndexChanging" OnRowCancelingEdit="gvDWSites_RowCancelingEdit" 
                OnRowDeleting="gvDWSites_RowDeleting" OnRowEditing="gvDWSites_RowEditing" OnRowUpdating="gvDWSites_RowUpdating" OnRowDataBound="gvDWSites_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="SCADA_SiteName" HeaderText="Site Name" ReadOnly="true" ItemStyle-Width="100" />
                    <asp:BoundField DataField="PERMIT_NO" HeaderText="Permit No." ItemStyle-Width="200" />
                    <asp:BoundField DataField="IP_ADDRESS" HeaderText="IP Address" ItemStyle-Width="100" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-Width="100" />
                    <asp:CommandField ShowEditButton="true" />
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Button" />
                </Columns>

                <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light " />
            </asp:GridView>
        </div>
        <div>
            <asp:Label ID="lblresult" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
