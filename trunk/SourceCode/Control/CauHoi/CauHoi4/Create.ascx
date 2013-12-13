<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi4_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();

            //xu ly checkbox khi click
            CheckMuc24();
            CheckOnlyAndEffect();
            //Validate 4.4 & 4.5
            $("#<%=rdlQ44.ClientId %>").click(function () {
                var value = $("#<%=rdlQ44.ClientId %> :checked").val();
                if (value == '3') {
                    $(".DivQ45 input").attr("disabled", true);
                }
                else {
                    $(".DivQ45 input").removeAttr("disabled");
                }
            });

            //Validate 4.2/4.6
            //Validate4246();
            $("#<%=chkQ421.ClientID %> ").click(function () {
                Validate4246();
            });
            $("#<%=chkQ422.ClientID %> ").click(function () {
                Validate4246();
            });

            //4.10.4 Nếu chkQ4104 check đủ/không thì disable txtQ41041. Ngược lại  
            CheckQ4104($("#<%=chkQ4104.ClientId %> input:checkbox:checked "));
            $("#<%=chkQ4104.ClientId %> input:checkbox:checked ").live("click", function () {
                CheckQ4104($(this));
            });
        });
        function CheckQ4104(e) {
            if ($(e).val() * 1 == 2) {
                $("#<%=txtQ41041.ClientId %>").removeAttr("disabled").removeClass("aspNetDisabled");
            }
            else {
                $("#<%=txtQ41041.ClientId %>").attr("disabled", true).addClass("aspNetDisabled");
            }
        }
        function CheckMuc24() {
            var hidTroCapMatViec = $("#<%=hidTroCapMatViec.ClientId %>").val();
            if (hidTroCapMatViec == 0) {
                $(".DivQ4102").find("input,textarea").attr("disabled", true);
            }
            else {
                $(".DivQ4102").find("input,textarea").removeAttr("disabled");
            }
        }
        //TestIsDNNN();
    }
    function Validate4246() {
        //Xử lý checkbox lương thời gian
        var chkQ421 = $("#<%=chkQ421.ClientID %> ");
        if (chkQ421.is(":checked") == true) {
            $("#<%=chkQ46a.ClientId %>").removeAttr("disabled").attr("checked", true);
        }
        else {
            $("#<%=chkQ46a.ClientId %>").attr("disabled", true).attr("checked", false);
        }
        //Xử lý checkbox lương sản phẩm
        var chkQ422 = $("#<%=chkQ422.ClientID %> ");
        if (chkQ422.is(":checked") == true) {
            $("#<%=chkQ46b.ClientId %>").removeAttr("disabled").attr("checked", true);
        }
        else {
            $("#<%=chkQ46b.ClientId %>").attr("disabled", true).attr("checked", false);
        }
        //CheckValidate cac textbox 4.6
        if (chkQ421.is(":checked") == true || chkQ422.is(":checked") == true) {
            $(".DivQ46 input[type='text']").removeAttr("disabled").removeClass("aspNetDisabled");
        }
        else {
            $(".DivQ46 input[type='text']").attr("disabled", true).addClass("aspNetDisabled");
        }
    }
    //DNNN van de binh thuong nghiavt update 17/08/2013
//    function TestIsDNNN() {
//        var hidIsDNNN = $("#<%=hidIsDNNN.ClientId %>").val();
//        if (hidIsDNNN == 2) {
//            $(".DivQ44").find("input,textarea").attr("disabled", true);
//            $(".DivQ45").find("input,textarea").attr("disabled", true);
//        }
//    }

    function TestVaild() {
        var txtQ41021 = $("#<%=txtQ41021.ClientId %>");
        var txtQ41022 = $("#<%=txtQ41022.ClientId %>");
        var txtQ410311 = $("#<%=txtQ410311.ClientId %>");
        var txtQ410312 = $("#<%=txtQ410312.ClientId %>");

        if ($("#<%=chkQ4102.ClientId  %> " + ":checkbox").is(':checked') && $("#<%=chkQ4102.ClientId  %> " + ":checked").val().replace(/,/gi, '') * 1 == 1 && (txtQ41022.val().replace(/,/gi, '') * 1 > txtQ41021.val().replace(/,/gi, '') * 1)) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số lao động mất việc, đủ điều kiện hưởng phải lớn hơn hoặc bằng số đã trả.", txtQ41022);
            txtQ41022.select();
            return false;
        }
        if ($("#<%=chkQ4103.ClientId  %> " + ":checkbox").is(':checked') && $("#<%=chkQ4103.ClientId  %> " + ":checked").val().replace(/,/gi, '') * 1 == 1 && (txtQ410312.val().replace(/,/gi, '') * 1 > txtQ410311.val().replace(/,/gi, '') * 1)) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Số lao động mất thôi việc đủ điều kiện hưởng phải lớn hơn hoặc bằng số đã trả.", txtQ410312);
            txtQ410312.select();
            return false;
        }
        //Mục 4.1 - Nợ lương của người lao động
        var chkQ4113 = $("#<%=chkQ4113.ClientId %> " + ":checked");
        var txtQ41131 = $("#<%=txtQ41131.ClientId %>");
        if (chkQ4113.val().replace(/,/gi, '') * 1 == 1 && (txtQ41131.val().replace(/,/gi, '') * 1 == 0)) {
            AlertboxThenFocus("Vui lòng nhập số tháng nợ lương chưa trả cho người lao động.", txtQ41131);
            txtQ41131.select();
            return false;
        }
        return true;
    }
