﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Detail.aspx.vb" Inherits="Page_Roles_Detail" %>

<%@ Register src="../../Control/Roles/Detail.ascx" tagname="Detail" tagprefix="uc1" %>
<%@ Register src="../../Control/Roles/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:Detail ID="Detail1" runat="server" />
</asp:Content>

