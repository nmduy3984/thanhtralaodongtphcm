﻿<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="CauHoi10.aspx.vb" Inherits="Page_BienBanThanhTra_CauHoi10" %>

<%@ Register src="../../Control/CauHoi/CauHoi10/Create.ascx" tagname="Create" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:Create ID="Create1" runat="server" />
</asp:Content>

