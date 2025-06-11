<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="SysOperationLog.aspx.cs" Inherits="WebApp._sys.SysOperationLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="QueryArea">
        <ul>
            <%--<li>分類：<asp:DropDownList ID="ddlDirQ" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </li>--%>
            <li>日期區間：
                <uc:ucDate ID="dteDateS" runat="server" />~<uc:ucDate ID="dteDateE" runat="server" />
            </li>
        </ul>
        <ul>
            <li>
                操作功能：<asp:DropDownList ID="ddlProgQ" runat="server" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </li>
            <li>
                操作類型：<asp:DropDownList ID="ddlTypeQ" runat="server" DataTextField="TYPE" DataValueField="TYPE"></asp:DropDownList>
            </li>
        </ul>
        <ul>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
            </li>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView Grid100"
                    GridLines="Vertical" AllowSorting="true" AllowPaging="true" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:BoundField DataField="SYS_PROG_ID" HeaderText="操作功能" SortExpression="SYS_PROG_ID" />
                        <asp:BoundField DataField="OPERATION_TYPE" HeaderText="操作類型" SortExpression="OPERATION_TYPE" />
                        <asp:BoundField DataField="OPERATION_USER" HeaderText="操作人員" SortExpression="OPERATION_USER" />
                        <asp:BoundField DataField="OPERATION_TIME" HeaderText="日期時間" SortExpression="OPERATION_TIME" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                        <asp:BoundField DataField="OPERATION_DESC" HeaderText="操作記錄" />
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
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
