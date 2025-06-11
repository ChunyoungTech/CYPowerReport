<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="SysUser.aspx.cs" Inherits="WebApp._sys.SysUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/pinTabs.css" rel="stylesheet" />
    <link href="../Content/edit.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.easytabs.min.js"></script>
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .5, Sub: "3" }];
        InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>部門：<uc:ucDept ID="ddlDeptQ" runat="server" isShowAll="true" />
            </li>
            <li>姓名：<asp:TextBox ID="txtNameQ" runat="server"></asp:TextBox>
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView"
                    GridLines="Vertical" ShowHeaderWhenEmpty="true" AllowPaging="true">
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
                        <asp:BoundField DataField="Code" HeaderText="帳號" />
                        <asp:BoundField DataField="Name" HeaderText="姓名" />
                        <asp:BoundField DataField="DeptName" HeaderText="部門" SortExpression="DeptName" />
                        <asp:TemplateField HeaderText="主管" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <uc:YesNo ID="ucYesNo1" ValueOrginal='<%#Eval("isManager") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="啟用" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <uc:YesNo ID="ucYesNo2" ValueOrginal='<%#Eval("Enabled") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
