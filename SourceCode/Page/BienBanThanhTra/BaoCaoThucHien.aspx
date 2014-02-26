<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="BaoCaoThucHien.aspx.vb" Inherits="Page_BienBanThanhTra_BaoCaoThucHien" %>

<%@ Register src="../../Control/CauHoi/BaoCaoThucHien.ascx" tagname="BaoCaoThucHien" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:BaoCaoThucHien ID="BaoCaoThucHien1" runat="server" />
</asp:Content>

