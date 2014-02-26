<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi12_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
            $("#<%=txtQ121.ClientID %>").attr("disabled", true);
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
            <div class="DivRequired" style="font-weight: bold">
                12. Lao động nữ và bình đẳng giới
            </div>
            <div class="DivRow fl1 width100 fl">
                <div style="width: 60%">
                    <div class="DivRow width100 fl">
                        <div class="fl fl1">
                            <asp:Label ID="Label34" CssClass="TextLabel" runat="server" Text="+ Số lao động nữ:" />
                        </div>
                        <div class="Child fr fr1">
                            <cc2:ValidationTextBox ID="txtQ121" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" />(người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label10b" CssClass="TextLabel" runat="server" Text="+ Bố trí nữ làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm: " />
                            </div>
                            <div class="Child fr fr1">
                                <%--<asp:CheckBox ID="chkQ1211a" Text=" Có " CssClass="checkbox" runat="server" style="margin-right:50px;"/>
                                <asp:CheckBox ID="chkQ1211b" Text=" Không " CssClass="checkbox" runat="server"  />--%>
                                <asp:CheckBoxList ID="chkQ1211" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label10c" CssClass="TextLabel" runat="server" Text="+ Giảm giờ làm đối với lao động nữ có thai, con bú: " />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ1212" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="+ Bố trí nơi vệ sinh phụ nữ và trả lương 30 phút vệ sinh mỗi ngày trong thời gian hành kinh: " />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ1213" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="fl fl1 DivHeader">
                            <asp:Label ID="Label38" CssClass="TextLabel" runat="server" Text="Bình đẳng giới" />
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label2" CssClass="TextLabel fl1" runat="server" Text="+ Tuyển dụng và trả công phân biệt nam và nữ: " />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ122" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label3v" CssClass="TextLabel fl1" runat="server" Text="+ Đào tạo, bổ nhiệm phân biệt nam và nữ: " />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ123" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label4" CssClass="TextLabel fl1" runat="server" Text="+ Tỉ lệ cán bộ nữ đi học: " />
                                <span>
                                    <cc2:ValidationTextBox ID="txtQ1241" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="nhập số" /></span> %. Tỷ lệ cán
                                bộ quản lý là nữ: <span>
                                    <cc2:ValidationTextBox ID="txtQ1242" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="nhập số" /></span> %
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="DivRequired" style="font-weight: bold">
                13. Kiến nghị của doanh nghiệp với nhà nước
            </div>
            <div class="DivRow width100 fl">
                <div class="DivRow width100 fl">
                    <div class="fl fl1">
                        &nbsp;
                    </div>
                    <div>
                        <asp:TextBox ID="txtYKienDN" runat="server" CssClass="TextBox" style="width: 699px; height: 200px; min-height:200px;max-height:200px;min-width: 699px;max-width:699px;"
                            TextMode="MultiLine" />
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
                            Text="Làm lại" />
                        &nbsp;<asp:Button ID="btnKetThuc" runat="server" CssClass="btn" Text="Kết thúc" 
                            Enabled="False" style="height: 26px" />
                        &nbsp;<asp:Button ID="btnXemKetLuan" runat="server" CausesValidation="false" CssClass="btn" Enabled="False"
                            Text="Xem kết quả" />&nbsp;<asp:Button ID="btnInPhieuKienNghi" runat="server" CausesValidation="false" CssClass="btn" Enabled="False"
                            Text="Phiếu kiến nghị" />&nbsp;<asp:Button ID="btnInBienBan" runat="server" CssClass="btn"
                                Text="In Biên Bản" Enabled="False" />
                        &nbsp;<asp:Button ID="btnInKetLuan" runat="server" CssClass="btn" Text="In Kết Luận"
                            Enabled="False" Style="height: 26px" />
                        &nbsp;<asp:Button ID="btnINBBVP" runat="server" CssClass="btn" Text=" In BBVP " Enabled="False" /></div>
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
