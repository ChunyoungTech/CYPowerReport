<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Grid.master" AutoEventWireup="true" CodeBehind="NotStandardList.aspx.cs" Inherits="WebApp._app.NotStandardList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectCheckBox input{
            margin-left:.5em;
        }
        .selectCheckBox label{
            min-width:2em;
        }
    </style>
    <script type="text/javascript">
        var extOpt = [{ reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .7, Sub: "1057", Da: strDate },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .6, Sub: "1064" },
            { reCtl: "#<%=lbRefresh.ClientID%>", reHid: "#hidRefresh", Width: .8, Sub: "0" }];

        InitExt(extOpt);
        //MyInitExt(extOpt);

        var strDate = "";

        function ReloadTownData(strValue) {
            $("#<%=hidCity.ClientID %>").val(strValue);
            return __doPostBack('<%= lkbReCityData.UniqueID %>', '');
        }

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
                            var strValue = x.attr("data-val");
                            var arrValue = strValue.split("_");
                            $.fancybox.open(
                                {
                                    title: "<div class='OpenWindowTitle'><span>" + (x.attr("data-t") == undefined ? x.val() : x.attr("data-t")) + "</span><a href='#'><img src='../_img/window_off.png' style='height:1.5em;float:right;' /><a/></div>",
                                    href: "../_edit/open.aspx?pa=" + arrValue[1] + "&app=" + (x.attr("data-app") == undefined ? $.url().param("app") : x.attr("data-app")) + "&sub=" + gOpt[gIdx].Sub + "&da=" + arrValue[0],
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
            <li>查詢日期：
                <uc:ucDate ID="dteDateS" runat="server" />
                ~<uc:ucDate ID="dteDateE" runat="server" />
            </li>
        </ul>
        <ul>
            <li>業主類別：
                <asp:CheckBoxList ID="CBL_SysType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox"></asp:CheckBoxList>
            </li>
        </ul>
        <ul>
            <li>方向：
                <asp:CheckBoxList ID="CBL_Bearing" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox"></asp:CheckBoxList>
            </li>
        </ul>
        <ul>
            <li>監控商：
                <asp:CheckBoxList ID="CBL_Equipment" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox"></asp:CheckBoxList>
            </li>
        </ul>
        <ul>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" RenderMode="Inline">
                <ContentTemplate>
            <li>地區：
                <asp:DropDownList ID="ddlCity" runat="server">
                </asp:DropDownList>
                <asp:HiddenField ID="hidCity" runat="server" />
                <input type="button" class="extBtn" value="地區選取" data-val='1_1' data-idx="1" />
            </li>
                <asp:LinkButton ID="lkbReCityData" runat="server" OnClick="lkbReCityData_Click"></asp:LinkButton>
                <asp:LinkButton ID="lkbReTownData" runat="server" OnClick="lkbReTownData_Click"></asp:LinkButton>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbReCityData" />
                </Triggers> 
            </asp:UpdatePanel>
            <li>案場名稱：<asp:TextBox ID="txtCaseName" runat="server"></asp:TextBox></li>
            <li>顯示條件：
                <asp:DropDownList ID="VStandard" runat="server" Height="28">
                    <asp:ListItem Selected="True" Value="0">顯示所有</asp:ListItem>
                    <asp:ListItem Value="1">發電量未上傳</asp:ListItem>
                    <asp:ListItem Value="2">發電小時<3.3</asp:ListItem>
                    <asp:ListItem Value="3">PR值<80</asp:ListItem>
                    <asp:ListItem Value="4">平均發電小時比較異常</asp:ListItem>
                    <asp:ListItem Value="5">顯示所有異常</asp:ListItem>
                </asp:DropDownList>
            </li>
            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" />
            </li>
            <li>
                <asp:Button ID="btnExport" runat="server" Text="匯出" />
                <asp:Button ID="btnExport2" runat="server" Text="匯出報表" OnClick="btnExport2_Click" />
            </li>
        </ul>
    </div>
    <div class="GridArea">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:LinkButton ID="lbRefresh" runat="server" />
                <input type="hidden" id="hidRefresh" value="" /><input type="hidden" id="hidGuid" value="" />
                <asp:Chart ID="Chart1" runat="server" Height="607px" Width="1421px" Visible="False" EnableViewState="true"></asp:Chart>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView Grid100"
                    GridLines="Vertical" AllowSorting="true" AllowPaging="true" ShowHeaderWhenEmpty="true"  OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3em">
                            <HeaderTemplate></HeaderTemplate>
                            <ItemTemplate>
                                <input type="button" class="extBtn" value="編輯" data-val='<%# Eval("SID") %>' data-idx="0" data-height=".9" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CBD_SEQ_ID" HeaderText="序號" ItemStyle-HorizontalAlign="Center" visible="false"/>
                        <asp:BoundField DataField="CBD_Equipment_Brand" HeaderText="監控廠商"  SortExpression="CBD_Equipment_Brand" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="CD_TYPE" HeaderText="業主類別" ItemStyle-HorizontalAlign="Center" visible="false"/>
                        <asp:BoundField DataField="CD_NAME" HeaderText="業主名稱" SortExpression="CD_NAME" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="CBD_Case_Name" HeaderText="案場名稱" SortExpression="CBD_Case_Name" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="City_Name" HeaderText="縣市" SortExpression="City_Name" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Town_Name" HeaderText="鄉鎮" SortExpression="Town_Name" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="DATA_DATE" HeaderText="資料日期"  DataFormatString = "{0:yyyy/MM/dd}" SortExpression="DATA_DATE"  ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="CBD_KW" HeaderText="設置容量" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="UPLOAD_AMT" HeaderText="發電量" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="UPLOAD_RATE" HeaderText="發電小時" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="UPLOAD_PR" HeaderText="PR值" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CM_AVG_7" HeaderText="7日均值" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CM_TS_AVG_7" HeaderText="7日鄉鎮均值" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CM_AVG_MONTH" HeaderText="上月均值" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CM_TS_AVG_MONTH" HeaderText="上月鄉鎮均值" DataFormatString = "{0:F}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CM_AVG_YEAR" HeaderText="去年均值" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="CM_TS_AVG_YEAR" HeaderText="去年鄉鎮均值" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="StandardTxt" HeaderText="異常說明" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="StandardTxt2" HeaderText="異常說明二" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="REMARK_User" HeaderText="異動人員" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="REMARK_Time" HeaderText="異動時間" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="REMARK" HeaderText="異動說明" ItemStyle-HorizontalAlign="Left"/>
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
