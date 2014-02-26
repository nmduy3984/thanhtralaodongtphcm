<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi3_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();

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
            //xu ly checkbox khi click
            CheckOnlyAndEffect();

            //Kiem tra co cong doan hay khong?neu co thi enable cac control, neu khong thi disable
            var isCongDoan = (/^true$/gi).test($("#<% =IsCD.ClientID %>").val());
            if (isCongDoan) {
                // Enable các mục cauhoi3
                $(".BoxField input, .BoxField textarea").removeAttr("disabled").removeClass("aspNetDisabled");
                $(".btn input").removeAttr("enabled");
            } else {
                // Disable các mục cauhoi3
                $(".BoxField input, .BoxField textarea").attr("disabled", true).addClass("aspNetDisabled");
                $(".btn input").attr("enabled", true);
            }

            //Nếu không có năm ký kết thì disable tất cả
            //TH: Load page
            checkYear($("#<%=txtCodeQ31.ClientId %>"));
            //TH:keyup
            $("#<%=txtCodeQ31.ClientId %>").live("keyup", function () {
                checkYear($("#<%=txtCodeQ31.ClientId %>"));
            });
        });
        function checkYear(obj) {
            var iYear = obj.val() * 1;
            if (iYear > 0) {
                $(".DivSum input").removeAttr("disabled").removeClass("aspNetDisabled");
                $(".DivSum textarea").removeAttr("disabled").removeClass("aspNetDisabled");
            }
            else {
                $(".DivSum input").attr("disabled", true).addClass("aspNetDisabled");
                $(".DivSum textarea").attr("disabled", true).addClass("aspNetDisabled");
            }

        }
        function TestNamInfo() {
            var datecurrent = new Date();
            var year = datecurrent.getFullYear();
            if ($("#<%=txtCodeQ31.ClientId %>").val() != "" && $("#<%=txtCodeQ31.ClientId %>").val() > year) {
                AlertboxThenFocus("Vui lòng nhập lại." + "\n" + " Năm ký kết phải nhỏ hơn hoặc bằng năm hiện tại.", $("#<%=txtCodeQ31.ClientId %>"));
                $("#<%=txtCodeQ31.ClientID %>").select();
                return false;
            }
            return true;
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
                    3. Thỏa ước lao động tập thể:
                </div>
                <div class="DivRow fl">
                    <div class="col20">
                        <div class="DivQ3">
                            <div class="DivRow width100 fl">
                                <div class="fl fl1">
                                    <asp:Label ID="Label29" CssClass="TextLabel" runat="server" Text="Năm ký kết:" />
                                </div>
                                <div class="fr fr1">
                                    <cc2:ValidationTextBox ID="txtCodeQ31" runat="server" CssClass='TextBox TextBox_short Group1'
                                        DataType="integer" AssociatedLableText="Năm ký kết" MaximumValue="9999" MaxLength="4"
                                        MinimumValue="1900" Range="True" />
                                </div>
                            </div>
                            <div class="DivSum">
                                <div class="DivRow31">
                                    <div class="DivHeader fl1">
                                        <asp:Label ID="Label25" CssClass="TextLabel" runat="server" Text="3.1. Quy trình thương lượng tập thể:" />
                                    </div>
                                    <div class="Child fl1" style="padding-left: 64%">
                                        <asp:CheckBoxList ID="chkQ34" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Text=" Đúng" style="margin-right: 50px;" />
                                            <asp:ListItem Value="1" Text=" Không đúng" />
                                        </asp:CheckBoxList>
                                    </div>
                                    <div class="DivRow Effect EffectGroup">
                                        <div class="fl fl1">
                                            <asp:Label ID="Label28" CssClass="TextLabel fl1" runat="server" Text="Các bước thương lượng không đúng:" />
                                        </div>
                                        <div class="fr fr1">
                                            <asp:TextBox ID="txtQ341" runat="server" CssClass='TextBox textarea1' Height="100px"
                                                TextMode="MultiLine" />
                                        </div>
                                    </div>
                                </div>
                                <div class="DivRow">
                                    <div class="DivHeader fl fl1">
                                        <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="3.2. Gửi đến cơ quan quản lý nhà nước về lao động địa phương:" />
                                    </div>
                                    <div class="Child fr fr1">
                                        <asp:CheckBoxList ID="chkQ32" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text=" Đã gửi CQQL" style="margin-right: 50px;" />
                                            <asp:ListItem Value="0" Text=" Chưa gửi CQQL" />
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="DivRow">
                                    <div class="DivHeader fl fl1">
                                        <asp:Label ID="Label30" CssClass="TextLabel" runat="server" Text="3.3. Nội dung thỏa ước không phù hợp pháp luật:" />
                                    </div>
                                    <div class="fr fr1">
                                        <asp:TextBox ID="txtQ33" runat="server" CssClass='TextBox Group1 textarea1' Height="100px"
                                            TextMode="MultiLine" />
                                    </div>
                                </div>
                                <div class="DivRow">
                                    <div class="DivHeader fl1">
                                        <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="3.4. Thực hiện các nội dung thỏa ước lao động tập thể đã ký:" />
                                    </div>
                                    <div class="Child fl1" style="padding-left: 61%">
                                        <asp:CheckBoxList ID="chkQ35" CssClass="checkbox case1" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Text=" Đầy đủ" style="margin-right: 50px;" />
                                            <asp:ListItem Value="1" Text=" Không đầy đủ" />
                                        </asp:CheckBoxList>
                                    </div>
                                    <div class="DivRow Effect EffectGroup">
                                        <div class="fl fl1">
                                            <asp:Label ID="Label4" CssClass="TextLabel fl1" runat="server" Text="Nội dung chưa hiện:" />
                                        </div>
                                        <div class="fr fr1">
                                            <asp:TextBox ID="txtQ351" runat="server" CssClass='TextBox textarea1' Height="100px"
                                                TextMode="MultiLine" />
                                        </div>
                                    </div>
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
                            <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn Group1" CausesValidation="true"
                                CommandArgument="" />
                            &nbsp; &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn Group1"
                                Text="Làm lại" /></div>
                        <div style="float: right; text-align: right">
                            &nbsp;<%--<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" />--%></div>
                    </div>
                </div>
            </div>
            <div style="display: none">
                <asp:HiddenField ID="IsCD" Value="0" runat="server" />
                <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
                <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
                <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
