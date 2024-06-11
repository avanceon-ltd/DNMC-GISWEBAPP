<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkLogs.aspx.cs" Inherits="WebAppForm.WorkLogs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work Logs</title>
    <style type="text/css">
        .heading {
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 15px;
            font-size-adjust: none;
            font-stretch: normal;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            margin: 0;
            padding: 3px 5px 3px 0;
            text-align: left;
        }
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
    
 
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <div class="heading"><asp:Label ID="lblHeading" runat="server"></asp:Label>
        <asp:LinkButton Style="padding-left: 10px; float: right;" ID="lnkbtnback" runat="server" Text="Back" Visible="true" 
                                Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" /> </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                        CellPadding="10" CellSpacing="10" AllowPaging="true" OnPageIndexChanging="gvCustomers_PageIndexChanging" 
                        AllowCustomPaging="true" >
                        <Columns>
                            <asp:BoundField DataField="RECORDKEY" HeaderText="Record" ItemStyle-Width="100" />
                            <asp:BoundField DataField="CLASS" HeaderText="Class" />
                            <asp:BoundField DataField="CREATEBY" HeaderText="Created By" ItemStyle-Width="100" />
                            <asp:BoundField DataField="CREATEDATE" HeaderText="Created Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="MODIFYBY" HeaderText="Modified By" ItemStyle-Width="100" />
                            <asp:BoundField DataField="MODIFYDATE" HeaderText="Modified Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="LOGTYPE" HeaderText="Type" />
                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Summary" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="800" />                            
                        </Columns>
                        <HeaderStyle BackColor="#3a5570" />
                        <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                        <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                    </asp:GridView>

                </td>
            </tr>
            <tr>
                <td style="text-align:left;">
                    Total Records: 
                    <asp:Label runat="server" ID="lblRecordCount" ForeColor="White" Font-Bold="true" />
                </td>
            </tr>

        </table>
    </form>
</body>
</html>
