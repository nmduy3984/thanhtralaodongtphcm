<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi6_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
            // Dành cho phần check chon 1 trong 2 giá trị co hoặc không
            CheckOnlyAndEffect();
            //6.1. Tổng số người thuộc đối tượng tham gia BHXH bắt buộc
            $(".Group1").val(FormatsNumber($(".Group1").val()));
            //$(".Group1").attr('disabled', true);
            //$(".Group1").removeAttr("aspNetDisabled");
            var iSum3HDLDFirst = $("#<%=hidSum3HDLDFirst.ClientID %>").val() * 1;
            if (iSum3HDLDFirst <= 0) {
                $(".Group2").attr('disabled', true).addClass("aspNetDisabled");
                //$(".Group2").removeAttr("aspNetDisabled");
            }

            //6.2. Tổng số người thuộc đối tượng tham gia Bảo hiểm thất nghiệp 
            $(".Group3").val(FormatsNumber($(".Group3").val()));
            //$(".Group3").attr('disabled', true);
            //$(".Group3").removeAttr("aspNetDisabled");
            var iSum2HDLDFirst = $("#<%=hidSum2HDLDFirst.ClientID %>").val() * 1;
            if (iSum2HDLDFirst <= 0) {
                $(".Group4").attr('disabled', true).addClass("aspNetDisabled");

            }

            //6.5. Số sổ BHXH chưa trả cho người thôi việc
            $(".Group5").blur(function () {
                var iVal = $(".Group5").val().replace(/,/g, '') * 1;
                if (iVal > 0) {
                    $(".Group6").removeAttr("disabled").removeClass("aspNetDisabled");
                }
                else {
                    $(".Group6").attr('disabled', true).addClass("aspNetDisabled");
                    //$(".Group6").removeAttr("aspNetDisabled");
                }
            });
            $("#<%=txtQ641.ClientId %>").keypress(function (event) {
                IntegerAndDecimal(event, this, 'false');
            });
            $("#<%=txtQ642.ClientId %>").keypress(function (event) {
                IntegerAndDecimal(event, this, 'false');
            });
            //Kiểm tra nếu mục 6.4.1 có thì mục 6.6 cho nhập
            Check64();
            $("#<%=txtQ641.ClientID %>").keypress(function () {
                Check64();
            });
            //6.7 Nếu chkQ67 check đủ/không thì disable txtQ671. Ngược lại  
            Check67($("#<%=chkQ67.ClientId %> input:checkbox:checked "));
            $("#<%=chkQ67.ClientId %> input:checkbox:checked ").live("click", function () {
                Check67($(this));
            });
        });
        function Check67(e) {
            if ($(e).val() * 1 == 2) {
                $("#<%=txtQ671.ClientId %>").removeAttr("disabled").removeClass("aspNetDisabled");
            }
            else {
                $("#<%=txtQ671.ClientId %>").attr("disabled", true).addClass("aspNetDisabled");
            }
        }
    }

    function Check64() {
        var txtQ641 = $("#<%=txtQ641.ClientID %>");
        if (txtQ641.val().replace(/,/gi, '') * 1 > 0) {
            $(".DivQ66").find("input").removeAttr("disabled").removeClass("aspNetDisabled");
        }
        else {
            $(".DivQ66").find("input").attr("disabled", true).addClass("aspNetDisabled");
        }
    }
    function TestVaild() {
        //Xét ràng buộc dữ liệu        
        var txtQ611 = $("#<%=txtQ611.ClientId %>");
        var txtQ612 = $("#<%=txtQ612.ClientId %>");
        var txtQ621 = $("#<%=txtQ621.ClientId %>");
        var txtQ622 = $("#<%=txtQ622.ClientId %>");
        var txtQ63 = $("#<%=txtQ63.ClientId %>");
        var txtQ642 = $("#<%=txtQ642.ClientId %>");
        var txtQ65 = $("#<%=txtQ65.ClientId %>");

        if (txtQ612.val().replace(/,/gi, '') * 1 > txtQ611.val().replace(/,/gi, '') * 1) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số người đã tham gia BHXH phải nhỏ hơn hoặc bằng tổng số người thuộc đối tượng tham gia BHXH bắt buộc.", txtQ612);
            txtQ612.select();
            return false;
        }

        if (txtQ622.val().replace(/,/gi, '') * 1 > txtQ621.val().replace(/,/gi, '') * 1) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số người đã tham gia bảo hiểm thất nghiệp phải nhỏ hơn hoặc bằng tổng số người thuộc đối tượng tham gia bảo hiểm thất nghiệp.", txtQ622);
            txtQ622.select();
            return false;
        }
        if (txtQ63.val().replace(/,/gi, '') * 1 > txtQ612.val().replace(/,/gi, '') * 1) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số người đã được cấp sổ BHXH phải nhỏ hơn hoặc bằng số người đã tham gia BHXH.", txtQ63);
            txtQ63.select();
            return false;
        }

        if (txtQ65.val().replace(/,/gi, '') * 1 > txtQ63.val().replace(/,/gi, '') * 1) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số sổ BHXH chưa trả cho người thôi việc phải nhỏ hơn hoặc bằng số người đã được cấp sổ BHXH.", txtQ65);
            txtQ65.select();
            return false;
        }
        return true;
    }
