<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_CauHoi9_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            $(".TextBox:first").focus();
 
        });
    }
    function TestVaild() {
        if (validateFormInputs()) {
            var txtQ9111 = $("#<%=txtQ9111.ClientId %>");
            var txtQ9112 = $("#<%=txtQ9112.ClientId %>");                      

            if (txtQ9112.val().replace(/,/gi, '') * 1 > txtQ9111.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số vụ tranh chấp cá nhân phải lớn hơn hoặc bằng số vụ đã hòa giải thành công.", txtQ9112);
                txtQ9112.select();
                return false;
            }
            var txtQ92 = $("#<%=txtQ92.ClientId %>");
            var txtQ921 = $("#<%=txtQ921.ClientId %>");
            var txtQ922 = $("#<%=txtQ922.ClientId %>");
            var sum = (txtQ921.val().replace(/,/gi, '') * 1 + txtQ922.val().replace(/,/gi, '') * 1);
            if (sum > txtQ92.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số vụ tranh chấp tập thể phải lớn hơn hoặc bằng tổng số vụ tranh chấp về lợi ích và đình công tự phát.", txtQ921);
                txtQ921.select();
                return false;
            }
            var txtQ93 = $("#<%=txtQ93.ClientId %>");
            var txtQ931 = $("#<%=txtQ931.ClientId %>");

            if (txtQ931.val().replace(/,/gi, '') * 1 > txtQ93.val().replace(/,/gi, '') * 1) {
                AlertboxThenFocus("Vui lòng nhập lại.\n Số vụ khiếu nại về lao động phải lớn hơn hoặc bằng số vụ đã giải quyết xong.", txtQ931);
                txtQ931.select();
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
                    <asp:Label ID="lblTitleCompany" runat="server" Text="TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG" /></h3>
            </div>
            <div class="DivRequired" style="font-weight: bold">
                9. Tranh chấp lao động:
            </div>
            <div class="DivRow fl1">
                <div class="col20">
                    <div class="DivRow901"> 
                        <div class="DivRow width100 fl">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label15" CssClass="TextLabel" runat="server" Text="9.1. Tranh chấp cá nhân:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ9111" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                               (vụ)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text=" + đã hòa giải thành công:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ9112" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (vụ)
                            </div>                      
                        </div>
                    </div>
                    <div class="DivRow901"> 
                        <div class="DivRow width100 fl">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="9.2. Tranh chấp tập thể:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ92" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (vụ)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label7a" CssClass="TextLabel" runat="server" Text=" + trong đó tranh chấp về lợi ích:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ921" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (vụ)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label7b" CssClass="TextLabel" runat="server" Text=" + đình công tự phát:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ922" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (vụ)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label9" CssClass="TextLabel" runat="server" Text=" + Số người tham gia đình công sau số vụ đình công tự phát:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ925" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (người)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text=" + người lao động đòi:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:43px">                               
                                 <asp:TextBox ID="txtQ923" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine"/>
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label7d" CssClass="TextLabel" runat="server" Text=" + Kết quả giải quyết:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:43px">                               
                                 <asp:TextBox ID="txtQ924" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine"/>
                            </div>                      
                        </div>
                        
                    </div>
                </div>
                <div class="col20">
                    <div class="DivRow903">
                        <div class="DivRow width100 fl">
                            <div class="DivHeader fl">
                                <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="9.3. Số vụ khiếu nại về lao động:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ93" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (vụ)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl ">
                            <div class="fl fl1">
                                <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text=" + đã giải quyết xong:" />
                            </div>
                            <div class="fr fr1 TextChirld" style="margin-right:14px">                               
                                <cc2:ValidationTextBox ID="txtQ931" runat="server" CssClass='TextBox TextBox_short' DataType="integer"/>
                                (vụ)
                            </div>                      
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1">
                                <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="+ Nguyên nhân chính:" />
                            </div>
                            <div class="fr fr1" style="margin-right:43px">
                                <asp:TextBox ID="txtQ932" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine"/>
                            </div>
                        </div>
                        <div class="DivRow width100 fl">
                            <div class="fl fl1" >
                                <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text=" + Kết quả giải quyết:" />
                            </div>
                            <div class="fr fr1" style="margin-right:43px">
                                <asp:TextBox ID="txtQ933" runat="server" CssClass='TextBox textarea1' TextMode="MultiLine"/>
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
                            CommandArgument="" OnClientClick="javascript:return TestVaild();" />
                        
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
            <asp:HiddenField ID="hidIsCongDoan" Value="False" runat="server" />
            <asp:HiddenField ID="hidModePhieu" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
