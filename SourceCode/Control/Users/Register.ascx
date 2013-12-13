<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Register.ascx.vb" Inherits="Control_Users_Register" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc3" %>
<%@ Register Assembly="WebControlCaptcha" Namespace="WebControlCaptcha" TagPrefix="cc1" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            //Popup
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }

            $("#<%=txtUserNameReg.ClientID() %>").focus();


            //Kiem tra trung
            $("#<%= txtUserNameReg.ClientID %>").live("focusout", function () {
                var sName = $(this).val();
                $.ajax({
                    type: "POST", //Phuong thuc truyen du lieu luon la POST
                    url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/CheckName")%>',
                    data: "{'strName': '" + sName + "'}", //thong so truyen vao
                    dataType: "", //kieu du lieu tra ve
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        var resultReturn = eval("(" + result.d + ")");
                        if (resultReturn == "0") {
                            AlertboxThenFocus("Người dùng đã tồn tại hãy chọn tên khác.", $("#<%= txtUserNameReg.ClientID %>"));
                            return false;
                        }

                    }, //ham thuc thi neu thanh cong    
                    error: function (XMLHttpRequest, textStatus, errorThrown) {

                    } //ham thuc thi neu loi xay ra.

                });
            });

            //btnRegister

            $("#<%= btnRegister.ClientID %>").click(function () {

                if (validateFormInputs('reg') == true) {
                    return true;
                } else {
                    return false;
                }
            });
            //Popup
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }

            $('.Date').datepicker({ dateFormat: 'dd/mm/yy' });
            // Dùng cho 2 checkbox có, không
            CheckOnlyAndEffect();
        });
    }


    // Hàm dùng để Reset lại giá trị ban đầu cho  form
    function resetForm() {
        $("div[id*=edit] input[type=text]").val("");
        $("div[id*=edit] textarea").val("");
        $("div[id*=edit] select").attr('selectedIndex', 0);
        return true;
    }
     
