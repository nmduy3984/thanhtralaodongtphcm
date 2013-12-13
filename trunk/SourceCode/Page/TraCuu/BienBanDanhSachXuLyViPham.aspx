<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="BienBanDanhSachXuLyViPham.aspx.vb" Inherits="Page_TraCuu_BienBanDanhSachXuLyViPham" %>

<%@ Register src="../../Control/TraCuu/BienBanDanhSachXuLyViPham.ascx" tagname="BienBanDanhSachXuLyViPham" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:BienBanDanhSachXuLyViPham ID="BienBanDanhSachXuLyViPham1" runat="server" />
</asp:Content>

