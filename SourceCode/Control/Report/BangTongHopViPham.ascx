<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BangTongHopViPham.ascx.vb"
    Inherits="Control_Report_BangTongHopViPham" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            // Băt sự kiện khi nhập dữ liệu và nhấn Enter
            $("#divNam input").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#btnExportShow").click();
                    return false;
                }
            });

            // Handler cho Checkbox khi click vào 
            $("#<% =trvChuyenMuc.ClientId() %> input").click(function () {
                var id = $(this).attr('id');
                var nameCtrl = id.substring(id.lastIndexOf("_") + 1, id.length - 8);
                CheckOrUnCheck(id, $(this).attr("checked"), nameCtrl);

            });
            //Click vao nhung link trong TreeView ViPham
            $("#<% =trvChuyenMuc.ClientID %> a:not(:has(img))").click(function () {
                //Lay id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = $(this).parent().find("input:checkbox:first").attr('id');

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)                
                $("#" + sameCheckBoxId).attr("checked", !$("#" + sameCheckBoxId).attr("checked"));
                var nameCtrl = $(this).attr('id').substring($(this).attr('id').lastIndexOf("_") + 1, $(this).attr('id').length);
                CheckOrUnCheck(sameCheckBoxId, $("#" + sameCheckBoxId).attr("checked"), nameCtrl);
                return false;
            });

            // Handler cho Checkbox khi click vào 
            $("#<% =trvThongKe.ClientId() %> input").click(function () {
                var id = $(this).attr('id');
                var nameCtrl = id.substring(id.lastIndexOf("_") + 1, id.length - 8);
                CheckOrUnCheck(id, $(this).attr("checked"), nameCtrl);

            });
            //Click vao nhung link trong TreeView ViPham
            $("#<% =trvThongKe.ClientID %> a:not(:has(img))").click(function () {
                //Lay id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = $(this).parent().find("input:checkbox:first").attr('id');

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)                
                $("#" + sameCheckBoxId).attr("checked", !$("#" + sameCheckBoxId).attr("checked"));
                var nameCtrl = $(this).attr('id').substring($(this).attr('id').lastIndexOf("_") + 1, $(this).attr('id').length);
                CheckOrUnCheck(sameCheckBoxId, $("#" + sameCheckBoxId).attr("checked"), nameCtrl);
                return false;
            });

            var hidTinhThanhTraSo = $("#<%=hidTinhThanhTraSo.ClientID %>").val();
            if (hidTinhThanhTraSo > 0)
                $(".NguoiDung").css("display", "none");
            else {
                $(".WrapHuyen").css("display", "none");
                $(".WrapKhuCongNghiep").css("display", "none");
            }
            //Load huyện theo tỉnh
            $("#<%=ddlTinh.ClientId %>").live("change", function () {
                LoadHuyen($(this));
            });
        });
    }
    function LoadHuyen(obj) {
        $(".Huyen select option").remove();
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
                var tdcol2 = "<option value='0'>--- Chọn ---</option>";
                if (data.d.length > 0) {
                    for (i = 0; i < data.d.length; i++) {
                        if (data.d[i].ID.trim().length > 0) {
                            tdcol2 += "<option value='" + data.d[i].ID + "'>" + data.d[i].FullText + "</option>";
                        }
                    }

                }
                $(".Huyen select").append(tdcol2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
    // Check hoặc uncheck control checkbox
    function CheckOrUnCheck(id, status, NameCtrl) {
        var newId;
        //Neu click vao checkbox
        if (id.indexOf("CheckBox") != -1) {
            newId = id.replace("CheckBox", "Nodes");
        }
        else { //Click vao link
            newId = id.replace("trv" + NameCtrl + "t", "trv" + NameCtrl + "n");
            newId = newId + "Nodes";
        }
        $("#" + newId).attr("checked", status);
        //check or uncheck his child
        $("#" + newId + " input").each(function () {
            $(this).attr("checked", status);
        });
    }

    //kiểm tra user nhập
    function checkValid(pos) {
        var error = "";
        var countbug = 0;
        var result = true;

        if (countbug >= 1) {
            //remove all old error
            $('#lstErrorContent li').remove();
            //show new list bug
            $("#lstErrorContent").append(error);
            $('html, body').animate({ scrollTop: $(".Error").height() }, 500);
            result = false
        }
        if (result == true) {
            if (pos == 1) {
                $("#<%=btnExport.ClientID()%>").click();
            }
            else if (pos == 2) {
                $("#<%=btnExportExcel.ClientID()%>").click();
            } else {
                $("#<%=btnBCTong.ClientID()%>").click();
            }
        }
        return false;
    }  
</script>
<style type="text/css">
    .HeaderHidden
    {
        display: none;
    }
</style>
<asp:UpdatePanel ID="uplDailyDebt" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblReportName" runat="server" Text="BẢNG TÔNG HỢP VI PHẠM CỦA DOANH NGHIỆP THEO QUYẾT ĐỊNH THANH TRA" />
                </h3>
            </div>
            <div class="Error-Center">
                <div id="divError">
                    <ul id="lstErrorContent">
                    </ul>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div id="SearchCondition">
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Người dùng:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlNguoiDung" runat="server" CssClass="DropDownList" AutoPostBack="true" />
                    </div>
                </div>
                <div style="clear: both; height: 10px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Số quyết định:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlSoQuyetDinh" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div style="clear: both; height: 10px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="lblAuditnumber" CssClass="TextLabel" runat="server" Text="Tỉnh thành:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlTinh" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow WrapHuyen">
                    <div class="DivLabel">
                         <asp:Label ID="Label6" runat="server" Text=" Quận/Huyện " />
                    </div>
                    <div class="DivTextBox Huyen">
                        <asp:DropDownList ID="ddlHuyen" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow WrapKhuCongNghiep">
                    <div class="DivLabel">
                         <asp:Label ID="Label7" runat="server" Text=" Khu công nghiệp " />
                    </div>
                    <div class="DivTextBox KhuCongNghiep">
                        <asp:DropDownList ID="ddlKhuCongNghiep" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div style="clear: both; height: 10px;">
                </div>
                <div class="DivRow NguoiDung">
                    <div class="DivLabel">
                        <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Người dùng:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlLoaiThanhTra" runat="server" CssClass="DropDownList">
                            <asp:ListItem Value="0">--- Tất cả ---</asp:ListItem>
                            <asp:ListItem Value="1">Thanh tra bộ</asp:ListItem>
                            <asp:ListItem Value="2">Thanh tra sở</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="clear: both; height: 20px;">
                </div>
                <div id="divExt" style="float: left; width: 50%; margin-right: 30px;">
                    <div class="DivRow">
                        <div>
                            <div class="DivLabel">
                                <asp:Label ID="Label1" runat="server" Text="Chọn chuyện muc:" />
                            </div>
                            <div class="DivTextBox" style="height: 300px; width: 75%; overflow: auto; margin-left: 150px;">
                                <asp:TreeView ID="trvChuyenMuc" runat="server" LineImagesFolder="~/TreeLineImages"
                                    ShowCheckBoxes="All" ShowLines="True" CssClass="tree" />
                            </div>
                        </div>
                    </div>
                </div>
                <div style="float: left; width: 40%">
                    <div class="DivRow">
                        <div class="DivLabel">
                            <asp:Label ID="Label5" runat="server" Text="Chọn mục thống kê:" />
                        </div>
                        <br />
                        <div style="height: 300px; overflow: auto;">
                            <div class="DivTextBox">
                                <asp:TreeView ID="trvThongKe" runat="server" LineImagesFolder="~/TreeLineImages"
                                    ShowCheckBoxes="All" ShowLines="True" CssClass="tree" EnableTheming="True">
                                </asp:TreeView>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 20px;">
                </div>
            </div>
            <input type="button" id="btnExportShow" value="Tìm Kiếm" class="btn" onclick="javascript:return checkValid(1);" />
            <input type="button" id="btnExportSum" value="Báo Cáo Tổng" class="btn" onclick="javascript:return checkValid(3);" />
            <span style="display: none">
                <asp:Button ID="btnExport" runat="server" Text="Tìm Kiếm" CausesValidation="false"
                    CssClass="btn" />
                <asp:Button ID="btnExportExcel" runat="server" Text="Xuất Excel" CausesValidation="false"
                    CssClass="btn" />
                <asp:Button ID="btnBCTong" runat="server" Text="Báo Cáo Tổng" CausesValidation="false"
                    CssClass="btn" />
            </span>
            <input type="button" id="btnExportExcelShow" value="Xuất Excel" class="btn" onclick="javascript:return checkValid(2);" />
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="false" EmptyDataText="{Không có dữ liệu}"
            AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
            <HeaderStyle Height="26px" HorizontalAlign="Center" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div style="display:none">
            <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
