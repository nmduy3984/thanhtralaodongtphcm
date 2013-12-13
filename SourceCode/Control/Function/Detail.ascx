<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_Function_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            XEM CHI TIẾT MENU
        </h3>
    </div></div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblCode_detail" runat="server" Text="Tên menu(không dấu):" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblFunctionName" runat="server" Text="" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTitle_detail" runat="server" Text="Tên menu hiển thị:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblHrefName" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="Label1" runat="server" Text="Url:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblUrl" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblDescription" runat="server" Text="Thứ tự:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSort" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="Label3" runat="server" Text="Kích hoạt:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblIsMenu" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblParentid_detail" runat="server" Text="Menu cấp cha:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblParent" runat="server" Text="" />
        </div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />