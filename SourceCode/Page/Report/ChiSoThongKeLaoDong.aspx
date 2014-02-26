<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ChiSoThongKeLaoDong.aspx.vb" Inherits="Page_Report_ChiSoThongKeLaoDong" %>

<%@ Register src="../../Control/Report/ChiSoThongKeLaoDongToanQuoc.ascx" tagname="ChiSoThongKeLaoDongToanQuoc" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ChiSoThongKeLaoDongToanQuoc ID="ChiSoThongKeLaoDongToanQuoc1" 
        runat="server" />
</asp:Content>

