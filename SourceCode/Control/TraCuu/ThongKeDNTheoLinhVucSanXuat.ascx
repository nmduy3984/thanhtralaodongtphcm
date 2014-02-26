<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ThongKeDNTheoLinhVucSanXuat.ascx.vb" Inherits="Control_TraCuu_ThongKeDNTheoLinhVucSanXuat" %>

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
                    <asp:Label ID="lblReportName" runat="server" Text="Tra cứu doanh nghiệp thuộc Lĩnh vực sản xuất" />
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
                            <asp:Label ID="Label3" runat="server" Text=" Địa phương:" />
                        </div>
                        <div class="DivTextBox">                       
                             <asp:DropDownList ID="ddlDiaPhuong" runat="server" CssClass="DropDownList" />
                        </div>
                </div> 
                <div class="DivRow">
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label5" runat="server" Text="Lĩnh vực sản xuất:" />
                    </div>
                    <div class="DivTextBox">
                         <asp:DropDownList ID="ddlLinhVuc"  runat="server" CssClass="DropDownList" />
                    </div>
                </div>          
             
                <div class="DivRow">
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label1" runat="server" Text="Năm:" />
                    </div>
                    <div class="DivTextBox">
                            <asp:DropDownList ID="ddlNam"  runat="server" CssClass="DropDownList" />
                           
                    </div>
                </div>
                
                <div style="clear: both; height: 10px;">
                </div>
                  <div class="DivRow">
                    <div class="DivLabel" style="width:260px" >
                        
                    </div>
                    <div class="DivTextBox" style="padding-left:280px">
                      <asp:Button ID="btnExport"  runat="server" Text="Tìm Kiếm" CausesValidation="true"
                    CssClass="btn"  />
                    <asp:Button ID="btnHuy" runat="server" Text="Đặt lại" CausesValidation="false" CssClass="btn"  />
                    </div>
                    </div>
                <div style="clear: both; height: 10px;">
                </div>
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>

                    <asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                         <ItemStyle Width='5%' />
                    </asp:TemplateField>

                    <asp:BoundField DataField='TenTinh'  HeaderText="Địa phương">
                        <ItemStyle Width='15%' />
                        <HeaderStyle  />
                    </asp:BoundField>

                    <asp:BoundField DataField='SoDN' HeaderText="Tổng số Doanh nghiệp">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>

                    <asp:BoundField DataField='TongGiaTriSP' HeaderText='Tổng giá trị sản phẩm' DataFormatString="{0:###,##}">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='TongSoNhanVien' HeaderText='Tổng số lao động hiện có' DataFormatString="{0:###,##}">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoNhanVienDaTuyen' HeaderText='Tổng số lao động đã tuyển' DataFormatString="{0:###,##}">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='SoNhanVienCanTuyen' HeaderText='Tổng số lao động cần tuyển' DataFormatString="{0:###,##}">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    
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
                <asp:ListItem Text="100" Value="100" Selected></asp:ListItem>
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
                 <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
            </div>                    
          </div>    
          
    </ContentTemplate>
</asp:UpdatePanel>
