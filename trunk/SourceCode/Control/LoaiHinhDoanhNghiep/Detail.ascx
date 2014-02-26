<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_LoaiHinhDoanhNghiep_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            XEM LOẠI HÌNH DOANH NGHIỆP</h3>
    </div></div>
   <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTitle_detail" runat="server" Text="Tên loại hình:" /></div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTitle" runat="server" Text="" /></div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
