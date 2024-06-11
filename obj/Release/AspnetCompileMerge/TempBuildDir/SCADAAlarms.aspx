<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.SCADAAlarms"
    Title="SCADA Alarms Report" CodeBehind="SCADAAlarms.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="skmValidators" Namespace="skmValidators" TagPrefix="skm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link type="text/css" rel="stylesheet" href="Styles/bootstrap.min.css" />
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    
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
            height: 28px;
            width: 100px;
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
        .heading{
            font-size: 30px;
            text-align: center;
            padding-left: 10px;
        }

        .GridPager a, .GridPager span
    {
        display: block;
        height: 15px;
        width: 15px;
        font-weight: bold;
        text-align: center;
        text-decoration: none;
    }
    .GridPager a
    {
        background-color: #f5f5f5;
        color: #969696;
        border: 1px solid #969696;
    }
    .GridPager span
    {
        background-color: #A1DCF2;
        color: #000;
        border: 1px solid #3AC0F2;
    }
    .catchment{
        font-size:12px;
        
    }
    .downloadsearch{
        text-align: center;
    }
    .header-center{
        text-align: center !important;
    }
    .date-range{
        font-size: 20px;
        font-weight: normal;
    }
    .spinner-overlay {
    position: fixed;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    background-color: rgba(0, 0, 0, 0.5);
    z-index: 1000;
    display: none; 
    text-align: center;
    color: white; 
}

.spinner {
    border: 6px solid #f3f3f3; 
    border-top: 6px solid #3498db; 
    border-radius: 50%;
    width: 60px;
    height: 60px;
    animation: spin 2s linear infinite;
    position: absolute;
    top: 50%;
    left: 50%;
    margin-left: -30px; 
    margin-top: -30px; 
}

