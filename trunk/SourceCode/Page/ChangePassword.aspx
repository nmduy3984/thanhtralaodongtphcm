<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb"
    Inherits="Page_ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <link href="../Styles/Popup.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float:left">
            <img src="../images/arrow_popup.gif" width="10" height="5" alt="" />
            <span class="TitlePopup">Thay đổi mật khẩu</span>
        </div>
        <div style="float:right">
            <asp:ImageButton ID="imgClose" CausesValidation="false" OnClientClick="self.parent.tb_remove();"
                ToolTip="Đóng cửa sổ" ImageUrl="~/images/close_popup.png" align="absmiddle"
                border="0" Width="19" Height="19" runat="server" />
        </div>
    </div>
    <div style="clear:both"/>
    <div>    

        <div class="Error">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
        </div>
        <div  class="lblEdit">
            <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
        </div>
        <div class="lblEdit">
            <asp:Label ID="Label1" runat="server" Text="Mật khẩu cũ"></asp:Label><span class="fieldRequired">&nbsp;</span>
        </div>
        <div>
    
            <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="TextBox" Width="180px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassword"
                ErrorMessage="Hãy nhập mật khẩu cũ" SetFocusOnError="True">*</asp:RequiredFieldValidator></div>
        <div class="lblEdit">
             <asp:Label ID="Label2" runat="server" Text="Mật khẩu mới: "></asp:Label><span class="fieldRequired">&nbsp;</span>
        </div>
        <div>
   
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="TextBox" Width="180px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword"
                ErrorMessage="Hãy nhập mật khẩu mới" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtReNewPassword"
                ControlToValidate="txtNewPassword" ErrorMessage="Mật khẩu mới và nhập lại mật khẩu mới không chính xác.">*</asp:CompareValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNewPassword"
                ErrorMessage="Mật khẩu ít nhất 6 ký tự" ValidationExpression="[\S+ ]{6,}">*</asp:RegularExpressionValidator></div>
        <div class="lblEdit">
              <asp:Label ID="Label3" runat="server" Text="Nhập lại mật khẩu mới"></asp:Label><span class="fieldRequired">&nbsp;</span>
        </div>
        <div>
   
            <asp:TextBox ID="txtReNewPassword" runat="server" TextMode="Password" CssClass="TextBox" Width="180px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtReNewPassword"
                ErrorMessage="Hãy nhập 'Nhập lại mật khẩu mới' " SetFocusOnError="True">*</asp:RequiredFieldValidator></div>
        
        </div>
    <div style="padding-top:5px">
        <asp:Button CssClass="btn"  ID="btnSave" Text=" Lưu " runat="server" /> &nbsp;
        <asp:Button Text=" Hủy " ID="btnCancel" runat="server"  CssClass="btn" OnClientClick="self.parent.tb_remove();" CausesValidation="false"/>    
    </div>
    </form>
</body>
</html>
