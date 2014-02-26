<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="ThongTinChung.aspx.vb" Inherits="Page_PhieuKiemTra_ThongTinChung" %>

<%@ Register src="../../Control/CauHoi/ThongTinChungPhieu.ascx" tagname="ThongTinChungPhieu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ThongTinChungPhieu ID="ThongTinChungPhieu1" runat="server" />
</asp:Content>

