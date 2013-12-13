<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DSDoanhNghiepChuaDoMoiTruong.aspx.vb" Inherits="Page_TraCuu_DSDoanhNghiepChuaDoMoiTruong" %>

<%@ Register src="../../Control/TraCuu/DSDNChuaDoMoiTruongLadoDong.ascx" tagname="DSDNChuaDoMoiTruongLadoDong" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:DSDNChuaDoMoiTruongLadoDong ID="DSDNChuaDoMoiTruongLadoDong1" 
        runat="server" />
</asp:Content>

