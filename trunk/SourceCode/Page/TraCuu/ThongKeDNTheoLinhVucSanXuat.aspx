<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ThongKeDNTheoLinhVucSanXuat.aspx.vb" Inherits="Page_TraCuu_ThongKeDNTheoLinhVucSanXuat" %>

<%@ Register src="../../Control/TraCuu/ThongKeDNTheoLinhVucSanXuat.ascx" tagname="ThongKeDNTheoLinhVucSanXuat" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:ThongKeDNTheoLinhVucSanXuat ID="ThongKeDNTheoLinhVucSanXuat1" 
        runat="server" />
</asp:Content>

