<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_DanhMucTrichDan_Detail" %>
<div id="view" class="BoxField" runat="server">
<div class="DivRow tbl_row_0">
    <div class="HeadTitle">
        <h3>
            XEM TRÍCH DẪN
        </h3>
    </div></div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblCode_detail" runat="server" Text="Nội dung trích dẫn:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNDTrichDan" runat="server" Text="" />
        </div>
    </div>
     
</div>
<asp:HiddenField ID="hidID" Value="0" runat="server" />
