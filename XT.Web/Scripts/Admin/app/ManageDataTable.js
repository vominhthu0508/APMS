
///////////////////////////////////////////////////
//////////////////////////////////////////////////
//ManageResource
$(".select-page-size").change(function (e) {
    this.form.submit();
});
$("._btnEdit").click(function (e) {
    e.preventDefault();
    //check validation
    ActionForm($(this.form).serialize(), postEditLink);
})
$(".btnAddRow").click(function (e) {
    e.preventDefault();
    var currentRow = parseInt($("#currentRow").val()) + 1;
    $("#currentRow").val(currentRow);
    var value = '<tr>'
                   + ' <td> '
                     + '   <textarea class="form-control" cols="20" id="lstResx_' + currentRow + '__Id" name="lstResx[' + currentRow + '].Id" rows="2"></textarea>'
               + '     </td> '
               + '     <td> '
               + '         <textarea class="form-control" cols="20" id="lstResx_' + currentRow + '__English" name="lstResx[' + currentRow + '].English" rows="2"></textarea> '
               + '     </td> '
                + '    <td> '
                + '        <textarea class="form-control" cols="20" id="lstResx_' + currentRow + '__Viet" name="lstResx[' + currentRow + '].Viet" rows="2"></textarea> '
                 + '   </td>'
                + '</tr>';
    $("#tbAddResx").append(value);
})
$(".btnTranslate").click(function (e) {
    e.preventDefault();
    $("label[style='display:none']").each(function (index) {
        var id = $(this).attr("class");
        $("textarea#" + id).val($.trim($(this).text()));
    })
})
$('body').on("click", ".btn-delete-translation", function (e) {
    e.preventDefault();
    var action = $(this).attr("data-action");
    var id = $(this).attr("data-id");
    var api_url = $(this).attr("data-api");
    var parentid = $(this).attr("data-parent-id");
    var entity = $(this).attr("data-entity");

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
                    $.ajax({
                        url: api_url,
                        type: 'POST',
                        content: "application/json;charset=utf-8",
                        dataType: 'json',
                        data: {
                            id: id,
                            parentid: parentid,
                            entity: entity
                        },
                        success: function (data) {
                            dialog.close();
                            location.reload();
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            // TODO: Show error
                            console.log(xhr.responseText);
                            ProcessAjaxError(xhr);
                        }
                    });
                }
            }]
    }).setType(BootstrapDialog.TYPE_DANGER);
});

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Manage Page
$("#btnAddPage").click(function (e) {
    for (var instanceName in CKEDITOR.instances)
        CKEDITOR.instances[instanceName].updateElement();
    e.preventDefault();
    //check validation
    if (typeof checkDataValidation == 'function') {
        if (checkDataValidation(this.form) == false)
            return;
    }
    //if (typeof SendRequestToServer == 'function') {
    //    //Nếu có tồn tại Image thì post Image cùng với Form Data đến postAddLink và return (không ActionForm nữa)
    //    SendRequestToServer(this.form, postAddLink);
    //}
    if (isUploadImage == "true") {
        //Nếu có tồn tại Image thì post Image cùng với Form Data đến postAddLink và return (không ActionForm nữa)
        SendRequestToServer(this.form, postAddLink);
    }
    else {
        ActionForm($(this.form).serialize(), postAddLink);
    }
})

