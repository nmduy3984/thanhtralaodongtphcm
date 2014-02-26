<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_Roles_Edit" %>
<div class="BoxField">
   
    
    <div class="HeadTitle">
         <h3>CHỈNH SỬA VAI TRÒ</h3>
        <span class="fieldRequired"> &nbsp;</span>Trường yêu cầu nhập dữ liệu
    </div>
     <div class="Error">
        
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    </div>

    <div class="DivRow">
        <div class="DivLabel">
            <asp:Label ID="lblTitle" CssClass="TextLabel" runat="server" Text="Vai trò:" /><span class="fieldRequired"></span>
        </div>
        <div class="DivTextBox">
            <asp:TextBox ID="txtTitle" CssClass="TextBox" runat="server" Text="" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                Display="None" ErrorMessage="Vui lòng nhập tên Vai trò!" SetFocusOnError="True" />
        </div>
    </div>
    <div class="DivRow">
        <div class="DivLabel">
            <asp:Label ID="lblDescription" CssClass="TextLabel" runat="server" Text="Mô tả:" />
        </div>
        <div class="DivTextBox">
            <asp:TextBox ID="txtDescription" CssClass="TextArea" runat="server" Text="" TextMode="MultiLine"/>
        </div>
    </div>
    <div class="DivRow">
        <div class="DivLabel">
            &nbsp;&nbsp;&nbsp;</div>
        <div class="DivTextBox">
            <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" />
            <input type="button" value="Hủy" class="btn" onclick="javascript: history.go(-1)" />&nbsp;
        </div>
    </div>
</div>
<div style="display: none">
    <asp:HiddenField ID="hidID" Value="0" runat="server" />
    <asp:HiddenField ID="hidTitle" Value="0" runat="server" />
</div>
