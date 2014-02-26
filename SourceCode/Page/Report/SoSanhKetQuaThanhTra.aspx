<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="SoSanhKetQuaThanhTra.aspx.vb" Inherits="Page_Report_SoSanhKetQuaThanhTra" %>

<%@ Register src="../../Control/Report/SoSanhKetQuaThanhTra.ascx" tagname="SoSanhKetQuaThanhTra" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:SoSanhKetQuaThanhTra ID="SoSanhKetQuaThanhTra1" runat="server" />
</asp:Content>

