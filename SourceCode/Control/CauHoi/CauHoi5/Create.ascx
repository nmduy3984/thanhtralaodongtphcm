<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi5_Create" %>
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
            $("#<%=txtQ511.ClientId %>").keypress(function (event) {
                IntegerAndDecimal(event, this, 'true');
            });
            $("#<%=txtQ512.ClientId %>").keypress(function (event) {
                IntegerAndDecimal(event, this, 'true');
            });
            $("#<%=txtQ514.ClientId %>").keypress(function (event) {
                IntegerAndDecimal(event, this, 'true');
            });
            $("#<%=txtQ515.ClientId %>").keypress(function (event) {
                IntegerAndDecimal(event, this, 'true');
            });
        });
    }
</script>
<style type="text/css">
    .SizeTextBox
    {
        max-height: 50px;
        max-width: 250px;
        min-height: 50px;
        min-width: 250px;
    }
</style>
<asp:UpdatePanel ID="uplCauhoi5" runat="server">
    <ContentTemplate>
        <div id="generate">
            <div class="BoxField">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
                </div>
                <div class="DivRequired" style="font-weight: bold">
                    5. Thời giờ làm việc, thời giờ nghỉ ngơi
                </div>
                <div class="DivRow fl1">
                    <div style="width: 67%;">
                        <div class="DivQ51">
                            <div class="DivRow">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label17" CssClass="TextLabel" runat="server" Text="5.1. Số giờ làm việc:" />
                                </div>
                                <div class="fr fr1">
                                </div>
                            </div>
                            <div class="width100 fl EffectGroup">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="- Công việc bình thường:" />
                                </div>
                            </div>
                            <div class="width100 fl EffectGroup">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="&nbsp;&nbsp;Với người hưởng lương thời gian:" />
                                </div>
                                <div class="fr fr1">
                                    <asp:TextBox ID="txtQ511" runat="server" CssClass='TextBox TextBox_short'></asp:TextBox>(giờ/ngày)
                                    <asp:TextBox ID="txtQ512" runat="server" CssClass='TextBox TextBox_short'></asp:TextBox>(giờ/tuần)
                                </div>
                            </div>
                            <div class="width100 fl EffectGroup">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="&nbsp;&nbsp;Với người hưởng lương sản phẩm:" />
                                </div>
                                <div class="fr fr1">
                                    <asp:TextBox ID="txtQ514" runat="server" CssClass='TextBox TextBox_short'></asp:TextBox>(giờ/ngày)
                                    <asp:TextBox ID="txtQ515" runat="server" CssClass='TextBox TextBox_short'></asp:TextBox>(giờ/tuần)
                                </div>
                            </div>
                            <div class="width100 fl EffectGroup">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="- Công việc đặt biệt nặng nhọc, độc hại, nguy hiểm:" />
                                </div>
                                <div class="" style="padding-left: 62.4%">
                                    <cc2:ValidationTextBox ID="txtQ513" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="numeric" AssociatedLableText="Nhập số(giờ/ngày)" />(giờ/ngày)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ52 ">
                            <div class="DivRow width100 fl Child">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label20" CssClass="TextLabel" runat="server" Text="5.2. Làm thêm giờ:" />
                                </div>
                                <div class="fr fr1" style="margin-right: 10px;">
                                    <asp:CheckBoxList ID="chkQ52" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="width100 fl EffectGroup">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label23" CssClass="TextLabel" runat="server" Text="- Số giờ làm thêm cao nhất trong ngày:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ521" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(giờ/người)
                                </div>
                            </div>
                            <div class="width100 fl EffectGroup">
                                <div class="fl fl1">
                                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="- Số giờ làm thêm cao nhất trong năm:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ522" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(giờ/người)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ53">
                            <div class="DivRow width100 fl Child">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label29" CssClass="TextLabel" runat="server" Text="5.3. Thực hiện ngày nghỉ hàng năm hưởng nguyên lương." />
                                </div>
                                <div class="fr fr1" style="margin-right: 10px;">
                                    <asp:CheckBoxList ID="chkQ53" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow fl1">
                                <asp:Label ID="Label30" CssClass="TextLabel" runat="server" Text="- Nếu có:" />
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label33" CssClass="TextLabel fl1" runat="server" Text="+ Lao động làm công việc bình thường:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ531" runat="server" CssClass='TextBox TextBox_short GroupQ53'
                                        DataType="integer" AssociatedLableText="Nhập số" />(ngày/năm)
                                </div>
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl1 fl">
                                    <asp:Label ID="Label34" CssClass="TextLabel fl1" runat="server" Text="+ Lao động làm công việc nặng nhọc, độc hại, nguy hiểm:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ532" runat="server" CssClass='TextBox TextBox_short GroupQ53'
                                        DataType="integer" AssociatedLableText="Nhập số" />(ngày/năm)
                                </div>
                            </div>
                            <div class="DivRow EffectGroup">
                                <div class="fl1 fl">
                                    <asp:Label ID="Label35" CssClass="TextLabel fl1" runat="server" Text="+ Lao động làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ533" runat="server" CssClass='TextBox TextBox_short GroupQ53'
                                        DataType="integer" AssociatedLableText="Nhập số" />(ngày/năm)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ54">
                            <div class="DivRow DivHeader fl">
                                <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="5.4. Thực hiện nghỉ ngày lễ hưởng nguyên lương:" />
                            </div>
                            <div class="DiwRow Child fr fr1" style="margin-right: 10px; position: relative;">
                                <asp:CheckBoxList ID="chkQ54" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Đủ" style="margin-right: 375px;" />
                                    <asp:ListItem Value="0" Text=" Không thực hiện" />
                                </asp:CheckBoxList>
                                <div class="DivRow- " style="position: absolute; top: -1px; left: 78px">
                                    <div class="fr fr1">
                                        <div style="padding-top: 2px; padding-right: 3px; float: left">
                                            thiếu</div>
                                        <asp:TextBox ID="txtQ541" Width="250px" Height="50px" runat="server" CssClass='TextBox TextBox_short GroupQ53 ToolTip SizeTextBox'
                                            DataType="none" AssociatedLableText="Nếu nhập ngày thiếu thì không chọn đủ hoặc không thực hiện"
                                            TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="height: 85px;">
                        </div>
                        <div class="DivRow width100 fl " style="padding-top: 5px;">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="5.5. Nghỉ việc riêng:" />
                            </div>
                            <div class="Child fr fr1" style="margin-right: 10px;">
                                <asp:CheckBoxList ID="chkQ55" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        &nbsp;&nbsp;&nbsp;</div>
                    <div class="DivTextBox">
                        <div style="float: left; padding-top: 10px;">
                            <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" CausesValidation="true"
                                CommandArgument="" OnClientClick="return validateFormInputs();" />
                            &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                                Text="Làm lại" /></div>
                        <div style="float: right; text-align: right">
                            &nbsp;<%--<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" />--%></div>
                    </div>
                </div>
            </div>
            <div style="display: none">
                <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
                <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
                <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
