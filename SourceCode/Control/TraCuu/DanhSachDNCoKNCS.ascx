<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DanhSachDNCoKNCS.ascx.vb" Inherits="Control_TraCuu_DanhSachDNCoKNCS" %>
<%@ Register Assembly="ValidationTextBox" Namespace="ValidationTextBox.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Assembly="ValidationDropdownlist" Namespace="ValidationDropdownlist.CustomControls"
    TagPrefix="cc1" %>
<script type="text/javascript">
    function ajaxJquery() {
      
    }
</script>

<asp:UpdatePanel ID="uplDailyDebt" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>                    
                    <asp:Label ID="lblReportName" runat="server" Text="[PHIẾU] DANH SÁCH DOANH NGHIỆP CÓ KIẾN NGHỊ VỀ CHÍNH SÁCH" />
                </h3>
            </div>
            <div class="Error-Center">
                <div id="divError">
                    <ul id="lstErrorContent">
                    </ul>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div id="SearchCondition" class="Div-Center">
                <div class="DivRow">
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label3" runat="server" Text="Địa phương:" />
                    </div>
                    <div class="DivTextBox">
                       
                         <asp:DropDownList ID="ddlDiaPhuong" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel" style="width: 260px">
                        <asp:Label ID="Lab5" runat="server" Text="Từ năm:" />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlYearFrom" Width="90px" runat="server" CssClass="DropDownList" />
                        <asp:Label ID="Label5" runat="server" Text="Tới năm:" />
                        <asp:DropDownList ID="ddlYearTo"  Width="95px" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <%--<div class="DivRow">
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label1" runat="server" Text=" Lĩnh vực:" />
                    </div>
                    <div class="DivTextBox">
                           <cc2:ValidationTextBox ID="txtLoaihinhSX" runat="server" CssClass='TextBox'></cc2:ValidationTextBox>
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label6" runat="server" Text="Có kiến nghị về chính sách:" />
                    </div>
                    <div class="DivTextBox">                       
                        <asp:RadioButtonList ID="rptKienNghi" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="-1" Text=" Tất cả "/>
                            <asp:ListItem Value="1" style="margin:0 50px 0 50px" Text=" Có " />
                            <asp:ListItem Value="0" Text=" Không " />
                        </asp:RadioButtonList>                       
                    </div>
                </div>--%>
                <div style="clear: both; height: 10px;">
                </div>
                  <div class="DivRow">
                    <div class="DivLabel" style="width:260px" >
                        
                    </div>
                    <div class="DivTextBox" style="padding-left:280px">
                      <asp:Button ID="btnExport"  runat="server" Text="Tìm Kiếm" CausesValidation="true"
                    CssClass="btn"  />
                    <asp:Button ID="btnHuy" runat="server" Text="Đặt lại" CausesValidation="false"
                    CssClass="btn"  />
                    </div>
                    </div>
                <div style="clear: both; height: 10px;">
                </div>
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='STT'  HeaderText="STT">
                        <ItemStyle Width="4%" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col1'  HeaderText="Tên doanh nghiệp">
                        <ItemStyle Width='18%' />
                        <HeaderStyle   />
                    </asp:BoundField>
                    <asp:BoundField DataField='col2' HeaderText="Địa chỉ">
                        <ItemStyle Width='25%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col3' HeaderText='Lĩnh vực sản xuất'>
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col4' HeaderText="Tổng số lao động" DataFormatString="{0:n0}">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                       <asp:TemplateField HeaderText="Ý kiến của doanh nghiệp">
                        <ItemTemplate >
                             <asp:Literal ID="ltrYkien" runat="server"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle  Width='40%' HorizontalAlign="Left" />
                    </asp:TemplateField>

                </Columns>
                <HeaderStyle Height="26px" HorizontalAlign="Center" CssClass="GridHeader" />
                <RowStyle CssClass="GridRow" Height="23px" />
                <AlternatingRowStyle CssClass="GridAlterRow" />
                <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
                <PagerSettings Mode="Numeric" PageButtonCount="2" />
                <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
            </asp:GridView>
            <div>
                <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
                <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" CssClass="input_form"
                    Width="70px" >
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblTotal" runat="server"></asp:Label>
            </div>
            <div style="width: 100%">
                <!--phan trang-->
                <div id="results_resultsNavigationDiv" class="paginationEMP">
                    <ul>
                        <asp:LinkButton ID="lnkFirst" runat="server" ></asp:LinkButton>
                        <asp:Repeater ID="rptPage" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTitle" runat="server" CssClass="page_num" OnClick="lnkTitle_Click"
                                    CausesValidation="false">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:LinkButton ID="lnkLast" runat="server" ></asp:LinkButton>
                    </ul>
                </div>
                <asp:HiddenField ID="hidCount" runat="server" Value="0" />
                <asp:HiddenField ID="hidIndex_page" runat="server" Value="0" />
                <asp:HiddenField ID="hidCur_Page" runat="server" Value="1" />
            </div>          
    </ContentTemplate>
</asp:UpdatePanel>
