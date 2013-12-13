<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DSDNChuaHuanLuyenAnToanLaoDong.aspx.vb" Inherits="Page_TraCuu_DSDNChuaHuanLuyenAnToanLaoDong" %>

<%@ Register src="../../Control/TraCuu/DSDNChuaHuanLuyenAnToanLaoDong.ascx" tagname="DSDNChuaHuanLuyenAnToanLaoDong" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:DSDNChuaHuanLuyenAnToanLaoDong ID="DSDNChuaHuanLuyenAnToanLaoDong1" 
        runat="server" />
</asp:Content>

