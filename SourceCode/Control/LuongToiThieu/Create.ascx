<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_LuongToiThieu_Create" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {

            $(".TextBox:first").focus();
            $('.Date').datepicker({ dateFormat: 'dd/mm/yy' });
        });

    }
    function CheckValid() {
        return validateFormInputs() ;
    }
     
</script>
<asp:UpdatePanel ID="uplDanhmuchanhvi" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="THÊM MỚI LƯƠNG TỐI THIỂU" /></h3>
            </div>
            <div class="DivRequired">
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMota" CssClass="TextLabel" runat="server" Text="Mô tả:" /><span class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    
                    <cc2:ValidationTextBox ID="txtMota" runat="server" 
                        CssClass='TextBox LargeTextarea' Height="40px" 
                        TextMode="MultiLine" DataType="required" Width="100px" 
                        AssociatedLableText="mô tả"></cc2:ValidationTextBox>
                </div>
            </div>
             
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMucluongtoithieu" CssClass="TextLabel" runat="server" Text="Mức lương tối thiểu:" /><span class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtMucLuongToiThieu" runat="server" CssClass='TextBox' 
                        AssociatedLableText="mức lương tối thiểu" DataType="requiredAndInteger"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Quyết định:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtQuyetDinh" runat="server" CssClass='TextBox SmallTextarea'
                        Height="40px" TextMode="MultiLine" DataType="required" AssociatedLableText="Nhập quyết định"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Ngày quyết định:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtNgayQuyetDinh" runat="server" CssClass='TextBox Date'
                        AssociatedLableText="đúng định dạng dd/mm/yyyy" DataType="required" 
                        CustomPattern="ddMMyyyy"></cc2:ValidationTextBox>(dd/mm/yyyy)
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblLoainhanuoc" CssClass="TextLabel" runat="server" Text="Áp dụng chung cho nhà nước?:" /> 
                </div>
                <div class="DivTextBox">
                   
                    <cc1:ValidationDropdownlist ID="ddlLoainhanuoc" runat="server" 
                        CssClass='DropDownList' AssociatedLableText="loại nhà nước" DataType="none">                         
                        <asp:ListItem Value="1" Selected="True">Có</asp:ListItem>
                        <asp:ListItem Value="0">Không</asp:ListItem>
                    </cc1:ValidationDropdownlist>
                </div>
            </div>

                    <div class="DivRow">
                        <div class="DivLabel">
                            <asp:Label ID="lblTinhid" CssClass="TextLabel" runat="server" Text="Tỉnh:" /><span
                                class="fieldRequired">&nbsp;</span>
                        </div>
                        <div class="DivTextBox">
                        <cc1:ValidationDropdownlist ID="ddlTinh" runat="server" CssClass='DropDownList' 
                            AssociatedLableText="tỉnh" DataType="required" AutoPostBack= "true" />
                         </div>

                    </div>
                    <div class="DivRow">

                          <div class="DivLabel">
                            <asp:Label ID="lblHuyenid" CssClass="TextLabel" runat="server" Text="Huyện:" /><span
                                class="fieldRequired">&nbsp;</span>
                        </div>
                         <div class="DivTextBox">
                        <cc1:ValidationDropdownlist ID="ddlHuyen" runat="server" CssClass='DropDownList' 
                            AssociatedLableText="huyện" DataType="required"/>
                        </div>


                    </div>


            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <div style="float: left">
                        <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" OnClientClick="javascript:return CheckValid()" />
                        &nbsp;<asp:Button ID="btnSaveAndNew" runat="server" CssClass="btn" OnClientClick="javascript:return CheckValid()" Text=" Lưu &amp; Làm mới" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"  OnClientClick="javascript:return CheckValid()"
                            Text="Làm lại" /></div>
                    <div style="float: right; text-align: right">
                        &nbsp;<asp:ImageButton ID="btnBack" ImageAlign="AbsMiddle" ToolTip="Quay lại" runat="server"
                            ImageUrl="~/images/back.png" /></div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
