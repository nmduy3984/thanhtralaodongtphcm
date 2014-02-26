<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhanTichTinhHinhViPhamLD1.ascx.vb" Inherits="Control_Report_PhanTichTinhHinhViPhamLD1" %>
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
    function CheckOrUnCheck(id, status,NameCtrl) {
        var newId;
        //Neu click vao checkbox
        if (id.indexOf("CheckBox") != -1) {
            newId = id.replace("CheckBox", "Nodes");
        }
        else { //Click vao link
            newId = id.replace("trv" + NameCtrl + "t", "trv"+ NameCtrl +"n");
            newId = newId + "Nodes";
        }
        $("#" + newId).attr("checked", status);
        //check or uncheck his child
        $("#" + newId + " input").each(function () {
            $(this).attr("checked", status);
        });
    }

    function SetHeader() {
        var valueType = $("#SearchType option:selected").val();
        var reportName = $("#<%=lblReportName.ClientID %>");
        switch (valueType * 1) {
            case 1: // ẩn đi 2 div năm và Loại hình sản xuất
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH VI PHẠM LAO ĐỘNG THEO LOẠI HÌNH DOANH NGHIỆP");
                $('#divExt').hide();               
                break;
            case 2:// Hiển thị div LHSX
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH VI PHẠM LAO ĐỘNG THEO LOẠI HÌNH SẢN XUẤT");
                $('#divNam').hide();
                $('#divLHSX').show();
                break;
            case 3:  // Hiển thị div Năm
                reportName.text("BÁO CÁO PHÂN TÍCH TÌNH HÌNH VI PHẠM LAO ĐỘNG THEO NĂM");
                $('#divNam').show();
                $('#divLHSX').hide();
                break;
        }
    }
    //kiểm tra user nhập
    function checkValid() {
        var error = "";
        var countbug = 0;
        var result = true;
        switch ($("#SearchType option:selected").val() * 1) {
            case 2:
                var checkedItem = $("#<% =trvLHSX.ClientId() %> input:checked").size();
                if (checkedItem > 5) {
                    result = false;
                    Alertbox("Bạn đã chọn nhiều hơn 5 mục loại hình sản xuất để báo cáo.\nHãy chọn lại số mục tối đa là 5.");
                } else if (checkedItem == 0) {
                    result = false;
                    Alertbox("Bạn chưa chọn mục loại hình sản xuất nào để báo cáo.\nHãy chọn ít nhất là 1 mục và tối đa là 5 mục để báo cáo");
                }
                break;
            case 3:
                var yearFrom = $("#<%=txtYearFrom.ClientID %>").val();
                var yearTo = $("#<%=txtYearTo.ClientID %>").val();
                if (yearFrom == "") {
                    countbug += 1;
                    error = "<li>Vui lòng nhập từ năm </li>"
                } else if (yearTo == "") {
                    countbug += 1;
                    error = "<li>Vui lòng nhập đến năm </li>"
                } else if (yearFrom != "" && yearTo != "" && yearFrom * 1 > yearTo * 1) {
                    countbug += 1;
                    error = "<li>Vui lòng nhập từ năm nhỏ hơn đến năm </li>"
                } else if (yearTo * 1 - yearFrom * 1 >= 5) {
                    result = false;
                    Alertbox("Bạn đã chọn một giới hạn năm lơn 5 năm để báo cáo.\nHãy chọn lại giới hạn của năm tối đa là 5.");
                }
                break;
        }
        var checkedItemViPham = $("#<% =trvViPham.ClientId() %> input:checked").size();
        if (checkedItemViPham == 0) {
            result = false;
            Alertbox("Bạn chưa chọn mục vi phạm nào để báo cáo.\nHãy chọn ít nhất là 1 mục để báo cáo");
        }

        if (countbug >= 1) {
            //remove all old error
            $('#lstErrorContent li').remove();
            //show new list bug
            $("#lstErrorContent").append(error);
            $('html, body').animate({ scrollTop: $(".Error").height() }, 500);
            result = false
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
                <div id="SearchType" style="float:left;width:25%; margin-right:15px" >
                    <div class="DivLabel">
                        <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Chọn mục báo cáo theo: " />                  
                    </div>
                    <div class="DivTextBox" >
                        <asp:DropDownList runat="server" ID="ddlType" CssClass="DropDownList" AutoPostBack="true" style="width:195px;">  
                            <asp:ListItem Text="Loại hình doanh nghiệp" Value="1" /> 
                            <asp:ListItem Text="Loại hình Sản Xuất" Value="2" />
                            <asp:ListItem Text="Năm" Value="3" />
                        </asp:DropDownList>                        
                    </div>
                </div>
                <div id ="divExt" style="float:left;width:33%; margin-right:30px;">
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
                <div style="float:left;width:33%;">
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