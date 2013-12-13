<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="BangTongHopViPham.aspx.vb" Inherits="Page_Report_BangTongHopViPham" %>

<%@ Register src="../../Control/Report/BangTongHopViPham.ascx" tagname="BangTongHopViPham" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:BangTongHopViPham ID="BangTongHopViPham1" runat="server" />
</asp:Content>

