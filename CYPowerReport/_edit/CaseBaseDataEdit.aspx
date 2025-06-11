<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Edit.Master" AutoEventWireup="true" CodeBehind="CaseBaseDataEdit.aspx.cs" Inherits="WebApp._edit.CaseBaseDataEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .validmsg {
            font-size: small;
            color: red;
            line-height: .5em;
            display: block;
        }

        .labelTextBox {
            border: none;
            background-color: transparent;
            color: #000;
            width: 99%;
        }

            .labelTextBox:disabled {
                color: #000;
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

        //計算片數
        function MM() {
            var no = $("#<%= ddlCBD_Module_Model.ClientID %>").val();
            var kp = 0;

            for (var i = 0; i < mm_date.length; i++) {
                if (mm_date[i][0] == no) { kp = mm_date[i][2]; }
            }

            if (no == 0 || kp == 0) {
                $("#<%= txtCBD_Slices.ClientID %>").val(0);
            } else {
                $("#<%= txtCBD_Slices.ClientID %>").val(Math.round($("#<%= txtCBD_KW.ClientID %>").val() / kp));
            }
        }

        function DD() {
            $("#<%= txtCBD_Equipment_Brand.ClientID %>").val($("#<%= dllCBD_Equipment_Brand.ClientID %>").val());
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ButtonArea" runat="server">
    <%--<asp:HiddenField ID="hidID" runat="server" />--%>
    <asp:HiddenField ID="hidKey" runat="server" Value="" />
    <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClientClick="return checkData()" ValidationGroup="btnConfirm" />
    <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabTitle" runat="server">
    <li class='tab'><a href="#tabs-1">案場基本資料</a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabContent" runat="server">
    <div id="tabs-1" style="padding-top: 3px;">
        <table style="width: 100%;">
            <tr>
                <th class="label must">案場名稱</th>
                <td colspan="3">
                    <asp:TextBox ID="txtCBD_Case_Name" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="CBD_Case_NameValidator" runat="server" ControlToValidate="txtCBD_Case_Name" CssClass="validmsg"
                        ErrorMessage="[案場名稱]必填" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="label must">公司類別</th>
                <td colspan="3">
                    <asp:DropDownList ID="CompanyList" runat="server" Width="95%" DataTextField="COD_NAME" DataValueField="COD_SEQ_ID" Font-Size="1.1em"></asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CompanyList" CssClass="validmsg"
                        ErrorMessage="[案場公司]必填" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>

                <th class="label must">案場業主</th>
                <td>
                    <asp:DropDownList ID="ddlCBD_Case_Owner" runat="server" Width="95%" DataTextField="CD_NAME" DataValueField="CD_SEQ_ID" Font-Size="1.1em"></asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="ddlCBD_Case_OwnerValidator" runat="server" ControlToValidate="ddlCBD_Case_Owner" CssClass="validmsg"
                        ErrorMessage="[案場業主]必填" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RequiredFieldValidator>
                </td>
                <th class="label write">監控廠商</th>
                <td>
                    <asp:TextBox ID="txtCBD_Equipment_Brand" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                    <asp:DropDownList ID="dllCBD_Equipment_Brand" runat="server" DataTextField="CBD_Equipment_Brand" DataValueField="CBD_Equipment_Brand" Font-Size="1.1em" onchange="return DD()" Style="display: none"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="label write">縣市</th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlCBD_County_ID" runat="server" Width="20%" DataTextField="C_City_Name" DataValueField="C_City_ID" Font-Size="1.1em" AutoPostBack="True" OnSelectedIndexChanged="CBD_County_ID_SelectedIndexChanged"></asp:DropDownList>
                    鄉鎮<asp:DropDownList ID="ddlCBD_TownShip" runat="server" Width="20%" DataTextField="C_City_Name" DataValueField="C_City_ID" Font-Size="1.1em"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="label write">地址</th>
                <td colspan="3">
                    <asp:TextBox ID="txtCBD_Address" runat="server" Width="98%" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">定位點</th>
                <td>
                    <asp:TextBox ID="txtCBD_GPS" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                </td>
                <th class="label write">KW數</th>
                <td>
                    <asp:TextBox ID="txtCBD_KW" runat="server" Width="95%" MaxLength="18" onchange="return MM()"></asp:TextBox>
                    <br />
                    <asp:RegularExpressionValidator ID="CBD_KWFieldValidator1" runat="server" ControlToValidate="txtCBD_KW" CssClass="validmsg"
                        ErrorMessage="[KW數]必須為大於0的正數(最多小數3位)" ValidationExpression="\d+\.?\d{0,3}" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th class="label write">模組型號</th>
                <td>
                    <asp:DropDownList ID="ddlCBD_Module_Model" runat="server" Width="95%" DataTextField="MM_Module_Model" DataValueField="MM_SEQ_ID" onchange="return MM()" Font-Size="1.1em"></asp:DropDownList>
                </td>
                <th class="label write">片數</th>
                <td>
                    <asp:TextBox ID="txtCBD_Slices" runat="server" Width="95%" MaxLength="20"></asp:TextBox>
                    <br />
                    <asp:RegularExpressionValidator ID="CBD_SlicesExpressionValidator1" runat="server" ControlToValidate="txtCBD_Slices" CssClass="validmsg"
                        ErrorMessage="[片數]必須為大於0的正整數" ValidationExpression="^[0-9]*$" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th class="label write">方位</th>
                <td>
                    <asp:DropDownList ID="ddlCBD_Bearing" runat="server" Width="95%" Font-Size="1.1em">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="南北" Value="南北"></asp:ListItem>
                        <asp:ListItem Text="東西" Value="東西"></asp:ListItem>
                        <asp:ListItem Text="西北東南" Value="西北東南"></asp:ListItem>
                        <asp:ListItem Text="東北西南" Value="東北西南"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th class="label write">案場類型</th>
                <td>
                    <asp:DropDownList ID="ddlCBD_Case_Type" runat="server" Width="95%" Font-Size="1.1em">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="地面型" Value="地面型"></asp:ListItem>
                        <asp:ListItem Text="屋頂型" Value="屋頂型"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="label write">電壓型態</th>
                <td>
                    <asp:DropDownList ID="ddlCBD_Voltage_Type" runat="server" Width="95%" Font-Size="1.1em">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="高壓" Value="高壓"></asp:ListItem>
                        <asp:ListItem Text="低壓" Value="低壓"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th class="label write">設置容量</th>
                <td>
                    <asp:TextBox ID="txtCBD_Set_Amount" runat="server" Width="95%" MaxLength="18"></asp:TextBox>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCBD_Set_Amount" CssClass="validmsg"
                        ErrorMessage="[設置容量]必須為大於0的正數(最多小數3位)" ValidationExpression="\d+\.?\d{0,3}" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th class="label write">合約保發小時</th>
                <td>
                    <asp:TextBox ID="txtCBD_Deal" runat="server" Width="95%" MaxLength="18"></asp:TextBox>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCBD_Deal" CssClass="validmsg"
                        ErrorMessage="[合約保發小時]必須為大於0的正數(最多小數3位)" ValidationExpression="\d+\.?\d{0,3}" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RegularExpressionValidator>
                </td>
                <th class="label write">電費</th>
                <td>
                    <asp:TextBox ID="txtCBD_Electricity_Fee" runat="server" Width="95%" MaxLength="18"></asp:TextBox>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCBD_Electricity_Fee" CssClass="validmsg"
                        ErrorMessage="[電費]必須為大於0的正數(最多小數3位)" ValidationExpression="\d+\.?\d{0,3}" Display="Dynamic" ValidationGroup="btnConfirm"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th class="label write">有無日照計</th>
                <td>
                    <asp:DropDownList ID="ddlCBD_SunlightMeter" runat="server" Font-Size="1.1em" Width="95%">
                        <asp:ListItem Text="此案場[無]日照計" Value="0"></asp:ListItem>
                        <asp:ListItem Text="此案場[有]日照計" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th class="label write">案場對應編號</th>
                <td>
                    <asp:TextBox ID="txtCBD_Case_Code" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <%--總額換算比例」、「稅率」、「匯費」、「電錶租金--%>
            <tr>
                <th class="label write">總額換算比例</th>
                <td>
                    <asp:TextBox ID="txtTotalChangeRate" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                </td>
                <th class="label write">稅率</th>
                <td>
                    <asp:TextBox ID="txtTaxRate" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">匯費</th>
                <td>
                    <asp:TextBox ID="txtRemittanceFee" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                </td>
                <th class="label write">電錶租金</th>
                <td>
                    <asp:TextBox ID="txtMeterRent" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <th class="label write">備註</th>
                <td colspan="3">
                    <asp:TextBox ID="txtCBD_Remarks" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">停用日期</th>
                <td colspan="3">
                    <uc:ucDate ID="ucCBD_Stop_Date" runat="server" />
                </td>
            </tr>
            <tr>
                <th class="label write">電號</th>
                <td>
                    <asp:TextBox ID="txtElectricNumber" runat="server" Width="98%" MaxLength="50"></asp:TextBox>

                </td>
                <th class="label write">掛錶日期</th>
                <td>
                    <uc:ucDate ID="ucMeterUpDate" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
