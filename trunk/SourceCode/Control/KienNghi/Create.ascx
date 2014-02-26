<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_KienNghi_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();
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
    function CheckVaild() {
        if (validateFormInputs()) { 
            var txtMota = $("#<%=txtMota.ClientId %>");
            if (txtMota.val().length < 2) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Nội dung kiến nghị không được rỗng.", txtMota);
                txtMota.select();
                return false;
            }
            var checkedItem = $("#<% =trvMenu.ClientId() %> input:checked").size();
            if (checkedItem == 0) {
                Alertbox("Bạn chưa chọn mục vi phạm nào.\nHãy chọn ít nhất là 1 mục.");
                return false;
            }
            return true;
        }        
        return false;
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
<asp:UpdatePanel ID="uplDanhmuchanhvi" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="THÊM KIẾN NGHỊ MỚI" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMota" CssClass="TextLabel" runat="server" Text="Nội dung:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtMota" runat="server" MaxLength="3000" CssClass='TextBox LargeTextarea'
                        Height="40px" TextMode="MultiLine" Width="100px"></asp:TextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblLoaihanhvi" CssClass="TextLabel" runat="server" Text="Loại hành vi:" /><span class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                   
                    <cc1:ValidationDropdownlist ID="ddlLoaihanhvi" runat="server" 
                        CssClass='DropDownList' AssociatedLableText="loại hành vi" 
                        DataType="required" >
                    </cc1:ValidationDropdownlist>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMucvipham" CssClass="TextLabel" runat="server" Text="Mục vi phạm:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox" style="padding-left: 168px">
                    <asp:TreeView ID="trvMenu" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                        ShowLines="True" CssClass="tree">
                    </asp:TreeView>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <div style="float: left">
                        <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" OnClientClick="javascript:return CheckVaild();" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Làm lại" /></div>
                    <div style="float: right; text-align: right">
                        &nbsp;<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" /></div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
