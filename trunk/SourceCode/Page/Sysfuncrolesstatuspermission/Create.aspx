<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Create.aspx.vb" Inherits="Page_Sysfuncrolesstatuspermission_Create" %>

<%@ Register src="../../Control/Sysfuncrolesstatuspermission/Create.ascx" tagname="Create" tagprefix="uc1" %>

<%@ Register src="../../Control/Sysfuncrolesstatuspermission/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">

    <uc2:ContentEditor ID="ContentEditor1" runat="server" />

    <uc1:Create ID="Create1" runat="server" />

</asp:Content>

