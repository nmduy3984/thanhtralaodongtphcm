<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="KetQuaThanhTra2.aspx.vb" Inherits="Page_Report_KetQuaThanhTra2" %>

<%@ Register src="../../Control/Report/KetQuaThanhTra2.ascx" tagname="KetQuaThanhTra2" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:KetQuaThanhTra2 ID="KetQuaThanhTra21" runat="server" />
</asp:Content>

