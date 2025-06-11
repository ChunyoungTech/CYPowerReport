<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Edit.Master" AutoEventWireup="true" CodeBehind="CutomerDataEdit.aspx.cs" Inherits="WebApp._edit.CutomerDataEdit" %>
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

        function MM() {
            $("#<%= txtCD_TYPE.ClientID %>").val($("#<%= VCDType.ClientID %>").val());
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ButtonArea" runat="server">
    <asp:HiddenField ID="hidID" runat="server" /><asp:HiddenField ID="hidKey" runat="server" Value="" />
    <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClientClick="return checkData()" ValidationGroup="btnConfirm" />
    <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabTitle" runat="server">
    <li class='tab'><a href="#tabs-1">業主基本資料</a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabContent" runat="server">
    <div id="tabs-1" style="padding-top: 3px;">
        <table style="width: 100%;">
            <tr>
                <th class="label must">業主名稱</th>
                <td colspan="3">
                    <asp:TextBox ID="txtCD_NAME" runat="server" Width="95%" MaxLength="150"></asp:TextBox>
                    <br /><asp:RequiredFieldValidator ID="CD_NAMEValidator" runat="server" ControlToValidate="txtCD_NAME" CssClass="validmsg"
                        ErrorMessage="[業主名稱]必填" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="label write">地址</th>
                <td colspan="3">
                    <asp:TextBox ID="txtCD_ADDRESS" runat="server" Width="95%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">統一編號</th>
                <td>
                    <asp:TextBox ID="txtNumbers" runat="server" Width="90%" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">電話</th>
                <td>
                    <asp:TextBox ID="txtCD_Tel" runat="server" Width="90%" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">EMAIL</th>
                <td>
                    <asp:TextBox ID="txtCD_EMail" runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">聯絡</th>
                <td>
                    <asp:TextBox ID="txtCD_Contact" runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label must">業主清單</th>
                <td>
                    <asp:TextBox ID="txtCD_TYPE" runat="server" Width="40%" MaxLength="50"></asp:TextBox>
                    <asp:DropDownList ID="VCDType" runat="server" DataTextField="sysType" DataValueField="sysType" onchange="return MM()" Height="28" style="display:none"></asp:DropDownList>
                    <br /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCD_TYPE" CssClass="validmsg"
                        ErrorMessage="[業主清單]必填" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RequiredFieldValidator>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
