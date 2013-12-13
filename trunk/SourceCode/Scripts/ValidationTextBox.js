// Check IntegerAndDecimal
function IntegerAndDecimal(e, obj, isDecimal) {
    $(obj).keyup(function () {
        var str = obj.value;
        if (str.indexOf(".") > -1) {
            if (isDecimal == 'false') {
                obj.value = str.substring(0, str.length - 1);
                e.returnValue = false;
                return false;
            }
        }
        if ($(obj).attr("Id").indexOf("txtCode") == -1) { 
            obj.value = FormatsNumber(str);
        }
    });

    if ([e.keyCode || e.which] == 8) //neu nhap co backspace.
        return true;
    if ([e.keyCode || e.which] == 46) //neu co dau cham dong.
    {
        if (isDecimal == 'true') {
            var val = obj.value;
            if (val.indexOf(".") > -1) {
                e.returnValue = false;
                return false;
            }
            return true;
        } else {
            e.returnValue = false;
            return false;
        }
    }
    if ([e.keyCode || e.which] < 48 || [e.keyCode || e.which] > 57) {
        if ([e.keyCode || e.which] == 46 || [e.keyCode || e.which] == 9) {
            return true;
        } else { 
         e.preventDefault ? e.preventDefault() : e.returnValue = false;
        }
}
}
//Check nhap chu cai va so
function AlphaNumericOnly(e, isAlphaonly) {

    // Chi cho phep nhap ky tu chu cai va so
    var key = [e.keyCode || e.which];

    var keychar = String.fromCharCode([e.keyCode || e.which]);
    keychar = keychar.toLowerCase();

    if (isAlphaonly == 'true')
        checkString = "abcdefghijklmnopqrstuvwxyz";
    else
        checkString = "abcdefghijklmnopqrstuvwxyz0123456789";

    if ((key == null) || (key == 0) || (key == 8) ||
                        (key == 9) || (key == 13) || (key == 27))
        return true;
    else if (((checkString).indexOf(keychar) > -1)) {
        return true;
    } else if (key == 46) {
        return true;
    } else { 
      return false;
    }
}

function callOnload() {

    $("input[isRequired=true]").each(
                        function (n) {
                            $('#' + this.id).addClass('mandatory');

                            if (this.value === '') {
                                $('#' + this.id).removeClass('normal');
                                $('#' + this.id).addClass('mandatory');
                            }
                            else {
                                $('#' + this.id).removeClass('mandatory');
                                $('#' + this.id).addClass('normal');
                            }
                        }
             );

    $("select[isRequired=true]").each(
                    function (n) {
                        $('#' + this.id).addClass('mandatory');
                        if ($("#" + this.id + "").val() === '0' || $("#" + this.id + "").val() === '') {
                            $('#' + this.id).removeClass('normal');
                            $('#' + this.id).addClass('mandatory');
                        }
                        else {
                            $('#' + this.id).removeClass('mandatory');
                            $('#' + this.id).addClass('normal');
                        }
                    }
    );

    $("textarea[isRequired=true]").each(
                        function (n) {
                            $('#' + this.id).addClass('mandatory');
                            if (this.value === '') {
                                $('#' + this.id).removeClass('normal');
                                $('#' + this.id).addClass('mandatory');
                            }
                            else {
                                $('#' + this.id).removeClass('mandatory');
                                $('#' + this.id).addClass('normal');
                            }
                        }
             );
}
function changeSelected(cid) {

    if ($("#" + cid + " option:selected").val() === '0' || $("#" + cid + " option:selected").val() === '') {
        //  document.getElementById('spError' + cid).style.display = 'none';
        $('#' + cid).removeClass('normal');
        $('#' + cid).addClass('mandatory');
    }
    else {
        //  document.getElementById('spError' + cid).style.display = 'none';
        $('#' + cid).removeClass('mandatory');
        $('#' + cid).addClass('normal');
    }
}
function keyup(cid) {
   
    test = '';
    if ($('#' + cid).val() === ''  || $('#' + cid).val() === test) {
        //   document.getElementById('spError' + cid).style.display = 'none';
        $('#' + cid).removeClass('normal');
        $('#' + cid).addClass('mandatory');
    }
    else {
        // document.getElementById('spError' + cid).style.display = 'none';
        $('#' + cid).removeClass('mandatory');
        $('#' + cid).addClass('normal');
    }

    //RangeError
    var isrange = $('#' + cid).attr("IsRange");
    if (isrange == "true") {
        if ($('#' + cid).val() != '') {
            if (Number($('#' + cid).val()) != NaN) {
                if (Number($('#' + cid).attr("MaximumValue")) != NaN && Number($('#' + cid).attr("MinimumValue")) != NaN) {
                    if (validateTextNumericInRange($('#' + cid)) == false) {
                        //     document.getElementById('spRangeError' + cid).style.display = 'none';
                        $('#' + cid).removeClass('normal');
                        $('#' + cid).addClass('mandatory');
                    }
                    else {
                        //      document.getElementById('spRangeError' + cid).style.display = 'none';
                        $('#' + cid).removeClass('mandatory');
                        $('#' + cid).addClass('normal');
                    }
                }
            }
        } else {
            // document.getElementById('spRangeError' + cid).style.display = 'none';
            $('#' + cid).removeClass('mandatory');
            $('#' + cid).addClass('normal');
        }
    }

    //CheckReg
    var pattern = $('#' + cid).attr("pattern");
    if (pattern != '' && pattern != undefined) {
       
        var arrReg = new Array();
        arrReg[0] = "";
        arrReg[1] = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/; //Email
        arrReg[2] = /^(((http|ftp|https|ftps):\/\/)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))$/; //Http://
        arrReg[3] = /^([0-9]{1,9}[ ]{0,1})+$/; //Mobile
        arrReg[4] = /^0\d{1,5}\.\d{6,10}$/; //CellPhone
        arrReg[5] = /^\d{9}$/; //CMND
        arrReg[6] = /([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]/; //hhmmss
        arrReg[7] = /(?:0[1-9]|[12][0-9]|3[01])\/(?:0[1-9]|1[0-2])\/(?:19|20\d{2})/; 

        if ($('#' + cid).value !== '' && pattern !== '') {
            //  var filter = new RegExp(arrReg[pattern]);

            if (!arrReg[pattern].test($('#' + cid).val())) {
                //    document.getElementById('spRegError' + cid).style.display = 'none';
                $('#' + cid).removeClass('normal');
                $('#' + cid).addClass('mandatory');
            } else {

                //   document.getElementById('spRegError' + cid).style.display = 'none';
                $('#' + cid).removeClass('mandatory');
                $('#' + cid).addClass('normal');

            }
        } else {
            // document.getElementById('spRegError' + cid).style.display = 'none';
            $('#' + cid).removeClass('mandatory');
            $('#' + cid).addClass('normal');
        }
    }
   
}

