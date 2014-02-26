<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ResetPass.aspx.vb" Inherits="Page_Users_ResetPass" %>

<%@ Register src="../../Control/Users/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc1" %>
<%@ Register src="../../Control/Users/ResetPass.ascx" tagname="ResetPass" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server"> 
<uc1:ContentEditor ID="ContentEditor1" runat="server" />
    <uc2:ResetPass ID="ResetPass1" runat="server" />
   
</asp:Content>

