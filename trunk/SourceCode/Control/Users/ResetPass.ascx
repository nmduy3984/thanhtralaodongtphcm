<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ResetPass.ascx.vb" Inherits="Control_User_Detail" %>
<div id="view" class="BoxField" runat="server">
    <div class="HeadTitle">
        <h3>
            CẤP LẠI MẬT KHẨU NGƯỜI DÙNG
        </h3>
        <span class="fieldRequired"> &nbsp;</span>Trường yêu cầu nhập dữ liệu
    </div>
    <div class="Error">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />        
    </div>
    <div class="DivRow">
        <div class="DivLabel">
            <asp:Label ID="lblUsername_detail" runat="server" Text="Tên đăng nhập" />:
            </div>
        <div class = "DivTextBox">
            <asp:Label ID="lblUsername" CssClass="DivTextBox" runat="server" Text="lblUsername" />
        </div>
    </div>
    <div style="clear: both; padding-top: 4px"></div>
    <div class="DivRow">
        <div class="DivLabel">
            <asp:Label ID="lblPassword" CssClass="TextLabel" runat="server" Text="Mật khẩu:" /><span class="fieldRequired"></span>
        </div>
        <div class="DivTextBox">
            <asp:TextBox ID="txtPassword" CssClass="TextBox" runat="server" Text="" TextMode="Password" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                Display="None" ErrorMessage="Vui lòng nhập mật khẩu" SetFocusOnError="True"> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator
                    id="RegularExpressionValidator1"
                    runat="server"
                    ErrorMessage="Vui lòng nhập mật khẩu tối thiểu 6 ký tự!"
                    Display="None"
                    ControlToValidate="txtPassword"
                    ValidationExpression="[0-9a-zA-Z]{6,}" />
        </div>
    </div>
    <div style="clear: both; padding-top: 4px"></div>
    <div class="DivRow">
        <div class="DivLabel">
            <asp:Label ID="lblEmail_detail" runat="server" Text="Email" />:
            </div>
        <div class = "DivTextBox">
            <asp:Label ID="lblEmail" CssClass="DivTextBox" runat="server" Text="lblEmail" />
        </div>
    </div>
    
    <div style="clear: both; padding-top: 4px"></div>
    <div class="DivRow">
        <div class="DivLabel">
            <asp:Label ID="lblFullname_detail" runat="server" Text="Tên người dùng" />:
            </div>
        <div class = "DivTextBox">
            <asp:Label ID="lblFullname" CssClass="DivTextBox" runat="server" Text="lblFullname" />
        </div>
    </div>
    <div style="clear: both; padding-top: 4px"></div>
    <div class="DivRow">
        <div class="DivLabel">
            &nbsp;
        </div>
        <div class="DivTextBox">
            <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" />
            <input type="button" value="Hủy" class="btn" onclick="javascript: document.location.href = 'List.aspx'" />&nbsp;
        </div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
