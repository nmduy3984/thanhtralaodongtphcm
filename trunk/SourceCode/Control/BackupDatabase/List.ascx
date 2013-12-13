<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_BackupDatabase_List" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
            $(".Date").datepicker({ dateFormat: 'dd/mm/yy' });
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
        $("#<%=hidIndex.CLientID %>").val(ID); //Lấy vị trí index tron grdshow
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
<asp:UpdatePanel ID="uplPromotion" runat="server">
    <ContentTemplate>
        <div class="HeadTitle">
            <h3>
                QUẢN LÝ DANH SÁCH FILE BACKUP DỮ LIỆU</h3>
        </div>
        <div style="clear: both; height: 30px;">
        </div>
        <div>
            <asp:Button ID="bntBackup" CssClass="btn" runat="server" Text="Backup database" />
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
                         
                        <asp:LinkButton ID="lnkbtnDelete" Text="Xóa" runat="server" OnClick="lnkbtnDelete_Click"
                            CausesValidation='False'></asp:LinkButton> 
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="5%" />
                </asp:TemplateField>
                <asp:BoundField DataField='Id' HeaderText='STT'>
                    <ItemStyle Width='3%' HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText='Tên tập tin'>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplFileName" runat="server"  CausesValidation='False'></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width='32%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='Size' HeaderText='Kích thước(Byte)' DataFormatString="{0:n0}">
                    <ItemStyle Width='7%' HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField='Create' HeaderText='Ngày tạo' DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                    <ItemStyle Width='12%'  HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div>
            <%--<asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Xóa" />--%>
        </div>
        <div style='display: none'>
            <asp:DropDownList ID="drpMessage" runat="server">
                <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản ghi này không?' Value='0'></asp:ListItem>
                <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản đã chọn?' Value='1'></asp:ListItem>
                <asp:ListItem Text='Bạn phải chọn ít nhất một bản ghi.' Value='2'></asp:ListItem>
                <asp:ListItem Text='Bạn có chắc chắn muốn kế thừa bản ghi này không?' Value='3'></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
            <asp:Button ID="btnSubmit" Text="Click" runat="server" />
            <asp:Button ID="btnTenPhieu" Text="Click" runat="server" />
            <asp:HiddenField ID="hidIndex" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
