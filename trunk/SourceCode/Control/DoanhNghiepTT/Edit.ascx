<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_DoanhNghiep_Edit" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            //Kiem tra trung
            $("#<%= txtSochungnhandkkd.ClientID %>").live("keyup",function () {
                var sName = $(this).val();
                $.ajax({
                    type: "POST", //Phuong thuc truyen du lieu luon la POST
                    url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/CheckDuplicateSoDKKD")%>',
                    data: "{'strName': '" + sName + "'}", //thong so truyen vao
                    dataType: "", //kieu du lieu tra ve
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        var resultReturn = eval("(" + result.d + ")");
                        if (resultReturn == "0") {
                            AlertboxThenFocus("Vui lòng nhập số ĐKKD khác. Số này đã tồn tại.", $("#<%= txtSochungnhandkkd.ClientID %>"));
                            $("#<%= txtSochungnhandkkd.ClientID %>").focus();
                            return false;
                        }

                    }, //ham thuc thi neu thanh cong    
                    error: function (XMLHttpRequest, textStatus, errorThrown) {

                    } //ham thuc thi neu loi xay ra.

                });
            });
             

            //Popup
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }
            $(".TextBox:first").focus();
            $('.Date').datepicker({ dateFormat: 'dd/mm/yy' });
            // Dùng cho 2 checkbox có, không
            CheckOnlyAndEffect();
        });
    }

    function CheckValid() {
        return validateFormInputs() ? TestPhieuNhapInfo() : false;
    }
    function TestPhieuNhapInfo() {
        // Khai báo các biến dùng để kiểm tra
        var namTLDN = $("#<%=txtCodeNamtldn.ClientId %>"); // Năm thành lập doanh nghiệp

        var ngayCNKD = $("#<%=txtNgaychungnhandkkd.ClientId %>"); // Ngày chứng nhận đăng ký KD
        var soLanTD = $("#<%=txtLanthaydoi.ClientId %>"); // Số lần thay đổi số chứng nhận đăng ký kinh doanh
        var ngayTDCNKD = $("#<%=txtNgaythaydoi.ClientId %>"); // Ngày thay đổi chứng nhận KD
        var soNVNu = $("#<%=txtSolaodongnu.ClientId %>"); // Số nhân viên nữ
        var tongNV = $("#<%=txtTongsonhanvien.ClientId %>"); //Tổng số nhân viên

        var now = new Date();
        var year = now.getFullYear();
        var months = new Array('01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12');
        var date = ((now.getDate() < 10) ? "0" : "") + now.getDate();
        function fourdigits(number) {
            return (number < 1000) ? number + 1900 : number;
        }
        var today = date + "/" + months[now.getMonth()] + "/" + (fourdigits(now.getYear()));
        today = newDate(today);

        // Kiểm tra năm thành doanh nghiệp 
        if (namTLDN.val() * 1 > year) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Năm thành lập doanh nghiệp phải nhỏ hơn hoặc bằng năm hiện tại.", namTLDN);
            namTLDN.select();
            return false;
        }

        // Nếu có nhập số lần thay đổi CNDK KD thì bắt buộc nhập ngày/
        if (soLanTD.val() * 1 > 0 && ngayTDCNKD.val().trim() == "") {
            AlertboxThenFocus("Vui lòng nhập ngày thay đổi giấy ĐKKD.", ngayTDCNKD);
            return false;
        }

        // Kiểm tra định dạng ngày tháng năm của ngày chứng nhận đăng ký KD
        if (ngayCNKD.val().trim() != "" && !CheckRegular("Date", ngayCNKD.attr("ID"))) {
            AlertboxThenFocus("Ngày cấp giấy ĐKKD chưa đúng định dạng (dd/mm/yyyy)", ngayCNKD);
            return false;
        }

        // Kiểm tra định dạng ngày tháng năm của ngày thay đổi chứng nhận đăng ký KD
        if (ngayTDCNKD.val().trim() != "" && !CheckRegular("Date", ngayTDCNKD.attr("ID"))) {
            AlertboxThenFocus("Ngày thay đổi giấy ĐKKD chưa đúng định dạng (dd/mm/yyyy)", ngayTDCNKD);
            return false;
        }
        // Kiểm tra nếu nhập ngày thay đổi thì bắt buộc nhập số lần thay đổi vào
        if (ngayTDCNKD.val() != "" && soLanTD.val() * 1 == 0) {
            AlertboxThenFocus("Vui lòng nhập lần thay đổi giấy ĐKKD.", soLanTD);
            return false;
        }

        // Kiểm tra xem ngày cấp giấy ĐKKD có lớn hơn ngày hiện tại
        if (newDate(ngayCNKD.val()) > today) {
            AlertboxThenFocus("Vui lòng nhập ngày cấp giấy ĐKKD phải nhỏ hơn ngày hiện tại.", ngayCNKD);
            ngayTDCNKD.select();
            return false;
        }

        // Kiểm tra xem ngày thay đổi giấy ĐKKD có lớn hơn ngày hiện tại
        if (newDate(ngayTDCNKD.val()) > today) {
            AlertboxThenFocus("Vui lòng nhập ngày thay đổi giấy giấy ĐKKD phải nhỏ hơn ngày hiện tại.", ngayTDCNKD);
            ngayTDCNKD.select();
            return false;
        }
        // Kiểm tra xem ngày thay đổi chứng nhận kinh doanh có lơn hơn ngày chứng nhận kinh doanh hay không ?
        if (ngayTDCNKD.val() != "" && newDate(ngayTDCNKD.val()) < newDate(ngayCNKD.val())) {
            AlertboxThenFocus("Ngày thay đổi giấy ĐKKD phải lớn hơn hoặc bằng ngày cấp giấy ĐKKD.", ngayTDCNKD);
            ngayTDCNKD.select();
            return false;
        }

        // Kiểm tra số nhân viên nữ có nhở hơn tổng số nhân viên hay không ?             
        if (soNVNu.val().replace(/,/gi, '') * 1 > tongNV.val().replace(/,/gi, '') * 1) {
            AlertboxThenFocus("Vui lòng nhập số lao động nữ phải nhỏ hơn hoặc bằng tổng số nhân viên doanh nghiệp.", soNVNu);
            soNVNu.select();
            return false;
        }
        return true;
    }

    // Hàm dùng để Reset lại giá trị ban đầu cho  form
    function resetForm() {
        $("div[id*=edit] input[type=text]").val("");
        $("div[id*=edit] textarea").val("");
        $("div[id*=edit] select").attr('selectedIndex', 0);
        return true;
    }

    

