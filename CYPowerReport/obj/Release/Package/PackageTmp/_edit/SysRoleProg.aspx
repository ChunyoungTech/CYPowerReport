<%@ Page Title="" Language="C#" MasterPageFile="~/_master/EditS.Master" AutoEventWireup="true" CodeBehind="SysRoleProg.aspx.cs" Inherits="WebApp._edit.SysRoleProg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EditPanel">
                <div class="buttonArea">
                    <asp:Button ID="btnConfirm" runat="server" Text="確定" />
                    <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
                </div>
                <div id="tabs">
                    <ul class='etabs'>
                        <li class='tab'><a href="#tabs-1">功能設定</a></li>
                    </ul>
                    <div id="tabs-1" style="padding-top: 3px;">
                        <asp:GridView ID="GridView1" runat="server" CssClass="MainGridView" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="False" GridLines="Vertical"
                            Width="100%" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="DirName" HeaderText="分類" />
                                <asp:BoundField DataField="Name" HeaderText="功能名稱" />
                                <asp:TemplateField HeaderText="全部" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkID" runat="server" />
                                        <asp:HiddenField ID="hidMainID" runat="server" Value='<%# Eval("id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="細項">
                                    <ItemTemplate>
                                        <div style="margin-left: 1em;">
                                            <asp:CheckBoxList ID="chkSubList" runat="server" DataTextField="Name" DataValueField="ID"
                                                RepeatColumns="3" RepeatDirection="Vertical" CellPadding="3" CellSpacing="3">
                                            </asp:CheckBoxList>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate></PagerTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
