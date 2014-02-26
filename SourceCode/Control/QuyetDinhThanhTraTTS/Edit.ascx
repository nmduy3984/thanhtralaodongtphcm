<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_QuyetDinhThanhTra_Edit" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();
            datePicker();
            $("#<%=ddlChucDanhNguoiKy.ClientID %>").change(function () {
                SetNoiNhan($(this));
            });
            //Kiểm tra trùng số quyết định
            KiemTraTrungSoQD();

            //---------------------------------
            //  XỬ LÝ THÊM DOANH NGHIỆP
            //---------------------------------
            //Load doanh nghiệp
            LoadDSND();
            //Thêm doanh nghiệp
            $(".hplDSDN").click(function () {
                var flag = 1;
                $("Table#tblDSDN .GridRow").each(function () {
                    var tinh = $(this).find("select").eq(0).val();
                    var huyen = $(this).find("select").eq(1).val();
                    var input1 = $(this).find("input:text").eq(0).val();
                    var input2 = $(this).find("input:text").eq(1).val();
                    var input3 = $(this).find("input:text").eq(2).val();
                    if (input1.trim().length * 1 == 0 || input2.trim().length * 1 == 0 || input3.trim().length * 1 == 0 || tinh * 1 == 0 || huyen * 1 == 0) {
                        flag = 0;
                    }
                });
                if (flag == 1) {
                    LoadTinh($("#<%=hidIsThanhTra.ClientId %>").val(), $("#<%=hidTinhUser.ClientId %>").val());
                }
                else {
                    Alertbox("Vui lòng nhập đầy đủ thông tin doanh nghiệp đã thêm trước khi thêm mới!");
                    return false;
                }
            });
        });
    }
    function SetNoiNhan(obj) {
        var text = "";
        if ($(obj).val() * 1 == 1) {// Nếu là chánh thanh tra
            text = "Như Điều 4;\nTT Đảng Ủy Sở (để b/c);\nBan Giám đốc Sở (để b/c);\nChánh VP Sở (để phối hợp);\nLưu TTr."
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        } else if ($(obj).val() * 1 == 2) {//Nếu là Phó Chánh Thanh tra
            text = "Như Điều 4;\nTT Đảng Ủy Sở (để b/c);\nBan Giám đốc Sở (để b/c);\nChánh VP Sở (để phối hợp);\nLưu TTr."
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        } else if ($(obj).val() * 1 == 3) {//Nếu là Giám đốc Sở
            text = "Như Điều 4;\nLưu TTr, VT."
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        } else if ($(obj).val() * 1 == 4) {//Nếu là Phó Giám đốc Sở
            text = "Như Điều 4;\nGiám đốc (để b/c);\nLưu TTr, VT."
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        } else if ($(obj).val() * 1 == 5) {//Nếu là Phó chủ tịch
            text = ""
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        }
    }
    function CheckValid() {
        return validateFormInputs() ? getReloadInvoiceAddIn() : false;
    }
    //Set up select datetime for textboxs in gridview
    function datePicker() {
        $(".DateNoiCongTac").datepicker({ dateFormat: 'dd/mm/yy' });
    }
    //Remove Row Invoice Add In
    function removeRowInvAddIn(obj) {
        //get tr row
        var trParent = obj.parent().parent();
        var lstDNIdDel = $("#<%=hidlstDNIdDel.ClientId %>").val();
        if (trParent.find("td:first").text().trim().length > 0) {
            lstDNIdDel += trParent.find("td:first").text().trim() + "#";
            $("#<%=hidlstDNIdDel.ClientId %>").val(lstDNIdDel);
        }
        trParent.remove();
    }

    //Load danh sách Tỉnh
    function LoadTinh(IsThanhTra, TinhId) {
        $.ajax({
            url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSTinh")%>',
            data: "{'IsThanhTra': '" + IsThanhTra + "', 'TinhId': '" + TinhId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) {
                return data;
            },
            success: function (data) {
                if (data.d.length > 0) {

                    var tr = "<tr class='GridRow'><td align='center' style='display:none;' ></td><td align='center'><input type='text' class='TextBox' style='width: 350px' /></td><td align='center'><input type='text' class='TextBox' style='width: 260px' /></td>";
                    var tdcol1 = "<td align='center'><select class='DropDownList' onchange='LoadHuyen($(this));' style='width:145px'><option value='0'>--- Chọn ---</option>";
                    var tdcol2 = "<td class='Huyen' align='center'><select class='DropDownList' style='width:165px'><option value='0'>--- Chọn ---</option>";
                    for (i = 0; i < data.d.length; i++) {
                        if (data.d[i].ID.trim().length > 0) {
                            tdcol1 += "<option value='" + data.d[i].ID + "'>" + data.d[i].FullText + "</option>";
                        }

                    }
                    tdcol1 += "</select></td>";
                    tdcol2 += "</select></td>";
                    var tdcol3 = "<td align='center'><input type='text' class='TextBox DateNoiCongTac'  style='width:80px'/></td><td align='center' ><a style='cursor:pointer' onclick='removeRowInvAddIn($(this));'>Xóa</a></td></tr>";
                    tr += tdcol1 + tdcol2 + tdcol3;
                    $("#tblDSDN").append(tr);
                    datePicker();
                }
                else {
                    Alertbox("Không tồn tại dữ liệu Tỉnh.");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //Alertbox(textStatus);
            }
        });
    }
    //Get And Reset Data In hidlstNoiCongTac
    function getReloadInvoiceAddIn() {
        //renew data       
        $("#<%=hidlstDoanhNghiepId.ClientId %>").val("");
        $("#<%=hidlstDoanhNghiep.ClientId %>").val("");
        $("#<%=hidlstDiaChi.ClientId %>").val("");
        $("#<%=hidlstTinh.ClientId %>").val("");
        $("#<%=hidlstHuyen.ClientId %>").val("");
        $("#<%=hidlstThoiGian.ClientId %>").val("");
        var hidlstDoanhNghiepId = "";
        var hidlstDoanhNghiep = "";
        var hidlstDiaChi = "";
        var hidlstTinh = "";
        var hidlstHuyen = "";
        var hidlstThoiGian = "";

        $("Table#tblDSDN .GridRow").each(function () {
            var tinh = $(this).find("select").eq(0).val();
            var huyen = $(this).find("select").eq(1).val();
            var doanhnghiepid = $(this).find("td:first").text();
            var doanhnghiep = $(this).find("input:text").eq(0).val();
            var diachi = $(this).find("input:text").eq(1).val();
            var thoigian = $(this).find("input:text").eq(2).val();
            if (!(doanhnghiep.trim().length * 1 == 0 || diachi.trim().length * 1 == 0 || thoigian.trim().length * 1 == 0 || tinh * 1 == 0 || huyen * 1 == 0)) {
                hidlstDoanhNghiepId += doanhnghiepid + "#";
                hidlstDoanhNghiep += doanhnghiep + "#";
                hidlstDiaChi += diachi + "#";
                hidlstTinh += tinh + "#";
                hidlstHuyen += huyen + "#";
                hidlstThoiGian += thoigian + "#";
            }
        });
        if (hidlstDoanhNghiep.trim().length > 0) {
            $("#<%=hidlstDoanhNghiepId.ClientId %>").val(hidlstDoanhNghiepId);
            $("#<%=hidlstDoanhNghiep.ClientId %>").val(hidlstDoanhNghiep);
            $("#<%=hidlstDiaChi.ClientId %>").val(hidlstDiaChi);
            $("#<%=hidlstTinh.ClientId %>").val(hidlstTinh);
            $("#<%=hidlstHuyen.ClientId %>").val(hidlstHuyen);
            $("#<%=hidlstThoiGian.ClientId %>").val(hidlstThoiGian);
            return true;
        }
        else {
            $("#<%=hidlstDoanhNghiep.ClientId %>").val("");
            $("#<%=hidlstDiaChi.ClientId %>").val("");
            $("#<%=hidlstTinh.ClientId %>").val("");
            $("#<%=hidlstHuyen.ClientId %>").val("");
            $("#<%=hidlstThoiGian.ClientId %>").val("");
            Alertbox("Vui lòng nhập danh sách doanh nghiệp!");
            return false;
        }
    }
    function LoadHuyen(obj) {
        $.ajax({
            url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSHuyen")%>',
            data: "{'TinhId': '" + obj.val() + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) {
                return data;
            },
            success: function (data) {
                var cellHuyen = obj.parent().parent().find(".Huyen select");
                cellHuyen.remove();
                var tdcol2 = "<select class='DropDownList' style='width:165px'><option value='0'>--- Chọn ---</option>";
                if (data.d.length > 0) {
                    for (i = 0; i < data.d.length; i++) {
                        if (data.d[i].ID.trim().length > 0) {
                            tdcol2 += "<option value='" + data.d[i].ID + "'>" + data.d[i].FullText + "</option>";
                        }
                    }
                }
                tdcol2 += "</select>";
                obj.parent().parent().find(".Huyen").append(tdcol2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //Alertbox(textStatus);
            }
        });

    }
    //Load danh sách doanh nghiệp
    function LoadDSND() {
        var hidlstDoanhNghiepId = $("#<%=hidlstDoanhNghiepId.ClientId %>").val().split('#');
        var hidlstDoanhNghiep = $("#<%=hidlstDoanhNghiep.ClientId %>").val().split('#');
        var hidlstDiaChi = $("#<%=hidlstDiaChi.ClientId %>").val().split('#');
        var hidlstTinh = $("#<%=hidlstTinh.ClientId %>").val().split('#');
        var hidlstHuyen = $("#<%=hidlstHuyen.ClientId %>").val().split('#');
        var hidlstThoiGian = $("#<%=hidlstThoiGian.ClientId %>").val().split('#');
        var IsThanhTra = $("#<%=hidIsThanhTra.ClientId %>").val();
        var TinhId = $("#<%=hidTinhUser.ClientId %>").val();
        if (hidlstDoanhNghiep.length > 0) {
            $("Table#tblDSDN .GridRow").remove();
            for (i = 0; i < hidlstDoanhNghiep.length; i++) {
                
                var tr = '';
                tr = "<tr class='GridRow'><td align='center' style='display:none;'>" + hidlstDoanhNghiepId[i] + "</td><td align='center'><input type='text' class='TextBox' value='" + hidlstDoanhNghiep[i] + "' style='width: 350px' /></td><td align='center'><input type='text' class='TextBox' value='" + hidlstDiaChi[i] + "' style='width: 260px' /></td>";
                var tdcol1 = "<td id='Tinh" + i + "' align='center'></td>";
                var tdcol2 = hidlstDoanhNghiep[i] != '' ? "<td class='Huyen' id='Huyen" + i + "' align='center'></td>" : "<td class='Huyen' id='Huyen" + i + "' align='center'><select class='DropDownList' style='width:165px'><option value='0'>--- Chọn ---</option></select></td>";
                var tdcol3 = "<td align='center'><input type='text' class='TextBox DateNoiCongTac'  value='" + hidlstThoiGian[i] + "' style='width:80px'/></td><td align='center' ><a style='cursor:pointer' onclick='removeRowInvAddIn($(this));'>Xóa</a></td></tr>";
                tr += tdcol1 + tdcol2 + tdcol3; 
                $("#tblDSDN").append(tr);
                //Load Tinh
                InsertTinh(IsThanhTra, TinhId, hidlstTinh[i], i);
                //LoadHuyen
                InsertHuyen(hidlstTinh[i], hidlstHuyen[i], i);
                datePicker();
            }
        }
        return false;
    }
    function InsertTinh(IsThanhTra, TinhId, cmpTinhId, index) {
        $.ajax({
            url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSTinh")%>',
            data: "{'IsThanhTra': '" + IsThanhTra + "', 'TinhId': '" + TinhId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) {
                return data;
            },
            success: function (data) {
                tinh = "<select class='DropDownList' onchange='LoadHuyen($(this));' style='width:145px'><option value='0'>--- Chọn ---</option>";
                if (data.d.length > 0) {
                    for (j = 0; j < data.d.length; j++) {
                        tinh += (data.d[j].ID * 1 == cmpTinhId * 1) ? "<option selected='selected' value='" + data.d[j].ID + "'>" + data.d[j].FullText + "</option>" : "<option value='" + data.d[j].ID + "'>" + data.d[j].FullText + "</option>";
                    }
                }
                tinh += "</select>";
                $("#tblDSDN").find("#Tinh" + index).append(tinh);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //Alertbox(textStatus);
            }
        });
    }
    function InsertHuyen(tinhid, cmphuyenid, index) {
        //Load huyen
        $.ajax({
            url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSHuyen")%>',
            data: "{'TinhId': '" + tinhid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) {
                return data;
            },
            success: function (data) {
                huyen = "<select class='DropDownList' style='width:165px'><option value='0'>--- Chọn ---</option>";
                if (data.d.length > 0) {
                    for (j = 0; j < data.d.length; j++) {
                        huyen += (data.d[j].ID * 1 == cmphuyenid * 1) ? "<option selected='selected' value='" + data.d[j].ID + "'>" + data.d[j].FullText + "</option>" : "<option value='" + data.d[j].ID + "'>" + data.d[j].FullText + "</option>";
                    }
                }
                huyen += "</select>";
                $("#tblDSDN").find("#Huyen" + index).append(huyen);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //Alertbox(textStatus);
            }
        });
    }
    function KiemTraTrungSoQD() {
        $("#<%= txtCodeSoQuyetDinh.ClientID %>").live("focusout", function () {
            var SoQD = $(this).val() + $("#<%=lblNDSQD.ClientId %>").text();
            var SoQDNew = $("#<%=hidID.ClientId %>").val();
            if (SoQD != SoQDNew) {
                $.ajax({
                    type: "POST", //Phuong thuc truyen du lieu luon la POST
                    url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/KiemTraTrungSoQD")%>',
                    data: "{'strSoDKKD': '" + SoQD + "'}", //thong so truyen vao
                    dataType: "", //kieu du lieu tra ve
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        var resultReturn = eval("(" + result.d + ")");
                        if (resultReturn == "1") {
                            $("#<%= txtCodeSoQuyetDinh.ClientID %>").focus().addClass("TextBoxInValid");
                            var error = "<li>&nbsp;&nbsp;&nbsp;&nbsp;Vui lòng nhập Số Quyết định khác. Số này đã tồn tại.</li>";
                            //remove all old error
                            $('#lstErrorContent li').remove();
                            //show new list bug
                            $("#lstErrorContent").append(error);
                            $('html, body').animate({ scrollTop: $(".Error").height() }, 800);
                            return false;
                        }
                        else {
                            $("#<%= txtCodeSoQuyetDinh.ClientID %>").removeClass("TextBoxInValid");
                            $('#lstErrorContent li').remove();
                        }
                    }, //ham thuc thi neu thanh cong    
                    error: function (XMLHttpRequest, textStatus, errorThrown) {

                    } //ham thuc thi neu loi xay ra.
                });
            } else {
                $("#<%= txtCodeSoQuyetDinh.ClientID %>").removeClass("TextBoxInValid");
                $('#lstErrorContent li').remove();
            }
        });
    }
