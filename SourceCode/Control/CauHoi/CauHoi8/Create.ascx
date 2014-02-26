<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi8_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();

            //xu ly checkbox khi click
            CheckOnlyAndEffect();

            //Kiem tra nam ky ket <= nam hien tai
            //Xu ly nut btnSave
            $("#<%=btnSave.ClientId %>").click(function () {
                if (validateFormInputs() == true) {
                    return TestNamInfo();
                }
                else {
                    return false;
                }
            });

            DiableOrEnableItembyItem82($("#<%=txtQ82.ClientId %>"));
            DiableOrEnableItembyItem823($("#<%=txtQ823.ClientId %>"));
            $("#<%=txtQ82.ClientId %>").blur(function (e) {
                DiableOrEnableItembyItem82($(this));
            });


            $("#<%=txtQ823.ClientId %>").blur(function (e) {
                DiableOrEnableItembyItem823($(this));
            });
            //8.1
            //checkNBH($(".NBH input"));
            //TH:keyup
            //            $(".NBH input").live("keyup", function () {
            //                checkNBH($(".NBH input"));
            //            });
        });
        function checkNBH(obj) {
            var iNBH = obj.val().replace(/,/g, "") * 1;
            //obj.val(obj.val(iNBH).val());
            if (iNBH > 0) {
                obj.parents(".DivRow .NBH").parent().find(".Child input, .Child textarea ").removeAttr("disabled").removeClass("aspNetDisabled");
            }
            else {
                obj.parents(".DivRow .NBH").parent().find(".Child input, .Child textarea ").attr("disabled", true).addClass("aspNetDisabled");
                obj.val('');
            }

        }
        function TestNamInfo() {
            var datecurrent = new Date();
            var year = datecurrent.getFullYear();
            if ($("#<%=txtCodeQ8111.ClientId %>").val() != "" && $("#<%=txtCodeQ8111.ClientId %>").val() > year) {
                AlertboxThenFocus("Vui lòng nhập lại." + "\n" + " Năm ban hành phải nhỏ hơn hoặc bằng năm hiện tại.", $("#<%=txtCodeQ8111.ClientId %>"));
                $("#<%=txtCodeQ8111.ClientID %>").select();
                return false;
            }
            return true;
        }

        

        // Disable hay Enable các mục 8.3 theo giá trị của mục 8.23
        function DiableOrEnableItembyItem823(e) {
            var txtQ241 = $(e).val().replace(/,/g, '') * 1;
            // Disable các mục 8.3 và 8.5
            if (txtQ241 == 0) {
                $(".Group823 input:checkbox").attr("checked", false);
                $(".Group823 input[type=text]").val('');

                $(".Group823 input[type=text]").attr("disabled", true).addClass("aspNetDisabled");
                $(".Group823 input:checkbox").attr("disabled", true).addClass("aspNetDisabled");
            } else {
                $(".Group823 input:checkbox").removeAttr("disabled").removeClass("aspNetDisabled");
                $(".Group823 input[type=text]").removeAttr("disabled").removeClass("aspNetDisabled");
            }
        }
    }
    // Disable hay Enable các mục 8.3 và 8.5 và các mục con của 8.2 theo giá trị của mục 8.21
    function DiableOrEnableItembyItem82(e) {
        var txtQ241 = $(e).val().replace(/,/g, '') * 1;
        // Disable các mục 8.3 và 8.5
        if (txtQ241 == 0) {
            $(".Group82 input:checkbox").attr("checked", false);
            $(".Group82 input[type=text]").val('');
            $(".Group82 textarea").val('');

            $(".Group82 input[type=text]").attr("disabled", true).addClass("aspNetDisabled");
            $(".Group82 textarea").attr("disabled", true).addClass("aspNetDisabled");
            $(".Group82 input:checkbox").attr("disabled", true).addClass("aspNetDisabled");
        } else {
            $(".Group82 input:checkbox").removeAttr("disabled").removeClass("aspNetDisabled");
            $(".Group82 input[type=text]").removeAttr("disabled").removeClass("aspNetDisabled");
            $(".Group82 textarea").removeAttr("disabled").removeClass("aspNetDisabled");
        }
    }
    function TestVaild() {
        if (validateFormInputs()) {
            var txtQ82 = $("#<%=txtQ82.ClientId %>");
            var txtQ821 = $("#<%=txtQ821.ClientId %>");
            var txtQ822 = $("#<%=txtQ822.ClientId %>");
            var txtQ823 = $("#<%=txtQ823.ClientId %>");
            var txtQ824 = $("#<%=txtQ824.ClientId %>");

            var sum = (txtQ821.val().replace(/,/gi, '') * 1 + txtQ822.val().replace(/,/gi, '') * 1 + txtQ823.val().replace(/,/gi, '') * 1 + txtQ824.val().replace(/,/gi, '') * 1);
            if (sum > txtQ82.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Tổng số vụ kỷ luật lao động phải lớn hơn hoặc bằng các vụ chi tiết.", txtQ821);
                txtQ821.select();
                return false;
            }
            return true;
        }
        return false;
    }
