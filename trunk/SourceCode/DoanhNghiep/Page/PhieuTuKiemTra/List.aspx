<%@ Page Title="" Language="VB" MasterPageFile="~/DoanhNghiep.master" AutoEventWireup="false" CodeFile="List.aspx.vb" Inherits="DoanhNghiep_Page_PhieuTuKiemTra_List" %>



<%@ Register src="../../Control/ListPheuTuKiemTra.ascx" tagname="ListPheuTuKiemTra" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc1:ListPheuTuKiemTra ID="ListPheuTuKiemTra1" runat="server" />
    
</asp:Content>

