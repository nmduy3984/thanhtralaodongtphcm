<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Create.aspx.vb" Inherits="Page_Roles_Create" %>

<%@ Register src="../../Control/Roles/Create.ascx" tagname="Create" tagprefix="uc1" %>
<%@ Register src="../../Control/Roles/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:Create ID="Create1" runat="server" />
</asp:Content>

