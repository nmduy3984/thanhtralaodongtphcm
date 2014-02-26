<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false"
    CodeFile="Create.aspx.vb" Inherits="Page_KhuCongNghiep_Create" %>

<%@ Register Src="../../Control/KhuCongNghiep/ContentEditor.ascx" TagName="ContentEditor"
    TagPrefix="uc1" %>
<%@ Register Src="../../Control/KhuCongNghiep/Create.ascx" TagName="Create" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:ContentEditor ID="ContentEditor1" runat="server" />
    <uc2:Create ID="Create1" runat="server" />
</asp:Content>
