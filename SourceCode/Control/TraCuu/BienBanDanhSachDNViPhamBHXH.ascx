<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BienBanDanhSachDNViPhamBHXH.ascx.vb" Inherits="Control_TraCuu_BienBanDanhSachDNViPhamBHXH" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $("#<%=txtLoaihinhSX.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/SearchAutoComplete")%>',
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
        // Kiem tra lai loai hinh san xuat
        $("#<%= txtLoaihinhSX.ClientID %>").focusout(function () {
            var sName = $(this).val();
            if (sName != "") {
                $.ajax({
                    type: "POST", //Phuong thuc truyen du lieu luon la POST
                    url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/CheckNameLoaiHinhSX")%>',
                    data: "{'strName': '" + sName + "'}", //thong so truyen vao
                    dataType: "", //kieu du lieu tra ve
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        $("#<%= txtIDLoaihinhSX.ClientID %>").val("-1");
                        var resultReturn = eval("(" + result.d + ")");
                        if (resultReturn == "0") {

                            Alertbox("Ngành sản xuất không tồn tại vui lòng chọn tên khác.");
                            $("#<%= txtLoaihinhSX.ClientID %>").val("");
                            $("#<%= txtLoaihinhSX.ClientID %>").focus();
                        } else {
                            $("#<%= txtIDLoaihinhSX.ClientID %>").val(resultReturn);

                        }
                    }, //ham thuc thi neu thanh cong    
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    } //ham thuc thi neu loi xay ra.

                });
            }
        });

    }
</script>
<asp:UpdatePanel ID="uplDailyDebt" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblReportName" runat="server" Text="BIÊN BẢN DANH SÁCH DOANH NGHIỆP VI PHẠM VỀ BẢO HIỂM XÃ HỘI" />
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
                        <asp:Label ID="Label5" runat="server" Text=" Từ năm:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlYearFrom" Width="90px" runat="server" CssClass="DropDownList" />
                        <asp:Label ID="Label7" runat="server" Text="đến năm:" />
                        <asp:DropDownList ID="ddlYearTo" Width="95px" runat="server" CssClass="DropDownList" />
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
                        <asp:Button ID="btnHuy" runat="server" Text="Đặt lại" CausesValidation="false" CssClass="btn" />
                    </div>
                </div>
                <div style="clear: both; height: 10px;">
                </div>
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='STT'  HeaderText="STT">
                        <ItemStyle Width="4%" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col1' HeaderText="Tên doanh nghiệp">
                        <ItemStyle Width='15%' />
                        <HeaderStyle   />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col2' HeaderText="Địa chỉ">
                        <ItemStyle Width='20%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col3' HeaderText='Số LĐ chưa tham gia BHXH'>
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col4' HeaderText="Số tiền chậm nộp">
                        <ItemStyle  HorizontalAlign="Right" Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col5' HeaderText='Số tiền chiếm dụng của người LĐ'>
                        <ItemStyle HorizontalAlign="Right" Width='12%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col6' HeaderText='Số người chưa có sổ BHXH'>
                        <ItemStyle HorizontalAlign="Right" Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col7' HeaderText='Số sổ chưa trả'>
                        <ItemStyle HorizontalAlign="Right" Width='8%' />
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
                    Width="70px" >
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
                        <asp:LinkButton ID="lnkFirst" runat="server" ></asp:LinkButton>
                        <asp:Repeater ID="rptPage" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTitle" runat="server" CssClass="page_num" OnClick="lnkTitle_Click"
                                    CausesValidation="false">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:LinkButton ID="lnkLast" runat="server" ></asp:LinkButton>
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
                <asp:TextBox ID="TextBox1" Text="-1" runat="server"/>
            </div> 
            <div style="display: none">
                <asp:TextBox ID="txtIDLoaihinhSX" Text="-1" runat="server"></asp:TextBox>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
