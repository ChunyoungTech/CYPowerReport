<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="INVPowerReport.aspx.cs" Inherits="WebApp._app.INVPowerReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectCheckBox input{
            margin-left:.5em;
        }
        .selectCheckBox label{
            min-width:2em;
        }
    </style>
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "46" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "1051" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "1052" }];
        InitExt(extOpt);

        function ReloadCityData(strValue) {
            //alert("strValue:" + strValue);
            //$("#<%=hidCity.ClientID %>").val(strValue);
            $("#<%=hidCity.ClientID %>").val(strValue);
            return __doPostBack('<%= lkbReCityData.UniqueID %>', '');
        }


        function ReloadTownData(strValue) {
            //alert("strValue:" + strValue);
            //$("#<%=hidCity.ClientID %>").val(strValue);
            $("#<%=hidCity.ClientID %>").val(strValue);
            return __doPostBack('<%= lkbReCityData.UniqueID %>', '');
        }

        function ReloadCaseBaseDataData(strValue) {
            //alert("strValue:" + strValue);
            //$("#<%=hidCaseBaseData.ClientID %>").val(strValue);
            $("#<%=hidCaseBaseData.ClientID %>").val(strValue);
            return __doPostBack('<%= lkbReCaseBaseDataData.UniqueID %>', '');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>日期：
                <uc:ucDate ID="dteDateS" runat="server" />
                ~<uc:ucDate ID="dteDateE" runat="server" />
            </li>
            <li>報表類別:
                <asp:RadioButtonList ID="RBL_Type" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox">
                    <asp:ListItem Text="每日" Value="每日"></asp:ListItem>
                    <asp:ListItem Text="每週" Value="每週"></asp:ListItem>
                    <asp:ListItem Text="每月" Value="每月"></asp:ListItem>
                    <asp:ListItem Text="每年" Value="每年"></asp:ListItem>
                </asp:RadioButtonList>
            </li>
            <li style="display: none">鄉鎮地區:
                <asp:CheckBoxList ID="CBL_CountyID" runat="server" RepeatColumns="4" AutoPostBack="True" OnSelectedIndexChanged="CBL_CountyID_SelectedIndexChanged"></asp:CheckBoxList>
            </li>
        </ul>
        <ul>
            <li>業主類別:
                <asp:CheckBoxList ID="CBL_SysType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox"></asp:CheckBoxList><%-- AutoPostBack="True" OnSelectedIndexChanged="CBL_SysType_SelectedIndexChanged" RepeatColumns="4"--%>
            </li>
        </ul>
        <ul>
            <li>方向:
                <asp:CheckBoxList ID="CBL_Bearing" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox"></asp:CheckBoxList><%-- AutoPostBack="True" OnSelectedIndexChanged="CBL_Bearing_SelectedIndexChanged" RepeatColumns="2"--%>
            </li>
        </ul>
        <ul>
            <li>監控商:
                <asp:CheckBoxList ID="CBL_Equipment" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox"></asp:CheckBoxList><%-- RepeatColumns="2"--%>
            </li>
        </ul>
        <ul>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" RenderMode="Inline">
                <ContentTemplate>
                    <li>地區：
                <asp:DropDownList ID="ddlCity" runat="server">
                </asp:DropDownList>
                        <asp:HiddenField ID="hidCity" runat="server" />
                        <input type="button" class="extBtn" value="地區選取" data-val='1' data-idx="1" />
                    </li>
                    <asp:LinkButton ID="lkbReCityData" runat="server" OnClick="lkbReCityData_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lkbReTownData" runat="server" OnClick="lkbReTownData_Click"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbReCityData" />
                </Triggers>
            </asp:UpdatePanel>

            <li style="display: none">案場名稱:
                <asp:DropDownList ID="ddlCaseName" runat="server"></asp:DropDownList>
            </li>

            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" RenderMode="Inline">
                <ContentTemplate>
                    <li>案場名稱：
                <asp:DropDownList ID="ddlCaseBaseData" runat="server">
                </asp:DropDownList>
                        <asp:HiddenField ID="hidCaseBaseData" runat="server" />
                        <input type="button" class="extBtn" value="案場選取" data-val='1' data-idx="2" />
                    </li>
                    <asp:LinkButton ID="lkbReCaseBaseDataData" runat="server" OnClick="lkbReCaseBaseDataData_Click"></asp:LinkButton>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbReCaseBaseDataData" />
                </Triggers>
            </asp:UpdatePanel>

            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
            </li>
            <li>
                <asp:Label runat="server" ForeColor="Red">備註:『所有查詢條件需同時成立，不列入條件請勿勾選』</asp:Label>
            </li>

        </ul>

    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:Chart ID="Chart1" runat="server" Height="607px" Width="1421px" Visible="False" EnableViewState="true"></asp:Chart>
                <asp:Chart ID="Chart2" runat="server" Height="607px" Width="1421px" Visible="False" EnableViewState="true"></asp:Chart>
                <asp:Chart ID="Chart3" runat="server" Height="607px" Width="1421px" Visible="False" EnableViewState="true"></asp:Chart>
                <asp:Chart ID="Chart4" runat="server" Height="607px" Width="1421px" Visible="False" EnableViewState="true"></asp:Chart>
                <asp:Chart ID="Chart5" runat="server" Height="607px" Width="1421px" Visible="False" EnableViewState="true"></asp:Chart>

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="MainGridView Grid100"
                    GridLines="Vertical" AllowSorting="True" AllowPaging="True" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:BoundField DataField="CBD_Case_Name" HeaderText="案場名稱" SortExpression="CBD_Case_Name" />
                        <asp:BoundField DataField="DATA_DATE" HeaderText="DATE" DataFormatString="{0:yyyy/MM/dd}" />

                    </Columns>
                    <PagerTemplate>
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        <div class="NoData">
                            查無符合條件資料
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <uc:Pager ID="ucPager" runat="server" TargetID="GridView1" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