function CountText(field, maxlimit) {
    if (field.value.length < maxlimit) // if too long...trim it!
    {
        return true;
    }
    else
        return false;
}

function checkReg(obj, pattern) {
    var arrReg = new Array();
    arrReg[0] = "";
    arrReg[1] = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/; //Email
    arrReg[2] = /^(((http|ftp|https|ftps):\/\/)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))$/; //Http://
    arrReg[3] = /^([0-9]{1,9}[ ]{0,1})+$/; //Mobile
    arrReg[4] = /^0\d{1,5}\.\d{6,10}$/; //CellPhone
    arrReg[5] = /^\d{9}$/; //CMND
    arrReg[6] = /([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]/; //hhmmss
    arrReg[7] = /(?:0[1-9]|[12][0-9]|3[01])\/(?:0[1-9]|1[0-2])\/(?:19|20\d{2})/; //dd/MM/yyyy+
    if (obj.value !== '' && pattern !== '') {
        //var filter = new RegExp(arrReg[pattern]);
        if (!arrReg[pattern].test(obj.value)) {
            //   document.getElementById('spRegError' + obj.id).style.display = 'none';
            $(obj).removeClass('normal');
            $(obj).addClass('mandatory');
        } else {
            //  document.getElementById('spRegError' + obj.id).style.display = 'none';
            $(obj).removeClass('mandatory');
            $(obj).addClass('normal');
        }
    } else {
        //   document.getElementById('spRegError' + obj.id).style.display = 'none';
        $('#' + obj.id).removeClass('mandatory');
        $('#' + obj.id).addClass('normal');
    }
    return true;
}