</script>
<asp:UpdatePanel ID="uplCauhoi89101112" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" />
                </h3>
            </div>
            <div class="DivRequired" style="font-weight: bold">
                8. Kỷ luật lao động, trách nhiệm vật chất:
            </div>
            <div class="DivRow fl1">
                <div class="col20">
                    <div class="DivQ81">
                        <div class="DivHeader" style="float:left;">
                            <asp:Label ID="lbl1" CssClass="TextLabel" runat="server" Text="8.1. Xây dựng và đăng ký nội quy lao động" />
                        </div>
                        <div class="Child fl1" style="padding-left:72.4%;">
                            <asp:CheckBoxList ID="chkQ81" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                        <div class="DivRow EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label12" CssClass="TextLabel" runat="server" Text="- Năm ban hành:" />
                            </div>
                            <div class="fr fr1" style="margin-right: 43px">
                                <cc2:ValidationTextBox ID="txtCodeQ8111" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" MaxLength="4" MaximumValue="9999" MinimumValue="1900" Range="True"
                                    AssociatedLableText=" Năm ban hành" />
                            </div>
                        </div>
                        <div class="DivRow EffectGroup">
                            <div class="DivRow Child fl1">
                                <asp:CheckBoxList ID="chkQ8112" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Đã đăng ký" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Chưa đăng ký" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="DivRow EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="- Nội dung không phù hợp pháp luật:" />
                            </div>
                            <div class=" fr fr1" style="margin-right: 43px">
                                <asp:TextBox ID="txtQ812" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine"/>
                            </div>
                        </div>
                    </div>
                    <div style="clear: both; height: 15px;">
                    </div>
                    <div class="DivQ82">
                        <div class="DivRow">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="8.2. Tổng số vụ kỷ luật lao động" />
                            </div>
                            <div class="fr fr1" style="margin-right: 14px">
                                <cc2:ValidationTextBox ID="txtQ82" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" />
                                (vụ)
                            </div>
                        </div>
                        <div class="DivRow Group82">
                            <div class="fl fl1">
                                <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="+ Khiển trách:" />
                            </div>
                            <div class="fr fr1" style="margin-right: 14px">
                                <cc2:ValidationTextBox ID="txtQ821" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" />
                                (người)
                            </div>
                        </div>
                        <div class="DivRow Group82">
                            <div class="fl fl1" style="width: 85%;">
                                <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="+ Kéo dài thời hạn nâng lương không quá sáu tháng; cách chức:" />
                            </div>
                            <div class="fr fr1" style="margin-right: 14px">
                                <cc2:ValidationTextBox ID="txtQ822" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" />
                                (người)
                            </div>
                        </div>
                        <div class="DivRow Group82">
                            <div class="fl fl1">
                                <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="+ Sa thải:" />
                            </div>
                            <div class="fr fr1" style="margin-right: 14px">
                                <cc2:ValidationTextBox ID="txtQ823" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" />
                                (người)
                            </div>
                        </div>
                        <div class="DivRow Group82">
                            <div class="fl fl1">
                                <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="+ Khác:" />
                            </div>
                            <div class="fr fr1" style="margin-right: 14px">
                                <cc2:ValidationTextBox ID="txtQ824" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" />
                                (người)
                            </div>
                        </div>
                         <div class="DivRow Group82 ">
                            <div class="fl fl1">
                                <asp:Label ID="Label19" CssClass="TextLabel fl1" runat="server" Text="Hình thức xử lý" />
                            </div>
                            <div class="fr fr1" style="margin-right: 41px">
                                <asp:TextBox ID="txtQ8241" runat="server" CssClass='TextBox textarea1'
                                    DataType="none" TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col20">
                    <div class="DivQ83">
                        <div class="DivRow Group82 Group823">
                            <div class="fl DivHeader"  >
                                <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="8.3. Báo cáo với Sở LĐTBXH sau khi sa thải người lao động:" />
                            </div>
                            <div class="Child fl fl1"  >
                                <asp:CheckBoxList ID="chkQ83" CssClass="checkbox" runat="server" RepeatDirection="Horizontal"
                                    Width="200px">
                                    <asp:ListItem Value="1"> Có </asp:ListItem>
                                    <asp:ListItem Value="0"> Không </asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="DivQ84">
                        <div class="DivRow">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label9" CssClass="TextLabel" runat="server" Text="8.4. Số vụ bồi thường trách nhiệm vật chất" />
                            </div>
                            <div class="fr" style="margin-right: 0px">
                                <cc2:ValidationTextBox ID="txtQ84" runat="server" CssClass='TextBox TextBox_short'
                                    DataType="integer" />
                                (vụ)
                            </div>
                        </div>
                    </div>
                    <div class="DivQ85">
                        <div class="DivRow">
                            <div class="DivHeader">
                                <asp:Label ID="Label10" CssClass="TextLabel" runat="server" Text="8.5. Xử lý kỷ luật lao động, trách nhiệm vật chất" />
                            </div>
                        </div>
                        <div class="Child DivRow ">
                            <div class="fl1">
                                <asp:CheckBoxList ID="chkQ851" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text=" Đúng quy trình" style="margin-right: 50px;" />
                                    <asp:ListItem Value="0" Text=" Sai quy trình" />
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="DivRow fl1">
                            <asp:Label ID="Label11" CssClass="TextLabel" runat="server" Text="Sai gì:" />
                        </div>
                        <div class="DivRow  EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label14" CssClass="TextLabel fl1" runat="server" Text="+ Không đúng thẩm quyền" />
                            </div>
                            <div class="fr fr1">
                                <asp:CheckBox ID="chkQ8511" Text="" CssClass="checkbox" runat="server" />
                            </div>
                        </div>
                        <div class="DivRow  EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label15" CssClass="TextLabel fl1" runat="server" Text="+ Quá thời hiệu" />
                            </div>
                            <div class="fr fr1">
                                <asp:CheckBox ID="chkQ8512" CssClass="checkbox" runat="server" />
                            </div>
                        </div>
                        
                        <div class="DivRow  EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label17" CssClass="TextLabel fl1" runat="server" Text="+ Không chứng minh được lỗi của người lao động" />
                            </div>
                            <div class="fr fr1">
                                <asp:CheckBox ID="chkQ8514" Text="" CssClass="checkbox" runat="server" />
                            </div>
                        </div>
                        <div class="DivRow  EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label18" CssClass="TextLabel fl1" runat="server" Text="+ Họp xét kỷ luật không ghi biên bản họp" />
                            </div>
                            <div class="fr fr1">
                                <asp:CheckBox ID="chkQ8515" Text="" CssClass="checkbox" runat="server" />
                            </div>
                        </div>
                        <div class="DivRow  EffectGroup">
                            <div class="fl fl1">
                                <asp:Label ID="Label13" CssClass="TextLabel fl1" runat="server" Text="+ Sai khác" />
                            </div>
                            <div class="fr fr1">
                                <asp:TextBox ID="txtQ8516" runat="server" CssClass='TextBox textarea1'
                                    DataType="none" TextMode="MultiLine" />
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
                            CommandArgument=""/>
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
    </ContentTemplate>
</asp:UpdatePanel>
