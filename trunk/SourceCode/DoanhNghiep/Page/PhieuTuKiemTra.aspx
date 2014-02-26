<%@ Page Title="" Language="VB" MasterPageFile="~/DoanhNghiep.master" AutoEventWireup="false" CodeFile="PhieuTuKiemTra.aspx.vb" Inherits="DoanhNghiep_Page_PhieuTuKiemTra" %>

<%@ Register src="../Control/ListPheuTuKiemTra.ascx" tagname="ListPheuTuKiemTra" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ListPheuTuKiemTra ID="ListPheuTuKiemTra1" runat="server" />
</asp:Content>

