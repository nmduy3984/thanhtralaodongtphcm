<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ContentEditorPhieuTKK.ascx.vb" Inherits="DoanhNghiep_Control_ContentEditorPhieuTKK" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<div>
        <asp:UpdatePanel ID="uplCustomer_card" runat="server">
            <ContentTemplate>
                <div id="view" class="BoxField" runat="server">
                    <%--<div class="DivRow">
                        <div class="">
                            <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Phiếu kiểm tra: " /><span
                                class="fieldRequired">&nbsp;</span>
                        </div>
                        <cc1:ValidationDropdownlist ID="ddlPhieuNhapHeader" runat="server" CssClass='DropDownList'
                            AssociatedLableText=" " >
                        </cc1:ValidationDropdownlist>
                    </div>--%>
                    <div class="rowmain_780">
                        <div class="tabs_manage">
                            <ul>
                                <!-- CSS Tabs -->
                                <li id="li0"   runat="server">
                                    <asp:LinkButton ID="lbtPhieuNhap"   CausesValidation="false" runat="server" ToolTip="Thông tin chi tiết của doanh nghiệp"><span>Thông tin chung</span></asp:LinkButton></li>
                                <li id="li1"    runat="server">
                                    <asp:LinkButton ID="lbtCau1" CausesValidation="false" runat="server" ToolTip="Các loại báo cáo định kỳ"><span>Mục 1</span></asp:LinkButton></li>
                                <li id="li2"   runat="server">
                                    <asp:LinkButton ID="lbtCau2" CausesValidation="false" runat="server" ToolTip="Hợp đồng lao động"><span>Mục 2</span></asp:LinkButton></li>
                                <li id="li3"   runat="server">
                                    <asp:LinkButton ID="lbtCau3" CausesValidation="false" runat="server" ToolTip="Thỏa ước lao động tập thể"><span>Mục 3</span></asp:LinkButton></li>
                                <li id="li4"   runat="server">
                                    <asp:LinkButton ID="lbtCau4" CausesValidation="false" runat="server" ToolTip="Tiền lương"><span>Mục 4</span></asp:LinkButton></li>
                                <li id="li5"   runat="server">
                                    <asp:LinkButton ID="lbtCau5" CausesValidation="false" runat="server" ToolTip="Thời giờ làm việc, thời giờ nghỉ ngơi"><span>Mục 5</span></asp:LinkButton></li>
                                <li id="li6"   runat="server">
                                    <asp:LinkButton ID="lbtCau6" CausesValidation="false" runat="server" ToolTip="Bảo hiểm xã hội, bảo hiểm thất nghiệp"><span>Mục 6</span></asp:LinkButton></li>
                                <li id="li7"   runat="server">
                                    <asp:LinkButton ID="lbtCau7" CausesValidation="false" runat="server" ToolTip="An toàn lao động, vệ sinh lao động"><span>Mục 7</span></asp:LinkButton></li>
                                <li id="li8"   runat="server">
                                    <asp:LinkButton ID="lbtCau8" CausesValidation="false" runat="server" ToolTip="Kỷ luật lao động, trách nhiệm vật chất"><span>Mục 8</span></asp:LinkButton></li>
                                <li id="li9"   runat="server">
                                    <asp:LinkButton ID="lbtCau9" CausesValidation="false" runat="server" ToolTip="Tranh chấp lao động"><span>Mục 9</span></asp:LinkButton></li>
                                <li id="li10"   runat="server">
                                    <asp:LinkButton ID="lbtCau10" CausesValidation="false" runat="server" ToolTip="Lao động là ngư¬ời n¬ước ngoài"><span>Mục 10</span></asp:LinkButton></li>
                                <li id="li11"   runat="server">
                                    <asp:LinkButton ID="lbtCau11" CausesValidation="false" runat="server" ToolTip="Lao động chưa thành niên"><span>Mục 11</span></asp:LinkButton></li>
                                <li id="li12"   runat="server">
                                    <asp:LinkButton ID="lbtCau12" CausesValidation="false" runat="server" ToolTip="Lao động nữ và bình đẳng giới"><span>Mục 12</span></asp:LinkButton></li>
                               
                            </ul>
                        </div>
                    </div>
                    <%--<div style="clear: left">
                    </div>--%>
                </div>
                <asp:HiddenField ID="hidID" Value="0" runat="server" />
                <asp:HiddenField ID="hidPhieuIdGen" Value="0" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>