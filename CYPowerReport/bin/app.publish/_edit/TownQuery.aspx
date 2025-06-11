<%@ Page Title="" Language="C#" MasterPageFile="~/_master/EditS.Master" AutoEventWireup="true" CodeBehind="TownQuery.aspx.cs" Inherits="WebApp._edit.TownQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function closethis() { parent.jQuery.fancybox.close(); }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="EditPanel">
                <%--<table style="width: 630px;">--%>
                <table style="width:600px;margin:0 auto;">
                    <tr>
                        <%--<td></td>--%>
                        <td colspan="3">縣市：
                            <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <%--<td style="vertical-align: top;">
                            <div id="tree" style="max-height: 450px; overflow: auto; padding-left: 10px;">
                            </div>
                        </td>--%>
                        <td style="vertical-align: top;width:35%;">
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
                        <td style="vertical-align: top;width:35%;">
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
