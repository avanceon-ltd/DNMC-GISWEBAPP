<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.WorkOrderDetail" Title="WorkOrder Detail" CodeBehind="WorkOrderDetail.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
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

        #ParentDIV {
            width: 50%;
            height: 100%;
            font-size: 14px;
            font-family: "Segoe UI Light",Arial;
            background-color: aqua;
            color: white;
        }



        #myImg {
            border-radius: 5px;
            cursor: pointer;
            transition: 0.3s;
        }

            #myImg:hover {
                opacity: 0.7;
            }

        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.9); /* Black w/ opacity */
        }

        /* Modal Content (image) */
        .modal-content {
            margin: auto;
            display: block;
            width: 80%;
            max-width: 700px;
        }

        /* Caption of Modal Image */
        #caption {
            margin: auto;
            display: block;
            width: 80%;
            max-width: 700px;
            text-align: center;
            color: #ccc;
            padding: 10px 0;
            height: 150px;
        }

        /* Add Animation */
        .modal-content, #caption {
            -webkit-animation-name: zoom;
            -webkit-animation-duration: 0.6s;
            animation-name: zoom;
            animation-duration: 0.6s;
        }

        @-webkit-keyframes zoom {
            from {
                -webkit-transform: scale(0)
            }

            to {
                -webkit-transform: scale(1)
            }
        }

        @keyframes zoom {
            from {
                transform: scale(0)
            }

            to {
                transform: scale(1)
            }
        }

        /* The Close Button */
        .close {
            position: absolute;
            top: 15px;
            right: 35px;
            color: #f1f1f1;
            font-size: 40px;
            font-weight: bold;
            transition: 0.3s;
        }

            .close:hover,
            .close:focus {
                color: #bbb;
                text-decoration: none;
                cursor: pointer;
            }

        img {
            cursor: pointer;
        }
    </style>

    <div style="width: 460px; font-family: Segoe UI Light;">
        
        <div style="text-align: left; font-family: Segoe UI Light; font-size: Small;">WorkOrder Details 
           <%--<a href='javascript:void(0);' id='changeOwner' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">[Change Owner/Priority]</a>--%>
        <div style="width: 100%; padding: 5px; padding-left: 0px;">
            <a href='javascript:void(0);' id='lnkimgattch' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Images</a>
            <%--<a href='javascript:void(0);' id='myPopUp' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Dispatch WO</a>--%>
            <a href='javascript:void(0);' id='AssoVehPopUp' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Associated Vehicles</a>
            <%-- <asp:LinkButton ID="lnkbtnAssoVech" runat="server" style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;" Text="Associated Vehicles"  />--%>

            <asp:LinkButton ID="lnkbtnRefresh" Text="Refresh" runat="server" 
                Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" Style="padding-right: 4px;" OnClick="lnkbtnRefresh_Click" />
            <asp:LinkButton ID="lnkbtnback" runat="server" Text="Back" Visible="false" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" Style="float: right; padding-right: 10px;" />
        </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
            SelectCommand="SELECT [WONUM]
      ,[Status_Description]
      ,[WODescription]
      ,[PARENT]
      ,[WORKTYPE]
      ,[Attachements]
      ,[CRMS_REFID]      
      ,[WOStatusDate]
      ,[Location_Description]
      ,[ASSETNUM]
      ,[Asset_Description]
      ,[Asset_Condition_OnSite]
      ,[Classification_Description]
      ,[Internal_Priority]
      ,[Zone]
      ,[District]
      ,[Street]
      ,[Municipality]
      ,[Build_Elec_Water]
      ,[LATITUDEY]
      ,[LONGITUDEX]
      ,[Target_Start_Date]
      ,[Target_End_Date]
      ,[Schedule_Start_Date]
      ,[Schedule_End_Date]
      ,[Actual_Start_Date]
      ,[Actual_End_Date]
      ,[REPORTEDBY]
      ,[SR_Report_Date]
      ,[WO_Report_Date]
      ,[Job_Plan_No]
      ,[ONBEHALFOF]
      ,[Contact_Phone]
      ,[Contract_ID]
      ,[Vendor]
      ,[Crew_Lead_Name]
      ,[Scheduler_Name]
      ,[Inspector_Name]
      ,[Engineer_Name]
      ,[Section_Name]
      ,[Unit_Name]
      ,[Group_Area]
      ,[CSR_Group]
      ,[Related_SR]
      ,[Owner]
      ,[Priority] FROM [ODW].[GIS].[vwWorkOrderBase] where ([WONUM] = @ID)"
            OnSelecting="SqlDataSource1_Selecting">
            <SelectParameters>
                <asp:QueryStringParameter DbType="String" DefaultValue="10413922" Name="ID" QueryStringField="ID" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:DetailsView ID="DetailsView1" CaptionAlign="Top" Font-Name="Segoe UI Light" Style="min-width: 200px;" Font-Bold="true" Font-Size="18px" CellPadding="3" CellSpacing="3" BorderColor="#4c636f" runat="server" Height="50px" Width="450px" DataSourceID="SqlDataSource1">

            <RowStyle CssClass="textfield" />
            <AlternatingRowStyle CssClass="textfieldalt" />
        </asp:DetailsView>
    </div>

    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.js"></script>

    <script type="text/javascript">

        $(function () {
            //$("#tabs").tabs();


            $('#myPopUp').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                //  alert(str);
                window.open("DispatchWO.aspx?WONUM=" + str, "Popup", "width=730,height=500");
            });

            $('#changeOwner').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                //  alert(str);
                window.open("UpdateWorkOrder.aspx?WONUM=" + str, "Popup", "width=730,height=500");
            });

            
            $('#lnkimgattch').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                //  alert(str);
                window.open("GetImgAttachByWo.aspx?ID=" + str, "Popup", "width=760,height=500");
            });

            $('#AssoVehPopUp').click(function () {
                var strWO = '<%= Request.QueryString["ID"] %>';
                var strUser = '<%= Request.QueryString["USER"] %>';
                    //  alert(str);
                window.open("AssociatedVehicles.aspx?WONUM=" + strWO + "&USER=" + strUser , "Popup", "width=690,height=400");
                });

        });

    </script>


    

</asp:Content>
