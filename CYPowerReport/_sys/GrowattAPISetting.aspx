<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Main.Master" AutoEventWireup="true" CodeBehind="GrowattAPISetting.aspx.cs" Inherits="WebApp._sys.GrowattAPISetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #growatt {
            padding-top: 2em;
        }
        #growatt th {
            height: 2em;
            width: 35%;
            background-color: #1C5E55;
            color: white;
            text-align: right;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".datepicker").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="growatt">
        <table style="margin:0 auto; width:40em;">
            <tr>
                <th>API網址</th>
                <td><asp:TextBox ID="txtUrl" runat="server" Width="95%" /></td>
            </tr>
            <tr>
                <th>帳號</th>
                <td><asp:TextBox ID="txtAccount" runat="server" Width="95%" /></td>
            </tr>
            <tr>
                <th>密碼</th>
                <td><asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="95%" /></td>
            </tr>
            <tr>
                <th>TOKEN</th>
                <td><asp:TextBox ID="txtToken" runat="server" Width="95%" /></td>
            </tr>
            <tr>
                <th>重新取得日期</th>
                <td><asp:TextBox ID="txtDate" CssClass="datepicker" runat="server" Width="95%" /></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="儲存" OnClick="btnSave_Click" />
                    <asp:Button ID="btnReload" runat="server" Text="重新取得資料" OnClick="btnReload_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
