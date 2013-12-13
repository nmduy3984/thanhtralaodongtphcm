<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_KienNghi_List" %>

<%@ Register src="../../Control/KienNghi/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc1" %>
<%@ Register src="../../Control/KienNghi/List.ascx" tagname="List" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ContentEditor ID="ContentEditor1" runat="server" />
    <uc2:List ID="List1" runat="server" />
</asp:Content>

