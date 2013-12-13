<%@ Page Title="" Language="VB" MasterPageFile="~/DoanhNghiep.master" AutoEventWireup="false" CodeFile="PhieuKienNghi.aspx.vb" Inherits="DoanhNghiep_Page_PhieuKienNghi" %>

<%@ Register src="../Control/PhieuKienNghi.ascx" tagname="PhieuKienNghi" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc1:PhieuKienNghi ID="PhieuKienNghi1" runat="server" />
</asp:Content>

