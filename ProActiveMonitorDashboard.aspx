<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProActiveMonitorDashboard.aspx.cs" Inherits="WebAppForm.ProActiveMonitorDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="refresh" content="300;" />
    <style>
       .button-container {
            text-align: center;
        }
       .checkbox{
           float:right;
       }
       .Menu2{
           display: grid;
           grid-template-columns: auto 1fr;
       }
        .button {
            display: inline-block;
            padding: 10px 50px 10px 50px !important;
            margin: 10px;
            color: #FFFFFF;
            border: none;
            cursor: pointer;
            font-size:large;
        }
        .databutton{
            display: inline-block;
            padding: 5px 10px 5px 10px !important;
            margin: 10px;
            color: #FFFFFF;
            border: none;
            cursor: pointer;
            font-size:small;
        }
        .button:hover {
            background-color: #0056b3;
        }

        /* Style for the content */
        .content-container {
            display: none;
            text-align: center;
        }
        .content {
            margin: 20px;
            padding: 20px;
            background-color: #f2f2f2;
        }
        .headerSection{
            background-color:lightgray;
            margin-right: 30px;
            margin-left: 15px;
        }
        
        .tabs{
            
            
        }
        .tabContents{
            margin: 0px 15px 0px 15px;
        }
        .tab{
            display:flex;
            justify-content:center;
            align-items:center;
            top:1px;
            left:10px;
            /*border: solid 1px black;*/
            /*background-color:darkgray;*/
            /*padding: 10px 50px 10px 50px !important;*/ 
            margin:5px;
            /*border-style: outset;*/
            color:white;
        }
        .selectedTab{
            background-color:indianred;
            border-bottom:solid 1px indianred;
        }
        .row{
            display: flex;
            justify-content: space-between; 
            margin-top:15px;
        }
        .row2{
            
            display: flex;
            justify-content: space-between;
            margin-top:15px;
            
        }
        .column{
            float:left;
            width:33%;
            padding:10px;
            margin-right:15px;
            border-style:groove;
            border-color: floralwhite;
        }
        .column2{
            float:left;
            width:47%;
            padding:10px;
            margin-right:15px;
            border-style:groove;
            border-color: floralwhite;
        }
        .column span{
            margin-top:20px;
            margin-bottom:20px;
        }
        .column3{
            float:left; width:100%;
            padding:10px;
            margin-right:15px;
            border-style:groove;
            border-color: floralwhite;
        }
        .column10{
            float:left; width:75%;
            padding:10px;
            margin-right:15px;
            border-style:groove;
            border-color: floralwhite;
        }
        .row::after {
          content: "";
          clear: both;
          display: table;
        }
        table {
            border-collapse: collapse;
            border-spacing: 0;
            width: 100%;
            border: 1px solid #ddd;
        }
        th{
            font-size:16px;
        }
        th, td {
            text-align: center;
            padding: 10px;
         }
        td{
                font-size: 14px;
        }
        .tableData{
            min-height:250px;
            max-height:250px;
            overflow-y:auto;
            margin-top:15px;
        }
        .tableData1{
            overflow-y:auto;
            margin-top:15px;
        }
        tr:nth-child(even) {
            background-color: #f2f2f2;
         }
        @keyframes blink {
          0% { opacity: 1; }
          50% { opacity: 0; }
          100% { opacity: 1; }
        }

        /* Apply the animation to the button */
        .blink-tab {
          animation: blink 1s infinite;
        }
    </style>
    <%--//<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript">
            
        function HideOtherData1() {
            debugger;
            var divToHide1 = document.getElementById('content1');
            var divToHide2 = document.getElementById('content2');
            var divToHide3 = document.getElementById('content3');
            var divToHide4 = document.getElementById('content4');
            var divToHide5 = document.getElementById('content5');
            if (divToHide1) {
                divToHide1.style.display = 'block';
            }
            if (divToHide2) {
                divToHide2.style.display = 'none';
            }
            if (divToHide3) {
                divToHide3.style.display = 'none';
            }
            if (divToHide4) {
                divToHide4.style.display = 'none';
           }
           if (divToHide5) {
                divToHide5.style.display = 'none';
            }
        }
        function HideOtherData2() {
            debugger;
            var divToHide1 = document.getElementById('content1');
            var divToHide2 = document.getElementById('content2');
            var divToHide3 = document.getElementById('content3');
            var divToHide4 = document.getElementById('content4');
            var divToHide5 = document.getElementById('content5');

            if (divToHide1) {
                divToHide1.style.display = 'none';
            }
            if (divToHide2) {
                divToHide2.style.display = 'block';
            }
            if (divToHide3) {
                divToHide3.style.display = 'none';
            }
            if (divToHide4) {
                divToHide4.style.display = 'none';
            }
            if (divToHide5) {
                divToHide5.style.display = 'none';
            }

        }
        function HideOtherData3() {
            var divToHide1 = document.getElementById('content1');
            var divToHide2 = document.getElementById('content2');
            var divToHide3 = document.getElementById('content3');
            var divToHide4 = document.getElementById('content4');
            var divToHide5 = document.getElementById('content5');


            if (divToHide1) {
                divToHide1.style.display = 'none';
            }
            if (divToHide2) {
                divToHide2.style.display = 'none';
            }
            if (divToHide3) {
                divToHide3.style.display = 'block';
            }
            if (divToHide4) {
                divToHide4.style.display = 'none';
            }
            if (divToHide5) {
                divToHide5.style.display = 'none';
            }
        }
        function HideOtherData4() {
            var divToHide1 = document.getElementById('content1');
            var divToHide2 = document.getElementById('content2');
            var divToHide3 = document.getElementById('content3');
            var divToHide4 = document.getElementById('content4');
            var divToHide5 = document.getElementById('content5');


            if (divToHide1) {
                divToHide1.style.display = 'none';
            }
            if (divToHide2) {
                divToHide2.style.display = 'none';
            }
            if (divToHide3) {
                divToHide3.style.display = 'none';
            }
            if (divToHide4) {
                divToHide4.style.display = 'block';
            }
            if (divToHide5) {
                divToHide5.style.display = 'none';
            }
        }

        function HideOtherData5() {
            var divToHide1 = document.getElementById('content1');
            var divToHide2 = document.getElementById('content2');
            var divToHide3 = document.getElementById('content3');
            var divToHide4 = document.getElementById('content4');
            var divToHide5 = document.getElementById('content5');


            if (divToHide1) {
                divToHide1.style.display = 'none';
            }
            if (divToHide2) {
                divToHide2.style.display = 'none';
            }
            if (divToHide3) {
                divToHide3.style.display = 'none';
            }
            if (divToHide4) {
                divToHide4.style.display = 'none';
            }
            if (divToHide5) {
                divToHide5.style.display = 'block';
            }
        }

        //function sendRequestToServer() {
        //    debugger;
        //$.ajax({
        //    type: "POST", // Change to "GET" or "POST" as needed
        //    url: "ProActiveMonitorDashboard.aspx/SendScreenshot", // Replace with your server method's URL
        //    data: "{}", // Pass any data to the server if needed
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (response) {
        //        // Handle the server's response here
        //    },
        //    error: function (xhr, status, error) {
        //        // Handle errors here
        //    }
        //});
        //}
        async function encryptKey(key) {
            debugger;
            const encoder = new TextEncoder();
            const data = encoder.encode(key);

            const subtleCrypto = window.crypto.subtle;
            const encryptionKey = await subtleCrypto.generateKey(
                { name: "AES-GCM", length: 256 },
                true,
                ["encrypt", "decrypt"]
            );

            const iv = window.crypto.getRandomValues(new Uint8Array(12));
            const encryptedData = await subtleCrypto.encrypt(
                { name: "AES-GCM", iv: iv },
                encryptionKey,
                data
            );

            const encryptedKey = btoa(String.fromCharCode(...new Uint8Array(encryptedData)));
            return encryptedKey;
        }

        //const key = "isChecked";
        //encryptKey(key).then(encryptedKey => {
        //    debugger;
        //    // Append the encrypted key to the URL
        //    const url = `ProActiveMonitorDashboard.aspx?encryptedKey=${encodeURIComponent(encryptedKey)}`;
        //    window.location.href = url;
        //});
        window.onload = function () {
                        
            
                    debugger;
                    
                    var myJsVariable = '<%= buttonClick %>';
                    console.log(myJsVariable);
                    if (myJsVariable == 1) {
                        HideOtherData1();
                    }
                    else if (myJsVariable == 2) {
                        HideOtherData2();
                    }
                    else if (myJsVariable == 3) {
                        HideOtherData3();

                    }
                    else if (myJsVariable == 4) {
                        HideOtherData4();
                    }
                    else {
                        HideOtherData5();
            }
            //sendRequestToServer(); // Call the function when the page is completely loaded
            //alert("The web form page has finished loading.");
            <%--var viewClientId = '<%= View1.ClientID %>';

            // Now you can use viewClientId in JavaScript
            var viewElement = document.getElementById(viewClientId);
            if (viewElement) {
                // Do something with the viewElement
                viewElement.classList.add('blink-button');
            }--%>
      <%--  var btn = document.getElementById('<%= View1.ClientID %>');
        btn.classList.add('blink-button');

        // Stop the blinking animation after 3 seconds (adjust as needed)
        setTimeout(function() {
          btn.classList.remove('blink-button');
        }, 3000);--%>
      }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <div>
          <div class="headerSection">
               <p align="center" style="font-weight: bold; font-size: 24px; font-family: 'Segoe UI Light', Arial;">
                    ECC - DSS System Pro-Active Monitoring 
                  <span>
                       (<%= currentSystem %>)
                  </span>
                   <br />

              </p>
              <div id="Menu1" class="tab">
                 <asp:Button ID="btnButton1" runat="server" CssClass="button" Text="ISD Systems" BackColor="green" OnClick="btnButton1_Click"/>
                 <asp:Button ID="btnButton2" runat="server" CssClass="button"  Text="ODW" BackColor="green" OnClick="btnButton2_Click"/>
                 <asp:Button ID="btnButton3" runat="server" CssClass="button"  Text="ESRI Systems" BackColor="green" OnClick="btnButton3_Click"/>
                 <asp:Button ID="btnButton4" runat="server" CssClass="button"  Text="GISIZE" BackColor="green" OnClick="btnButton4_Click"/>
                 <asp:Button ID="btnButton5" runat="server" CssClass="button"  Text="Data Count" BackColor="green" OnClick="btnButton5_Click"/>
             </div>
             <div class="Menu2">
                 <asp:Button ID="sendScreenshot" runat="server" CssClass="databutton"  Text="Send Email" BackColor="green" OnClick="btnSendEmail_Click"/>              
                 <p align="right" style="color:black; font-size:14px" > * This report is last updated on <%= reportTime %></p>
             </div>
                 
          </div>
         
          <div class="tabContents" >
              <div id="content1" style="display:block">
              <div class="row">
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">ISD Integration System Status</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Status Data will be refreshed after every 1 hour </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab1Table1" runat="server" />

                              </div>
                              <%--<table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Complaints and Work Order Data Count</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 Minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab1Table2" runat="server" />

                              </div>
                                <%--<table>
                                  <tr>
                                      <th>Fetch Date</th>
                                      <th>Name</th>
                                      <th>Updated On</th>
                                      <th>Count</th>
                                  
                                  </tr>
                                </table>--%>
                          </div>
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">New Complaints and Work Orders Recorded</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 Minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab1Table3" runat="server" />

                              </div>
                              <%--<table>
                                  <tr>
                                      <th>Service Name</th>
                                      <th>Machine Name</th>
                                      <th>Last Updated Time</th>
                                      <th>Status</th>
                                  </tr>
                          </table>--%>
                          </div>
                      </div>
              <%--<div class="row">
                          <div class="column3">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Mismatch Work Orders Status</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 minutes </asp:Label>
                             <div class="tableData">
                                  <asp:PlaceHolder ID = "tab1Table4" runat="server" />

                              </div>
                             
                          </div>
                      </div>--%>
        </div>
              <div id="content2" style="display:none">
            <div class="row2">
                          <div class="column2">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Complaints and Work Orders Data Count</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab2Table1" runat="server" />

                              </div>
                              <%--<table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                          <div class="column2">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">New Complaints and Work Orders Recorded</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 Minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab2Table2" runat="server" />

                              </div>
                              <%--<table>
                                  <tr>
                                      <th>Fetch Date</th>
                                      <th>Name</th>
                                      <th>Updated On</th>
                                      <th>Count</th>
                                  
                                  </tr>
                          </table>--%>
                          </div>
                      </div>
            <div class="row2">
                          <div class="column2">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">ODW Jobs Status</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab2Table3" runat="server" />

                              </div>
                              <%--<table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                              <div class="column2">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">ODW Windows Services Status</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 1 hour </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab2Table4" runat="server" />
                              </div>
                              <%--<table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                      </div>
        </div>
              <div id="content3" style="display:none">
            <div class="row">
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Layer Status</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Layer Status Data will be refreshed after every 1 hour </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab3Table1" runat="server" />
                              </div>
                             <%-- <table>
                                <tr>
                                  <th>Layer Name</th>
                                  <th>Service Name</th>
                                  <th>Last Updated Time</th>
                                  <th>Status</th>
                                </tr>
                            </table>--%>
                          </div>
                          <div class="column">
                               <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Count of Complaint/WO</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Complaint/WO Count Data will be refreshed after every 5 minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab3Table2" runat="server" />
                              </div>
                              <%--<table>
                                  <tr>
                                      <th>Description</th>
                                      <th>Last Updated Time</th>
                                      <th>Status</th>
                                  
                                  </tr>
                          </table>--%>
                          </div>
                          <div class="column">
                            <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Windows Services Status</asp:Label>
                            <br />
                            <asp:Label Font-Size="Small" runat="server">Windows Service Status Data will be refreshed after every 1 hour </asp:Label>
                            <div class="tableData">
                                  <asp:PlaceHolder ID = "tab3Table3" runat="server" />
                              </div>
                              <%--<table>
                                  <tr>
                                      <th>Service Name</th>
                                      <th>Machine Name</th>
                                      <th>Last Updated Time</th>
                                      <th>Status</th>
                                  </tr>
                          </table>--%>
                          </div>
                      </div>
            <div class="row2">
                          <div class="column2">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Services Logs</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Services Logs Data will be refreshed after every 5 minutes </asp:Label>
                              <asp:CheckBox ID="chkBox" runat="server" Text="Resolved" AutoPostBack="true" OnCheckedChanged="chkBox_CheckedChanged" CssClass="checkbox"/>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab3Table4" runat="server" />
                              </div>
                              <%-- <table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                              <div class="column2">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Complaints/WO Details</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Complaint/WO Details will be refreshed after every 5 minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab3Table5" runat="server" />
                              </div>
                                  <%--<table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                      </div>
        </div>
              <div id="content4" style="display:none">
            <div class="row">
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">GiSize Data Count</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">GiSize Data Count will be refreshed after every 5 minutes</asp:Label>
                             <div class="tableData">
                                  <asp:PlaceHolder ID = "tab4Table1" runat="server" />
                              </div>
                              <%--<table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Windows Services Status</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Windows Service Status Data will be refreshed after every 1 hour </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab4Table2" runat="server" />
                              </div>
                              <%--<table>
                                  <tr>
                                      <th>Fetch Date</th>
                                      <th>Name</th>
                                      <th>Updated On</th>
                                      <th>Count</th>
                                  
                                  </tr>
                          </table>--%>
                          </div>
                          <div class="column">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Complaints/Wo Details</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Complaint/WO Details will be refreshed after every 5 Minutes </asp:Label>
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab4Table3" runat="server" />
                              </div>
                              <%--<table>
                                  <tr>
                                      <th>Service Name</th>
                                      <th>Machine Name</th>
                                      <th>Last Updated Time</th>
                                      <th>Status</th>
                                  </tr>
                          </table>--%>
                          </div>
                      </div>
            <div class="row">
                          <div class="column3">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Service Logs</asp:Label>
                              <br />
                              <asp:Label Font-Size="Small" runat="server">Services Logs Data will be refreshed after every 5 minutes </asp:Label>
                               <asp:CheckBox ID="chkBoxGISIZE" runat="server" Text="Resolved" AutoPostBack="true" OnCheckedChanged="chkBoxGISIZE_CheckedChanged" CssClass="checkbox"/>
                              
                              <div class="tableData">
                                  <asp:PlaceHolder ID = "tab4Table4" runat="server" />
                              </div>
                          </div>
                      </div>
        </div>
              <div id="content5" style="display:none">
              <div class="row">
                          <div class="column10">
                              <asp:Label Font-Size="Medium" Font-Bold="true" runat="server">Comparing Data Count</asp:Label>
                              <br />
                              <%--<asp:Label Font-Size="Small" runat="server">The Data count will be refreshed after every 5 minutes </asp:Label>--%>
                             <div class="tableData1">
                                  <asp:PlaceHolder ID = "tab5Table1" runat="server" />

                              </div>
                              <%-- <table>
                                <tr>
                                  <th>Fetch Date</th>
                                  <th>Name</th>
                                  <th>Description</th>
                                  <th>IP Address</th>
                                </tr>
                            </table>--%>
                          </div>
                      </div>

        </div>
          </div>
      </div>
    </form>
</body>
</html>
