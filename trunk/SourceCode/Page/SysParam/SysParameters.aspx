<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="SysParameters.aspx.vb" Inherits="Page_SysParam_SysParameters" %>


<%@ Register src="../../Control/SysParameter/Create.ascx" tagname="Create" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc1:Create ID="Create1" runat="server" />
    
</asp:Content>

