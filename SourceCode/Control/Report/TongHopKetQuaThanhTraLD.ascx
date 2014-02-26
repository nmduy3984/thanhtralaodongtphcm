<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TongHopKetQuaThanhTraLD.ascx.vb"
    Inherits="Control_Report_TongHopKetQuaThanhTraLD" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            // Handler enter trên input nhập
            $("#SearchCondition input[type=text]").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#btnExportShow").click();
                    return false;
                }
            });
            //Danh sách số quyết định được chọn
            $(".SoQD").live("click", function () {
                var hidSoQD = '';
                $(".SoQD  input[type='checkbox']").each(function () {
                    if ($(this).is(':checked')) {
                        hidSoQD += $(this).val() + '#';
                    }
                });
                $("#<%=hidSoQD.ClientId %>").val(hidSoQD);
            });
        });
    }
    //kiểm tra user nhập
    function checkValid(pos) {
        var error = "";
        var countbug = 0;
        var result = true;
        var ddlNam = $("#<%=ddlNam.ClientId %> ").val();
        if (ddlNam * 1 == 0) {
            Alertbox("Vui lòng chọn năm báo cáo!");
            result = false;
         }
        var hidSoQD = $("#<%=hidSoQD.ClientId %>").val();
        if (ddlNam * 1 > 0 && hidSoQD.trim().length == 0) {
            Alertbox("Vui lòng chọn Số quyết định!");
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
                    <asp:Label ID="lblReportName" runat="server" Text="âaaaaa" />
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
            <div id="SearchCondition" class="Div-Center">
                <div class="DivRow">
                    <div class="DivLabel">
                        <%--<span class="fieldRequired"></span>--%>
                        <asp:Label ID="Label1" runat="server" Text=" Năm " />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlNam" runat="server" AutoPostBack="true" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label2" runat="server" Text="Số quyết định:" />
                    </div>
                    <div class="DivTextBox SoQD" style="height: 150px;width:237px; overflow: auto;">
                        <asp:CheckBoxList runat="server" ID="chkSoQD">
                        </asp:CheckBoxList>
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
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="false" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='STT' HeaderText="Stt">
                        <ItemStyle Width='5%' />
                        
                    </asp:BoundField>
                    <asp:BoundField DataField='SQD' HeaderText="Số QĐ">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='TenTinh' DataFormatString="{0:n0}" HeaderText="Tỉnh/TP">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoDNTT' DataFormatString="{0:n0}" HeaderText="Số doanh nghiệp Thanh tra">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoKN' DataFormatString="{0:n0}" HeaderText="Số kiến nghị" >
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoDNBCTH' DataFormatString="{0:n0}" HeaderText="Số DN báo cáo thực hiện">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoBBVP' DataFormatString="{0:n0}" HeaderText="Số biên bản VP" >
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='DaPhat' DataFormatString="{0:n0}" HeaderText="Đã phạt">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='DaNop' DataFormatString="{0:n0}" HeaderText="Đã nộp">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle Height="26px" HorizontalAlign="Center" CssClass="GridHeader" />
                <RowStyle CssClass="GridRow" Height="23px" />
                <AlternatingRowStyle CssClass="GridAlterRow" />
                <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
                <PagerSettings Mode="Numeric" PageButtonCount="2" />
                <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
            </asp:GridView>
            <div>
            </div>
            <div style="display: none">
                <asp:HiddenField ID="hidSoQD" Value="" runat="server" />
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