</script>
<asp:UpdatePanel ID="UpdatePanelRegister" runat="server">
    <ContentTemplate>
        <div class="form_login">
            <div class="form_border">
                <div class="form_main">
                    <div class="col_left">
                        <div class="label_form">
                            <h1>
                                đăng nhập</h1>
                            <h3 class="label_row">
                                Tên đăng nhập<span class="fieldRequired">&nbsp;</span></h3>
                            <cc2:ValidationTextBox ID="txtUserNameLogin" AssociatedLableText="Tên đăng nhập"
                                DataType="required" runat="server" CssClass="TextBox" Group="login" />
                            <h3 class="label_row">
                                Mật khẩu<span class="fieldRequired">&nbsp;</span></h3>
                            <cc2:ValidationTextBox ID="txtPassowrd" runat="server" AssociatedLableText="Mật khẩu"
                                DataType="required" TextMode="Password" CssClass="TextBox" Group="login" />
                            <h3 class="label_row">
                                <asp:CheckBox ID="chkRememberMe" runat="server" />&nbsp; &nbsp; Nhớ tên đăng nhập
                                ?
                            </h3>
                            <asp:Button runat="server" ID="btnLogin" CausesValidation="true" CommandArgument=""
                                OnClientClick="return validateFormInputs('login');" CssClass="btn" Text="Đăng nhập" />
                            <%--<asp:HyperLink ID="hplForgotpassword" CssClass="thickbox"  runat="server">Quên mật khẩu</asp:HyperLink>--%>
                        </div>
                    </div>
                    <div class="col_right">
                        <div class="label_form">
                            <h1>
                                đăng ký</h1>
                            <h2>
                                Thông tin tài khoản</h2>
                            <div class="div_left">
                                <div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Tên truy cập <span class="fieldRequired">&nbsp;</span>
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtUserNameReg" runat="server" CssClass="TextBox" DataType="required"
                                            Group="reg" AssociatedLableText="tên truy cập" />
                                    </div>
                                </div>
                                <div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Mật khẩu <span class="fieldRequired">&nbsp;</span>
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtPass" runat="server" CssClass="TextBox" DataType="required"
                                            TextMode="Password" Group="reg" AssociatedLableText="mật khẩu truy cập" />
                                    </div>
                                </div>
                                <div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Xác nhận mật khẩu <span class="fieldRequired">&nbsp;</span>
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtRePass" runat="server" CssClass="TextBox" DataType="required"
                                            TextMode="Password" Compage="True" AssociatedLableText="mật khẩu xác nhận" CompareTo="ContentPlaceHolder1_Register1_txtPass"
                                            CompareOperator="Equal" Group="reg" />
                                    </div>
                                </div>
                            </div>
                            <div class="div_right">
                                &nbsp;
                            </div>
                            <h2>
                                Thông tin cá nhân</h2>
                            <div class="div_left">
                                <div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Họ tên <span class="fieldRequired">&nbsp;</span>
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtHoTen" runat="server" AssociatedLableText="họ tên"
                                            CssClass="TextBox" DataType="required" Group="reg" />
                                    </div>
                                </div>
                                <%--<div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Ngày sinh
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtNgaySinh" runat="server" AssociatedLableText="ngày sinh" CssClass="TextBox Date" CustomPattern="ddMMyyyy"
                                            DataType="Custom" Group="reg" />
                                    </div>
                                </div>--%>
                                <div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Email <span class="fieldRequired">&nbsp;</span>
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtEmail" runat="server" AssociatedLableText="email" CssClass="TextBox"
                                            CustomPattern="Email" DataType="requiredCustom" Group="reg" />
                                    </div>
                                </div>
                                <div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Điện thoại DĐ
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtCodeDienThoai" runat="server" CssClass="TextBox" CustomPattern="None"
                                            DataType="integer" AssociatedLableText="Điện thoại" CompareData="Number" />
                                    </div>
                                </div>
                                <%--<div class="DivRow">
                                    <div class="DivLabelNormal">
                                        Địa chỉ
                                    </div>
                                    <div class="DivTextBox">
                                        <cc2:ValidationTextBox ID="txtDiaChi" runat="server" CssClass="TextBox" DataType="none" />
                                    </div>
                                </div>--%>
                            </div>
                            <div class="div_right">
                                &nbsp;
                            </div>
                            <div class="div_left" style="width: 100%">
                                <h2>
                                    Thông tin doanh nghiệp</h2>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Tên doanh nghiệp <span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtTendoanhnghiep" runat="server" CssClass='TextBox' AssociatedLableText="tên doanh nghiệp"
                                                DataType="required" Group="reg" />
                                        </div>
                                    </div>
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Năm thành lập<span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtCodeNamtldn" runat="server" CssClass='TextBox' DataType="requiredAndInteger"
                                                AssociatedLableText="năm thành lập doanh nghiệp" MaxLength="4" Range="False"
                                                Group="reg" />
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Loại hình KT <span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <div class="DivTextBox">
                                            <cc3:ValidationDropdownlist ID="ddlLoaiHinhDN" runat="server" Group="reg" CssClass='DropDownList'
                                                AssociatedLableText="loại hình kinh tế" DataType="required" Width="173px" />
                                        </div>
                                    </div>
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Loại hình sản xuất <span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <cc3:ValidationDropdownlist ID="ddlLinhVuc" runat="server" CssClass='DropDownList'
                                            Style="width: 173px" DataType="required" />
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Địa chỉ <span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtTrusochinh" runat="server" Group="reg" CssClass='TextBox'
                                                DataType="required" AssociatedLableText="địa chỉ trụ sở chính" />
                                        </div>
                                    </div>
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            &nbsp; &nbsp; &nbsp;
                                        </div>
                                        <div class="DivTextBox">
                                            &nbsp; &nbsp; &nbsp;
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Tỉnh<span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <div class="DivTextBox">
                                            <cc3:ValidationDropdownlist ID="ddlTinh" runat="server" CssClass='DropDownList' AssociatedLableText="tỉnh"
                                                DataType="required" AutoPostBack="true" Width="173px" Group="reg" />
                                        </div>
                                    </div>
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Huyện <span class="fieldRequired">&nbsp;</span>
                                        </div>
                                        <div class="DivTextBox">
                                            <cc3:ValidationDropdownlist ID="ddlHuyen" runat="server" CssClass='DropDownList'
                                                AssociatedLableText="huyện" DataType="required" Width="173px" Group="reg" />
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Website
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtUrl" runat="server" CssClass='TextBox' CustomPattern="UrlWeb"
                                                DataType="custom" AssociatedLableText="Địa chỉ website" />
                                        </div>
                                    </div>
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Email
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtEmailDN" runat="server" CssClass='TextBox' CustomPattern="Email"
                                                AssociatedLableText="Email" DataType="custom" Group="reg" />
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Điện thoại
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtCodeDienThoaiDN" runat="server" CssClass='TextBox'
                                                CustomPattern="None" DataType="integer" AssociatedLableText="Điện thoại" CompareData="Number" />
                                        </div>
                                    </div>
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormalPortlet">
                                            Fax
                                        </div>
                                        <div class="DivTextBox">
                                            <cc2:ValidationTextBox ID="txtFax" DataType="none" runat="server" CssClass='TextBox' />
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div style="float: left; width: 100%">
                                    <div class="DivRowPortlet">
                                        <div class="DivLabelNormal">
                                        </div>
                                        <div class="DivTextBox">
                                            <div style="display: block; text-align: center;">
                                                &nbsp;<cc1:CaptchaControl ID="CaptchaControl1" runat="server" CaptchaBackgroundNoise="Medium"
                                                    CaptchaLineNoise="Low" Text="Nhập mã kiểm tra:" ToolTip="Nhập mã kiểm tra" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="div_btn" style="margin-left: 137px;">
                            <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" CssClass="btn" CausesValidation="true"
                                CommandArgument="" />
                            <asp:Button ID="btnCancel" CausesValidation="false" runat="server" Text="Hủy" CssClass="btn" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="display: none">
        <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
