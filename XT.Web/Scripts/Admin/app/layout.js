
var ROOTPATH = "";
if ($("#rootPath").length > 0)
{
    ROOTPATH = $("#rootPath").val()
}
var file_images = [];
var file_images_id = 0;

//General
String.prototype.removeMVCPath = function (n) {
    var str = this;
    if (str.indexOf("~/") == 0) {
        str = str.substr(1);
    }

    return str;
};

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = c == undefined ? 0 : (isNaN(c = Math.abs(c)) ? 2 : c),
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}

Date.prototype.ddmmyyyy = function () {
    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = this.getDate().toString();
    return (dd[1] ? dd : "0" + dd[0]) + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + yyyy; // padding
};

Date.prototype.mmyyyy = function () {
    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
    return (mm[1] ? mm : "0" + mm[0]) + "/" + yyyy; // padding
};

// a and b are javascript Date objects
var _MS_PER_DAY = 1000 * 60 * 60 * 24;
function dateDiffInDays(a, b) {
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}

function parseDateFromString(str)
{
    var parts = str.split("/");
    return new Date(parseInt(parts[2], 10),
                      parseInt(parts[1], 10) - 1,
                      parseInt(parts[0], 10));
}

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

function isScrolledIntoView(elem) {
    var docViewTop = $(window).scrollTop();
    var docViewBottom = docViewTop + $(window).height();

    var elemTop = $(elem).offset().top;
    var elemBottom = elemTop + $(elem).height();

    return ((elemBottom >= docViewTop) && (elemTop <= docViewBottom)
      && (elemBottom <= docViewBottom) && (elemTop >= docViewTop));
}

//escapeRegExp: label.text().replaceAll("<br/>", "\r\n")
String.prototype.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};

