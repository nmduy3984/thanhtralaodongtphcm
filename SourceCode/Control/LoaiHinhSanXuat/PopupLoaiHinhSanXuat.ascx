<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PopupLoaiHinhSanXuat.ascx.vb" Inherits="Control_LoaiHinhSanXuat_PopupLoaiHinhSanXuat" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {
            //Selected from parent
            //var sListId = window.parent.getCustomerValue();

            $('#<%=grdShow.ClientID%> tr[id]').mouseover(function () {
                $(this).css({ cursor: "hand", cursor: "pointer" });
            });

            $("#frmSearch input").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnFilter.ClientID()%>").click();
                    return false;
                }
            });
           
        });
    }
   
    function SelectLoaiHinhSanXuat(LHSXID, LHSXName) {
        window.parent.SetLoaiHinhSanXuatID(LHSXID, LHSXName);
        window.parent.tb_remove();
        return false;
    }
     
</script>
<asp:UpdatePanel ID="uplContents" runat="server">
    <ContentTemplate>
        <div class="BoxField" style="background-color: #ffffff; padding-top: 8px;" id = "frmSearch">
         
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblGroupname" CssClass="TextLabel" runat="server" Text="Nhập mã hoặc tên LHSX: " />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtLHSX" CssClass="TextBox" runat="server" Text="" Width="208px" />
                    &nbsp;<asp:Button ID="btnFilter" runat="server" Text=" Tìm kiếm " CssClass="btn" />
                </div>
            </div>
             
            <div class="DivRow" style="padding-top: 10px">
                <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                    AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:RadioButton ID="chkItem"  runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:TemplateField> 
                        <asp:BoundField DataField='RowNum' HeaderText=' STT '>
                            <ItemStyle Width='5%' />
                        </asp:BoundField>
                         <asp:BoundField DataField='Code' HeaderText=' Mã '>
                            <ItemStyle Width='10%' />
                        </asp:BoundField>
                        <asp:BoundField DataField='Title' HeaderText='Tên loại hình'>
                            <ItemStyle Width='25%' />
                        </asp:BoundField>
                        <asp:BoundField DataField='ParentTite' HeaderText='Trực thuộc'>
                            <ItemStyle Width='20%' />
                        </asp:BoundField>  
                                           
                        <asp:BoundField DataField='Description' HeaderText='Mô tả'>
                            <ItemStyle Width='20%' />
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
                    <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px"
                        CssClass="input_form">
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="20" Value="20" />
                        <asp:ListItem Text="50" Value="50" />
                        <asp:ListItem Text="100" Value="100"  Selected="True"/>
                    </asp:DropDownList>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                <div style="width: 100%">
                    <!--phan trang-->
                    <div id="results_resultsNavigationDiv" class="paginationEMP">
                        <ul>
                            <asp:LinkButton ID="lnkFirst" runat="server" />
                            <asp:Repeater ID="rptPage" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTitle" CssClass="page_num" runat="server" OnClick="lnkTitle_Click" />
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:LinkButton ID="lnkLast" runat="server" />
                        </ul>
                    </div>
                    <div style="display:none">
                        <center>
                            <asp:Button ID="btnSelect" runat="server" Text=" Chọn " CssClass="btn" />
                        </center>
                    </div>
                    <asp:HiddenField ID="hidCount" Value="0" runat="server" />
                    <asp:HiddenField ID="hidIndex_page" Value="0" runat="server" />
                    <asp:HiddenField ID="hidCur_Page" Value="1" runat="server" />
                </div>
            </div>
        </div>        
    </ContentTemplate>
</asp:UpdatePanel>
