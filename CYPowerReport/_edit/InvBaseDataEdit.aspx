<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Edit.Master" AutoEventWireup="true" CodeBehind="InvBaseDataEdit.aspx.cs" Inherits="WebApp._edit.InvBaseDataEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .validmsg{
            font-size:small;color:red;line-height:.5em;display:block;
        }
    </style>
    <script type="text/javascript">
        //驗證
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
    <li class='tab'><a href="#tabs-1">逆變器基本資料</a></li>
    <li class='tab' style="display:none;"><a href="#tabs-2">上傳資料</a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabContent" runat="server">
    <div id="tabs-1" style="padding-top: 3px;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="MainGridView"
            GridLines="Vertical" AllowSorting="false" AllowPaging="false" ShowHeaderWhenEmpty="true" PagerSettings-Visible="false">
            <Columns>
                <asp:TemplateField HeaderText="項次" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2.5em">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="逆變器名稱">
                    <ItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" Text='<%#Eval("IBD_INV_NAME")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KW">
                    <ItemTemplate>
                        <asp:TextBox ID="txtKW" runat="server" Text='<%#Eval("IBD_INV_KW")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KWP">
                    <ItemTemplate>
                        <asp:TextBox ID="txtKWP" runat="server" Text='<%#Eval("IBD_INV_KWP")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" Visible="false">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSEQ" runat="server" Text='<%#Eval("IBD_INV_ID")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="White" />
            <EmptyDataTemplate><div class="NoData">查無符合條件資料</div></EmptyDataTemplate>
        </asp:GridView>

        <%--<asp:Table ID="INV_Table" runat="server" BorderStyle="Solid">
            <asp:TableRow HorizontalAlign="Center"><asp:TableCell>項次</asp:TableCell><asp:TableCell>逆變器名稱</asp:TableCell><asp:TableCell>KW</asp:TableCell><asp:TableCell>KWP</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>1</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_1" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_1" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_1" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>2</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_2" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_2" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_2" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>3</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_3" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_3" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_3" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>4</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_4" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_4" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_4" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>5</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_5" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_5" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_5" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>6</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_6" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_6" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_6" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>7</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_7" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_7" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_7" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>8</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_8" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_8" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_8" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>9</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_9" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_9" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_9" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>10</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_10" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_10" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_10" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>11</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_11" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_11" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_11" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>12</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_12" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_12" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_12" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>13</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_13" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_13" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_13" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>14</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_14" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_14" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_14" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>15</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_15" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_15" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_15" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>16</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_16" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_16" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_16" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>17</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_17" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_17" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_17" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>18</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_18" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_18" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_18" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>19</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_19" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_19" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_19" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>20</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_20" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_20" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_20" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>21</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_21" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_21" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_21" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>22</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_22" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_22" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_22" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>23</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_23" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_23" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_23" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>24</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_24" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_24" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_24" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>25</asp:TableCell><asp:TableCell><asp:TextBox ID="txt_NAME_25" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KW_25" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txt_KWP_25" Text="" runat="server"/></asp:TableCell></asp:TableRow>
        </asp:Table>--%>
    </div>
    <div id="tabs-2" style="padding-top: 3px;">
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="MainGridView"
            GridLines="Vertical" AllowSorting="false" AllowPaging="false" ShowHeaderWhenEmpty="true" PagerSettings-Visible="false">
            <Columns>
                <asp:TemplateField HeaderText="項次" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2.5em">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="逆變器名稱">
                    <ItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" Text='<%#Eval("IBD_INV_NAME")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="White" />
            <EmptyDataTemplate><div class="NoData">查無符合條件資料</div></EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
