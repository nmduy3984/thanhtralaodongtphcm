<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_GopY_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();

        });
    }
</script>
<asp:UpdatePanel ID="uplYtgchanthuong" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div style="border: 1px solid #ABC1DE; padding: 8px; margin-bottom: 20px; width: 70%">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="Label3" runat="server" Text="GÓP Ý" /></h3>
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        <asp:Label ID="lblDescription" CssClass="TextLabel" runat="server" Text="Nội dung góp ý:" />
                    </div>
                    <div class="DivTextBox">
                        <cc1:ValidationTextBox ID="txtNoiDung" CssClass="TextArea" runat="server" Text=""
                            TextMode="MultiLine" DataType="required" Width="664px" Height="129px"></cc1:ValidationTextBox>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        &nbsp;&nbsp;&nbsp;</div>
                    <div class="DivTextBox">
                        <asp:Button ID="btnSave" CausesValidation="true" CommandArgument="" OnClientClick="return validateFormInputs();"
                            runat="server" Text=" Lưu " CssClass="btn" />
                    </div>
                </div>
            </div>
            <div style="border: 1px solid #ABC1DE; padding: 8px; margin-top: 20px; width: 70%">
                <div class="HeadTitle">
                    <h3>
                        <asp:Label ID="Label1" runat="server" Text="ĐÁNH GIÁ" /></h3>
                </div>
                <div class="DivRow RadioList">
                    <div class="DivLabel">
                        &nbsp;
                    </div>
                    <div class="fl1">
                        <asp:RadioButtonList ID="rdlTieuChi" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Text=" Rất hay " Value="1" />
                            <asp:ListItem Text=" Trung bình " Value="2" style="margin: 0 30px 0 60px;" />
                            <asp:ListItem Text=" Không cần " Value="3" />
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="DivRow" style="padding-top:10px">
                    <div class="DivLabel">
                        <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Kết quả đánh giá:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:Label ID="lblKetQua" CssClass="TextLabel" runat="server" Text="" />
                    </div>
                </div>
                <div class="DivRow" style="padding-top:10px">
                    <div class="DivLabel">
                        <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Số người:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:Label ID="lblSoNguoi" CssClass="TextLabel" runat="server" Text="" />
                    </div>
                </div>
                <div style="height: 20px;">
                </div>
                <div class="DivRow">
                    <div class="DivLabel">
                        &nbsp;&nbsp;&nbsp;</div>
                    <div class="DivTextBox">
                        <asp:Button ID="btnBieuQuyet" CausesValidation="true" runat="server" Text="Biểu quyết"
                            CssClass="btn" />
                    </div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
