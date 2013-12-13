<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="Page_BienBanThanhTra_ListBaoCaoThucHien" %>



<%@ Register src="../../Control/CauHoi/ListBaoCaoThucHien.ascx" tagname="ListBaoCaoThucHien" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ListBaoCaoThucHien ID="ListBaoCaoThucHien1" runat="server" />
    </asp:Content>

