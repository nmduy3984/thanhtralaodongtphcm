<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_PhieuKiemTra_List" %>

<%@ Register src="../../Control/CauHoi/ListPKT.ascx" tagname="ListPKT" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ListPKT ID="ListPKT1" runat="server" />
</asp:Content>

