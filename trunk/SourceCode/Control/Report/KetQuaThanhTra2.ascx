﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="KetQuaThanhTra2.ascx.vb" Inherits="Control_Report_KetQuaThanhTra2" %>
<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            SetHeader();
            $("#<%=ddlType.ClientID %>").change(function (event) {
                SetHeader();
            });

            // Handler cho Checkbox khi click vào 
            $("#<% =trvTinh.ClientID %> input, #<% =trvChuyenMuc.ClientId() %> input").click(function () {
                var id = $(this).attr('id');
                CheckOrUnCheck(id, $(this).attr("checked"));

                var divParent = $(this).parent();

            });
            //Click vao nhung link trong TreeView ViPham
            $("#<% =trvChuyenMuc.ClientID %> a:not(:has(img))").click(function () {
                //Lay id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = $(this).parent().find("input:checkbox:first").attr('id');

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)                
                $("#" + sameCheckBoxId).attr("checked", !$("#" + sameCheckBoxId).attr("checked"));
                CheckOrUnCheck(sameCheckBoxId, $("#" + sameCheckBoxId).attr("checked"), "trvChuyenMuc");
                return false;
            });
            //Click vao nhung link trong TreeView ViPham
            $("#<% =trvTinh.ClientID %> a:not(:has(img))").click(function () {
                //Lay id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = $(this).parent().find("input:checkbox:first").attr('id');

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)                
                $("#" + sameCheckBoxId).attr("checked", !$("#" + sameCheckBoxId).attr("checked"));
                CheckOrUnCheck(sameCheckBoxId, $("#" + sameCheckBoxId).attr("checked"), "trvTinh");
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
        var valueType = $("#SearchType option:selected").val();
        var reportName = $("#<%=lblReportName.ClientID %>");
        switch (valueType*1) {
            case 1: // Hiển thị divTinhThanh
                reportName.text("BÁO CÁO TỔNG HỢP KẾT QUẢ THANH TRA THEO TỈNH/THÀNH PHỐ");
                $('#divTinh').show();
                $('#divNam').hide();
                $('#divChuyenMuc').hide();
                break;
            case 2: // Hiển thị div Năm
                reportName.text("BÁO CÁO TỔNG HỢP KẾT QUẢ THANH TRA THEO NĂM");
                $('#divTinh').hide();
                $('#divNam').show();
                $('#divChuyenMuc').hide();
                break;
            case 3: // Hiển thị div Chuyên mục
                reportName.text("BÁO CÁO TỔNG HỢP KẾT QUẢ THANH TRA THEO CHUYÊN MỤC");
                $('#divTinh').hide();
                $('#divNam').hide();
                $('#divChuyenMuc').show();
                break;
        }
    }
    //kiểm tra user nhập
    function checkValid() {
        var error = "";
        var countbug = 0;
        var result = true;
        switch ($("#SearchType option:selected").val()*1) {
            case 1:
            var checkedItem= $("#<% =trvTinh.ClientId() %> input:checked").size();
                if ( checkedItem> 5) {
                    Alertbox("Bạn đã chọn nhiều hơn 5 mục để báo cáo.\nHãy chọn lại số mục tối đa là 5.");
                    result = false;
                } else if (checkedItem == 0) {
                    Alertbox("Bạn chưa chọn mục nào để báo cáo.\nHãy chọn ít nhất là 1 mục và tối đa là 5 mục để báo cáo");
                    result = false;
                }
                break;
            case 2:
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
                } else if (yearTo * 1 - yearFrom * 1 > 5) {
                    result = false;
                    Alertbox("Bạn đã chọn một giới hạn năm lơn 5 năm để báo cáo.\nHãy chọn lại giới hạn của năm tối đa là 5.");
                }

                break;
            case 3:
                var checkedItem = $("#<% =trvChuyenMuc.ClientId() %> input:checked").size();
                if (checkedItem > 5) {
                    result = false;
                    Alertbox("Bạn đã chọn nhiều hơn 5 mục để báo cáo.\nHãy chọn lại số mục tối đa là 5.");
                } else if (checkedItem == 0) {
                    result = false;
                    Alertbox("Bạn chưa chọn mục nào để báo cáo.\nHãy chọn ít nhất là 1 mục và tối đa là 5 mục để báo cáo");
                }
                break;
        }
       
        if (countbug >= 1) {
            //remove all old error
            $('#lstErrorContent li').remove();
            //show new list bug
            $("#lstErrorContent").append(error);
            $('html, body').animate({ scrollTop: $(".Error").height() }, 500);
            result=false
        }
        return result;
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
                <div id="SearchType" style="float:left; margin-right:100px" >
                    <div class="DivLabel">
                        <asp:Label ID="Label3" CssClass="TextLabel" runat="server" Text="Chọn mục báo cáo theo: " />
                  
                    </div>
                    <div class="DivTextBox" >
                        <asp:DropDownList runat="server" ID="ddlType" CssClass="DropDownList" AutoPostBack="true"/>                           
                    </div>
                </div>
                <div style="float:left;" >
                    <div id="divTinh">
                         <div class="DivLabel">
                            <asp:Label ID="lblAuditnumber" CssClass="TextLabel" runat="server" Text="Tỉnh thành:" />
                        </div>
                        <br />
                        <div style="height:260px; width:200px;overflow:auto;">
                            <div class="DivTextBox" >
                                <asp:TreeView ID="trvTinh" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                                    ShowLines="True" CssClass="tree">
                                </asp:TreeView>
                            </div>
                        </div>
                    </div>
                    <div id="divNam" style="display:none">                    
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label2" runat="server" Text="Từ năm:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtYearFrom" CssClass="TextBox Date" runat="server" MaxLength="4"/>
                            </div>
                        </div>
                        <div class="DivRow">
                             <div class="DivLabel">
                                <asp:Label ID="Label4" runat="server" Text="đến năm:" />
                            </div>
                            <div class="DivTextBox">
                                <asp:TextBox ID="txtYearTo" CssClass="TextBox Date" runat="server" MaxLength="4"/>
                            </div>
                        </div>
                    </div>
                    <div id="divChuyenMuc" style="display:none">
                        <div class="DivRow">
                            <div class="DivLabel">
                                <asp:Label ID="Label1" runat="server" Text="Chọn chuyện muc:" />
                            </div>
                            <br />
                            <div class="DivTextBox">
                               <asp:TreeView ID="trvChuyenMuc" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                                ShowLines="True" CssClass="tree">
                            </asp:TreeView>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 20px;">
                </div>
                <asp:Button ID="btnExport" runat="server" Text="Tìm Kiếm" CausesValidation="false"
                    CssClass="btn" OnClientClick="javascript:return checkValid();"/>
            </div>
            <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                <Columns>
                    <asp:BoundField DataField='Name'>
                        <ItemStyle Width='20%' />
                        <HeaderStyle CssClass="HeaderHidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col1' HeaderText="Số lượng" DataFormatString="{0:n0}">
                        <ItemStyle Width='8%'  />
                    </asp:BoundField>
                    <asp:BoundField DataField='P_Col1' HeaderText="%" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%'  />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col2' HeaderText="Số lượng" DataFormatString="{0:n0}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>                   
                    <asp:BoundField DataField='P_Col2' HeaderText='%' DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col3' HeaderText="Số lượng" DataFormatString="{0:n0}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='P_Col3' HeaderText="%" DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>
                    <asp:BoundField DataField='Col4' HeaderText="Số lượng" DataFormatString="{0:n0}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>                   
                    <asp:BoundField DataField='P_Col4' HeaderText='%' DataFormatString="{0:n2}">
                        <ItemStyle Width='8%' />
                    </asp:BoundField>  
                    <asp:BoundField DataField='Col5' HeaderText="Số lượng" DataFormatString="{0:n0}">
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