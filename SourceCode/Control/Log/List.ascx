<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_Log_List" %>
<script type="text/javascript" language="javascript">
    function confirmMultiDelete(strID) {
        var grdShow = document.getElementById("<%=grdShow.ClientID %>");
        var count = 0;
        for (i = 1; i < grdShow.children.item(0).children.length; i++) {
            if (grdShow.children.item(0).children.item(i).children.item(0).children.item(0).checked) {
                count += 1;
            }
        }
        if (count > 0) {
            return ComfirmDialog('<%=drpMessage.Items(1).Text%>', 0, strID, 0, 0);
        } else {
            Alertbox('<%=drpMessage.Items(2).Text%>');
            return false;
        }
    }
    function ComfirmDialog(strText, value, ObjectIdClick, ID, Type) {
        if (value == 0) {
            Confirmbox(strText, value, ObjectIdClick, ID, Type);
            return false;
        } else {
            if (Type == 1) {
                document.getElementById('<%=hidID.ClientID %>').value = ID;
                eval($("#" + ObjectIdClick + "").attr("href"));
            }
            else {
                $("#" + ObjectIdClick + "").click();
            }
        }
    }

    //Check ngày đến có lớn hơn ngày bắt đầu hay không
    function CheckDateThanEqual(source, arguments) {
        //So sanh ngay
        if ($("#<%=txtFrom.ClientID %>").val() != "" && $("#<%=txtTo.ClientID %>").val() != "") {
            var objFrom = $("#<%=txtFrom.ClientID %>").val().split('/');
            var objEnd = $("#<%=txtTo.ClientID %>").val().split('/');
            var FromDate = new Date(objFrom[2], objFrom[1], objFrom[0], 0, 0, 0)
            var EndDate = new Date(objEnd[2], objEnd[1], objEnd[0], 0, 0, 0)

            if (EndDate < FromDate) {
                arguments.IsValid = false;
                return false;
            }
            else {
                arguments.IsValid = true;
                return true;
            }
        }
        else {
            arguments.IsValid = true;
            return true;
        }
    }

    function ajaxJquery() {
        $(".SubjectTitle input, .SubjectTitle select").keypress(function (e) {
            if (e.keyCode == 13) {
                $("#<%=btnSearch.ClientID()%>").click();
                return false;
            }
        });

        $(document).ready(function () {

            $(".Date").datepicker({ dateFormat: 'dd/mm/yy' });
  
            //check cookie to show or hide search pannel
            if ($.cookie('CookieSSMSearch') == 1) {
                $('#pnlSearch').show();
            }
            else {
                $('#pnlSearch').hide();
            }

            $('#B1').click(function (event) {

                //set cookie
                if ($.cookie('CookieSSMSearch') == 1) {
                    $.cookie('CookieSSMSearch', 0)
                }
                else {
                    $.cookie('CookieSSMSearch', 1)
                }
                $('#pnlSearch').slideToggle(500);



                if ($('#imgShowHide').attr("src") == "../../images/arrow_popup.gif") {

                    $('#imgShowHide').attr("src", "../../images/arrow_showless.gif");
                }
                else {
                    $('#imgShowHide').attr("src", "../../images/arrow_popup.gif");
                }
            });

        });
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <div class="HeadTitle">
                <h3>
                    NHẬT KÝ HỆ THỐNG</h3>
            </div>
            <div class="Error">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="#FF3300" />        
            </div>

            <div style="clear: both"></div>
            <div class="BoxField">               
                <div  id="SInfor" runat="server">              
                    <legend class="SubjectTitle">
                        <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
                        <a id="B1" href="#">Tìm kiếm </a></legend>
                </div>
                <div id="pnlSearch">
                    <div class="SubjectTitle">
                        <div class="DivRow">
                            <div  class="DivLeft">                                
                                <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Nội dung:" />
                            </div>
                            <div class="DivRight" >
                                <asp:TextBox ID="txtContent" CssClass="TextBox" runat="server" Text="" />
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">                                
                                 <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Chức năng:" />
                            </div>
                            <div class="DivRight" >
                                <asp:DropDownList ID="drlFunction" CssClass="DropDownList" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">
                                <asp:Label ID="lblActtime" CssClass="TextLabel" runat="server" Text="Từ:" />
                            </div>
                            <div class="DivRight" >
                                <asp:TextBox ID="txtFrom" CssClass="TextBox Date" runat="server" Text="" /><span  style="font-family: Arial,tahoma;font-size: 10pt;font-weight: normal;" >(dd/mm/yyyy)</span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtFrom"
                                    Display="None" ErrorMessage="Ngày bắt đầu không đúng định dạng" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">
                                <asp:Label ID="Label2" CssClass="TextLabel" runat="server" Text="Tới:" />
                            </div>
                            <div class="DivRight">
                                <asp:TextBox ID="txtTo" CssClass="TextBox Date" runat="server" Text="" /><span style="font-family: Arial,tahoma;font-size: 10pt;font-weight: normal;" >(dd/mm/yyyy)</span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtTo"
                                    Display="None" ErrorMessage="Ngày kết thúc không đúng định dạng" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                                
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Ngày bắt đầu phải nhỏ hơn ngày kết thúc."
                                    ClientValidationFunction="CheckDateThanEqual" ControlToValidate="txtTo" Display="None"
                                    SetFocusOnError="false"></asp:CustomValidator>
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">
                                <asp:Label ID="lblEventid" CssClass="TextLabel" runat="server" Text="Sự kiện:" />
                            </div>
                            <div class="DivRight">
                                <asp:DropDownList ID="drlAction" CssClass="DropDownList" runat="server">
                                    <asp:ListItem Value="0">------Tất cả-------</asp:ListItem>
                                    <asp:ListItem Value="1">Tạo mới</asp:ListItem>
                                    <asp:ListItem Value="2">Sửa</asp:ListItem>
                                    <asp:ListItem Value="4">Xóa</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">
                                <asp:Label ID="lblClientip" CssClass="TextLabel" runat="server" Text="Ip:" />
                            </div>
                            <div class="DivRight">
                                <asp:TextBox ID="txtClientip" CssClass="TextBox" runat="server" Text="" />
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">
                                <asp:Label ID="lblUsername" CssClass="TextLabel" runat="server" Text="Người dùng:" />
                            </div>
                            <div class="DivRight">
                                <asp:TextBox ID="txtUsername" CssClass="TextBox" runat="server" Text="" />
                            </div>
                        </div>
                        <div class="DivRow">
                            <div class="DivLeft">
                                &nbsp;&nbsp;&nbsp;</div>
                            <div class="DivRight">
                                <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="btn" />
                                <asp:Button ID="btnHuy" CausesValidation="false" runat="server" Text=" Hủy " CssClass="btn" />&nbsp;
                            </div>
                        </div>
                        
                    </div>                    
                </div>
                
               
                <div class="SubjectTitle" style="float:left; padding-top: 10px">
                                       
                </div>
                <div class="clearboth"></div>
                <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" 
                    EmptyDataText="{Không có dữ liệu}" Width='100%' BorderWidth="1px" 
                    CssClass="GridBorder">
                    <Columns>
                       
                        <asp:TemplateField HeaderText="STT">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nội dung">                           
                            <ItemTemplate>
                                <asp:Literal ID="ltrContent" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="40%" />
                        </asp:TemplateField>
                        <asp:BoundField DataField='FunctionName' HeaderText='Chức năng'>
                            <ItemStyle Width='10%' />
                        </asp:BoundField>
                        <asp:BoundField DataField='EventName' HeaderText='Sự kiện'>
                            <ItemStyle Width='10%' />
                        </asp:BoundField>
                        <asp:BoundField DataField='ActTime' DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText='Thời điểm'>
                            <ItemStyle Width='15%' />
                        </asp:BoundField>
                        <asp:BoundField DataField='ClientIP' HeaderText='IP'>
                            <ItemStyle Width='10%' />
                        </asp:BoundField>
                        <asp:BoundField DataField='UserName' HeaderText='Người dùng'>
                            <ItemStyle Width='15%' />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
                    <RowStyle CssClass="GridRow" Height="23px" />
                    <AlternatingRowStyle CssClass="GridAlterRow" />
                    <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
                    <PagerSettings Mode="Numeric" PageButtonCount="2" />
                    <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
                </asp:GridView>
                <div style="clear: both"></div>
                <div>
                    <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Xóa" Visible="false" />
                    <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:"></asp:Label>
                    <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px"
                        CssClass="input_form">
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                <div style="width: 100%">
                    <!--phan trang-->
                    <div id="results_resultsNavigationDiv" class="paginationEMP">
                        <ul>
                            <asp:LinkButton ID="lnkFirst" runat="server"></asp:LinkButton>
                            <asp:Repeater ID="rptPage" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTitle" CssClass="page_num" runat="server" OnClick="lnkTitle_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:LinkButton ID="lnkLast" runat="server"></asp:LinkButton>
                        </ul>
                    </div>
                    <asp:HiddenField ID="hidCount" Value="0" runat="server" />
                    <asp:HiddenField ID="hidIndex_page" Value="0" runat="server" />
                    <asp:HiddenField ID="hidCur_Page" Value="1" runat="server" />
                </div>
                <div style='display: none'>
          
                    <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
       
                    <asp:DropDownList ID="drpMessage" runat="server">
                        <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản ghi này không?' Value='0'></asp:ListItem>
                        <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản đã chọn?' Value='1'></asp:ListItem>
                        <asp:ListItem Text='Bạn phải chọn ít nhất một bản ghi.' Value='2'></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hidID" Value="0" runat="server" />
                    <asp:HiddenField ID="hidSearch" Value="0" runat="server" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
