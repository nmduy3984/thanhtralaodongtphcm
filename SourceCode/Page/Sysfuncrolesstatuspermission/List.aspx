<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_Sysfuncrolesstatuspermission_List" %>

<%@ Register src="../../Control/Sysfuncrolesstatuspermission/List.ascx" tagname="List" tagprefix="uc1" %>
<%@ Register src="../../Control/Sysfuncrolesstatuspermission/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:List ID="List1" runat="server" />
</asp:Content>