$(".btnEditPage").click(function (e) {
    for (var instanceName in CKEDITOR.instances)
        CKEDITOR.instances[instanceName].updateElement();
    e.preventDefault();
    //check validation
    if (typeof checkDataValidation == 'function') {
        if (checkDataValidation(this.form) == false)
            return;
    }
    if (isUploadImage == true) {
        SendRequestToServer(this.form, postEditLink);
    }
    else {
        ActionForm($(this.form).serialize(), postEditLink);
    }
})

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//CKEDITOR
//CKEDITOR.replace('Payment_Timeline', {
//    filebrowserImageUploadUrl: '/Admin/UploadImage'
//});
//CKEDITOR.replace('Payment_Discount', {
//    filebrowserImageUploadUrl: '/Admin/UploadImage'
//});
//CKEDITOR.replace('Payment_Promotion', {
//    filebrowserImageUploadUrl: '/Admin/UploadImage'
//});
function LoadCkeditor() {
    var multiple = $(".room_block").find("textarea");
    for (var i = 0; i < multiple.length; i++) {
        var name = multiple.eq(i).attr("id");
        var isEditor = multiple.eq(i).attr("data-editor");
        if (isEditor == "true") {
            CKEDITOR.replace(name, {
                filebrowserImageUploadUrl: '/Admin/UploadImage'
            });
        }
    }
};
window.onload = LoadCkeditor;

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Manage Action
//$('.btnDelete').on("click", function (e) {
//    e.preventDefault();
//    var action = "xóa";
//    var id = $(this).attr("data-id");
//    var api_url = postDeleteLink;

//    AskAndAction(action, id, api_url);
//});

$('body').on("click", ".btn-admin-action", function (e) {
    e.preventDefault();
    var action = $(this).attr("data-action");
    var id = $(this).attr("data-id");
    var current_id = $(this).attr("data-current-id");
    var data = current_id ? { id: id, current_id: current_id } : { id: id };
    var api_url = $(this).attr("data-api");

    var callback_func = null;
    //if ($(this).closest(".can-delete").length > 0) {
    //    var li = $(this).closest(".can-delete");
    //    callback_func = function () {
    //        li.remove();
    //    };
    //}
    if (typeof ReloadTableDataAfterSubmit == 'function') {
        callback_func = ReloadTableDataAfterSubmit;
    }

    AskAndActionData(action, data, api_url, null, callback_func);
});

function ReloadChildrenData($this, page, type) {
    var parent = $this.parents(".ajax-search-group");
    var user_id = parent.attr("data-id");
    var url = parent.attr("data-url");

    var $container = parent.find(".Search_Result");
    $container.addClass("loading");

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: {
            user_id: user_id,
            page: page,
            type: type
        },
        success: function (d) {
            $container.html(d);
            $container.removeClass("loading");
        },
        error: function (xhr, textStatus, errorThrown) {
            // TODO: Show error
            console.log(xhr.responseText);
            //alert(xhr.responseText);
            ProcessAjaxError(xhr);
        }
    });
}

//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
////////////////////////////// Profile Page///////////////////////////

$('body').on("click", ".ajax-search-group-paging .pagination-container .pagination > li > a", function (e) {
    e.preventDefault();
    var $this = $(this);

    ReloadChildrenData($this, $this.attr("href").replace('#', ''));
});

$("body").on("click", ".ajax-search-group-paging ul.dropdown-menu > li > a", function (e) {
    e.preventDefault();
    var $this = $(this);

    var type = $this.attr("data-type");
    var type_name = $this.html();
    $this.parents(".ajax-search-group").attr("data-type", type);
    $this.parents(".ajax-search-group").find(".dropdown-toggle").html(type_name + ' <span class="caret"></span>');

    ReloadChildrenData($this, 1, type);
});

//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
////////////////////////////// Ajax search///////////////////////////
var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();

$(".textboxSearch, .txtSearchBox").each(function () {
    $(this).data("reservedInfo", $(this).parents(".ajax-search-group").find(".Search_Result").html());
});

function doTextboxSearch($this, url, getData) {
    var reservedInfo = $this.data("reservedInfo");
    delay(function () {
        var name = $this.val();
        var $container = $this.parents(".ajax-search-group").find(".Search_Result");
        if (name && name.length >= 1) {
            $container.addClass("loading");

            var data = getData();

            $.ajax({
                type: "GET",
                url: url,
                contentType: "application/json; charset=utf-8",
                data: data,
                success: function (d) {
                    $container.html(d);
                    $container.removeClass("loading");
                },
                error: function (xhr, textStatus, errorThrown) {
                    // TODO: Show error
                    console.log(xhr.responseText);
                    //alert(xhr.responseText);
                    ProcessAjaxError(xhr);
                }
            });
        }
        else {
            if (!name) {
                $container.html(reservedInfo);
            }
        }

    }, 500);
}

