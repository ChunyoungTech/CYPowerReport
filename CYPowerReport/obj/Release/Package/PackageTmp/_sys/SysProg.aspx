<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="SysProg.aspx.cs" Inherits="WebApp._sys.SysProg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <link href="../Content/pinTabs.css" rel="stylesheet" />
    <link href="../Content/edit.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.easytabs.min.js"></script>--%>
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .5, Sub: "1" }];
        InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>分類：<asp:DropDownList ID="ddlDirQ" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </li>
            <li>啟用：<asp:DropDownList ID="ddlEnabledQ" runat="server">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                   </asp:DropDownList>
            </li>
        </ul>
        <ul>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" Visible="false" />
            </li>
            <li style="float:right;">
                <asp:Button ID="btnReInit" runat="server" Text="重新載入" OnClick="btnReInit_Click" />
            </li>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView"
                    GridLines="Vertical" AllowSorting="true" AllowPaging="true" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3em">
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("ID") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Name" HeaderText="功能名稱" />
                        <asp:BoundField DataField="DirName" HeaderText="分類" SortExpression="DirName" />
                        <asp:TemplateField HeaderText="啟用" ItemStyle-HorizontalAlign="Center" SortExpression="Enabled" >
                            <ItemTemplate>
                                <uc:YesNo ID="ucYesNo1" ValueOrginal='<%#Eval("Enabled") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Seq" HeaderText="排序" SortExpression="Seq" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <PagerTemplate>
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        <div class="NoData">查無符合條件資料</div>
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
