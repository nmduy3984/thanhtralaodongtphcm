<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChiSoThongKeLaoDongToanQuoc.ascx.vb"
    Inherits="Control_Report_ChiSoThongKeLaoDongToanQuoc" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $("#<%=ddlNam.ClientID %>").change(function (event) {
                var valueType = $("#SearchType option:selected").val();
                if (valueType * 1 > 0) {
                    $("#<%=lblReportName.ClientID %>").text('Chỉ số thống kê lao động tỉnh năm ' + valueType);
                }
                else {
                    $("#<%=lblReportName.ClientID %>").text('Chỉ số thống kê lao động tỉnh theo năm');
                }
                return false;
            });

            // Handler cho Checkbox khi click vào 
            $("#<% =trvTinh.ClientID %> input").click(function () {
                var id = $(this).attr('id');
                CheckOrUnCheck(id, $(this).attr("checked"));
                var divParent = $(this).parent();
            });

            //Click vao nhung link trong TreeView ViPham
            $("#<% =trvTinh.ClientID %> a:not(:has(img))").click(function () {
                //Lay id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = $(this).parent().find("input:checkbox:first").attr('id');

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)                
                $("#" + sameCheckBoxId).attr("checked", !$("#" + sameCheckBoxId).attr("checked"));
                CheckOrUnCheck(sameCheckBoxId, $("#" + sameCheckBoxId).attr("checked"), "trvTinh");
                return false;
            });

            var hidTinhThanhTraSo = $("#<%=hidTinhThanhTraSo.ClientId %>").val();
            if (hidTinhThanhTraSo > 0)
                $("#divTinh").css("display","none");
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
        var ddlNam = $("#<%=ddlNam.ClientId %> option:selected").val();
        if (ddlNam * 1 == 0) {
            Alertbox("Vui lòng chọn năm báo cáo!");
            result = false;
        }
        var checkedItem = $("#<% =trvTinh.ClientId() %> input:checked").size();
        var hidTinhThanhTraSo = $("#<%=hidTinhThanhTraSo.ClientId %>").val();
        if (ddlNam * 1 > 0 && checkedItem == 0 && hidTinhThanhTraSo == 0) {
            Alertbox("Vui lòng chọn ít nhất 1 tỉnh để báo cáo!");
            result = false;
        }

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
            else {
                $("#<%=btnExportExcel.ClientID()%>").click();
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
                    <asp:Label ID="lblReportName" runat="server" Text="Chỉ số thống kê lao động toàn quốc/tỉnh theo năm" />
                </h3>
            </div>
            <%-- <div class="DivRequired Center">
                <span class="fieldRequired">&nbsp;</span>
            </div>--%>
            <div class="Error-Center">
                <div id="divError">
                    <ul id="lstErrorContent">
                    </ul>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div id="SearchCondition">
                <div id="SearchType" style="float: left; margin-right: 100px">
                    <div class="DivLabel">
                        <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Chọn năm báo cáo: " />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList runat="server" ID="ddlNam" CssClass="DropDownList" AutoPostBack="false" />
                    </div>
                </div>
                <div style="float: left;">
                    <div id="divTinh">
                        <div class="DivLabel">
                            <asp:Label ID="lblAuditnumber" CssClass="TextLabel" runat="server" Text="Tỉnh thành:" />
                        </div>
                        <br />
                        <div style="height: 260px; width: 200px; overflow: auto;">
                            <div class="DivTextBox">
                                <asp:TreeView ID="trvTinh" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                                    ShowLines="True" CssClass="tree">
                                </asp:TreeView>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 20px;">
                </div>
                <input type="button" id="btnExportShow" value="Tìm Kiếm" class="btn" onclick="javascript:return checkValid(1);" />
                <span style="display: none">
                    <asp:Button ID="btnExport" runat="server" Text="Tìm Kiếm" CausesValidation="false"
                        CssClass="btn" />
                    <asp:Button ID="btnExportExcel" runat="server" Text="Xuất Excel" CausesValidation="false"
                        CssClass="btn" />
                </span>
                <input type="button" id="btnExportExcelShow" value="Xuất Excel" class="btn" onclick="javascript:return checkValid(2);" />
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='Stt' HeaderText="STT" DataFormatString="{0:n0}">
                        <ItemStyle Width='5%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='TieuChi' HeaderText="Tiêu chí thống kê" >
                        <ItemStyle Width='35%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='BienBan' HeaderText="Kết quả Thanh tra" DataFormatString="{0:n2}" >
                        <ItemStyle Width='20%' HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField='Phieu' HeaderText='Kết quả tự kiểm tra' DataFormatString="{0:n2}">
                        <ItemStyle Width='20%' HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField='Chung' HeaderText="Kết quả Chung" DataFormatString="{0:n2}">
                        <ItemStyle Width='20%' HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle Height="26px" HorizontalAlign="Center" CssClass="GridHeader" />
                <RowStyle CssClass="GridRow" Height="23px" />
                <AlternatingRowStyle CssClass="GridAlterRow" />
                <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
                <PagerSettings Mode="Numeric" PageButtonCount="2" />
                <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
            </asp:GridView>
            <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
