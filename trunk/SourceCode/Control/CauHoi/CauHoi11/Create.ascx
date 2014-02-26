<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi11_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();

            // Dùng cho 2 checkbox có, không và enable các control kiểu input, textarea nằm trong class Group707
            CheckOnlyAndEffect();
        });
    }
</script>
<asp:UpdatePanel ID="uplCauhoi89101112" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
            </div>
            <div class="DivHeader" style="float: left;">
                <asp:Label ID="lbl1" CssClass="TextLabel" runat="server" Text="11. Lao động đặc thù" />
            </div>
            <div class="Child " style="padding-left: 53.10%;">
                <asp:CheckBoxList ID="chkQ11" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                    <asp:ListItem Value="0" Text=" Không" />
                </asp:CheckBoxList>
            </div>
            <div class="DivRow fl1 width100 fl">
                <div class="DivHeader">
                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="11.1 Lao động chưa thành niên" />
                </div>
                <div style="width: 66%">
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label41" CssClass="TextLabel" runat="server" Text="+ Số lao động dưới 15 tuổi:" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ1111" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" />
                            (người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label41b" CssClass="TextLabel" runat="server" Text="+ từ 15 đến dưới 18 tuổi:" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ1112" runat="server" CssClass='TextBox TextBox_short'
                                ataType="integer" />
                            (người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label41c" CssClass="TextLabel" runat="server" Text="+ Lập sổ theo dõi riêng:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ112" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label41d" CssClass="TextLabel" runat="server" Text="+ Ký hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ113" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label44e" CssClass="TextLabel" runat="server" Text="+ Làm các công việc nặng nhọc, nguy hiểm:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ114" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label44f" CssClass="TextLabel" runat="server" Text="+ Lập hồ sơ sức khỏe:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ115" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label412" CssClass="TextLabel" runat="server" Text="+ Thực hiện giảm giờ làm với lao động chưa thành niên :" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ116" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label413" CssClass="TextLabel" runat="server" Text="+ Tạo điều kiện cho người chưa thành niên học văn hóa:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ117" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="+ Bố trí công việc cấm người chưa thành niên:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ118" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl" style="padding-left:2%;">
                            <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="+ Tham gia BHXH:" />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ119" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl DivHeader">
                            <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="11.2 Lao động người cao tuổi:" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ1121" runat="server" CssClass='TextBox TextBox_short'
                                ataType="integer" />
                            (người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl EffectGroup">
                        <div class="fl DivHeader">
                            <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="11.3 Lao động là người khuyết tật:" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ1131" runat="server" CssClass='TextBox TextBox_short'
                                ataType="integer" />
                            (người)
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
                        &nbsp;&nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
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
    </ContentTemplate>
</asp:UpdatePanel>
