<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_HanhVi_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            THÔNG TIN HÀNH VI</h3>
    </div> </div>
     
       <div class="DivRow tbl_row_1">
            <div class="tbl_colLeft">
                <asp:Label ID="lblMota_detail" CssClass="DivLabel" runat="server" Text="Mô tả:" /></div>
            <div class="tbl_colRight">
                <asp:TextBox ID="lblMota" CssClass="TextBox LargeTextarea " Height="40px" TextMode="MultiLine"
                    runat="server" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="DivRow tbl_row_0">
            <div class="tbl_colLeft">
                <asp:Label ID="lblMucphatmin_detail" CssClass="DivLabel" runat="server" Text="Mức phạt min:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblMucphatmin" CssClass="TextLabel" runat="server" /></div>
        </div>
        <div class="DivRow tbl_row_1">
            <div class="tbl_colLeft">
                <asp:Label ID="lblMucphatmax_detail" CssClass="DivLabel" runat="server" Text="Mức phạt max:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblMucphatmax" CssClass="TextLabel" runat="server" /></div>
        </div>
        <div class="DivRow tbl_row_0">
            <div class="tbl_colLeft">
                <asp:Label ID="lblLoaihanhvi_detail" CssClass="DivLabel" runat="server" Text="Loại hành vi:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblLoaihanhvi" CssClass="TextLabel" runat="server" /></div>
        </div>
     
    <div class="DivRow" style="text-align: right">
        <asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
            ImageUrl="~/images/back.png" />
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
