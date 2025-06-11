<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Edit.Master" AutoEventWireup="true" CodeBehind="Module_ModelEdit.aspx.cs" Inherits="WebApp._edit.Module_ModelEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .validmsg{
            font-size:small;color:red;line-height:.5em;display:block;
        }
    </style>
    <script type="text/javascript">
        function checkData() {
            if (!Page_ClientValidate()) {
                return false;
            }
            else {
                if ($("#<%=hidKey.ClientID%>").val().length == 0) {
                    ReLogin(function (k) {
                        if (k.length > 0) {
                            $("#<%=hidKey.ClientID%>").val(k);
                            $("#<%=btnConfirm.ClientID%>").trigger("click");
                        }
                    });
                    return false;
                }
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ButtonArea" runat="server">
    <asp:HiddenField ID="hidID" runat="server" /><asp:HiddenField ID="hidKey" runat="server" Value="" />
    <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClientClick="return checkData()" ValidationGroup="btnConfirm" />
    <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabTitle" runat="server">
    <li class='tab'><a href="#tabs-1">模組型號資料</a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabContent" runat="server">
    <div id="tabs-1" style="padding-top: 3px;">
        <table style="width: 100%;">
            <tr>
                <th class="label must">模組型號名稱</th>
                <td colspan="3">
                    <asp:TextBox ID="txtModule_Model" runat="server" Width="95%" MaxLength="150"></asp:TextBox>
                    <br /><asp:RequiredFieldValidator ID="CD_NAMEValidator" runat="server" ControlToValidate="txtModule_Model" CssClass="validmsg"
                        ErrorMessage="[模組型號名稱]必填" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="label write" >片數</th>
                <td colspan="3">
                    <asp:TextBox ID="txtKw_Pis" runat="server" Width="95%" MaxLength="22"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <th class="label write">停用日期</th>
                <td>
                    <uc:ucDate ID="ucMM_StopDate" runat="server" />
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
