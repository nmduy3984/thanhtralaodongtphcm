<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="PhieuPhanTichTinhHinhViPhamLD.aspx.vb" Inherits="Page_Report_PhieuPhanTichTinhHinhViPhamLD" %>

<%@ Register src="../../Control/Report/PhieuPhanTichTinhHinhViPhamLD.ascx" tagname="PhieuPhanTichTinhHinhViPhamLD" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:PhieuPhanTichTinhHinhViPhamLD ID="PhieuPhanTichTinhHinhViPhamLD1" 
        runat="server" />
</asp:Content>

