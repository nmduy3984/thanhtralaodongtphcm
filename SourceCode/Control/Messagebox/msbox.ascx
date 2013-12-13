<%@ Control Language="VB" AutoEventWireup="false" CodeFile="msbox.ascx.vb" Inherits="UI_Customize_msbox" %>
<style type="text/css">
    /* class popup */
    
    .box_popup_login
    {
        float: left;
        width: 300px;
    }
    .title_popup
    {
        float: left;
        width: 305px;
        height: 20px;
        vertical-align: top;
        margin: 2px 2px 2px 2px;
        padding: 2px 2px 2px 2px; /*background:url(../images/title_popup.png) no-repeat top left;*/
    }
    .title_popup_login
    {
        float: left;
        width: 305px;
        height: 20px;
        margin: 2px 2px 2px 2px;
        padding: 2px 2px 2px 2px;
        vertical-align: top; /*background:url(../images/title_popup.png) no-repeat top left;*/
    }
    .text_title_popup
    {
        float: left;
        font: bold 11px Arial, Tahoma, Verdana, Helvetica, sans-serif;
        color: #066bb9;
        padding: 2px 2px 2px 2px;
        text-transform: uppercase;
        text-align: left;
        height: 10px;
        vertical-align: text-top;
    }
    
    .icon_title_popup
    {
        float: right;
        font: bold 11px Arial, Tahoma, Verdana, Helvetica, sans-serif;
        color: #ea3214;
        padding: 2px 2px 2px 2px;
        text-transform: uppercase;
        text-align: left;
        vertical-align: top;
        height: 10px;
        width: 17px;
    }
    
    .mid_popup_login
    {
        /*background:url(../images/mid_popup.png) top left repeat-y;		*/
        text-align: center;
        width: 320px;
        height: 50px;
    }
    .mid_popup
    {
        /*background:url(../images/mid_popup.png) top left repeat-y;		*/
        height: 120px;
    }
    .bottom_popup
    {
        /*background:url(../images/bottom_popup.png);		*/
        height: 20px;
        text-align: center;
    }
    .icon_mid_popup
    {
        font: normal 12px Arial, Tahoma, Verdana, Helvetica, sans-serif;
        color: #3f3f3f;
        padding: 10px 5px 5px 0px;
        width: 30%;
        text-align: right;
        float: left;
    }
    .text_mid_popup
    {
        font: normal 12px Arial, Tahoma, Verdana, Helvetica, sans-serif;
        text-transform: none;
        color: #3f3f3f;
        padding: 10px 5px 5px 0px;
        width: 65%;
        float: left;
        text-align: center;
    }
    
    .text_label_popup
    {
        font: normal 12px Arial, Tahoma, Verdana, Helvetica, sans-serif;
        color: #3f3f3f;
        padding: 5px 5px 5px 0px;
        width: 20%;
        text-align: center;
        float: left;
    }
    .text_input_popup
    {
        font: normal 12px Arial, Tahoma, Verdana, Helvetica, sans-serif;
        color: #3f3f3f;
        padding: 5px 5px 5px 0px;
        width: 75%;
        text-align: left;
        vertical-align: middle;
        float: left;
    }
    .clearboth
    {
        clear: both;
        height: 5px;
    }
</style>
<script type="text/javascript">
    //show alert dang mac dinh
    function Alertbox(strTex) {
        $(function () {
            $('div#alerttitile').text("");
            $('<div>' + strTex + '</div>').appendTo("div#alerttitile");
            $('#btnShowAlert').click();
            $('[name=btnAcept]').focus();
            $('[name=btnAcept]').click(function () {
                self.parent.tb_remove();
            });
        });
    }
    //show alert dang mac dinh
    function AlertboxThenFocus(strTex,txtId) {
        $(function () {
            $('div#alerttitilethenfocus').text("");
            $('<div>' + strTex + '</div>').appendTo("div#alerttitilethenfocus");
            $('#btnShowAlertThenFocus').click();
            $('[name=AcceptThenFocus]').focus();
            $('[name=AcceptThenFocus]').click(function () {
                self.parent.tb_remove();
                $(txtId).focus();
            });
        });
    }
    //show alert dang nhay trang 
    function AlertboxRedirect(strTex, strUrl) {
        $(function () {
            $('div#alerttitile').text("");
            $('<div>' + strTex + '</div>').appendTo("div#alerttitile");
            $('#btnShowAlert').click();
            $('[name=btnAcept]').focus();
            $('[name=btnAcept]').click(function () {
                if (strUrl != '') {
                    self.parent.location = strUrl;
                } else {
                    self.parent.tb_remove();
                }
            });
        });
    }
    //show alert dang mo rong kich thuoc
    function AlertboxResize(strTex, strUrl, intHeight) {
        $(function () {
            $('div#alerttitile').text("");
            $('<div>' + strTex + '</div>').appendTo("div#alerttitile");
            var strAlt = $('#btnAlerResize').attr("alt");
            strAlt = strAlt.replace("height=120", "height=" + parseInt(intHeight));
            $('#btnAlerResize').attr("alt", strAlt);
            $('#btnAlerResize').click();
            $('[name=btnAcept]').focus();
            $('[name=btnAcept]').click(function () {
                if (strUrl != '') {
                    self.parent.location = strUrl;
                } else {
                    self.parent.tb_remove();
                }
            });
        });
    }
    //show alert dang mo rong kich thuoc va redirect
    function AlertboxResize(strTex, intHeight) {
        $(function () {
            $('div#alerttitile').text("");
            $('<div>' + strTex + '</div>').appendTo("div#alerttitile");
            var strAlt = $('#btnAlerResize').attr("alt");
            strAlt = strAlt.replace("height=120", "height=" + parseInt(intHeight));
            $('#btnAlerResize').attr("alt", strAlt);
            $('#btnAlerResize').click();
            $('[name=btnAcept]').focus();
            $('[name=btnAcept]').click(function () {

                self.parent.tb_remove();

            });
        });
    }
    //show confirm dang mac dinh
    var _btnClickRef;
    var intID;
    var intType;
    function Confirmbox(strTex, value, btnClick, id, type) {
        $(document).ready(function () {
            _btnClickRef = btnClick;
            intID = id;
            intType = type;
            $('div#comfirmtitile').text("");
            $('<div>' + strTex + '</div>').appendTo("div#comfirmtitile");
            $('#btnShow').click();
            $('[name=btnCancel]').focus();
        });
    }
    function Accept() {
        window.parent.ComfirmDialog("", 1, _btnClickRef, intID, intType);
        self.parent.tb_remove();
    }
    function CloseComfirm() {
        self.parent.tb_remove();
    }
