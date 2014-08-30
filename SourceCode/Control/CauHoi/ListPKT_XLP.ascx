<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListPKT_XLP.ascx.vb" Inherits="Control_CauHoi_ListPKT_XLP" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
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
    function confirmMultiKeThua(strID) {
        var grdShow = document.getElementById("<%=grdShow.ClientID %>");
        var count = 0;
        for (i = 1; i < grdShow.children.item(0).children.length; i++) {
            if (grdShow.children.item(0).children.item(i).children.item(0).children.item(0).checked) {
                count += 1;
            }
        }
        if (count > 0) {
            return ComfirmDialog('<%=drpMessage.Items(4).Text%>', 0, strID, 0, 0);
        } else {
            Alertbox('<%=drpMessage.Items(2).Text%>');
            return false;
        }
    }
    function ComfirmDialog(strText, value, ObjectIdClick, ID, Type) {
        $("#<%=hidIndex.CLientID %>").val(ID);
        if (value == 0) {
            Confirmbox(strText, value, ObjectIdClick, ID, Type);
            return false;
        } else {
            if (Type == 1) {
                document.getElementById('<%=hidID.ClientID %>').value = ID;
                eval($("#" + ObjectIdClick + "").attr("href"));
            }
            else {

                $("#" + ObjectIdClick + "").click();
            }
        }
    }
    function setIndex(index) {

        $("#<%=hidIndex.CLientID %>").val(index);
    }
        
</script>
<asp:UpdatePanel ID="uplPhieunhapheader" runat="server">
    <ContentTemplate>
        <div class="box_thongke_PTKT">
            <asp:Label ID="lblThongKe" runat="server" Text="" />
        </div>
        <div class="HeadTitle">
            <h3>
                DANH SÁCH PHIẾU TỰ KIỂM TRA</h3>
        </div>
        <%--phần thông tin tìm kiếm--%>
        <div style="clear: both; height: 5px;">
        </div>
        <legend>
            <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
            <a id="B1" href="#">Tìm kiếm</a>&nbsp;&nbsp; </legend>
        <div id="pnlSearch" class="SubjectTitle">
            <div id="search">
                <%--<div class="DivRow">
                    <asp:Label ID="lblMaDoanhNghiepp" runat="server" Text="Tên phiếu:" />
                    <asp:TextBox ID="txtTitleFilter" CssClass="TextBox" runat="server" Text="" MaxLength="100" /><asp:Button
                        ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />&nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="btn"
                        Text="Hủy" />
                </div>--%>
                <div class="BoxField">
                    <asp:Label ID="Label1" runat="server" Text="Mã/Tên phiếu:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:TextBox ID="txtTitleFilter" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label2" runat="server" Text="Người tạo:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:TextBox ID="txtNguoiTao" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label3" runat="server" Text="Ngày tạo: từ" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:TextBox ID="txtFromDate" CssClass="TextBox Date" runat="server" Text="" />
                    <asp:Label ID="Label4" runat="server" Text="đến" />
                    <asp:TextBox ID="txtToDate" CssClass="TextBox Date" runat="server" Text="" />
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label5" runat="server" Text="Trạng thái:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:DropDownList runat="server" ID="ddlTrangThai" CssClass="DropDownList">
                        <asp:ListItem Value="0">--- Chọn ---</asp:ListItem>
                        <asp:ListItem Value="1">Hoàn thành</asp:ListItem>
                        <asp:ListItem Value="2">Chưa hoàn thành</asp:ListItem>
                    </asp:DropDownList>
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
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="CheckAll" runat="server" onclick="CheckAll(this);" Width="30px" />
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItem" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" Text="Sửa" runat="server" OnClick="btnSubmit_Click"
                            CausesValidation='False'></asp:LinkButton>&nbsp;|&nbsp;
                        <asp:LinkButton ID="lnkbtnDelete" Text="Xóa" runat="server" OnClick="lnkbtnDelete_Click"
                            CausesValidation='False'></asp:LinkButton>&nbsp;|&nbsp;<asp:LinkButton ID="lnkTaoPhieu" 
                            Text="Kế thừa" runat="server" 
                            CausesValidation='False' onclick="lnkTaoPhieu_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="13%" />
                </asp:TemplateField>
                <asp:BoundField DataField='RowNum' HeaderText='Stt'>
                    <ItemStyle Width='3%' />
                </asp:BoundField>
                <asp:TemplateField HeaderText='Tên phiếu'>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkTenphieu" runat="server" OnClick="btnTenPhieu_Click" CausesValidation='False'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width='34%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='IsHoanThanh' HeaderText='Hoàn Thành'>
                    <ItemStyle Width='10%' />
                </asp:BoundField>
                <asp:BoundField DataField='NgayTao' HeaderText='Ngày tạo' DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                    <ItemStyle Width='15%' />
                </asp:BoundField>
                <asp:BoundField DataField='NguoiTao' HeaderText='Người tạo'>
                    <ItemStyle Width='7%' />
                </asp:BoundField>
                <asp:TemplateField HeaderText="In văn bản kiến nghị">
                    <ItemTemplate>
                        <asp:HyperLink ID="hplPKN" Text="Phiếu kiến nghị" runat="server" Target="_blank" ToolTip="In phiếu kiến nghị doanh nghiệp"
                            CausesValidation='False' />
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="15%" />
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
            <asp:Button ID="btnKeThua" CssClass="btn" runat="server" Text="Kế thừa" />
            <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
            <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px"
                CssClass="input_form">
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="100" Value="100"  Selected="True"></asp:ListItem>
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
                <asp:ListItem Text='Bạn có chắc chắn muốn kế thừa bản ghi này không?' Value='3'></asp:ListItem>
                <asp:ListItem Text='Bạn có chắc chắn muốn kế thừa bản ghi đã chọn?' Value='4'></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:Button ID="btnSubmit" Text="Click" runat="server" />
            <asp:Button ID="btnTenPhieu" Text="Click" runat="server" />
            <asp:Button ID="btnBBTT" Text="Click" runat="server" />
            <asp:Button ID="btnKLTT" Text="Click" runat="server" />
            <asp:HiddenField ID="hidIndex" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
            <asp:HiddenField ID="hidUserId" Value="0" runat="server" />
            <asp:HiddenField ID="hidUserName" Value="" runat="server" />
            <asp:HiddenField ID="hidPhieuIdNew" Value="0" runat="server" />
            <asp:HiddenField ID="hidDNId" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>