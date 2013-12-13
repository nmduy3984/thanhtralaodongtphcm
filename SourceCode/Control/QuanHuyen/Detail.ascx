<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_QuanHuyen_Detail" %>
<div id="view" class="BoxField" runat="server">
    <div class="DivRow tbl_row_0">
        <div class="HeadTitle">
            <h3>
                XEM QUẬN-HUYỆN
            </h3>
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTitle_detail" runat="server" Text="Tên Quận/Huyện:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTitle" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblParentid_detail" runat="server" Text="Tỉnh/TP:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTinhTP" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="Label1" runat="server" Text="Lương tối thiểu:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblLuongToiThieu" runat="server" Text="" />
        </div>
    </div>
    <div style="clear: both">
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblDescription" runat="server" Text="Mô tả:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblDescription_detail" runat="server" Text="" />
        </div>
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
