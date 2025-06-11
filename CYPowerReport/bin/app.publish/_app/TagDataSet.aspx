<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="TagDataSet.aspx.cs" Inherits="WebApp._app.TagDataSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "7" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "9" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "15" }];
        InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>資料點名稱：<asp:TextBox ID="txtNameQ" runat="server"></asp:TextBox>
            </li>
            <li>資料點類型：
                <asp:DropDownList ID="ddlTypeQ" runat="server">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="AI" Value="AI"></asp:ListItem>
                    <asp:ListItem Text="DI" Value="DI"></asp:ListItem>
                </asp:DropDownList>
            </li>
        </ul>
        <ul>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
            </li>
            <li style="float: right;">
                <input type="button" class="extBtn" value="來源轉入" data-val='0' data-idx="2" />
                <input type="button" class="extBtn" value="整批匯入" data-val='0' data-idx="1" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" />
                <asp:Button runat="server" Text="下載範例" OnClick="ExampleDownload" />
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
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("ID") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Tag_Name" HeaderText="資料點名稱" SortExpression="Tag_Name" />
                        <asp:BoundField DataField="Unit" HeaderText="資料點單位" SortExpression="Unit" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Tag_Type" HeaderText="資料點類型" SortExpression="Tag_Type" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="HiHi_Limit" HeaderText="HIHI警報值" SortExpression="HiHi_Limit" />
                        <asp:BoundField DataField="Hi_Limit" HeaderText="HI警報值" SortExpression="Hi_Limit" />
                        <asp:BoundField DataField="Lo_Limit" HeaderText="LO警報值" SortExpression="Lo_Limit" />
                        <asp:BoundField DataField="LoLo_Limit" HeaderText="LOLO警報值" SortExpression="LoLo_Limit" />
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
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
