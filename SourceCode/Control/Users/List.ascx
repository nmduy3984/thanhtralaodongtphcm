<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_Users_List" %>
<script type='text/javascript'>

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
    function ajaxJquery() {
        $(function () {
            $("#<%=txtLastLogin.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%=txtCreatedFrom.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%=txtCreatedTo.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $(".SubjectTitle input, .SubjectTitle select").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnFilter.ClientID()%>").click();
                    return false;
                }
            });
            //check cookie to show or hide search pannel
            if ($.cookie('CookieSSMSearch') == 1) {
                $('#pnlSearch').show();
            }
            else {
                $('#pnlSearch').hide();
            }


            $('#B1').click(function (event) {

                //set cookie
                if ($.cookie('CookieSSMSearch') == 1) {
                    $.cookie('CookieSSMSearch', 0)
                }
                else {
                    $.cookie('CookieSSMSearch', 1)
                }
                $('#pnlSearch').slideToggle(500);
                if ($('#imgShowHide').attr("src") == "../../images/arrow_popup.gif") {

                    $('#imgShowHide').attr("src", "../../images/arrow_showless.gif");
                }
                else {
                    $('#imgShowHide').attr("src", "../../images/arrow_popup.gif");
                }
            });
        });
    }

</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="SubjectTitle">
            <div class="HeadTitle">
                <h3>
                    Quản lý người dùng</h3>
            </div>
        </div>
        <div class="HeadTitle">
            <div class="Error">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="#FF3300" />
            </div>
        </div>
        <div class="SubjectTitle" id="SInfor" runat="server">
            <legend class="SubjectTitle">
                <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
                <a id="B1" href="#">Tìm kiếm </a>&nbsp;&nbsp;</legend>
        </div>
        <div id="pnlSearch" class="SubjectTitle">
            <div style="float: left; width: 200px; height: 20px; text-align: right; padding-bottom: 5px;"
                class="SubjectTitle">
                Tên đăng nhập:</div>
            <div style="width: 500px; height: 20px; padding-bottom: 5px;">
                <asp:TextBox ID="txtTitleFilter" runat="server" Width="207px" CssClass="TextBox"></asp:TextBox>
            </div>
            <div style="clear: both; height: 5px;">
            </div>
            <div style="float: left; width: 200px; height: 20px; text-align: right; padding-bottom: 5px;"
                class="SubjectTitle">
                Loại người dùng:</div>
            <div style="width: 500px; height: 20px; padding-bottom: 5px;">
                <asp:DropDownList ID="ddlTypeUser" runat="server" Width="215px" CssClass="TextBox" />
            </div>
            <div style="clear: both; height: 5px;">
            </div>
            <div style="float: left; width: 200px; height: 20px; text-align: right; padding-bottom: 5px;"
                class="SubjectTitle">
                Ngày đăng nhập cuối:</div>
            <div style="width: 800px; height: 20px; padding-bottom: 5px;">
                <asp:TextBox ID="txtLastLogin" runat="server" Width="207px" CssClass="TextBox"></asp:TextBox>(dd/mm/yyyyy)
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtLastLogin"
                    Display="None" ErrorMessage="Ngày đăng nhập cuối không đúng định dạng" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
            </div>
            <div style="clear: both; height: 5px;">
            </div>
            <div style="float: left; width: 200px; height: 20px; text-align: right; padding-bottom: 5px;"
                class="SubjectTitle">
                Ngày tạo:</div>
            <div style="width: 800px; height: 20px; padding-bottom: 5px;">
                <asp:TextBox ID="txtCreatedFrom" runat="server" CssClass="TextBox" Width="80px"></asp:TextBox>đến:<asp:TextBox
                    ID="txtCreatedTo" runat="server" CssClass="TextBox" Width="90px"></asp:TextBox>(dd/mm/yyyyy)
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtCreatedFrom"
                    Display="None" ErrorMessage="Ngày tạo đầu không đúng định dạng" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCreatedTo"
                    Display="None" ErrorMessage="Ngày tạo sau không đúng định dạng" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
            </div>
            <div style="clear: both; height: 5px;">
            </div>
            <div style="float: left; width: 200px; height: 20px; text-align: right;" class="SubjectTitle">
            </div>
            <div style="width: 500px; height: 20px;">
                <asp:Button ID="btnFilter" runat="server" Text='Lọc' CssClass='btn' />
                &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="btn"
                    Text="Hủy" />
            </div>
            <div style="clear: both; height: 10px;">
            </div>
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
            Width='100%' BorderWidth="1px" CssClass="GridBorder">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="CheckAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                            Width="30px" />
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
                        <asp:Label runat="server" ID="lblSTT" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText='Tên đăng nhập'>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplUsername" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width='12%' CssClass='text_list_links' HorizontalAlign='Left' />
                </asp:TemplateField>
                <asp:BoundField DataField='FullName' HeaderText='Họ tên'>
                    <ItemStyle Width='15%' />
                </asp:BoundField>
                <asp:BoundField DataField='LastLogin' HeaderText='Đăng nhập lần cuối' DataFormatString="{0:dd/MM/yyyy HH:mm}">
                    <ItemStyle Width='15%' />
                </asp:BoundField>
                <asp:TemplateField HeaderText='Loại người dùng'>
                    <ItemTemplate>
                        <asp:Label ID="lblAccountType" runat="server" Text="" />
                    </ItemTemplate>
                    <ItemStyle Width='13%' />
                </asp:TemplateField>
                <asp:BoundField DataField='Created' HeaderText='Ngày tạo' DataFormatString="{0:dd/MM/yyyy HH:mm}">
                    <ItemStyle Width='15%' />
                </asp:BoundField>
                <%--<asp:BoundField DataField='IsActivated' HeaderText='Kích hoạt'>
                    <ItemStyle Width='15%' />
                </asp:BoundField>--%>
                <asp:TemplateField HeaderText='Kích hoạt'>
                    <ItemTemplate>
                        <asp:Label ID="lblIsActivated" runat="server" Text="" />
                    </ItemTemplate>
                    <ItemStyle Width='15%' />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        <div style="padding-top: 5px;">
            <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Xóa" />
            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" PostBackUrl="~/Page/Users/List.aspx"
                Text="Xuất Excel" CausesValidation="false" Visible="false"></asp:Button>
            <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
            <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px"
                CssClass="input_form">
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="100" Value="100"></asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
        <div style="width: 80%">
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
            <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