//_partial_Search_Form.cshtml
$(".textboxSearch").keyup(function () {
    var $this = $(this);
    //var url = $this.closest("form").attr("action");
    var url = $this.closest(".search-form").attr("data-action");
    var id = $this.parents(".ajax-search-group").attr("data-id");

    doTextboxSearch($this, url, function () {
        return {
            name: $this.val(),
            id: id
        };
    });
});

//_partial_Search_Form2.cshtml
$(".txtSearchBox").keyup(function () {
    var $this = $(this);
    var url = $this.closest(".search-form").attr("data-action");//"/Admin/SearchByName";
    var model = $this.closest(".search-form").attr("data-model"); //$this.closest("form").data("model");

    doTextboxSearch($this, url, function () {
        return {
            name: $this.val(),
            model: model
        };
    });
});

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Show row detail

$("body").on("click", "table.dataTable tbody a.viewdetail", function (e) {
    e.preventDefault();
    var tr = $(this).parents("tr");
    var tbody = tr.parent();

    if (tr.hasClass("active")) {
        tr.removeClass("active");
    }
    else {
        tbody.removeClass("active");
        tr.addClass("active");
    }
});

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Filter Form

function RewriteUrl($form)
{
    var params = location.href.split('?');
    var new_url = params[0];

    var values = $form.serializeArray();
    if (values.length > 0) {
        new_url += "?";
        for (var i in values) {
            new_url += values[i].name + "=" + values[i].value + "&";
        }
    }

    //rewrite url
    if (new_url[new_url.length - 1] == '&')
        new_url = new_url.slice(0, -1);

    history.pushState({}, '', new_url);
}

