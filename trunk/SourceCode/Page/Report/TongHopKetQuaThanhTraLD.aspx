<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="TongHopKetQuaThanhTraLD.aspx.vb" Inherits="Page_Report_TongHopKetQuaThanhTraLD" %>

<%@ Register src="../../Control/Report/TongHopKetQuaThanhTraLD.ascx" tagname="TongHopKetQuaThanhTraLD" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:TongHopKetQuaThanhTraLD ID="TongHopKetQuaThanhTraLD1" runat="server" />
</asp:Content>

