function resetByDateText() {
    $('.by-date #daterange-btn span').html("Theo ngày");
    $('.by-month .input-group .date').val("Theo tháng");
    $(".by-quater .by-quater-text").html("Theo quý");
}

//Date range as a button
$('.by-date #daterange-btn').daterangepicker(
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
        var start_date = start.format('DD/MM/YYYY');
        var end_date = end.format('DD/MM/YYYY');
        //$("input[name='Start_Date']").val(start_date);
        //$("input[name='End_Date']").val(end_date);
        var text = start_date + " - " + end_date;
        if (!start.isValid())
            text = "Tất cả ngày tháng";
        //else
        //{
        //    //set url
        //    var url = "/Admin/ReportByCompany?Start_Date=" + start_date + "&End_Date=" + end_date;
        //    history.pushState({}, '', url);
        //}

        //$('.by-month .input-group .date').val("Theo tháng");
        resetByDateText();
        $(".by-date #daterange-btn span").html(text);
        $(".content-header .form-group").removeClass("active");
        $(".by-date #daterange-btn").parents(".form-group").addClass("active");

        ReloadFilterReport(start_date, end_date);
        //FilterReport();
    }
);

//datepicker
$('.by-month .input-group .date').datepicker({
    format: "mm/yyyy",
    minViewMode: 1,
    autoclose: true
}).on("changeDate", function (e) {
    var date = $(this).datepicker("getDate");
    var start = new Date(date.getFullYear(), date.getMonth(), 1);
    var end = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    //$(this).attr("data-start-date", firstDay.ddmmyyyy());
    //$(this).attr("data-end-date", lastDay.ddmmyyyy());

    //$("input[name='Start_Date']").val(start.ddmmyyyy());
    //$("input[name='End_Date']").val(end.ddmmyyyy());

    //$('.by-date #daterange-btn span').html("Theo ngày");
    resetByDateText();
    $('.by-month .input-group .date').val(date.mmyyyy());
    $(".content-header .form-group").removeClass("active");
    $(this).parents(".form-group").addClass("active");

    //FilterReport();
    ReloadFilterReport(start.ddmmyyyy(), end.ddmmyyyy());
});

//by-quater
$(".by-quater .dropdown-menu li a").click(function (e) {
    e.preventDefault();

    var start_date = $(this).attr("data-start-date");
    var end_date = $(this).attr("data-end-date");

    //$("input[name='Start_Date']").val(start);
    //$("input[name='End_Date']").val(end);

    resetByDateText();
    $(".by-quater .by-quater-text").html($(this).html());
    $(".content-header .form-group").removeClass("active");
    $(this).parents(".form-group").addClass("active");

    //FilterReport();
    ReloadFilterReport(start_date, end_date);
});

//////////////////////////////////////////////////////
/////////////////////////////////////////////////////
//Filter Report
var $container = $("#filter-container").find(".Search_Result");
var reservedInfo = $container.html();
function FilterReport() {
    var $form = $("#filter-form");
    var url = $form.attr("action");

    var fd = new FormData();
    var values = $form.serializeArray();
    for (var i in values) {
        fd.append(values[i].name, values[i].value);
    }

    $container.addClass("loading");
    $.ajax({
        url: url,
        data: fd,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (d) {
            $container.html(d);
            reservedInfo = $container.html();
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

function ReloadFilterReport(Start_Date, End_Date) {
    $("input[name='Start_Date']").val(Start_Date);
    $("input[name='End_Date']").val(End_Date);
    var action = $("#hdAction").val();

    var params = location.href.split('?');
    var url = params[0];//"";
    url += "?";

    if (params.length > 1) {
        params = params[1].split('&');
        for (var i = 0; i < params.length; i++) {
            var param = params[i];
            if (param.indexOf("Start_Date") > -1) {
                url += "Start_Date=" + Start_Date + "&";
            }
            else if (param.indexOf("End_Date") > -1) {
                url += "End_Date=" + End_Date + "&";
            }
            else {
                url += param + "&";
            }
        }
    }
    else {
        url += "Start_Date=" + Start_Date + "&End_Date=" + End_Date;
    }

    if (url[url.length - 1] == '&')
        url = url.slice(0, -1);

    history.pushState({}, '', url);

    FilterReport();
}

$("body").on("change", "select.filter-data", function (e) {
    var val = $(this).find("option:selected").val();
    var filter = $(this).attr("data-filter");
    $("#filter-form input[name='" + filter + "']").val(val);

    FilterReport();
});