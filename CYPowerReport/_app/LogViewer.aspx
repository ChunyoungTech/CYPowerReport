<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="LogViewer.aspx.cs" Inherits="WebApp._app.LogViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //var extOpt = [];
        //InitExt(extOpt);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <li>查詢日期：
               
                <uc:ucDate ID="dteDateS" runat="server" />
                ~<uc:ucDate ID="dteDateE" runat="server" />
            </li>
            <li>關鍵字：<asp:TextBox ID="searchString" runat="server"></asp:TextBox></li>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
            </li>
            <%--            <li>
                <asp:Button ID="btnExport" runat="server" Text="匯出" />
            </li>--%>
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
                        <asp:BoundField DataField="SGL_Date" HeaderText="訊息時間" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SGL_MsgLog" HeaderText="訊息內容" />
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
                <%--<asp:PostBackTrigger ControlID="btnExport" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
