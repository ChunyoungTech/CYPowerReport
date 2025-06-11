<%@ Page Title="" Language="C#" MasterPageFile="~/_master/EditS.Master" AutoEventWireup="true" CodeBehind="RegionQuery.aspx.cs" Inherits="WebApp._edit.RegionQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function closethis() { parent.jQuery.fancybox.close(); }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="EditPanel">
        <table style="width: 630px;">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlCode1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCode1_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <div id="tree" style="max-height: 450px; overflow: auto; padding-left: 10px;">
                    </div>
                </td>
                <td style="vertical-align: top;">
                    <asp:ListBox ID="ListBox1" runat="server" Height="400px" Width="150px" SelectionMode="Multiple"></asp:ListBox>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="加入>>>" onclick="btnAdd_Click" />
                    <br />
                    <asp:Button ID="btnRemove" runat="server" Text="<<<移除" onclick="btnRemove_Click" />
                </td>
                <td style="vertical-align: top;">
                    <asp:ListBox ID="ListBox2" runat="server" Height="400px" Width="150px" SelectionMode="Multiple" DataTextField="name" DataValueField="id"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnJoinSelect" runat="server" Text="確定" CssClass="btn btn-warning"
                        OnClick="btnJoinSelect_Click" />                    
                    <input id="btnCancel" type="button" value="取消" onclick="closethis();" />
                </td>
            </tr>
        </table> 
    </div> 
</asp:Content>
