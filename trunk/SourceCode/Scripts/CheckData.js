/// <reference path="jquery-1.3.2-vsdoc.js" />

// This is the initial jQuery starting point.  The 'main' function is run once the DOM of every page is established.
//20/09/2011 duynp trim()
// Add trim vao kieu string de xoa khoang trang
String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); }
//---------------------------
var arrReg = new Array();
//20/09/2011 duynp checkvalid()
//check Email valid
arrReg["Email"] = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
//check Date for dd/mm/yy or dd/mm/yyyy
arrReg["Date"] = /^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/;
arrReg["Integer"] = /^\d+$/;
//Example: CheckRegular('Date','<%=txtDate.ClientID %>')
//*** NOTE: //strCheck: Email or Date...; IDClient: Id Client cua control TextBox
function CheckRegular(strCheck, IDClient) { 
    if (IDClient != "") {
        var obj = document.getElementById(IDClient).value.trim();
        if (obj != "" && strCheck != "") {
            return arrReg[strCheck].test(obj);
        }
        return true;
    }
}

//Check maxlenth of Description(Thaivv)
function CheckMaxLengthDescriptionKeyPress(Des) {
    var vDes = document.getElementById(Des);
    if (parseFloat(vDes.value.length) > 498) {
        vDes.focus();
        vDes.value = vDes.value.substring(0, 498);
    }
}

//Kiem tra va convert theo kieu Money
function CheckMoneyFormat(ctrl) {
    var vobj = document.getElementById(ctrl);
    var objtb = $("#" + ctrl);
    var Amount = vobj.value.replace(/,/g, '');
    if (isNaN(Amount) == 1) {
        alert('Vui lòng nhập đúng kiểu số');
        objtb.val(objtb.attr('title'));
        vobj.focus();
    }
    else {
        vobj.value = Formatcurrency(Amount);

    }

}

function Formatcurrency(amount) {
    var delimiter = ","; // replace comma if desired
    amount = parseFloat(amount);
    var a = amount.toString().split('.', 2)
    var d = a[1];
    var i = parseInt(a[0]);
    if (isNaN(i)) { return ''; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    var n = new String(i);
    var a = [];
    while (n.length > 3) {
        var nn = n.substr(n.length - 3);
        a.unshift(nn);
        n = n.substr(0, n.length - 3);
    }
    if (n.length > 0) { a.unshift(n); }
    n = a.join(delimiter);
    if (d != null) { amount = n + '.' + d; }
    else { amount = n; }
    amount = minus + amount;
    return amount;
}


//Remove add item of select
var count1 = 0;
var count2 = 0;

function insertOptionBefore(num, selectX)
{
  var elSel = document.getElementById(selectX);
  if (elSel.selectedIndex >= 0) {
    var elOptNew = document.createElement('option');
    elOptNew.text = 'Insert' + num;
    elOptNew.value = 'insert' + num;
    var elOptOld = elSel.options[elSel.selectedIndex];  
    try {
      elSel.add(elOptNew, elOptOld); // standards compliant; doesn't work in IE
    }
    catch(ex) {
      elSel.add(elOptNew, elSel.selectedIndex); // IE only
    }
  }
}

function removeOptionSelected(selectX)
{
  var elSel = document.getElementById(selectX);
  var i;
  for (i = elSel.length - 1; i>=0; i--) {
    if (elSel.options[i].selected) {
      elSel.remove(i);
    }
  }
}

function appendOptionLast(sID, sText, selectX) {
    var arrID = sID.split(';');
    var arrText = sText.split(';');
    var i = 0;
    for (i = 0; i < arrID.length - 1; i++) {
        var elOptNew = document.createElement('option');
        elOptNew.text = arrText[i];
        elOptNew.value = arrID[i];
        var elSel = document.getElementById(selectX);

        try {
            elSel.add(elOptNew, null); // standards compliant; doesn't work in IE
        }
        catch (ex) {
            elSel.add(elOptNew); // IE only
        }
    }
}

function removeOptionLast(selectX)
{
  var elSel = document.getElementById(selectX);
  if (elSel.length > 0)
  {
    elSel.remove(elSel.length - 1);
  }
}

function removeAllOption(selectX) {
    var elSel = document.getElementById(selectX);
    if (elSel.length > 0) {
        var i = 0;
        for (i = elSel.length - 1; i >= 0; i--)
            elSel.remove(i);
    }
}
