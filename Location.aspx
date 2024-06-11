<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Location.aspx.cs" Inherits="WebAppForm.Location" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Locations - Work Order</title>
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
                background: #1773D8 url(Images/grid-header.png) repeat-x top;
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

        .search {
            background-color: #0090CB;
            border-color: #0090CB;
            color: #fff;
        }

            .search:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }

        .searchtxt {
            display: inline-block;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #555;
            vertical-align: middle;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc
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

        .search {
            background-color: #0090CB;
            border-color: #0090CB;
            color: #fff;
        }

            .search:hover {
                background-color: #3498db;
                border: solid 1px #fff;
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
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">
    <form id="form1" runat="server" style="text-align: center;">
        <div>
             <table>

                         <tr>
                            <td class="required" align="right" style="text-align: right;">
                                <span class="normal">Data Field 1:</span>
                            </td>
                           <td><asp:DropDownList ID="ddlField1" runat="server" CssClass="textfield" Width="150">
                                    <asp:ListItem Value="LOCATION">Location</asp:ListItem>
                                    <asp:ListItem Value="DESCRIPTION">Description</asp:ListItem>
                                    <asp:ListItem Value="EXT_LOCATIONTAG">Location Tag</asp:ListItem>
                                    <asp:ListItem Value="EXT_FRAMEZONE">Framework Zone</asp:ListItem>
                                </asp:DropDownList></td>
                             <td class="requiredvalue">
                                <asp:TextBox ID="txtField1" runat="server" class="textfield" MaxLength="30" Width="150" />                                
                            </td>
                             <td><asp:DropDownList ID="ddlAndOr" runat="server" CssClass="textfield" Width="60">
                                    <asp:ListItem Value="OR">OR</asp:ListItem>
                                    <asp:ListItem Value="AND">AND</asp:ListItem>                                    
                                </asp:DropDownList></td>
                             <td class="required" style="text-align: right;">
                                <span class="normal">Data Field 2:</span>
                            </td>
                           <td><asp:DropDownList ID="ddlField2" runat="server" CssClass="textfield" Width="150">
                                   <asp:ListItem Value="LOCATION">Location</asp:ListItem>
                                    <asp:ListItem Value="DESCRIPTION">Description</asp:ListItem>
                                    <asp:ListItem Value="EXT_LOCATIONTAG">Location Tag</asp:ListItem>
                                    <asp:ListItem Value="EXT_FRAMEZONE">Framework Zone</asp:ListItem>
                                </asp:DropDownList></td>
                             <td class="requiredvalue">
                                <asp:TextBox ID="txtField2" runat="server" class="textfield" MaxLength="30" Width="150"  />                                
                            </td>                           
                             <td>                                
                                <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                <tr>
                    <td colspan="8">&nbsp;</td>
                </tr></table>

           <%-- <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>" SelectCommand="SELECT Location,DESCRIPTION,EXT_LOCATIONTAG, TYPE
  FROM [ODW].[EAMS].[DimLocation]  WHERE isActive = 1 Order by LOCATION"
                OnSelected="SqlDataSource1_Selected"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>" SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[EAMS].[DimLocation]
  where isActive = 1"></asp:SqlDataSource>--%>
            <asp:GridView ID="gvLocations" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light"
                CellPadding="10" CellSpacing="10" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging">
                
                <Columns>
                    <asp:BoundField DataField="LOCATION" HeaderText="LOCATION" ItemStyle-Width="100" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ItemStyle-Width="200" />
                    <asp:BoundField DataField="EXT_LOCATIONTAG" HeaderText="LOCATION TAG" ItemStyle-Width="200" />
                    <asp:BoundField DataField="EXT_FRAMEZONE" HeaderText="Framework Zone"  ItemStyle-Width="100" />
                    <asp:TemplateField HeaderText="TYPE">
                        <ItemTemplate>OPERATING</ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SELECT">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect" runat="server" Text="SELECT" CommandName="Select"
                                OnClientClick="return GetSelectedRow(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
            </asp:GridView>

            <table>
            <tr>
                <td style="text-align:left;">
                    Total Records: 
                    <asp:Label runat="server" ID="lblRecordCount" ForeColor="White" Font-Bold="true" />
                </td>
            </tr>
                </table> 

            <script type="text/javascript">
                function GetSelectedRow(lnk) {


                    if (window.opener != null && !window.opener.closed) {

                        var row = lnk.parentNode.parentNode;
                        var rowIndex = row.rowIndex - 1;
                        var name = row.cells[0].innerHTML;
                        var LocDesc = row.cells[1].innerHTML;
                        window.close();
                        window.opener.document.getElementById("ContentPlaceHolder1_txtLocation").setAttribute('title', LocDesc);
                        window.opener.document.getElementById("ContentPlaceHolder1_txtLocation").value = name;
                        return false;
                    }
                    window.close();
                }
            </script>
        </div>
    </form>
</body>
</html>
