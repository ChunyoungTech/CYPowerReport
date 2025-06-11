<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="CompanyData.aspx.cs" Inherits="WebApp._app.CompanyData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "1066" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "9" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "15" }];
        InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>名稱：<asp:TextBox ID="txtName" runat="server"></asp:TextBox></li>
            <li>統一編號：<asp:TextBox ID="txtUniformNumbers" runat="server"></asp:TextBox></li>
            <li>電話：<asp:TextBox ID="txtTel" runat="server"></asp:TextBox></li>
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
                    <%--<AlternatingRowStyle BackColor="White" />--%>
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3em">
                            <HeaderTemplate>
                                <input type="button" class="extBtn" value="新增" data-val='0' data-idx="0" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("COD_SEQ_ID") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="COD_SEQ_ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="COD_NAME" HeaderText="名稱" SortExpression="COD_NAME" />
                        <asp:BoundField DataField="COD_ADDRESS" HeaderText="地址" SortExpression="COD_ADDRESS"/>
                        <asp:BoundField DataField="COD_Uniform_Numbers" HeaderText="統一編號" SortExpression="COD_Uniform_Numbers" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="COD_Tel" HeaderText="電話" SortExpression="COD_Tel" />
                        <asp:BoundField DataField="COD_EMail" HeaderText="EMAIL" SortExpression="COD_EMail" />
                        <asp:BoundField DataField="COD_Contact" HeaderText="聯絡" SortExpression="COD_Contact" />
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
