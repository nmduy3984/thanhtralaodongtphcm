<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListPhieuKienNghi.ascx.vb"
    Inherits="DoanhNghiep_Control_ListPhieuKienNghi" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
            //check cookie to show or hide search pannel            

        });
    }
     
    function setIndex(index) {

        $("#<%=hidIndex.CLientID %>").val(index);
    }
        
</script>
<asp:UpdatePanel ID="uplPhieunhapheader1" runat="server">
    <ContentTemplate>
        <div class="HeadTitle">
            <h3>
                DANH SÁCH PHIẾU KIẾN NGHỊ</h3>
        </div>
        <%--phần thông tin tìm kiếm--%>
        
        <div class="DivRow">
            &nbsp;</div>
        <div class="DivRow">
            <asp:Label ID="lblMaDoanhNghiepp" runat="server" Text="Tìm phiếu:" />
            <asp:TextBox ID="txtTitleFilter" CssClass="TextBox" runat="server" Text="" MaxLength="100" /><asp:Button
                ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
            AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
            <Columns>
                
                <asp:TemplateField HeaderText="Stt">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Tên phiếu'>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkTenphieu" runat="server" OnClick="btnTenPhieu_Click" CausesValidation='False'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width='35%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên Cty">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDoanhNghiep" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mục trả lời">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCauHoiDaTraLoi" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Ngày tạo'>
                    <ItemTemplate>
                        <asp:Label ID="lblNgayTao" runat="server" Text="" />
                    </ItemTemplate>
                    <ItemStyle Width='15%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='NguoiTao' HeaderText='Người tạo'>
                    <ItemStyle Width='10%' />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplBBTT" Text="Kiến nghị" runat="server" ToolTip="Xem kiến nghị"
                            CausesValidation='False' />
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div>
            <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Xóa" />
            <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
            <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px"
                CssClass="input_form">
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="100" Value="100"></asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
        <div style="width: 100%">
            <!--phan trang-->
            <div id="results_resultsNavigationDiv" class="paginationEMP">
                <ul>
                    <asp:LinkButton ID="lnkFirst" runat="server"></asp:LinkButton>
                    <asp:Repeater ID="rptPage" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkTitle" CssClass="page_num" runat="server" OnClick="lnkTitle_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:LinkButton ID="lnkLast" runat="server"></asp:LinkButton>
                </ul>
            </div>
            <asp:HiddenField ID="hidCount" Value="0" runat="server" />
            <asp:HiddenField ID="hidIndex_page" Value="0" runat="server" />
            <asp:HiddenField ID="hidCur_Page" Value="1" runat="server" />
        </div>
        <div style='display: none'>
            <asp:DropDownList ID="drpMessage" runat="server">
                <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản ghi này không?' Value='0'></asp:ListItem>
                <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản đã chọn?' Value='1'></asp:ListItem>
                <asp:ListItem Text='Bạn phải chọn ít nhất một bản ghi.' Value='2'></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:Button ID="btnSubmit" Text="Click" runat="server" />
            <asp:Button ID="btnTenPhieu" Text="Click" runat="server" />
            <asp:Button ID="btnBBTT" Text="Click" runat="server" />
            <asp:Button ID="btnKLTT" Text="Click" runat="server" />
            <asp:HiddenField ID="hidIndex" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
