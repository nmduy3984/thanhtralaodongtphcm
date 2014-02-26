<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="XemKetLuan.aspx.vb" Inherits="Page_BienBanThanhTra_XemKetLuan" %>

<%@ Register src="../../Control/CauHoi/XemKetLuan.ascx" tagname="XemKetLuan" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:XemKetLuan ID="XemKetLuan1" runat="server" />
</asp:Content>

