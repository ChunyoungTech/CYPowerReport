<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="ElectricityBilling.aspx.cs" Inherits="WebApp._app.ElectricityBilling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "46" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "1047" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "15" }];
        InitExt(extOpt);

        function ReloadCityData(strValue) {
            //alert("strValue:" + strValue);
            //$("#<%=hidCity.ClientID %>").val(strValue);
            $("#<%=hidCity.ClientID %>").val(strValue);
            return __doPostBack('<%= lkbReCityData.UniqueID %>', '');
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
            <li>發票日期：
                <uc:ucDate ID="invoiceDateS" runat="server" />
                ~<uc:ucDate ID="invoiceDateE" runat="server" />
            </li>
            <li>業主：
                <asp:DropDownList ID="ddlCD_TYPE" runat="server">
                </asp:DropDownList>
            </li>
            <li>公司類別：
                <asp:DropDownList ID="CompanyList" runat="server" DataTextField="COD_NAME" DataValueField="COD_SEQ_ID" Height="28"></asp:DropDownList>
            </li>
            <li>案場名稱：
                <asp:TextBox ID="txtCase_Name" runat="server"></asp:TextBox>
            </li>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
            </li>
            <li>
                <asp:Button ID="btnExport" runat="server" Text="匯出" />
            </li>
        </ul>
        <ul style="display: none">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" RenderMode="Inline">
                <ContentTemplate>
                    <li>地區：
                <asp:DropDownList ID="ddlCity" runat="server">
                </asp:DropDownList>
                        <asp:HiddenField ID="hidCity" runat="server" />
                        <input type="button" class="extBtn" value="地區選取" data-val='1' data-idx="1" />
                    </li>
                    <asp:LinkButton ID="lkbReCityData" runat="server" OnClick="lkbReCityData_Click"></asp:LinkButton>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbReCityData" />
                </Triggers>
            </asp:UpdatePanel>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView Grid100"
                    GridLines="Vertical" AllowSorting="true" AllowPaging="true" ShowHeaderWhenEmpty="true">
                    <%--<AlternatingRowStyle BackColor="White" />--%>
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3em">
                            <HeaderTemplate>
                                <input type="button" class="extBtn" value="新增" data-val='0' data-idx="0" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("EC_SEQ_ID") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EC_SEQ_ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" Visible="False" />
                        <asp:BoundField DataField="CBD_Case_Name" HeaderText="案場名稱" SortExpression="CBD_Case_Name" />
                        <asp:BoundField DataField="COD_NAME" HeaderText="公司類別" SortExpression="COD_NAME" />
                        <asp:BoundField DataField="CBD_KW" HeaderText="kw數" SortExpression="CBD_KW" />
                        <asp:BoundField DataField="InvoiceDate" HeaderText="發票日期" SortExpression="InvoiceDate" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Last_Meter_Reading" HeaderText="計費時間(起)" SortExpression="Last_Meter_Reading" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Current_Meter_Reading" HeaderText="計費時間(止)" SortExpression="Current_Meter_Reading" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="EC_Days" HeaderText="天數" SortExpression="EC_Days" />
                        <asp:BoundField DataField="EC_Meter_Record" HeaderText="抄表紀錄公司購電-度數" SortExpression="EC_Meter_Record" />
                        <asp:BoundField DataField="EC_Calculation_Rate" HeaderText="電費計算費率" SortExpression="EC_Calculation_Rate" />
                        <asp:BoundField DataField="EC_Calculation_Record" HeaderText="電費計算計費度數" SortExpression="EC_Calculation_Record" />
                        <asp:BoundField DataField="EC_Calculation_Amt" HeaderText="電費計算金額" SortExpression="EC_Calculation_Amt" />
                        <asp:BoundField DataField="EC_Daily_Amount" HeaderText="日金額" SortExpression="EC_Daily_Amount" />
                        <asp:BoundField DataField="EC_Duarantee_Rate" HeaderText="日保發度數" SortExpression="EC_Duarantee_Rate" />
                        <asp:BoundField DataField="EC_Daily_Billing" HeaderText="日計費度數" SortExpression="EC_Daily_Billing" />
                        <asp:BoundField DataField="EC_Check_Amount" HeaderText="驗算" SortExpression="EC_Check_Amount" />
                        <asp:BoundField DataField="electricNumber" HeaderText="電號" SortExpression="electricNumber" />
                        <asp:BoundField DataField="meterUpDate" HeaderText="掛錶日期" SortExpression="meterUpDate" />
                    </Columns>
                    <%--                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />--%>
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
