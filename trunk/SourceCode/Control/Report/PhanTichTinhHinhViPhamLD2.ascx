<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhanTichTinhHinhViPhamLD2.ascx.vb" Inherits="Control_Report_PhanTichTinhHinhViPhamLD2" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            SetHeader();
            $("#<%=ddlType.ClientID %>").change(function (event) {
                SetHeader();
            });


            // Băt sự kiện khi nhập dữ liệu và nhấn Enter
            $("#divNam input[type=text]").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#btnExportShow").click();
                    return false;
                }
            });

            // Bỏ đi sự kiện enter trên các checkbox của TreeView
            $(".tree input").keypress(function (e) {
                return e.keyCode != 13;
            });

            // Handler cho Checkbox khi click vào 
            $("#<% =trvViPham.ClientId() %> input, #<% =trvLHSX.ClientId() %> input").click(function () {
                var id = $(this).attr('id');
                var nameCtrl = id.substring(id.lastIndexOf("_") + 1, id.length - 8);
                CheckOrUnCheck(id, $(this).attr("checked"), nameCtrl);
            });
            //Click vao nhung link trong TreeView 
            $("#<% =trvViPham.ClientId() %> a:not(:has(img)),#<% =trvLHSX.ClientId() %> a:not(:has(img))").click(function () {
                //Lay id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = $(this).parent().find("input:checkbox:first").attr('id');

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)                
                $("#" + sameCheckBoxId).attr("checked", !$("#" + sameCheckBoxId).attr("checked"));
                var nameCtrl = $(this).attr('id').substring($(this).attr('id').lastIndexOf("_") + 1, $(this).attr('id').length);
                CheckOrUnCheck(sameCheckBoxId, $("#" + sameCheckBoxId).attr("checked"), nameCtrl);
                return false;
            });
        });
    }

    // Check hoặc uncheck control checkbox
    function CheckOrUnCheck(id, status, NameCtrl) {
        var newId;
        //Neu click vao checkbox
        if (id.indexOf("CheckBox") != -1) {
            newId = id.replace("CheckBox", "Nodes");
        }
        else { //Click vao link
            newId = id.replace("trv" + NameCtrl + "t", "trv" + NameCtrl + "n");
            newId = newId + "Nodes";
        }
        $("#" + newId).attr("checked", status);
        //check or uncheck his child
        $("#" + newId + " input").each(function () {
            $(this).attr("checked", status);
        });
    }

    function SetHeader() {
        var valueType = $("#SearchType option:selected").val() *1 ;
        var reportName = $("#<%=lblReportName.ClientID %>");
        switch (valueType) {
            case 1: //Mẫu 4 - ẩn đi 2 div năm và Loại hình sản xuất
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH LAO ĐỘNG THEO LOẠI HÌNH DOANH NGHIỆP \nCỦA CÁC DOANH NGHIỆP CÓ LĐ LÀM CÔNG VIỆC ĐỘC HẠI / YÊU CẦU NGHIÊM NGẶT VỀ ANLĐ");
                $('#divExt').hide();
                $('#divViPham').show();
                break;
            case 2: // Mẫu 5 -  Hiển thị div LHSX
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH LAO ĐỘNG THEO LOẠI HÌNH SẢN XUẤT \nCỦA CÁC DOANH NGHIỆP CÓ LĐ LÀM CÔNG VIỆC ĐỘC HẠI / YÊU CẦU NGHIÊM NGẶT VỀ ANLĐ");
                $('#divNam').hide();
                $('#divExt').css('width', '30%');
                $('#divLHSX').show();
                $('#divViPham').show();
                break;
            case 3:  // Mẫu 6 -  Hiển thị div Năm
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH LAO ĐỘNG THEO NĂM \nCỦA CÁC DOANH NGHIỆP CÓ LĐ LÀM CÔNG VIỆC ĐỘC HẠI / YÊU CẦU NGHIÊM NGẶT VỀ ANLĐ");
                $('#divNam').show();
                $('#divExt').css('width', '20%');
                $('#divLHSX').hide();
                $('#divViPham').show();

//                // Gán năm hiện tại vào textbox
//                var nowYear = new Date().getFullYear();
//                $(".Date").val(nowYear);
                break;
            case 4: // Mẫu 7 -  Hiển thị div LHSX
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH LAO ĐỘNG THEO CÁC YẾU TỐ LIÊN QUAN \nCỦA CÁC DOANH NGHIỆP CÓ LĐ LÀM CÔNG VIỆC ĐỘC HẠI / YÊU CẦU NGHIÊM NGẶT VỀ ANLĐ");
                $('#divExt').hide();
                $('#divViPham').show();
                break;
            case 5: // Mẫu 8 - 
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH XỬ LÝ VI PHẠM THEO LOẠI HÌNH DOANH NGHIỆP ");
                $('#divExt').hide();
                $('#divViPham').hide();
                break;
            case 6: // Mẫu 9 - 
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH XỬ LÝ VI PHẠM THEO ĐỊA PHƯƠNG ");
                $('#divExt').hide();
                $('#divViPham').hide();
                break;
            case 7: // Mẫu 10 - 
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH TAI NẠN LAO ĐỘNG THEO LĨNH VỰC SẢN XUẤT/ GIAI ĐOẠN");
                $('#divNam').css('width', '150px');
                $('#divNam').show();
                $('#divLHSX').hide();
                $('#divViPham').hide();

//                // Gán năm hiện tại vào textbox
//                var nowYear = new Date().getFullYear();
//                $(".Date").val(nowYear);
                break;
            case 8: // Mẫu 11 - 
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH TAI NẠN LAO ĐỘNG THEO ĐỊA PHƯƠNG/ GIAI ĐOẠN ");
                $('#divNam').css('width', '150px');
                $('#divNam').show();
                $('#divLHSX').hide();
                $('#divViPham').hide();

//                // Gán năm hiện tại vào textbox
//                var nowYear = new Date().getFullYear();
//                $(".Date").val(nowYear);
                break;
        }
    }
    //kiểm tra user nhập
    function checkValid() {
        var error = "";
        var countbug = 0;
        var result = true;
        var valueSearchType = ($("#SearchType option:selected").val() * 1);
        if (valueSearchType != 5 && valueSearchType != 6) {
            var checkedItemViPham = $("#<% =trvViPham.ClientId() %> input:checked").size();
            switch (valueSearchType) {
                case 1:
                    if (checkedItemViPham == 0) {
                        result = false;
                        Alertbox("Bạn chưa chọn mục vi phạm nào để báo cáo.\nHãy chọn ít nhất là 1 mục để báo cáo");
                    }
                    break;
                case 2:
                    // Xét xem có chọn lỗi vi phạm để thông kê chưa ?
                    if (checkedItemViPham == 0) {
                        result = false;
                        Alertbox("Bạn chưa chọn mục vi phạm nào để báo cáo.\nHãy chọn ít nhất là 1 mục để báo cáo");
                    }
                    var checkedLHSXItem = $("#<% =trvLHSX.ClientId() %> input:checked").size();
                    if (checkedLHSXItem > 5) {
                        result = false;
                        Alertbox("Bạn đã chọn nhiều hơn 5 mục loại hình sản xuất để báo cáo.\nHãy chọn lại số mục tối đa là 5.");
                    } else if (checkedLHSXItem == 0) {
                        result = false;
                        Alertbox("Bạn chưa chọn mục loại hình sản xuất nào để báo cáo.\nHãy chọn ít nhất là 1 mục và tối đa là 5 mục để báo cáo");
                    }

                    break;
                case 3:
                    // Xét xem có chọn lỗi vi phạm để thông kê chưa ?
                    if (checkedItemViPham == 0) {
                        result = false;
                        Alertbox("Bạn chưa chọn mục vi phạm nào để báo cáo.\nHãy chọn ít nhất là 1 mục để báo cáo");
                    }
                    var yearFrom = $("#<%=txtYearFrom.ClientID %>").val();
                    var yearTo = $("#<%=txtYearTo.ClientID %>").val();
                    if (yearFrom == "") {
                        //countbug += 1;
                        //error = "<li>Vui lòng nhập từ năm </li>"
                        result = false;
                        Alertbox("Vui lòng nhập từ năm ");
                    } else if (yearTo == "") {
                        result = false;
                        Alertbox("Vui lòng nhập đến năm");
                    } else if (yearFrom.length < 4 || yearTo.length <4) {
                        result = false;
                        Alertbox("Bạn nhập sai kiểu năm");
                    }else if (yearFrom != "" && yearTo != "" && yearFrom * 1 > yearTo * 1) {
                        result = false;
                        Alertbox("Vui lòng nhập từ năm nhỏ hơn đến năm");
                    } else if (yearTo * 1 - yearFrom * 1 > 5) {
                        result = false;
                        Alertbox("Bạn đã chọn một giới hạn năm lơn 5 năm để báo cáo.\nHãy chọn lại giới hạn của năm tối đa là 5.");
                    }
                    break;
                case 4:
                    // Xét xem có chọn lỗi vi phạm để thông kê chưa ?
                    if (checkedItemViPham == 0) {
                        result = false;
                        Alertbox("Bạn chưa chọn mục vi phạm nào để báo cáo.\nHãy chọn ít nhất là 1 mục để báo cáo");
                    }
                    break;
                case 7:
                    var yearFrom = $("#<%=txtYearFrom.ClientID %>").val();
                    var yearTo = $("#<%=txtYearTo.ClientID %>").val();
                    if (yearFrom != "" && yearTo != "" && yearFrom * 1 > yearTo * 1) {
                        //countbug += 1;
                        //error = "<li>Vui lòng nhập từ năm nhỏ hơn đến năm </li>"
                        result = false;
                        Alertbox("Vui lòng nhập từ năm nhỏ hơn đến năm");
                    }
                    break;
                case 8:
                    var yearFrom = $("#<%=txtYearFrom.ClientID %>").val();
                    var yearTo = $("#<%=txtYearTo.ClientID %>").val();
                    if (yearFrom != "" && yearTo != "" && yearFrom * 1 > yearTo * 1) {
                        result = false;
                        Alertbox("Vui lòng nhập từ năm nhỏ hơn đến năm");
                    }
                    break;
            }
        }
        if (result == true) {
            $("#<%=btnExport.ClientID()%>").click();
            return false;
        } else {
            return false;
        }
    }  
