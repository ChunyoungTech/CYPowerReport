<%@ Page Title="" Language="C#" MasterPageFile="~/_master/EditS.Master" AutoEventWireup="true" CodeBehind="OperationHistoryQ.aspx.cs" Inherits="WebApp._edit.OperationHistoryQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .MainGridView table td {border-right-style:none;}
    </style>
    <script type="text/javascript">
        $(function () {
            $("#gridCheck").on("change", function () {
                $(".gridCheck").prop("checked", $(this).prop("checked"));
            });
        });

        var op = $.url().param("Open");
        var ID = $.url().param("pa");
        function closethis() {
            if (op == 'true') {
                close();
            } else {
                parent.jQuery.fancybox.close();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="EditPanel">
        <div class="buttonArea">
            <asp:Button ID="btnConfirm" runat="server" Text="確定" Visible="false" />
            <input id="btnCancel" type="button" value="關閉" onclick="closethis();" />
        </div>
        <div id="tabs">
            <ul class='etabs'>
                <li class='tab'><a href="#tabs-1">目前資料</a></li>
            </ul>
            <div id="tabs-1" style="padding-top: 3px;">
                <asp:GridView ID="GridView1" runat="server" CssClass="MainGridView" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="False" GridLines="Vertical"
                    Width="100%">
                    <PagerTemplate></PagerTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="cycTabs">
            <ul class='etabs'>
                <li class='tab'><a href="#tabs-2">操作歷史記錄</a></li>
            </ul>
            <div id="tabs-2" style="padding-top: 3px;">
                <asp:GridView ID="GridView2" runat="server" CssClass="MainGridView" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="False" GridLines="Vertical"
                    Width="100%">
                    <PagerTemplate></PagerTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
<%--    <div>
        <table style="width: 100%">
            <tr>
                <td style="vertical-align: top;">

                </td>
            </tr>
            <tr>
                <td>操作歷史記錄</td>
            </tr>
            <tr>
                <td style="vertical-align: top;">

                </td>
            </tr>

            <tr>
                <td>

                </td>
            </tr>
        </table>
    </div>--%>
</asp:Content>
