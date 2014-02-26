<%@ Page Title="" Language="VB" MasterPageFile="~/DoanhNghiepCauHoi.master" AutoEventWireup="false" CodeFile="ThongTinChung.aspx.vb" Inherits="DoanhNghiep_Page_PhieuTuKiemTra_ThongTinChung" %>
 <%@ Register src="../../Control/ThongTinChungPhieu.ascx" tagname="ThongTinChungPhieu" tagprefix="uc1" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ThongTinChungPhieu ID="ThongTinChungPhieu1" runat="server" />
 </asp:Content>