</script>
<style type="text/css">
 .HeaderHidden{display:none}
</style>
<asp:UpdatePanel ID="uplDailyDebt" runat="server">
    <ContentTemplate>
        <div class="BoxField" >
            <div class="HeadTitle">
                <h3>
                    <asp:Label ID="lblReportName" runat="server" Text="" />
                </h3>
            </div>
           <%-- <div class="DivRequired Center">
                <span class="fieldRequired">&nbsp;</span>
            </div>--%>
            <div class="Error-Center">
                <div id="divError">
                    <ul id="lstErrorContent">
                    </ul>
                </div>
            </div>
            <div style="clear: both; height: 15px;">
            </div>
            <div id="SearchCondition">
                <div id="SearchType" style="float:left; padding-right:10px" >
                    <div class="DivLabel">
                        <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Chọn mục báo cáo theo: " />                  
                    </div>
                    <div class="DivTextBox" style="width:330px;">
                        <asp:DropDownList runat="server" ID="ddlType" CssClass="DropDownList" AutoPostBack="true" style="width:315px;">  
                            <asp:ListItem Text="Loại hình doanh nghiệp" Value="1" /> <%-- Mẫu 4--%>
                            <asp:ListItem Text="Loại hình Sản Xuất" Value="2" /><%-- Mẫu 5--%>
                            <asp:ListItem Text="Năm" Value="3" />  <%--Mẫu 6--%>
                            <asp:ListItem Text="Tình hình vi phạm theo các yếu tố liên quan" Value="4" /><%-- Mẫu 7--%>
                            <asp:ListItem Text="Tình hình xử lý vi phạm theo loại hình doanh nghiệp" Value="5" /><%--Mẫu 8--%>
                            <asp:ListItem Text="Tình hình xử lý vi phạm theo địa phương" Value="6" /><%--Mẫu 9--%>
                            <asp:ListItem Text="Tình hình tai nạn lao động theo lĩnh vực sản xuất" Value="7" /><%--Mẫu 10--%>
                            <asp:ListItem Text="Tình hình tai nạn lao động theo địa phương" Value="8" /><%--Mẫu 11--%>
                        </asp:DropDownList>                        
                    </div>
                </div>
                <div id ="divExt" style="float:left;padding-right:10px;">
                    <div id="divLHSX" style="display:none">
                        <div class="DivRow" >
                            <div class="DivLabel" style="width:155px;">
                                <asp:Label ID="Label1" runat="server" Text="Chọn Loại hình sản xuất:" />
                            </div>
                            <br />
                            <div style="height:300px;overflow:auto">
                                <div class="DivTextBox" >
                                   <asp:TreeView ID="trvLHSX" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                                        ShowLines="True" CssClass="tree">
                                    </asp:TreeView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divNam" style="display:none">                    
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label2" runat="server" Text="Từ năm:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtYearFrom" CssClass="TextBox Date" runat="server" MaxLength="4" onkeypress='javascript:return isNumberKey(event);'/>
                            </div>
                        </div>
                        <div class="DivRow">
                             <div class="DivLabel">
                                <asp:Label ID="Label4" runat="server" Text="đến năm:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtYearTo" CssClass="TextBox Date" runat="server" MaxLength="4" onkeypress='javascript:return isNumberKey(event);'/>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divViPham" style="float:left;width:30%;">
                    <div class="DivRow">
                        <div class="DivLabel">
                            <asp:Label ID="Label5" runat="server" Text="Chọn mục vi phạm:" />
                        </div>
                        <br />
                        <div style="height:300px;overflow:auto;">
                            <div class="DivTextBox">
                                <asp:TreeView ID="trvViPham" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                                    ShowLines="True" CssClass="tree" EnableTheming="True">
                                </asp:TreeView>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 20px;">
                </div>
                <input type="button" ID="btnExportShow" value="Tìm Kiếm" class="btn" onClick="javascript:return checkValid();"/>

                <span style="display:none">
                <asp:Button ID="btnExport" runat="server" Text="Tìm Kiếm" CausesValidation="false"
                    CssClass="btn" /></span>
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='Name'>
                        <ItemStyle Width='20%' />
                        <HeaderStyle CssClass="HeaderHidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col1' HeaderText="Số DN" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%'  />
                    </asp:BoundField>
                    <asp:BoundField DataField='P_Col1' HeaderText="%" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%'  />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col2' HeaderText="Số DN" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>                   
                    <asp:BoundField DataField='P_Col2' HeaderText='%' DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col3' HeaderText="Số DN" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='P_Col3' HeaderText="%" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col4' HeaderText="Số DN" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>                   
                    <asp:BoundField DataField='P_Col4' HeaderText='%' DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>  
                    <asp:BoundField DataField='Col5' HeaderText="Số DN" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>                   
                    <asp:BoundField DataField='P_Col5' HeaderText='%' DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' /> 
                    </asp:BoundField>                  
                </Columns>
                <HeaderStyle Height="26px" HorizontalAlign="Center" CssClass="GridHeader" />
                <RowStyle CssClass="GridRow" Height="23px" />
                <AlternatingRowStyle CssClass="GridAlterRow" />
                <PagerStyle CssClass="GridPaged" HorizontalAlign="Right" Height="23px" />
                <PagerSettings Mode="Numeric" PageButtonCount="2" />
                <EmptyDataRowStyle CssClass="GridTextEmpty" HorizontalAlign="Center" />
            </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>