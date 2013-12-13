<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Create.ascx.vb" Inherits="Control_SysParameter_Create" %>

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ajaxjquery();
        }
    }
    //start page
    $(document).ready(function () {

        initial();
    });
    //after postback in updatepanel
    function ajaxjquery() {
        $(document).ready(function () {
            //setup hid ddl typeboolean hide=true
            checkHideStatusddlTypeBoolean();
            initial();
        });
    }
    //initial form
    function initial() {
        $('#<%=txtName.ClientID %>').addClass("aspNetDisabled TextBox");
        //setup hid ddl typeboolean hide=true
        checkHideStatusddlTypeBoolean();
        $("#<%=ddlType.ClientID %>").change(function () {
            setTypeData();
        });
    }

    //check ddlTypeboolean hide/unhide
    function checkHideStatusddlTypeBoolean() {
        if ($("#<%=ddlType.ClientID %> option:selected").val() != "4") {
            setVisibled($("#IDIsBoolean"), true);
        }
    }
    function setTypeData() {
        var ddlType = $("#<%=ddlType.ClientID %> option:selected").val();
        var txtVal = $("#<%=txtVal.ClientID %>");
        var txtVal2 = $("#<%=txtVal2.ClientID %>");
        var ddlBoolean = $("#<%=ddlBoolean.ClientID %>");
        //boolean type 
        if (ddlType == "4") {
            setDisabled(txtVal2, true, "TextBox");
            $(txtVal2).val("");
            setVisibled($("#IDNotBoolean"), true);
            setVisibled($("#IDIsBoolean"), false);
        } else {
            setDisabled(txtVal2, false, "TextBox");
            //            setVisibled(ddlBoolean, true);
            setVisibled($("#IDIsBoolean"), true);
            setVisibled($("#IDNotBoolean"), false);
        }
        //datetime type 
        if (ddlType == "3") {
            $(txtVal).datepicker({ dateFormat: "dd/mm/yy" });
            $(txtVal).datepicker('setDate', new Date()); //default value
            $(txtVal2).datepicker({ dateFormat: "dd/mm/yy" });
            $(txtVal2).datepicker('setDate', new Date()); //default value
        } else {
            $(txtVal).datepicker("destroy");
            $(txtVal2).datepicker("destroy");
        }
        //numeric type 
        if (ddlType == "2") {
            $(txtVal).val("0");
            $(txtVal2).val("0");
        }
    }
    //set readonly for control true/false
    function setDisabled(ctrl, value, type_ctrl) {
        if (value == true) {
            $(ctrl).attr("disabled", value);
            $(ctrl).addClass("aspNetDisabled " + type_ctrl);
        } else {
            $(ctrl).removeAttr("disabled");
            $(ctrl).removeClass("aspNetDisabled " + type_ctrl);
            $(ctrl).addClass("TextBox");
        }
    }
    //set visible for control
    function setVisibled(ctrl, value) {
        if (value == true) {
            $(ctrl).css("display", "none");
        } else {
            $(ctrl).css("display", "");
        }
    }

    function Edit(ID) {
        document.getElementById("<%=hidID.ClientID %>").value = ID;
    }
    function confirmDelete(ID) {
        if (confirm('Xóa tham số hệ thống có thể gây hậu quả nghiêm trọng đến hệ thống và Cơ sở dữ liệu \nCó thật sự muốn xóa tham số hệ thống này không?')) {
            document.getElementById("<%=hidID.ClientID %>").value = ID;
            return true;
        } else return false;
    }
    //resetForm
    function resetForm() {
        $('#regionBindData [type=text]').each(function (e) {
            $(this).val("");
        });
        $('#<%=hidID.ClientID %>').val("");
        $('#<%=txtName.ClientID %>').removeAttr("disabled");
        $('#<%=chkIsActivated.ClientID %>').removeAttr('checked');
        $('#<%=chkisSys.ClientID %>').removeAttr('checked');
        $('#<%=ddlType.ClientID %>').attr('selectedIndex', 0);
        setTypeData();
        $('#lstErrorContent li').remove();
        return false;
    }
    //checkvalid
    function checkValid() {
        var ddlType = $("#<%=ddlType.ClientID %> option:selected").val();
        var txtName = $('#<%=txtName.ClientID %>').val();
        var txtVal = $("#<%=txtVal.ClientID %>").val();
        var txtVal2 = $("#<%=txtVal2.ClientID %>").val();
        //4: boolean
        //3: datetime
        //2: numeric
        //b1: check required field
        // $("#content ul").append('<li><a href="/user/messages"><span class="tab">Message Center</span></a></li>');
        var error = "";
        var countbug = 0;
        if (txtName.trim().length <= 0) {
            error += "<li>Vui lòng chọn nhập tên tham số hệ thống [SYS Name]</li>";
            countbug += 1;
        }

        if (ddlType == "0") {
            error += "<li>Vui lòng chọn kiểu dữ liệu cho giá trị tham số hệ thống [Type]</li>";
            countbug += 1;
        }

        if (ddlType == "3") {
            if (!CheckRegular("Date", "<%=txtVal.ClientID %>")) {
                error += "<li>Dữ liệu nhập vào [Value (1)] phải là kiểu Ngày tháng (DD/MM/YYYY)</li>";
                countbug += 1;
            }
            if (txtVal2.trim().length >= 1) {
                if (!CheckRegular("Date", "<%=txtVal2.ClientID %>")) {
                    error += "<li>Dữ liệu nhập vào [Value (2)] phải là kiểu Ngày tháng (DD/MM/YYYY)</li>";
                    countbug += 1;
                }
            }
        }
        if (ddlType == "2") {
            if (!CheckRegular("Integer", "<%=txtVal.ClientID %>")) {
                error += "<li>Dữ liệu nhập vào [Value (1)] phải là kiểu số</li>";
                countbug += 1;
            }
            if (txtVal2.trim().length >= 1) {
                if (!CheckRegular("Integer", "<%=txtVal2.ClientID %>")) {
                    error += "<li>Dữ liệu nhập vào [Value (2)] phải là kiểu số</li>";
                    countbug += 1;
                }
            }
        }
        if (ddlType == 1) {
            if (txtVal.trim().length <= 0) {
                error += "<li>Vui lòng chọn nhập giá trị bắt buộc [Value (1)]</li>";
                countbug += 1;
            }
        }
        if (countbug >= 1) {
            //remove all old error
            $('#lstErrorContent li').remove();
            //show new list bug
            $("#lstErrorContent").append(error);
            return false;
        }
        return true;
    }
