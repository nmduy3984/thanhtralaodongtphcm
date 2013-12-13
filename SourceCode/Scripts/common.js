/// <reference path="jquery-1.3.2-vsdoc.js" />

// This is the initial jQuery starting point.  The 'main' function is run once the DOM of every page is established.



  // This function must be called with all modal dialog boxes which have fields that
  // require validation.  It performs a call to the ASP.Net function, "Page_ClientValidate()",
  // and then continues on calling "CloseModalDialog()" if the validation is okay.
  // Note: Surprisingly, companion functions like "ValidatorValidate()" do not work, 
  // perhaps due to the way ThickBox rearranges DOM elements.
  function CheckValidationBeforeClose(element) {
    if (Page_ClientValidate())
      CloseModalDialog(element);
  }


  // Used to close a ThickBox modal dialog and then force a postback,
  // which a server-side control is seemingly unable to do on its own
  // when fired from within the confines of a ThickBox object.
  function CloseModalDialog(element) {
    tb_remove();
    setTimeout('__doPostBack(\'' + element.name + '\',\'\')', 500);  // 500ms seems to give ThickBox enough time to remove itself
  }


  // Used to close a ThickBox modal dialog without causing a postback.
  function CancelModalDialog() {
    tb_remove();
  }



  // Prepares all textboxes such that the background changes to a distinct
  // color upon focus and then returns to white after focus is lost.
  function PrepareDefaultEventHandlers() {
    $(":text").focus(textboxHighlight).blur(textboxRemoveHighlight);
    $(":password").focus(textboxHighlight).blur(textboxRemoveHighlight);
    $("textarea").focus(textboxHighlight).blur(textboxRemoveHighlight);
  }

  function textboxHighlight() {
    $("#" + this.id).css({ 'background-color': '#ffff40' });
  }

  function textboxRemoveHighlight() {
    $("#" + this.id).css({ 'background-color': 'white' });
}
// Format Copy content from Word
function removeTextFormat(ControlID) 
{   
         
        var  CuteEditor =  document.getElementById(ControlID);
        var mystring = CuteEditor.getHTML();
        
        var reRemoveHTMLComment = /<(?:!(?:--[\s\S]*?--\s*)?(>)\s*|(?:script|style)[\s\S]*?<\/(?:script|style)>)/gi;
        var reRemoveTag = /<[/\?]?(font|a|xml|h1|h2|h3|h4|h5|h6|h7|h8|b|strong|em|del|ins|st\d+\:[\w\-]+|[ovwxp]:\w+)[^>]*?>/gi;
        var reGetAnyTag = /(<[^>]+>)/gi;
        var clearStyle = /(style=".*?")/gi;
        var clearClass = /(class=".*?")/gi;
        var reRemoveAttribute = /(?:class|style|lang|name|alt|id|href|border|target|size|face|on\w+|[ovwxp]:\w+)(?:\=(?:'[^']*'|"[^"]*"|[^\s>]+|)|[\s])/gi;
        mystring = mystring.replace(clearClass,'');
        mystring = mystring.replace(clearStyle,'');
        mystring = mystring.replace(reRemoveHTMLComment, function(m,$1){return $1?'':m;});
        mystring = mystring.replace(reRemoveTag, '');
        mystring = mystring.replace(reGetAnyTag, function($1){isTable=$1.match(/^<table(?=\s|>)/gi); return $1.replace(reRemoveAttribute, function($2){
                                var arr = $2.split('=');
                                var attr = arr[0];
                                var value = arr[1]?arr[1]:'';

                                if (attr.toLowerCase() == 'style' && value != '' && value != '""' && value != "''")
                                {
                                            var reChangeStyleAttr = /(\s*(?:mso[^:]*|-moz-[^:]*|color|background[^:]*|font[^:]*|border[^:]*)[\s]*:[^;]*(;|(?='|"))|(?=;|'|"|)\s*[^:]+;;)/gi;

                                            onlyOne=true;
                                            value = value.replace(reChangeStyleAttr, function($3){
                                                                    if ($3.match(/^\s*border[^:]*:/gi) && isTable && onlyOne)
                                                                    {
                                                                                onlyOne=false; 
                                                                                return 'border-collapse: collapse;';
                                                                    }
                                                                    else return '';
                                                                                                                                                                                                        });

                                            if (value != '""' && value != "''") return 'style=' + value;
                                            else return ''
                                }
                                else if (attr.toLowerCase() == 'border' && value != '0' && value != '"0"' && value != "'0'")
                                {
                                            return 'border=' + value + ' bordercolor="black" style="border-collapse: collapse;" ';
                                }
                                else return '';
        });});
        

    mystring = mystring.replace("<img ", "<img class='center' " );
        
   CuteEditor.setHTML(mystring) ;
}

// check all and uncheck all for GridView
// date add: 21/02/2011
function CheckAll(e) {
        var xState = e.checked;
        var theBox = e;
        var elm = theBox.form.elements;
        var i = 0;
        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" && elm[i].id != theBox.id)
                if (elm[i].checked != xState && !elm[i].disable)
                    elm[i].click();
    }

function getLog() {
    var CountClass = $(".DivLabel").length;
    var valText = "";
    var valValue = "";
    for (var i = 0; i < CountClass; i++) {
        valText = valText + ($(".DivLabel").eq(i).find("span").html() + ";");
        if ($(".DivTextBox").eq(i).find("input").length != 0) {
            valValue = valValue + ($(".DivTextBox").eq(i).find("input").val() + ";");
        }
        if ($(".DivTextBox").eq(i).find("select").length != 0) {
            valValue = valValue + ($(".DivTextBox").eq(i).find('option:selected').val() + ";");
        }
    }
getValueLog(valValue);
getTextLog(valText);
}
function getValueLog(Value) {
    return Value;
}
function getTextLog(Text) {
    return Text;
}

function CheckOnlyAndEffect(){
    // Dùng cho 2 checkbox có, không và enable các control kiểu input, textarea nằm trong class EffectGroup
    $(".Child input:checkbox").click(function () {
        // Lấy giá trị hiện tại của checkbox được click
        var oldVal = $(this).is(':checked');
        // Lấy ra những checkbox được mà checked
        var items = $(this).parents(".Child").find(" input:checked");

        if (items.size() > 1) {
            //uncheck cả 2 checkbox
            items.attr("checked", false);
            //gán lại giá trị ban đầu cho checkbox được check
            $(this).attr("checked", oldVal);
        }

        // Phần này để disable or enable những control EffectGroup theo Checkbox 
        if ($(this).parents(".Child").parent().find(".EffectGroup").size() > 0) {

            // Nếu có class 'case1' thì thực hiện kiểm tra như sau:
            //      + Nếu check Có              : --> Enable các element có class EffectGroup
            //      + Ngược lại nếu check Không : --> Disable các element có class EffectGroup
            // Ngược lại nếu không có class 'case1' thì thực hiện check như sau:
            //      + Nếu check Có              : --> Disable các element có class EffectGroup
            //      + Ngược lại nếu check Không : --> Enable các element có class EffectGroup
            if($(this).parents(".Child").find(".case1").size() > 0){ 
                if ($(this).is(':checked') && $(this).val()*1==1) {
                    $(this).parents(".Child").parent().find(".EffectGroup input,.EffectGroup textarea").removeAttr("disabled").removeClass("aspNetDisabled");
                } else {
                    $(this).parents(".Child").parent().find(".EffectGroup input,.EffectGroup textarea").attr("disabled", true).addClass("aspNetDisabled");
                }
            }else {
                if ($(this).is(':checked') && $(this).val()*1==0) {
                    $(this).parents(".Child").parent().find(".EffectGroup input,.EffectGroup textarea").removeAttr("disabled").removeClass("aspNetDisabled");
                } else {
                    $(this).parents(".Child").parent().find(".EffectGroup input,.EffectGroup textarea").attr("disabled", true).addClass("aspNetDisabled");
                }
            }
        }
    });
}

//******************************
// Tuancd : Dec 20 2012
// Dùng để check Regular theo một số kiểu dữ liện cơ bản 
//*******************************
var arrReg = new Array();
//check Email valid
arrReg["Email"] = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
//check Date for dd/mm/yy or dd/mm/yyyy
arrReg["Date"] = /^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/;
arrReg["Website"] = /^(((http|ftp|https|ftps):\/\/)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))$/;
arrReg["Decimal"] = /^[0-9]*(\.)?[0-9]+$/; ///^[+]?\d*\.?\d*$/;
arrReg["hhmm"] = /([0-1][0-9]|2[0-3]):[0-5][0-9]/;
arrReg["Integer"] = /^\d+$/;
arrReg["Username"] = /^[a-zA-Z0-9_]{3,}$/; //user name khong ton tai ký tu dac biet và phải lớn hơn 3 ký tự
arrReg["User_Password"] = /^.{6,20}$/; //user password phải lớn hơn 6 ký tự và bé hơn 20 ký tự
arrReg["Regular_Name"] = /^[a-zA-Z0-9_ ]+$/; //user name khong ton tai ký tu dac biet 
arrReg["Mobile"] = /^([0-9]{1,9}[ ]{0,1})+$/;
arrReg["hhmmss"] = /([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]/;
arrReg["CellPhone"] = /^0\d{1,5}\.\d{6,10}$/;
arrReg["Phone"] = /^0\d{9,10}$/;
arrReg["CMND"] = /^\d{9}$/;


//Example: CheckRegular('Date','<%=txtDate.ClientID %>')
function CheckRegular(strCheck, IDClient) { //strCheck: Email or Date...; IDClient: Id Client cua control TextBox
    if (IDClient != "") {    
        var obj = document.getElementById(IDClient).value.trim();
        if (obj != "" && strCheck != "") {
            return arrReg[strCheck].test(obj);
        }
        return true;
    }
}
function newDate(strDate) {//dd/mm/yyyy
    var arrDate = new Array();
    arrDate = strDate.split("/");
    var myDate = new Date();
    var month = parseFloat(arrDate[1]) - 1; //Month 0-11 javascript
    myDate.setFullYear(arrDate[2], month, arrDate[0]);
    return myDate;
}
// Tuancd: 09/01/2013
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        evt.cancel = true;
        evt.returnValue = false;
        return false;
    }
    return charCode !=13 ;
}