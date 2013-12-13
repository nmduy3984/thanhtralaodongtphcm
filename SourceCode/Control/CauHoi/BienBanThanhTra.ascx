<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BienBanThanhTra.ascx.vb"
    Inherits="Control_CauHoi_BienBanThanhTra_BienBanThanhTra" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }
            $("#<%=txtCoQuanBanHanh.ClientId %>").live("keyup", function () {
                var hidTTBo = $("#<%=hidTTBo.ClientId %>");
                var hidDoanTT = $("#<%=hidDoanTT.ClientId %>");
                var lines = $(this).val().split(/\n/);
                hidTTBo.val(lines[0]);
                hidDoanTT.val(lines[1]);
            });
        });
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
<asp:UpdatePanel ID="uplKetluan" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="NHẬP THÔNG TIN BIÊN BẢN" /></h3>
                <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Style="color: Red;" Text="Trưởng Đoàn phải kiểm tra kỹ nội dung biên bản trước khi in." />
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblTencotcauhoi" CssClass="TextLabel" runat="server" Text="Cơ quan ban hành:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtCoQuanBanHanh" runat="server" CssClass='TextBox SmallTextarea'
                        Height="50px" TextMode="MultiLine" DataType="required" AssociatedLableText="cơ quan ban hành"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblNdketluan" CssClass="TextLabel" runat="server" Text="Phạm vi thanh tra:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtPhamVi" runat="server" CssClass='TextBox SmallTextarea' Height="50px"
                        TextMode="MultiLine" DataType="required" AssociatedLableText="phạm vi thanh tra"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblIsthuchien" CssClass="TextLabel" runat="server" Text="Quyết định thanh tra:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtQDThanhTra" runat="server" CssClass='TextBox SmallTextarea' Height="50px"
                        TextMode="MultiLine" DataType="required" AssociatedLableText="quyết định thanh tra"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Thành phần tham gia:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtDoanThanhTra" runat="server" CssClass='TextBox SmallTextarea'
                        Height="70px" DataType="required" TextMode="MultiLine" AssociatedLableText="thành phần tham gia"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblTenbangcauhoi" CssClass="TextLabel" runat="server" Text="Đại diện doanh nghiệp:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtDaiDienDN" runat="server" CssClass='TextBox SmallTextarea' Height="70px"
                        DataType="required" TextMode="MultiLine" AssociatedLableText="đại diện doanh nghiệp"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="Nội dung làm việc:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtNoiDungLamViec" runat="server" CssClass='TextBox LargeTextarea'
                        Height="70px" DataType="required" TextMode="MultiLine" AssociatedLableText="nội dung làm việc"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="Kiến nghị của Đoàn:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtKNDoan" runat="server" CssClass='TextBox LargeTextarea' Height="70px"
                        DataType="required" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Người ký(đại diện doanh nghiệp):" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtNguoiKy" runat="server" CssClass='TextBox' DataType="required"
                        AssociatedLableText="đại diện doanh nghiệp"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Trưởng đoàn thanh tra:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtTruongDoanTT" runat="server" CssClass='TextBox' DataType="required"
                        AssociatedLableText="trưởng đoàn thanh tra"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <div style="float: left">
                        <asp:Button ID="btnInPhieu" runat="server" Text=" In Biên Bản " CssClass="btn" ToolTip="Đề nghị kiểm tra lại thông tin vừa nhập trước khi in biên bản thanh tra" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Làm lại" /></div>
                    <div style="float: right; text-align: right">
                        &nbsp;<asp:ImageButton ID="btnBack" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/back.png"
                            ToolTip="Quay lại" OnClientClick="javascript: history.go(-1);" /></div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:HiddenField ID="hidKienNghiID" Value="0" runat="server" />
            <asp:HiddenField ID="hidUsername" Value="" runat="server" />
            <asp:HiddenField ID="hidTTBo" Value="" runat="server" />
            <asp:HiddenField ID="hidDoanTT" Value="" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
