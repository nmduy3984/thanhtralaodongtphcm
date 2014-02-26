<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="PhieuKienNghi.aspx.vb" Inherits="Page_PhieuKiemTra_PhieuKienNghi" %>

<%@ Register src="../../DoanhNghiep/Control/PhieuKienNghi.ascx" tagname="PhieuKienNghi" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:PhieuKienNghi ID="PhieuKienNghi1" runat="server" />
</asp:Content>

