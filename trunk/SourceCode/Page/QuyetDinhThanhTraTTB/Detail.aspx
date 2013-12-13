<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Detail.aspx.vb" Inherits="Page_QuyetDinhThanhTra_Detail" %>

 
<%@ Register src="../../Control/QuyetDinhThanhTraTTB/Detail.ascx" tagname="Detail" tagprefix="uc1" %>
<%@ Register src="../../Control/QuyetDinhThanhTraTTB/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc2" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:ContentEditor ID="ContentEditor1" runat="server" />
    <uc1:Detail ID="Detail1" runat="server" />
 </asp:Content>

