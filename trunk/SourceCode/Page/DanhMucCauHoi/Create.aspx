<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Create.aspx.vb" Inherits="Page_DanhMucCauHoi_Create" %>

<%@ Register src="../../Control/DanhMucCauHoi/Create.ascx" tagname="Create" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:Create ID="Create1" runat="server" />
</asp:Content>

