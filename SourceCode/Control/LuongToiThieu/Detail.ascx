<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_LuongToiThieu_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            THÔNG TIN LƯƠNG TỐI THIỂU</h3>
    </div>
    </div>
    
        <div class="DivRow tbl_row_1">
            <div class="tbl_colLeft">
                <asp:Label ID="lblMota_detail" CssClass="DivLabel" runat="server" Text="Mô tả:" /></div>
            <div class="tbl_colRight">
                <asp:TextBox ID="lblMota" CssClass="TextBox SmallTextarea " Height="40px" TextMode="MultiLine"
                    runat="server" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="DivRow tbl_row_0">
            <div class="tbl_colLeft">
                <asp:Label ID="lblMucphatmin_detail" CssClass="DivLabel" runat="server" Text="Mức lương tối thiểu:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblMucluongtoithieu" CssClass="TextLabel" runat="server" /></div>
        </div>
        <div class="DivRow tbl_row_1">
            <div class="tbl_colLeft">
                <asp:Label ID="Label1" CssClass="DivLabel" runat="server" Text="Quyết định:" /></div>
             
                <div class="tbl_colRight">
                <asp:TextBox ID="lblQuyetDinh" CssClass="TextBox SmallTextarea " Height="40px" TextMode="MultiLine"
                    runat="server" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="DivRow tbl_row_0">
            <div class="tbl_colLeft">
                <asp:Label ID="Label3" CssClass="DivLabel" runat="server" Text="Ngày quyết định:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblNgayQuyetDinh" CssClass="TextLabel" runat="server" /></div>
        </div>
        <div class="DivRow tbl_row_1">
            <div class="tbl_colLeft">
                <asp:Label ID="lblLoaihanhvi_detail" CssClass="DivLabel" runat="server" Text="Áp dụng đối với DN:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblLoainhanuoc" CssClass="TextLabel" runat="server" /></div>
        </div>
    
    <div class="DivRow" style="text-align: right">
        <asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
            ImageUrl="~/images/back.png" />
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
