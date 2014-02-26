<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="BienBanViPham.aspx.vb" Inherits="Page_BienBanThanhTra_BienBanViPham" %>

<%@ Register src="../../Control/CauHoi/BienBanViPham.ascx" tagname="BienBanViPham" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:BienBanViPham ID="BienBanViPham1" runat="server" />
</asp:Content>

