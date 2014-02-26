<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_LoaiHinhSanXuat_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="aspNewControls" Namespace="NewControls" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $("#<%=txtLoaihinhSX.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/SearchAutoComplete")%>',
                        data: "{ 'strSrc': '" + request.term.replace(/&/g, "").replace(/>/g, "").replace(/</g, "").replace(/"/g, "") + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) {
                            return data;
                        },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            //alert(textStatus);
                        }
                    });
                },
                minLength: 3
            });
            // Kiem tra lai loai hinh san xuat
            $("#<%= txtLoaihinhSX.ClientID %>").focusout(function () {
                var sName = $(this).val();
                if (sName != "") {
                    $.ajax({
                        type: "POST", //Phuong thuc truyen du lieu luon la POST
                        url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/CheckNameLoaiHinhSX")%>',
                        data: "{'strName': '" + sName + "'}", //thong so truyen vao
                        dataType: "", //kieu du lieu tra ve
                        contentType: "application/json; charset=utf-8",
                        success: function (result) {
                            $("#<%= txtIDLoaihinhSX.ClientID %>").val("");
                            var resultReturn = eval("(" + result.d + ")");
                            if (resultReturn == "0") {

                                Alertbox("Ngành sản xuất không tồn tại vui lòng chọn tên khác.");
                                $("#<%= txtLoaihinhSX.ClientID %>").val("");
                                $("#<%= txtLoaihinhSX.ClientID %>").focus();
                            } else {
                                $("#<%= txtIDLoaihinhSX.ClientID %>").val(resultReturn);

                            }
                        }, //ham thuc thi neu thanh cong    
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                        } //ham thuc thi neu loi xay ra.

                    });
                }
            });

           

        });
    }
</script>
<asp:UpdatePanel ID="uplYtgchanthuong" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="THÊM LOẠI HÌNH SẢN XUẤT" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblCode" CssClass="TextLabel" runat="server" Text="Mã :" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtCode" CssClass="TextBox" runat="server" DataType="requiredAndAlphaNumeric"
                        placeholder="Chỉ nhập số hoặc chữ cái vd:A1,A2,03..." AssociatedLableText="mã " 
                        MaxLength="5"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblTitle" CssClass="TextLabel" runat="server" Text="Tên loại hình:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtTitle" CssClass="TextBox" runat="server" DataType="required"
                        AssociatedLableText="tên loại hình" MaxLength="200"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Trực thuộc:" />
                </div>
                <div class="DivTextBox">

                    <cc1:ValidationTextBox ID="txtLoaihinhSX" runat="server" CssClass='TextBox'></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblDescription" CssClass="TextLabel" runat="server" Text="Số giờ làm thêm tối đa / năm:" />
                </div>
                <div class="DivTextBox">
                    
                <cc1:ValidationTextBox ID="txtSoGioLamThemToiDaTheoNam" CssClass="TextBox" 
                        runat="server" DataType="requiredAndInteger"
                        AssociatedLableText="Số giờ làm thêm" MaxLength="10"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <asp:Button ID="btnSave" CausesValidation="true" CommandArgument="" OnClientClick="return validateFormInputs();"
                        runat="server" Text=" Lưu " CssClass="btn" />
                    <asp:Button ID="btnHuy" CausesValidation="false" runat="server" Text=" Hủy " CssClass="btn" />&nbsp;
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:TextBox ID="txtIDLoaihinhSX" runat="server"></asp:TextBox>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
