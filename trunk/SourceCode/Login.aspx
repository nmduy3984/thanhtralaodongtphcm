<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<%@ Register Src="Control/Messagebox/msbox.ascx" TagName="msbox" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login Form</title>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
    <link href="Styles/thickbox.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="Styles/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" media="screen" />
    <script src='<%=ResolveUrl("~/Scripts/jquery-1.4.1.min.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery-ui-1.8.custom.min.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery.ui.datepicker-vi.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/thickbox.js")%>' type="text/javascript"></script>
    <%--<script src='<%=ResolveUrl("~/Scripts/thickbox-compressed.js")%>' type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(function () {
            $("#<%=txtUserName.ClientID() %>").focus();


            $(".form_login input").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnLogin.ClientID()%>").click();
                    return false;
                }
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pagecontent">
        <div class="content">
            <div class="content_left">
                <div class="left_title">
                    <%--<h2>
                        Hệ thống quản lý dữ liệu</h2>--%>
                    <h1>
                        Thanh tra lao động</h1>
                </div>
                <div class="version">
                    Version 1.0</div>
            </div>
            <div class="content_right">
                <div class="right_title">
                    <h1>
                        Đăng nhập</h1>
                </div>
                <div class="rows">
                    <div class="labelLogin">
                        Tên đăng nhập</div>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="txt_user"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                        ErrorMessage="Nhập tên đăng nhập" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                </div>
                <div class="rows">
                    <div class="labelLogin">
                        Mật khẩu</div>
                    <div class="rows_pass">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="txt_pass"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="Nhập mật khẩu" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                        <asp:Button ID="btnLogin" runat="server" CssClass="btnLogin" />
                    </div>
                </div>
                <div class="rows" style="margin-top: 6px">
                    <label class="label_check" for="checkbox-02">
                        <asp:CheckBox ID="chkRememberMe" runat="server" />
                        Nhớ tên đăng nhập.</label>
                    &nbsp;&nbsp;<a href="Register.aspx" class="forgetPassword">Đăng ký tài khoản</a>
                </div>
            </div>
        </div>
    </div>
    <script language="javascript">
        var d = document;
        var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
        var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
        onload = function () {

            var body = gebtn(d, 'body')[0];
            body.className = body.className && body.className != '' ? body.className + ' has-js' : 'has-js';

            if (!d.getElementById || !d.createTextNode) return;
            var ls = gebtn(d, 'label');
            for (var i = 0; i < ls.length; i++) {
                var l = ls[i];
                if (l.className.indexOf('label_') == -1) continue;
                var inp = gebtn(l, 'input')[0];
                if (l.className == 'label_check') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_check c_on' : 'label_check c_off';
                    l.onclick = check_it;
                };
                if (l.className == 'label_radio') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_radio r_on' : 'label_radio r_off';
                    l.onclick = turn_radio;
                };
            };
        };
        var check_it = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_check c_off' || (!safari && inp.checked)) {
                this.className = 'label_check c_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_check c_off';
                if (safari) inp.click();
            };
        };
        var turn_radio = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_radio r_off' || inp.checked) {
                var ls = gebtn(this.parentNode, 'label');
                for (var i = 0; i < ls.length; i++) {
                    var l = ls[i];
                    if (l.className.indexOf('label_radio') == -1) continue;
                    l.className = 'label_radio r_off';
                };
                this.className = 'label_radio r_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_radio r_off';
                if (safari) inp.click();
            };
        };
    </script>
    <asp:Label ID="lblLoginFail" runat="server" Text="Tên đăng nhập hoặc mật khẩu chưa chính xác. <br> Hãy nhập lại."
        Visible="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    <uc1:msbox ID="msbox1" runat="server" />
    </form>
</body>
</html>
