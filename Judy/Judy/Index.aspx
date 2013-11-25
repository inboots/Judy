<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Judy.Index" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../Style/site.css" rel="stylesheet" />
</head>
<body>
    <form id="ContentForm" runat="server">

        <div id="container">
            <div id="header">
                JUDY
           <div id="bar"><a href="Index.aspx">HOME</a>&nbsp;&nbsp;<a href="Post.aspx">WRITE</a></div>
            </div>
            <div id="middle">
                <div id="content">

                    <asp:Repeater ID="Twitters" runat="server">
                        <ItemTemplate>
                            <div class="single">
                                 <div class="taotao"><%#Eval("jtext").ToString() %></div>
                                 <div class="datetime"><%#Eval("jdatetime").ToString() %></div>
                                <div class="geo"><%#Eval("jaddress").ToString() %></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                  
                </div>
                <div id="navigate">
                    <asp:Button ID="btnPrev" CssClass="button" runat="server" Text="PREVIOUS" OnClick="btnPrev_Click" />
                    <asp:Button ID="btnNext" runat="server" CssClass="button" Text="NEXT" OnClick="btnNext_Click" />
                </div>
            </div>
            <div id="footer">
                &copy;2013 YAHCH
            </div>
        </div>
    </form>
</body>
</html>
