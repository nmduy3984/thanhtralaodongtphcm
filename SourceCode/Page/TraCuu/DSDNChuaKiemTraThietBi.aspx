<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DSDNChuaKiemTraThietBi.aspx.vb" Inherits="Page_TraCuu_DSDNChuaKiemTraThietBi" %>

<%@ Register src="../../Control/TraCuu/DSDNChuaKiemTraThietBi.ascx" tagname="DSDNChuaKiemTraThietBi" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:DSDNChuaKiemTraThietBi ID="DSDNChuaKiemTraThietBi1" runat="server" />
</asp:Content>

