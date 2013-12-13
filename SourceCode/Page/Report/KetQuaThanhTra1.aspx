<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="KetQuaThanhTra1.aspx.vb" Inherits="Page_Report_KetQuaThanhTra1" %>

<%@ Register src="../../Control/Report/KetQuaThanhTra1.ascx" tagname="KetQuaThanhTra1" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <uc1:KetQuaThanhTra1 ID="RptKetQuaThanhTra11" runat="server" />

</asp:Content>

