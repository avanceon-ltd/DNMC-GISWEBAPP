<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssociatedVehicles.aspx.cs" Inherits="WebAppForm.AssociatedVehicles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Associated Vehicles - Work Order</title>
    <style type="text/css">
        .hiddencol {
            display: none;
        }

        .Grid {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: "Segoe UI Light",Arial;
            color: white;
        }

            .Grid td {
                padding: 5px;
                color: white;
            }

            .Grid th {
                padding: 5px 6px;
                color: white;
            }

            .Grid table {
                margin: 3px 0;
                color: white;
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
    </style>
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <script type="text/javascript">
        function close_window() {
            if (confirm("Close Window?")) {
                close();
            }
        }
        function showModal() {
            $("#myModal").modal('show');

        }
        function hideModal() {
            $("#myModal").modal('hide');
        }


    </script>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">

    <form id="form1" runat="server" style="text-align: center;">

        <div style="text-align: left; float: left; font-family: Segoe UI Light; font-size: 13px;">WorkOrder: <%=Request.QueryString["WONUM"]%> --> Associated Vehicles</div>
        <div style="text-align: left; float: right; font-family: Segoe UI Light; font-size: 13px; padding-right: 6px;">USER: <%=Request.QueryString["USER"]%></div>
        <br />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT wv.[WONUM],wv.[PlateNumber],wv.[IsAssigned],wv.[CreatedDate],wv.[CreatedBy],v.[Vehicle_Details],v.Contractor FROM [ODW].[AVLS].[vwWorkOrderVehicleAssignment] wv left join [ODW].[GIS].[vwVehicleInfo] v
  on wv.[PlateNumber]=v.Title where ([WONUM] = @WONUM)">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="11047675" Name="WONUM" QueryStringField="WONUM" />
            </SelectParameters>

        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT Count(*) AS TotalRows FROM [ODW].[AVLS].[vwWorkOrderVehicleAssignment] where ([WONUM] = @WONUM)">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="11047675" Name="WONUM" QueryStringField="WONUM" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:GridView ID="gvCustomers" CssClass="Grid" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false" CellPadding="16" CellSpacing="16" DataSourceID="SqlDataSource1" OnRowDeleting="OnRowDeleting" OnRowDataBound="OnRowDataBound">
            <Columns>
                <asp:BoundField DataField="WONUM" HeaderText="WorkOrder Number" />
                <asp:BoundField DataField="PlateNumber" HeaderText="Title" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" />
                <asp:BoundField DataField="Vehicle_Details" HeaderText="Vehicle_Details" />
                <asp:BoundField DataField="Contractor" HeaderText="Contractor" />
                <asp:CommandField ShowDeleteButton="True"  ButtonType="Link" DeleteText="UnAssign" />
            </Columns>
            <HeaderStyle BackColor="#3a5570" Font-Names="Segoe UI Light" />
            <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
            <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
        </asp:GridView>

        <asp:DataList ID="DataList5" runat="server" Font-Size="Small" Font-Names="Segoe UI Light" DataSourceID="SqlDataSource2">
            <ItemTemplate>
                Total Records:
                    <asp:Label ID="label1" Font-Bold="true" Font-Size="Small" Font-Names="Segoe UI Light" runat="server" Text='<%# Eval("TotalRows") %>'></asp:Label><br />
            </ItemTemplate>
        </asp:DataList>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog modal-sm">
                <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                    <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                        <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="font-weight: bold; font-family: Segoe UI Light; font-size: 14px;">UnAssign Vehicle: MessageBox</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblMessage" runat="server" Style="font-family: Segoe UI Light; color: white; font-size: 12px;"></asp:Label>
                    </div>
                    <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                        <asp:Button ID="btnConfirmation" runat="server" Text="OK" CssClass="popupbtn" />

                    </div>
                </div>
            </div>
        </div>


    </form>
</body>
</html>
