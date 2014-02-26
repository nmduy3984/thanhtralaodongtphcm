<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeFile="UserMenu.aspx.vb" Inherits="Page_Users_UserMenu" %>

<%@ Register src="../../Control/Users/UserMenu.ascx" tagname="UserMenu" tagprefix="uc1" %>

<%@ Register src="../../Control/Users/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">

    <uc2:ContentEditor ID="ContentEditor1" runat="server" />

    <uc1:UserMenu ID="UserMenu1" runat="server" />

</asp:Content>