<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_Users_Edit" %>
<div class="HeadTitle">
    <h3>
        CHỈNH SỬA NGƯỜI DÙNG
    </h3>
    <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
</div>
<div class="Error">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblUsername" CssClass="TextLabel" runat="server" Text="Tên đăng nhập:" /><span
            class="fieldRequired"></span>
        <asp:HiddenField ID="curUserName" runat="server" />
    </div>
    <div class="DivTextBox">
        <asp:TextBox ID="txtUsername" CssClass="TextBox" runat="server" Text="" ReadOnly="true"/>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsername"
            Display="None" ErrorMessage="Vui lòng nhập tên đăng nhập" SetFocusOnError="True"> </asp:RequiredFieldValidator>
    </div>
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblPassword" CssClass="TextLabel" runat="server" Text="Mật khẩu:" /><span
            class="fieldRequired"></span>
    </div>
    <div class="DivTextBox">
        <asp:TextBox ID="txtPassword" CssClass="TextBox" runat="server" Text="" TextMode="Password" />
    </div>
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblFullname" CssClass="TextLabel" runat="server" Text="Họ và Tên:" /><span
            class="fieldRequired"></span>
    </div>
    <div class="DivTextBox">
        <asp:TextBox ID="txtFullname" CssClass="TextBox" runat="server" Text="" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFullname"
            Display="None" ErrorMessage="Vui lòng nhập họ và tên" SetFocusOnError="True"> </asp:RequiredFieldValidator>
    </div>
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblEmail" CssClass="TextLabel" runat="server" Text="Email:" /><span
            class="fieldRequired"></span>
    </div>
    <div class="DivTextBox">
        <asp:TextBox ID="txtEmail" CssClass="TextBox" runat="server" Text="" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail"
            Display="None" ErrorMessage="Vui lòng nhập địa chỉ Email" SetFocusOnError="True"> </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Vui lòng kiểm tra lại Email"
            ValidationExpression="^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" ControlToValidate="txtEmail"
            Display="None" SetFocusOnError="True">*</asp:RegularExpressionValidator>
    </div>
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblRoleid" CssClass="TextLabel" runat="server" Text="Vai trò:" />
    </div>
    <div class="DivTextBox">
        <asp:CheckBoxList runat="server" ID="chklstRole">
            <asp:ListItem Value="1" Text="--- Chọn --- " />
        </asp:CheckBoxList>
    </div>
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblIsuser" CssClass="TextLabel" runat="server" Text="Loại người dùng:" />
        <span class="fieldRequired"></span>
    </div>
    <div class="DivTextBox">
        <asp:DropDownList ID="ddlTypeUser" runat="server" CssClass="DropDownList" Width="248px">
            <asp:ListItem>--- Chọn ---</asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="None"
            ControlToValidate="ddlTypeUser" ErrorMessage="Vui lòng chọn loại người dùng"
            SetFocusOnError="True">*</asp:RequiredFieldValidator>
    </div>
</div>
<div class="DivRow">
    <div class="DivLabel">
        <asp:Label ID="lblIsactivated" CssClass="TextLabel" runat="server" Text="Kích hoạt:" />
    </div>
    <div class="DivTextBox">
        <asp:CheckBox runat="server" ID="chkIsActivated" Checked="true" /></div>
</div>
<div class="DivRow">
    <div class="DivLabel">
    </div>
    <div class="DivTextBox" style="padding-left: 168px!important; padding-top: 10px;">
        <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" />
        <input type="button" value="Hủy" class="btn" onclick="javascript: history.go(-1)" />&nbsp;
    </div>
</div>
<div style="display: none">
    <asp:HiddenField ID="hidID" Value="0" runat="server" />
    <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
</div>
