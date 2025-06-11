<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="CutomerData.aspx.cs" Inherits="WebApp._app.CutomerData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "43" },
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
            <li>業主清單：<asp:DropDownList ID="VCDType" runat="server" DataTextField="sysType" DataValueField="sysType" Height="28"></asp:DropDownList></li>
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
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("CD_SEQ_ID") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CD_SEQ_ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CD_NAME" HeaderText="名稱" SortExpression="CD_NAME" />
                        <asp:BoundField DataField="CD_ADDRESS" HeaderText="地址" SortExpression="CD_ADDRESS"/>
                        <asp:BoundField DataField="CD_Uniform_Numbers" HeaderText="統一編號" SortExpression="CD_Uniform_Numbers" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CD_Tel" HeaderText="電話" SortExpression="CD_Tel" />
                        <asp:BoundField DataField="CD_EMail" HeaderText="EMAIL" SortExpression="CD_EMail" />
                        <asp:BoundField DataField="CD_Contact" HeaderText="聯絡" SortExpression="CD_Contact" />
                        <asp:BoundField DataField="CD_TYPE" HeaderText="業主清單" SortExpression="CD_TYPE" />
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
