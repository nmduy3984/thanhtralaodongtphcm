<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListPheuTuKiemTra.ascx.vb" Inherits="DoanhNghiep_Control_ListPheuTuKiemTra" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
            //check cookie to show or hide search pannel            
          
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

                $("#" + ObjectIdClick + "").click();
            }
        }
    }
    function setIndex(index) {

        $("#<%=hidIndex.CLientID %>").val(index);
    }
        
</script>

<asp:UpdatePanel ID="uplPhieunhapheader1" runat="server">
    <ContentTemplate>

        <div class="HeadTitle">
            <h3>DANH SÁCH PHIẾU KIỂM TRA</h3>
        </div>
        
        <%--phần thông tin tìm kiếm--%>
       <div class="DivRow">
              <asp:Button ID="btnCreate" runat="server" Text="Tạo mới  Phiếu" CssClass="btn" />
        </div>
        
        <div class="DivRow">&nbsp;</div>
               
        <div class="DivRow">
                 
                <asp:Label ID="lblMaDoanhNghiepp" runat="server" Text="Tìm phiếu:" />

                <asp:TextBox ID="txtTitleFilter" CssClass="TextBox" runat="server" Text="" MaxLength="100"
                    /><asp:Button ID="btnFilter" runat="server" Text='Tìm kiếm' CssClass='btn' />
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
                            CausesValidation='False'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="7%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stt">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Tên phiếu'>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkTenphieu" runat="server" OnClick="btnTenPhieu_Click" CausesValidation='False'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width='20%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên Cty">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDoanhNghiep" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mục trả lời">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCauHoiDaTraLoi" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Ngày tạo'>
                    <ItemTemplate>
                        <asp:Label ID="lblNgayTao" runat="server" Text="" />
                    </ItemTemplate>
                    <ItemStyle Width='15%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='NguoiTao' HeaderText='Người tạo'>
                    <ItemStyle Width='10%' />
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
            <asp:Button ID="btnSubmit" Text="Click" runat="server" />
            <asp:Button ID="btnTenPhieu" Text="Click" runat="server" />
            <asp:Button ID="btnBBTT" Text="Click" runat="server" />
            <asp:Button ID="btnKLTT" Text="Click" runat="server" />
            <asp:HiddenField ID="hidIndex" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
