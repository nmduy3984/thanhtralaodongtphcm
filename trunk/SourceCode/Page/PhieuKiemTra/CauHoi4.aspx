<%@ Page Title="" Language="VB" MasterPageFile="~/CauHoi.master" AutoEventWireup="false" CodeFile="CauHoi4.aspx.vb" Inherits="Page_PhieuKiemTra_CauHoi4" %>

<%@ Register src="../../Control/CauHoi/CauHoi4/Create.ascx" tagname="Create" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:Create ID="Create1" runat="server" />
</asp:Content>