</script>
<asp:UpdatePanel ID="uplCauhoi4" runat="server">
    <ContentTemplate>
        <div id="generate">
            <div class="BoxField">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
                </div>
                <div class="DivRequired" style="font-weight: bold">
                    4. Tiền lương:
                </div>
                <div class="DivRow fl fl1">
                    <div class="col20">
                        <div class="DivQ41 width100 fl">
                            <div class="DivRow">
                                <div class="fl DivHeader">
                                    <asp:Label ID="Label20" CssClass="TextLabel" runat="server" Text="4.1. Mức lương:" />
                                </div>
                            </div>
                        </div>
                        <div class="DivQ41 width100 fl">
                            <div class="DivRow">
                                <div class="fl ">
                                    <asp:Label ID="lbl1" CssClass="TextLabel" runat="server" Text="- Mức lương tối thiểu đang áp dụng:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ4111" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Mức lương tối thiểu" />(đồng)
                                </div>
                            </div>
                        </div>
                        <div class="DivQ41 width100 fl">
                            <div class="DivRow">
                                <div class="fl ">
                                    <asp:Label ID="Label29" CssClass="TextLabel" runat="server" Text="- Mức lương thấp nhất với lao động đã qua đào tạo:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ4114" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText="Mức lương tối thiểu" />(% LTT)
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                         <div class="DivQ41 width100 fl">
                            <div class="DivRow">
                                <div class="fl ">
                                    <asp:Label ID="Label35" CssClass="TextLabel" runat="server" Text="- Mức tiền lương trung bình/ngày:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ4116" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer"  />(đồng)
                                </div>
                            </div>
                        </div>
                        <div class="DivQ41 width100 fl">
                            <div class="DivRow">
                                <div class="fl ">
                                    <asp:Label ID="Label19" CssClass="TextLabel" runat="server" Text="- Thu nhập trung bình/người/tháng:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ4112" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" AssociatedLableText=" " />(đồng)
                                </div>
                            </div>
                        </div>
                        <div class="DivQ11 width100 fl">
                            <div class="DivRow Child">
                                <div class="" style="float: left;">
                                    <asp:Label ID="Label17" CssClass="TextLabel" runat="server" Text="- Nợ lương của người lao động:" />
                                </div>
                                <div class="fl1" style="padding-left: 64%;">
                                    <asp:CheckBoxList ID="chkQ4113" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label18" CssClass="TextLabel " runat="server" Text="+ Số tháng chưa trả:" />
                                </div>
                                <div class="fr" style="margin-right: 22px;">
                                    <cc2:ValidationTextBox ID="txtQ41131" runat="server" CssClass='TextBox TextBox_short '
                                        DataType="integer" AssociatedLableText="Số trường hợp đã phạt" />
                                    <span>(tháng)</span>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ42  width100 fl">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="lbl2" CssClass="TextLabel" runat="server" Text="4.2. Hình thức trả lương:" />
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="- Lương thời gian" />
                                </div>
                                <div class="fr fr1">
                                    <asp:CheckBox ID="chkQ421" Text="" CssClass="checkbox PaddingRight" runat="server" />
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="- Lương sản phẩm" />
                                </div>
                                <div class="fr fr1">
                                    <asp:CheckBox ID="chkQ422" Text="" CssClass="checkbox PaddingRight" runat="server" />
                                </div>
                            </div>
                            <div class="DivRow">
                                <div class="fl fl1">
                                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="- Lương khoán " />
                                </div>
                                <div class="fr fr1">
                                    <asp:CheckBox ID="chkQ423" Text="" CssClass="checkbox PaddingRight" runat="server" />
                                </div>
                            </div>
                            <div class="DivRowQ424">
                                <div class="DivRow">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label33" CssClass="TextLabel" runat="server" Text="- Thỏa thuận trách nhiệm người lao động phải trả phí mở, duy trì tài khoản nhận lương qua ATM:" />
                                    </div>
                                </div>
                                <div class="DivRow fl1">
                                    <div class="RadioList">
                                        <asp:RadioButtonList ID="rdlQ424" runat="server" RepeatDirection="Horizontal" CssClass="ToolTip"
                                            ToolTip="Chỉ xét khi người lao động phải trả phí">
                                            <asp:ListItem Selected="true" Text=" Có " Value="1" />
                                            <asp:ListItem Text=" Không " Value="2" style="margin: 0 10px 0 30px;" />
                                            <asp:ListItem Text=" Doanh nghiệp trả phí " Value="3" style="margin: 0 17px 0 19px;" />
                                            <asp:ListItem Text=" Trả tiền mặt " Value="4" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ43 width100 fl">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="4.3. Xây dựng định mức lao động:" />
                            </div>
                            <div class="Child fl1">
                                <asp:CheckBoxList ID="chkQ43" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ44">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="4.4. Áp dụng thang lương:" />
                            </div>
                            <div class="DivRow RadioList">
                                <div class="fl1">
                                    <asp:RadioButtonList ID="rdlQ44" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text=" Của nhà nước " Value="1" />
                                        <asp:ListItem Text=" Tự xây dựng " Value="2" style="margin: 0 30px 0 60px;" />
                                        <asp:ListItem Selected="True" Text=" Chưa xây dựng " Value="3" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ45 width100 fl">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label10" CssClass="TextLabel" runat="server" Text="4.5. Gửi thang bảng lương đến cơ quan quản lý nhà nước về lao động địa phương" />
                            </div>
                            <div class="Child fl1">
                                <asp:CheckBoxList ID="chkQ45" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ46">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label11" CssClass="TextLabel" runat="server" Text="4.6. Mức trả lương làm thêm giờ theo:" />
                            </div>
                            <div class="DivRow fl1">
                                <asp:CheckBox ID="chkQ46a" runat="server" CssClass="checkbox PaddingRight" Text=" Lương thời gian" />
                                <asp:CheckBox ID="chkQ46b" runat="server" CssClass="checkbox PaddingRight" Text=" Lương sản phẩm" />
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label12" CssClass="TextLabel" runat="server" Text="+ Làm thêm giờ vào ngày thường:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ461" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" Range="False" AssociatedLableText="Ngày thường" MaximumValue=""
                                        MinimumValue="" MaxLength="4" />
                                    %</div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label31" CssClass="TextLabel" runat="server" Text="+ Làm thêm giờ vào ngày nghỉ hàng tuần:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ462" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" Range="False" AssociatedLableText="Ngày nghỉ hàng tuần" MaximumValue=""
                                        MinimumValue="" MaxLength="4" />
                                    %
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label32" CssClass="TextLabel" runat="server" Text="+ Làm thêm giờ vào ngày lễ, tết:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ463" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" Range="False" AssociatedLableText="Ngày lễ, tết" MaximumValue=""
                                        MinimumValue="" MaxLength="4" />
                                    %
                                </div>
                            </div>
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="+ Trả thêm tiền làm thêm giờ vào ban đêm:" />
                                </div>
                                <div class="fr fl" style="padding-right: 30px;">
                                    <cc2:ValidationTextBox ID="txtQ464" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" Range="False" AssociatedLableText="Làm thêm giờ vào ban đêm"
                                        MaximumValue="" MinimumValue="" MaxLength="4" />
                                    %
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ47">
                            <div class="DivRow">
                                <div class="DivHeader fl">
                                    <asp:Label ID="Label13" CssClass="TextLabel" runat="server" Text="4.7. Mức trả lương làm đêm:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtQ47" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" Range="False" AssociatedLableText="Mức trả lương làm đêm"
                                        MaximumValue="" MinimumValue="" MaxLength="4" />
                                    %
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col20">
                        <div class="DivQ48 width100 fl">
                            <div class="DivRow Child">
                                <div class="DivHeader">
                                    <asp:Label ID="Label15" CssClass="TextLabel" runat="server" Text="4.8. Phạt tiền, phạt trừ lương:" />
                                </div>
                                <div class="fl1">
                                    <asp:CheckBoxList ID="chkQ48" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label16" CssClass="TextLabel " runat="server" Text="- Nếu có, đã phạt:" />
                                </div>
                                <div class="fr" style="margin-right: -23px;">
                                    <cc2:ValidationTextBox ID="txtQ481" runat="server" CssClass='TextBox TextBox_short '
                                        DataType="integer" AssociatedLableText="Số trường hợp đã phạt" />
                                    <span>trường hợp</span>
                                </div>
                            </div>
                            <div class="DivRow EffectGroup width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label16a" CssClass="TextLabel " runat="server" Text="- Tổng số tiền đã phạt:" />
                                </div>
                                <div class="fr" style="margin-right: 2px;">
                                    <cc2:ValidationTextBox ID="txtQ482" runat="server" CssClass='TextBox TextBox_short '
                                        DataType="integer" AssociatedLableText="Nhập số tiền" />
                                    <span>(đồng)</span>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ49">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="4.9. Công khai thang lương, bảng lương và  quy chế thưởng" />
                            </div>
                            <div class="Child fl1">
                                <asp:CheckBoxList ID="chkQ49" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ410">
                            <div class="DivRow DivHeader">
                                <asp:Label ID="Label9" CssClass="TextLabel" runat="server" Text="4.10. Trả lương chế độ theo:" /><i
                                    style="color: Red; font-weight: normal;">(Bạn phải chọn 1 trong 3 loại lương)</i>
                            </div>
                            <div class="fl1" style="padding: 0% 0% 1% 10.8%;">
                                <%--<asp:ListBox ID="lbQ410" ToolTip="Phải lựa chọn 1 trong 3 loại lương" runat="server"
                                Width="115px">
                                <asp:ListItem Value="1">Lương tối thiểu</asp:ListItem>
                                <asp:ListItem Value="2">Lương cơ bản</asp:ListItem>
                                <asp:ListItem Value="3">Lương thực lĩnh</asp:ListItem>
                            </asp:ListBox>--%>
                                <cc1:ValidationDropdownlist ID="ddlQ410" runat="server" CssClass='DropDownList ToolTip'
                                    AssociatedLableText="Phải lựa chọn 1 trong 3 loại lương" DataType="none">
                                    <asp:ListItem Value="0">--- Chọn ---</asp:ListItem>
                                    <asp:ListItem Value="1">Lương tối thiểu</asp:ListItem>
                                    <asp:ListItem Value="2">Lương cơ bản</asp:ListItem>
                                    <asp:ListItem Value="3">Lương thực lĩnh</asp:ListItem>
                                </cc1:ValidationDropdownlist>
                            </div>
                            <div class="DivQ4101 Effect fl width100">
                                <div class="DivHeader fl1 fl">
                                    <asp:Label ID="Label14" CssClass="TextLabel" runat="server" Text="4.10.1 Trả lương ngừng việc:" />
                                </div>
                                <div class="fr" style="margin-right: -10px">
                                    bằng
                                    <cc2:ValidationTextBox ID="txtQ4101" runat="server" CssClass='TextBox TextBox_short'
                                        DataType="integer" Range="False" AssociatedLableText="Trả lương ngừng việc" MaximumValue=""
                                        MinimumValue="" MaxLength="4" />
                                    % lương
                                </div>
                            </div>
                            <div class="DivRow  Effect width100 fl ">
                                <div class="fl fl1">
                                    <asp:Label ID="Label30" CssClass="TextLabel fl1" runat="server" Text="- Mức lương thử việc:" />
                                </div>
                                <div class="fr" style="padding-right: 29px">
                                    <cc2:ValidationTextBox ID="txtQ41011" runat="server" CssClass='TextBox TextBox_short '
                                        DataType="integer" AssociatedLableText="Nhập số" />%
                                </div>
                            </div>
                            <div class="DivQ4102 ">
                                <div class="DivRow DivHeader fl1">
                                    <asp:Label ID="Label21" CssClass="TextLabel" runat="server" Text="4.10.2 Trợ cấp mất việc:" />
                                </div>
                                <div class="Child fl1">
                                    <asp:CheckBoxList ID="chkQ4102" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="DivRow  Effect width100 fl ">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label22" CssClass="TextLabel fl1" runat="server" Text="- Số lao động mất việc, đủ điều kiện hưởng:" />
                                    </div>
                                    <div class="fr">
                                        <cc2:ValidationTextBox ID="txtQ41021" runat="server" CssClass='TextBox TextBox_short GroupQ411 GroupMatViec'
                                            DataType="integer" AssociatedLableText="Nhập số" />(người)
                                    </div>
                                </div>
                                <div class="DivRow  Effect width100 fl EffectGroup">
                                    <div class="fl fl1">
                                        <asp:Label ID="Label222" CssClass="TextLabel fl1" runat="server" Text="- đã trả:" />
                                    </div>
                                    <div class="fr">
                                        <cc2:ValidationTextBox ID="txtQ41022" runat="server" CssClass='TextBox TextBox_short GroupQ411 GroupMatViec'
                                            DataType="integer" AssociatedLableText="Nhập số" />(người)
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both; height: 15px;">
                        </div>
                        <div class="DivQ4103">
                            <div class="DivRow fl1 DivHeader">
                                <asp:Label ID="Label24" CssClass="TextLabel" runat="server" Text="4.10.3 Trợ cấp thôi việc:" />
                            </div>
                            <div class="Child fl1">
                                <asp:CheckBoxList ID="chkQ4103" CssClass="checkbox fl1 case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" />
                                </asp:CheckBoxList>
                            </div>
                            <div class="DivRow  Effect width100 fl ">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label25" CssClass="TextLabel fl1" runat="server" Text="- Số lao động thôi việc đủ điều kiện hưởng:" />
                                </div>
                                <div class="fr">
                                    <cc2:ValidationTextBox ID="txtQ410311" runat="server" CssClass='TextBox  TextBox_short GroupQ412 GroupMatViec'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow Effect width100 fl EffectGroup">
                                <div class=" fl fl1">
                                    <asp:Label ID="Label26" CssClass="TextLabel fl1" runat="server" Text="- đã trả:" />
                                </div>
                                <div class="fr">
                                    <cc2:ValidationTextBox ID="txtQ410312" runat="server" CssClass='TextBox TextBox_short GroupQ412 GroupMatViec'
                                        DataType="integer" AssociatedLableText="Nhập số" />(người)
                                </div>
                            </div>
                            <div class="DivRow fl1  width100 fl EffectGroup">
                                <div class="fl fl1">
                                    <asp:Label ID="Label27" CssClass="TextLabel" runat="server" Text="- Tính số tháng lẻ." />
                                </div>
                                <div class="Child fr">
                                    <asp:CheckBoxList ID="chkQ41032" CssClass="checkbox fl1" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                        <asp:ListItem Value="0" Text=" Không" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="DivRow Effect EffectGroup">
                                <div class="fl fl1">
                                    <asp:Label ID="Label28" CssClass="TextLabel fl1" runat="server" Text="- Lý do chưa trả:" />
                                </div>
                                <div class="fr PaddingRight">
                                    <asp:TextBox ID="txtQ41033" runat="server" CssClass='TextBox GroupQ412 GroupMatViec textarea1'
                                        Height="100px" TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <div class="DivQ4104">
                            <div class="DivRow fl1 DivHeader">
                                <asp:Label ID="Label23" CssClass="TextLabel" runat="server" Text="4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:" />
                            </div>
                            <div class="Child fl1">
                                <asp:CheckBoxList ID="chkQ4104" CssClass="checkbox fl1 case1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không" style="margin-right: 50px;" />
                                    <asp:ListItem Value="2" Text=" Không đầy đủ" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="DivRow Effect EffectGroup">
                                <div class="fl fl1">
                                    <asp:Label ID="Label3" CssClass="TextLabel fl1" runat="server" Text="- Chưa thực hiện với các nghề:" />
                                </div>
                                <div class="fr PaddingRight">
                                    <asp:TextBox ID="txtQ41041" runat="server" CssClass='TextBox  textarea1'
                                        Height="100px" TextMode="MultiLine" />
                                </div>
                            </div>
                        <div class="DivQ4105">
                            <div class="DivRow fl1 DivHeader">
                                <asp:Label ID="Label34" CssClass="TextLabel" runat="server" Text="4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương:" />
                            </div>
                            <div class="Child fl1">
                                <asp:CheckBoxList ID="chkQ4105" CssClass="checkbox fl1 case1" runat="server"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Không"  style="margin-right: 50px;" />
                                    <asp:ListItem Value="2" Text=" Không trả với người hưởng lương sản phẩm" />
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
                            &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="true" CssClass="btn"
                                Text="Làm lại" /></div>
                        <div style="float: right; text-align: right">
                            &nbsp;<%--<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" />--%></div>
                    </div>
                </div>
            </div>
            <div style="display: none">
                <asp:HiddenField ID="hidIsDNNN" Value="0" runat="server" />
                <asp:HiddenField ID="hidTroCapMatViec" Value="0" runat="server" />
                <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
                <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
                <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
                <asp:HiddenField ID="hidlbQ46" Value="" runat="server" />
                <asp:HiddenField ID="hidlbQ410" Value="" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
