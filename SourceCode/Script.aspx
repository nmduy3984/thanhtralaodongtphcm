<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Script.aspx.vb" Inherits="Script" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <asp:TextBox ID="txtScript" runat="server" CssClass="TextArea" TextMode="MultiLine" Height="300px" Width="100%"></asp:TextBox><br/>
    <asp:Button ID="submit" runat="server" Text="Submit" />
</asp:Content>

