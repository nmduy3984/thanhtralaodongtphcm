<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_KienNghi_Detail" %>
<div id="view" class="BoxField" runat="server">
    <div class="DivRow tbl_row_0">
        <div class="HeadTitle">
            <h3>
                THÔNG TIN KIẾN NGHỊ</h3>
        </div>
    </div>
    
        <div class="DivRow tbl_row_1">
            <div class="tbl_colLeft">
                <asp:Label ID="lblMota_detail" CssClass="DivLabel" runat="server" Text="Nội dung:" /></div>
            <div class="tbl_colRight">
                <asp:TextBox ID="lblMota" CssClass="TextBox SmallTextarea " Height="40px" TextMode="MultiLine"
                    runat="server" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="DivRow tbl_row_0">
            <div class="tbl_colLeft">
                <asp:Label ID="Label1" CssClass="DivLabel" runat="server" Text="Mục vi phạm:" /></div>
            <div class="tbl_colRight">
                <asp:Label ID="lblMucViPham" CssClass="DivLabel" Style="font-weight: normal; width: 100%;" runat="server" />
            </div>
        </div>
   
    <div class="DivRow" style="text-align: right">
        <asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
            ImageUrl="~/images/back.png" />
    </div>
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
