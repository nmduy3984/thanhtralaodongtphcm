<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhieuKetLuan.ascx.vb"
    Inherits="Control_CauHoi_PhieuKetLuan" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }
            $("#<%=txtCQBH.ClientId %>").live("keyup", function () {
                var hidcqbh1 = $("#<%=hidcqbh1.ClientId %>");
                var hidcqbh2 = $("#<%=hidcqbh2.ClientId %>");
                var lines = $(this).val().split(/\n/);
                hidcqbh1.val(lines[0]);
                hidcqbh2.val(lines[1]);
            });

            $("#<%=ddlChucDanh.ClientID %>").change(function () {
                SetNoiNhan($(this));
            });

            // hiển thị dữ liệu ban đầu cho phần Chức danh
            SetNoiNhan($("#<%=ddlChucDanh.ClientID %> option:first"));

            //Lấy HanhviId khi pageload
            var lstKNID = "#";
            $("#<%=grdShow.ClientId %> tr:not(:has(th))").find(".clKienNghiID").each(function () {
                lstKNID += $(this).text() + "#";
            });
            $("#<%=hidKienNghiID.ClientID %>").val(lstKNID);
            //Xóa row trong gridview hanhvi
            $(".lnkDelete").click(function () {
                var valRemove = $(this).parent().parent().find(".clKienNghiID").text();
                var lstKNID = $("#<%=hidKienNghiID.ClientID %>").val();
                $("#<%=hidKienNghiID.ClientID %>").val(lstKNID.replace('#' + valRemove + '#', "#"));
                var lnkDelete = $(this).parent().parent();
                lnkDelete.remove();
                var count = 1;
                $(".STTClass").each(function () {
                    $(this).text(count);
                    count++;
                })
            });
        });
    }

    function SetNoiNhan(obj) {
        var text = "";
        if ($(obj).val() * 1 == 1) {// Nếu là chánh thanh tra
            text = "Doanh nghiệp (để t/h);"
            if ($("#<%= hidIsThanhTra.ClientId%>").val() * 1 == 1) {
                text = text + "\nSở LĐTB & XH " + $("#<%=hidTinh.ClientID %>").val() + " (để p/h);"
            }
            text += "\nLưu TTr, Hồ sơ.";
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        } else {
            text = "Doanh nghiệp (để t/h);\nChánh Thanh Tra (để b/c);\nLưu TTr, Hồ sơ."
            $("#<%=txtNoiNhan.ClientID %>").val(text);
        }
    }

    function CheckValid() {
        return true;
    }
    //Lấy tập giá trị HanhViId từ popup chọn hành vi
    function SetKienNghiID(KienNghiID) {
        $("#<%=hidKienNghiID.ClientID %>").val(KienNghiID);
        $("#<%=btnSubmit.ClientID %>").click();
    }
    //Lấy tập giá trị HanhViId trong grdshow
    function GetKienNghis() {
        return $("#<%=hidKienNghiID.ClientID()%>").val();
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
                    <asp:Label ID="Label3" runat="server" Text="PHIẾU KẾT LUẬN" />
                </h3>
                <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Style="color: Red;" Text="Yêu cầu kiểm tra nội dung và cập nhật văn bản trích dẫn trước khi in." />
            </div>
            <div class="SubjectTitle">
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="lblcqbh" CssClass="TextLabel" runat="server" Text="Cơ quan ban hành" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtCQBH" runat="server" CssClass="TextBox SmallTextarea" Height="50px"
                            TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="lblPV" runat="server" Text="Phạm vi:" />
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
                        <asp:Label ID="lblCC" runat="server" Text="Căn cứ:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtCC" runat="server" CssClass="TextBox LargeTextarea" TextMode="MultiLine" />
                    </div>
                </div>
                <div style="clear: both; height: 15px;">
                </div>
                <%-- Danh sách các kiến nghị đối vs doanh nghiệp--%>
                <div class="DivRow">
                    <div class="DivLabel" style="width: 146px">
                        <%--<asp:HyperLink ID="hpKienNghi" runat="server" Text="Chọn kiến nghị" />--%>
                        <asp:Label ID="Label5" runat="server" Text="Danh sách kiến nghị:" />
                    </div>
                    <%--  bảng thể hiện thông tin--%>
                    <div style="float: left; width: 80%;">
                        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                            CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width="100%" TabIndex="11">
                            <Columns>
                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="lnkDelete">
                                            Xóa</div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="3%" />
                                </asp:TemplateField>--%>
                                <%--Cột mã DoanhNghiep--%>
                                <asp:TemplateField HeaderText="STT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSTT" CssClass="STTClass" runat="server" Text='' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="KienNghiId" HeaderText="Kiến nghị ID" HeaderStyle-CssClass="NonDisplay">
                                    <ItemStyle Width='95%' CssClass="NonDisplay clKienNghiID" />
                                </asp:BoundField>
                                <%--Nội dung Kiến nghị--%>
                                <asp:BoundField DataField="NDKienNghi" HeaderText="Nội dung kiến nghị">
                                    <ItemStyle Width="95%" />
                                </asp:BoundField>
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
                    <div class="DivLabel">
                        <asp:Label ID="Label1" runat="server" Text="Chức danh:" />
                    </div>
                    <div class="fl">
                        <asp:DropDownList ID="ddlChucDanh" runat="server" CssClass="DropDownList">
                            <asp:ListItem Value="1" Text="Chánh Thanh Tra" />
                            <asp:ListItem Value="2" Text="Phó chánh Thanh Tra" />
                        </asp:DropDownList>
                    </div>
                    <div class="fl" style="margin: 0 20px 0 15px; font-weight: bold; color: #000000;">
                        <asp:Label ID="Label4" runat="server" Text="Nơi nhận: " />
                    </div>
                    <div>
                        <asp:TextBox ID="txtNoiNhan" CssClass="TextBox textarea1" runat="server" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="Label2" runat="server" Text="Người ký:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="txtNguoiKy" CssClass="TextBox" runat="server" Text="" />
                    </div>
                </div>
            </div>
            <div style="clear: both; width: 15px;">
            </div>
            <div class="DivRow">
                <div class="DivTextBox">
                    <div style="text-align: center">
                        <asp:Button ID="btnExport" runat="server" Text=" In Kết Luận " CssClass="btn" CausesValidation="true"
                            ToolTip="Trưởng đoàn phải kiểm tra kỷ nội dung và văn bản trích dẫn trước khi in, chịu trách nhiệm kết luận của mình"
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
            <asp:HiddenField ID="hidKienNghiID" Value="0" runat="server" />
            <asp:HiddenField ID="hidTinh" Value="0" runat="server" />
            <asp:HiddenField ID="hidUsername" Value="" runat="server" />
            <asp:HiddenField ID="hidcqbh1" Value="" runat="server" />
            <asp:HiddenField ID="hidcqbh2" Value="" runat="server" />
            <asp:HiddenField ID="hidCanCu" Value="" runat="server" />
            <asp:HiddenField ID="hidNDCanCu" Value="" runat="server" />
            <asp:HiddenField ID="hidIsThanhTra" Value="1" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
