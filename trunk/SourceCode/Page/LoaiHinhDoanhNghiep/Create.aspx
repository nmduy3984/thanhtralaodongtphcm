﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Create.aspx.vb" Inherits="Page_LoaiHinhDoanhNghiep_Create" %>

<%@ Register src="../../Control/LoaiHinhDoanhNghiep/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc1" %>
<%@ Register src="../../Control/LoaiHinhDoanhNghiep/Create.ascx" tagname="Create" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc1:ContentEditor ID="ContentEditor1" runat="server" /><uc2:Create ID="Create1" runat="server" />
</asp:Content>
