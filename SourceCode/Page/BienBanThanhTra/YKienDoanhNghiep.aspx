<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="YKienDoanhNghiep.aspx.vb" Inherits="Page_BienBanThanhTra_YKienDoanhNghiep" %>

<%@ Register src="../../Control/CauHoi/YKienDoanhNghiep.ascx" tagname="YKienDoanhNghiep" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:YKienDoanhNghiep ID="YKienDoanhNghiep1" runat="server" />
</asp:Content>

