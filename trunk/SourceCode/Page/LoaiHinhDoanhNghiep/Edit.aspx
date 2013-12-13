<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="Page_LoaiHinhDoanhNghiep_Edit" %>

<%@ Register src="../../Control/LoaiHinhDoanhNghiep/ContentEditor.ascx" tagname="ContentEditor" tagprefix="uc1" %>
<%@ Register src="../../Control/LoaiHinhDoanhNghiep/Edit.ascx" tagname="Edit" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc1:ContentEditor ID="ContentEditor1" runat="server" /><uc2:Edit ID="Edit1" runat="server" />
</asp:Content>

