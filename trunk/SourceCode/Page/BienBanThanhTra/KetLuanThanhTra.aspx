<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="KetLuanThanhTra.aspx.vb" Inherits="Page_BienBanThanhTra_KetLuanThanhTra" %>

<%@ Register src="../../Control/CauHoi/PhieuKetLuan.ascx" tagname="PhieuKetLuan" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:PhieuKetLuan ID="PhieuKetLuan1" runat="server" />
</asp:Content>

