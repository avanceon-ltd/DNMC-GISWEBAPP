<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.CRMSComplaintDetail"
    Title="CRMS Complaint Detail" CodeBehind="CRMSComplaintDetail.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        /*Header*/
.Tab .ajax__tab_header
{
    color: #4682b4;
    font-family:Calibri;
    font-size: 14px;
    font-weight: bold;
    background-color: #022130;
    margin-left: 0px;
}
/*Body*/
.Tab .ajax__tab_body
{
    
    padding-top:0px;
}
/*Tab Active*/
.Tab .ajax__tab_active .ajax__tab_tab
{
    color: #ffffff;
    background:url("../images/tab/tab_active.gif") repeat-x;
    
    height:20px;
}
.Tab .ajax__tab_active .ajax__tab_inner
{
    color: #ffffff;
    background:url("../images/tab/tab_left_active.gif") no-repeat left;
    padding-left:10px;
}
.Tab .ajax__tab_active .ajax__tab_outer
{
    color: #ffffff;
    background:url("../images/tab/tab_right_active.gif") no-repeat right;
    padding-right:6px;
}
/*Tab Hover*/
.Tab .ajax__tab_hover .ajax__tab_tab
{
    color: #000000;
    background:url("../images/tab/tab_hover.gif") repeat-x;
    height:20px;
}
.Tab .ajax__tab_hover .ajax__tab_inner
{
    color: #000000;
    background:url("../images/tab/tab_left_hover.gif") no-repeat left;
    padding-left:10px;
}
.Tab .ajax__tab_hover .ajax__tab_outer
{
    color: #000000;
    background:url("../images/tab/tab_right_hover.gif") no-repeat right;
    padding-right:6px;
}
/*Tab Inactive*/
.Tab .ajax__tab_tab
{
    color: #666666;
    background:url("../images/tab/tab_Inactive.gif") repeat-x;
    height:20px;
}
.Tab .ajax__tab_inner
{
    color: #666666;
    background:url("../images/tab/tab_left_inactive.gif") no-repeat left;
    padding-left:10px;
}
.Tab .ajax__tab_outer
{
    color: #666666;
    background:url("../images/tab/tab_right_inactive.gif") no-repeat right;
    padding-right:6px;
    margin-right: 2px;
}
        .textfield {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
        }
         
        .textvalue {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #001119;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }

        .textfieldalt {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #022130;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
            font-weight: bold;
        }

        .textvaluealt {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #022130;
            padding: 0.4em;
            color: white;
            font-family: "Segoe UI Light",Arial;
            font-size: 12px;
        }
         .simplebutton1 {
            color: #fff;
            background-color: #154670;
            height: 27px;
            width: 85px;
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
        .detailsheader {
            column-span: 2;
        }
       
    </style>
    <asp:SqlDataSource ID="sdsEAMS" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
        SelectCommand="SELECT 
	   sr.[EXT_CRMSRID]
	  ,sr.[DESCRIPTION]
      ,sr.[REPORTDATE]
      ,sr.[STATUS]
      ,sr.[WONUM]
      ,sr.[WO_Status]
      ,sr.[OWNER]
      ,sr.[PRIORITY]
      ,sr.[KPI_Status] 
      ,sr.[LATITUDEY]
      ,sr.[LONGITUDEX]
      ,sr.[TICKETID]
      
      ,sr.[AFFECTEDPERSON]
      ,sr.[AFFECTEDPHONE]
      ,sr.[CHANGEDATE]      
      ,sr.[EXT_FRAMEZONE]
      ,sr.ParentCRMSRID,sr.IsDuplicate,sr.HaveDuplicates
     

	  
       
  FROM  [GIS].[vwECCComplaints] sr LEFT JOIN [CRMS].[DrainageComplaints] crms 
  ON crms.Service_Request_Number = sr.EXT_CRMSRID
  WHERE sr.EXT_CRMSRID = @ID"
        OnSelecting="sdsEAMS_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="07102019-000" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsCRMS" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
        SelectCommand="SELECT 
	  crms.[Service_Request_Number]
	  ,crms.[Application_Status] AS [CRMS_Status]
	 
     
      ,crms.[Contact_Reason]
      ,crms.[Customer]
      ,crms.[Mobile_Number]
      ,crms.[Email]
      ,crms.[Preferred_Method_Of_Contact]
      ,crms.[Created_By]
      ,crms.[Contact_Source]
      ,crms.[Details]     
      ,crms.[Municipality]
      ,crms.[Zone]
      ,crms.[District]
      ,crms.[Street]
      ,crms.[Building_Number]
      ,crms.[Nearest_Land_Mark]
      ,crms.[Pin]
      ,crms.[Electricity_No]
      ,crms.[Water_No]      
      ,crms.[Application_Stage]       
      ,crms.[EAMS_SR_Number]
      ,crms.[Created_On]
       
  FROM  [GIS].[vwECCComplaints] sr LEFT JOIN [CRMS].[DrainageComplaints] crms 
  ON crms.Service_Request_Number = sr.EXT_CRMSRID
  WHERE crms.Service_Request_Number = @ID"
        OnSelecting="sdsCRMS_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter DbType="String" DefaultValue="07102019-000" Name="ID" QueryStringField="ID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <table style="width: 460px; font-family: Segoe UI Light;">       
        <tr>
            <td><asp:LinkButton ID="lnkBtnSR" runat="server" Text="Associated SR Details" Font-Names="Segoe UI Light" Font-Size="Small"  ForeColor="White"></asp:LinkButton>
                <asp:LinkButton style="padding-left:10px;" ID="lnkBtnWO" runat="server" Text="Associated WO Details" Font-Names="Segoe UI Light" Font-Size="Small"  ForeColor="White"></asp:LinkButton>
                <asp:HiddenField runat="server" ID="hdnbtn" />
                <asp:LinkButton style="padding-left:10px;" ID="lnkbtnRefresh" runat="server" Text="Refresh" Font-Names="Segoe UI Light" Font-Size="Small" 
                    ForeColor="White" OnClick="lnkbtnRefresh_Click"></asp:LinkButton>
                <asp:LinkButton style="padding-left:5px;" ID="lnkbtnCreateWO" runat="server" Text="Create WO" Font-Names="Segoe UI Light" Font-Size="Small"  ForeColor="White" OnClientClick="OpenCreateWO();"></asp:LinkButton>
                <asp:LinkButton style="padding-left:5px;" ID="lnkbtnCRMSLink" runat="server" Text="CRMS Link" Font-Names="Segoe UI Light" Font-Size="Small"  ForeColor="White" OnClick="lnkbtnCRMSLink_Click"></asp:LinkButton>
            </td>
        </tr>
        
        <tr>
            <td><input type="button" id="smsBtn" class="simplebutton1" value="MapLink/SMS" /></td>
        </tr>      
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="background-color:#022130;">
                

                <ajax:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"  CssClass="Tab" Width="450" BorderWidth="0">
                    <ajax:TabPanel runat="server" HeaderText="EAMS Data" ID="TabPanel1" BackColor="#022130">
                        <ContentTemplate>
                            <%--<asp:DetailsView ID="dvEAMS" Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" 
                                BorderColor="#4c636f" runat="server" Height="50px" DataSourceID="sdsEAMS" BackColor="#022130">  
                                
                                <RowStyle CssClass="textfield" />
                                <AlternatingRowStyle CssClass="textfieldalt" />
                            </asp:DetailsView>--%>

                            <asp:Repeater ID="Repeater1" runat="server">  
                                <ItemTemplate>  
                                    <div>  
                                        <table cellspacing="3" cellpadding="3" rules="all" border="1" class="detailstable">  
                                            
                                            <tr class="textfield">  
                                                <td>CRMS ID</td>  
                                                <td style="font-weight: normal;"><%#Eval("EXT_CRMSRID")%></td>  
                                            </tr>
                                            <tr class="textfieldalt">  
                                                <td>Description</td>  
                                                <td><%#Eval("DESCRIPTION")%></td>  
                                            </tr>  
                                            <tr class="textfield">  
                                                <td>Report Date</td>  
                                                <td><%#Eval("REPORTDATE")%></td>  
                                            </tr>  
                                            <tr class="textfieldalt">  
                                                <td>Complaint Status</td>  
                                                <td><%#Eval("STATUS")%></td>  
                                            </tr>  
                                            <tr class="textfield">  
                                                <td>WONUM</td>  
                                                <td><%#Eval("WONUM")%></td>  
                                            </tr>  
                                            <tr class="textfieldalt">  
                                                <td>WO Status</td>  
                                                <td><%#Eval("WO_Status")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>WO Owner</td>  
                                                <td><%#Eval("OWNER")%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>WO Priority</td>  
                                                <td><%#Eval("PRIORITY")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>KPI Status</td>  
                                                <td><%#Eval("KPI_Status")%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>Latitude Y</td>  
                                                <td><%#Eval("LATITUDEY")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>Longitude X</td>  
                                                <td><%#Eval("LONGITUDEX")%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>Ticket ID</td>  
                                                <td><%#Eval("TICKETID")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>Affected Person</td>  
                                                <td><%#Eval("AFFECTEDPERSON")%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>Affected Phone</td>  
                                                <td><%#Eval("AFFECTEDPHONE")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>Change Date</td>  
                                                <td><%#Eval("CHANGEDATE")%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>Framework Zone</td>  
                                                <td><%#Eval("EXT_FRAMEZONE")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>Parent CRMS ID</td>  
                                                <td><%#Eval("ParentCRMSRID")%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>Child CRMS ID</td>  
                                                <td><%#Eval("ChildCRMS")%></td>  
                                            </tr> 
                                             <tr class="textfield">  
                                                <td>Is Duplicate</td>  
                                                <td><%# ConvertNullableBoolToYesNo(Eval("IsDuplicate"))%></td>  
                                            </tr> 
                                             <tr class="textfieldalt">  
                                                <td>Have Duplicates</td>  
                                                <td><%# ConvertNullableBoolToYesNo(Eval("HaveDuplicates"))%></td>  
                                            </tr> 
                                        </table>  
                                    </div>  
                                </ItemTemplate>  
                            </asp:Repeater>  
                        </ContentTemplate>
                    </ajax:TabPanel>
                    <ajax:TabPanel ID="TabPanel2" runat="server" HeaderText="CRMS Data">
                        <ContentTemplate>
                            <asp:DetailsView ID="dvCRMS" Font-Name="Segoe UI Light" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" 
                                BorderColor="#4c636f" runat="server" Height="50px" DataSourceID="sdsCRMS">                    
                                <RowStyle CssClass="textfield" />
                                <AlternatingRowStyle CssClass="textfieldalt" />
                            </asp:DetailsView>
                        </ContentTemplate>
                    </ajax:TabPanel>
                </ajax:TabContainer>

            </td>
        </tr>
    </table>
    

    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.js"></script>

    <script type="text/javascript">

        $(function () {

            var popup;
            $('#smsBtn').click(function () {
                var Id = '<%= Request.QueryString["ID"] %>';
         
                var lat = $("#ContentPlaceHolder1_TabContainer1_TabPanel1").find("td:eq(19)").text();
                var lon = $("#ContentPlaceHolder1_TabContainer1_TabPanel1").find("td:eq(21)").text();
                var ph = $("#ContentPlaceHolder1_TabContainer1_TabPanel1").find("td:eq(27)").text();
                if (ph != "" && !ph.startsWith("974")) {
                    ph = "974" + ph;
                }
                var isWorkOrder = "0";
                debugger;
                popup = window.open("MapsLinkView.aspx?lat=" + lat + "&long=" + lon + "&ph=" + ph + "&Id=" + Id + "&isWorkOrder=" + isWorkOrder, + "Popup", "width=600,height=500");
                popup.focus();
            });

        });
        function OpenCreateWO() {
			const urlParams = new URLSearchParams(window.location.search);
            const crmsid = urlParams.get('id');
            popup = window.open("CreateWOCustomer.aspx?crmsid=" + crmsid, "Popup", "width=800,height=720");
            popup.focus();
			return false;
		}

        function OpenCRMSLink() {
   //         const urlParams = new URLSearchParams(window.location.search);
   //         callCSharpFunction();
   //         const crmsid = "2973AEB3-7557-EE11-A32D-005056954211"; //urlParams.get('ID'  );
           
   //         popup = window.open("https://crmsuat2.crm4.dynamics.com/main.aspx?appid=12d0a13f-63a4-e911-a2cc-00155dc60a3b&forceUCI=1&pagetype=entityrecord&etn=incident&id=2973AEB3-7557-EE11-A32D-005056954211" + crmsid, "Popup", "width=800,height=720");
   //         popup.focus();
			//return false;
        }
        function callCSharpFunction() {
            alert();
            var parameterValue = "yourValue";
            debugger;
        $.ajax({
            type: "GET",
            url: "CRMSComplaintDetail.aspx/YourFunction",
            data: JSON.stringify({ parameter: parameterValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                alert(response.d);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }

        {/*function callBackendController() {
    // Example URL of the backend controller action
    var url = '/test';
 
    // Example data to be sent in the request body (if needed)
    var data = {
        key1: 'value1',
        key2: 'value2'
    };
 
    // Make an AJAX request to the backend controller
    $.ajax({
        type: 'POST', // or 'GET' depending on your backend setup
        url: url,
        data: data,
        dataType: 'json', // Change dataType based on your response type
        success: function(response) {
            // Handle successful response from the backend
            console.log('Success:', response);
            // You can perform further actions with the response data here
        },
        error: function(xhr, status, error) {
            // Handle error response from the backend
            console.error('Error:', error);
            // You can display an error message to the user or perform other actions here
        }
    });
} */}
    </script>

</asp:Content>
