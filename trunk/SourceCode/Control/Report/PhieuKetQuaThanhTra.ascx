<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhieuKetQuaThanhTra.ascx.vb" Inherits="Control_Report_PhieuKetQuaThanhTra" %>

<script type="text/javascript">
    function ajaxJquery() {
        $(function () {
            SetHeader();
            $("#SearchCondition input,select").change(function (event) {
                SetHeader();
            });

            // Băt sự kiện khi nhập dữ liệu và nhấn Enter
            $("#SearchCondition input[type=text]").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#btnExportShow").click();
                    return false;
                }
            });
            var hidTinhThanhTraSo = $("#<%=hidTinhThanhTraSo.ClientID %>").val();
            if (hidTinhThanhTraSo == 0) {
                $(".WrapHuyen").css("display", "none");
                $(".WrapKhuCongNghiep").css("display", "none");
            }
            //Load huyện theo tỉnh
            $("#<%=ddlProvince.ClientId %>").live("change", function () {
                LoadHuyen($(this));
            });
        });
    }
    function LoadHuyen(obj) {
        $(".Huyen select option").remove();
        $.ajax({
            url: '<%=ResolveUrl("~/Services/wsAutoComplete.asmx/LayDSHuyen")%>',
            data: "{'TinhId': '" + obj.val() + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) {
                return data;
            },
            success: function (data) {
                var tdcol2 = "<option value='0'>--- Chọn ---</option>";
                if (data.d.length > 0) {
                    for (i = 0; i < data.d.length; i++) {
                        if (data.d[i].ID.trim().length > 0) {
                            tdcol2 += "<option value='" + data.d[i].ID + "'>" + data.d[i].FullText + "</option>";
                        }
                    }

                }
                $(".Huyen select").append(tdcol2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
    //Set Header for Report
    function SetHeader() {
        // Set header for Report
        var reportName = $("#<%=lblReportName.ClientID %>");
        var tinh = $("#<%=ddlProvince.ClientID %> option:selected").text();
        var tuNam = $("#<%=txtYearFrom.ClientID %>").val();
        var denNam = $("#<%=txtYearTo.ClientID %>").val();
        reportName.text("BẢNG TỔNG HỢP KẾT QUẢ TỰ KIỂM TRA TP. Hồ Chí Minh" + (tuNam == "" ? "" : (" TỪ NĂM  " + tuNam)) + (denNam == "" ? "" : ("   ĐẾN NĂM  " + denNam)));
    }
    //kiểm tra user nhập
    function checkValid() {

        var yearFrom = $("#<%=txtYearFrom.ClientID %>").val();
        var yearTo = $("#<%=txtYearTo.ClientID %>").val();
        var error = "";
        var countbug = 0;

        if (yearFrom != "" && yearTo != "" && yearFrom * 1 > yearTo * 1) {
            countbug += 1;
            error = "<li>Vui lòng nhập từ năm nhỏ hơn đến năm </li>";
        }

        if (countbug >= 1) {
            //remove all old error
            $('#lstErrorContent li').remove();
            //show new list bug
            $("#lstErrorContent").append(error);
            $('html, body').animate({ scrollTop: $(".Error").height() }, 500);
            return false;
        } else {
            $("#<%=btnExport.ClientID()%>").click();
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
            <div id="SearchCondition"  class="Div-Center">
                <div class="DivRow">
                    <div class="DivLabel">
                        <%--<span class="fieldRequired"></span>--%>
                        <asp:Label ID="Label1" runat="server" Text=" Địa phương " />
                    </div>
                    <div class="DivTextBox">
                        <asp:DropDownList ID="ddlProvince" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow WrapHuyen">
                    <div class="DivLabel">
                         <asp:Label ID="Label3" runat="server" Text=" Quận/Huyện " />
                    </div>
                    <div class="DivTextBox Huyen">
                        <asp:DropDownList ID="ddlHuyen" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
                <div class="DivRow WrapKhuCongNghiep">
                    <div class="DivLabel">
                         <asp:Label ID="Label5" runat="server" Text=" Khu công nghiệp " />
                    </div>
                    <div class="DivTextBox KhuCongNghiep">
                        <asp:DropDownList ID="ddlKhuCongNghiep" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
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
                <%--<div class="DivRow">
                     <div class="DivLabel">
                        <asp:Label ID="Label3" runat="server" Text="Chuyên mục" />
                    </div>
                    <div class="DivTextBox">
                        <asp:TextBox ID="TextBox1" CssClass="TextBox Date" runat="server" MaxLength="4"/>
                    </div>
                </div>--%>

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
                    <%--<asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <ItemStyle Width="4%" HorizontalAlign="Left" />
                    </asp:TemplateField>--%> 
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
            <div>                
                    
        </div>
        <div style="display:none">
            <asp:HiddenField ID="hidTinhThanhTraSo" Value="0" runat="server" />
            </div>
    </ContentTemplate>
</asp:UpdatePanel>