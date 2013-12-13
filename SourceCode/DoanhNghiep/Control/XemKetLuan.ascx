<%@ Control Language="VB" AutoEventWireup="false" CodeFile="XemKetLuan.ascx.vb" Inherits="Control_CauHoi_XemKetLuan" %>
<asp:UpdatePanel ID="uplCauhoi1" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="XEM KẾT QUẢ" />
                </h3>
            </div>
            <div class="SubjectTitle">
                <%-- Danh sách các kiến nghị đối vs doanh nghiệp--%>
                <div class="DivRow">
                    <div class="DivLabel" style="width: 146px">
                        <%--<asp:HyperLink ID="hpKienNghi" runat="server" Text="Chọn kiến nghị" />--%>
                        <asp:Label ID="Label5" runat="server" Text="Kết quả kiểm tra:" />
                    </div>
                    <%--  bảng thể hiện thông tin--%>
                    <div style="float: left; width: 80%;">
                        <asp:GridView ID="grdShowVP" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                            CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width="100%" TabIndex="11">
                            <Columns>
                                <%--Cột mã DoanhNghiep--%>
                                <asp:TemplateField HeaderText="STT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSTT" CssClass="STTClass" runat="server" Text='' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                </asp:TemplateField>
                                <%--Nội dung Kiến nghị--%>
                                <asp:BoundField DataField="NDKetLuan" HeaderText="Những việc đã thực hiện">
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
                    <div class="DivLabel" style="width: 146px">
                    </div>
                    <%--  bảng thể hiện thông tin--%>
                    <div style="float: left; width: 80%; padding-left: 164px;">
                        <asp:GridView ID="grdshowKVP" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                            CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width="100%" TabIndex="11">
                            <Columns>
                                <%--Cột mã DoanhNghiep--%>
                                <asp:TemplateField HeaderText="STT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSTT" CssClass="STTClass" runat="server" Text='' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                </asp:TemplateField>
                                <%--Nội dung Kiến nghị--%>
                                <asp:BoundField DataField="NDKetLuan" HeaderText="Những việc chưa thực hiện">
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
                    <%--<div class="DivRow">
                        <div class="DivLabel">
                            &nbsp;&nbsp;&nbsp;</div>
                        <div class="DivTextBox">
                            <div style="float: left">
                                <asp:Button ID="btnInPhieu" runat="server" Text=" In Biên Bản " CssClass="btn" />&nbsp;<asp:Button ID="btnInPhieuKienNghi" runat="server" Text=" In Phiếu " CssClass="btn" />
                            </div>
                            <div style="float: right; text-align: right">
                                &nbsp;<asp:ImageButton ID="btnBack" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/back.png"
                                    ToolTip="Quay lại" OnClientClick="javascript: history.go(-1);" /></div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