</script>
<style type="text/css">
.textmulti
{
    width:690px ;    
}
.divwrap
{
    width:895px;   
}
</style>
<asp:UpdatePanel ID="uplDoanhnghiep" runat="server">
    <ContentTemplate>
        <div class="BoxField" id="edit">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="THÔNG TIN PHIẾU NHẬP" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Div-Left divwrap">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTendoanhnghiep" CssClass="TextLabel" runat="server" Text="1. Tên doanh nghiệp:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtTendoanhnghiep" runat="server" CssClass='TextBox ToolTip textmulti'  AssociatedLableText="Tên ghi đúng theo con dấu"
                    DataType="required" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblNamtllhdn" CssClass="TextLabel" runat="server" Text="Năm bắt đầu hoạt động:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtCodeNamtldn" runat="server" CssClass='TextBox ToolTip' DataType="requiredAndInteger"
                    AssociatedLableText="Vui lòng nhập năm khai trương, không theo năm đổi tên doanh nghiệp" MaxLength="4"
                    MinimumValue="" Range="False" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="Tên viết tắt:" /> 
                </div>
                <cc2:ValidationTextBox ID="txtTenVietTat" runat="server" CssClass='TextBox'   />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left" style="float:left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblLoaihinhdnid" CssClass="TextLabel" runat="server" Text="2. Loại hình kinh tế:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc1:ValidationDropdownlist ID="ddlLoaiHinhDN" runat="server" CssClass='DropDownList'
                    Style="width: 248px" AssociatedLableText="loại hình doanh nghiệp" DataType="required" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTrusochinh" CssClass="TextLabel" runat="server" Text="3. Trụ sở chính:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtTrusochinh" runat="server" CssClass='TextBox ToolTip' DataType="required"
                    AssociatedLableText="số nhà, Đường, Phố, Thôn, Bản (không nhập Huyện, Tỉnh)" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTinhid" CssClass="TextLabel" runat="server" Text="Tỉnh:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc1:ValidationDropdownlist ID="ddlTinh" runat="server" CssClass='DropDownList' AssociatedLableText="tỉnh hoặc nhập tên tỉnh"
                    Style="width: 248px" DataType="required" AutoPostBack="true" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblHuyenid" CssClass="TextLabel" runat="server" Text="Huyện:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc1:ValidationDropdownlist ID="ddlHuyen" runat="server" CssClass='DropDownList'
                    Style="width: 248px" AssociatedLableText="huyện hoặc nhập tên huyện" DataType="required" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblKhucongnghiep" CssClass="TextLabel" runat="server" Text="Khu công nghiệp:" />
                </div>
                <cc1:ValidationDropdownlist ID="ddlKhuCongNghiep" runat="server" CssClass='DropDownList' AssociatedLableText="khu công nghiệp"
                    Style="width: 248px" DataType="none" AutoPostBack="false" />
            </div>
            <div class="DivRow">
            </div>
           <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblLoaihinhsxid1" CssClass="TextLabel" runat="server" Text="4. Lĩnh vực sản xuất kinh doanh chủ yếu:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc1:ValidationDropdownlist ID="ddlLinhVuc" runat="server" CssClass='DropDownList'
                    Style="width: 248px" DataType="required" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                </div>
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblSotknganhang" CssClass="TextLabel" runat="server" Text="Số tài khoản:" />
                </div>
                <cc2:ValidationTextBox ID="txtSotknganhang" runat="server" CssClass='TextBox' CustomPattern="CellPhone" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTennganhang" CssClass="TextLabel" runat="server" Text="tại ngân hàng:" />
                </div>
                <cc2:ValidationTextBox ID="txtTennganhang" runat="server" CssClass='TextBox' CustomPattern="CellPhone" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblDienthoai" CssClass="TextLabel" runat="server" Text="Điện thoại:" />
                </div>
                <cc2:ValidationTextBox ID="txtCodeDienthoai" runat="server" CssClass='TextBox' CustomPattern="None"
                    DataType="integer" AssociatedLableText="điện thoại" CompareData="Number" MaxLength="20" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblFax" CssClass="TextLabel" runat="server" Text="Fax:" />
                </div>
                <asp:TextBox ID="txtFax" runat="server" CssClass='TextBox' />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblEmail" CssClass="TextLabel" runat="server" Text="Email:" />
                </div>
                <cc2:ValidationTextBox ID="txtEmail" runat="server" CssClass='TextBox' CustomPattern="Email"
                    AssociatedLableText="Email" DataType="custom" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblUrl" CssClass="TextLabel" runat="server" Text="Website:" />
                </div>
                <cc2:ValidationTextBox ID="txtUrl" runat="server" CssClass='TextBox' CustomPattern="UrlWeb"
                    DataType="custom" AssociatedLableText="Địa chỉ website" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblSochungnhandkkd" CssClass="TextLabel" runat="server" Text="5. Giấy ĐKKD số:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtSochungnhandkkd" runat="server" CssClass='TextBox ToolTip'
                    DataType="requiredAndAlphaNumeric" AssociatedLableText="số chứng nhận ĐKKD hoặc giấy phép đầu tư" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblNgaychungnhandkkd" CssClass="TextLabel" runat="server" Text="Ngày cấp giấy ĐKKD:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtNgaychungnhandkkd" runat="server" CssClass='TextBox Date ToolTip'
                    AssociatedLableText="đúng định dạng dd/mm/yyyy" DataType="required" />
                (dd/mm/yyyy)
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblLanthaydoi" CssClass="TextLabel" runat="server" Text="Lần thay đổi giấy ĐKKD:" />
                </div>
                <cc2:ValidationTextBox ID="txtLanthaydoi" runat="server" CssClass='TextBox' DataType="integer" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblNgaythaydoi" CssClass="TextLabel" runat="server" Text="Ngày thay đổi giấy ĐKKD:" />
                </div>
                <cc2:ValidationTextBox ID="txtNgaythaydoi" runat="server" CssClass='TextBox Date'
                    CompareData="Date" CompareOperator="GreaterThanEqual" CompareTo="txtNgaychungnhandkkd" />(dd/mm/yyyy)
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label runat="server" ID="lbl6Quimo" CssClass="TextLabel" Text="6. Quy mô: " />
                </div>
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblSochinhanh" CssClass="TextLabel" runat="server" Text="Số chi nhánh, đơn vị hạch toán phụ thuộc:" />
                </div>
                <cc2:ValidationTextBox ID="txtSochinhanh" runat="server" CssClass='TextBox' DataType="integer" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTongsonhanvien" CssClass="TextLabel" runat="server" Text="Tổng số lao động:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtTongsonhanvien" runat="server" CssClass='TextBox ToolTip' AssociatedLableText="Tổng số người đang làm việc đến thời điểm kiểm tra"
                    DataType="requiredAndInteger" />
            </div>
             <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Số người làm công việc có y/c nghiêm ngặt về AT:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtNguoilamCVCoYCNN" runat="server" CssClass='TextBox'
                    DataType="requiredAndInteger" AssociatedLableText="Nếu không có nhập số 0" ToolTip="1. Các công việc tiến hành trong môi trường có yếu tố độc hại như hóa chất độc, phóng xạ, vi sinh vật gây bệnh...;