</script>
<style type="text/css">
    .TextBoxInValid
    {
        border: 1px solid #FFA500;
    }
</style>
<asp:UpdatePanel ID="uplQuyetdinhthanhtra" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="CHỈNH SỬA QUYẾT ĐỊNH THANH TRA" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error-Center">
                <div id="divError">
                    <ul id="lstErrorContent">
                    </ul>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblSoquyetdinh" CssClass="TextLabel" runat="server" Text="Số Quyết định:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtCodeSoQuyetDinh" CssClass="TextBox" Width="110px" runat="server"
                        Text="" DataType="requiredAndInteger" MaxLength="4" /><asp:Label ID="lblNDSQD" CssClass="TextLabel"
                            runat="server" Text="" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <%--<div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblSoquyetdinh" CssClass="TextLabel" runat="server" Text="Số Quyết định:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <asp:Label ID="lblNDSQD" CssClass="TextLabel" runat="server" Text="" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>--%>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblLoaiquyetdinh" CssClass="TextLabel" runat="server" Text="Loại Quyết định:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc1:ValidationDropdownlist ID="ddlLoaiQuyetDinh" runat="server" CssClass='DropDownList'
                    Style="width: 248px" AssociatedLableText="" DataType="required">
                    <asp:ListItem Value="1">Thanh tra</asp:ListItem>
                    <asp:ListItem Value="2">Kiểm tra</asp:ListItem>
                </cc1:ValidationDropdownlist>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblPV" runat="server" Text="Phạm vi:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtPhamVi" runat="server" CssClass="TextBox SmallTextarea"
                        Height="50px" TextMode="MultiLine" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Căn cứ Luật:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtCanCuLuat" runat="server" CssClass="TextBox LargeTextarea" TextMode="MultiLine" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblCanCuQuyetDinh" CssClass="TextLabel" runat="server" Text="Căn cứ Quyết định:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtCancuquyetdinh" CssClass="TextBox LargeTextarea" TextMode="MultiLine"
                        runat="server" Text="" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblDiabanttkt" CssClass="TextLabel" runat="server" Text="Địa bàn thanh tra, kiểm tra:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc1:ValidationDropdownlist ID="ddlDiaBanTTKT" runat="server" CssClass='DropDownList'
                    Style="width: 248px" AssociatedLableText="" DataType="required" />
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Thành viên Đoàn:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtThanhVienDoan" CssClass="TextBox LargeTextarea" TextMode="MultiLine"
                        runat="server" Text="" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblTrachnhiemthihanh" CssClass="TextLabel" runat="server" Text="Trách nhiệm thi hành:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtTrachNhiemThiHanh" CssClass="TextBox LargeTextarea" TextMode="MultiLine"
                        runat="server" Text="" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblChucdanhnguoiky" CssClass="TextLabel" runat="server" Text="Chức danh người kí:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="fl">
                    <cc1:ValidationDropdownlist ID="ddlChucDanhNguoiKy" runat="server" CssClass='DropDownList'
                        Style="width: 248px" AssociatedLableText="" DataType="required" />
                </div>
                <div class="fl" style="margin: 0 20px 0 15px; font-weight: bold; color: #000000;">
                    <asp:Label ID="lblNoinhan" runat="server" Text="Nơi nhận: " />
                </div>
                <div>
                    <asp:TextBox ID="txtNoiNhan" CssClass="TextBox textarea1" runat="server" TextMode="MultiLine" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblNguoikyquyetdinh" runat="server" Text="Người ký quyết định:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtNguoiKyQuyetDinh" CssClass="TextBox" runat="server"
                        Text="" DataType="required" />
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label9" CssClass="TextLabel" runat="server" Text="Doanh nghiệp thanh tra:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
            </div>
        </div>
        <div style="clear: both; height: 15px;">
        </div>
        <div class="DivRow">
            <div id="DivTextBox"  style="padding-left:30px">
                <table id='tblDSDN' class="GridBorder" style="border-width: 1px; border-style: solid;
                    width: 96%; border-collapse: collapse;" rules="all" cellspacing="0">
                    <tbody>
                        <tr id='Tr2' class='GridHeader' style='height: 26px;'>
                            <th width='1px' align='center' scope='col' style='display: none;'>
                                <b>Id</b>
                            </th>
                            <th width='350px' align='center' scope='col'>
                                <b>Tên doanh nghiệp</b>
                            </th>
                            <th width='260px' align='center' scope='col'>
                                <b>Địa chỉ</b>
                            </th>
                            <th width='145px' align='center' scope='col'>
                                <b>Tỉnh</b>
                            </th>
                            <th width='165px' align='center' scope='col'>
                                <b>Huyện</b>
                            </th>
                            <th width='80px' align='center' scope='col'>
                                <b>Thời gian</b>
                            </th>
                            <td width='30px' scope='col' class='GridTextLink'>
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="DivRow">
            <div class="DivLabel">
                &nbsp;</div>
            <div class="DivTextBox" style="float: right;padding-right:34px">
                <a id="A4" class="hplDSDN" style="color: #005EDB; cursor: pointer; margin-right: 10px"
                    runat="server">Thêm doanh nghiệp</a>
            </div>
        </div>
        <div style="clear: both; height: 15px;">
        </div>
        <div class="DivRow">
            <div class="DivLabel">
                &nbsp;&nbsp;&nbsp;</div>
            <div class="DivTextBox">
                <div style="float: left">
                    <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" OnClientClick="javascript:return CheckValid();" />
                    &nbsp;<asp:Button ID="btnInQuyetDinh" runat="server" CausesValidation="false" CssClass="btn"
                        Text="In Quyết Định" />&nbsp;<asp:Button ID="btnInDSDoanhNghiep" runat="server" CausesValidation="false"
                            CssClass="btn" Text="In Doanh Nghiệp" /></div>
                <div style="float: right; text-align: right">
                    &nbsp;<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                        ImageUrl="~/images/back.png" /></div>
            </div>
        </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsThanhTra" Value="0" runat="server" />
            <asp:HiddenField ID="hidTinhUser" Value="0" runat="server" />
            <asp:HiddenField ID="hidlstDoanhNghiepId" Value="" runat="server" />
            <asp:HiddenField ID="hidlstDoanhNghiep" Value="" runat="server" />
            <asp:HiddenField ID="hidlstDiaChi" Value="" runat="server" />
            <asp:HiddenField ID="hidlstTinh" Value="" runat="server" />
            <asp:HiddenField ID="hidlstHuyen" Value="" runat="server" />
            <asp:HiddenField ID="hidlstThoiGian" Value="" runat="server" />
            <asp:HiddenField ID="hidlstDNIdDel" Value="" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
