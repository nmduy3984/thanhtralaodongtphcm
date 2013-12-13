<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BaoCaoThucHien.ascx.vb"
    Inherits="Control_CauHoi_BaoCaoThucHien" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {
            $(".Disable").attr("disabled", true);
            $('.Date').datepicker({ dateFormat: 'dd/mm/yy' });
        });
    }
    function CheckValid() {
        var count = 0;
        var strIsThucHien = "";

        $("#<%=grdShow.ClientId() %> tr:not(:has(th))").find("input:checked").each(function () {
            count += 1;
            var ID = $(this).parent().parent().find(".clKienNghiID").text().trim();
            strIsThucHien += ID + ",";
        });
        $("#<%=hidIsThucHien.ClientId %>").val(strIsThucHien);
        $("#<%=hidCountDaThucHienKN.ClientId %>").val(count);
        var txtTienPhatDuKien = $("#<%=txtTienPhatDuKien.ClientId %>");
        var txtTienDaPhat = $("#<%=txtTienDaPhat.ClientId %>");
        if (txtTienDaPhat.val().replace(/,/gi, '') * 1 > txtTienPhatDuKien.val().replace(/,/gi, '') * 1) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số tiền nộp phạt phải nhỏ hơn hoặc bằng số tiền phạt.", txtTienDaPhat);
            txtTienDaPhat.select();
            return false;
        }
        return validateFormInputs();
    }
</script>
<asp:UpdatePanel ID="uplDoanhnghiep" runat="server">
    <ContentTemplate>
        <div class="BoxField" id="edit">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="BÁO CÁO THỰC HIỆN" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="DivRow">
                <div class="DivLabel" style="width: 166px">
                    <asp:HyperLink ID="hpKienNghi" runat="server" Text="Chọn kiến nghị đã thực hiện" />
                </div>
                <div style="float: left; width: 80%;">
                    <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                        CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width="100%" TabIndex="11">
                        <Columns>                            
                            <asp:TemplateField HeaderText="STT">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1%>
                                </ItemTemplate>
                                <ItemStyle Width="3%" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="KienNghiId" HeaderText="Kiến nghị ID" HeaderStyle-CssClass="NonDisplay">
                                <ItemStyle  CssClass="NonDisplay clKienNghiID" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NDKienNghi" HeaderText="Các kiến nghị đối với doanh nghiệp">
                                <ItemStyle Width="" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl" runat="server" Text="Đã thực hiện"></asp:Label><br>
                                    <asp:CheckBox ID="CheckAll" runat="server"  ToolTip="Đã thực hiện" onclick="CheckAll(this);"   />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridHeader" Height="26px" HorizontalAlign="Left" />
                        <RowStyle CssClass="GridRow" Height="23px" />
                        <AlternatingRowStyle CssClass="GridAlterRow" />
                        <PagerStyle CssClass="GridPaged" Height="23px" HorizontalAlign="Right" />
                        <PagerSettings Mode="Numeric" PageButtonCount="2" />
                        <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTienPhatDuKien" CssClass="TextLabel" runat="server" Text="Tiền phạt:" />
                </div>
                <cc2:ValidationTextBox ID="txtTienPhatDuKien" runat="server" CssClass='TextBox Disable'
                    AssociatedLableText="Số tiền nộp phạt" />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblTienDaPhat" CssClass="TextLabel" runat="server" Text="Tiền nộp phạt:" />
                </div>
                <cc2:ValidationTextBox ID="txtTienDaPhat" runat="server" CssClass='TextBox' AssociatedLableText="Số tiền nộp phạt"
                    DataType="Integer"   />
            </div>
            <div class="DivRow">
            </div>
            <div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="lblNgayNopPhat" CssClass="TextLabel" runat="server" Text="Ngày nộp phạt:" />
                </div>
                <cc2:ValidationTextBox ID="txtNgayNopPhat" runat="server" 
                    CssClass='TextBox Date' AssociatedLableText="đúng định dạng dd/mm/yyyy"
                    DataType="none" CompareData="Date" CustomPattern="ddMMyyyy" />
            </div>
            <%--<div class="Div-Left">
                <div class="DivLabelDoanhNghiep">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Ngày nộp phạt:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <cc2:ValidationTextBox ID="txtNgayNopPhat" runat="server" CssClass='TextBox Date'
                    AssociatedLableText="Ngày nộp phạt" DataType="required" />
                (dd/mm/yyyy)
            </div>--%>
            <div class="DivRow">
            </div>
            <div class="DivRow">
                <div class="DivLabelDoanhNghiep">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <div style="float: left; padding-top: 10px;">
                        <asp:Button ID="btnSave" runat="server" Text="&nbsp;&nbsp;&nbsp;Lưu&nbsp;&nbsp;&nbsp;" CssClass="btn" CausesValidation="true"
                            CommandArgument="" OnClientClick="javascript:return CheckValid();" Style="height: 26px" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Làm lại" OnClientClick="javascript:return resetForm();" /></div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsHasPermission" Value="0" runat="server" />
            <asp:HiddenField ID="hidCountDaThucHienKN" Value="0" runat="server" />
            <asp:HiddenField ID="hidCountKN" Value="0" runat="server" />
            <asp:HiddenField ID="hidPhieuId" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsThucHien" Value="" runat="server" />
            <!-- 0: Không có quyền; 1: có quyền sửa-->
            <asp:TextBox ID="txtIDLoaihinhSX" runat="server"></asp:TextBox>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
