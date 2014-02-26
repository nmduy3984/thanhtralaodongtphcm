<%@ Page Title="" Language="VB" MasterPageFile="~/DoanhNghiep.master" AutoEventWireup="false" CodeFile="ThongTin.aspx.vb" Inherits="DoanhNghiep_Page_ThongTin" %>

<%@ Register src="../Control/EditThongTin.ascx" tagname="EditThongTin" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:EditThongTin ID="EditThongTin1" runat="server" />
</asp:Content>

