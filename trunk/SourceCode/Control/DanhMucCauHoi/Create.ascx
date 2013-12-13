<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_DanhMucCauHoi_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();

        });
    }
</script>
<asp:UpdatePanel ID="uplYtgchanthuong" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="THÊM MỤC VI PHẠM" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Câu hỏi ID:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtCauHoiId" CssClass="TextBox " runat="server" DataType="required"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblTitle" CssClass="TextLabel" runat="server" Text="Nội dung mục vi phạm:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtNDMucVP" CssClass="TextBox " runat="server"
                        DataType="required" Height="80px" TextMode="MultiLine"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Parent ID:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtParentId" CssClass="TextBox " runat="server"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Câu hỏi viết tắt:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtCauHoiVietTat" CssClass="TextBox " runat="server"  Height="80px" TextMode="MultiLine"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="Là tiêu chí:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtTieuChi" CssClass="TextBox " runat="server"  Height="80px" TextMode="MultiLine"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="Điều kiện:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtDieuKien" CssClass="TextBox " runat="server"  Height="80px" TextMode="MultiLine"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="Sắp xếp:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtSapXep" CssClass="TextBox " runat="server"  Height="80px" TextMode="MultiLine"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="Là điều kiện mới:" />
                </div>
                <div class="DivTextBox">
                    <asp:CheckBox ID="chkIsTieuChiMoiBC" runat="server" />
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
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
