<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Post.aspx.cs" Inherits="Judy.Post" %>
<!DOCTYPE html>
<html>
<head>
    <title>Write</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../Style/site.css" rel="stylesheet" />
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=95d08ea349db34150ab76aff1d24de5f"></script>
    <script type="text/javascript">

        function geo() {
            var geolocation = new BMap.Geolocation();
            geolocation.getCurrentPosition(function (r) {
                if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                    //alert('您的位置：' + r.point.lng + ',' + r.point.lat);
                    document.getElementById('hidLng').value = r.point.lng;
                    document.getElementById('hidLat').value = r.point.lat;
                    var gc = new BMap.Geocoder();
                    gc.getLocation(r.point, function (rs) {
                        var addComp = rs.addressComponents;
                        var addr = addComp.province + addComp.city + addComp.district + addComp.street + addComp.streetNumber;
                        document.getElementById('hidAddress').value = addr;
                        document.getElementById('location').innerHTML = addr;
                    });
                }
                else {
                    alert('未能获取你的位置，错误代码:' + this.getStatus());
                }
            }, { enableHighAccuracy: true })
            //关于状态码
            //BMAP_STATUS_SUCCESS	检索成功。对应数值“0”。
            //BMAP_STATUS_CITY_LIST	城市列表。对应数值“1”。
            //BMAP_STATUS_UNKNOWN_LOCATION	位置结果未知。对应数值“2”。
            //BMAP_STATUS_UNKNOWN_ROUTE	导航结果未知。对应数值“3”。
            //BMAP_STATUS_INVALID_KEY	非法密钥。对应数值“4”。
            //BMAP_STATUS_INVALID_REQUEST	非法请求。对应数值“5”。
            //BMAP_STATUS_PERMISSION_DENIED	没有权限。对应数值“6”。(自 1.1 新增)
            //BMAP_STATUS_SERVICE_UNAVAILABLE	服务不可用。对应数值“7”。(自 1.1 新增)
            //BMAP_STATUS_TIMEOUT	超时。对应数值“8”。(自 1.1 新增)
        }

    </script>
</head>
<body>
    <form id="ContentForm" runat="server">

        <div id="container">
            <div id="header">
                JUDY
             <div id="bar"><a href="Index.aspx">HOME</a></div>
            </div>
            <div id="middle">
                <div style="padding:0.1em; margin:0.1em;">
                    <asp:TextBox ID="txtTwitter" TextMode="MultiLine" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <input type="button" class="button" onclick="javascript: geo();" value="LOCATION" />
                    <asp:Button ID="btnAdd" CssClass="button" runat="server" Text="SUBMIT" OnClick="btnAdd_Click" />
                    
                    <br/>
                    <br />
                    <p id="location">&nbsp;</p>

                </div>
            </div>
            <div id="footer">
                &copy;2013 YAHCH.COM
            </div>
        </div>
        <asp:HiddenField ID="hidLng" runat="server" />
        <asp:HiddenField ID="hidLat" runat="server" />
        <asp:HiddenField ID="hidAddress" runat="server" />
        <asp:HiddenField ID="hidDevc" runat="server" />

    </form>
</body>
</html>
