<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Top100DoanhNghiep.aspx.vb" Inherits="Page_TraCuu_Top100DoanhNghiep" %>

<%@ Register src="../../Control/TraCuu/DS100DoanhNghiep.ascx" tagname="DS100DoanhNghiep" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:DS100DoanhNghiep ID="DS100DoanhNghiep1" runat="server" />
</asp:Content>

