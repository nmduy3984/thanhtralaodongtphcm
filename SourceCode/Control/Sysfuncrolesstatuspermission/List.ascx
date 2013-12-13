<%@ Control Language="VB" AutoEventWireup="false" CodeFile="List.ascx.vb" Inherits="Control_Sysfuncrolesstatuspermission_List" %>

	<script type='text/javascript'>
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
	    function ajaxJquery() {

	        $(document).ready(function () {
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
	<div class="BoxField" ></div>
    
    

	<div class="HeadTitle"><h3>QUẢN LÝ VAI TRÒ - CHỨC NĂNG</h3></div><div class="SubjectTitle" >

    <div class="SubjectTitle" id="SInfor" runat="server">
            <legend class="SubjectTitle">
                <img alt="" src="../../images/arrow_showless.gif" id="imgShowHide" />
                <a id="B1" href="#">Tìm kiếm </a></legend>
        </div>
        <div id="pnlSearch">
            <div class="BoxField">
                <div class="SubjectTitle" style="width: 25%; text-align: right; float: left">Vai trò:</div>
                <div class="DivLabel" style="width: 70%; float: left">
                    <asp:DropDownList ID="ddlRoleid" runat="server" CssClass="TextBox"></asp:DropDownList></div>
            </div>
            <div class="BoxField">
                <div style="width: 25%; text-align: right; float: left" class="SubjectTitle">Chức năng:</div>
                <div class="DivLabel" style="width: 70%;; float: left;">
                    <asp:DropDownList ID="ddlFunctionID" runat="server" CssClass="TextBox"></asp:DropDownList>
                </div>
            </div>
            <div class="BoxField">
                <div style="width: 25%; text-align: right; float: left" class="SubjectTitle">Trạng thái:</div>
                <div class="DivLabel" style="width: 70%; float: left;">
                    <asp:DropDownList ID="drlStatus" runat="server" CssClass="TextBox"></asp:DropDownList>
                </div>
            </div>
            <div class="BoxField">
                <div style="float: left; width: 200px; height: 20px; text-align: right;" class="SubjectTitle">
                </div>
                <div class="DivLabel" style="padding-left:114px" >
                    <asp:Button ID="btnFilter" runat="server" Text='Lọc' CssClass='btn' />
                </div>
            </div>
        </div>
        <div style="clear: both; height: 10px;">
        </div>
    
    </div>
	<asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
		  PageSize="10" AllowPaging="false"   Width='100%' BorderWidth="1px" CssClass="GridBorder">
		    <Columns>
		<asp:TemplateField>
		    <HeaderTemplate>
                <asp:CheckBox ID="CheckAll" runat="server" AutoPostBack="False" onclick="CheckAll(this)"
                            Width="30px" />
			        <%--<asp:CheckBox ID="CheckAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged" Width="30px" />--%>
		    </HeaderTemplate>
		    <HeaderStyle HorizontalAlign="Center" />
		     <ItemTemplate>
			        <asp:CheckBox ID="chkItem" runat="server" />
		     </ItemTemplate>
		     <ItemStyle Width="5%" HorizontalAlign="Center" />
		 </asp:TemplateField>
	<asp:TemplateField>
	<ItemTemplate>
		   <asp:HyperLink ID="hplEdit" runat="server" Text="Sửa"></asp:HyperLink>&nbsp;|&nbsp;
		   <asp:LinkButton ID="lnkbtnDelete" Text="Xóa" runat="server" OnClick="lnkbtnDelete_Click"
			       CausesValidation='False'></asp:LinkButton>
	</ItemTemplate>
	<ItemStyle CssClass="GridTextLink" HorizontalAlign="Center" Width="10%" />
	</asp:TemplateField>
	<asp:TemplateField HeaderText="STT">
		  <ItemTemplate>
			    <asp:Label runat="server" ID="lblSTT" Text=''></asp:Label>
	 </ItemTemplate>
		  <ItemStyle Width="5%"  HorizontalAlign="Left" />
	</asp:TemplateField>
		 <asp:TemplateField HeaderText='Vai trò'>
		  <ItemTemplate>
			    <asp:HyperLink ID="hplRoleid" runat="server"></asp:HyperLink>
		 </ItemTemplate>
		 <ItemStyle Width='20%' CssClass='text_list_links' HorizontalAlign='Left' />
		</asp:TemplateField>
         <asp:BoundField DataField='SysFunctionName' HeaderText='Chức năng'>
			 <ItemStyle  Width='15%' />
		 </asp:BoundField>
		 <%--<asp:BoundField DataField='StatusName' HeaderText='Trạng thái'>
			 <ItemStyle  Width='10%' />
		 </asp:BoundField>--%>
          <asp:TemplateField HeaderText='Thêm | Xóa |&nbsp;Sửa&nbsp;|&nbsp;Xem&nbsp;| Đệ trình | Xuất bản' HeaderStyle-HorizontalAlign="Center">
		  <ItemTemplate>
			    <asp:CheckBoxList runat="server" ID="chklst" RepeatDirection="Horizontal" Enabled="false" CellPadding="5" CssClass="chkRoleCheckBox"></asp:CheckBoxList>
		 </ItemTemplate>
		 <ItemStyle Width='30%' CssClass='text_list_links' HorizontalAlign="Center" />
		</asp:TemplateField>
	</Columns>
	 <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
	 <RowStyle CssClass="GridRow" Height="23px" />
	 <AlternatingRowStyle CssClass="GridAlterRow" />
	 <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
	 <PagerSettings Mode="Numeric" PageButtonCount="2" />
	 <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
	</asp:GridView>
	<div><asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Xóa" /> <asp:Label ID="lblPage_Size" runat="server" Text="Số bản ghi trên trang:" ></asp:Label>		 <asp:DropDownList ID="drpPage_Size" runat="server" AutoPostBack="True" Width="70px" CssClass="input_form">
		 <asp:ListItem Text="10" Value="10"></asp:ListItem>
		 <asp:ListItem Text="20" Value="20"></asp:ListItem>
		 <asp:ListItem Text="50" Value="50"></asp:ListItem>
		 <asp:ListItem Text="100" Value="100"></asp:ListItem>
		 </asp:DropDownList>
		  <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
	<div style="width: 80%">
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
	<asp:DropDownList ID="drpMessage" runat="server" >
		  <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản ghi này không?' Value='0'></asp:ListItem>
		   <asp:ListItem Text='Bạn có chắc chắn muốn xóa bản đã chọn?' Value='1'></asp:ListItem>
	 <asp:ListItem Text='Bạn phải chọn ít nhất một bản ghi.' Value='2'></asp:ListItem>
	</asp:DropDownList>
	 <asp:HiddenField ID="hidID" Value="0" runat="server" />
     <asp:HiddenField ID="hidSearch" Value="0" runat="server" />
	</div>
</ContentTemplate>
</asp:UpdatePanel>

