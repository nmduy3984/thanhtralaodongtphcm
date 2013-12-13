<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_OtherList_List" %>

<%@ Register src="../../Control/OtherList/List.ascx" tagname="List" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:List ID="List1" runat="server" />
</asp:Content>

