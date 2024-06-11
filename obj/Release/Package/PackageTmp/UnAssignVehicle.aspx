<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnAssignVehicle.aspx.cs" Inherits="WebAppForm.UnAssignVehicle"
    MasterPageFile="~/MasterPage.master" Title="Vehicle Un-Assignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <style type="text/css">
        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 35px;
            width: 140px;
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

    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 300px; margin: 0 auto;" class="modal-content">
                <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                    <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" style="font-weight: bold; font-size: 14px;">Assign Vehicle: MessageBox</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                    <asp:Button ID="btnConfirmation" runat="server" Text="OK" CssClass="popupbtn" />

                </div>
            </div>
        </div>
    </div>
    <p>&nbsp;</p>

    <table runat="server" id="tblConfirm">
        <tr>

            <td style="padding-left: 10px;">
                <asp:Label ID="lblConfirm" Height="17px" runat="server" Font-Bold="true" Text="Are you sure you want to proceed?"
                    class="textfield" Style="height: 35px; width: 250px; font-size: 1.1em;" /></td>
            <td class="required" style="padding-left: 5px;">
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="simplebutton1" Width="80" /></td>

        </tr>
    </table>

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
        $(function () {
            if (document.getElementById('hfposted').value != '1') {
                if (confirm("Do you want to Acknowledge?")) {
                    document.getElementById('hidField').value = "Yes";
                } else {
                    document.getElementById('hidField').value = "No";
                }
                document.forms[0].submit();
            }
        });

        function UrFunction() {
            var r = confirm("Wanna change Password?");
            if (r == true)
                window.location.assign("/AssignVehicle.aspx");
        }
    </script>

</asp:Content>