</script>
<%--Phan danh cho Comfirmbox--%>
<div id="msbox" style="display: none" class="box_popup_login">
    <div class="title_popup_login">
        <div class="text_title_popup">
            <img src='<%=ResolveUrl("msbox_image/arrow_popup.gif") %>' width="10" height="5" />Thông
            báo</div>
        <div class="icon_title_popup">
            <asp:ImageButton ID="imgClose" CausesValidation="false" OnClientClick="CloseComfirm();"
                ToolTip="Đóng cửa sổ" ImageUrl="msbox_image/close_popup.png" align="absmiddle"
                border="0" Width="19" Height="19" runat="server" />
        </div>
    </div>
    <div class="clearboth">
    </div>
    <div class="mid_popup_login">
        <div style="float: left" class="text_label_popup">
            <img src='<%=ResolveUrl("msbox_image/icon_error_2.jpg")%>' align="absmiddle" alt=""
                height="30" width="40" />
        </div>
        <div id="comfirmtitile" class="text_input_popup">
        </div>
    </div>
    <div class="clearboth">
    </div>
    <div class="bottom_popup">
        <asp:Button ID="btnOK" OnClientClick="Accept();" class="btn" runat="server" Text="Chấp nhận" />
        &nbsp;
        <input name="btnCancel" type="submit" onclick="CloseComfirm();" value="Hủy bỏ" class="btn">
    </div>
</div>
<%-- Phan danh cho Alertbox--%>
<div id="alrtmsbox" style="display: none" class="box_popup_login">
    <div class="title_popup_login">
        <div class="text_title_popup">
            <img src='<%=ResolveUrl("msbox_image/arrow_popup.gif")%>' width="10" height="5" />Thông
            báo</div>
        <div class="icon_title_popup">
            <asp:ImageButton ID="image" CausesValidation="false" OnClientClick="self.parent.tb_remove();"
                ToolTip="Đóng cửa sổ" ImageUrl="msbox_image/close_popup.png" align="absmiddle"
                border="0" Width="19" Height="19" runat="server" />
        </div>
    </div>
    <div class="clearboth">
    </div>
    <div class="mid_popup_login">
        <div class="text_label_popup">
            <img src='<%=ResolveUrl("msbox_image/icon_error_2.jpg")%>' alt="" align="absmiddle"
                height="30" width="40" /></div>
        <div id="alerttitile" class="text_input_popup" style="min-height: 60px;">
        </div>
    </div>
    <div class="clearboth">
    </div>
    <div class="bottom_popup">
        <input name="btnAcept" id="btnAcept" type="submit" value="Xong" class="btn" />
    </div>
</div>
<%-- Phan danh cho AlertboxThenFocus--%>
<div id="alrtmsboxfocus" style="display: none" class="box_popup_login">
    <div class="title_popup_login">
        <div class="text_title_popup">
            <img src='<%=ResolveUrl("msbox_image/arrow_popup.gif")%>' width="10" height="5" />Thông
            báo</div>
        <div class="icon_title_popup">
            <asp:ImageButton ID="ImageButton1" CausesValidation="false" OnClientClick="self.parent.tb_remove();"
                ToolTip="Đóng cửa sổ" ImageUrl="msbox_image/close_popup.png" align="absmiddle"
                border="0" Width="19" Height="19" runat="server" />
        </div>
    </div>
    <div class="clearboth">
    </div>
    <div class="mid_popup_login">
        <div class="text_label_popup">
            <img src='<%=ResolveUrl("msbox_image/icon_error_2.jpg")%>' alt="" align="absmiddle"
                height="30" width="40" /></div>
        <div id="alerttitilethenfocus" class="text_input_popup" style="min-height: 60px;">
        </div>
    </div>
    <div class="clearboth">
    </div>
    <div class="bottom_popup">
        <input name="AcceptThenFocus" id="Submit1" type="submit" value="Xong" class="btn" />
    </div>
</div>
<div style="display: none">
    <input id="btnShow" alt="#TB_inline?height=120&width=305&modal=true&inlineId=msbox"
        title="add a caption to title attribute / or leave blank" class="thickbox" type="button"
        value="Show" />
    <input id="btnShowAlert" alt="#TB_inline?height=130&width=315&modal=true&inlineId=alrtmsbox"
        title="add a caption to title attribute / or leave blank" class="thickbox" type="button"
        value="Show" />
    <input id="btnShowAlertThenFocus" alt="#TB_inline?height=130&width=315&modal=true&inlineId=alrtmsboxfocus"
        title="add a caption to title attribute / or leave blank" class="thickbox" type="button"
        value="Show" />
    <input id="btnAlerResize" alt="#TB_inline?height=120&width=305&modal=true&inlineId=alrtmsbox"
        title="add a caption to title attribute / or leave blank" class="thickbox" type="button"
        value="Show" />
</div>
