<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_Function_Edit" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>

<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
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
                    <asp:Label ID="Label3" runat="server" Text="CHỈNH SỬA MENU" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblCode" CssClass="TextLabel" runat="server" Text="Tên menu(không dấu):" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtFunctionName" CssClass="TextBox" runat="server" DataType="required"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Tên hiển thị:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtHrefName" CssClass="TextBox" runat="server" DataType="required"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblTitle" CssClass="TextLabel" runat="server" Text="Url:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtUrl" CssClass="TextBox" runat="server" 
                         ></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Thứ tự:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtSort" CssClass="TextBox" runat="server" 
                        DataType="integer"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblDescription" CssClass="TextLabel" runat="server" Text="Kích hoạt:" />
                </div>
                <div class="DivTextBox">
                    <asp:CheckBox ID="chkIsMenu" CssClass="checkbox" runat="server" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Menu cấp cha:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationDropdownlist ID="ddlParentId" runat="server" CssClass='DropDownList'
                       >
                    </cc1:ValidationDropdownlist>
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