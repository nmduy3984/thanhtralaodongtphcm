<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="UserProfile.aspx.vb" Inherits="Page_Users_UserProfile" %>

<%@ Register src="../../Control/Users/UserProfile.ascx" tagname="UserProfile" tagprefix="uc1" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">
    <uc1:UserProfile ID="UserProfile1" runat="server" />
</asp:Content>

