<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="CaseBaseData.aspx.cs" Inherits="WebApp._app.CaseBaseData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "45" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "1056" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "15" }];
        InitExt(extOpt);

        function MM() {
            var CDType = $("#<%= VCDType.ClientID %>").find("option:selected").text();
            $("#<%= VOwner.ClientID %>").empty();
            $("#<%= VOwner.ClientID %>").append("<option value=''></option>");
            $.each(cd_data, function (index, value) {
                if (value[2] == CDType || CDType == '') $("#<%= VOwner.ClientID %>").append("<option value='" + value[0] + "'>" + value[1] + "</option>");
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>案場名稱：<asp:TextBox ID="txtCaseName" runat="server"></asp:TextBox></li>

            <li>業主：
                <asp:DropDownList ID="VOwner" runat="server" DataTextField="CD_NAME" DataValueField="CD_SEQ_ID" Height="28"></asp:DropDownList>
            </li>
            <li>公司類別：
                <asp:DropDownList ID="CompanyList" runat="server" DataTextField="COD_NAME" DataValueField="COD_SEQ_ID" Height="28"></asp:DropDownList>
            </li>
            <li style="display: none">業主類別：
                <asp:DropDownList ID="VCDType" runat="server" DataTextField="sysType" DataValueField="sysType" Height="28" onchange="MM()"></asp:DropDownList>
            </li>


            <li>縣市：
                <asp:DropDownList ID="VCity" runat="server" DataTextField="C_City_Name" DataValueField="C_City_ID" Height="28"></asp:DropDownList>
            </li>
            <li>電壓類型：
                <asp:DropDownList ID="VType" runat="server" DataTextField="VType" DataValueField="VType" Height="28"></asp:DropDownList>
            </li>
            <br>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
            </li>
            <li>
                <asp:Button ID="btnExport" runat="server" Text="匯出" />
            </li>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView Grid100"
                    GridLines="Vertical" AllowSorting="true" AllowPaging="true" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3em">
                            <HeaderTemplate>
                                <input type="button" class="extBtn" value="新增" data-val='0' data-idx="0" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("CBD_SEQ_ID") %>' data-idx="0" />
                                <input type="button" class="extBtn" value="逆變器" data-val='<%# Eval("CBD_SEQ_ID") %>' data-idx="1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CBD_SEQ_ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="COD_NAME" HeaderText="公司類別" SortExpression="COD_NAME" />
                        <asp:BoundField DataField="CBD_Case_Name" HeaderText="案場名稱" SortExpression="CBD_Case_Name" />
                        <asp:BoundField DataField="CD_NAME" HeaderText="案場業主" SortExpression="CD_NAME" />
                        <asp:BoundField DataField="CBD_Equipment_Brand" HeaderText="監控廠商" SortExpression="CBD_Equipment_Brand" />
                        <asp:BoundField DataField="C_City_Name" HeaderText="縣市" SortExpression="C_City_Name" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CBD_TownName" HeaderText="鄉鎮" SortExpression="CBD_TownName" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CBD_Address" HeaderText="地址" />
                        <asp:BoundField DataField="CBD_GPS" HeaderText="定位點" />
                        <asp:BoundField DataField="CBD_KW" HeaderText="KW數" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="CBD_Slices" HeaderText="片數" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="MM_Module_Model" HeaderText="模組型號" SortExpression="MM_Module_Model" />
                        <asp:BoundField DataField="CBD_Bearing" HeaderText="方位" SortExpression="CBD_Bearing" />
                        <asp:BoundField DataField="CBD_Case_Type" HeaderText="案場類型" SortExpression="CBD_Case_Type" />
                        <asp:BoundField DataField="CBD_Voltage_Type" HeaderText="電壓型態" SortExpression="CBD_Voltage_Type" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CBD_Remarks" HeaderText="備註" SortExpression="CBD_Remarks" />
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
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
