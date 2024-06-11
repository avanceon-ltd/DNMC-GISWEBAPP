<%@ Page Title=" ICCS Address Book" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ICCSList.aspx.cs" Inherits="WebAppForm.ICCSList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        .Grid {
            background-color: #022130;
            margin: 5px 0 10px 0;
            border: solid 1px #c1c1c1;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
            white-space: nowrap;
            padding: 5px;
            width: fit-content;
        }

            .Grid td {
                padding: 5px;
                border: solid 1px #c1c1c1;
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
            width: fit-content;
            border: solid 1px #fff;
            font-size: 12px;
            font-weight: bold;
            -webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
            padding: 5px;
        }

        .searchbtn {
            color: #fff;
            background-color: #154670;
            height: 30px;
            width: 100px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            -webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
        }

        .btnDisabled {
            color: grey;
            background-color: #154670;
            height: 35px;
            width: 110px;
            border: solid 1px #fff;
            font-size: 15px;
            font-weight: bold;
            -webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2x 2px;
            border-radius: 2px 2px 2px 2px;
            cursor: not-allowed;
        }

        .simplebutton1:hover {
            background-color: #3498db;
            border: solid 1px #fff;
        }

        .searchbtn:hover {
            background-color: #3498db;
            border: solid 1px #fff;
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

        .Header {
            text-align: center;
            width: auto;
            height: 35px;
        }

        .Item {
            width: 25%;
            text-align: center;
        }

        .Delete {
            color: red;
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
            margin: 5px;
        }

        .Type {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            border: 1px solid #4c636f;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            margin: 5px;
            text-align: center;
        }

        .Total {
            font-weight: bold;
        }
    </style>
    <script type="text/javascript" src="Scripts/1.12.4_jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <div>
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog modal-sm">
                <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                    <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                        <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">ICCS: MessageBox</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                        <asp:Button ID="Button1" CssClass="popupbtn" runat="server" Text="OK" OnClientClick="BindData" />
                    </div>
                </div>
            </div>
        </div>

        <p align="center" style="font-weight: bold; padding-left: 20px; padding-top: 20px; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
            <asp:Label ID="Label1" runat="server" Text="ICCS Address Book"></asp:Label>
        </p>

        <div style="padding-left: 20px; margin-top: 20px">
            <div>
                <div class="modal fade" id="UploadModal" role="dialog">
                    <div class="modal-dialog modal-sm">
                        <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                            <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                                <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title" style="font-weight: bold; font-size: 14px; text-align: center;">ICCS: Select Excel File</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="Label2" runat="server"></asp:Label>
                                <asp:FileUpload ID="FileUpload" runat="server" />

                            </div>
                            <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                                <asp:Button ID="Upload" CssClass="popupbtn" runat="server" Text="Upload" OnClick="Upload_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <div style="float: left">
                    <asp:Button ID="btnAdd" runat="server" CssClass="simplebutton1" Enabled="true" Text="Add Contact" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnImport" runat="server" CssClass="simplebutton1" Enabled="true" Text="Import Contact" OnClick="btnImport_Click" />
                    <asp:Button ID="btnExport" runat="server" CssClass="simplebutton1" Enabled="true" Text="Export Contact" OnClick="btnExport_Click" />
                </div>
                <div style="float: right; margin-bottom: 5px">
                    <table>

                        <tr>
                            <td class="required" style="text-align: right;">
                                <span class="normal">Data Field1:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="SearchType1" runat="server" CssClass="textfield" Width="125">
                                    <asp:ListItem Value="Region_Name">Region Name</asp:ListItem>
                                    <asp:ListItem Value="Name">Name</asp:ListItem>
                                    <asp:ListItem Value="Conact_Number">Contact Number</asp:ListItem>
                                    <asp:ListItem Value="Category_Name">Category Name</asp:ListItem>
                                    <asp:ListItem Value="Contact_Source">Contact Source</asp:ListItem>
                                </asp:DropDownList></td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtField1" runat="server" class="textfield" MaxLength="30" Width="150" ClientIDMode="Static" />
                            </td>
                            <td class="required" style="text-align: right;">
                                <span class="normal">Data Field2:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="Type" runat="server" CssClass="Type" Width="55">
                                    <asp:ListItem Value="OR">OR</asp:ListItem>
                                    <asp:ListItem Value="AND">AND</asp:ListItem>
                                </asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="SearchType2" runat="server" CssClass="textfield" Width="125">
                                    <asp:ListItem Value="Region_Name">Region Name</asp:ListItem>
                                    <asp:ListItem Value="Name">Name</asp:ListItem>
                                    <asp:ListItem Value="Conact_Number">Contact Number</asp:ListItem>
                                    <asp:ListItem Value="Category_Name">Category Name</asp:ListItem>
                                    <asp:ListItem Value="Contact_Source">Contact Source</asp:ListItem>
                                </asp:DropDownList></td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtField2" runat="server" class="textfield" MaxLength="30" Width="150" ClientIDMode="Static" />
                            </td>

                            <td>
                                <asp:Button ID="btnSearch" runat="server" CssClass="searchbtn" Text="Search" OnClick="btnSearch_Click" ClientIDMode="Static" />
                            </td>
                            
                        </tr>
                    </table>
                </div>
            </div>
            <div>


                <asp:GridView ID="ICCSGrid" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light"
                    CellPadding="10" CellSpacing="10" PageSize="15" CssClass="Grid" AllowPaging="true" DataKeyNames="ID"
                    OnPageIndexChanging="ICCSGrid_PageIndexChanging" OnRowDeleting="ICCSGrid_RowDeleting" OnRowDataBound="ICCSGrid_RowDataBound"
                    OnRowEditing="ICCSGrid_RowEditing">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" Visible="false" />
                        <asp:BoundField DataField="Region_Name" HeaderText="Region Name" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="CATEGORY_ID" HeaderText="CATEGORY ID" ReadOnly="true" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" Visible="false" />
                        <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="true" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="TITLE" HeaderText="Designation" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Contact Number 1" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="NUMBER_ALIAS" HeaderText="Contact Source 1" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="NUMBER1" HeaderText="Contact Number 2" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="NUMBER_ALIAS1" HeaderText="Contact Source 2" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="NUMBER2" HeaderText="Contact Number 3" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:BoundField DataField="NUMBER_ALIAS2" HeaderText="Contact Source 3" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:CommandField ShowEditButton="true" HeaderText="Edit" ItemStyle-CssClass="Item" HeaderStyle-CssClass="Header" />
                        <asp:CommandField ShowDeleteButton="true" ButtonType="Button" HeaderText="Delete" ItemStyle-CssClass="Delete" HeaderStyle-CssClass="Header" />
                    </Columns>
                    <HeaderStyle BackColor="#3a5570" />
                    <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                    <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light " />
                </asp:GridView>
                <asp:Label runat="server" ID="Total_Records" CssClass="Total"> </asp:Label>
            </div>

        </div>
    </div>

    <link rel="stylesheet" href="Styles/1.12.1-jquery-ui.css">
    <script type="text/javascript" src="Scripts/1.12.1-jquery-ui.min.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');

        }
        function hideModal() {
            $("#UploadModal").modal('hide');
        }
        function showUpload() {
            $("#UploadModal").modal('show');

        }
        function hideUpload() {
            $("#myModal").modal('hide');
        }
    </script>
</asp:Content>
