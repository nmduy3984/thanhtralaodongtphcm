<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DSYkienDoanhNghiep.aspx.vb" Inherits="Page_TraCuu_DSYkienDoanhNghiep" %>

<%@ Register src="../../Control/TraCuu/DSKienNghiCuaDoanhNghiepNam.ascx" tagname="DSKienNghiCuaDoanhNghiepNam" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:DSKienNghiCuaDoanhNghiepNam ID="DSKienNghiCuaDoanhNghiepNam1" 
        runat="server" />
</asp:Content>