2. Các công việc thường xuyên tiếp xúc với nguồn điện và các thiết bị điện dễ gây tai nạn;
3. Sản xuất, sử dụng, bảo quản, vận chuyển các loại thuốc nổ và phương tiện nổ (kíp, dây nổ, dây cháy chậm...);
4. Các công việc có khả năng phát sinh cháy, nổ;
5. Các công việc tiến hành trong môi trường có tiếng ồn cao, độ ẩm cao;
6. Khoan, đào hầm lò, hố sâu, khai khoáng, khai thác mỏ;
7. Các công việc trên cao, nơi cheo leo nguy hiểm, trên sông, trên biển, lặn sâu dưới nước;
8. Vận hành, sửa chữa nồi hơi, hệ thống điều chế và nạp khí, bình chịu lực, hệ thống lạnh, đường ống dẫn hơi nước, đường ống dẫn khí đốt; chuyên chở khí nén, khí hóa lỏng, khí hòa tan;
9. Vận hành, sửa chữa các loại thiết bị nâng, các loại máy xúc, xe nâng hàng, thiết bị nâng không dùng cáp hoặc xích, thang máy, thang cuốn;
10. Vận hành, sửa chữa các loại máy cưa, cắt, đột, dập, nghiền, trộn... dễ gây các tai nạn như cuốn tóc, cuốn tay, chân, kẹp, va đập...;
11. Khai thác lâm sản, thủy sản; thăm dò, khai thác dầu khí;
12. Vận hành, sửa chữa, bảo dưỡng máy, thiết bị trong hang hầm, hầm tàu;
13. Sơn, hàn trong thùng kín, hang hầm, đường hầm, hầm tàu;
14. Làm việc trong khu vực có nhiệt độ cao dễ gây tai nạn như: làm việc trên đỉnh lò cốc; sửa chữa lò cốc; luyện cán thép, luyện quặng, luyện cốc; nấu đúc kim loại nóng chảy; lò quay nung clanke xi măng, lò nung vật liệu chịu lửa;
15. Vận hành, bảo dưỡng, kiểm tra các thiết bị giải trí như đu quay, cáp treo, các thiết bị tạo cảm giác mạnh của các công trình vui chơi, giải trí.
" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Số lao động nữ:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtSolaodongnu" runat="server" CssClass='TextBox' AssociatedLableText="Số lao động nữ"
                    DataType="requiredAndInteger" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblSonguoilamnghenguyhiem" CssClass="TextLabel" runat="server" Text="Số người làm công việc độc hại:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtSonguoilamnghenguyhiem" runat="server" CssClass='TextBox ToolTip'
                    DataType="requiredAndInteger" AssociatedLableText="Nếu không có nghề độc hại nhập số 0" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTonggiatrisp" CssClass="TextLabel" runat="server" Text="Tổng giá trị sản phẩm:" />
                </div>
                <cc2:ValidationTextBox ID="txtTonggiatrisp" runat="server" CssClass='TextBox' Style="width: 185px;"
                    AssociatedLableText="tổng giá trị sản phẩm" DataType="numeric" />(tỷ
                đồng)
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Tổng lợi nhuận sau thuế:" />
                </div>
                <cc2:ValidationTextBox ID="txtTongLoiNhuanSauThue" runat="server" CssClass='TextBox'
                    Style="width: 185px;" DataType="numeric" AssociatedLableText="nhập số" />(tỷ
                đồng)
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblIscongdoan" CssClass="TextLabel" runat="server" Text="7. Công đoàn cơ sở:" />
                </div>
                <div class="DivTextBox">
                    <asp:CheckBoxList ID="chkIsCongDoan" CssClass="Child" runat="server" RepeatDirection="Horizontal"
                        Width="200px">
                        <asp:ListItem Value="1"> Có </asp:ListItem>
                        <asp:ListItem Value="0" Selected="True"> Không </asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </div>
           <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="Người liên hệ(phụ trách Nhân sự hoặc An toàn):" />
                </div>
                <cc2:ValidationTextBox ID="txtNguoiLienHe" runat="server" CssClass='TextBox' Style="width: 185px;" />
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="Số điện thoại liên hệ:" />
                </div>
                <cc2:ValidationTextBox ID="txtCodeDienThoaiLH" runat="server" CssClass='TextBox'
                    DataType="integer" MaxLength="20" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="Địa chỉ e-mail:" />
                </div>
                <cc2:ValidationTextBox ID="txtEmailLH" runat="server" CssClass='TextBox' Style="width: 185px;" />
            </div>
            <div class="DivRow">
            </div>
            <div class="DivRow">
                <div class="DivLabelDoanhNghiep">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <div style="float: left; padding-top: 10px;">
                        <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" CausesValidation="true"
                            CommandArgument="" OnClientClick="javascript:return CheckValid();" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Làm lại" OnClientClick="javascript:return resetForm();" /></div>
                    <%--<div style="float: right; text-align: right">
                        &nbsp;<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" /></div>--%>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsHasPermission" Value="0" runat="server" />
            <!-- 0: Không có quyền; 1: có quyền sửa-->
            <asp:TextBox ID="txtIDLoaihinhSX" runat="server"></asp:TextBox>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
