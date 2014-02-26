<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_Users_Detail" %>
<div id="view" class="BoxField" runat="server">
    <div class="DivRow tbl_row_0">
        <div class="HeadTitle">
            <h3>
                XEM THÔNG TIN NGƯỜI DÙNG
            </h3>
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblUsername_detail" CssClass="" runat="server" Text="Tên đăng nhập" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblUsername" CssClass="" runat="server" Text="lblUsername" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblEmail_detail" CssClass="" runat="server" Text="Email" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblEmail" CssClass="" runat="server" Text="lblEmail" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblLastlogin_detail" CssClass="" runat="server" Text="Truy cập gần nhất" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblLastlogin" CssClass="" runat="server" Text="lblLastlogin" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblRoleid_detail" CssClass="" runat="server" Text="Vai trò" />:
        </div>
        <div class="tbl_colRight">
            <asp:CheckBoxList runat="server" ID="chklstRole" Enabled="false">
            </asp:CheckBoxList>
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblFullname_detail" CssClass="" runat="server" Text="Tên người dùng" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblFullname" CssClass="" runat="server" Text="lblFullname" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblIsuser_detail" CssClass="" runat="server" Text="Loại người dùng" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblIsuser" CssClass="" runat="server" Text="lblIsuser" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblCreated_detail" CssClass="" runat="server" Text="Ngày tạo" />:
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblCreated" CssClass="" runat="server" Text="lblCreated" /></div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblIsactivated_detail" CssClass="" runat="server" Text="Đã kích hoạt" />:
        </div>
        <div class="tbl_colRight">
            <asp:CheckBox runat="server" ID="CheckIsactivated" Enabled="false"></asp:CheckBox></div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
