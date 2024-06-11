<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ECCCrew.aspx.cs" Inherits="WebAppForm.ECCCrew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ECC Crew Lead - Work Order</title>
    <style type="text/css">
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
    </style>
</head>

<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">
    <form id="form1" runat="server" style="text-align: center;">
        <div>

            <asp:GridView Width="500px" ID="gvCustomers" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light" CellPadding="6" CellSpacing="6"
                DataKeyNames="FID" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img alt="" style="cursor: pointer" src="Images/plus.png" />
                            <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                <asp:GridView ID="gvOrders" DataKeyNames="CID" runat="server" AutoGenerateColumns="false" Font-Size="12px" Font-Names="Segoe UI Light" CellPadding="10" CellSpacing="10">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="Desc" HeaderText="Description" />
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
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="150px" DataField="Name" HeaderText="Name" />
                    <asp:BoundField ItemStyle-Width="150px" DataField="Desc" HeaderText="Description" />
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

            <script type="text/javascript" src="Scripts/jquery18.min.js"></script>

            <script type="text/javascript">
                $("[src*=plus]").live("click", function () {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                    $(this).attr("src", "images/minus.png");
                });
                $("[src*=minus]").live("click", function () {
                    $(this).attr("src", "images/plus.png");
                    $(this).closest("tr").next().remove();
                });
            </script>




            <script type="text/javascript">
                function GetSelectedRow(lnk) {


                    if (window.opener != null && !window.opener.closed) {

                        var row = lnk.parentNode.parentNode;
                        var rowIndex = row.rowIndex - 1;
                        var name = row.cells[0].innerHTML;
                     
                        window.close();
                     
                        window.opener.document.getElementById("ContentPlaceHolder1_txtCrew").value = name;
                        return false;
                    }
                    window.close();
                }
            </script>
        </div>
    </form>
</body>

</html>
