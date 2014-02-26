<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi2_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();

            //xu ly dau thap phan trong textbox khi load page
            $(".DivQ21 input").each(function () {
                $(this).val(FormatsNumber($(this).val()));
            });
            //xu ly checkbox khi click
            CheckOnlyAndEffect();

            //Disable txtQ218
            $(".Total :input").attr("disabled", true);
            //Tổng số lao động đã được kí hợp đồng
            $(".SumGroup input").live("keyup", function () {
                //xu ly dau thap phan tai textbox dang nhap
                var iValCur = $(this).val().replace(/,/g, '') * 1;
                $(this).val(FormatsNumber($(this).val(iValCur).val()));
                //tinh tong
                var sum = 0;
                $(".SumGroup input").each(function () {
                    sum += $(this).val().replace(/,/g, '') * 1;
                });
                $(".Total :input").val(FormatsNumber($(".Total :input").val(sum).val()));
            });

            //2.4. Mất việc làm:
            //TH: Load page
            checkLDMV($(".LDMV input"));
            //TH:keyup
            $(".LDMV input").live("keyup", function () {
                checkLDMV($(".LDMV input"));
            });

        });
        function checkLDMV(obj) {
            var iLDMV = obj.val().replace(/,/g, "") * 1;
            obj.val(FormatsNumber(obj.val(iLDMV).val()));
            if (iLDMV > 0) {
                obj.parents(".DivRow .LDMV").parent().find(".Child input").removeAttr("disabled");
                obj.parents(".DivRow .LDMV").parent().find(".Child input").removeClass("aspNetDisabled");
            }
            else {
                obj.parents(".DivRow .LDMV").parent().find(".Child input").attr("disabled", true);
                obj.parents(".DivRow .LDMV").parent().find(".Child input").addClass("aspNetDisabled");
            }

        }

    }
