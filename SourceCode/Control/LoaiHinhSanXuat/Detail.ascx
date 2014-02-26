<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_LoaiHinhSanXuat_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            XEM LOẠI HÌNH SẢN XUẤT</h3>
    </div></div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblCode_detail" runat="server" Text="Mã:" /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblCode" runat="server" Text="" /></div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTitle_detail" runat="server" Text="Tên loại hình:" /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTitle" runat="server" Text="" /></div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblParentid_detail" runat="server" Text="Trực thuộc:" /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblParentid" runat="server" Text="" /></div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblDescription_detail" runat="server" Text="Mô tả:" /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblDescription" runat="server" Text="" /></div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
