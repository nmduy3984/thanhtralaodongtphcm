<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="BienBanThanhTra.aspx.vb" Inherits="Page_PhieuKiemTra_BienBanThanhTra" %>

<%@ Register src="../../Control/CauHoi/BienBanThanhTra.ascx" tagname="BienBanThanhTra" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:BienBanThanhTra ID="BienBanThanhTra1" runat="server" />
</asp:Content>

