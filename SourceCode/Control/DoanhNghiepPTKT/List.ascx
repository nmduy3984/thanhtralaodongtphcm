<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_DoanhNghiep_List" %>
<script type='text/javascript'>

    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();

            // Để check được 1 trong nhiều checkbox
            CheckOnlyAndEffect();

            //check cookie to show or hide search pannel            
            if ($.cookie('CookieSiteSearch') == 1) {
                $('#search').show();
            }
            else {
                $('#search').hide();
            }
            $('#B1').click(function (event) {
                //set cookie
                if ($.cookie('CookieSiteSearch') == 1) {
                    $.cookie('CookieSiteSearch', 0)
                }
                else {
                    $.cookie('CookieSiteSearch', 1)
                }
                $('#search').slideToggle(500);
                if ($('#imgShowHide').attr("src") == "../../images/arrow_popup.gif") {
                    $('#imgShowHide').attr("src", "../../images/arrow_showless.gif");
                }
                else {
                    $('#imgShowHide').attr("src", "../../images/arrow_popup.gif");
                }
            });
        });
    }

    function confirmMultiDelete(strID) {
        var grdShow = document.getElementById("<%=grdShow.ClientID %>");
        var count = 0;
        for (i = 1; i < grdShow.children.item(0).children.length; i++) {
            if (grdShow.children.item(0).children.item(i).children.item(0).children.item(0).checked) {
                count += 1;
            }
        }
        if (count > 0) {
            return ComfirmDialog('<%=drpMessage.Items(1).Text%>', 0, strID, 0, 0);
        } else {
            Alertbox('<%=drpMessage.Items(2).Text%>');
            return false;
        }
    }

    function ComfirmDialog(strText, value, ObjectIdClick, ID, Type) {

        if (value == 0) {
            Confirmbox(strText, value, ObjectIdClick, ID, Type);
            return false;
        } else {
            if (Type == 1) {

                document.getElementById('<%=hidID.ClientID %>').value = ID;
                eval($("#" + ObjectIdClick + "").attr("href"));
            }
            else {
                document.getElementById('<%=hidID.ClientID %>').value = ID;
                $("#" + ObjectIdClick + "").click();
            }
        }
    }

