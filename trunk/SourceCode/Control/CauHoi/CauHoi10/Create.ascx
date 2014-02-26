<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi10_Create" %>
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
<asp:UpdatePanel ID="uplCauhoi10" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
            </div>
            <div class="DivRequired" style="font-weight: bold">
            </div>
            <div class="DivRow fl1 width100 fl">
                <div style="width: 65%">
                    <div class="DivRow DivHeader">
                        <asp:Label ID="Label22" CssClass="TextLabel" runat="server" Text="10. Lao động là người nước ngoài:" />
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="fl fl1">
                            <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="+ Tổng số lao động là người nước ngoài" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ105" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" />(người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="fl fl1">
                            <asp:Label ID="Label41" CssClass="TextLabel" runat="server" Text="+ Số người phải có giấy phép lao động" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ101" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" />(người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl">
                        <div class="fl fl1">
                            <asp:Label ID="Label42" CssClass="TextLabel" runat="server" Text=" + Số người chưa được cấp giấy phép lao động" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ102" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" />(người)
                        </div>
                    </div>
                    <div class="DivRow width100 fl" style="display:none">
                        <div class="fl fl1">
                            <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text=" + Số người chưa được gia hạn giấy phép lao động" />
                        </div>
                        <div class="fr fr1">
                            <cc2:ValidationTextBox ID="txtQ106" runat="server" CssClass='TextBox TextBox_short'
                                DataType="integer" />(người)
                        </div>
                    </div>
                    
                    <div class="DivRow width100 fl">
                        <div class="fl fl1" style="width: 60%">
                            <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="+ Thực hiện chế độ báo cáo định kỳ sáu tháng và hằng năm về tình hình sử dụng người lao động nước ngoài với cơ quan quản lý về lao động tại địa phương." />
                        </div>
                        <div class="Child fr fr1">
                            <asp:CheckBoxList ID="chkQ104" CssClass="checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text=" Có" style="margin-right:50px;"/> 
                                <asp:ListItem Value="0" Text=" Không"/>
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
            <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
            <asp:HiddenField ID="hidPhieuID" Value="0" runat="server" />
            <asp:HiddenField ID="hidIsUser" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