</script>
<asp:UpdatePanel ID="uplCauhoi23" runat="server">
    <ContentTemplate>
        <div id="generate">
            <div class="BoxField">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
                </div>
                <div class="DivRequired" style="font-weight: bold">
                    2. Hợp đồng lao động
                </div>
                <div class="DivRow fl fl1">
                    <div class="col20">
                        <div class="DivRow21">
                            <div class="DivRow width100 fl Checked">
                                <div class="DivHeader fl ">
                                    <asp:Label ID="lbl1" CssClass="TextLabel" runat="server" Text="2.1. Ký hợp đồng đúng loại:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ21" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="lbl2" CssClass="TextLabel" runat="server" Text="+ HĐLĐ không xác định thời hạn:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ211" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow  SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="+ HĐLĐ xác định thời hạn từ 12 tháng đến 36 tháng:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ212" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="+ HĐLĐ xác định thời hạn từ 3 tháng đến dưới 12 tháng:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ213" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="+ HĐLĐ mùa vụ dưới 3 tháng:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ214" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="+ Hợp đồng khoán gọn theo vụ việc:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ215" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="+ Hợp đồng học nghề, thử việc:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ216" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label21" CssClass="TextLabel" runat="server" Text="- Chưa ký hợp đồng lao động:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ217" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label27" CssClass="TextLabel" runat="server" Text="- Lao động thuê của đơn vị khác:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ219" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow SumGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label34" CssClass="TextLabel" runat="server" Text="- Không phải ký hợp đồng lao động:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ2110" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow Total width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="- Tổng số lao động:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ218" runat="server" CssClass='TextBox TextBox_short aspNetDisabled'
                                        DataType="integer" AssociatedLableText="nhập số" />(người)
                                </div>
                            </div>
                        </div>
                        <div class="DivQ22">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label9" CssClass="TextLabel" runat="server" Text="2.2. Thỏa thuận và ghi hợp đồng lao động cụ thể về:" />
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="+ Chức danh nghề, công việc độc hại, nguy hiểm" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ221" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label29" CssClass="TextLabel" runat="server" Text="+ Công việc và địa điểm làm việc" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ222" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label30" CssClass="TextLabel" runat="server" Text="+ Thời hạn của hợp đồng" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ223" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label10" CssClass="TextLabel" runat="server" Text="+ Mức lương" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ224" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label31" CssClass="TextLabel" runat="server" Text="+ Chế độ nâng lương" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ225" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label32" CssClass="TextLabel" runat="server" Text="+ Thời giờ làm việc, nghỉ ngơi" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ226" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label11" CssClass="TextLabel" runat="server" Text="+ Phương tiện bảo vệ cá nhân" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ227" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label12" CssClass="TextLabel" runat="server" Text="+ Chế độ bảo hiểm xã hội, bảo hiểm y tế" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ228" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label33" CssClass="TextLabel" runat="server" Text="+ Đào tạo nâng cao trình độ, tay nghề" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ229" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label22" CssClass="TextLabel" runat="server" Text="+ Bí mật công nghệ, bí mật kinh doanh" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ2210" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label23" CssClass="TextLabel" runat="server" Text="+ Thỏa thuận trái luật" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ2211" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 40px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="DivRow EffectGroup">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label28" CssClass="TextLabel fl1" runat="server" Text="Nội dung:" />
                                    </div>
                                    <div class="fr PaddingRight">
                                        <asp:TextBox ID="txtQ22111" runat="server" CssClass='TextBox textarea1' Height="100px"
                                            TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col20">
                        <div class="DivQ23 width100 fl">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label15" CssClass="TextLabel" runat="server" Text="2.3. Tuyển dụng, đào tạo nghề và thử việc" />
                            </div>
                            <div class="DivRow">
                                <div class="fl1" style="float: left">
                                    <asp:Label ID="Label35" CssClass="TextLabel" runat="server" Text="- Nhu cầu tuyển dụng năm hiện tại" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ234" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl1" style="float: left">
                                    <asp:Label ID="Label36" CssClass="TextLabel" runat="server" Text="- Số lao động mới được tuyển" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ235" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl1" style="float: left">
                                    <asp:Label ID="Label37" CssClass="TextLabel" runat="server" Text="- Số người tuyển qua các Trung tâm giới thiệu việc làm" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ238" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class=" fl1">
                                    <asp:Label ID="Label16" CssClass="TextLabel" runat="server" Text="- Thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="Child fl1">
                                    <asp:CheckBoxList ID="chkQ231" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class=" fl1" style="float: left;">
                                    <asp:Label ID="Label25" CssClass="TextLabel" runat="server" Text="- Giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động trái luật" />
                                </div>
                                <div class="Child fl1" style="padding-left: 73%">
                                    <asp:CheckBoxList ID="chkQ233" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="DivRow EffectGroup" style="padding-left: 3%;">
                                    <asp:Label ID="Label40" CssClass="TextLabel" runat="server" Text="+ Đã giữ" />
                                    <cc2:ValidationTextBox ID="txtQ2331" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(trường hợp)
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class=" fl1">
                                    <asp:Label ID="Label38" CssClass="TextLabel" runat="server" Text="- Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="Child fl1">
                                    <asp:CheckBoxList ID="chkQ236" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class=" fl1">
                                    <asp:Label ID="Label39" CssClass="TextLabel" runat="server" Text="- Thực hiện ký kết hợp đồng đào tạo nghề (nếu có)" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="Child fl1">
                                    <asp:CheckBoxList ID="chkQ237" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow fl1 width100 fl">
                                <asp:Label ID="Label18" CssClass="TextLabel" runat="server" Text="- Áp dụng thời gian thử việc:" />
                            </div>
                            <div class="DivRow fl1">
                                <div class=" fl1  Child">
                                    <asp:Label ID="Label19" CssClass="TextLabel" runat="server" Text="+ Quá 60 ngày đối với lao động có chức danh nghề cần trình độ chuyên môn kỹ thuật từ cao đẳng trở lên" />
                                    <asp:CheckBoxList ID="chkQ2321" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow fl1">
                                <div class=" fl1 Child">
                                    <asp:Label ID="Label13" CssClass="TextLabel" runat="server" Text="+ Quá 30 ngày đối với lao động có chức danh nghề cần trình độ trung cấp" />
                                    <asp:CheckBoxList ID="chkQ2322" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow fl1">
                                <div class=" fl1 Child">
                                    <asp:Label ID="Label20" CssClass="TextLabel" runat="server" Text="+ Quá 6 ngày đối với lao động khác" />
                                    <asp:CheckBoxList ID="chkQ2323" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                        <div class="DivQ24 width100 fl">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label17" CssClass="TextLabel" runat="server" Text="2.4. Mất việc làm:" />
                            </div>
                            <div class="DivRow LDMV width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label14" CssClass="TextLabel" runat="server" Text="- Số lao động bị mất việc làm:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ241" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow fl1 width100 fl">
                                <asp:Label ID="Label24" CssClass="TextLabel" runat="server" Text="- Báo cáo với Sở LĐTBXH khi cho nhiều lao động thôi việc do mất việc làm" />
                            </div>
                            <div class="DivRow Child fl1">
                                <asp:CheckBoxList ID="chkQ242" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="DivQ25 width100 fl">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label26" CssClass="TextLabel" runat="server" Text="2.5. Số lao động thôi việc(kể cả bỏ việc):" />
                            </div>
                            <div class="DivRow  width100 fl">
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ251" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
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
                                CommandArgument="" OnClientClick="javascript:return validateFormInputs();" />
                            &nbsp; &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
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
