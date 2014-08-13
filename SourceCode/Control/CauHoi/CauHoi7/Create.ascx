<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi56_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();

            //$(".textarea1").attr("disabled", true);
            //Mục 7.6: TH: Số người đã được cấp thẻ an toàn > số người phải cấp thẻ an toàn            
//            $("#<%=txtQ706232.ClientId %>").live("keyup", function () {
//                var txtQ706232 = $(this);
//                var txtSoNguoiPhaiCapTheAT = $("#<%=txtSoNguoiPhaiCapTheAT.ClientId %>");
//                if (txtSoNguoiPhaiCapTheAT.val().replace(/,/gi, '') * 1 > 0 && txtQ706232.val().replace(/,/gi, '') * 1 > txtSoNguoiPhaiCapTheAT.val().replace(/,/gi, '') * 1) {
//                    AlertboxThenFocus("Vui lòng nhập lại.\n Số người đã được cấp thẻ an toàn phải nhỏ hơn hoặc bằng số người phải cấp thẻ an toàn.", txtQ706232);
//                    txtQ706232.select();
//                    return false;
//                }
//            });
            //Validate mục 7.6 Số người phải cấp thẻ an toàn
            var txtSoNguoiPhaiCapTheAT = $("#<%=txtSoNguoiPhaiCapTheAT.ClientId %>").val().replace(/,/g, '') * 1;
            if (txtSoNguoiPhaiCapTheAT == 0) {
                $("#<%=txtQ706232.ClientId %>").attr("disabled", true).addClass("aspNetDisabled");
            }
            //tính số được khám 7.14 khi nhập chưa được khám
            //$("#<%=txtQ71421.ClientId %>").attr("disabled",true);
            $("#<%=txtQ71422.ClientId %>").live("keyup", function () {
                var hidTongLaoDong = $("#<%=hidTongLaoDong.ClientId %>").val();
                var txtQ71422 = $(this).val().replace(/,/g, '') * 1;
                var result = hidTongLaoDong - txtQ71422;
                if (result < 0) {
                    AlertboxThenFocus("Vui lòng nhập lại.\n Số chưa được khám phải nhỏ hơn số phải khám; Lao động dưới 3 tháng không phải khám vì kết quả khám tuyển còn hiệu lực.", $(this));
                    $(this).select();
                    return false;
                }
            });

            //Disable mục 7.12 nếu doanh nghiệp không có lao động làm công việc độc hại, nguy hiểm
            var hidSoLDLamCVDHNH = $("#<%=hidSoLDLamCVDHNH.ClientId %>").val() * 1;
            if (hidSoLDLamCVDHNH == 0) {
                $(".DivRow712 input").attr("disabled", true).addClass("aspNetDisabled");
                $(".DivRow712 textarea").attr("disabled", true).addClass("aspNetDisabled");
            }

            // Dùng cho Textbox khi blur ra thì xét xem có disable các mục liên quan
            $(".TextChild input[type=text]").blur(function () {
                if ($(this).val().replace(/,/g, '') * 1 == 0) {
                    $(this).parents(".TextChild").parent().find(".Group1 textarea,.Group1 input").attr("disabled", true).addClass("aspNetDisabled");
                } else {
                    $(this).parents(".TextChild").parent().find(".Group1 textarea,.Group1 input").removeAttr("disabled").removeClass("aspNetDisabled");
                }
            });

            // Dùng khi các radio không đầy đủ click để enable Textarea
            $(".RadioList input").live("click", function () {
                var obj = $(this).parents(".DivRow").next().find("textarea");
                if ($(this).val() != 3) {
                    obj.attr("disabled", true).addClass("aspNetDisabled");
                } else {
                    obj.removeAttr("disabled").removeClass("aspNetDisabled");
                }
            });

            // khi nhấp chọn checkbox đủ thì enable textbox bên cạnh
            $(".Child1").click(function () {
                if ($(this).find(" input:checkbox").is(':checked')) {
                    $(this).next().attr("disabled", true).addClass("aspNetDisabled");
                } else {
                    $(this).next().removeAttr("disabled").removeClass("aspNetDisabled");
                }
            });

            // Dùng cho 2 checkbox có, không và enable các control kiểu input, textarea nằm trong class EffectGroup
            CheckOnlyAndEffect();
            //7.5
            $(".DivRow705 .Total :input").attr("disabled", true);
            //Tổng số lao động đã được kí hợp đồng
            $(".DivRow705 .KDTB input").live("keyup", function () {
                //xu ly dau thap phan tai textbox dang nhap
                var iValCur = $(this).val().replace(/,/g, '') * 1;
                $(this).val(FormatsNumber($(this).val(iValCur).val()));
                //tinh tong
                var sum = 0;
                $(".DivRow705 .KDTB input").each(function () {
                    sum += $(this).val().replace(/,/g, '') * 1;
                });
                $(".DivRow705 .Total :input").val(FormatsNumber($(".DivRow705 .Total :input").val(sum).val()));
            });

            //Disable txtQ7118
            $(".Total :input").attr("disabled", true);
            //Tổng số lao động đã được kí hợp đồng
            $(".TaiNan input").live("keyup", function () {
                //xu ly dau thap phan tai textbox dang nhap
                var iValCur = $(this).val().replace(/,/g, '') * 1;
                $(this).val(FormatsNumber($(this).val(iValCur).val()));
                //tinh tong
                var sum = 0;
                $(".TaiNan input").each(function () {
                    sum += $(this).val().replace(/,/g, '') * 1;
                });
                $(".Total :input").val(FormatsNumber($(".Total :input").val(sum).val()));
            });
            //7.11 reset các textbox khi tổng số vụ tai nạn lao động bằng 0 hoặc rỗng
            $("#<%=txtQ711.ClientId %>").live("keyup", function () {
                if ($(this).val() * 1 == 0 || $(this).val() == '') {
                    $(".TaiNan input").each(function () {
                        $(this).val('');
                    });
                    $(".Total :input").val('');
                }
            });
            //7.10
            $(".CapTheAT :input").attr("disabled", true);

            //7.1  Tổng số người làm công tác an toàn, vệ sinh lao động            
            //TH: Load page
            checkATVS($(".ATVS input"));
            //TH:keyup
            $(".ATVS input").live("keyup", function () {
                checkATVS($(".ATVS input"));
            });
        });
    }
    function checkATVS(obj) {
        var iATVS = obj.val().replace(/,/g, "") * 1;
        obj.val(FormatsNumber(obj.val(iATVS).val()));
        if (iATVS > 0) {
            obj.parents(".DivRow .ATVS").parent().find(".Child input").removeAttr("disabled").removeClass("aspNetDisabled");
        }
        else {
            obj.parents(".DivRow .ATVS").parent().find(".Child input").attr("disabled", true).addClass("aspNetDisabled");
        }

    }
    function TestVaild() {
        if (validateFormInputs()) {
            var txtQ70611 = $("#<%=txtQ70611.ClientId %>");
            var txtQ70612 = $("#<%=txtQ70612.ClientId %>");

            //7.11
            var txtQ711 = $("#<%=txtQ711.ClientId %>");
            var txtQ7113 = $("#<%=txtQ7113.ClientId %>");
            var txtQ7111 = $("#<%=txtQ7111.ClientId %>");
            var txtQ7112 = $("#<%=txtQ7112.ClientId %>");
            var txtQ7116 = $("#<%=txtQ7116.ClientId %>");
            var txtQ7114 = $("#<%=txtQ7114.ClientId %>");
            var txtQ7122 = $("#<%=txtQ7122.ClientId %>");
            var txtQ71221 = $("#<%=txtQ71221.ClientId %>");
            var txtQ715211 = $("#<%=txtQ715211.ClientId %>");
            var txtQ715212 = $("#<%=txtQ715212.ClientId %>");
            var txtQ715213 = $("#<%=txtQ715213.ClientId %>");

            if (txtQ70611.val().replace(/,/gi, '') * 1 > txtQ70612.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số cán bộ quản lý phải lớn hơn hoặc bằng số cán bộ quản lý được huấn luyện.", txtQ70611);
                txtQ70611.select();
                return false;
            }
            if (txtQ7113.val().replace(/,/gi, '') * 1 > txtQ711.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Tổng số vụ tai nạn lao động phải lớn hơn hoặc bằng số vụ khai báo, điều tra.", txtQ7113);
                txtQ7113.select();
                return false;
            }
            var sum = (txtQ7111.val().replace(/,/gi, '') * 1 + txtQ7112.val().replace(/,/gi, '') * 1 + txtQ7116.val().replace(/,/gi, '') * 1);
            if (sum < txtQ7114.val() * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số người được giải quyết chế độ phải nhỏ hơn hoặc bằng tổng của số người chết, số người bị thương nặng, số người bị thương nhẹ.", txtQ7114);
                txtQ7114.select();
                return false;
            }
            if (txtQ71221.val().replace(/,/gi, '') * 1 > txtQ7122.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số mẫu không đạt phải nhỏ hơn hoặc bằng số mẫu đã đo.", txtQ71221);
                txtQ71221.select();
                return false;
            }
            if (txtQ715212.val().replace(/,/gi, '') * 1 > txtQ715211.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số được cấp sổ phải nhỏ hơn hoặc bằng số được giám định, điều trị.", txtQ715212);
                txtQ715212.select();
                return false;
            }
            if (txtQ715213.val().replace(/,/gi, '') * 1 > txtQ715211.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số người được chuyển công việc khác phải nhỏ hơn hoặc bằng số được giám định, điều trị.", txtQ715213);
                txtQ715213.select();
                return false;
            }
            //7.12
//            var txtQ706232 = $("#<%=txtQ706232.ClientId %>");
//            var txtSoNguoiPhaiCapTheAT = $("#<%=txtSoNguoiPhaiCapTheAT.ClientId %>");
//            if (txtQ706232.val().replace(/,/gi, '') * 1 > txtSoNguoiPhaiCapTheAT.val().replace(/,/gi, '') * 1) {
//                AlertboxThenFocus("Vui lòng nhập lại.\n Số người đã được cấp thẻ an toàn phải nhỏ hơn hoặc bằng số người phải cấp thẻ an toàn.", txtQ706232);
//                txtQ706232.select();
//                return false;
//            }
            //7.14

            var hidTongLaoDong = $("#<%=hidTongLaoDong.ClientId %>").val();
            var txtQ71422 = $("#<%=txtQ71422.ClientId %>").val().replace(/,/g, '') * 1;
            var result = hidTongLaoDong - txtQ71422;
            if (result < 0) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số chưa được khám phải nhỏ hơn số phải khám; Lao động dưới 3 tháng không phải khám vì kết quả khám tuyển còn hiệu lực.", $("#<%=txtQ71422.ClientId %>"));
                $("#<%=txtQ71422.ClientId %>").select();
                return false;
            }
            return true;
        }
        return false;
    }