if ($("#filter-form").length > 0) {
    //reserve search result container
    var $container = $("#filter-form").parents(".ajax-search-group").find(".Search_Result");
    var $form = $("#filter-form");
    var url = $form.attr("action");
    //var reservedInfo = $container.html();

    function FilterForm(pageChange) {
        console.log("FilterForm");

        //loading = loading == undefined ? false : loading;
        pageChange = pageChange == undefined ? true : pageChange;
        if (pageChange)
        {
            $form.find("input[name='pageChange']").val(0);
        }

        var fd = new FormData();
        var values = $form.serializeArray();
        if (values.length > 0) {
            for (var i in values) {
                fd.append(values[i].name, values[i].value);
            }
        }

        //rewrite url
        //RewriteUrl($form);

        $container.addClass("loading");
        $.ajax({
            url: url,
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (d) {
                $container.html(d);
                //if (!isSearch) {
                //    reservedInfo = $container.html();
                //}
                $container.removeClass("loading");
                PluginLoad();
                //$form.find("input[name='pageChange']").val(0);
                //$("#filter-form input[name='pageChange']").val(0);

                //PagePluginLoad
                if (typeof PagePluginLoad == 'function') {
                    PagePluginLoad();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                // TODO: Show error
                console.log(xhr.responseText);
                //alert(xhr.responseText);
                ProcessAjaxError(xhr);
            }
        });
    }

    $('#form-daterange-btn').daterangepicker(
        {
            ranges: {
                'Tất cả ngày tháng': [null, null],
                'Hôm nay': [moment(), moment()],
                'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '7 ngày qua': [moment().subtract(6, 'days'), moment()],
                '30 ngày qua': [moment().subtract(29, 'days'), moment()],
                'Tuần này': [moment().startOf('isoweek'), moment().endOf('isoweek')],
                'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
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
            $("#form-daterange-btn span").html(text);

            FilterForm();
        }
    );

    $('#filter-form .daterange-btn').daterangepicker(
    {
        ranges: {
            'Tất cả ngày tháng': [null, null],
            'Hôm nay': [moment(), moment()],
            'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Tháng này': [moment().startOf('month'), moment().endOf('month')],
            'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'Tháng sau': [moment().add(1, 'month').startOf('month'), moment().add(1, 'month').endOf('month')]
        },
        startDate: moment().subtract(29, 'days'),
        endDate: moment()
    },
        function (start, end) {
            var Start_Date = "";
            var End_Date = "";
            var text = "";
            if (start.isValid()) {
                Start_Date = start.format('DD/MM/YYYY');
                End_Date = end.format('DD/MM/YYYY');
                text = Start_Date + " - " + End_Date;
            }
            else {
                text = "Tất cả ngày tháng";
            }

            var $parent = this.element.closest(".daterange-box");

            $parent.find("input.Start_Date").val(Start_Date);
            $parent.find("input.End_Date").val(End_Date);

            $parent.find(".daterange-btn span").html(text);

            FilterForm();
        }
    );

    $("#filter-form .monthdatepicker").datepicker({
        format: "dd/mm/yyyy",
        minViewMode: 0,
        autoclose: true
    }).on("changeDate", function (e) {
        var date = $(this).datepicker("getDate");
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        $(this).attr("data-start-date", firstDay.ddmmyyyy());
        $(this).attr("data-end-date", lastDay.ddmmyyyy());

        FilterForm();
    });

    $("#filter-form .monthpicker").datepicker({
        format: "mm/yyyy",
        minViewMode: 1,
        autoclose: true
    }).on("changeDate", function (e) {
        var date = $(this).datepicker("getDate");
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        $(this).attr("data-start-date", firstDay.ddmmyyyy());
        $(this).attr("data-end-date", lastDay.ddmmyyyy());

        FilterForm();
    });

    //ajax actions
    $("#filter-form select").on("change", function (e) {
        e.preventDefault();

        var isAjax = $(this).attr("data-isajax");
        if (isAjax != "none")
            FilterForm();
    });

    $("#filter-form input[type='text']").keyup(function () {
        delay(function () {
            FilterForm();
        }, 500);
    });

    //if (url.indexOf("FilterByName") == -1) {
    //    $('body').on("click", ".ajax-search-group .pagination-container .pagination > li > a", function (e) {
    //        e.preventDefault();

    //        //var page = $(this).html();
    //        var page = $(this).attr("href").replace('#', '');

    //        $("#filter-form input[name='page']").val(page);
    //        $("#filter-form input[name='pageChange']").val(1);

    //        FilterForm(false);
    //    });
    //}

    $('body').on("click", ".ajax-search-group .pagination-container .pagination > li > a", function (e) {
        e.preventDefault();

        //var page = $(this).html();
        var page = $(this).attr("href").replace('#', '');

        $("#filter-form input[name='page']").val(page);
        $("#filter-form input[name='pageChange']").val(1);

        FilterForm(false);
    });

    /////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////
    ///sorting

    $("body").on("click", ".sorting", function (e) {
        e.preventDefault();

        var asc = true;

        if ($(this).hasClass("asc")) {
            asc = false;
            $(".table thead th").removeClass("asc").removeClass("desc");
            $(this).addClass("desc");//false
        }
        else {
            asc = true;
            $(".table thead th").removeClass("asc").removeClass("desc");
            $(this).addClass("asc");//true
        }

        var sorting_target = $(this).attr("data-sorting");
        $("#filter-form input[name='sort_target']").val(sorting_target);
        $("#filter-form input[name='sort_rank']").val(asc);

        FilterForm();
    });

    function ReloadTableDataAfterSubmit() {
        //if (typeof ReloadTableDataAfterSubmit_Before == 'function') {
        //    ReloadTableDataAfterSubmit_Before();
        //}

        FilterForm(false);
        //$(".modal").modal("hide");
        //$('body').removeClass('modal-open');
        //$('body').css("padding", "0px");
        //$('.modal-backdrop').remove();

        //if (typeof ReloadTableDataAfterSubmit_After == 'function') {
        //    ReloadTableDataAfterSubmit_After();
        //}
    }
}
