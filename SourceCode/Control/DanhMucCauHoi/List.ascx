<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_DanhMucCauHoi_List" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {
            //Xử lý nút Hủy
            $("#<%=btnHuy.ClientId %>").click(function () {
                $(".BoxField").find("input[type='text']").each(function () {
                    $(this).val('');
                });

                return false;
            });
        });

    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <%  If Session("IsUser") <> 2 Then
            
        %>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    QUẢN LÝ CÂU HỎI</h3>
                <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
            </div>
            <div class="Error">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMota" CssClass="TextLabel" runat="server" Text="Tiêu đề câu hỏi:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtTieuDeDayDu" runat="server" CssClass='TextBox SmallTextarea'
                        Height="40px" DataType="required" AssociatedLableText="Nhập tiêu đề"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Tên lỗi vi phạm:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtTieuDeVietTat" runat="server" CssClass='TextBox SmallTextarea'
                        Height="40px" DataType="none" AssociatedLableText="Nhập tiêu đề rút gọn"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Là tiêu chí:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtTieuChi" runat="server" CssClass='TextBox ' DataType="none"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Điều kiện:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtDieuKien" runat="server" CssClass='TextBox ' DataType="none"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label4" CssClass="TextLabel" runat="server" Text="Sắp xếp:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtSapXep" runat="server" CssClass='TextBox ' DataType="none"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label8" CssClass="TextLabel" runat="server" Text="Câu hỏi ID:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtCauHoiId" runat="server" CssClass='TextBox ' DataType="none"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label6" CssClass="TextLabel" runat="server" Text="ParentId:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtParentId" runat="server" CssClass='TextBox ' DataType="none"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label7" CssClass="TextLabel" runat="server" Text="ParentBCId:" />
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtParentBCId" runat="server" CssClass='TextBox ' DataType="none"></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label5" CssClass="TextLabel" runat="server" Text="Là điều kiện mới:" />
                </div>
                <div class="DivTextBox">
                    <asp:CheckBox ID="chkIsTieuChiMoiBC" runat="server" />
                </div>
            </div>
            <%--<div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="ParentId:" /> 
                </div>
                <div class="DivTextBox">
                    <cc2:ValidationTextBox ID="txtParentId" runat="server" 
                        CssClass='TextBox ' 
                        DataType="none"  ></cc2:ValidationTextBox>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Là vi phạm:" /> 
                </div>
                <div >
                    <asp:CheckBox ID="chkLaViPham" runat="server" />
                    
                </div>
            </div>--%>
            <%--<div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Mục câu hỏi cha:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationDropdownlist ID="ddlCauHoiParent" runat="server" CssClass='DropDownList' AutoPostBack="true"
                      DataType="required" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Mục câu hỏi con:" ></asp:Label>
                </div>
                <div class="DivTextBox">
                    <cc1:ValidationDropdownlist ID="ddlCauHoiSub" runat="server" CssClass='DropDownList'>
                    <asp:ListItem Value="0">--- Tất cả ---</asp:ListItem>
                    </cc1:ValidationDropdownlist>
                     
                </div>
            </div>--%>
            <div style="margin-top: 10px; padding-left: 168px">
                <asp:Button ID="btnSave" runat="server" Text=" Lưu " OnClientClick="javascript:return validateFormInputs();"
                    CssClass="btn" />
                <asp:Button ID="btnHuy" CausesValidation="false" runat="server" Text=" Hủy " CssClass="btn" />&nbsp;
            </div>
            <div style="float: left">
                <div class="DivLabelKN">
                    <asp:Label ID="lblAuditnumber" CssClass="TextLabel" runat="server" Text="Câu hỏi:" />
                </div>
                <div class="DivTextBox">
                    <asp:TreeView ID="trvMenu" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="None"
                        ShowLines="True" CssClass="tree">
                    </asp:TreeView>
                </div>
            </div>
            <div style="clear: both">
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="hidCauHoiID" Value="" runat="server" />
        </div>
        <%End If%>
    </ContentTemplate>
</asp:UpdatePanel>
