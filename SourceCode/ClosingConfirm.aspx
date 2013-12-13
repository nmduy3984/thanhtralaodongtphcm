<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ClosingConfirm.aspx.vb" Inherits="ClosingConfirm" %>


 <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  
<script type="text/javascript">

    var needToConfirm = false;
    window.onbeforeunload = confirmExit;
    function confirmExit() {
        if (needToConfirm)
            return "Thông báo!!!";
    }

    $(function () {
        
        $(':input').bind("change", function () { needToConfirm = true; }); // Prevent accidental navigation away when changing data in form
    });

</script>


Gõ vào đây: <asp:TextBox ID="TextBox1" runat="server" CssClass="TextBox"></asp:TextBox>
<input type="button" value="Save" onclick="needToConfirm = false;" />

</asp:Content>

