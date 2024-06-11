<%@ Page Title="ECC Owners Detail" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EccOwnerDetails.aspx.cs" Inherits="WebAppForm.EccOwnerDetails" %>

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
                    <asp:Label ID="Label2" runat="server" Text="List of Ecc Owners"></asp:Label>                    
        </p>
        
        <div style="padding-left: 20px;">
            
            <asp:GridView ID="gvEccOwners" runat="server" AutoGenerateColumns="false" PageSize="15" CssClass="Grid" AllowPaging="true"
                DataKeyNames="ALNDOMAINID" OnPageIndexChanging="gvEccOwners_PageIndexChanging" OnRowCancelingEdit="gvEccOwners_RowCancelingEdit" 
                OnRowEditing="gvEccOwners_RowEditing" OnRowUpdating="gvEccOwners_RowUpdating" OnRowDataBound="gvEccOwners_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ALNDOMAINID" HeaderText="Domain ID" ItemStyle-Width="100" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description"   ItemStyle-Width="200" />
                    <asp:BoundField DataField="VALUE" HeaderText="value"  ItemStyle-Width="100" />
                    <asp:CheckBoxField DataField="IsEccOwner" HeaderText="IsEccOwner" ItemStyle-Width="100" />
					<asp:BoundField DataField="Sequence" HeaderText="Sequence" ItemStyle-Width="100" />
                    <asp:CommandField ShowEditButton="true" />
                </Columns>

                <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
            </asp:GridView>
        </div>
        <div>
            <asp:Label ID="lblresult" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
