<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CauHoiKienNghi.ascx.vb"
    Inherits="Control_CauHoiKienNghi_CauHoiKienNghi" %>
<script type="text/javascript">

    function ajaxJquery() {

        $(function () {
            $("#<% =trvMenu.ClientId() %> input").click(function () {
                var id = $(this).attr('id');
                CheckOrUnCheck(id, $(this).attr("checked"))

            });
            //Click vao nhung link trong trvMenu co class chua Content_trvMenu
            $("#<% =trvMenu.ClientId() %> [class*=_trvMenu_]").click(function () {
                //Lay id cua link vua click
                var id = $(this).attr('id');
                //Lay Id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = id.replace("trvMenut", "trvMenun");
                sameCheckBoxId = sameCheckBoxId + "CheckBox";

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)
                if ($("#" + sameCheckBoxId).attr("checked") == true) {
                    $("#" + sameCheckBoxId).attr("checked", false);
                }
                else {

                    $("#" + sameCheckBoxId).attr("checked", true);
                }

                CheckOrUnCheck(id, $("#" + sameCheckBoxId).attr("checked"));
                return false;
            });
        });

        function CheckOrUnCheck(id, status) {
            var newId;
            //Neu click vao checkbox
            if (id.indexOf("CheckBox") != -1) {

                newId = id.replace("CheckBox", "Nodes");
            }
            else { //Click vao link
                newId = id.replace("trvMenut", "trvMenun");
                newId = newId + "Nodes";
            }
            $("#" + newId).attr("checked", status);
            //check or uncheck his child
            $("#" + newId + " input").each(function () {
                $(this).attr("checked", status);
            });
        }

    }
    function ClientValidate(source, arguments) {
        var treeView = document.getElementById("<%= trvMenu.ClientID %>");
        var checkBoxes = treeView.getElementsByTagName("input");
        var checkedCount = 0;
        for (var i = 0; i < checkBoxes.length; i++) {
            if (checkBoxes[i].checked) {
                checkedCount++;
            }
        }
        if (checkedCount > 0) {
            arguments.IsValid = true;
        } else {
            arguments.IsValid = false;
        }
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <%  If Session("IsUser") <> 2 Then
            
        %>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    KIẾN NGHỊ - CÂU HỎI</h3>
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </div>
            <div style="float: left; margin-right: 30px">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Danh mục kiến nghị:" /><span
                        class="fieldRequired">&nbsp;</span><asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                            runat="server" ControlToValidate="ddlDMKNID" Display="None" ErrorMessage="Vui lòng chọn một kiến nghị."
                            InitialValue="0">*</asp:RequiredFieldValidator>
                </div>
                <div class="DivTextBox">
                    <asp:ListBox ID="ddlDMKNID" Height="300px" Width="220px" runat="server" CssClass="TextBox"
                        AutoPostBack="True"></asp:ListBox>
                </div>
                <div style="margin-top: 10px">
                    <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" />
                    <asp:Button ID="btnHuy" CausesValidation="false" runat="server" Text=" Hủy " CssClass="btn" />&nbsp;
                </div>
            </div>
            <div style="float: left">
                <div class="DivLabelKN">
                    <asp:Label ID="lblAuditnumber" CssClass="TextLabel" runat="server" Text="Câu hỏi:" /><span
                        class="fieldRequired">&nbsp;</span>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ClientValidate"
                        ErrorMessage="Vui lòng chọn ít nhất một câu hỏi." Display="None" OnServerValidate="CustomValidator1_ServerValidate">*</asp:CustomValidator>
                </div>
                <div class="DivTextBox">
                    <asp:TreeView ID="trvMenu" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                        ShowLines="True" CssClass="tree">
                    </asp:TreeView>
                </div>
            </div>
            <div style="clear: both">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
        </div>
        <%End If%>
    </ContentTemplate>
</asp:UpdatePanel>
