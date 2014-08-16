<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="KeThuaXLP.aspx.vb" Inherits="Page_PhieuKiemTra_KeThuaXLP" %>

<%@ Register src="../../Control/CauHoi/KeThuaPKT_XLP.ascx" tagname="KeThuaPKT_XLP" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:KeThuaPKT_XLP ID="KeThuaPKT_XLP1" runat="server" />
</asp:Content>

