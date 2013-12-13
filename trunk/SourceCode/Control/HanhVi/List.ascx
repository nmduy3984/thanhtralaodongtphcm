<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_HanhVi_List" %>
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
</script>
<asp:UpdatePanel ID="uplDanhmuchanhvi" runat="server">
    <ContentTemplate>
        <div class="BoxField">
        </div>
        <div class="HeadTitle">
            <h3>
                QUẢN LÝ HÀNH VI</h3>
        </div>
        <legend>
            <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
            <a id="B1" href="#">Tìm kiếm</a>&nbsp;&nbsp; </legend>
        <div id="pnlSearch" class="SubjectTitle">
            <div id="search">
                <div class="DivRow">
                    <asp:Label ID="lblMaDoanhNghiepp" runat="server" Text="Mô tả:" />
                    <asp:TextBox ID="txtTitleFilter" runat="server" CssClass='TextBox'></asp:TextBox><asp:Button
                        ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />&nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="btn"
                        Text="Hủy" />
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
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stt">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Mô tả'>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplMota" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width='40%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Mức phạt thấp nhất'>
                    <ItemTemplate>
                        <asp:Label ID="lblMucPhatMin" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width='10%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Mức phạt cao nhất'>
                    <ItemTemplate>
                        <asp:Label ID="lblMucPhatMax" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width='10%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Loại hành vi'>
                    <ItemTemplate>
                        <asp:Label ID="lblLoaiHanhVi" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width='18%' CssClass='text_list_links' HorizontalAlign='Left' />
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
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
