<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Create.aspx.vb" Inherits="Page_QuanHuyen_Create" %>

<%@ Register src="../../Control/QuanHuyen/Create.ascx" tagname="Create" tagprefix="uc1" %>
<%@ Register src="../../Control/QuanHuyen/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:Create ID="Create1" runat="server" />
</asp:Content>

