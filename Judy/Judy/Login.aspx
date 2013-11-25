<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Judy.Login" %>

<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../Style/site.css" rel="stylesheet" />
    <script type="text/javascript">

        function checklogin() {
            var user = document.getElementById('txtPassword').value;

            if (user.length >= 5) {
                return true;
            }
            else {
                alert('PLEASE INPUT TRHE CORRECT PASSWORD');
                return false;
            }
        }


    </script>
</head>
<body>
    <form id="ContentForm" runat="server">
        <div id="container">
            <div id="header">
                JUDY LOGIN
            </div>
            <div id="middle">
                <div id="loginbox">
                    <table>
                        <tr>
                            <td>密码</td>
                            <td>
                                <asp:TextBox ID="txtPassword" MaxLength="20" TextMode="Password" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="text-align:right">记住我
                    <asp:CheckBox ID="chkRemeberMe" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="text-align:right">
                                <asp:Button ID="btnLogin" runat="server" CssClass="button" Text="登陆" OnClientClick="return checklogin();" OnClick="btnLogin_Click"
                                    Style="height: 21px" /></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="footer">
                &copy;2013 YAHCH.COM
            </div>
        </div>


    </form>
</body>
</html>
