<%@ Page Title="" Language="C#" MasterPageFile="~/_master/EditS.Master" AutoEventWireup="true" CodeBehind="CaseBaseDataQuery.aspx.cs" Inherits="WebApp._edit.CaseBaseDataQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function closethis() { parent.jQuery.fancybox.close(); }

        $(document).ready(function () {
            $("#form1").keydown(function (e) {
                if (e.keyCode == 13) {
                    return __doPostBack('<%= btnQuery.UniqueID %>', '');
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div>
                <table style="width:900px;margin:0 auto;">
                    <tr>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlCode1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCode1_SelectedIndexChanged"></asp:DropDownList>
                            &nbsp;案場名稱：<asp:TextBox ID="txtName" runat="server" KeyDown="txtName_OnKeyDown"></asp:TextBox><font color="red">【按ENTER即可篩選】</font>
                            <div style="display: none">
                            <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="清除" OnClick="btnClear_Click" />
                            </div>                        
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="4"></td>
                    </tr>--%>
                    <tr>
                        <%--<td style="vertical-align: top;">
                            <div id="tree" style="max-height: 450px; overflow: auto; padding-left: 10px;">
                            </div>
                        </td>--%>
                        <td style="vertical-align: top;width:45%;"">
                            <asp:ListBox ID="ListBox1" runat="server" Height="400px" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td style="text-align:center;width:10em;">
                            <asp:Button ID="btnAddAll" runat="server" Text="全部加入>>>" OnClick="btnAddAll_Click" />
                            <br />
                            <asp:Button ID="btnAdd" runat="server" Text="加入>" OnClick="btnAdd_Click" />
                            <br />
                            <asp:Button ID="btnRemove" runat="server" Text="<移除" OnClick="btnRemove_Click" />
                            <br />
                            <asp:Button ID="btnRemoveAll" runat="server" Text="<<<全部移除" OnClick="btnRemoveAll_Click" />
                        </td>
                        <td style="vertical-align: top;width:45%;"">
                            <asp:ListBox ID="ListBox2" runat="server" Height="400px" Width="100%" SelectionMode="Multiple" DataTextField="name" DataValueField="id"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button ID="btnJoinSelect" runat="server" Text="確定" CssClass="btn btn-warning"
                                OnClick="btnJoinSelect_Click" />
                            <input id="btnCancel" type="button" value="取消" onclick="closethis();" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