// Check định dạng Email
function xfomatEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\ ".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function CheckAuthentication() {
    var isAuthenticated = $("#isAuthenticated").val();
    if (isAuthenticated == 0 || isAuthenticated == false || isAuthenticated == "false") {
        return false;
    }
    return true;
}

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Process Ajax Error
function ProcessAjaxError(xhr) {
    if (xhr.responseJSON.message == "NotAuthorized") {
        location.href = xhr.responseJSON.LogOnUrl;
    }
    else {
        alert('Có lỗi xảy ra, vui lòng thực hiện lại. Xin cảm ơn!');
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Action Form dùng cho các trang quản lý

function ActionData(data, api_url, redirect_url, callback_func) {
    //with_popup = typeof with_popup !== 'undefined' ? with_popup : false;
    redirect_url = typeof redirect_url !== 'undefined' ? redirect_url : "";
    if (data && api_url) {
        $.ajax({
            type: "POST",
            url: api_url,
            content: "application/json;charset=utf-8",
            dataType: "json",
            data: data,
            success: function (d) {
                if (d.success == true) {
                    //if (d.message) {
                    //    if (with_popup)
                    //        alert(d.message);
                    //}
                    if (d.redirect) {
                        redirect_url = d.redirect;
                    }
                    if (redirect_url) {
                        window.location = redirect_url;
                    }
                    else {
                        if (typeof callback_func == 'function') {
                            callback_func();
                        }
                        else {
                            window.location.reload();
                        }
                    }
                }
                else {
                    if (d.message) {
                        alert(d.message);
                    }
                    else {
                        alert("Không gửi được yêu cầu!");
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                // TODO: Show error
                console.log(xhr.responseText);
                ProcessAjaxError(xhr);
            }
        });
    }
}

function Action(id, api_url, redirect_url) {
    ActionData({ id: id }, api_url, redirect_url);
}

function ActionForm(form_data, api_url, redirect_url) {
    ActionData(form_data, api_url, redirect_url);
}

//function ActionFormWithoutPopup(form_data, api_url, redirect_url) {
//    ActionData(form_data, api_url, redirect_url);
//}

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//AskAndAction

function AskAndAction(action, id, api_url, redirect_url) {
    BootstrapDialog.show({
        cssClass: 'delete-car-dialog',
        title: 'Cảnh báo',
        message: 'Bạn có chắc là muốn ' + action + ' thông tin này không?',
        buttons: [{
            label: 'Không',
            cssClass: 'btn-default',
            action: function (dialog) {
                dialog.close();
            }
        },
        {
            label: 'Có',
            cssClass: 'btn-danger',
            action: function (dialog) {
                Action(id, api_url, redirect_url);
                dialog.close();
            }
        }]
    }).setType(BootstrapDialog.TYPE_DANGER);
}

function AskAndActionData(action, data, api_url, redirect_url, callback_func) {
    BootstrapDialog.show({
        cssClass: 'delete-car-dialog',
        title: 'Cảnh báo',
        message: 'Bạn có chắc là muốn ' + action + ' thông tin này không?',
        buttons: [{
            label: 'Không',
            cssClass: 'btn-default',
            action: function (dialog) {
                dialog.close();
            }
        },
        {
            label: 'Có',
            cssClass: 'btn-danger',
            action: function (dialog) {
                ActionData(data, api_url, redirect_url, callback_func);
                dialog.close();
            }
        }]
    }).setType(BootstrapDialog.TYPE_DANGER);
}


/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//waitingLoadingDialog

var watingLoadingDialog = null;
function showWaitingDialog(message) {
    if (!watingLoadingDialog) {
        watingLoadingDialog = BootstrapDialog.show({
            title: 'Loading',
            message: message
        }).setType(BootstrapDialog.TYPE_PRIMARY);
    }
}

function hideWaitingDialog() {
    if (watingLoadingDialog) {
        watingLoadingDialog.close();
        watingLoadingDialog = null;
    }
}

function ShowBootstrapDialog(title, message, type, isReload) {
    isReload = typeof isReload !== 'undefined' ? isReload : false;
    BootstrapDialog.show({
        title: title,
        message: message,
        buttons: [{
            label: 'Đóng',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    }).setType(type);

    if (isReload) {
        window.location.reload();
    }
}

function ShowBootstrapDialog_Success(title, message, isReload) {
    isReload = typeof isReload !== 'undefined' ? isReload : false;
    ShowBootstrapDialog(title, message, BootstrapDialog.TYPE_SUCCESS, isReload);
}

function ShowBootstrapDialog_Failed(title, message, isReload) {
    isReload = typeof isReload !== 'undefined' ? isReload : false;
    ShowBootstrapDialog(title, message, BootstrapDialog.TYPE_WARNING, isReload);
}

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//show city name in url

//google translate - not use anymore 
//$('input:text').each(function (index) {

//    //Call the Google API
//    $.ajax({
//        type: "GET",
//        url: "https://ajax.googleapis.com/ajax/services/language/translate",
//        dataType: 'jsonp',
//        cache: false,
//        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
//        data: "v=1.0&q=" + $(this).val() + "&langpair=en|vi",
//        success: function (iData) {
//            //update the value 
//            if (iData && iData["responseData"]) {
//                $(this).val(iData["responseData"]["translatedText"]);
//            }
//        },
//        error: function (xhr, ajaxOptions, thrownError) { }
//    });
//});

$("body").on("mouseup", ".auto-select", function () {
    $(this).select();
});

/////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////
$('.btn-copy-translation').click(function () {
    var label = $(this).prev();
    var input = $(this).nextAll('input').first();
    if (input.length == 0)
        input = $(this).nextAll('textarea').first();
    input.val(label.text().replaceAll("<br/>", "\r\n"));
});
$('.btn-original').click(function () {
    var label = $(this).prev();
    var input = $(this).next();
    input.val(label.text());
});

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//admin side menu action

var active_action = $("#active_action").val();
$(".action-menu").each(function () {
    var action = $(this).attr("data-action");
    if (active_action == action)
    {
        $(this).addClass("active");
        $(this).parents(".treeview").addClass("active");
        return false;
    }
});

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Layout Elements Initiation

function PluginLoad() {
    $(".select2").select2();
    //Datemask dd/mm/yyyy
    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    //Datemask2 mm/dd/yyyy
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    //Money Euro
    if ($("input[data-mask]").length > 0) {
        $("input[data-mask]").inputmask();
    }
    //datepicker
    if ($(".monthpicker").length > 0) {
        $(".monthpicker").datepicker({
            format: "mm/yyyy",
            minViewMode: 1,
            autoclose: true
        }).on("changeDate", function (e) {
            var date = $(this).datepicker("getDate");
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            $(this).attr("data-start-date", firstDay.ddmmyyyy());
            $(this).attr("data-end-date", lastDay.ddmmyyyy());
        });
    }
    if ($(".monthdatepicker").length > 0) {
        $(".monthdatepicker").datepicker({
            format: "dd/mm/yyyy",
            minViewMode: 0,
            autoclose: true
        }).on("changeDate", function (e) {
            var date = $(this).datepicker("getDate");
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            $(this).attr("data-start-date", firstDay.ddmmyyyy());
            $(this).attr("data-end-date", lastDay.ddmmyyyy());
        });
    }
    //Date range as a button
    $('#daterange-btn').daterangepicker(
        {
            ranges: {
                'Tất cả ngày tháng': [null, null],
                'Hôm nay': [moment(), moment()],
                'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Tuần này': [moment().startOf('isoweek'), moment().endOf('isoweek')],
                'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Tháng sau': [moment().add(1, 'month').startOf('month'), moment().add(1, 'month').endOf('month')]
            },
            startDate: moment().subtract(29, 'days'),
            endDate: moment()
        },
        function (start, end) {
            $("input[name='Start_Date']").val(start.format('DD/MM/YYYY'));
            $("input[name='End_Date']").val(end.format('DD/MM/YYYY'));
            var text = start.format('DD/MM/YYYY') + " - " + end.format('DD/MM/YYYY');
            if (!start.isValid())
                text = "Tất cả ngày tháng";
            $("#daterange-btn span").html(text);

            //FilterDeal(false);
        }
    );
    $(".textbox-price").on("keyup", function (e) {
        var val = parseInt($(this).val());
        var n = parseInt($(this).val().replace(/\D/g, ''), 10);
        if (n) {
            $(this).val(n.formatMoney());
        }
        //$(this).val(val.formatMoney());
    });
}

//Initialize Select2 Elements
//$(".select2").select2();
PluginLoad();

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//select-students

function formatRepo(state) {
    if (!state.id) { return state.text; }
    var $state = $(
      //'<span><img src="' + state.Student_Avatar + '" class="img-flag" /> ' + state.Student_FullName + '</span>'
      '<div class="user-block">' +
            '<img class="img-circle img-bordered-sm" src="' + state.Student_Avatar.removeMVCPath() + '" alt="user image">' +
            '<span class="username">' +
                '' + state.Student_FullName + '' +
            '</span>' +
            '<span class="description">' + state.Student_EnrollNumber + '</span>' +
        '</div>'
    );
    return $state;
};

function formatRepoSelection(state) {
    return state.Student_FullName || state.text;
}

if ($(".select-students").length > 0) {
    $(".select-students").select2({
        ajax: {
            url: ROOTPATH + "/ManageAcademic/FilterStudent_Ajax",
            dataType: 'json',
            delay: 250,
            type: "POST",
            dropdownParent: "#add-modal",//remove tabindex="-1" from modal
            data: function (params) {
                return {
                    q: params.term, // search term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 1,
        templateResult: formatRepo,
        templateSelection: formatRepoSelection
    });
}

function setSelectTargetValue($this)
{
    if ($this.length > 0) {
        var val = $this.find("option:selected").attr("data-value");
        var targets = $this.attr("data-target").split(",");

        for (var i = 0; i < targets.length; i++) {
            $this.parents(".modal").find("input[name='" + targets[i] + "']").val(val);
        }
    }
}

$("body").on("change", ".modal select.target-select", function () {
    var $this = $(this);

    setSelectTargetValue($this);
});

$("body").on("shown.bs.modal", "#add-modal", function (e) {
    var $this = $("#add-modal").find("select.target-select");

    setSelectTargetValue($this);
});

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Show Student FeePlan Modal
$("body").on("click", ".feeplan-status", function (e) {
    e.preventDefault();

    var modal = $("#feeplan-modal");
    modal.modal("show");
    var $container = modal.find(".modal-body");
    $container.addClass("loading");

    var $this = $(this);
    var id = $this.attr("data-id");//feeplan id
    //var active = true;
    //if ($("#deal-history-row-active").length > 0)
        //active = true;//$("#deal-history-row-active").val();

    $.ajax({
        type: "GET",
        url: ROOTPATH + "/ManageAcademic/GetStudentFeePlanDetail",
        contentType: "application/json; charset=utf-8",
        data: {
            id: id
        },
        success: function (d) {
            $container.html(d);
            $container.removeClass("loading");
            //if (active)
            //    $this.parents("tr").addClass("active");
        },
        error: function (xhr, textStatus, errorThrown) {
            // TODO: Show error
            console.log(xhr.responseText);
            //alert(xhr.responseText);
            ProcessAjaxError(xhr);
        }
    });
});

