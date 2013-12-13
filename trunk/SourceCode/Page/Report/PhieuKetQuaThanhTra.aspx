<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="PhieuKetQuaThanhTra.aspx.vb" Inherits="Page_Report_PhieuKetQuaThanhTra" %>

<%@ Register src="../../Control/Report/PhieuKetQuaThanhTra.ascx" tagname="PhieuKetQuaThanhTra" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:PhieuKetQuaThanhTra ID="PhieuKetQuaThanhTra1" runat="server" />
</asp:Content>

