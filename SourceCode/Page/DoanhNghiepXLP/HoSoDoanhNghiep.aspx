<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="HoSoDoanhNghiep.aspx.vb" Inherits="Page_DoanhNghiep_HoSoDoanhNghiep" %>

 
<%@ Register src="../../Control/DoanhNghiepXLP/HoSoDoanhNghiep.ascx" tagname="HoSoDoanhNghiep" tagprefix="uc1" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:HoSoDoanhNghiep ID="HoSoDoanhNghiep2" runat="server" />
 </asp:Content>

