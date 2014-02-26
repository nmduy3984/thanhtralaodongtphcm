<%@ Page Title="" Language="VB" MasterPageFile="~/Ano.master" AutoEventWireup="false" CodeFile="Register.aspx.vb" Inherits="Page_Users_Register" %>

<%@ Register src="../../Control/Users/Register.ascx" tagname="Register" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Register ID="Register1" runat="server" />
</asp:Content>

