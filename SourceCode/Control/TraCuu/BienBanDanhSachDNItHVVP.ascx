<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BienBanDanhSachDNItHVVP.ascx.vb" Inherits="Control_TraCuu_BienBanDanhSachDNItHVVP" %>

<script type="text/javascript">
    function ajaxJquery() {
            }
</script>

<asp:UpdatePanel ID="uplDailyDebt" runat="server">
    <ContentTemplate>
        <div class="BoxField">
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblReportName" runat="server" Text="BIÊN BẢN DANH SÁCH DOANH NGHIỆP CÓ ÍT VI PHẠM NHẤT" />
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
                        <asp:Label ID="Label5" runat="server" Text=" Từ năm:" />
                    </div>
                    <div class="DivTextBox">
                         <asp:DropDownList ID="ddlYearFrom"  Width="90px" runat="server" CssClass="DropDownList" />
                          <asp:Label ID="Label7" runat="server" Text="đến năm:" />
                           <asp:DropDownList ID="ddlYearTo" Width="95px" runat="server" CssClass="DropDownList" />
                    </div>
                </div>          
             
                <div class="DivRow">
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label1" runat="server" Text=" Chuyên mục vi phạm:" />
                    </div>
                    <div class="DivTextBox">
                           <asp:DropDownList ID="ddlChuyenMuc" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow">
                    <div class="DivLabel" style="width:260px" >
                        <asp:Label ID="Label2" runat="server" Text="Có vi phạm:" />
                    </div>
                    <div class="DivTextBox">                    
                        <asp:RadioButtonList ID="rptViPham" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="1" style="margin:0 50px 0 0" Text= " Có" />
                            <asp:ListItem Value="0" Text=" Không"/>
                        </asp:RadioButtonList>
                   
                    </div>
                </div>
                <%--<div class="DivRow" style="display:none" >
                    <div class="DivLabel" style="width:260px" >
                        <asp:Label ID="Label2" runat="server" Text="Có yếu tố nguy hiểm,độc hại:" />
                    </div>
                    <div class="DivTextBox">
                    
                        <asp:RadioButtonList ID="rptCoYTNHDH"  CellPadding="5" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="-1">Tất cả </asp:ListItem>
                            <asp:ListItem Value="1" style="margin:0 50px 0 50px">Có </asp:ListItem>
                            <asp:ListItem Value="0">Không </asp:ListItem>
                        </asp:RadioButtonList>
                   
                    </div>
                </div>
                <div class="DivRow" style="display:none" >
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label4" runat="server" Text="Có tai nạn lao động,bệnh nghề nghiệp:" />
                    </div>
                    <div class="DivTextBox">
                       
                        <asp:RadioButtonList ID="rptCoTNNN" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="-1">Tất cả </asp:ListItem>
                            <asp:ListItem Value="1" style="margin:0 50px 0 50px" >Có </asp:ListItem>
                            <asp:ListItem Value="0">Không </asp:ListItem>
                        </asp:RadioButtonList>
                       
                    </div>
                </div>
                <div class="DivRow" style="display:none" >
                    <div class="DivLabel" style=" width:260px">
                        <asp:Label ID="Label6" runat="server" Text="Có kiến nghị về chính sách:" />
                    </div>
                    <div class="DivTextBox">
                       
                        <asp:RadioButtonList ID="rptKienNghi" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="-1">Tất cả </asp:ListItem>
                            <asp:ListItem Value="1" style="margin:0 50px 0 50px" >Có </asp:ListItem>
                            <asp:ListItem Value="0">Không </asp:ListItem>
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
                    <asp:Button ID="btnHuy" runat="server" Text="Đặt lại" CausesValidation="false" CssClass="btn"  />
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
                        <ItemStyle Width='20%' />
                        <HeaderStyle  />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col2' HeaderText='Lĩnh vực sản xuất'>
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col3' HeaderText="Tổng số LĐ">
                        <ItemStyle Width='10%' />
                    </asp:BoundField>
                     <asp:BoundField DataField='Col4' HeaderText="Địa chỉ">
                        <ItemStyle Width='25%' />
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
            <div style="display: none">
                <asp:DropDownList ID="drpMessage" runat="server">
                    <asp:ListItem Text="Bạn có chắc chắn muốn xóa bản ghi này không?" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Bạn có chắc chắn muốn xóa bản đã chọn?" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Bạn phải chọn ít nhất một bản ghi." Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hidID" runat="server" Value="0" />
              <asp:TextBox ID="txtIDLoaihinhSX" Text="-1" runat="server"></asp:TextBox>
            </div>        
          </div>    
          
    </ContentTemplate>
</asp:UpdatePanel>
