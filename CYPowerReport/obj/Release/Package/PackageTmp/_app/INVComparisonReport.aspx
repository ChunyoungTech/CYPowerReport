<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="INVComparisonReport .aspx.cs" Inherits="WebApp._app.INVComparisonReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "46" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "1059" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "1060" }];
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
        setTimeout(() => {
            $("#ContentPlaceHolder1_ContentPlaceHolder1_RBL_Type_0").change(x => {
                $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2").attr("disabled", false);
                $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2")[0].checked = false
            })
            $("#ContentPlaceHolder1_ContentPlaceHolder1_RBL_Type_1").change(x => {
                $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2").attr("disabled", true);
                $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2")[0].checked = false})
            $("#ContentPlaceHolder1_ContentPlaceHolder1_RBL_Type_2").change(x => { $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2").attr("disabled", true); $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2")[0].checked = false })
            $("#ContentPlaceHolder1_ContentPlaceHolder1_RBL_Type_3").change(x => { $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2").attr("disabled", true); $("#ContentPlaceHolder1_ContentPlaceHolder1_CheckBox2")[0].checked = false })
        }, 500)
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
                <asp:RadioButtonList ID="RBL_Type" runat="server" RepeatColumns="2">
                    <asp:ListItem Text="每日" Value="每日"></asp:ListItem>
                    <asp:ListItem Text="每週" Value="每週"></asp:ListItem>
                    <asp:ListItem Text="每月" Value="每月"></asp:ListItem>
                    <asp:ListItem Text="每年" Value="每年"></asp:ListItem>
                </asp:RadioButtonList>
            </li>
            <li style="display: none">鄉鎮地區:
                <asp:CheckBoxList ID="CBL_CountyID" runat="server" RepeatColumns="4" AutoPostBack="True" OnSelectedIndexChanged="CBL_CountyID_SelectedIndexChanged"></asp:CheckBoxList>
            </li>
            <li>業主類別:
                <asp:CheckBoxList ID="CBL_SysType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CBL_SysType_SelectedIndexChanged" RepeatColumns="4"></asp:CheckBoxList>
            </li>
            <li>方向:
                <asp:CheckBoxList ID="CBL_Bearing" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CBL_Bearing_SelectedIndexChanged" RepeatColumns="2"></asp:CheckBoxList>
            </li>
            <li>監控商:
                <asp:CheckBoxList ID="CBL_Equipment" runat="server" RepeatColumns="2"></asp:CheckBoxList>
            </li>
        </ul>
        <ul>
            <li>
                <asp:CheckBox ID="CheckBox1" runat="server" RepeatColumns="2"></asp:CheckBox>
                只列低於-10%資料(低於同案場INV之平均)
            </li>
            <li>
                <asp:CheckBox ID="CheckBox2" runat="server" RepeatColumns="2"></asp:CheckBox>
                連三日異常
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
                    GridLines="Vertical" AllowSorting="True" AllowPaging="True" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="CBD_Number" HeaderText="案場編號" SortExpression="CBD_Number" />
                        <asp:BoundField DataField="CBD_Case_Name" HeaderText="案場名稱" SortExpression="CBD_Case_Name" />
                        <asp:BoundField DataField="INV_Number" HeaderText="Inv編號" SortExpression="INV_Number" />
                        <asp:BoundField DataField="INV_KW" HeaderText="INV Kw數" SortExpression="INV_KW" />
                        <asp:BoundField DataField="INV_KWP" HeaderText="INV 串併KWP" SortExpression="INV_KWP" />

                        <asp:BoundField DataField="timeSpanId" HeaderText="日期" DataFormatString="{0:yyyy/MM/dd}" />

                        <asp:BoundField DataField="INV_Power" HeaderText="INV發電量" SortExpression="INV_Power" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="INV_KWH" HeaderText="INV KWH" SortExpression="INV_KWH" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="INV_KWH_Percent" HeaderText="INV KWH差異%" SortExpression="INV_KWH_Percent" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="INV_KWHP" HeaderText="INV串併KWH" SortExpression="INV_KWHP" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="INV_KWP_Percent" HeaderText="INV串併 KWH差異%" SortExpression="INV_KWP_Percent" DataFormatString="{0:0.00}" />

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
