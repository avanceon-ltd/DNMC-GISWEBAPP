<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FIFAStadiumComplaints.aspx.cs" Inherits="WebAppForm.FIFAStadiumComplaints" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FIFA Stadium detail</title>
    <meta http-equiv="refresh" content="1800">
     
        <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>

     <style type="text/css">
      
    <style>
       .card {
  background-color: transparent;
  color: white;
  text-align:left;
  /*padding: 1rem;
  height: 4rem;*/
}

.cards {
  max-width: 1200px;
  /*margin: 0 auto;*/
  display: grid;
  grid-gap: 1rem;
}

/* Screen larger than 600px? 2 column */
@media (min-width: 600px) {
  .cards { grid-template-columns: repeat(2, 1fr); }
}

/* Screen larger than 900px? 3 columns */
@media (min-width: 900px) {
  .cards { grid-template-columns: repeat(3, 1fr); }
}
    </style>
</head>
<body style="background-color: #022130; font-family: Segoe UI Light; color: white;">
    <%--<form id="form1" runat="server">
    <table>
        <tr>
            <th style="text-align:left;">Rain Water Complaints</th><th> <Button ID="btnRefresh"   OnClick="ReloadPage()"  style=" color: #fff;background-color: #154670; height: 27px;width: 90px;border: solid 1px #fff;font-size: 15px;font-weight: bold; border-radius: 2px 2px 2px 2px;">Refresh</Button></th>
        </tr>
        <tr>
            <td>UnAssigned</td><td><asp:TextBox ID="txtRainUnAssigned" runat="server"  class="textfield"></asp:TextBox></td>
        </tr>
        <tr>
            <td>In-Progress</td><td><asp:TextBox ID="txtRainInPregress"  runat="server" class="textfield"></asp:TextBox></td>
        </tr>
        <tr>
            <th>Non Rain Water Complaints</th>
        </tr>
        <tr>
            <td>UnAssigned</td><td><asp:TextBox ID="txtNonRainUnAssigned" runat="server"  class="textfield"></asp:TextBox></td>
        </tr>
        <tr>
            <td>In-Progress</td><td><asp:TextBox ID="txtNonRainInPregress" runat="server"  class="textfield"></asp:TextBox></td>
        </tr>
    </table>
        </form>--%>
    
   <form id="form1" runat="server" >
     
          <div class="cards">
              <div class="card">
                  <button id="btnRefresh"   OnClick="ReloadPage()"  style="margin-bottom:12px;color: #fff;background-color: #154670; height: 27px;width: 90px;border: solid 1px #fff;font-size: 15px;font-weight: bold; border-radius: 2px 2px 2px 2px;">Refresh</button>
                   <br /> <label style="border: solid 1px #fff;">View: <label id="lblStadium" runat="server"></label></label><br /> 
                    <div style="margin-top:12px;">
                        <asp:LinkButton id="rainWater" runat="server" OnClick="btn_RainWater" style="color:#fff;" Text="Rain Water"/>
                        <asp:LinkButton id="nonRainWater" runat="server" OnClick="btn_NonRainWater" style="color:#fff;margin-left:60px;" Text="Non-Rain Water"/>
                    </div>
              </div>
              <div class="card" style="margin-left:6rem;">
                  <div style="text-align:center;"><label id="txtLabel" runat="server"></label> <label id="txtCount" runat="server"></label></div>
                   <asp:GridView ID="gvOwner" runat="server" AutoGenerateColumns="false"  Font-Size="12px" Font-Names="Segoe UI Light"
                CellPadding="10" CellSpacing="10" AllowPaging="true">
                <Columns>
                    <%--<asp:BoundField DataField="Stadium_Name" HeaderText="Stadium Name" ItemStyle-Width="50" />--%>
                    <asp:BoundField DataField="UnAssigned" HeaderText="UnAssigned" ItemStyle-Width="50" />
                    <asp:BoundField DataField="Assigned" HeaderText="Assigned" ItemStyle-Width="50" />
                    <asp:BoundField DataField="UnAttended" HeaderText="UnAttended" ItemStyle-Width="50" />
                    <asp:BoundField DataField="InProg" HeaderText="In-Progress" ItemStyle-Width="50" />
                    <asp:BoundField DataField="Complete" HeaderText="Work-Complete" ItemStyle-Width="50" />
                    <asp:BoundField DataField="Resolved" HeaderText="Resolved" ItemStyle-Width="50" />
                   
                </Columns>

                <HeaderStyle BackColor="#3a5570" />
                <RowStyle BackColor="#001119" ForeColor="white" Font-Names="Segoe UI Light" />
                <AlternatingRowStyle BackColor="#022130" ForeColor="white" Font-Names="Segoe UI Light" />
            </asp:GridView>
              </div>
          </div>

    </form>
    <script>
        function ReloadPage() {
            window.location.reload();
        }
    </script>
</body>
</html>

