﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PopupSelectKienNghi.ascx.vb"
    Inherits="Control_KienNghi_PopupSelectKienNghi" %>
<script type='text/javascript'>
    function ajaxJquery() {
        $(function () {

            $("#frmSearch input").keypress(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnFilter.ClientID()%>").click();
                    return false;
                }
            });
            // Gán lại giá trị null khi chuyển trang
            $("#<%=txtListKienNghiID.ClientID %>").val('');
            $("#<%=txtListOld.ClientID %>").val(window.parent.GetKienNghis());

            //TreeView
            $("#<% =trvMenu.ClientId() %> input").click(function () {
                var id = $(this).attr('id');
                CheckOrUnCheck(id, $(this).attr("checked"))

            });
            //Click vao nhung link trong trvMenu co class chua Content_trvMenu
            $("#<% =trvMenu.ClientId() %> [class*=_trvMenu_]").click(function () {
                //Lay id cua link vua click
                var id = $(this).attr('id');
                //Lay Id cua checkbox tuong ung voi link vua click
                var sameCheckBoxId = id.replace("trvMenut", "trvMenun");
                sameCheckBoxId = sameCheckBoxId + "CheckBox";

                //Check or uncheck checkbox tuong ung voi link vua click(checkbox cha)
                if ($("#" + sameCheckBoxId).attr("checked") == true) {
                    $("#" + sameCheckBoxId).attr("checked", false);
                }
                else {

                    $("#" + sameCheckBoxId).attr("checked", true);
                }

                CheckOrUnCheck(id, $("#" + sameCheckBoxId).attr("checked"));
                return false;
            });
        });
    }
    //CheckOrUnCheck TreeView
    function CheckOrUnCheck(id, status) {
        var newId;
        //Neu click vao checkbox
        if (id.indexOf("CheckBox") != -1) {

            newId = id.replace("CheckBox", "Nodes");
        }
        else { //Click vao link
            newId = id.replace("trvMenut", "trvMenun");
            newId = newId + "Nodes";
        }
        $("#" + newId).attr("checked", status);
        //check or uncheck his child
        $("#" + newId + " input").each(function () {
            $(this).attr("checked", status);
        });
    }
    function CheckAll() {
        //xử lý checkall
        if ($("#<%= grdShow.ClientID %> tr th span").find("input[type=checkbox]").attr("checked") == true) {
            if ($("#<%=grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('disabled') != true) {

                $("#<%=grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('checked', true);
            }
        } else {

            $("#<%=grdShow.ClientID %> tr td").find("input[type=checkbox]").attr('checked', false);
        }
    }
    function SelectKienNghi() {
        var txtListKienNghiID = $("#<%=txtListKienNghiID.ClientID %>");
        var lstId = "#";
        $("#<%=grdShow.ClientId() %> tr:not(:has(th))").find("input:checked").each(function () {
            var ID = $(this).parent().parent().find(".clKienNghiID").text().trim();
            lstId += ID + "#";
        });
        txtListKienNghiID.val(lstId);

        var KienNghiID = $("#<%=txtListKienNghiID.ClientID %>").val() + '#' + $("#<%=txtListOld.ClientID %>").val();
        if (KienNghiID.trim() != "")
            window.parent.SetKienNghiID(KienNghiID);
        window.parent.tb_remove();
        return false;
    }
     
</script>
<asp:UpdatePanel ID="uplContents" runat="server">
    <ContentTemplate>
        <div class="BoxField" style="background-color: #ffffff; padding-top: 8px;" id="frmSearch">
            <%-- <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="Label1" CssClass="TextLabel" runat="server" Text="Nhập mã kiến nghị: " />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtKienNghiID" CssClass="TextBox" runat="server" Text="" Width="208px" />
                </div>
            </div>--%>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblGroupname" CssClass="TextLabel" runat="server" Text="Nội dung kiến nghị: " />
                </div>
                <div class="DivTextBox">
                    <asp:TextBox ID="txtKienNghiName" CssClass="TextBox" runat="server" Text="" Width="208px" />
                    &nbsp;<asp:Button ID="btnFilter" runat="server" Text=" Tìm kiếm " CssClass="btn" />
                </div>
            </div>
            <div class="DivRow">
                <div class="DivLabel">
                    <asp:Label ID="lblMucvipham" runat="server" Text="Mục vi phạm:" class="SubjectTitle"
                        Style="width: 66%; text-align: right; float: left" />
                </div>
                <div class="DivTextBox">
                    <asp:TreeView ID="trvMenu" runat="server" LineImagesFolder="~/TreeLineImages" ShowCheckBoxes="All"
                        Style="padding-left: 28%" ShowLines="True" CssClass="tree">
                    </asp:TreeView>
                </div>
            </div>
            <div class="DivRow" style="padding-top: 10px">
                <asp:GridView ID="grdShow" runat="server" AutoGenerateColumns="False" EmptyDataText="{Không có dữ liệu}"
                    AllowPaging="false" Width='100%' BorderWidth="1px" CssClass="GridBorder">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckAll" runat="server" onclick="CheckAll();" Width="30px" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItem" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RowNum" HeaderText="Stt">
                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="KienNghiID" HeaderStyle-CssClass="NonDisplay">
                            <ItemStyle Width='1px' CssClass="NonDisplay clKienNghiID" />
                        </asp:BoundField>
                        <asp:BoundField DataField='NoiDungKN' HeaderText='Nội dung kiến nghị'>
                            <ItemStyle Width='90%' />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle Height="26px" HorizontalAlign="Left" CssClass="GridHeader" />
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
                        <asp:ListItem Text="100" Value="100" />
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
                    <div>
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
<div style='display: none'>
    <%--<asp:HiddenField ID="hidID" Value="0" runat="server" />--%>
    <asp:TextBox ID="txtListKienNghiID" runat="server" Text="" />
    <asp:TextBox ID="txtListOld" runat="server" Text="" />
</div>
