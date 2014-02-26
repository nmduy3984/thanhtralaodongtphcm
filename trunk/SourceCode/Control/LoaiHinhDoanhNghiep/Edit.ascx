<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_LoaiHinhDoanhNghiep_Edit" %>
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
                    <asp:Label ID="Label3" runat="server" Text="CHỈNH SỬA LOẠI HÌNH DOANH NGHIỆP" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error">
            </div>
            <%--<div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblCode" CssClass="TextLabel" runat="server" Text="Mã loại hình:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtCode" CssClass="TextBox" runat="server" DataType="requiredAndInteger"
                        placeholder="Chỉ nhập số vd:01,02,03..." AssociatedLableText="mã loại hình" 
                        MaxLength="3"></cc1:ValidationTextBox>
                </div>
            </div>--%>
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
            <%--<div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Trực thuộc:" />
                </div>
                <div class="DivTextBox">
                    <asp:DropDownList ID="ddlType" CssClass="DropDownList" runat="server">
            </asp:DropDownList>
		    
                </div>
            </div>--%>
            <%--<div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblDescription" CssClass="TextLabel" runat="server" Text="Mô tả:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtDescription" CssClass="TextArea" runat="server" Text="" TextMode="MultiLine"
                        MaxLength="200" />
                </div>
            </div>--%>
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