.loading-text {
    position: absolute;
    top: 42%; 
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 20px; 
    color: white; 
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
    
    </style>

    <div>
        <h1 class="heading">SCADA Historical Alarms Overview - <asp:Label runat="server" ID="lblRecordCount" ForeColor="White" Font-Bold="true" /> Record(s)
       
        <span class="date-range">
        <asp:Label ID="Label1" runat="server" Text="date from"></asp:Label>
        <asp:Label ID="sdate" runat="server" Text="startdate"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="-"></asp:Label>
        <asp:Label ID="edate" runat="server" Enabled="False" Text="enddate"></asp:Label>
    </span>    
     
        </h1>
    </div>
    

  
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-sm" role="document">
            <div style="background-color: #022130; font-family: Segoe UI Light; color: white; width: 440px; margin: 0 auto;" class="modal-content">
                <div class="modal-header" style="padding: 6px; border-bottom: 1px solid #4c636f;">
                    <button type="button" class="close" style="opacity: 1; color: white;" data-dismiss="modal">&times;</button>
                    <h4 id="modalTitle" class="modal-title" style="font-weight: bold; font-size: 14px;">Download: SCADA Alarms Report</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMessage" ForeColor="White" runat="server"></asp:Label>
                </div>
                <div class="modal-footer" style="border-top: 1px solid #4c636f; padding: 10px;">
                    <asp:Button ID="Button1" CssClass="popupbtn" runat="server" Text="OK" OnClientClick="return;" />
                </div>
            </div>
        </div>
    </div>

        <table width="990">
            <tr>
                <td>
                    <table id="divone" style="text-align: right">
                         <tr>
                            <td class="required" align="right" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">Start Date:</span>
                            </td>
                            <td class="requiredvalue" style="text-align: left;">
                                <asp:TextBox ID="txtTargetStart" runat="server" class="textfield" Width="185"
                                    onfocus="document.getElementById('divone').className='borderfull';"
                                    onblur="document.getElementById('divone').className='bordernone';"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTargetStart" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Target Start is Required" ForeColor="white" />
                            </td>

                            <td class="required" align="left" style="text-align: right;">
                                <span style="color: red;">* </span><span class="normal">End Date:</span>
                            </td>
                            <td class="requiredvalue">
                                <asp:TextBox ID="txtTargetFinish" runat="server" class="textfield"
                                    Width="186" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTargetFinish" Display="None"
                                    ValidationGroup="test" Text="*" ErrorMessage="Target Finish is Required" ForeColor="white" />
                                <asp:CustomValidator ID="StartEndDiffValidator"
                                    ValidateEmptyText="true"
                                    ValidationGroup="test"
                                    OnServerValidate="StartEndDiffValidator_ServerValidate"
                                    Display="None"
                                    ErrorMessage="Target Start must be earlier than Schedule Target"
                                    runat="server" />
                            </td>
                             <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                                SelectCommand="SELECT DISTINCT ISNULL(NULLIF(CZF_ODW, 'NA'), 'Other') AS CZF_ODW FROM dbo.WebAppTestView;"></asp:SqlDataSource>

                            <td class="required" align="right" style="text-align: right;">
                             <span class="catchment">Catchment Zone:</span>
                            </td>
                            <td>
                                   <asp:DropDownList ID="ddlField1" runat="server" CssClass="textfield" Width="200px" OnSelectedIndexChanged="ddlField1_SelectedIndexChanged" style="width: 182px; margin: 9px;"
                                        DataSourceID="SqlDataSource3" DataTextField="CZF_ODW" DataValueField="CZF_ODW" AppendDataBoundItems="true">
                                   <asp:ListItem Value="All"></asp:ListItem>
                                    </asp:DropDownList>
                             </td>
                             </td>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                                    SelectCommand="SELECT DISTINCT ISNULL(Network_ODW, 'Other') AS Network_ODW FROM dbo.WebAppTestView"></asp:SqlDataSource>

                            <td class="required" align="right" style="text-align: right;">
                                <span class="catchment">Network Type:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="NetworkTypelist" runat="server" CssClass="textfield" Width="200" style="width: 182px; margin: 9px;" 
                                    DataSourceID="SqlDataSource4" DataTextField="Network_ODW" DataValueField="Network_ODW" AppendDataBoundItems="true">
                                    <asp:ListItem Value="All"></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                                    SelectCommand="SELECT DISTINCT [Asset Group_ODW] FROM dbo.WebAppTestView"></asp:SqlDataSource>
                            <td class="required" align="right" style="text-align: right;">
                                <span class="catchment">Asset Type:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="AssetTypeList" runat="server" CssClass="textfield" Width="200" style="width: 182px; margin: 9px;" 
                                    DataSourceID="SqlDataSource6" DataTextField="Asset Group_ODW" DataValueField="Asset Group_ODW" AppendDataBoundItems="true">
                                    <asp:ListItem Value="All"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
            <tr>    
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                    SelectCommand="SELECT DISTINCT [TagName] FROM [ODW].[SCADA].[vwAlarms]"></asp:SqlDataSource>

                    <td class="required" align="right" style="text-align: right;">
    <span class="catchment">Tag Name:</span>
        </td>
    <td>
    <asp:DropDownList ID="TagNameList" runat="server" CssClass="textfield" Width="200" style="width: 182px; margin: 9px;"
        DataSourceID="SqlDataSource5"
        DataTextField="TagName"
        DataValueField="TagName"
        AppendDataBoundItems="true">
        <asp:ListItem Value="All"></asp:ListItem>
    </asp:DropDownList>
    </td>
                   <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                    SelectCommand="SELECT DISTINCT SUBSTRING([TagName], LEN([TagName]) - CHARINDEX('.', REVERSE([TagName])) + 2, LEN([TagName])) AS [Attribute] FROM SCADA.vwAlarms WITH (NOLOCK)"></asp:SqlDataSource>         
            <td class="required" align="right" style="text-align: right;">
                                <span class="catchment">Attribute:</span>
                            </td>
                            <td>
    <asp:DropDownList ID="AttributeList" runat="server" CssClass="textfield" Width="200" style="width: 182px; margin: 9px;"
        DataSourceID="SqlDataSource8"
        DataTextField="Attribute"
        DataValueField="Attribute"
        AppendDataBoundItems="true">
        <asp:ListItem Value="All"></asp:ListItem>
    </asp:DropDownList>
</td>
                
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                    SelectCommand="SELECT DISTINCT [Alarm State] FROM [ODW].[SCADA].[vwAlarms]"></asp:SqlDataSource>
                        
                     <td class="required" align="right" style="text-align: right;">
                            <span class="catchment">Alarm State:</span>
                        </td>
                    <td>
                               <asp:DropDownList ID="AlarmStatelist" runat="server" CssClass="textfield" Width="200px" style="width: 182px; margin: 9px;"
                             DataSourceID="SqlDataSource1"
                             DataTextField="Alarm State" DataValueField="Alarm State" AppendDataBoundItems="true">
                            <asp:ListItem Value="All"></asp:ListItem>
                            </asp:DropDownList>
                    </td>
                      
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                    SelectCommand="SELECT DISTINCT CASE  WHEN [Priority] >= 1 AND [Priority] <= 250 THEN 'Critical' WHEN [Priority] > 250 AND [Priority] <= 500 THEN 'High'  WHEN [Priority] > 500 AND [Priority] < 950 THEN 'Medium' END AS [Priority]  FROM [ODW].[SCADA].[vwAlarms]">
                </asp:SqlDataSource>

                <td class="required" align="right" style="text-align: right;">
                    <span class="catchment">Priority:</span>
                </td>
                <td>
                     <asp:DropDownList ID="PriorityList" runat="server" CssClass="textfield" Width="200" style="width: 182px; margin: 9px;" 
                         DataSourceID="SqlDataSource2"
                         DataTextField="Priority" DataValueField="Priority" AppendDataBoundItems="true">
                         <asp:ListItem Value="All"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                            
                            <td class="required" align="right" style="text-align: right;">
                                <span class="catchment">IsSilenced?:</span>
                            </td>
                            <td><asp:DropDownList ID="IsSilencedList" runat="server" CssClass="textfield" Width="200" style="width: 182px; margin: 9px;" OnSelectedIndexChanged="NetworkTypelist_SelectedIndexChanged">
                                    <asp:ListItem Value="All"></asp:ListItem>
                                    <asp:ListItem Value="False"></asp:ListItem>
                                    <asp:ListItem Value="True"></asp:ListItem>
                                </asp:DropDownList></td>
                            </tr>
            
                <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString_Live %>"
                    SelectCommand="SELECT DISTINCT [Asset] FROM SCADA.vwAlarms WITH (NOLOCK)">
                </asp:SqlDataSource>
                <td class="required" align="right" style="text-align: right;">
                                <span class="catchment">Asset:</span>
                            </td>
                            <td><asp:DropDownList ID="AssetList" runat="server" CssClass="textfield" Width="200px" style="width: 182px; margin: 9px;"
                             DataSourceID="SqlDataSource7"
                             DataTextField="Asset" DataValueField="Asset" AppendDataBoundItems="true">
                            <asp:ListItem Value="All"></asp:ListItem>
                            </asp:DropDownList></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                            <td class="downloadsearch">
                               <asp:Button ID="btnSearch" runat="server" CssClass="simplebutton1" Text="Search" OnClick="btnSearch_Click" OnClientClick="showSpinner(); return true;"/>
                            </td>
                             <td class="downloadsearch">
                                 <asp:Button ID="btnDownload" runat="server" CssClass="simplebutton1" Text="Download" Font-Underline="true" OnClick="btnDownload_Click"  BackColor="#006600" OnClientClick="showSpinner(); return true;"/>
                             </td>
                            
            </>
                    </table>
                
           
            <tr>
                <td style="padding-left: 10px;">
                    <br />
                    <asp:GridView ID="gvCustomers" Font-Size="12px" Font-Names="Segoe UI Light" runat="server" AutoGenerateColumns="false"
                        CellPadding="10" CellSpacing="10" AllowPaging="true"  OnRowDataBound="gvCustomers_RowDataBound" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                        AllowCustomPaging="true" Width="100%" Height="600px">
                        <Columns>
                            <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-HorizontalAlign="left" ItemStyle-Width="130" />
                            <asp:BoundField DataField="Asset" HeaderText="Asset" />        
                            <asp:BoundField DataField="TagName" HeaderText="Tag Name" HeaderStyle-HorizontalAlign ="Center" />                           
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Previous State" HeaderText="Value" ItemStyle-Width="80" />
                            <asp:BoundField DataField="Alarm State" HeaderText="Alarm State" />
                            <asp:BoundField DataField="Severity" HeaderText="Severity" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="true"  ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" />
                         
                        </Columns>
                        <HeaderStyle BackColor="#3a5570" />
                        <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                        <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
                        <PagerStyle HorizontalAlign = "Right" CssClass = "GridPager" />
                    </asp:GridView>

                </td>
            </tr>
    <div class="spinner-overlay" id="spinnerOverlay">
    <div class="spinner"></div>
    <div class="loading-text">Loading. Please wait...</div>
</div>
<script type="text/javascript">
    function showSpinner() {
    document.getElementById('spinnerOverlay').style.display = 'block';
    }

    function hideSpinner() {
    document.getElementById('spinnerOverlay').style.display = 'none';
    }
    function showModal(filename, includeDownloadPrefix) {
    var title = includeDownloadPrefix ? "Download: " + filename : filename;
    document.getElementById("modalTitle").innerText = title;
    $('#myModal').modal('show');
}
    function hideModal() {
     $("#myModal").modal('hide');
    }
    </script>

    <link rel="stylesheet" href="Styles/jquery-ui.css">
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <link href="Styles/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $('#ContentPlaceHolder1_txtTargetStart').datetimepicker({
            dateFormat: 'mm/dd/yy',
            showOtherMonths: true,
            selectOtherMonths: true,
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtTargetFinish').datetimepicker({
           dateFormat: 'mm/dd/yy',
           showOtherMonths: true,
           selectOtherMonths: true,
           timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleStart').datetimepicker({
            timeFormat: "hh:mm tt"
        });
        $('#ContentPlaceHolder1_txtScheduleFinish').datetimepicker({
            timeFormat: "hh:mm tt"
        });

    </script>
    
</asp:Content>