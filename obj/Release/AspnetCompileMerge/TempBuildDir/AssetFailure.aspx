<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.AssetFailure"
    Title="Ashgal GIS AssetFailure" CodeBehind="AssetFailure.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .fieldlabel {
            text-align: right;
            width: 50%;
        }

        .fieldvalue {
            text-align: left;
        }

      body,  * {
            margin: 0;
            padding: 0;
        }

        h1, h2, h3, h4, h5, h6, pre, code, td {
            font-size: 1em;
            font-weight: normal;
        }

        ul, ol {
            list-style: none;
        }

        a img, :link img, :visited img, fieldset {
            border: 0;
        }

        a {
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }

        td {
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        h2, .h2 {
            font-family: "Segoe UI Light",Arial;
            font-size: 15px;
        }

        h3 {
            font-family: "Segoe UI Light",Arial;
            font-size: 15px;
            color: #154670;
        }

        h1 {
            font-family: "Segoe UI Light",Arial;
            font-size: 19px;
            color: #154670;
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

            .textfield:hover {
                border: 1px solid #4c636f;
            }



        .CustomValidatorCalloutStyle div, .CustomValidatorCalloutStyle td {
            border: solid 1px blue;
            background-color: Black;
        }

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

        .required {
            font-weight: bold;
            padding: 10px 0 10px 5px;
            text-align: right;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .requiredvalue {
            padding: 10px 10px 5px;
            text-align: left;
            padding-left: 0px;
        }

        .chkrequired {
            font-weight: bold;
            padding: 10px 0 3px 5px;
            text-align: left;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .text_padding {
            padding-top: 20px;
        }

        .normal {
            font-size: 12px;
            font-family: "Segoe UI Light",Arial;
            vertical-align: middle;
        }

        .chknormal {
            line-height: 23px;
        }

        .all_required {
            color: Red;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: normal;
            line-height: 30px;
        }

        .style3 {
            font-weight: bold;
            padding: 10px 0 3px 5px;
            text-align: left;
            vertical-align: middle;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            width: 205px;
        }



        ul li {
            list-style-type: square;
            margin-left: 40px;
        }

        .validationsummary {
            border: 1px solid #fff;
            background-color: #001119;
            padding: 0px 0px 13px 0px;
            font-size: 12px;
            width: 99%;
            color: white;
        }

            .validationsummary ul {
                padding-top: 5px;
                padding-left: 45px;
            }

                .validationsummary ul li {
                    padding: 2px 0px 0px 15px;
                }

        .wo {
            color: white;
            padding: 1em 1.5em;
            text-decoration: none;
            text-transform: uppercase;
        }

            .wo a:hover {
                background-color: #555;
            }

            .wo a:active {
                background-color: black;
            }

            .wo a:visited {
                background-color: #ccc;
            }

        .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 35px;
            width: 140px;
            padding: 10px;
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


    <table id="table1" style="border-collapse: collapse;" width="100%" border="0">


        <tr>
            <td style="text-align: right;">
                <asp:HyperLink ID="linkAF" runat="server" NavigateUrl="~/AssetFailureList.aspx" Text="AssetFailure ListView" Font-Bold="true" CssClass="wo"></asp:HyperLink></td>
        </tr>
        <tr>
            <td>
                <p align="center" style="font-weight: bold; font-size: 20px; font-family: 'Segoe UI Light', Arial;">
                 <asp:Label ID="lbl_label" Text="Asset Indication - Express Form" runat="server" /><br />
                </p>
            </td>
        </tr>
          <tr>
            <td>
                <p align="right" style="font-weight: bold; font-family: 'Segoe UI Light', Arial;">UserName:
                 <asp:Label ID="userName" Text="" runat="server" /><br />
                </p>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:Label ID="lblMessage" ForeColor="Green" runat="server" Text=""></asp:Label>
                <br />
                <asp:ValidationSummary
                    ID="valSum"
                    DisplayMode="BulletList"
                    runat="server" CssClass="validationsummary"
                    ValidationGroup="test"
                    EnableClientScript="true"
                    HeaderText="&nbsp;Please correct the below errors:" ForeColor="white" ShowSummary="true" />
                <br />
            </td>
        </tr>

        <tr>
            <td>

                <table width="100%">
                    <tr>
                        <td class="requiwhite" align="right" style="text-align: right;">
                            <span class="normal" style="font-weight: bold;"><span style="color: red;">* </span>Asset Description:</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left;">
                            <asp:TextBox ID="txtAssetDes" runat="server" class="textfield" TextMode="MultiLine" Height="40" Width="185" Text=""
                                Rows="30" Columns="20"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAssetDes" Display="None" EnableClientScript="true"
                                    ValidationGroup="test" Text="*" ErrorMessage="Asset Description is Required" ForeColor="white" />

                        </td>

                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal">Status:</span>
                        </td>
                        <td class="requiredvalue">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="textfield" Width="197px" Style="margin-left: 8px;">
                                <asp:ListItem Value="Unattended">Un-Attended</asp:ListItem>
                                <asp:ListItem Value="InProgress">In-Progress</asp:ListItem>
                                <asp:ListItem Value="Resolved">Resolved</asp:ListItem>

                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>Longitude (X):</span>
                        </td>
                        <td class="requiredvalue" style="text-align: left;">
                            <asp:TextBox ID="txtlong" runat="server" class="textfield" Width="185"
                                 MaxLength="18" Text="4.14"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                FilterType="Numbers, Custom" ValidChars="."
                                TargetControlID="txtlong">
                            </ajax:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="reqtxtWodesc" runat="server" ControlToValidate="txtlong" Display="None" EnableClientScript="true"
                                    ValidationGroup="test" Text="*" ErrorMessage="Longitude is Required" ForeColor="white" />

                        </td>

                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal"><span style="color: red;">* </span>Latitude (Y):</span>
                        </td>
                        <td class="requiredvalue" style="    padding-left: 10px;">
                            <asp:TextBox ID="txtLat" runat="server" class="textfield" MaxLength="18"
                                onfocus="document.getElementById('divone').className='borderfull';"
                                onblur="document.getElementById('divone').className='bordernone';"
                                Text="3.14" Width="182" />
                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                FilterType="Numbers, Custom" ValidChars="."
                                TargetControlID="txtLat">
                            </ajax:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLat" Display="None" EnableClientScript="true"
                                    ValidationGroup="test" Text="*" ErrorMessage="Latitude is Required" ForeColor="white" />

                        </td>
                    </tr>

                    <tr>
                        <td class="required" align="right" style="text-align: right;">
                            <span class="normal">Remarks:</span>
                        </td>
                        <td class="requiredvalue" colspan="3" style="text-align: left;">
                            <asp:TextBox ID="txtRemarks" runat="server" class="textfield" TextMode="MultiLine" Height="40" Width="185" Text=""
                                Rows="30" Columns="40"></asp:TextBox>
                        </td>


                    </tr>

                </table>

            </td>
        </tr>


        <tr>

            <td align="center" >
                <br />
                <asp:HiddenField ID="hid_UID" runat="server" />
                <asp:Button ID="btnSubmit" runat="server"  ValidationGroup="test"
                    OnClick="btnSubmit_Click" CssClass="simplebutton1" Width="180px" Text="Indicate Asset Failure" />
                  <asp:Button ID="btnDel" runat="server"  ValidationGroup="test"
                    OnClick="btnDel_Click" CssClass="simplebutton1" Text="Delete Assert" />
            </td>
        </tr>



    </table>


</asp:Content>
