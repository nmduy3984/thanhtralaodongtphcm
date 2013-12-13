<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false"
    CodeFile="ThongTinChung.aspx.vb" Inherits="Page_BienBanThanhTra_ThongTinChung" %>

<%@ Register src="../../Control/CauHoi/ThongTinChungBB.ascx" tagname="ThongTinChungBB" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:ThongTinChungBB ID="ThongTinChungBB1" runat="server" />
</asp:Content>
