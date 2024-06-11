<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" Inherits="WebAppForm.WorkOrderDetail" Title="WorkOrder Detail" CodeBehind="WorkOrderDetail.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajax" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
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
    </style>

    <div style="width: 460px; font-family: Segoe UI Light;">
        <div class="heading">WorkOrder Detail</div>


        <div style="width: 100%; padding: 5px; padding-left: 0px;">
            <span style="font-family: Segoe UI Light; font-size: Small; padding-right: 2px; font-weight: bold;">WONUM:</span>
            <asp:DropDownList ID="ddlWONUM" runat="server" CssClass="textfield" Width="100" Height="28"
                AutoPostBack="true" OnSelectedIndexChanged="ddlWONUM_SelectedIndexChanged">
            </asp:DropDownList>
            <a href='javascript:void(0);' id='lnkimgattch' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px; padding-left: 10px;">Images</a>
            <%--<a href='javascript:void(0);' id='myPopUp' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Dispatch WO</a>--%>
            <a href='javascript:void(0);' id='AssoVehPopUp' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Associated Vehicles</a>

            <%-- <a href='javascript:void(0);' id='linkDispatchWO' style="font-family: Segoe UI Light; font-size: Small; color: white; padding-right: 4px;">Dispatch WO</a>--%>
            <asp:LinkButton ID="linkDispatchWO" runat="server" Text="Dispatch WO" Style="padding-right: 5px;" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White"></asp:LinkButton>

            <asp:LinkButton ID="lnkWorkLog" runat="server" Text="Work Logs" Style="padding-right: 5px;" Font-Names="Segoe UI Light" 
                Font-Size="Small" ForeColor="White" OnClick="lnkWorkLog_Click"></asp:LinkButton>

            <asp:LinkButton ID="lnkbtnRefresh" Text="Refresh" runat="server"
                Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" Style="padding-right: 4px;" OnClick="lnkbtnRefresh_Click" />

            <asp:LinkButton ID="lnkbtnback" runat="server" Text="Back" Visible="false" Font-Names="Segoe UI Light" Font-Size="Small" ForeColor="White" Style="float: right; padding-right: 50px; padding-top: 5px;" />
        </div>
        <input type="button" id="smsBtn" class="simplebutton1" value="MapLink/SMS" />

    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ODWConnectionString %>"
        SelectCommand="SELECT [WONUM]
      ,[Status_Description] AS [EAMS_Status]
                  ,[EXT_FRAMEZONE] as FrameWorkZone
      ,[EXT_DEFECTTYPE] as DEFECTTYPE
            ,[Owner]
      ,[Priority]
      ,[WODescription]
      ,[PARENT]
      ,[WORKTYPE]     
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
      ,[REPORTEDBY]
      ,[SR_Report_Date]
      ,[WO_Report_Date]
      ,[Job_Plan_No]
      ,[ONBEHALFOF]
      ,[Contact_Phone]
      ,[Contract_ID]
      ,[Vendor]
      ,[Crew_Lead_Name] + ' (' + EXT_CREWLEAD + ') ' AS Crew_Lead
      ,[Scheduler_Name] + ' (' + EXT_SCHEDULER + ') ' AS Scheduler
      ,[Engineer_Name]
      ,[Section_Name]
      ,[Unit_Name]
      ,[EXT_DNMC] AS WO_Created_From
      ,[CSR_Group]
      ,[Related_SR]
       FROM [ODW].[GIS].[vwWorkOrderBase] where ([WONUM] = @WONUM)"
        OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlWONUM" DbType="String" Name="WONUM" />

        </SelectParameters>
    </asp:SqlDataSource>

    <asp:DetailsView ID="DetailsView1" CaptionAlign="Top" Font-Name="Segoe UI Light" Style="min-width: 200px;"
        Font-Bold="True" Font-Size="18px" CellPadding="3" CellSpacing="3" BorderColor="#4C636F" AutoGenerateRows="true"
        runat="server" Height="50px" Width="450px" DataSourceID="SqlDataSource1" Font-Names="Segoe UI Light">
        <RowStyle CssClass="textfield" />
        <AlternatingRowStyle CssClass="textfieldalt" />
    </asp:DetailsView>


    <script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui.js"></script>

    <script type="text/javascript">

        $(function () {

            $('#linkDispatchWO').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                var isframework = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(5)").text();
                debugger;

                if (isframework.trim()) {
                    window.open("DispatchWOBAU.aspx?WONUM=" + str, "_blank", "width=730,height=500");

                } else {
                    window.open("DispatchWO.aspx?WONUM=" + str, "_blank", "width=730,height=500");
                }

            });
            var popup;
            $('#smsBtn').click(function () {
                var Id = '<%= Request.QueryString["ID"] %>';
                var lat = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(45)").text();
                var lon = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(47)").text();
                var ph = $("#ContentPlaceHolder1_DetailsView1").find("td:eq(67)").text();
                if (!ph.startsWith("974")) {
                    ph = "974" + ph;
                }
                var isWorkOrder = "1";
                debugger;
                popup = window.open("MapsLinkView.aspx?lat=" + lat + "&long=" + lon + "&ph=" + ph + "&Id=" + Id + "&isWorkOrder=" + isWorkOrder, + "Popup", "width=600,height=500");
                popup.focus();
            });

            $('#changeOwner').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                //  alert(str);
                window.open("UpdateWorkOrder.aspx?WONUM=" + str, "_blank", "width=730,height=500");
            });

            $('#lnkimgattch').click(function () {
                var str = '<%= Request.QueryString["ID"] %>';
                //  alert(str);
                window.open("GetImgAttachByWo.aspx?ID=" + str, "_blank", "width=760,height=500");
            });

            $('#AssoVehPopUp').click(function () {
                var strWO = '<%= ddlWONUM.SelectedValue %>';
                var strUser = String.raw`<%= Request.QueryString["USER"] %>`.replace(/\\/g, "\\");
                //  alert(str);
                window.open("AssociatedVehicles.aspx?WONUM=" + strWO + "&USER=" + strUser, "_blank", "width=690,height=400");
            });

        });

    </script>




</asp:Content>
