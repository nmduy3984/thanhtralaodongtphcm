<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_QuyetDinhThanhTra_List" %>
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
    function CheckAll() {
        if ($("#<%= grdShow.ClientID %> tr th span").find("input[type=checkbox]").attr("checked") == true) {
            if ($("#<%=grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('disabled') != true) {

                $("#<%=grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('checked', true);
            }
        } else {

            $("#<%=grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('checked', false);
        }
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

                $("#" + ObjectIdClick + "").click();
            }
        }
    }
    function getDSDN(SoQD) {
        LoadDN(SoQD);        
      }
     function LoadDN(SoQD) {
         $.ajax({
             type: "POST", //Phuong thuc truyen du lieu luon la POST
             url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSDNTheoSoQD")%>',
             data: "{'SoQD': '" + SoQD + "'}", //thong so truyen vao
             dataType: "json", //kieu du lieu tra ve
             contentType: "application/json; charset=utf-8",
             success: function (data) {
                 var iheightdialog = 100;
                 $("Table#ListDoanhNghiep .GridRow").remove();
                 if (data.d.length > 0) {
                     for (i = 0; i < data.d.length; i++) {
                         iheightdialog = iheightdialog + 50;
                         var tr = "";
                         tr += "<tr style='height: 23px;' class='GridRow'>";
                         tr += "<td align='left' style='width: 3%;' class='text_list_links'><span class='TextLabel' >" + (i + 1) + "</span></td>";
                         tr += "<td align='left' style='width: 15%;' class='text_list_links'><a style='color:#003780;' href='../../Page/DoanhNghiepTT/Edit.aspx?DNId=" + data.d[i].DoanhNghiepId + "' >" + data.d[i].TenDoanhNghiep + "</a></td>";
                         tr += "<td align='left' style='width: 15%;' class='text_list_links'><span class='TextLabel'>" + data.d[i].DiaChi + "</span></td>";
                         tr += "<td style='width: 4%;'>" + data.d[i].ThoiGianLamViec + "</td>";
                         tr += "</tr>";
                         $("#ListDoanhNghiep").append(tr);
                     }
                     if (iheightdialog > 400)
                         iheightdialog = 400;
                 }
                 else {
                     var tr = "";
                     tr += "<tr style='height: 23px;' class='GridRow'>";
                     tr += "<td align='left' style='width: 3%;' colspan='4' class='text_list_links'><span class='TextLabel' >Không có dữ liệu</span></td>";
                     tr += "</tr>";
                     $("#ListDoanhNghiep").append(tr);
                     return false;
                 }
                 $("#RenderDialog").dialog({
                     resizable: false,
                     height: iheightdialog,
                     width: 600,
                     modal: true,
                     buttons: {
                         Ok: function () {
                             $(this).dialog("close");
                         }
                     }
                 });
                 return true;
             },
             error: function (XMLHttpRequest, textStatus, errorThrown) {
                 //Alertbox(textStatus);
             }
         });
     }
</script>
<asp:UpdatePanel ID="uplQuyetdinhthanhtra" runat="server">
    <ContentTemplate>
        <div class="BoxField">
        </div>
        <div class="HeadTitle">
            <h3>
                QUẢN LÝ QUYẾT ĐỊNH THANH TRA</h3>
        </div>
        <%--phần thông tin tìm kiếm--%>
        <legend>
            <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
            <a id="B1" href="#">Tìm kiếm</a>&nbsp;&nbsp; </legend>
        <div id="pnlSearch" class="SubjectTitle">
            <div id="search">
                <div class="BoxField">
                    <asp:Label ID="lblMaDoanhNghiepp" runat="server" Text="Số Quyết định:" class="SubjectTitle"
                        Style="width: 25%; text-align: right; float: left" />
                    <asp:TextBox ID="txtTitleFilter" CssClass="TextBox" runat="server" Text="" MaxLength="100" />
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label2" runat="server" Text="Từ năm:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:DropDownList runat="server" ID="ddlFromDate" CssClass="DropDownList" Width="94px">
                    </asp:DropDownList>
                    <asp:Label ID="Label4" runat="server" Text="đến năm" />
                    <asp:DropDownList runat="server" ID="ddlToDate" CssClass="DropDownList" Width="94px">
                    </asp:DropDownList>
                </div>
                <div class="BoxField">
                    <asp:Label ID="Label5" runat="server" Text="Trạng thái:" class="SubjectTitle" Style="width: 25%;
                        text-align: right; float: left" />
                    <asp:DropDownList runat="server" ID="ddlTrangThai" CssClass="DropDownList">
                        <asp:ListItem Value="">--- Chọn ---</asp:ListItem>
                        <asp:ListItem Value="1">Chưa thanh tra</asp:ListItem>
                        <asp:ListItem Value="0">Đã thanh tra</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="BoxField">
                    <div style="float: left; width: 200px; height: 20px; text-align: right;" class="SubjectTitle">
                    </div>
                    <div class="DivLabel" style="padding-left: 114px; width: 300px;">
                        <asp:Button ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Hủy" />
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
                        <asp:CheckBox ID="CheckAll" runat="server" onclick="CheckAll();" Width="30px" />
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItem" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplEdit" runat="server" Text="Sửa"></asp:HyperLink>&nbsp;|&nbsp;
                        <asp:LinkButton ID="lnkbtnDelete" Text="Xóa" runat="server" OnClick="lnkbtnDelete_Click"
                            CausesValidation='False'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="STT">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Số Quyết định'>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplSoQuyetDinh" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width='25%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số Doanh Nghiệp">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtnSoQD" Text="" runat="server" CausesValidation='False'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="text_list_links" HorizontalAlign="Left" Width="15%" />
                </asp:TemplateField>
                <asp:BoundField DataField='TenTinh' HeaderText='Tỉnh'>
                    <ItemStyle Width='15%' />
                </asp:BoundField>
                <asp:BoundField DataField='NgayTao' HeaderText='Ngày Tạo' DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                    <ItemStyle Width='15%' />
                </asp:BoundField>
                <asp:BoundField DataField='NguoiTao' HeaderText='Người Tạo'>
                    <ItemStyle Width='15%' />
                </asp:BoundField>
            </Columns>
            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div id="RenderDialog" style="display: none;" title="DANH SÁCH DOANH NGHIỆP THANH TRA">
            <div>
                <table cellspacing="0" style="border-width: 1px; border-style: solid; width: 98%;
                    border-collapse: collapse;" id="ListDoanhNghiep"
                    rules="all" class="GridBorder" >
                    <tbody>
                        <tr align="left" style="height: 26px;" class="GridHeader">
                            <th scope="col">
                                STT
                            </th>
                            <th scope="col">
                                Tên Doanh Nghiệp
                            </th>
                            <th scope="col">
                                Địa chỉ
                            </th>
                            <th scope="col">
                                Thời Gian
                            </th>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div>
            <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Xóa" />
            <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
            <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px"
                CssClass="input_form">
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="100" Value="100" Selected="True"></asp:ListItem>
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
            <asp:HiddenField ID="hidSoQuyetDinh" Value="0" runat="server" />
            <asp:HiddenField ID="hidUserName" Value="" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
