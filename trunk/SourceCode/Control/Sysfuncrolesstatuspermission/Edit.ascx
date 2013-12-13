<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_Sysfuncrolesstatuspermission_Edit" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div  class="BoxField">
<div class="HeadTitle"><h3>CHỈNH SỬA VAI TRÒ - CHỨC NĂNG</h3>
<span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
</div>
 <div class="Error">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    </div>
        <div class="DivRow">
		<div  class="DivLabel">
			<asp:Label ID="Label2" CssClass="TextLabel" runat="server"  Text="Tên chức năng: " /><span class="fieldRequired">&nbsp;</span>
		</div>
		<div  class="DivTextBox" >
            <asp:DropDownList ID="ddlFunctionID" runat="server" AutoPostBack="True" CssClass="TextBox" >
            </asp:DropDownList>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" 
                ControlToValidate="ddlFunctionID" ErrorMessage="Vui lòng chọn tên chức năng" 
                SetFocusOnError="True">*</asp:RequiredFieldValidator>
		</div>
		</div>
		<div class="DivRow">
		<div  class="DivLabel">
			<asp:Label ID="lblRoleid" CssClass="TextLabel" runat="server" Text="Tên vai trò: " ></asp:Label><span class="fieldRequired">&nbsp;</span>
		</div>
		<div  class="DivTextBox" >
			<asp:DropDownList ID="ddlRoleid" runat="server" CssClass="TextBox" >
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="None" 
                ControlToValidate="ddlRoleid" ErrorMessage="Vui lòng chọn tên vai trò" 
                SetFocusOnError="True">*</asp:RequiredFieldValidator>
		</div>
		</div>
		 <div class="DivRow" id="divStatus" runat="server" visible="false">
		<div  class="DivLabel">
			<asp:Label ID="lblStatusid" CssClass="TextLabel" runat="server" Text="Trạng thái: " />
		</div>
		<div  class="DivTextBox" >
			<asp:DropDownList ID="drlStatus" runat="server" CssClass="TextBox">
            </asp:DropDownList>
		</div>
		</div>
		<div class="DivRow">
		<div  class="DivLabel">
			<asp:Label ID="lblAuditnumber" CssClass="TextLabel" runat="server" Text="Quyền" />
		</div>
		<div  class="DivTextBox" >
            <asp:CheckBoxList ID="chklstAuditnumber" runat="server">
            </asp:CheckBoxList>
		</div>
		</div>
		<div class="DivRow">
		<div class="DivLabel" >&nbsp;&nbsp;&nbsp;</div>
		<div class="DivTextBox" >
			<asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" />
			<asp:Button ID="btnHuy" CausesValidation="false" runat="server" Text=" Hủy " CssClass="btn" />&nbsp;
		</div>
		</div>

</div>
<div style="display: none">
<asp:HiddenField ID="hidID" Value="0" runat="server" />
<asp:HiddenField ID="hidStatus" Value="0" runat="server" />
<asp:HiddenField ID="hidFunction" Value="0" runat="server" />
<asp:HiddenField ID="hidRole" Value="0" runat="server" />
<asp:HiddenField ID="hidStatusValue" Value="0" runat="server" />

</div>

</ContentTemplate>
</asp:UpdatePanel>
