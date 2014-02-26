<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PopUpDoanhNghiep.ascx.vb"
    Inherits="Control_DoanhNghiep_PopUpDoanhNghiep" %>
    <script type='text/javascript'>
        function ajaxJquery() {
            $(function () {
                //Selected from parent

                var strSoQD = window.parent.getSoQuyetDinh();
                alert(strSoQD);
                
            });
        }
       
</script>
<asp:UpdatePanel ID="uplPromotion" runat="server">
    <ContentTemplate>
        <div style='padding-top: 10px; text-align:center;'>
            <h3>
                DANH SÁCH DOANH NGHIỆP THANH TRA</h3>
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
            CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width="98%" >
            <Columns>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <asp:Label ID="lblStt" CssClass="TextLabel" runat="server" Text="" />
                    </ItemTemplate>
                    <ItemStyle CssClass="text_list_links" HorizontalAlign="Left" Width="3%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên Doanh Nghiệp">
                    <ItemTemplate>
                        <asp:HyperLink ID="hplTenDN" runat="server" />
                    </ItemTemplate>
                    <ItemStyle CssClass="text_list_links" HorizontalAlign="Left" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Địa chỉ">
                    <ItemTemplate>
                        <asp:Label ID="lblDiaChi" CssClass="TextLabel" runat="server" Text="" />
                    </ItemTemplate>
                    <ItemStyle CssClass="text_list_links" HorizontalAlign="Left" Width="15%" />
                </asp:TemplateField>
                <asp:BoundField DataField="ThoiGianLamViec" DataFormatString="{0:dd/MM/yyyy}"
                    HeaderText="Thời Gian">
                    <ItemStyle Width="4%" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle CssClass="GridHeader" Height="26px" HorizontalAlign="Left" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" Height="23px" HorizontalAlign="Right" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div>
            &nbsp;&nbsp;&nbsp;<asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
            <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" CssClass="input_form"
                Width="70px">
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="100" Value="100" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblTotal" runat="server"></asp:Label>
        </div>
        <div style="width: 100%">
            <!--phan trang-->
            <div id="results_resultsNavigationDiv" class="paginationEMP">
                <ul>
                    <asp:LinkButton ID="lnkFirst" runat="server"></asp:LinkButton>
                    <asp:Repeater ID="rptPage" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkTitle" runat="server" CssClass="page_num" OnClick="lnkTitle_Click"
                                CausesValidation="false">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:LinkButton ID="lnkLast" runat="server"></asp:LinkButton>
                </ul>
            </div>
            <asp:HiddenField ID="hidCount" runat="server" Value="0" />
            <asp:HiddenField ID="hidIndex_page" runat="server" Value="0" />
            <asp:HiddenField ID="hidCur_Page" runat="server" Value="1" />
        </div>
        <div style="display: none">
            <asp:DropDownList ID="drpMessage" runat="server">
                <asp:ListItem Text="Bạn có chắc chắn muốn xóa bản ghi này không?" Value="0"></asp:ListItem>
                <asp:ListItem Text="Bạn có chắc chắn muốn xóa bản đã chọn?" Value="1"></asp:ListItem>
                <asp:ListItem Text="Bạn phải chọn ít nhất một bản ghi." Value="2"></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField ID="hidID" runat="server" Value="0" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