</script>
<asp:UpdatePanel ID="uplCauhoi7" runat="server">
    <ContentTemplate>
        <div id="generate">
            <div class="BoxField">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
                </div>
                <div class="DivRequired DivHeader">
                    7. An toàn lao động, vệ sinh lao động:
                </div>
                <div class="DivRow fl1">
                    <div class="col20">
                        <div class="DivRow701">
                            <div class="DivRow ATVS width100 fl">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="7.1. Tổng số người làm công tác an toàn, vệ sinh lao động:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ701" runat="server" AssociatedLableText="Gồm cả cán bộ chuyên trách, kiêm nhiệm và thuê dịch vụ"
                                        CssClass='TextBox  TextBox_short ToolTip' DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="- Cán bộ chuyên trách an toàn" />
                                </div>
                                <div class="Child fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7011" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="- Hợp đồng với Tổ chức dịch vụ an toàn lao động:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ7015" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="- Cán bộ y tế" />
                                </div>
                                <div class="Child fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7012" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label10" CssClass="TextLabel" runat="server" Text="- Hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ7016" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="- Mạng lưới an toàn viên" />
                                </div>
                                <div class="Child fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7013" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="- Hội đồng bảo hộ lao động" />
                                </div>
                                <div class="Child fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7014" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow702">
                            <div class="DivRow DivHeader width100 fl">
                                <asp:Label ID="Label9" CssClass="TextLabel" runat="server" Text="7.2. Phân định trách nhiệm về an toàn vệ sinh lao động:" />
                            </div>
                            <div class="Child DivRow width100 fl fl1">
                                <asp:CheckBoxList ID="chkQ702" CssClass="checkbox" runat="server" RepeatDirection="Horizontal"
                                    Width="168px">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="Child fl fl1">
                                <asp:Label ID="Label79" CssClass="TextLabel" runat="server" Text="- Thống kê số người làm công việc độc hại:" />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ7021" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" style="margin-right: 50px;" />
                                    <asp:ListItem Value="2" Text=" Không đầy đủ" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow703">
                            <div class="DivRow">
                                <div class="DivHeader">
                                    <asp:Label ID="Label16" CssClass="TextLabel" runat="server" Text="7.3. Xây dựng kế hoạch công tác ATVSLĐ hàng năm:" />
                                </div>
                            </div>
                            <div class="DivRow fl1">
                                <div class="RadioList">
                                    <asp:RadioButtonList ID="rdlQ703" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="true" Text=" Có " Value="1" />
                                        <asp:ListItem Text=" Không " Value="2" style="margin: 0 50px 0 60px;" />
                                        <asp:ListItem Text=" Không đầy đủ " Value="3" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="DivRow GroupByRadio">
                                <asp:Label ID="Label37" CssClass="TextLabel fl1" runat="server" Text="Thiếu nội dung:" />
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7031" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow704">
                            <div class="DivRow">
                                <div class="DivHeader">
                                    <asp:Label ID="Label17" CssClass="TextLabel" runat="server" Text="7.4. Xây dựng quy trình, biện pháp an toàn:" />
                                </div>
                            </div>
                            <div class="DivRow fl1">
                                <div class="RadioList">
                                    <asp:RadioButtonList ID="rdlQ704" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="true" Text=" Có " Value="1" />
                                        <asp:ListItem Text=" Không " Value="2" style="margin: 0 50px 0 60px;" />
                                        <asp:ListItem Text=" Không đầy đủ " Value="3" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="DivRow GroupByRadio">
                                <asp:Label ID="Label18" CssClass="TextLabel fl1" runat="server" Text="Thiếu quy trình:" />
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7041" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow705">
                            <div class="DivRow">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label19" CssClass="TextLabel" runat="server" Text="7.5. Kiểm định thiết bị:" />
                                </div>
                            </div>
                            <div class="DivRow width100 fl  KDTB">
                                <div class="fl fl1">
                                    <asp:Label ID="Label42" CssClass="TextLabel" runat="server" Text="+ Số thiết bị nâng hạ, vận chuyển:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7054" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 fl  KDTB">
                                <div class="fl fl1">
                                    <asp:Label ID="Label80" CssClass="TextLabel" runat="server" Text="+ Số thiết bị chịu áp lực và đường ống:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7055" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 fl  KDTB">
                                <div class="fl fl1">
                                    <asp:Label ID="Label81" CssClass="TextLabel" runat="server" Text="+ Số Hệ thống lạnh:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7056" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 fl KDTB">
                                <div class="fl fl1">
                                    <asp:Label ID="Label82" CssClass="TextLabel" runat="server" Text="+ Bồn chứa hóa chất:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7057" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 fl KDTB">
                                <div class="fl fl1">
                                    <asp:Label ID="Label84" CssClass="TextLabel" runat="server" Text="+ Thiết bị khác:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7059" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 fl Total">
                                <div class="fl fl1">
                                    <asp:Label ID="Label83" CssClass="TextLabel" runat="server" Text="+ Tổng số thiết bị có yêu cầu nghiêm ngặt:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7058" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label20" CssClass="TextLabel" runat="server" Text="+ Số đã kiểm định:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7051" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 TextChild fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label21" CssClass="TextLabel" runat="server" Text="+ Số chưa kiểm định:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7052" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                            <div class="DivRow width100 Group1 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="+ Số thiết bị chưa kiểm định mỗi loại" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7053" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow706">
                            <%--<div class="DivRow">--%>
                            <div class="DivHeader fl">
                                <asp:Label ID="Label22" CssClass="TextLabel" runat="server" Text="7.6. Huấn luyện an toàn lao động, vệ sinh lao động" />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ706" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                            <%--</div>--%>
                            <div class="DivRow width100 fl EffectGroup">
                                <div class="DivRow">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label23" CssClass="TextLabel" runat="server" Text="- Số CBQL đã huấn luyện/TS cán bộ quản lý:" />
                                    </div>
                                    <div class="fr fr1">
                                        <cc2:ValidationTextBox ID="txtQ70611" runat="server" CssClass='TextBox TextBox_short'
                                            DataType="integer" />
                                        <cc2:ValidationTextBox ID="txtQ70612" runat="server" CssClass='TextBox TextBox_short'
                                            DataType="integer" />(người)
                                    </div>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label24" CssClass="TextLabel" runat="server" Text="- Cho người lao động:" />
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label25" CssClass="TextLabel fl1" runat="server" Text="Lần đầu:" />
                                </div>
                                <div class="fr fr1 EffectGroup">
                                    Đủ
                                    <asp:CheckBox ID="chkQ706211" Text="" CssClass="checkbox Child1" runat="server" Style="margin-right: 30px;" />thiếu
                                    <cc2:ValidationTextBox ID="txtQ706212" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label26" CssClass="TextLabel fl1" runat="server" Text="Định kỳ:" />
                                </div>
                                <div class="fr fr1 EffectGroup">
                                    Đủ
                                    <asp:CheckBox ID="chkQ706221" Text="" CssClass="checkbox Child1" runat="server" Style="margin-right: 30px;" />thiếu
                                    <cc2:ValidationTextBox ID="txtQ706222" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl CapTheAT">
                                <div class="fl fl1">
                                    <asp:Label ID="Label71" CssClass="TextLabel fl1" runat="server" Text="Số người làm công việc có yêu cầu nghiêm ngặt về an toàn:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtSoNguoiPhaiCapTheAT" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl EffectGroup">
                                <div class="fl fl1">
                                    <asp:Label ID="Label27" CssClass="TextLabel fl1" runat="server" Text="Số người được huấn luyện đúng quy định:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ706232" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow707">
                            <div class="DivRow">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label28" CssClass="TextLabel" runat="server" Text="7.7. Hồ sơ huấn luyện:" />
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ707" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Đầy đủ" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không đầy đủ" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="EffectGroup fr fr1">
                                    <asp:CheckBox ID="chkQ7071" Text=" Thiếu tài liệu " CssClass="checkbox" runat="server" />
                                    <asp:CheckBox ID="chkQ7072" Text=" Sổ theo dõi " CssClass="checkbox" runat="server"
                                        Style="margin: 0 22px 0 52px" />
                                    <asp:CheckBox ID="chkQ7073" Text=" Bài kiểm tra " CssClass="checkbox" runat="server"
                                        Style="padding-right: 15px;" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow708">
                            <div class="DivRow">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label29" CssClass="TextLabel" runat="server" Text="7.8. Nội dung huấn luyện:" />
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ708" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Đầy đủ" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không đầy đủ" />
                                    </asp:CheckBoxList>
                                </div>
                                <div style="margin: 13px 0px 5px 10px;">
                                    Thiếu nội dung:</div>
                                <div class="EffectGroup fr fr1">
                                    <asp:CheckBox ID="chkQ7081" Text=" Vệ sinh lao động " CssClass="checkbox" runat="server" />
                                    <asp:CheckBox ID="chkQ7082" Text=" Cấp cứu tai nạn lao động " CssClass="checkbox"
                                        runat="server" Style="margin: 0 22px 0 52px" />
                                    <asp:CheckBox ID="chkQ7083" Text=" Quy trình, biện pháp an toàn " CssClass="checkbox"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow DivRow709">
                            <div class="DivHeader DivRow">
                                <asp:Label ID="Label30" CssClass="TextLabel" runat="server" Text="7.9. Trang bị phương tiện bảo vệ cá nhân theo danh mục nghề:" />
                            </div>
                            <div class="DivRow width100 fl Child">
                                <div class="fl fl1">
                                </div>
                                <div class="fr fr1">
                                    <asp:CheckBoxList ID="chkQ709" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Đầy đủ" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không đầy đủ" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="lbl1" CssClass="TextLabel" runat="server" Text="Thiếu loại gì" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7091" runat="server" CssClass='TextBox textarea1' Height="100px"
                                        TextMode="MultiLine" />
                                </div>
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label31" CssClass="TextLabel" runat="server" Text="cho chức danh nghề gì" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ70911" runat="server" CssClass='TextBox textarea1' Height="100px"
                                        TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow DivRow710 ">
                            <div class="DivHeader DivRow" style="width: 90%">
                                <asp:Label ID="Label35" CssClass="TextLabel" runat="server" Text="7.10. Thực hiện bồi dưỡng cho người lao động làm các công việc độc hại, nguy hiểm và đặc biệt độc hại, nguy hiểm:" />
                            </div>
                            <div class="DivRow width100 fl ">
                                <div class="DivRow width100 fl ">
                                    <div style="float: left; padding: 0 5px 0px 90px;">
                                        <asp:Label ID="Label11" CssClass="TextLabel" runat="server" Text="Đầy đủ:" />
                                    </div>
                                    <div class="Child fr fr1">
                                        <asp:CheckBoxList ID="chkQ71011" CssClass="checkbox" runat="server" RepeatDirection="Horizontal"
                                            Style="float: left">
                                            <asp:ListItem Value="1" Text=" Có" style="margin-right: 20px;" />
                                            <asp:ListItem Value="0" Text=" Không" />
                                        </asp:CheckBoxList>
                                    </div>
                                    <div class="DivRow  width100 fl">
                                        <div class="fl fl1 EffectGroup">
                                            <asp:Label ID="Label34a" CssClass="TextLabel fl1" runat="server" Text="Mức 1" Style="margin-right: 43px" />
                                            <cc2:ValidationTextBox ID="txtQ710111" runat="server" CssClass='TextBox TextBox_short'
                                                DataType="integer" />
                                            (đồng)
                                        </div>
                                    </div>
                                    <div class="DivRow  width100 fl">
                                        <div class="fl fl1 EffectGroup">
                                            <asp:Label ID="Label34" CssClass="TextLabel fl1" runat="server" Text="Mức 2" Style="margin-right: 43px" />
                                            <cc2:ValidationTextBox ID="txtQ710112" runat="server" CssClass='TextBox TextBox_short'
                                                DataType="integer" />
                                            (đồng)
                                        </div>
                                    </div>
                                    <div class="DivRow  width100 fl">
                                        <div class="fl fl1 EffectGroup">
                                            <asp:Label ID="Label65" CssClass="TextLabel fl1" runat="server" Text="Mức 3" Style="margin-right: 43px" />
                                            <cc2:ValidationTextBox ID="txtQ710113" runat="server" CssClass='TextBox TextBox_short'
                                                DataType="integer" />
                                            (đồng)
                                        </div>
                                    </div>
                                    <div class="DivRow  width100 fl">
                                        <div class="fl fl1 EffectGroup">
                                            <asp:Label ID="Label72" CssClass="TextLabel fl1" runat="server" Text="Mức 4" Style="margin-right: 43px" />
                                            <cc2:ValidationTextBox ID="txtQ710114" runat="server" CssClass='TextBox TextBox_short'
                                                DataType="integer" />
                                            (đồng)
                                        </div>
                                    </div>
                                    <div class="DivRow EffectGroup width100 fl">
                                        <div class="fl fl1">
                                            <asp:Label ID="Label36" CssClass="TextLabel fl1" runat="server" Text="  Chưa thực hiện với các nghề" />
                                        </div>
                                        <div class="fr fr1" style="margin-right: 43px">
                                            <asp:TextBox ID="txtQ710115" runat="server" CssClass='TextBox textarea1' Height="100px"
                                                TextMode="MultiLine" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div style="float: left; padding: 0 40px 0px 12px;">
                                <asp:Label ID="Label15" CssClass="TextLabel fl1" runat="server" Text="Bồi dưỡng bằng tiền:" />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ71012" CssClass="checkbox" runat="server" RepeatDirection="Horizontal"
                                    Style="float: left">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 20px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="DivRow DivRow711">
                            <div class="DivRow width100 fl TextChild">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label67" CssClass="TextLabel" runat="server" Text="7.11. Tổng số vụ tai nạn lao động:" />
                                </div>
                                <div class="fr fr1 " style="margin-right: 14px">
                                    <cc2:ValidationTextBox ID="txtQ711" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />
                                    (vụ)
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label40" CssClass="TextLabel" runat="server" Text="- Đã khai báo, điều tra:" />
                                </div>
                                <div class="fr fr1" style="margin-right: 14px">
                                    <cc2:ValidationTextBox ID="txtQ7113" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />
                                    (vụ)
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label85" CssClass="TextLabel" runat="server" Text="- Chưa khai báo, điều tra:" />
                                </div>
                                <div class="fr fr1" style="margin-right: 14px">
                                    <cc2:ValidationTextBox ID="txtQ71118" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />
                                    (vụ)
                                </div>
                            </div>
                            <div class="DivRow width100 fl Group1 TaiNan">
                                <div class="fl fl1">
                                    <asp:Label ID="Label38" CssClass="TextLabel" runat="server" Text="- Số người chết:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7111" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl TaiNan">
                                <div class="fl fl1">
                                    <asp:Label ID="Label39" CssClass="TextLabel" runat="server" Text="- Số người bị thương nặng:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7112" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl TaiNan">
                                <div class="fl fl1">
                                    <asp:Label ID="Label68" CssClass="TextLabel" runat="server" Text="- Số người bị thương nhẹ:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7116" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow  width100 fl Total">
                                <div class="fl fl1">
                                    <asp:Label ID="Label70" CssClass="TextLabel" runat="server" Text="- Tổng số người bị tai nạn:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7118" runat="server" CssClass='TextBox TextBox_short '
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label41" CssClass="TextLabel" runat="server" Text="- Giải quyết chế độ:" />
                                </div>
                                <div class="Child fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7114" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label12" CssClass="TextLabel fl1" runat="server" Text="+ Tiền lương trong thời gian điều trị:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ7119" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label73" CssClass="TextLabel fl1" runat="server" Text="+ Bồi thường, trợ cấp:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ71110" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label74" CssClass="TextLabel fl1" runat="server" Text="+ Bố trí làm việc phù hợp:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ71111" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label75" CssClass="TextLabel fl1" runat="server" Text="+ Thanh toán chi phí Y tế:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ71112" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="- Việc điều tra có xác định rõ nguyên nhân:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ7115" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label69" CssClass="TextLabel" runat="server" Text="- Tổng thiệt hại:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ7117" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(triệu đồng)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col20">
                        <div class="DivRow712">
                            <div class="DivRow DivHeader width100 fl">
                                <asp:Label ID="Label43" CssClass="TextLabel" runat="server" Text="7.12. Đo đạc, kiểm tra môi trường tại nơi làm việc:" />
                            </div>
                            <div class="DivRow TextChild width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label44" CssClass="TextLabel" runat="server" Text="- Năm gần nhất:" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <cc2:ValidationTextBox ID="txtCodeQ7121" runat="server" AssociatedLableText="Nếu không nhập năm là chưa đo MTLĐ"
                                        CssClass='TextBox TextBox_short ToolTip' DataType="integer" />
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label45" CssClass="TextLabel" runat="server" Text="- Số mẫu đã đo:" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <cc2:ValidationTextBox ID="txtQ7122" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl1">
                                    <asp:Label ID="Label46" CssClass="TextLabel" runat="server" Text="Trong đó:" />
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label47" CssClass="TextLabel fl1" runat="server" Text="+ Số mẫu không đạt:" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <cc2:ValidationTextBox ID="txtQ71221" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />
                                </div>
                            </div>
                            <div class="DivRow Group1 width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label48" CssClass="TextLabel fl1" runat="server" Text="+ Các yếu tố độc hại vượt tiêu chuẩn cho phép" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ71222" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow713">
                            <div class="DivRow width100 fl">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label49" CssClass="TextLabel" runat="server" Text="7.13. Các biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ713" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 Group1 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label76" CssClass="TextLabel" runat="server" Text="- Đã thực hiện" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7131" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                </div>
                            </div>
                            <div class="DivRow width100 Group1 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label77" CssClass="TextLabel" runat="server" Text="- Chưa thực hiện" />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7132" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow DivRow714">
                            <div class="DivRow DivHeader fl">
                                <asp:Label ID="Label50" CssClass="TextLabel" runat="server" Text="7.14. Khám sức khỏe định kỳ cho người lao động:" />
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label51" CssClass="TextLabel" runat="server" Text="- Năm gần nhất:" />
                                </div>
                                <div class="Child fr fr1" style="margin-right: 43px">
                                    <cc2:ValidationTextBox ID="txtCodeQ7141" runat="server" AssociatedLableText="Không nhập năm là chưa khám sức khỏe lần nào"
                                        CssClass='TextBox TextBox_short ToolTip' DataType="integer" />
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label53" CssClass="TextLabel" runat="server" Text="- Số người phải khám:" />
                                    <cc2:ValidationTextBox ID="txtQ71421" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                                <div class="Child fr fr1">
                                    <asp:Label ID="Label52" CssClass="TextLabel" runat="server" Text=", chưa được khám: " />
                                    <cc2:ValidationTextBox ID="txtQ71422" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label14a" CssClass="TextLabel" runat="server" Text="- Hồ sơ quản lý sức khỏe:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ7143" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="Child fl fl1">
                                    <asp:Label ID="Label14" CssClass="TextLabel" runat="server" Text="- Khám sức khỏe tuyển dụng với lao động mùa vụ:" />
                                </div>
                                <div class="Child fr fr1">
                                    <asp:CheckBoxList ID="chkQ7144" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text="  Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow DivRow715">
                            <div class="DivRow DivHeader fl">
                                <asp:Label ID="Label54" CssClass="TextLabel" runat="server" Text="7.15. Khám phát hiện bệnh nghề nghiệp hàng năm cho người lao động:" />
                            </div>
                            <div>
                                <div class="DivRow width100 fl TextChild">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label13" CssClass="TextLabel" runat="server" Text="- Được khám:" />
                                    </div>
                                    <div class="fr fr1 ">
                                        <cc2:ValidationTextBox ID="txtQ7151" runat="server" CssClass='TextBox TextBox_short'
                                            DataType="integer" />(người),
                                    </div>
                                </div>
                                <div class="DivRow width100 fl Group1">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label32" CssClass="TextLabel fl1" runat="server" Text="với các loại bệnh(ghi rõ từng loại)" />
                                    </div>
                                    <div class="fr fr1" style="margin-right: 43px">
                                        <asp:TextBox ID="txtQ71511" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both; height: 15px;">
                            </div>
                            <div>
                                <div class="DivRow width100 fl TextChild ">
                                    <div class=" fl fl1">
                                        <asp:Label ID="Label33" CssClass="TextLabel" runat="server" Text="- Số người bệnh nghề nghiệp cộng dồn:" />
                                    </div>
                                    <div class="fr fr1">
                                        <cc2:ValidationTextBox ID="txtQ7152" runat="server" CssClass='TextBox TextBox_short'
                                            DataType="integer" />(người)
                                    </div>
                                </div>
                                <div class="DivRow width100 fl Group1 ">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label64" CssClass="TextLabel fl1" runat="server" Text="từng loại bệnh cụ thể:" />
                                    </div>
                                    <div class="fr fr1" style="margin-right: 43px">
                                        <asp:TextBox ID="txtQ71521" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="lbl2" CssClass="TextLabel" runat="server" Text="Trong đó:" />
                                </div>
                            </div>
                            <div class="DivRow ">
                                <div class="fl fl1">
                                    <asp:Label ID="Label55" CssClass="TextLabel fl1" runat="server" Text="+ Được giám định, điều trị: " />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ715211" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label56" CssClass="TextLabel fl1" runat="server" Text="+ Được cấp sổ: " />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ715212" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label57" CssClass="TextLabel fl1" runat="server" Text="+ Số người được chuyển công việc khác: " />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ715213" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" />(người)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow DivRow716">
                            <div class="DivRow Child">
                                <div class=" DivHeader fl">
                                    <asp:Label ID="Label58" CssClass="TextLabel" runat="server" Text="7.16. Xây dựng phương áp cấp cứu:" />
                                </div>
                                <div class="fr fr1">
                                    <asp:CheckBoxList ID="chkQ716" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow fl Child">
                                <div class="fl fl1">
                                    <asp:Label ID="Label59" CssClass="TextLabel fl1" runat="server" Text="Trang bị phương tiện, túi thuốc cấp cứu:" />
                                </div>
                                <div>
                                    <div class="fr fr1">
                                        <asp:CheckBoxList ID="chkQ7161" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                            <asp:ListItem Value="0" Text=" Không" />
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow DivRow717">
                            <div class="DivRow fl width100">
                                <div class=" DivHeader fl">
                                    <asp:Label ID="Label60" CssClass="TextLabel" runat="server" Text="7.17. Kiểm tra thực tế yếu tố nguy hiểm tại nơi làm việc:" />
                                </div>
                            </div>
                            <div class="Child DivRow fl fl1">
                                <asp:CheckBoxList ID="chkQ717" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                            <div class="DivRow fl fl1">
                                <asp:Label ID="Label61" CssClass="TextLabel" runat="server" Text="Có loại gì:" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7171" Text=" Bộ phận chuyển động không bao che " CssClass="checkbox fl1"
                                    runat="server" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7172" Text=" Thiếu lan can, rào ngăn tại nơi nguy hiểm" CssClass="checkbox fl1"
                                    runat="server" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7173" Text=" Thiếu biển báo nơi nguy hiểm " CssClass="checkbox fl1"
                                    runat="server" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7177" Text=" Thiếu bảng chỉ dẫn an toàn đặt tại nơi làm việc "
                                    CssClass="checkbox fl1" runat="server" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7174" Text=" Hộp cầu dao hở " CssClass="checkbox fl1" runat="server" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7175" Text=" Đấu nối điện không đúng quy cách" CssClass="checkbox fl1"
                                    runat="server" />
                            </div>
                            <div class="DivRow fl1 EffectGroup width100 fl">
                                <asp:CheckBox ID="chkQ7178" Text=" Người lao động không sử dụng phương tiện bảo vệ cá nhân "
                                    CssClass="checkbox fl1" runat="server" />
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label62" CssClass="TextLabel fl1" runat="server" Text="Yếu tố nguy hiểm khác: " />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7176" runat="server" CssClass='TextBox textarea1 ToolTip' AssociatedLableText="Nhập tiếp các yếu tố nguy hiểm khác và cách nhau bằng dấu phẩy"
                                        Height="100px" TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow718">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label63" CssClass="TextLabel" runat="server" Text="7.18. Tự kiểm tra an toàn vệ sinh lao động:" />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ718" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0" Text=" Đúng quy định" style="margin-right: 50px;" />
                                    <asp:ListItem Value="1" Text=" Không đúng" />
                                </asp:CheckBoxList>
                            </div>
                            <div class="DivRow fl1 width100 fl EffectGroup">
                                <asp:CheckBox ID="chkQ7181" Text=" Không thường xuyên " CssClass="checkbox" runat="server" />&nbsp;&nbsp;&nbsp;
                            </div>
                            <div class="DivRow fl1 width100 fl EffectGroup">
                                <asp:CheckBox ID="chkQ7182" Text=" Không ghi sổ kiểm tra " CssClass="checkbox" runat="server" />
                            </div>
                            <div class="DivRow fl1 width100 fl EffectGroup">
                                <asp:CheckBox ID="chkQ7183" Text=" Không tu sửa, nghiệm thu an toàn thiết bị trước khi sử dụng "
                                    CssClass="checkbox" runat="server" />
                            </div>
                            <div class="DivRow fl1 width100 fl EffectGroup">
                                <asp:CheckBox ID="chkQ7185" Text=" Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét "
                                    CssClass="checkbox" runat="server" />
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="DivRow fl fl1">
                                    <asp:Label ID="Label66" CssClass="TextLabel" runat="server" Text="Khác: " />
                                </div>
                                <div class="fr fr1" style="margin-right: 43px">
                                    <asp:TextBox ID="txtQ7184" runat="server" CssClass='TextBox textarea1' AssociatedLableText=""
                                        Height="100px" TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivRow719">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label78" CssClass="TextLabel" runat="server" Text="7.19. Lấy ý kiến đại diện người lao động về công tác an toàn vệ sinh lao động:" />
                            </div>
                            <div class="Child fr fr1">
                                <asp:CheckBoxList ID="chkQ719" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
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
                                CommandArgument="" OnClientClick="javascript:return TestVaild();" />
                            &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                                Text="Làm lại" /></div>
                        <div style="float: right; text-align: right">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
            <div style="display: none">
                <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
                <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
                <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
                <asp:HiddenField ID="hidSoLDLamCVDHNH" Value="0" runat="server" />
                <asp:HiddenField ID="hidTongLaoDong" Value="0" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
