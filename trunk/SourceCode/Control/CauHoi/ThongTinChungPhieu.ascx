<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ThongTinChungPhieu.ascx.vb"
    Inherits="Control_CauHoi_ThongTinChungPhieu" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
             
        });

    }
   
</script>
<div id="view" class="BoxField" runat="server">
    <div class="DivRow tbl_row_0">
        <div class="HeadTitle">
            <h3>
                THÔNG TIN CHI TIẾT DOANH NGHIỆP
            </h3>
        </div>
    </div>
    <div class="DivRow tbl_row_1">
    <div class="ModePhieu" style="font: normal 12px/18px Tahoma, Geneva, sans-serif;
        color: #FF0000;">
        <% If Not IsNothing(Session("ModePhieu")) Then%>
        <% If Session("ModePhieu") = Cls_Common.ModePhieu.Create Then%>
        Mode Tạo mới
        <%ElseIf Session("ModePhieu") = Cls_Common.ModePhieu.Edit Then%>
        Mode Chỉnh sửa
        <%Else%>
        Mode Xem chi tiết
        <%End If%>
        <%Else%>
        Mode Xem chi tiết
        <%End If%>
    </div></div>
    <div style="clear: both;  ">
    </div>
    
     
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTendoanhnghiep_detail" runat="server" Text="Tên doanh nghiệp:" />
        </div>
        <div class="tbl_colRight">
              <asp:LinkButton ID="lkbTendoanhnghiep" runat="server" OnClick="btnSubmit_Click" CausesValidation='False'></asp:LinkButton>
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblDienthoai_detail" runat="server" Text="Điện thoại:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblDienthoai" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblFax_detail" runat="server" Text="Fax:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblFax" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblNamtldn_detail" runat="server" Text="Năm thành lập:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNamtldn" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTrusochinh_detail" runat="server" Text="Trụ sở chính:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTrusochinh" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTinhid_detail" runat="server" Text="Tỉnh:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTinhid" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblHuyenid_detail" runat="server" Text="Huyện:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblHuyenid" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblLoaihinhdnid_detail" runat="server" Text="Loại hình kinh tế:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblLoaihinhdnid" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblLoaihinhsxid_detail" runat="server" Text="Loại hình sản xuất:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblLoaihinhsxid" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblKhucongnghiep_detail" runat="server" Text="Khu Công nghiệp:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblKhucongnghiep" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblSotknganhang_detail" runat="server" Text="Số TK Ngân hàng:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSotknganhang" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTennganhang_detail" runat="server" Text="Tên ngân hàng:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTennganhang" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblUrl_detail" runat="server" Text="Url:" />
        </div>
        <div class="tbl_colRight">
            <asp:HyperLink ID="lblUrl" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblEmail_detail" runat="server" Text="Email:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblEmail" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblSochungnhandkkd_detail" runat="server" Text="Số chứng nhận đăng kiểm kinh doanh:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSochungnhandkkd" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblNgaychungnhandkkd_detail" runat="server" Text="Ngày chứng nhận dkkd:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNgaychungnhandkkd" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblSochinhanh_detail" runat="server" Text="Số chi nhánh:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSochinhanh" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTongsonhanvien_detail" runat="server" Text="Tổng số nhân viên:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTongsonhanvien" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblSolaodongnu_detail" runat="server" Text="Số lao động nữ:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSolaodongnu" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="Label2" runat="server" Text="Số người làm công việc có yêu cầu nghiêm ngặt về an toàn:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNguoilamCVCoYCNN" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblSonguoilamnghenguyhiem_detail" runat="server" Text="Số người làm nghề độc hại:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSonguoilamnghenguyhiem" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTonggiatrisp_detail" runat="server" Text="Tổng giá trị sản phẩm:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTonggiatrisp" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="Label3" runat="server" Text="Tổng lợi nhuận sau thuế:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTongloinhuansauthue" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblIscongdoan_detail" runat="server" Text="Công đoàn:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblIscongdoan" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="Label4" runat="server" Text="Người liên hệ:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNguoiLienHe" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="Label5" runat="server" Text="Điện thoại liên hệ:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblDienThoaiLH" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="Label7" runat="server" Text="Email liên hệ:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblEmailLH" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow" style="text-align: right">
        <asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
            ImageUrl="~/images/back.png" />
    </div>
</div>
<div style="display: none">
    <asp:HiddenField ID="hidID" Value="0" runat="server" />
        <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="submit" />

  </div>
