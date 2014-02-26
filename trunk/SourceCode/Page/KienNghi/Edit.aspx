<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="Page_KienNghi_Edit" %>

<%@ Register src="../../Control/KienNghi/Edit.ascx" tagname="Edit" tagprefix="uc1" %>
<%@ Register src="../../Control/KienNghi/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:Edit ID="Edit1" runat="server" />
</asp:Content>

