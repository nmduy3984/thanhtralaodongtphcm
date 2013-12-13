<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BienBanViPham.ascx.vb"
    Inherits="Control_CauHoi_BienBanViPham" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }
            //Lấy HanhviId khi pageload
            var lstHVID = "#";
            $("#<%=grdShow.ClientId %> tr:not(:has(th))").find(".clHanhViID").each(function () {
                lstHVID += $(this).text() + "#";
            });
            $("#<%=hidHanhViID.ClientID %>").val(lstHVID);

            //Tính tổng số tiền phạt
            SumTienPhat();
            $("input").live("keyup", function () {
                SumTienPhat();
            });
            //Xóa row trong gridview hanhvi
            $(".lnkDelete").click(function () {
                var valRemove = $(this).parent().parent().find(".clHanhViID").text();
                var lstHVID = $("#<%=hidHanhViID.ClientID %>").val();
                $("#<%=hidHanhViID.ClientID %>").val(lstHVID.replace('#' + valRemove + '#', "#"));
                var lnkDelete = $(this).parent().parent();
                lnkDelete.remove();
                var count = 1;
                $(".STTClass").each(function () {
                    $(this).text(count);
                    count++;
                })
                SumTienPhat();
            });

        });
    }
    //Tính tông số tiền phạt
    function SumTienPhat() {
        var sum = 0;
        $("#<%=grdShow.ClientId %> tr:not(:has(th))").find("input[type='text']").each(function () {
            sum += $(this).val().replace(/,/g,'') * 1;
        });
        $(".SumClass").html(FormatsNumber(sum.toString()));
    }
    //Lấy tập giá trị HanhViId từ popup chọn hành vi
    function SetHanhViID(hanhViID) {
        $("#<%=hidHanhViID.ClientID %>").val(hanhViID);
        $("#<%=btnSubmit.ClientID %>").click();
    }
    //Lấy tập giá trị HanhViId trong grdshow
    function GetHanhVis() {
        return $("#<%=hidHanhViID.ClientID()%>").val();
    }
    function CheckValid() {
        return true;
    }
</script>
<style type="text/css">
    .lnkDelete, .lnkDelete:hover
    {
        color: Blue;
        cursor: pointer;
    }
    .lnkDelete:hover
    {
        text-decoration: underline;
    }
</style>
<asp:UpdatePanel ID="uplCauhoi1" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="BIÊN BẢN VI PHẠM HÀNH CHÍNH" />
                </h3>
                <asp:Label ID="Label12" CssClass="TextLabel" runat="server" Style="color: Red;" Text="Yêu cầu kiểm tra nội dung và cập nhật văn bản trích dẫn trước khi in." />
            </div>
            <div class="SubjectTitle">
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="lblcqbh" CssClass="TextLabel" runat="server" Text="Đoàn thanh tra:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtCQBH" runat="server" CssClass="TextBox SmallTextarea" Height="55px"
                            TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="lblPV" runat="server" Text="Thời gian, địa điểm:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtPV" runat="server" CssClass="TextBox SmallTextarea" Height="50px"
                            TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label5" runat="server" Text="Người đại diện hợp pháp:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtNguoiDaiDienHopPhap" runat="server" CssClass="TextBox " Width="200px" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label9" runat="server" Text="Người lập biên bản:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtNguoiLapBB" runat="server" CssClass="TextBox " Width="200px" />
                    </div>
                    <div class="DivLabel">
                        <asp:Label ID="Label10" runat="server" Text="Chức vụ:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtChucVuNguoiLapBB" runat="server" CssClass="TextBox " Width="200px" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label6" runat="server" Text="Người chứng kiến:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtNguoiChungKien" runat="server" CssClass="TextBox " Width="200px" />
                    </div>
                    <div class="DivLabel">
                        <asp:Label ID="Label8" runat="server" Text="Chức vụ:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtChucVuNguoiChungKien" runat="server" CssClass="TextBox " Width="200px" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label11" runat="server" Text="Địa chỉ thanh tra sở:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtDiaChiTT" runat="server" CssClass="TextBox " Width="400px" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel" style="width: 146px">
                        <asp:HyperLink ID="hpKienNghi" runat="server" Text="Chọn hành vi" />
                    </div>
                    <div style="float: left; width: 80%;">
                        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                            CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width='100%'>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="lnkDelete">
                                            Xóa</div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="STT">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSTT" CssClass="STTClass" Text='' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="HanhViId" HeaderStyle-CssClass="NonDisplay">
                                    <ItemStyle Width='1px' CssClass="NonDisplay clHanhViID" />
                                </asp:BoundField>
                                <asp:BoundField DataField='Title' HeaderText='Mô tả'>
                                    <ItemStyle />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText='Mức phạt đề nghị'>
                                    <ItemTemplate>
                                        <cc1:ValidationTextBox ID="txtMucPhatDeNghi" runat="server" CssClass='TextBox' Width="100px"
                                            AssociatedLableText="Mức phạt đề nghị" DataType="requiredAndInteger"></cc1:ValidationTextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width='6%' CssClass='text_list_links' HorizontalAlign='Left' />
                                </asp:TemplateField>                                
                            </Columns>                           
                            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
                            <RowStyle CssClass="GridRow" Height="23px" />
                            <AlternatingRowStyle CssClass="GridAlterRow" />
                            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
                            <PagerSettings Mode="Numeric" PageButtonCount="2" />
                            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label1" runat="server" Text="Ý kiến của người vi phạm:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtYKienNguoiViPham" runat="server" CssClass="TextBox SmallTextarea"
                            Height="50px" TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label2" runat="server" Text="Ý kiến của người làm chứng:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtYKienNguoiLamChung" runat="server" CssClass="TextBox SmallTextarea"
                            Height="50px" TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label4" runat="server" Text="Ý kiến của người đại diện/ tổ chức bị hại:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtYKienNguoiBiHai" runat="server" CssClass="TextBox SmallTextarea"
                            Height="50px" TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label7" runat="server" Text="Các biện pháp ngăn chặn vi phạm hành chính được áp dụng gồm:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtBienPhap" runat="server" CssClass="TextBox LargeTextarea" TextMode="MultiLine" />
                    </div>
                </div>
            </div>
            <div style="clear: both; width: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivTextBox">
                    <div style="text-align: center">
                        <asp:Button ID="btnExport" runat="server" Text=" In biên bản " CssClass="btn" CausesValidation="true"
                            CommandArgument="" OnClientClick="javascript:return CheckValid();" />
                    </div>
                    <div style="float: right; text-align: right">
                        &nbsp;<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            OnClientClick="javascript: history.go(-1);" ImageUrl="~/images/back.png" /></div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:HiddenField ID="hidHanhViID" Value="0" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
