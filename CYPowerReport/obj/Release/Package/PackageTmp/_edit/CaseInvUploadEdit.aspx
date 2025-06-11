<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Edit.Master" AutoEventWireup="true" CodeBehind="CaseInvUploadEdit.aspx.cs" Inherits="WebApp._edit.CaseInvUploadEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .validmsg{
            font-size:small;color:red;line-height:.5em;display:block;
        }
    </style>
    <script type="text/javascript">
        //驗證
        function checkData() {
            //alert("test");
            if (!Page_ClientValidate()) {
                //alert("1");
                return false;
            }
            else {
                //alert("2");
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
    <asp:HiddenField ID="hidKey" runat="server" Value="" />
<%--    <asp:HiddenField ID="hidID" runat="server" />
    <asp:HiddenField ID="hidDate" runat="server" /><asp:HiddenField ID="HiddenField2" runat="server" Value="" />--%>
    <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClientClick="return checkData()" ValidationGroup="btnConfirm" />
    <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabTitle" runat="server">
    <li class='tab'><a href="#tabs-1">案場每日發電量</a></li>
    <li class='tab'><a href="#tabs-2">逆變器每日發電量</a></li>
    <li class='tab'><a href="#tabs-3">處置說明</a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabContent" runat="server">
    <div id="tabs-1" style="padding-top: 3px;">
        <asp:Table ID="Case_Table" style="width: 80%;" runat="server" BorderStyle="Solid">
            <asp:TableRow HorizontalAlign="Center"><asp:TableCell>發電量</asp:TableCell><asp:TableCell>RATE</asp:TableCell><asp:TableCell>PR</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell><asp:TextBox ID="txtAMT" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR" Text="" runat="server"/></asp:TableCell></asp:TableRow>
        </asp:Table>
    </div>
    <div id="tabs-2" style="padding-top: 3px;">

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
                        <asp:TextBox ID="txt_INV_NAME" runat="server" Text='<%#Eval("INV_NAME")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="發電量">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_INV_AMT" runat="server" Text='<%#Eval("INV_AMT")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RATE">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_INV_RATE" runat="server" Text='<%#Eval("INV_RATE")%>'></asp:TextBox>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PR">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_INV_PR" runat="server" Text='<%#Eval("INV_PR")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="White" />
            <EmptyDataTemplate><div class="NoData">查無符合條件資料</div></EmptyDataTemplate>
        </asp:GridView>

        <asp:Table ID="INV_Table" style="width: 80%;" runat="server" BorderStyle="Solid" Visible="false">
            <asp:TableRow HorizontalAlign="Center"><asp:TableCell>項次</asp:TableCell><asp:TableCell>逆變器名稱</asp:TableCell><asp:TableCell>發電量</asp:TableCell><asp:TableCell>RATE</asp:TableCell><asp:TableCell>PR</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>1</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_1" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_1" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_1" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_1" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>2</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_2" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_2" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_2" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_2" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>3</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_3" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_3" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_3" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_3" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>4</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_4" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_4" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_4" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_4" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>5</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_5" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_5" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_5" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_5" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>6</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_6" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_6" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_6" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_6" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>7</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_7" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_7" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_7" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_7" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>8</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_8" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_8" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_8" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_8" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>9</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_9" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_9" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_9" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_9" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>10</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_10" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_10" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_10" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_10" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>11</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_11" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_11" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_11" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_11" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>12</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_12" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_12" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_12" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_12" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>13</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_13" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_13" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_13" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_13" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>14</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_14" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_14" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_14" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_14" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>15</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_15" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_15" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_15" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_15" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>16</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_16" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_16" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_16" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_16" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>17</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_17" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_17" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_17" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_17" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>18</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_18" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_18" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_18" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_18" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>19</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_19" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_19" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_19" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_19" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>20</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_20" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_20" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_20" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_20" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>21</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_21" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_21" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_21" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_21" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>22</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_22" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_22" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_22" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_22" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>23</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_23" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_23" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_23" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_23" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>24</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_24" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_24" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_24" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_24" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>25</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNAME_25" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtAMT_25" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtRATE_25" Text="" runat="server"/></asp:TableCell><asp:TableCell><asp:TextBox ID="txtPR_25" Text="" runat="server"/></asp:TableCell></asp:TableRow>
        </asp:Table>
    </div>
    <div id="tabs-3" style="padding-top: 3px;">
        <asp:Table ID="Remark_Table" style="width: 80%;" runat="server" BorderStyle="Solid">
            <asp:TableRow><asp:TableCell style="width: 10%;">異動說明</asp:TableCell><asp:TableCell><asp:TextBox ID="txtREMARK" Width="100%" Text="" runat="server"/></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>異動人員</asp:TableCell><asp:TableCell ID="REMARK_User"></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell>異動時間</asp:TableCell><asp:TableCell ID="REMARK_Time"></asp:TableCell></asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
