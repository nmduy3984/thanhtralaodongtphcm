﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_KhuCongNghiep_List" %>

<%@ Register src="../../Control/KhuCongNghiep/List.ascx" tagname="List" tagprefix="uc1" %>
<%@ Register src="../../Control/KhuCongNghiep/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:List ID="List1" runat="server" />
</asp:Content>

