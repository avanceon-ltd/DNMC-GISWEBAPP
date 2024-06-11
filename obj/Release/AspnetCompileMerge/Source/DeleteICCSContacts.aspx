<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DeleteICCSContacts.aspx.cs" Inherits="WebAppForm.DeleteICCSContacts"
    Title="Add Contact Information" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <script src="Scripts/jquery-1.4.1.min.js"></script>

    <style type="text/css">
        .fieldlabel {
            text-align: right;
            width: 50%;
        }

        .fieldvalue {
            text-align: left;
        }

        * {
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
            Width: 186px;
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

        .btnSubmit {
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
            margin-right: 24px;
            margin-left: 5px;
        }

            .btnSubmit:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }
            .btnCancel {
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
            margin-left: 5px;
        }

            .btnCancel:hover {
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


        .ui-autocomplete {
            cursor: pointer;
            height: 200px;
            overflow-y: hidden;
            -moz-background-clip: border !important;
            -moz-background-inline-policy: continuous !important;
            -moz-background-origin: padding !important;
            background: #001119 !important;
            border: 1px solid #4c636f !important;
            padding: 0.4em !important;
            color: white !important;
            font-family: "Segoe UI Light",Arial !important;
            font-size: 12px !important;
        }

            .ui-autocomplete:hover {
                border: 1px solid #4c636f !important;
            }

        .custom {
            width: 200px;
            padding: 5px;
            height: 30px;
            background-color: #022130;
            border: 1px solid cadetblue;
        }

        .title {
            font-size: 12px;
            font-family: "Segoe UI Light",Arial;
            font-weight: bold;
            padding: 10px 0 10px 5px;
            text-align: right;
            vertical-align: middle;
        }
        #Title{
            text-align:center;
            font-weight:bold;
            font-size:20px;
            font-family:'Segoe UI Light', Arial;
            margin-top:20px;
            padding-top:20px;
        }
        .ListviewPage{
            text-decoration:underline;
            color:white;
        }
    </style>
    <script type="text/javascript" src="Scripts/1.12.4_jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <link href="Content/select2.min.css" rel="stylesheet" />
    <script src="Scripts/select2.min.js"></script>

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
                    <asp:Button ID="Button1" CssClass="popupbtn" runat="server" Text="OK" OnClientClick="return;" />
                </div>
            </div>
        </div>
    </div>

    <table id="table1" style="border-collapse: collapse;" width="100%" border="0">
        <tr>
            <td>
                <p id="Title" align="center">
                    <asp:Label ID="Heading" runat="server" Text="Delete Contacts"></asp:Label>                    
                </p>
            </td>
        </tr>       
        <tr>
            <div id="divone">
                <table width="100%">
                    <tr>
                        <td class="required"><span style="color: red;">* </span>Region:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Regions" runat="server" CssClass="custom" AutoPostBack="true" OnSelectedIndexChanged="Regions_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="required">Total Records: </td>
                        <td style="padding-left: 10px;" colspan="3"><asp:Label runat="server" ID="lblRecords"></asp:Label></td>
                        <%--<td class="required">Category:</td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="Category" runat="server" CssClass="custom" AutoPostBack="true" OnSelectedIndexChanged="Category_SelectedIndexChanged"></asp:DropDownList>
                        </td>--%>
                    </tr> 
                    
                    <tr>
                        <td colspan="3"><asp:Button ID="btncancel" runat="server" ValidationGroup="test" onClick="contactList_Click" CssClass="btnCancel" Enabled="true" Text="Cancel" Visible="true" /></td>
                        <td style="padding-top: 20px;">                            
                            <asp:Button ID="btnSubmit" runat="server" ValidationGroup="test" OnClick="btnSubmit_Click" CssClass="btnSubmit" Enabled="true" Text="Delete" OnClientClick="if(!confirm('Are you sure you want to delete records?')){ return false; };;" />
                        </td>
                    </tr>

                </table>
            </div>
        </tr>
    </table>

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
        $(document).ready(function () {
            $("#ContentPlaceHolder1_Regions").select2({
                placeholder: "Select Region",
                allowClear: true
            });
            $("#ContentPlaceHolder1_Category").select2({
                placeholder: "Select Category",
                allowClear: true
            });
            $("#ContentPlaceHolder1_Designations").select2({
                placeholder: "Select Designation",
                allowClear: true
            });
            $("#ContentPlaceHolder1_CS1").select2({
                placeholder: "Select Contact Source 1",
                allowClear: true
            });
            $("#ContentPlaceHolder1_CS2").select2({
                placeholder: "Select Contact Source 2",
                allowClear: true
            });
            $("#ContentPlaceHolder1_CS3").select2({
                placeholder: "Select Contact Source 3",
                allowClear: true
            });
        });

    </script>
</asp:Content>
