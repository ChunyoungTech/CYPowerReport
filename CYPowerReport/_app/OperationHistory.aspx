<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="OperationHistory.aspx.cs" Inherits="WebApp._app.OperationHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "1048", Table:strTable}];
        MyInitExt(extOpt);

        var strTable = "";

        function MyInitExt(o) {
            gOpt = o;
            $.extend(jQuery.fancybox.defaults, { parent: $('form:first'), type: "iframe", scrollOutside: false, closeBtn: false, autoSize: true, padding: 5 });
            var ohelpers = { overlay: { closeClick: false, locked: false, css: { 'background-color': 'Gray', 'opacity': 0.3 } }, title: { type: 'inside', position: 'top' } };

            $(document).on("click", ".extBtn", function (e) { OpenExtWindow($(this)); e.preventDefault(); });
            $(document).on('click', '.OpenWindowTitle a', function () { $.fancybox.close(); });

            function OpenExtWindow(x) {
                if (!(x.attr("data-val") == undefined || x.attr("data-idx") == undefined || gOpt == undefined)) {
                    gIdx = parseInt(x.attr("data-idx"));
                    if (gIdx != isNaN) {
                        if (gOpt[gIdx] != null) {
                            var strValue=x.attr("data-val");
                            var arrValue = strValue.split("-");
                            if (arrValue.length != 2) {
                                alert("Log資料有誤，無法瀏覽明細!");
                                return;
                            }
                            else {
                                if (arrValue[1] == "") {
                                    alert("Log資料有誤，無法瀏覽明細!");
                                    return;
                                }
                            }
                            $.fancybox.open(
                                {
                                    title: "<div class='OpenWindowTitle'><span>" + (x.attr("data-t") == undefined ? x.val() : x.attr("data-t")) + "</span><a href='#'><img src='../_img/window_off.png' style='height:1.5em;float:right;' /><a/></div>",
                                    href: "../_edit/open.aspx?pa=" + arrValue[1] + "&app=" + (x.attr("data-app") == undefined ? $.url().param("app") : x.attr("data-app")) + "&sub=" + gOpt[gIdx].Sub + "&table=" + arrValue[0],
                                    width: (x.attr("data-width") == undefined ? gOpt[gIdx].Width * gWidth : x.attr("data-width")),
                                    minHeight: (x.attr("data-height") == undefined ? gHeight * .5 : gHeight * x.attr("data-height")),
                                    beforeShow: function () {
                                        $(".MainGridView .gr_select").removeClass("gr_select");
                                        x.parentsUntil(".MainGridView tbody").addClass("gr_select");
                                        this.wrap.tinyDraggable();
                                    }
                                }, { helpers: ohelpers }
                            );
                        }
                    }
                }
            }
        }

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
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3em" >
                            <HeaderTemplate>
                                <asp:Label Text="記錄" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="瀏覽" data-val='<%# Eval("編號") %>' data-idx="0" />
                            </ItemTemplate>
                        </asp:TemplateField>

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
