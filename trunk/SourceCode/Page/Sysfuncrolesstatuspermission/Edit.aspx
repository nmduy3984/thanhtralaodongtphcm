﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="Page_Sysfuncrolesstatuspermission_Edit" %>

<%@ Register src="../../Control/Sysfuncrolesstatuspermission/Edit.ascx" tagname="Edit" tagprefix="uc1" %>
<%@ Register src="../../Control/Sysfuncrolesstatuspermission/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:Edit ID="Edit1" runat="server" />
</asp:Content>
