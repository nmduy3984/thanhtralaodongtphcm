<%@ Control Language="VB" AutoEventWireup="false" CodeFile="KeThuaPKT_XLP.ascx.vb" Inherits="Control_CauHoi_KeThuaPKT_XLP" %>
<script language="javascript">
    function ajaxJquery() {
        $(function () {
            $("#<%=btnSave.ClientId %>").click(function () {
                var iSQD = $("#<%=ddlSoQuyetDinh.ClientId %>").val();
                if (iSQD * 1 == 0) {
                    Alertbox("Vui lòng chọn Số quyết định.");
                    return false;
                }
                else {
                    return true;
                }
            });
        });
    }

</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="HeadTitle">
            <h3>
                KẾ THỪA</h3>
        </div>
        <div id="pnlSearch" class="SubjectTitle">
            <div id="search">
                <div class="BoxField">
                    <asp:Label ID="Label5" runat="server" Text="Chọn số quyết định:" class="SubjectTitle"
                        Style="width: 25%; text-align: right; float: left" />
                    <asp:DropDownList ID="ddlSoQuyetDinh" runat="server" AutoPostBack="false" Width="225px"
                        CssClass="input_form">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div style="clear: both; height: 30px;">
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
            AllowPaging="false" Width='70%' BorderWidth="1px" CssClass="GridBorder">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="CheckAll" runat="server" Checked="true" Onclick="CheckAll(this);"
                            Width="30px" />
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItem" Checked="true" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTT" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField='TenDoanhNghiep' HeaderText='Tên lĩnh vực hoạt động'>
                    <ItemStyle Width='50%' HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div style='width: 100%; clear: both;'>
        </div>
        <div class="DivTextBox">
            <asp:Button ID="btnSave" runat="server" Text=" Kế Thừa " CssClass="btn" />
            <asp:Button ID="btnBack" runat="server" Text=" Quay lại " CssClass="btn" CausesValidation="false" />
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:HiddenField ID="hidPhieuIdNew" Value="0" runat="server" />

        </div>
    </ContentTemplate>
</asp:UpdatePanel>
