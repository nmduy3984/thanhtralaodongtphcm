<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Detail.ascx.vb" Inherits="Control_QuyetDinhThanhTra_Detail" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            LoadDN($("#<%=hidID.ClientId %>").val());
        });
    }

    //Load danh sách doanh nghiệp
    function LoadDN(SoQD) {
        $.ajax({
            type: "POST", //Phuong thuc truyen du lieu luon la POST
            url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSDNTheoSoQD")%>',
            data: "{'SoQD': '" + SoQD + "'}", //thong so truyen vao
            dataType: "json", //kieu du lieu tra ve
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("Table#ListDoanhNghiep .GridRow").remove();
                if (data.d.length > 0) {
                    for (i = 0; i < data.d.length; i++) {
                        var tr = "";
                        tr += "<tr style='height: 23px;' class='GridRow'>";
                        tr += "<td align='left' style='width: 3%;' class='text_list_links'><span class='TextLabel' >" + (i + 1) + "</span></td>";
                        tr += "<td align='left' style='width: 15%;' class='text_list_links'><a style='color:#003780;' href='../../Page/DoanhNghiepBBTT/Edit.aspx?DNId=" + data.d[i].DoanhNghiepId + "' >" + data.d[i].TenDoanhNghiep + "</a></td>";
                        tr += "<td align='left' style='width: 15%;' class='text_list_links'><span class='TextLabel'>" + data.d[i].DiaChi + "</span></td>";
                        tr += "<td style='width: 4%;'>" + data.d[i].ThoiGianLamViec + "</td>";
                        tr += "</tr>";
                        $("#ListDoanhNghiep").append(tr);
                    }
                }
                else {
                    var tr = "";
                    tr += "<tr style='height: 23px;' class='GridRow'>";
                    tr += "<td align='left' style='width: 3%;' colspan='4' class='text_list_links'><span class='TextLabel' >Không có dữ liệu</span></td>";
                    tr += "</tr>";
                    $("#ListDoanhNghiep").append(tr);
                    return false;
                }
                return true;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //Alertbox(textStatus);
            }
        });
    }
</script>
<div id="view" class="BoxField" runat="server">
    <div class="DivRow tbl_row_0">
        <div class="HeadTitle">
            <h3>
                THÔNG TIN CHI TIẾT QUYẾT ĐỊNH THANH TRA
            </h3>
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTendoanhnghiep_detail" runat="server" Text="Số Quyết định:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblSoQD" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblDienthoai_detail" runat="server" Text="Loại Quyết định:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblLoaiQuyetDinh" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblFax_detail" runat="server" Text="Phạm vi:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblPhamVi" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblNamtldn_detail" runat="server" Text="Căn cứ Luật:" />
        </div>
        <div class="tbl_colRight" style="padding-left: 18px; width: 68%">
            <asp:Label ID="lblCanCuLuat" runat="server" Text="" />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTrusochinh_detail" runat="server" Text="Căn cứ Quyết định:" />
        </div>
        <div class="tbl_colRight" style="padding-left: 18px; width: 68%">
            <asp:Label ID="lblCanCuQD" runat="server" Text="" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTinhid_detail" runat="server" Text="Địa bàn thanh tra, kiểm tra:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblDiaBan" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblHuyenid_detail" runat="server" Text="Thành viên Đoàn:" />
        </div>
        <div class="tbl_colRight" style="padding-left: 13px; width: 68%">
            <asp:Label ID="lblThanhTraDoan" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblLoaihinhdnid_detail" runat="server" Text="Trách nhiệm thi hành:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblTrachNhiemThiHanh" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblLoaihinhsxid_detail" runat="server" Text="Chức danh người kí:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblChucDanh" CssClass="" runat="server" />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblKhucongnghiep_detail" runat="server" Text="Nơi nhận:" />
        </div>
        <div class="tbl_colRight" style="padding-left: 13px; width: 68%">
            <asp:Label ID="lblNoiNhan" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="lblSotknganhang_detail" runat="server" Text="Người ký quyết định:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNguoiKyQuyetDinh" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="Label1" runat="server" Text="Ngày tạo:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNgayTao" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_0">
        <div class="tbl_colLeft">
            <asp:Label ID="Label3" runat="server" Text="Người tạo:" />
        </div>
        <div class="tbl_colRight">
            <asp:Label ID="lblNguoiTao" CssClass="" runat="server" Text=" " />
        </div>
    </div>
    <div class="DivRow tbl_row_1">
        <div class="tbl_colLeft">
            <asp:Label ID="lblTennganhang_detail" runat="server" Text="Danh sách doanh nghiệp thanh tra:" />
        </div>
        <div class="tbl_colRight" style="width:70% !important">
            <table cellspacing="0" style="border-width: 1px; border-style: solid; width: 100%;
                border-collapse: collapse;" id="ListDoanhNghiep" rules="all" class="GridBorder">
                <tbody>
                    <tr align="left" style="height: 26px;" class="GridHeader">
                        <th scope="col">
                            STT
                        </th>
                        <th scope="col">
                            Tên Doanh Nghiệp
                        </th>
                        <th scope="col">
                            Địa chỉ
                        </th>
                        <th scope="col">
                            Thời Gian
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="DivRow" style="text-align: right">
        <asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
            ImageUrl="~/images/back.png" />
    </div>
    <div style="display: none">
        <asp:HiddenField ID="hidID" Value="0" runat="server" />
     </div>
</div>
