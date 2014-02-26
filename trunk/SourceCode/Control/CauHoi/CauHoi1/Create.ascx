<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi1_Create" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();
            //xu ly checkbox khi click
            CheckOnlyAndEffect();
            $("#generate").live('keypress', function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnSave.ClientId %>").click();
                }
                return false;
            });
        });
    }
</script>
<asp:UpdatePanel ID="uplCauhoi1" runat="server">
    <ContentTemplate>
        <div id="generate">
            <div class="BoxField">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
                </div>
                <div class="DivRequired" style="font-weight: bold">
                    1. Các loại báo cáo định kỳ
                </div>
                <div class="DivQ11">
                    <div class="DivRow">
                        <div class="">
                            <asp:Label ID="lblTendoanhnghiep" CssClass="TextLabel" runat="server" Text="1.1. Khai trình, báo cáo định kỳ về tuyển dụng, sử dụng lao động với cơ quan quản lý nhà nước về lao động địa phương:" />
                        </div>
                        <div class="Child">
                            <asp:CheckBoxList ID="chkQ11" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="DivQ12">
                    <div class="DivRow">
                        <div class="">
                            <asp:Label ID="lblLoaihinhdnid" CssClass="TextLabel" runat="server" Text="1.2. Báo cáo định kỳ về công tác an toàn vệ sinh lao động với cơ quan quản lý nhà nước về lao động địa phương:" />
                        </div>
                        <div class="Child">
                            <asp:CheckBoxList ID="chkQ12" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right: 50px;" />
                                <asp:ListItem Value="0" Text=" Không" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="DivQ13">
                    <div class="DivRow">
                        <div class="">
                            <asp:Label ID="lblTonggiatrisp" CssClass="TextLabel" runat="server" Text="1.3. Báo cáo định kỳ về tai nạn lao động với cơ quan quản lý nhà nước về lao động địa phương:" />
                        </div>
                        <div class="Child">
                            <asp:CheckBoxList ID="chkQ13" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
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
                            CommandArgument="" />
                        &nbsp; &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Làm lại" /></div>
                    <div style="float: right; text-align: right">
                        &nbsp;<%--<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                        ImageUrl="~/images/back.png" />--%></div>
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
