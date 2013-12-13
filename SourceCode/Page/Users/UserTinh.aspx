<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="UserTinh.aspx.vb" Inherits="Page_Users_UserTinh" %>

<%@ Register src="../../Control/Users/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc1" %>
<%@ Register src="../../Control/Users/UserTinh.ascx" tagname="UserTinh" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<uc1:ContentEditor ID="ContentEditor1" runat="server" />
    <uc2:UserTinh ID="UserTinh1" runat="server" />
    
</asp:Content>

