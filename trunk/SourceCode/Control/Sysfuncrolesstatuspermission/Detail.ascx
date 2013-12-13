<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_Sysfuncrolesstatuspermission_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            XEM THÔNG TIN VAI TRÒ - CHỨC NĂNG
        </h3>
    </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblRoleid_detail" CssClass="DivLabel" runat="server" Text="Vai trò: " /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblRoleid"  runat="server" Text="lblRoleid" /></div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="Label1" CssClass="DivLabel" runat="server" Text="Quyền hệ thống: " /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblFunctionId"   runat="server" Text="" /></div>
    </div>
    <%--<div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblStatusid_detail" CssClass="DivLabel" runat="server" Text="Trạng thái:" /></div>
        <div class="tbl_colRight">
            <asp:DropDownList ID="drlStatus" runat="server" CssClass="TextBox" Enabled="false">
            </asp:DropDownList>
        </div>
    </div>--%>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblAuditnumber_detail" CssClass="DivLabel" runat="server" Text="Quyền: " /></div>
        <div class="tbl_colRight">
            <asp:CheckBoxList ID="chklstAuditnumber" runat="server" Enabled="false">
            </asp:CheckBoxList>
        </div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
