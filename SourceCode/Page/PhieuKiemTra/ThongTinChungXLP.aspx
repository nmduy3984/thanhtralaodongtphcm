<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="ThongTinChungXLP.aspx.vb" Inherits="Page_PhieuKiemTra_ThongTinChungXLP" %>

<%@ Register src="../../Control/CauHoi/ThongTinChungXLP.ascx" tagname="ThongTinChungXLP" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ThongTinChungXLP ID="ThongTinChungXLP1" runat="server" />
</asp:Content>

