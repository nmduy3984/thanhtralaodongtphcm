<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Footer.ascx.vb" Inherits="Control_Footer" %>
<div class="fotter_bg">
    <%--<div class="status_online">
        <span>Thanh tra viên:</span><asp:Label ID="lblThanhTraVien" CssClass="count_online ThanhTraVien"
            runat="server" Text="0" />
    </div>--%>
    <div class="status_online">
        <span>Đang truy cập:</span><asp:Label ID="lblDangTruyCap" CssClass="count_online DangTruyCap"
            runat="server" Text="" />
    </div>
    <div class="status_online">
        <span>Lượt truy cập:</span><asp:Label ID="lblLuotTruyCap" CssClass="count_online LuotTruyCap"
            runat="server" Text="" />
    </div>
</div>
