﻿<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="CauHoi6.aspx.vb" Inherits="Page_BienBanThanhTra_CauHoi6" %>

<%@ Register src="../../Control/CauHoi/CauHoi6/Create.ascx" tagname="Create" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:Create ID="Create1" runat="server" />
</asp:Content>
