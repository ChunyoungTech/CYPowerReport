<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="SysRole.aspx.cs" Inherits="WebApp._sys.SysRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../_css/edit.css" rel="stylesheet" />
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .5, Sub: "4" },
                    { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .5, Sub: "5" },
                    { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "6" }
        ];
        InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="QueryArea">
        <ul>
            <li style="float:right;">
                <asp:Button ID="btnReInit" runat="server" Text="重新載入" OnClick="btnReInit_Click" />
            </li>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                    AllowSorting="true" CssClass="MainGridView"
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
                        <asp:BoundField DataField="ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3.5em" />
                        <asp:BoundField DataField="Name" HeaderText="名稱" SortExpression="Name" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="預設群組" HeaderStyle-Width="3.5em">
                            <ItemTemplate>
                                <uc:YesNo ID="ynDefault" runat="server" ValueOrginal='<%# Eval("isDefault") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="啟用" HeaderStyle-Width="3.5em">
                            <ItemTemplate>
                                <uc:YesNo ID="ynENabled" runat="server" ValueOrginal='<%# Eval("Enabled") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40">
                            <ItemTemplate>
                                <input id="btnProg" type="button" class="extBtn" value="功能" data-val='<%# Eval("ID") %>' data-t="角色" data-idx="2" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40">
                            <ItemTemplate>
                                <input id="btnUser" type="button" class="extBtn" value="使用者" data-val='<%# Eval("ID") %>' data-width="800" data-t="角色" data-idx="1" />
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
        </asp:UpdatePanel>
    </div>

</asp:Content>
