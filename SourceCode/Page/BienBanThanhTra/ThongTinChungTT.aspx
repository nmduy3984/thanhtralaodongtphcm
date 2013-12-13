<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="ThongTinChungTT.aspx.vb" Inherits="Page_BienBanThanhTra_ThongTinChungTT" %>

<%@ Register src="../../Control/CauHoi/ThongTinChungTT.ascx" tagname="ThongTinChungTT" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ThongTinChungTT ID="ThongTinChungTT1" runat="server" />
</asp:Content>

