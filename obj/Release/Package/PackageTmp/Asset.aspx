<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asset.aspx.cs" Inherits="WebAppForm.Asset" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assets - Work Order</title>
    <style type="text/css">
        .hiddencol {
            display: none;
        }

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
        <table>
            <tr>
                <td>
                    <table>

                         <tr>
                            <td class="required" align="right" style="text-align: right;">
                                <span class="normal">Data Field 1:</span>
                            </td>
                           <td><asp:DropDownList ID="ddlField1" runat="server" CssClass="textfield" Width="150">
                                    <asp:ListItem Value="a.ASSETNUM">Asset Number</asp:ListItem>
                                    <asp:ListItem Value="a.DESCRIPTION">Description</asp:ListItem>
                                    <asp:ListItem Value="a.HIERARCHYPATH">Hierarchy Path</asp:ListItem>
                                    <asp:ListItem Value="l.LOCATION">Location</asp:ListItem>                                    
                                    <asp:ListItem Value="a.EXT_FRAMEZONE">Framework Zone</asp:ListItem>                                    
                                </asp:DropDownList></td>
                             <td class="requiredvalue">
                                <asp:TextBox ID="txtField1" runat="server" class="textfield" MaxLength="30" Width="150"  />                                
                            </td>
                             <td><asp:DropDownList ID="ddlAndOr" runat="server" CssClass="textfield" Width="60">
                                    <asp:ListItem Value="OR">OR</asp:ListItem>
                                    <asp:ListItem Value="AND">AND</asp:ListItem>                                    
                                </asp:DropDownList></td>
                             <td class="required" style="text-align: right;">
                                <span class="normal">Data Field 2:</span>
                            </td>
                           <td><asp:DropDownList ID="ddlField2" runat="server" CssClass="textfield" Width="150">
                                    <asp:ListItem Value="a.ASSETNUM">Asset Number</asp:ListItem>
                                    <asp:ListItem Value="a.DESCRIPTION">Description</asp:ListItem>
                                    <asp:ListItem Value="a.HIERARCHYPATH">Hierarchy Path</asp:ListItem>
                                    <asp:ListItem Value="l.LOCATION">Location</asp:ListItem>                                    
                                    <asp:ListItem Value="a.EXT_FRAMEZONE">Framework Zone</asp:ListItem>                                     
                                </asp:DropDownList></td>
                             <td class="requiredvalue">
                                <asp:TextBox ID="txtField2" runat="server" class="textfield" MaxLength="30" Width="150"  />                                
                            </td>                           
                             <td>                                
                                <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        
                       
                    </table>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>

                    <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                        CellPadding="10" CellSpacing="10" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                        AllowCustomPaging="true" >
                        <Columns>
                            <asp:BoundField DataField="ASSETNUM" HeaderText="ASSETNUM" ItemStyle-Width="100" />
                            <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="400" />
                            <asp:BoundField DataField="HIERARCHYPATH" HeaderText="HIERARCHY PATH" ItemStyle-Width="100" />
                            <asp:BoundField DataField="STATUS" HeaderText="STATUS" ItemStyle-Width="100" />
                            <asp:BoundField DataField="GISLONG" HeaderText="LONGITUDEX" />
                            <asp:BoundField DataField="GISLAT" HeaderText="LATITUDEY" />                            
                            <asp:BoundField DataField="LOCATION" HeaderText="LOCATION" ItemStyle-Width="100" />    
                            <asp:BoundField DataField="EXT_FRAMEZONE" HeaderText="FrameWork Zone" ItemStyle-Width="100" />

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

                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hdnFrameZone" runat="server"  Value="0"/>
                </td>
            </tr>

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
                    debugger;
                    var row = lnk.parentNode.parentNode;
                    var rowIndex = row.rowIndex - 1;
                    var name = row.cells[0].innerHTML;
                    var asssetDesc = row.cells[1].innerHTML;
                    var lang = row.cells[4].innerHTML;
                    var lat = row.cells[5].innerHTML;
                    var Location = row.cells[6].innerHTML;
                    var FrameZone = row.cells[7].innerHTML;
                   
                    window.close();
                    window.opener.document.getElementById("ContentPlaceHolder1_txtAsset").value = name;
                    window.opener.document.getElementById("ContentPlaceHolder1_txtAsset").setAttribute('title', asssetDesc);
                    window.opener.document.getElementById("ContentPlaceHolder1_txtlong").value = lang;
                    window.opener.document.getElementById("ContentPlaceHolder1_txtLat").value = lat;

                    if (1 == <%=Request.QueryString["IsLocation"]%>)
                    {
                        if (Location !="&nbsp;")
                            window.opener.document.getElementById("ContentPlaceHolder1_txtLocation").value = Location;
                        
                    }
                    var isFramezone = document.getElementById("hdnFrameZone").value;
                    if (1 == isFramezone)
                    {
                        window.opener.document.getElementById("ContentPlaceHolder1_txtFrameZone").value = FrameZone == "&nbsp;" ? '' : FrameZone;
                        window.opener.document.getElementById("ContentPlaceHolder1_FrameImg").click();
                    }
                    return false;
                }
                window.close();
            }
        </script>
        
    </form>
</body>
</html>