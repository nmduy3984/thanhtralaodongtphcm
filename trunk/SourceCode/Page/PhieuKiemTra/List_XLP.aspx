<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List_XLP.aspx.vb" Inherits="Page_PhieuKiemTra_List_XLP" %>

<%@ Register src="../../Control/CauHoi/ListPKT_XLP.ascx" tagname="ListPKT_XLP" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ListPKT_XLP ID="ListPKT_XLP1" runat="server" />
</asp:Content>