</script>
<style type="text/css">
    .DivRow
    {
        width: 100%;
        float: left;
    }
    .DivLabel
    {
        width: 20%;
        float: left;
    }
    .DivTextBox
    {
        width: 75%;
        float: left;
    }
</style>
<asp:UpdatePanel runat="server" ID="updpnl">
    <ContentTemplate>
        <div class="HeadTitle">
            <h3>
                QUẢN LÝ THAM SỐ HỆ THỐNG</h3>
            <span class="fieldRequired">&nbsp;</span>Trường yêu cầu nhập dữ liệu
        </div>
        <div class="Error">
            <div id="divError">
                <ul id="lstErrorContent">
                </ul>
            </div>
        </div>
        <div class="BoxField" id="regionBindData">
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblName" CssClass="TextLabel" runat="server" Text="Tên tham số:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtName" CssClass="TextBox" runat="server" Text="" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblType" CssClass="TextLabel" runat="server" Text="Kiểu dữ liệu:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <asp:DropDownList runat="server" ID="ddlType" CssClass="DropDownList">
                        <asp:ListItem Text="" Value="0" />
                        <asp:ListItem Text="Chuỗi" Value="1" />
                        <asp:ListItem Text="Số" Value="2" />
                        <asp:ListItem Text="Ngày tháng" Value="3" />
                        <asp:ListItem Text="Logic" Value="4" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblVal" CssClass="TextLabel" runat="server" Text="Giá trị:" /><span
                        class="fieldRequired">&nbsp;</span>
                </div>
                <div class="DivTextBox">
                    <div id="IDNotBoolean">
                        <asp:TextBox ID="txtVal" CssClass="TextBox" runat="server" Text="" />
                    </div>
                    <div id="IDIsBoolean">
                        <asp:DropDownList runat="server" ID="ddlBoolean" CssClass="DropDownList">
                            <asp:ListItem Text="True" Value="1" />
                            <asp:ListItem Text="False" Value="0" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="DivRow" style="display:none">
                <div class="DivLabel">
                    <asp:Label ID="lblVal2" CssClass="TextLabel" runat="server" Text="Giá trị (2):" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtVal2" CssClass="TextBox" runat="server" Text="" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblDescription" CssClass="TextLabel" runat="server" Text="Mô tả:" />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtDescription" CssClass="TextBox" runat="server" Text="" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblActivated" CssClass="TextLabel" runat="server" Text="Kích hoạt:" />
                </div>
                <div class="DivTextBox">
                    <asp:CheckBox ID="chkIsActivated" runat="server" />
                </div>
            </div>
             <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Tham số hệ thống:" />
                </div>
                <div class="DivTextBox">
                    <asp:CheckBox ID="chkisSys" runat="server" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    &nbsp;&nbsp;&nbsp;</div>
                <div class="DivTextBox">
                    <asp:Button ID="btnSave" runat="server" Text=" Lưu " CssClass="btn" OnClientClick="javascript:return checkValid();" />
                    <input type="button" value="Xóa trắng" class="btn" onclick="javascript:return resetForm();" />
                </div>
            </div>
            <div style="display: none">
                <input type="hidden" id="hidID" value="" runat="server" />
            </div>
            <div class="Clearboth">
                &nbsp</div>
        </div>
        <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
            AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtnEdit" CausesValidation="False" runat="server" Text="Sửa"
                            OnClick="lnkbtnEdit_Click"></asp:LinkButton>&nbsp;|
                        <asp:LinkButton ID="lnkbtnDelete" Text="Xóa" runat="server" OnClick="lnkbtnDelete_Click"
                            CausesValidation="False"></asp:LinkButton>
                    </ItemTemplate>
                     <ItemStyle HorizontalAlign="Center" Width="7%" />
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Tên tham số">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width='10%' />
                </asp:BoundField >
                <asp:TemplateField HeaderText="Giá trị" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblVal" Text='<%# Bind("Val") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="8%" HorizontalAlign="left" />
                </asp:TemplateField>
   <%--             <asp:TemplateField HeaderText="Giá trị (2)">  
                    <HeaderStyle HorizontalAlign="Center" />                  
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblVal2" Text='<%# Bind("Val2") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="8%" HorizontalAlign="left" />
                </asp:TemplateField>--%>
                <asp:BoundField DataField="Type" HeaderText="Kiểu dữ liệu">
                     <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width='5%' />
                </asp:BoundField >
                <asp:BoundField DataField="Description" HeaderText="Mô tả" >
                     <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width='15%' />
                </asp:BoundField >
                <asp:TemplateField HeaderText="Kích hoạt">                
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>                    
                        <asp:CheckBox ID="chkIsActivated" runat="server" Enabled="False" Checked='<%# Bind("Activated") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:TemplateField>

                <%--<asp:TemplateField>
                    <HeaderTemplate>
                        <span>Activated</span></HeaderTemplate>
                        
                    <ItemTemplate>
                        <asp:CheckBox ID="chkIsActivated" runat="server" Enabled="False" Checked='<%# Bind("Activated") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                
            </Columns>
            <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
            <RowStyle CssClass="GridRow" Height="23px" />
            <AlternatingRowStyle CssClass="GridAlterRow" />
            <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
            <PagerSettings Mode="Numeric" PageButtonCount="2" />
            <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
        </asp:GridView>
        
    </ContentTemplate>
</asp:UpdatePanel>
