//Timepicker
function initTimePicker() {
    $(".timepicker").timepicker({
        showInputs: false,
        maxHours: 24,
        showMeridian: false,
        defaultTime: 'value'
        //defaultTime: setTime(this)
        //appendWidgetTo: '#add-modal .modal-content'
    }).on('changeTime.timepicker', function (e) {
        $(this).parent().find("#timepicker-value").val(e.time.hours + "," + (e.time.minutes*100/60));

        //console.log('The time is ' + e.time.value);
        //console.log('The hour is ' + e.time.hours);
        //console.log('The minute is ' + e.time.minutes);
        //console.log('The meridian is ' + e.time.meridian);
    });
}

initTimePicker();

function PagePluginLoad()
{
    initTimePicker();
}

function GetModulesByCourseFamily(course_id, $form)
{
    $.ajax({
        url: ROOTPATH + "/ManageSchedule/GetModulesByCourseFamily",
        type: 'POST',
        content: "application/json;charset=utf-8",
        dataType: 'json',
        data: {
            id: course_id
        },
        success: function (d) {
            var $select_blocks = $form.find("select[name='Module_Id']");
            $select_blocks.empty();
            if (d && d.length > 0) {
                $.each(d, function (index, value) {
                    $select_blocks.append("<option value='" + value.Id + "' data-value='" + value.Target_Value + "'>" + value.Name + "</option>");
                });
            }
            $select_blocks.select2();
        },
        error: function (xhr, textStatus, errorThrown) {
            // TODO: Show error
            console.log(xhr.responseText);
            ProcessAjaxError(xhr);
        }
    });
}

$("body").on("change", ".modal select[name='Class_Id']", function () {
    //var duration = $(this).find("option:selected").attr("data-duration");

    //$(this).parents(".modal").find("input[name='Class_Module_DurationByDay']").val(duration);
    var $form = $(this).parents(".modal");
    var $current_class = $(this).find("option:selected");
    var course_id = $current_class.attr("data-value");
    if (course_id) {
        $form.find("select[name='Course_Id']").select2("val", course_id);

        GetModulesByCourseFamily(course_id, $form);
    }

    //class day
    var class_day = $current_class.attr("data-classday");
    if (class_day)
    {
        $form.find("select[name='Class_Module_Day']").select2("val", class_day);
    }

    //class hour
    var classhourstart = $current_class.attr("data-classhourstart");
    if (classhourstart) {
        $form.find("input[name='Class_Module_Hour_Start']").val(classhourstart);
        $form.find(".Class_Module_Hour_Start .timepicker").val($current_class.attr("data-classhourstartstring"));
    }
    var classhourend = $current_class.attr("data-classhourend");
    if (classhourend) {
        $form.find("input[name='Class_Module_Hour_End']").val(classhourend);
        $form.find(".Class_Module_Hour_End .timepicker").val($current_class.attr("data-classhourendstring"));
    }
});

$("body").on("change", ".modal select[name='Course_Id']", function () {
    var $form = $(this).parents(".modal");
    var course_id = $(this).find("option:selected").val();

    GetModulesByCourseFamily(course_id, $form);
});