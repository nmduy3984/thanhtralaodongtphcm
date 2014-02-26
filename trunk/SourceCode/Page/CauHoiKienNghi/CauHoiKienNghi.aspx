<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="CauHoiKienNghi.aspx.vb" Inherits="Page_CauHoiKienNghi_CauHoiKienNghi" %>

<%@ Register src="../../Control/CauHoiKienNghi/CauHoiKienNghi.ascx" tagname="CauHoiKienNghi" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:CauHoiKienNghi ID="CauHoiKienNghi1" runat="server" />
</asp:Content>

