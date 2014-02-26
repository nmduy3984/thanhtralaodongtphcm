<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DSDNChuaKhamSucKhoeDinhKy.ascx.vb"
    Inherits="Control_TraCuu_DSDNChuaKhamSucKhoeDinhKy" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
    }
</script>
<asp:UpdatePanel ID="uplDailyDebt" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblReportName" runat="server" Text="Danh sách doanh nghiệp chưa khám sức khỏe định kỳ" />
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
            <div id="SearchCondition" class="Div-Center">
                <div class="DivRow">
                    <div class="DivLabel" style="width: 260px">
                        <asp:Label ID="Label3" runat="server" Text=" Địa phương:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlDiaPhuong" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel" style="width: 260px">
                        <asp:Label ID="Label5" runat="server" Text="Năm:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlYearFrom" Width="90px" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow" style="display: none">
                    <div class="DivLabel" style="width: 260px">
                        <asp:Label ID="Label1" runat="server" Text=" Lĩnh vực:" />
                    </div>
                    <div class="DivTextBox">
                        <cc2:ValidationTextBox ID="txtLoaihinhSX" runat="server" CssClass='TextBox'></cc2:ValidationTextBox>
                    </div>
                </div>
                <div style="clear: both; height: 10px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel" style="width: 260px">
                    </div>
                    <div class="DivTextBox" style="padding-left: 280px">
                        <asp:Button ID="btnExport" runat="server" Text="Tìm Kiếm" CausesValidation="true"
                            CssClass="btn" />
                        <asp:Button ID="btnExportExcel" runat="server" Text="Xuất Excel" CausesValidation="false"
                            CssClass="btn" />
                        <asp:Button ID="btnHuy" runat="server" Text="Đặt lại" CausesValidation="false" CssClass="btn" />
                    </div>
                </div>
                <div style="clear: both; height: 10px;">
                </div>
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='RowNum' HeaderText="STT">
                        <ItemStyle Width="4%" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField='TenDoanhNghiep' HeaderText="Tên doanh nghiệp">
                        <ItemStyle Width='20%' />
                        <HeaderStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField='TruSoChinh' HeaderText="Địa chỉ DN">
                        <ItemStyle Width='15%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Email' HeaderText='Email DN'>
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='DienThoai' HeaderText='Điện thoại DN'>
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoNguoiPhaiKham' HeaderText='Số người phải khám'>
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='NguoiLienHe' HeaderText='Người liên hệ'>
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                     <asp:BoundField DataField='EmailLH' HeaderText='Email'>
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='DienThoaiLH' HeaderText='Điện thoại'>
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
                <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
                <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" CssClass="input_form"
                    Width="70px">
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
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
                <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
