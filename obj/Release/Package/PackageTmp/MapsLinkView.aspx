<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapsLinkView.aspx.cs" Inherits="WebAppForm.MapsLinkView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Map Link</title>
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

        .auto-style1 {
            text-align: left;
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
                .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 27px;
            width: 67px;
            border: solid 1px #fff;
            font-size: 11px;
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

<body style="background-color: #022130; font-family: Segoe UI Light; color: white; height: 372px; width: 500px;">
    <div class="modal-header" >
        <p style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;" class="auto-style1">
            Send Map Link<br />
        </p>
    </div>
    <form id="form1" runat="server" style="text-align:left;">
        
        <div>
            <div>
                <asp:Label ID="labelQRCode" runat="server" Text="" ></asp:Label>
            </div>
            <br />
            <div style="text-align: left">
                Send SMS:
                <asp:TextBox ID="phNumbers" class="textfield"  ToolTip="Multiple Numbers can be added seperated by ," runat="server" Text="" Width="100px"></asp:TextBox> &nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="sendMapLinkViaSMS" OnClientClick="this.disabled = true; this.value = 'Sending...';" UseSubmitBehavior =" false"  CssClass="simplebutton1" Text="Send" OnClick="sendMapLinkViaSMS_Click" />
            </div>
        </div>
        <br />
        <div style="height: 180px; text-align: left;">
            QR Code:<div style="height: 147px; text-align: left;">
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            </div>
        </div>
        <div> <asp:Label ID="notifyLabel" runat="server" Visible="false" Text="Message"></asp:Label> </div>
         <script type="text/javascript">
            function HideLabel() {
                var seconds = 5;
                setTimeout(function () {
                    document.getElementById("<%=notifyLabel.ClientID %>").style.display = "none";
                }, seconds * 1000);
            };
        </script>
    </form>
</body>
<script src="Scripts/1.12.4_jquery.min.js"></script>
<script src="Scripts/bootstrap.min.js"></script>
   
</html>
