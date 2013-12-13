<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PopupLoaiHinhSanXuat.aspx.vb" Inherits="Page_LoaiHinhSanXuat_PopupLoaiHinhSanXuat" %>

<%@ Register src="../../Control/LoaiHinhSanXuat/PopupLoaiHinhSanXuat.ascx" tagname="PopupLoaiHinhSanXuat" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Popup.css" rel="stylesheet" type="text/css" />
    <script src='<%=ResolveUrl("~/Scripts/jquery-1.4.1.min.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/common.js")%>' type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="padding-botom: 10px;">
        <div style="float: left">
        </div>
        <div style="float: right">
            <asp:ImageButton ID="imgClose" CausesValidation="false" OnClientClick="self.parent.tb_remove();"
                ToolTip="Đóng cửa sổ" ImageUrl="~/images/close_popup.png" align="absmiddle" border="0"
                Width="19" Height="19" runat="server" />
        </div>        
    </div>
    <div>      
    
    
        <uc1:PopupLoaiHinhSanXuat ID="PopupLoaiHinhSanXuat1" runat="server" />
    
    
    </div>
    </form>
</body>
</html>


