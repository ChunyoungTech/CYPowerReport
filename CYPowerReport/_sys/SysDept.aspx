<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="SysDept.aspx.cs" Inherits="WebApp._sys.SysDept" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/pinTabs.css" rel="stylesheet" />
    <link href="../Content/edit.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.easytabs.min.js"></script>
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .5, Sub: "2" }];
        InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="QueryArea">
                <ul>
                    <li>部門：<uc:ucDept ID="ddlDeptQ" runat="server" isShowAll="true" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <asp:Button ID="btnQuery" runat="server" Text="查詢" />
                        <asp:Button ID="btnExport" runat="server" Text="匯出" Visible="false" />
                    </li>
                </ul>
            </div>
            <div class="GridArea">
                <asp:LinkButton ID="lbRefresh" runat="server" OnClick="lbRefresh_Click" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView" AllowPaging="true"
                    GridLines="Vertical" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3em">
                            <HeaderTemplate>
                                <input type="button" class="extBtn" value="新增" data-val='0' data-idx="0" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("ID") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="序號" SortExpression="ID" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Code" HeaderText="部門代號" SortExpression="Code" />
                        <asp:BoundField DataField="Name" HeaderText="部門名稱" SortExpression="Name" />
                        <asp:BoundField DataField="UpperName" HeaderText="上級部門" SortExpression="UpperName" />
                    </Columns>
                    <PagerTemplate>
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        <div class="NoData">查無符合條件資料</div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <uc:Pager ID="ucPager" runat="server" TargetID="GridView1" />
            </div>
        </ContentTemplate>
<%--        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
