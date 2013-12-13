<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ContentEditor.ascx.vb"
    Inherits="Control_Sysfuncrolesstatuspermission_ContentEditor" %>
<script type='text/javascript'>
    function ComfirmDialog(strText, value, ObjectIdClick, ID, Type) {
        if (value == 0) {
            Confirmbox(strText, value, ObjectIdClick, ID, Type);
            return false;
        } else {
            eval($("#" + ObjectIdClick + "").attr("href"));
        }
    }
</script>
<div id="view" class="BoxField" runat="server">
    <div class="rowmain_780">
        <div class="tabs_manage">
            <ul>
                <!-- CSS Tabs -->
                <li>
                    <asp:LinkButton ID="lbtList" CausesValidation="false" runat="server"><span>Quản lý</span></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lbtAdd" CausesValidation="false" runat="server"><span>Thêm mới</span></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lbtEdit" CausesValidation="false" runat="server"><span>Sửa</span></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="btnDelete" CausesValidation="false" runat="server"><span>Xóa</span></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lbtView" CausesValidation="false" runat="server"><span>Xem</span></asp:LinkButton></li>
            </ul>
            <div class="tabs_state" id="tabs_state" runat="server" style="display:none">
                &nbsp;&nbsp;<asp:Label ID="lblStatus" runat="server" Text=" Trạng thái:"></asp:Label>
                &nbsp;&nbsp;&nbsp;
		        <asp:DropDownList ID="drlStatus" runat="server"   >
		        </asp:DropDownList>
		    </div>
        </div>
    </div>
    <div style="clear: left">
    </div>
    <!-- het tab quan tri -->
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
<asp:HiddenField ID="hidRoleID" Value="0" runat="server" />
<asp:HiddenField ID="hidStatusID" Value="0" runat="server" />
