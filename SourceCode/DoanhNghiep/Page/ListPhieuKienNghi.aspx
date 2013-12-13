<%@ Page Title="" Language="VB" MasterPageFile="~/DoanhNghiep.master" AutoEventWireup="false" CodeFile="ListPhieuKienNghi.aspx.vb" Inherits="DoanhNghiep_Page_ListPhieuKienNghi" %>

<%@ Register src="../Control/ListPhieuKienNghi.ascx" tagname="ListPhieuKienNghi" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ListPhieuKienNghi ID="ListPhieuKienNghi1" runat="server" />
</asp:Content>