function validateFormInputs(gropId) {
   
    var isAllValid = true;
    var searchConditon = "";
    //Textbox
    if (gropId !== "" && gropId !== undefined) { 
        searchConditon = searchConditon + "input[isRequired=true][GroupId=" + gropId + "]";
    }
    else {
        searchConditon = searchConditon + "input[isRequired=true]";
    }

    $(searchConditon).each(
                function (n) {
                   
                    test = $('#' + this.id).attr("LabelValue");
              
                    if (this.value === '' || this.value == test) {
                        isAllValid = false;
                        //      document.getElementById('spError' + this.id).style.display = 'none';
                        $('#' + this.id).removeClass('normal');
                        $('#' + this.id).addClass('mandatory');


                    }
                    else {

                    //     if (document.getElementById('spError' + this.id) != null) {
                            document.getElementById('spError' + this.id).style.display = 'none';
                            $('#' + this.id).removeClass('mandatory');
                            $('#' + this.id).addClass('normal');
                        }

      });


    //TextArea
    searchConditon = "";
    if (gropId !== "" && gropId !== undefined) {
        searchConditon = searchConditon + "textarea[isRequired=true][GroupId=" + gropId + "]";
    }
    else {

        searchConditon = searchConditon + "textarea[isRequired=true]";
    }
    $(searchConditon).each(
                                  function (n) {
                                      test = $('#' + this.id).attr("LabelValue");
                                      if (this.value === ''
                  || this.value == test) {
                                          isAllValid = false;
                                       //    document.getElementById('spError' + this.id).style.display = 'none';
                                          $('#' + this.id).removeClass('normal');
                                          $('#' + this.id).addClass('mandatory');
                                      }
                                      else {
                                          if (document.getElementById('spError' + this.id) != null)
                                       //        document.getElementById('spError' + this.id).style.display = 'none';
                                          $('#' + this.id).removeClass('mandatory');
                                          $('#' + this.id).addClass('normal');
                                       
                                      }
                                  }
                                 );


    //Dropdownlist
    searchConditon = "";
    if (gropId !== "" && gropId !== undefined) {

        searchConditon = searchConditon + "select[isRequired=true][GroupId=" + gropId + "]";
    }
    else {
        searchConditon = searchConditon + "select[isRequired=true]";
    }

    $(searchConditon).each(
    function (n) {
        test = $("#" + this.id + "").val();
        if (this.value === "0" || this.value === "") {
            isAllValid = false;
         //   document.getElementById('spError' + this.id).style.display = 'none';
            $('#' + this.id).removeClass('normal');
            $('#' + this.id).addClass('mandatory');
        }
        else {
            if (document.getElementById('spError' + this.id) != null)
            //        document.getElementById('spError' + this.id).style.display = 'none';
            $('#' + this.id).removeClass('mandatory');
            $('#' + this.id).addClass('normal');
        }
    }
                                 );

    //Check compage
    searchConditon = "";
    if (gropId !== "" && gropId !== undefined) {
        searchConditon = searchConditon + "input[IsCompage='true'][GroupId=" + gropId + "]";
    }
    else {
        searchConditon = searchConditon + "input[IsCompage='true']";
    }

    $(searchConditon).each(
                function (n) {
                    if (this.value != '' && this.value != undefined) {
                        //Compage
                        if ($(this).attr("Operator") != undefined && $(this).attr("Operator") != null) {
                            if (CompareText($(this), $(this).attr("compareto")) == true) {
                                isAllValid = false;
                            }
                        }
                    }

                }
     );

                //Check range
                searchConditon = "";
                if (gropId !== "" && gropId !== undefined) {
                    searchConditon = searchConditon + "input[IsRange='true'][GroupId=" + gropId + "]";
                }
                else {
                    searchConditon = searchConditon + "input[IsRange='true']";
                }

                $(searchConditon).each(
                function (n) {
                    if (this.value != '' && this.value != undefined) {
                        //range
                        if ($('#' + this.id).val() != '') {
                            if (Number($('#' + this.id).val()) != NaN) {
                                if (Number($('#' + this.id).attr("MaximumValue")) != NaN && Number($('#' + this.id).attr("MinimumValue")) != NaN) {
                                    if (validateTextNumericInRange($('#' + this.id)) == false) {
                                        //        document.getElementById('spRangeError' + this.id).style.display = 'none';
                                        $('#' + this.id).removeClass('normal');
                                        $('#' + this.id).addClass('mandatory');
                                        isAllValid = false;
                                    }
                                    else {
                                        //   document.getElementById('spRangeError' + this.id).style.display = 'none';
                                        $('#' + this.id).removeClass('mandatory');
                                        $('#' + this.id).addClass('normal');
                                    }
                                }
                            }
                        } else {
                            //  document.getElementById('spRangeError' + this.id).style.display = 'none';
                            $('#' + this.id).removeClass('mandatory');
                            $('#' + this.id).addClass('normal');
                        }
                    }

                }
     );


  //  $('.sp_error').each(function () {

//        if (this.style.display == 'none') {

//            isAllValid = false;
//        }
  //  });

    return isAllValid;
}
// Formats number
function FormatsNumber(nStr) {
    nStr = nStr.replace(/,/g, '');
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return (x1 + x2);
}
//Check range validation
function validateTextNumericInRange(obj) {
    var max = parseFloat($(obj).attr("MaximumValue").replace(/,/g, ''));
    var min = parseFloat($(obj).attr("MinimumValue").replace(/,/g, ''));
    var value = parseFloat($(obj).val().replace(/,/g, ''));
    if (!isNaN(value) && value >= min && value <= max) {
        return true;
    } else {
        return false;
    }
}
//Compare validation
function CompareText(obj, ControlToId) {
    var Id = $(obj).attr("Id");
    var flg = false;
   
   // if ($("#spRegError" + Id).css("display") != 'none' && $("#spRegError" + Id).css("display") != null) {
        var meValue = $(obj).val();
        var toValue = $("#" + ControlToId).val();
      
        var DataCompare = $(obj).attr("DataCompare");
        var OperatorCompare = $(obj).attr("Operator");

        if (meValue != "") {
           
            if (toValue != "") {
              

                switch (DataCompare) {
                    case "0": //String
                        switch (OperatorCompare) {
                            case "Equal":
                                if (meValue.toLowerCase() == toValue.toLowerCase()) {
                                    //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                } else {
                                    //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                    flg = true;
                                }
                                break;
                            case "NotEqual":
                                if (meValue.toLowerCase() != toValue.toLowerCase()) {
                                    $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                    //     $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                } else {
                                 $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                    //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    flg = true;
                                }
                                break;
                            case "GreaterThan":
                                if (meValue > toValue) {
                                    $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                    // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                } else {
                                 $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                    //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    flg = true;
                                }
                                break;
                            case "GreaterThanEqual":
                                if (meValue >= toValue) {
                                    $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                    //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                } else {
                                 $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                    //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    flg = true;
                                }
                                break;
                            case "LessThan":
                                if (meValue < toValue) {
                                    $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                    // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                } else {
                                 $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                    //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    flg = true;
                                }
                                break;
                            case "LessThanEqual":
                                if (meValue <= toValue) {
                                    $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                    //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                } else {
                                 $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                    //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    flg = true;
                                }
                                break;
                            default:
                                x = "Vui long xem lai";
                        }
                        break;
                    case "1": //Number
                        if (!isNaN(meValue) && !isNaN(toValue)) {

                            switch (OperatorCompare) {
                                case "Equal":
                                    if (parseFloat(meValue) == parseFloat(toValue)) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "NotEqual":
                                    if (parseFloat(meValue) != parseFloat(toValue)) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "GreaterThan":
                                    if (parseFloat(meValue) > parseFloat(toValue)) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //   $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "GreaterThanEqual":
                                    if (parseFloat(meValue) >= parseFloat(toValue)) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "LessThan":
                                    if (parseFloat(meValue) < parseFloat(toValue)) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "LessThanEqual":
                                    if (parseFloat(meValue) <= parseFloat(toValue)) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                default:
                                    x = "Vui long xem lai";
                            }
                        }
                        break;
                    case "2": //Date

                        if (meValue != "" && toValue != "") {
                            alert('fff');
                            var objFrom = meValue.split('/');
                            var objEnd = toValue.split('/');
                            var FromDate = new Date(objFrom[2], objFrom[1], objFrom[0], 0, 0, 0)
                            var EndDate = new Date(objEnd[2], objEnd[1], objEnd[0], 0, 0, 0)

                            switch (OperatorCompare) {

                                case "Equal":
                                    if (EndDate == FromDate) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');

                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "NotEqual":
                                    if (EndDate != FromDate) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "GreaterThan":
                                    if (FromDate > EndDate) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "GreaterThanEqual":
                                    if (FromDate >= EndDate) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "LessThan":
                                    if (FromDate < EndDate) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                case "LessThanEqual":
                                    if (FromDate <= EndDate) {
                                        $(obj).removeClass('mandatory');
                                    $(obj).addClass('normal');
                                        // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                    } else {
                                     $(obj).removeClass('normal');
                                    $(obj).addClass('mandatory');
                                        //  $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
                                        flg = true;
                                    }
                                    break;
                                default:
                                    x = "Vui long xem lai";
                            }


                        }
                        else {
                            flg = true;
                        }

                        break;
                    default:
                        x = "Vui long xem lai";
                }

            } else {
                // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
            }
        } else {
            // $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
        }
        return flg;
//    } else {
//        $('#spCompareError' + $(obj).attr("Id")).css('display', 'none');
//        return flg;
//    }
}


