<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListBaoCaoThucHien.ascx.vb"
    Inherits="Control_CauHoi_ListBaoCaoThucHien" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {
            //Popup
            var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (isAsyncPostback) {
                tb_init('a.thickbox, area.thickbox, input.thickbox');
            }

            $(".TextBox:first").focus();
            //Autocompletebynameprovince
            $("#<%=txtTinh.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/SearchAutoProvince")%>',
                        data: "{ 'strSrc': '" + request.term.replace(/&/g, "").replace(/>/g, "").replace(/</g, "").replace(/"/g, "") + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) {
                            return data;
                        },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            //alert(textStatus);
                        }
                    });
                },
                minLength: 3
            });
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
            var hidTinhThanhTraSo = $("#<%=hidTinhThanhTraSo.ClientId %>").val();
            if (hidTinhThanhTraSo > 0)
                $(".TinhTp").css("display", "none");
        });
    }

    //Set index trong grid
    function setIndex(index) {

        $("#<%=hidIndex.CLientID %>").val(index);
    }
    // Gán giá trị Loại hình sản xuất ID từ màn hình Popup
    function SetTenTinh(TenTinh) {
        $("#<%= txtTinh.ClientID %>").val(TenTinh);
    }    
</script>
<asp:UpdatePanel ID="uplBienBanThanhTra" runat="server">
    <ContentTemplate>
        <div class="HeadTitle">
            <h3>
                DANH SÁCH BIÊN BẢN THANH TRA</h3>
        </div>
        <%--phần thông tin tìm kiếm--%>
        <legend>
            <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
            <a id="B1" href="#">Tìm kiếm</a>&nbsp;&nbsp; </legend>
        <div id="pnlSearch" class="SubjectTitle">
            <div id="search">
                <div class="BoxField">
                    <asp:Label ID="lblMaDoanhNghiepp" runat="server" Text="Doanh nghiệp:" class="SubjectTitle"
                        Style="width: 25%; text-align: right; float: left" />
                    <asp:TextBox ID="txtTitleFilter" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                </div>
                <div class="BoxField TinhTp">
                    <asp:Label ID="Label1" runat="server" Text="Tỉnh/Tp:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:TextBox ID="txtTinh" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                    <asp:HyperLink ID="hplChonTinh" runat="server">Chọn</asp:HyperLink>
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label2" runat="server" Text="Năm:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:TextBox ID="txtNam" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label3" runat="server" Text="Số quyết định:" class="SubjectTitle"
                        Style="width: 25%; text-align: right; float: left" />
                    <asp:TextBox ID="txtSoQD" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                </div>
                <div class="BoxField">
                    <div style="float: left; width: 200px; height: 20px; text-align: right;" class="SubjectTitle">
                    </div>
                    <div class="DivLabel" style="padding-left: 114px">
                        <asp:Button ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />&nbsp;<asp:Button
                            ID="btnClear" runat="server" CausesValidation="false" CssClass="btn" Text="Hủy" />
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both; height: 30px;">
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
            AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
            <Columns>
                <asp:BoundField DataField='RowNum' HeaderText='STT'>
                    <ItemStyle Width='3%' HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText='Tên phiếu'>
                    <ItemTemplate>
                        <asp:Label ID="lblTenphieu" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width='30%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên Cty">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDoanhNghiep" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField='IsHoanThanh' HeaderText='Hoàn Thành'>
                    <ItemStyle Width='10%' />
                </asp:BoundField>
                <asp:BoundField DataField='NgayTao' HeaderText='Ngày tạo' DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                    <ItemStyle Width='10%' />
                </asp:BoundField>
                <asp:BoundField DataField='NguoiTao' HeaderText='Người tạo'>
                    <ItemStyle Width='10%' />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplBCTH" Text="Báo cáo thực hiện" runat="server" ToolTip="Báo cáo thực hiện"
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
            <asp:HiddenField ID="hidIndex" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
            <asp:HiddenField ID="hidUserId" Value="0" runat="server" />
            <asp:HiddenField ID="hidUserName" Value="" runat="server" />
             <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
