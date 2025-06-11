<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApp.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>智慧電量報表系統 </title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <style>
        .main{
            padding: 1em; display: flex; flex-direction: column;flex-wrap:wrap;border-radius:1em;box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }
        @media (min-width: 768px) {
            .main {
                width:25em;
            }
        }
    </style>
<%--    <script>
        $(function () {
            window.onresize = setMargin;
            setMargin();
        });
        var setMargin = function () {
            var wh = window.innerHeight, xh = $(".container").height();
            $(".container").css("margin-top", (wh - 10 > xh) ? ((wh - xh) / 2) : 5);
        };

    </script>--%>
</head>
<body>

<div style="height: 95vh; display: flex; align-items: center; justify-content: center;">

            <div class="main">

                <div class="form-group">
                    <h2 class="form-signin-heading">
                        <img src="_img/logo.jpg" width="200" height="150" alt="群創光電" />
                    </h2>
                    <h2 class="form-signin-heading">宇軒綠能</h2>
                    <h2 class="form-signin-heading">智慧電量報表系統
                    </h2>
                </div>

                <form id="form1" runat="server">
                    <div class="form-group">
                        
                        <label for="usr">UserID：</label>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="pwd">Password：</label>
                        <asp:TextBox ID="TextBox2" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group float-right">
                        <asp:Button ID="Button1" runat="server" Text="登入" CssClass="btn btn-primary" OnClick="Button1_Click" />
                    </div>
                    <div class="panel-warning text-danger text-center;">
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="form-group">
                    <a href="Doc/驗收資料_宇軒電量智慧報表系統_操作手冊.pdf">宇軒電量智慧報表系統_操作手冊</a>
                    </div>

                </form>

            </div>

        </div>

<%--    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-xs-1 col-sm-2 col-md-3 col-lg-4"></div>
                <div class="col-xs-10 col-sm-8 col-md-6 col-lg-4" style="border: 1px solid black;">
                    <div class="form-group">
                        <h2 class="form-signin-heading">
                            <img src="_img/logo.jpg" width="200" height="150" alt="群創光電" />
                        </h2>
                        <h2 class="form-signin-heading">群創光電</h2>
                        <h2 class="form-signin-heading">廠務智慧雲端系統
                        </h2>
                    </div>
                    <div class="form-group">
                        <label for="usr">UserID：</label>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="pwd">Password：</label>
                        <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="Button1" runat="server" Text="登入" CssClass="btn btn-primary" OnClick="Button1_Click" />
                    </div>
                    <div class="panel-warning text-center text-danger">
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-xs-1 col-sm-2 col-md-3 col-lg-4"></div>
            </div>
        </div>
    </form>--%>
</body>
</html>
