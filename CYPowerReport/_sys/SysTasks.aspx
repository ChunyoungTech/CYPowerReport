<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="SysTasks.aspx.cs" Inherits="WebApp._sys.SysTasks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Literal ID="ltlSysTask" runat="server"></asp:Literal>
    </div>
    <div class="QueryArea">
        <ul>
            <li>
                <asp:LinkButton ID="lbRefresh" runat="server" Text="Refresh" />
                <%--<asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />--%>
            </li>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <input type="hidden" id="hidRefresh" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                    AllowSorting="true" CssClass="MainGridView"
                    GridLines="Vertical" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:BoundField DataField="Key" HeaderText="Key" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Name" HeaderText="名稱" />
                        
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="啟動" ItemStyle-Width="2.5em">
                            <ItemTemplate>
                                <uc:YesNo ID="ynStarted" runat="server" ValueOrginal='<%# Eval("IsStarted") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LastExec" HeaderText="最近執行時間" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10em" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="成功" ItemStyle-Width="2.5em">
                            <ItemTemplate>
                                <uc:YesNo ID="ynSuccess" runat="server" ValueOrginal='<%# Eval("Success") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Message" HeaderText="訊息" />
                    </Columns>
                    <PagerTemplate>
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        <div class="NoData">查無符合條件資料</div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <%--<uc:Pager ID="ucPager" runat="server" TargetID="GridView1" />--%>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lbRefresh" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
