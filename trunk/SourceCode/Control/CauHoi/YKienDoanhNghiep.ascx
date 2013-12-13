<%@ Control Language="VB" AutoEventWireup="false" CodeFile="YKienDoanhNghiep.ascx.vb" Inherits="Control_CauHoi_YKienDoanhNghiep" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
           visibleContain($("#<%=chkQ11.ClientID%> input:radio:checked"));
            $(".TextBox:first").focus();
            $(".Child input:radio").click(function () {
                visibleContain($(this));
            });
        });

        function visibleContain(e) {
            if ($(e).is(':checked') && $(e).val() * 1 == 1) { // Nếu checkbox có được check
                $('.checkCo').hide('slow');
                $('.checkKhong').show('slow');
            } else {
                $('.checkCo').show('slow');
                $('.checkKhong').hide('slow');
                //$("#<%=txtYKienDN.ClientID %>").val("- ");
            }
        }

        function CheckValid() {
            // Nếu Check có thì kiểm tra xem người dùng có nhập ý kiến chưa ? 
            if ($("#<%=chkQ11.ClientID%> input:radio:first-child").val() * 1 == 1) {
                return $("#<%=txtYKienDN.ClientID %>").val().trim() != "";
            }
            return true;
        }
    }
</script>
<asp:UpdatePanel ID="uplCauhoi1" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="Ý KIẾN CỦA DOANH NGHIỆP" /></h3>
            </div>
            <div class="">
                <div class="DivRow">
                    <div class="fl fl1">
                        <asp:Label ID="lblTendoanhnghiep" CssClass="TextLabel" runat="server" Text="Doanh nghiệp có ý kiến " />
                    </div>
                    <div class="Child" style="margin-left:200px">
                        <asp:RadioButtonList ID="chkQ11" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text=" Có" Selected="True" style="margin-right:50px;"/> 
                            <asp:ListItem Value="0" Text=" Không"/>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="checkCo fl1">
                    <b> Doanh nghiệp nhất trí với nội dung biên bản nêu trên </b>               
                </div>
                <div class="checkKhong fl1" style="display:none" >
                    <b> Danh sách kiến nghị của doanh nghiệp với cơ quan quản lý nhà nước về lao động</b><!-- (mỗi ý kiến là 1 dòng bắt đầu bằng ký tự '-')</br>-->
                    <asp:TextBox ID="txtYKienDN" runat="server" CssClass="textbox" 
                        style="width:100%; height:250px;margin-top:10px;max-height:300px; max-width:100%; min-width:100%; min-height:100px" TextMode="MultiLine" />
                </div>
            </div>
        </div>
        <div style="clear:both; width:15px;" >

                </div>
        <div class="DivRow">
            <div class="DivTextBox">
                <div style="text-align:center">
                    <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" CausesValidation="true"
                        CommandArgument="" OnClientClick="javascript:return CheckValid();"/>
                </div>
                <div style="float: right; text-align: right">
                    &nbsp;<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" OnClientClick="javascript: history.go(-1);" runat="server"
                        ImageUrl="~/images/back.png" /></div>
            </div>
        </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>