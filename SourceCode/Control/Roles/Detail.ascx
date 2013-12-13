<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_Roles_Detail" %>
<div id="view" class="BoxField" runat="server">
    <div class="DivRow tbl_row_0">
        <div class="HeadTitle">
            <h3>
                XEM THÔNG TIN VAI TRÒ</h3>
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTitle_detail" CssClass="DivLabel" runat="server" Text="Tên vai trò" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTitle"   runat="server" Text="lblTitle" /></div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblDescription_detail" CssClass="DivLabel" runat="server" Text="Mô tả" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblDescription"   runat="server" Text="lblDescription" /></div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
