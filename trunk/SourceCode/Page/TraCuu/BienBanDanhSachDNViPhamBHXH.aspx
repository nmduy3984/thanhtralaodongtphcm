<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="BienBanDanhSachDNViPhamBHXH.aspx.vb" Inherits="Page_TraCuu_BienBanDanhSachDNViPhamBHXH" %>

<%@ Register src="../../Control/TraCuu/BienBanDanhSachDNViPhamBHXH.ascx" tagname="BienBanDanhSachDNViPhamBHXH" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:BienBanDanhSachDNViPhamBHXH ID="BienBanDanhSachDNViPhamBHXH1" 
        runat="server" />
</asp:Content>

