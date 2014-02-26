<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_OtherList_List" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc2" %>
<script type='text/javascript'>

    function ajaxJquery() {
        $(function () {
            $("#<%= txtTitleFilter.ClientID %>").focus();
            $("#<%= txtTitleFilter.ClientID %>").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnFilter.ClientID()%>").click();
                    return false;
                }
            });
        });
    }
    function CheckAll() {
        if ($("#<%= grdShow.ClientID %> tr th span").find("input[type=checkbox]").attr("checked") == true) {
            if ($("#<%= grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('disabled') != true) {

                $("#<%= grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('checked', true);
            }
        } else {

            $("#<%= grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('checked', false);
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <h3>
                TẠO DANH MỤC
            </h3>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Loại danh mục:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationDropdownlist ID="ddlType" AutoPostBack="true" runat="server" CssClass="DropDownList"
                        AssociatedLableText="loại danh mục" DataType="required">
                    </cc2:ValidationDropdownlist>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblId" CssClass="TextLabel" runat="server" Text="Id:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtId" CssClass="TextBox" runat="server" ReadOnly="true" Text="" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblName" CssClass="TextLabel" runat="server" Text="Tên danh mục:" />
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationTextBox ID="txtName" runat="server" CssClass="TextBox" AssociatedLableText="tên danh mục"
                        DataType="required"></cc1:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <asp:Button ID="btnSave" runat="server" Text=" Lưu " CausesValidation="true" CommandArgument=""
                        OnClientClick="return validateFormInputs();" CssClass="btn" />
                    <asp:Button ID="btnHuy" CausesValidation="false" runat="server" Text="Làm mới" CssClass="btn" />&nbsp;
                </div>
            </div>
        </div>
        <div class="HeadTitle">
            <h3>
                QUẢN LÝ DANH MỤC</h3>
        </div>
        <div class="SubjectTitle">
            <asp:Label ID="Label2" runat="server" Text="Lọc theo loại:"></asp:Label><asp:DropDownList
                runat="server" CssClass="DropDownList" ID="ddlTypeSearch" AutoPostBack="true" />
            <asp:Label ID="Label3" runat="server" Text="Tên danh mục:"></asp:Label>
            <asp:TextBox ID="txtTitleFilter" runat="server" CssClass='TextBox'></asp:TextBox>
            <asp:Button ID="btnFilter" runat="server" Text='Lọc' CausesValidation="false" CssClass='btn' />
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
                <asp:TemplateField HeaderText="Stt">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Mã'>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplId" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width='10%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='Name' HeaderText='Tên'>
                    <ItemStyle Width='25%' />
                </asp:BoundField>
                <asp:TemplateField HeaderText='Loại'>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplType" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width='25%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='Status' HeaderText='Trạng thái'>
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
        </div>
        <div style="display: none">
            <asp:HiddenField ID="HidType" Value="0" runat="server" />
            <asp:HiddenField ID="hidIdEdit" Value="0" runat="server" />
            <asp:HiddenField ID="isEdit" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
