<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_Log_List" %>

<%@ Register src="../../Control/Log/List.ascx" tagname="List" tagprefix="uc1" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent" ID="listpage">
       <uc1:List ID="List1" runat="server" />
       </asp:Content>