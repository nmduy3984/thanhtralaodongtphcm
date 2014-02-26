<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_BienBanThanhTra_List" %>

<%@ Register src="../../Control/CauHoi/ListBBTT.ascx" tagname="ListBBTT" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ListBBTT ID="ListBBTT1" runat="server" />
</asp:Content>

