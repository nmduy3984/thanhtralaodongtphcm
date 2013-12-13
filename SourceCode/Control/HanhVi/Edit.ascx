<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Edit.ascx.vb" Inherits="Control_HanhVi_Editor" %>
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
    function CheckValid() {
         
        return validateFormInputs() ? TestValidate() : false;
    }
    function TestValidate() {
        var txtMucphatmin = $("#<%=txtMucphatmin.ClientId %>").val().replace(/,/gi,'') * 1;
        var txtMucphatmax = $("#<%=txtMucphatmax.ClientId %>").val().replace(/,/gi, '') * 1;
           
        if (txtMucphatmin > txtMucphatmax) {
            AlertboxThenFocus("Vui lòng nhập lại.\n Mức phạt max phải lớn hơn hoặc bằng mức phạt min.", $("#<%=txtMucphatmin.ClientId %>"));
            $("#<%=txtMucphatmin.ClientId %>").select();
            return false;
        }
        return true;
    }
</script>
<asp:UpdatePanel ID="uplDanhmuchanhvi" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="Label3" runat="server" Text="CHỈNH SỬA HÀNH VI" /></h3>
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
                        TextMode="MultiLine" DataType="required"  
                        AssociatedLableText="mô tả"></cc2:ValidationTextBox>
                </div>
            </div>
             
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMucphatmin" CssClass="TextLabel" runat="server" Text="Mức phạt min:" /><span class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtMucphatmin" runat="server" CssClass='TextBox' 
                        AssociatedLableText="mức phạt min" DataType="requiredAndInteger"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMucphatmax" CssClass="TextLabel" runat="server" Text="Mức phạt max:" /><span class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtMucphatmax" runat="server" CssClass='TextBox' 
                        AssociatedLableText="mức phạt max" DataType="requiredAndInteger"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblLoaihanhvi" CssClass="TextLabel" runat="server" Text="Loại hành vi:" /><span class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                   
                    <cc1:ValidationDropdownlist ID="ddlLoaihanhvi" runat="server" 
                        CssClass='DropDownList' AssociatedLableText="loại hành vi" DataType="required">
                    </cc1:ValidationDropdownlist>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <div style="float: left">
                        <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" OnClientClick="javascript:return CheckValid()" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="Làm lại" /></div>
                    <div style="float: right; text-align: right">
                        &nbsp;<asp:ImageButton ID="btnBack" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/back.png"
                            ToolTip="Quay lại" /></div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidID" Value="0" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
