<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhieuKienNghi.ascx.vb"
    Inherits="Control_CauHoi_PhieuKienNghi" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
        });
    }
</script>
<asp:UpdatePanel ID="uplDanhmuckiennghi" runat="server">
    <ContentTemplate>
        <div class="BoxField">
        </div>
        <div class="HeadTitle">
            <h3>
                PHIẾU KIẾN NGHỊ</h3>
        </div>
        <div class="SubjectTitle">
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
            AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
            <Columns>
                <asp:BoundField HeaderText="STT" DataField="RowNum">
                    <ItemStyle Width='5%' />
                </asp:BoundField>
                <asp:TemplateField HeaderText='Nội dung kiến nghị'>
                    <ItemTemplate>
                        <asp:Label ID="lblNoidungkn" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass='text_list_links' HorizontalAlign='Left' />
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
        <div class="DivRow">
            <div class="fl" style="margin: 0 20px 0 15px; font-weight: bold; color: #000000;">
                <asp:Label ID="Label1" runat="server" Text="Nơi nhận: " />
            </div>
            <div>
                <asp:TextBox ID="txtNoiNhan" CssClass="TextBox textarea1" runat="server" TextMode="MultiLine" />
            </div>
        </div>
        <div class="DivRow">
            <div class="DivLabel" style="width: 74px;">
                <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Người ký:" />
            </div>
            <div class="DivTextBox">
                <asp:TextBox ID="txtNguoiKy" runat="server" CssClass='TextBox' DataType="required"
                    AssociatedLableText="người ký"></asp:TextBox>
            </div>
        </div>
        <div class="DivTextBox">
            <div style="float: left; padding-left: 82px">
                <asp:Button ID="btnInPhieuKN" CssClass="btn" runat="server" Text="In Phiếu" /></div>
            <div style="float: right; text-align: right">
                &nbsp;<asp:ImageButton ID="btnBack" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/back.png"
                    ToolTip="Quay lại" OnClientClick="javascript: history.go(-1);" /></div>
        </div>
        <div style='display: none'>
            <asp:DropDownList ID="drpMessage" runat="server">
                <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản ghi này không?' Value='0'></asp:ListItem>
                <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản đã chọn?' Value='1'></asp:ListItem>
                <asp:ListItem Text='Bạn phải chọn ít nhất một bản ghi.' Value='2'></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