</script>
<asp:UpdatePanel ID="uplPromotion" runat="server">
    <ContentTemplate>
        <div class="HeadTitle">
            <h3>
                QUẢN LÝ DANH SÁCH DOANH NGHIỆP</h3>
        </div>
        <legend>
            <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
            <a id="B1" href="#">Tìm kiếm</a>&nbsp;&nbsp; </legend>
        <div id="pnlsearch" class="SubjectTitle">
            <div id="search">
                <div>
                    <div class="Div-Left">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="lblTenDN" runat="server" Text="Tên doanh nghiệp:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtTenDN" CssClass="TextBox" runat="server" Text="" MaxLength="40" />
                            </div>
                        </div>
                    </div>
                    <div class="Div-Right">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="lblLoaiHinhDN" runat="server" Text="Loại hình kinh tế:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:DropDownList ID="ddlLoaiHinhDN" runat="server" CssClass='DropDownList' />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="Div-Left" style="display:none">
                        <div class="DivRow">
                             <div class="DivLabel">
                                &nbsp;
                            </div>
                            <div class="DivTextBox">
                                &nbsp;
                            </div>
                        </div>
                    </div>
                    <div class="Div-Right">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="lblHuyen" runat="server" Text="Quận/Huyện:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:DropDownList ID="ddlHuyen" runat="server" CssClass='DropDownList' />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="Div-Left">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="lblLHSX" runat="server" Text="Loại hình sản xuất:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:DropDownList ID="ddlLoaiHinhSX" runat="server" CssClass='DropDownList' />
                            </div>
                        </div>
                    </div>
                    <div class="Div-Right">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label2" runat="server" Text="Có công đoàn cơ sở:" />
                            </div>
                            <div class="DivTextBox Child">
                                <asp:RadioButtonList ID="rblCDCS" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="-1" Text=" Tất cả" />
                                    <asp:ListItem Value="1" Text=" Có" style="margin: 0 50px 0 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="Div-Left">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="lblEffectiveSdate" runat="server" Text="Năm thành lập từ:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtTuNam" CssClass="TextBox Date" runat="server" Text="" MaxLength="40" />
                            </div>
                        </div>
                    </div>
                    <div class="Div-Right">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label7" runat="server" Text="Đến năm:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtDenNam" CssClass="TextBox Date" runat="server" Text="" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="Div-Left">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label3" runat="server" Text="Tổng số lao động:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:DropDownList ID="ddlTongSoLaoDong" runat="server" CssClass='DropDownList'>
                                    <asp:ListItem Value="0">--- Chọn ---</asp:ListItem>
                                    <asp:ListItem Value="1">Dưới 50</asp:ListItem>
                                    <asp:ListItem Value="2">50 - 200</asp:ListItem>
                                    <asp:ListItem Value="3">200 - 1000</asp:ListItem>
                                    <asp:ListItem Value="4">Trên 1000</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="Div-Right">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label4" runat="server" Text="Lợi nhuận:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:DropDownList ID="ddlLoiNhuan" runat="server" CssClass='DropDownList'>
                                    <asp:ListItem Value="0">--- Chọn ---</asp:ListItem>
                                    <asp:ListItem Value="1">Dưới 0.05</asp:ListItem>
                                    <asp:ListItem Value="2">0.05 - 1</asp:ListItem>
                                    <asp:ListItem Value="3">1 - 100</asp:ListItem>
                                    <asp:ListItem Value="4">Trên 100</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="Div-Left">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label1" runat="server" Text="Số ĐKKD:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtSoDKKD" CssClass="TextBox Date" runat="server" Text="" MaxLength="40" />
                            </div>
                        </div>
                    </div>
                    <div class="Div-Right">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label5" runat="server" Text="Người tạo:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtNguoiTao" CssClass="TextBox " runat="server" Text="" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="Div-Left">
                        &nbsp;</div>
                    <div class="Div-Right">
                        <asp:Button ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Hủy" />
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both; height: 30px;">
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
            CssClass="GridBorder" EmptyDataText="{Không có dữ liệu}" Width="100%">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="CheckAll" runat="server" onclick="CheckAll(this);" Width="23px" />
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItem" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </asp:TemplateField>
                <%--edit và xóa thông tin--%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplEdit" runat="server" Text="Sửa " />
                        |<asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="False" OnClick="lnkbtnDelete_Click"
                            Text=" Xóa" />
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="7%" />
                </asp:TemplateField>
                <asp:BoundField DataField="RowNum" HeaderText="STT">
                    <ItemStyle Width="3%" />
                </asp:BoundField>
                <%--Cột tên Doanh Nghiep--%>
                <asp:TemplateField HeaderText="Tên Doanh Nghiệp">
                    <ItemTemplate>
                        <asp:HyperLink ID="hplTenDN" runat="server" />
                    </ItemTemplate>
                    <ItemStyle CssClass="text_list_links" HorizontalAlign="Left" Width="19%" />
                </asp:TemplateField>
                <%--Huyện - Tỉnh --%>
                <asp:BoundField DataField="Address" HeaderText="Địa chỉ">
                    <ItemStyle Width="19%" />
                </asp:BoundField>
                <%-- Tỉnh --%>
                <asp:BoundField DataField="TenTinh" HeaderText="Tỉnh" Visible="false">
                    <ItemStyle Width="8%" />
                </asp:BoundField>
                <%--Năm thành lập--%>
                <asp:BoundField DataField="NamTLDN" HeaderText="Năm thành lập">
                    <ItemStyle Width="7%" />
                </asp:BoundField>
                <%--Loại hình  sản xuất--%>
                <asp:BoundField DataField="LHSX" HeaderText="Loại Hình SX">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <%--Tổng số lao động--%>
                <asp:BoundField DataField="TongSoNhanVien" DataFormatString="{0:n0}" HeaderText="Tổng số lao động">
                    <ItemStyle Width="8%" />
                </asp:BoundField>
                <%--Số LĐ có y/c nghiêm ngặt--%>
                <asp:BoundField DataField="SoNguoiLamCongViecYeuCauNghiemNgat" DataFormatString="{0:n0}"
                    HeaderText="Số LĐ có y/c nghiêm ngặt">
                    <ItemStyle Width="8%" />
                </asp:BoundField>
                <%--Số LĐ làm nghề độc hại--%>
                <asp:BoundField DataField="SoNguoiLamNgheNguyHiem" DataFormatString="{0:n0}" HeaderText="Số LĐ làm nghề độc hại">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <%--Xem hồ sơ doanh nghiệp--%>
                <asp:TemplateField HeaderText="Xem hồ sơ DN">
                    <ItemTemplate>
                        <asp:HyperLink ID="hplHoSoDN" runat="server" Text="Xem hồ sơ" />
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridHeader" Height="26px" HorizontalAlign="Left" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" Height="23px" HorizontalAlign="Right" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div>
            <asp:Button ID="btnDelete" runat="server" CssClass="btn" Text="Xóa" />
            <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
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
            <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
