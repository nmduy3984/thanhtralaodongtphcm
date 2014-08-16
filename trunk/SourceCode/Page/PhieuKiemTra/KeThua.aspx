<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="KeThua.aspx.vb" Inherits="Page_PhieuKiemTra_KeThua" %>

<%@ Register src="../../Control/CauHoi/KeThuaPKT.ascx" tagname="KeThuaPKT" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:KeThuaPKT ID="KeThuaPKT1" runat="server" />
</asp:Content>