</script>
<asp:UpdatePanel ID="uplCauhoi456" runat="server">
    <ContentTemplate>
        <div id="generate">
            <div class="BoxField">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
                </div>
                <div class="DivRequired" style="font-weight: bold">
                    6. Bảo hiểm xã hội, bảo hiểm thất nghiệp
                </div>
                <div style="width: 75%;" class="fl1">
                    <div class="DivQ61">
                        <div class="DivRow width100 fl">
                            <div class=" DivHeader fl ">
                                <asp:Label ID="Label36" CssClass="TextLabel" runat="server" Text="6.1. Tổng số người thuộc đối tượng tham gia BHXH bắt buộc:" />
                            </div>
                            <div class=" Child fr fr1">
                                <cc2:ValidationTextBox ID="txtQ611" runat="server" CssClass='TextBox TextBox_short Group1'
                                    DataType="integer" AssociatedLableText="Đối tượng tham gia BHXH bắt buộc" />(người)
                            </div>
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="- Số người đã tham gia BHXH:" />
                            </div>
                            <div class="fr fr1">
                                <cc2:ValidationTextBox ID="txtQ612" runat="server" CssClass='TextBox TextBox_short Group2 Effect'
                                    DataType="integer" AssociatedLableText="Số người đã tham gia BHXH" CompareData="Number"
                                    CompareOperator="GreaterThan" CompareTo="txtQ611" />(người)
                            </div>
                        </div>
                    </div>
                    <div style="clear: both; height: 15px;">
                    </div>
                    <div class="DivQ62">
                        <div class="DivRow width100 fl">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label37" CssClass="TextLabel" runat="server" Text="6.2. Tổng số người thuộc đối tượng tham gia bảo hiểm thất nghiệp" />
                            </div>
                            <div class="fr fr1">
                                <cc2:ValidationTextBox ID="txtQ621" runat="server" CssClass='TextBox TextBox_short Group3'
                                    DataType="integer" AssociatedLableText="Nhập số" />(người)
                            </div>
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="- Số người đã tham gia bảo hiểm thất nghiệp" />
                            </div>
                            <div class="fr fr1">
                                <cc2:ValidationTextBox ID="txtQ622" runat="server" CssClass='TextBox TextBox_short Group4 Effect'
                                    DataType="integer" AssociatedLableText="Số người đã tham gia bảo hiểm thất nghiệp" />(người)
                            </div>
                        </div>
                    </div>
                    <div style="clear: both; height: 15px;">
                    </div>
                    <div class="DivQ63">
                        <div class="DivRow width100 fl">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label38" CssClass="TextLabel" runat="server" Text="6.3. Số người đã được cấp sổ BHXH:" />
                            </div>
                            <div class="fr fr1">
                                <cc2:ValidationTextBox ID="txtQ63" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" AssociatedLableText="Số người đã được cấp sổ BHXH" />(người)
                            </div>
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="- Số người ký HĐLĐ trên 01 tháng chưa làm thủ tục cấp sổ BHXH" />
                            </div>
                            <div class="fr fr1">
                                <cc2:ValidationTextBox ID="txtQ631" runat="server" CssClass='TextBox TextBox_short Group4 Effect'
                                    DataType="integer" />(người)
                            </div>
                        </div>
                    </div>
                    <div style="clear: both; height: 15px;">
                    </div>
                    <div class="DivQ64">
                        <div class="DivRow">
                            <div class="fl DivHeader" style="font-weight: bold">
                                <asp:Label ID="Label39" CssClass="TextLabel" runat="server" Text="6.4. Tổng số tiền chậm đóng/ số phải đóng hàng tháng:" />
                            </div>
                            <div class="fr fr1">
                                <%--<cc2:ValidationTextBox ID="txtQ641" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" AssociatedLableText="Số tiền chậm đóng" CompareData="string"
                                CompareOperator="None" CompareTo="" />--%>
                                <asp:TextBox ID="txtQ641" runat="server" CssClass='TextBox TextBox_short'></asp:TextBox>
                                /
                                <%--<cc2:ValidationTextBox ID="txtQ642" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" AssociatedLableText="Số phải đóng hàng tháng" CompareData="Number"
                                CompareOperator="LessThan" CompareTo="txtQ641" 
                                Style="margin-right: 7px;" />--%>
                                <cc2:ValidationTextBox ID="txtQ642" runat="server" CssClass='TextBox TextBox_short ToolTip'
                                    DataType="integer" AssociatedLableText="Chỉ nhập số tiền làm tròn hàng triệu" />(triệu)&nbsp;&nbsp;
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ65 width100 fl">
                            <div class="DivRow">
                                <div class="Child DivHeader fl">
                                    <asp:Label ID="Label40" CssClass="TextLabel" runat="server" Text="6.5. Số sổ BHXH chưa trả cho người thôi việc" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ65" runat="server" CssClass='TextBox TextBox_short Group5'
                                        DataType="integer" AssociatedLableText="Số sổ BHXH" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label41" CssClass="TextLabel" runat="server" Text="Lí do chưa trả sổ" />
                                </div>
                                <div class="fr fr1" style="margin-right: 41px">
                                    <asp:TextBox ID="txtQ651" runat="server" CssClass='TextBox Group6 Effect textarea1'
                                        Width="420px" TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ66">
                            <div class="DivRow width100 fl">
                                <div class="fl DivHeader">
                                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="6.6. Số tiền khấu trừ lương của người lao động chưa nộp cho bảo hiểm xã hội" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ66" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Số tiền khấu trừ lương" />(triệu)&nbsp;&nbsp;
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ67">
                            <div class="DivRow DivHeader fl">
                                <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="6.7. Trả tiền bảo hiểm xã hội vào lương và ngày nghỉ phép:" />
                            </div>
                            <div class="DiwRow Child fr fr1" style="margin-right: 10px; position: relative;">
                                <asp:CheckBoxList ID="chkQ67" CssClass="checkbox" runat="server" RepeatDirection="Horizontal"
                                    Width="258px">
                                    <asp:ListItem Value="1" Text=" Đủ" style="margin-right: 50x;" />
                                    <asp:ListItem Value="2" Text=" Chưa đủ" />
                                    <asp:ListItem Value="0" Text=" Không trả" />
                                </asp:CheckBoxList>
                                <%--<asp:CheckBoxList ID="chkQ67" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Đủ" style="margin-right: 237px;" />
                                    <asp:ListItem Value="0" Text=" Không trả" />
                                </asp:CheckBoxList>--%>
                                <%--<div class="DivRow- " style="position: absolute; top: -1px; left: 78px">
                                    <div class="fr fr1">
                                        thiếu
                                        <cc2:ValidationTextBox ID="txtQ671" runat="server" CssClass='TextBox TextBox_short ToolTip'
                                            DataType="integer" AssociatedLableText="Nếu nhập thiếu thì không chọn đủ hoặc không trả" />
                                    </div>
                                </div>--%>
                            </div>
                            <div class="DivRow width100 fl  ">
                                <div class="fl fl1">
                                    <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="Nội dung chưa đủ" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ671" runat="server" CssClass='TextBox Group6 Effect textarea1 ToolTip'
                                        Width="420px" TextMode="MultiLine" AssociatedLableText="Nếu nhập thiếu thì không chọn đủ hoặc không trả" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ68">
                            <div class="DivRow DivHeader fl">
                                <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="6.8. Làm thủ tục thanh toán các chế độ bảo hiểm xã hội đầy đủ, kịp thời:" />
                            </div>
                            <div class="DiwRow Child fr fr1" style="margin-right: 10px; position: relative;">
                                <asp:CheckBoxList ID="chkQ68" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="1" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                            <div class="DivRow width100 fl Effect EffectGroup">
                                <div class="fl fl1">
                                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="Nội dung không đầy đủ, kịp thời" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ681" runat="server" CssClass='TextBox Group6 Effect textarea1'
                                        Width="420px" TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="clear: both; height: 15px;">
                    </div>
                    <div class="DivRow">
                        <div class="DivLabel">
                            &nbsp;&nbsp;&nbsp;</div>
                        <div class="DivTextBox">
                            <div style="float: left; padding-top: 10px;">
                                <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" CausesValidation="true"
                                    CommandArgument="" OnClientClick="javascript:return TestVaild();" />
                                &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                                    Text="Làm lại" /></div>
                            <div style="float: right; text-align: right">
                                &nbsp;<%--<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" />--%></div>
                        </div>
                    </div>
                </div>
                <div style="display: none">
                    <asp:HiddenField ID="hidSum3HDLDFirst" Value="0" runat="server" />
                    <asp:HiddenField ID="hidSum2HDLDFirst" Value="0" runat="server" />
                    <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
                    <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
                    <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
