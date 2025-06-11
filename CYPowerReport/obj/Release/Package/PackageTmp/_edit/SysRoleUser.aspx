<%@ Page Title="" Language="C#" MasterPageFile="~/_master/EditS.Master" AutoEventWireup="true" CodeBehind="SysRoleUser.aspx.cs" Inherits="WebApp._edit.SysRoleUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkData() {
            $("#<%=hidSelect.ClientID%>").val($("#lstSelect option").map(function () { return this.value; }).get().join(","));
        }

        function AddUser() {
            var s = $("#lstDepEmp option:selected");
            var a = $("#lstSelect");
            if (a.length == 0) {
                s.each(function () {
                    a.append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
                });
            } else {
                s.each(function () {
                    if ($("#lstSelect option[value='" + $(this).val() + "']").length == 0) {
                        a.append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
                    }
                });
            }
        }

        function DelUser() {
            $("#lstSelect option:selected").each(function () {
                $(this).remove();
            });
        }

        function GetEmpData(d) {
            $.ajax({
                type: "GET",
                url: "../_Query/GetDepUser.ashx?Limit=false&Dep=" + d,
                success: function (data) {
                    var lst = $("#lstDepEmp");
                    lst.empty();
                    var myarray = $.parseJSON(data);
                    $.each(myarray, function (i, item) {
                        lst.append("<option value='" + item.ID + "'>" + item.Name + "</option>");
                    });
                }
            });
        }

        $(function () {
            $.ajax({
                type: "GET",
                url: "../_Query/GetDepUser.ashx?Limit=false",
                success: function (data) {
                    $("#tree").tree({
                        'autoOpen': 1,
                        'data': $.parseJSON(data)
                    });
                }
            });

            $('#tree').bind(
                'tree.click',
                function (event) {
                    var node = event.node;
                    var depNo = node.id;
                    GetEmpData(depNo);
                    //$('#tree').tree('toggle', node);
                }
            );

            $(document).on("change", "#txtCode,#txtName", function () {
                if ($(this).val().trim() == "") {
                    $("#txtCode").val("");
                    $("#txtName").val("");
                }
                $.get("../_Query/GetDepUser.ashx?Emp=" + $(this).val(), function (data) {
                    var user = JSON.parse(data);
                    if (user != null) {
                        $("#txtCode").val(user.ID);
                        $("#txtName").val(user.Name);
                    } else {
                        $("#txtCode").val("");
                        $("#txtName").val("");
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="EditPanel">
        <div class="buttonArea">
            <asp:Button ID="btnConfirm" runat="server" Text="儲存" OnClientClick="return checkData()" />
            <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
        </div>
        <div id="tabs">
            <ul class='etabs'>
                <li class='tab'><a href="#tabs-1">使用者設定</a></li>
            </ul>
            <div id="tabs-1" style="padding-top: 3px;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 40%;">部門</td>
                        <td style="width: 25%;">部門人員</td>
                        <td style="width: 10%;"></td>
                        <td style="width: 25%;">已選擇人員</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div id="tree" style="max-height: 450px; overflow: auto; padding-left: 10px;">
                            </div>
                        </td>
                        <td style="vertical-align: top;">
                            <select id="lstDepEmp" multiple="multiple" style="height: 450px; width: 100%;">
                            </select>
                        </td>
                        <td style="text-align: center;">
                            <input type="button" value="加入>>" onclick="AddUser();" /><br />
                            <br />
                            <input type="button" value="<<移除" onclick="DelUser();" />
                        </td>
                        <td style="vertical-align: top;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <select id="lstSelect" multiple="multiple" style="height: 450px; width: 100%;">
                                        <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
                                    </select>
                                    <asp:HiddenField ID="hidSelect" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirm" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
<%--                <table style="width: 630px;">
                    <tr>
                        <td style="width: 40%;">部門</td>
                        <td style="width: 25%;">部門人員</td>
                        <td style="width: 10%;"></td>
                        <td style="width: 25%;">已選擇人員</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <div id="tree" style="max-height: 450px; overflow: auto; padding-left: 10px;">
                            </div>
                        </td>
                        <td style="vertical-align: top;">
                            <asp:ListBox ID="ListBox1" runat="server" Height="450px" Width="150px" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td>
                            <input type="button" value="加入>>" onclick="AddUser();" /><br />
                            <br />
                            <input type="button" value="<<移除" onclick="DelUser();" />
                        </td>
                        <td style="vertical-align: top;">
                            <asp:ListBox ID="ListBox2" runat="server" Height="450px" Width="150px" SelectionMode="Multiple" DataTextField="name" DataValueField="id"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">人員：<input type="text" id="txtCode" style="width: 80px;" /><input type="text" id="txtName" style="width: 100px;" />
                            <input type="button" id="btnAdd" value="加入>>" />
                        </td>
                        <td></td>
                    </tr>
                </table>--%>
            </div>
        </div>
    </div>
</asp:Content>
