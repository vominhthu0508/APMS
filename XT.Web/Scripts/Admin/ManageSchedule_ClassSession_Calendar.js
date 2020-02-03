$(function () {
    var SUNDAY = 0, MONDAY = 1;
    var now = new Date();

    /* initialize the external events
         -----------------------------------------------------------------*/
    function ini_events(ele) {
        ele.each(function () {

            // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
            // it doesn't need to have a start or end
            var eventObject = {
                title: $.trim($(this).text()) // use the element's text as the event title
            };

            // store the Event Object in the DOM element so we can get to it later
            $(this).data('eventObject', eventObject);

            //// make the event draggable using jQuery UI
            //$(this).draggable({
            //    zIndex: 1070,
            //    revert: true, // will cause the event to go back to its
            //    revertDuration: 0  //  original position after the drag
            //});

            // store data so the calendar knows to render an event upon drop
            //$(this).data('event', {
            //    title: $.trim($(this).text()), // use the element's text as the event title
            //    //stick: true // maintain when user navigates (see docs on the renderEvent method)
            //});

            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });
        });
    }
    ini_events($('#external-events div.external-event'));

    /* initialize the calendar
     -----------------------------------------------------------------*/
    function render_events(events) {
        $('#calendar').fullCalendar({
            schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source',
            minTime: '07:00',
            maxTime: '22:00',
            now: now,
            editable: true, // enable draggable events
            droppable: true, // this allows things to be dropped onto the calendar
            aspectRatio: 1.8,
            scrollTime: '00:00', // undo default 6am scrollTime
            header: {
                left: 'today prev,next',
                center: 'title',
                right: 'timelineDay,agendaWeek,month'
            },
            defaultView: 'agendaWeek',
            columnFormat: 'ddd D/M',
            firstDay: MONDAY,
            //titleFormat: 'D/M/YYYY',
            views: {
                timelineDay: { // name of view
                    titleFormat: 'dddd D/M/YYYY',
                    // other view-specific options here
                },
                agendaWeek: { // name of view
                    titleFormat: 'D/M/YYYY',
                    // other view-specific options here
                }
            },
            resourceLabelText: 'Rooms',
            resources: [
                { id: '1', title: 'Lab 1' },
                { id: '2', title: 'Lab 2' },
                { id: '3', title: 'Lab 3' },
                { id: '4', title: 'Lab 4' },
                { id: '5', title: 'Lab 5' },
                { id: '6', title: 'Lab 6' },
                { id: '7', title: 'Class 1' },
                { id: '8', title: 'Class 2' },
                { id: '9', title: 'Conference' },
                { id: '10', title: 'Studio' },
                { id: '', title: '' },
            ],
            events: events,
            drop: function (date, allDay) { // this function is called when something is dropped

                // retrieve the dropped element's stored Event Object
                var originalEventObject = $(this).data('eventObject');

                // we need to copy it, so that multiple events don't have a reference to the same object
                var copiedEventObject = $.extend({}, originalEventObject);

                // assign it the date that was reported
                copiedEventObject.start = date;
                copiedEventObject.allDay = false;
                copiedEventObject.backgroundColor = $(this).css("background-color");
                copiedEventObject.borderColor = $(this).css("border-color");

                // render the event on the calendar
                // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

            },
            eventReceive: function (event) { // called when a proper external event is dropped
                console.log('eventReceive', event);
            },
            eventDrop: function (event) { // called when an event (already on the calendar) is moved
                console.log('eventDrop', event);
            },
            dayClick: function (date, jsEvent, view, resourceObj) {

                //alert('Date: ' + date.format());
                //alert('Resource ID: ' + resourceObj.id);

            },
            eventDragStop: function (event, jsEvent, ui, view) {
                var confirmDel = confirm("Are you sure?");
                if (confirmDel == true) {
                    var trashEl = $('div.box-body');
                    var ofs = trashEl.offset();
                    var x1 = ofs.left;
                    var x2 = ofs.left + trashEl.outerWidth(true);
                    var y1 = ofs.top;
                    var y2 = ofs.top + trashEl.outerHeight(true);

                    if (jsEvent.pageX >= x1 && jsEvent.pageX <= x2 &&
                        jsEvent.pageY >= y1 && jsEvent.pageY <= y2) {
                        $('#calendar').fullCalendar('removeEvents', event.id);
                    }
                }
            },
            // select many cell
            selectable: true,
            selectHelper: true,
            editable: true,
            // function add event when select cell
            select: function (start, end, jsEvent, view, resourceObj) {
                $('#add-modal').modal({ show: true });
                //$("#selectRoom").val(resourceObj.id);
                $('#timestart>input').val(start.format().toString().substring(11, 16));
                $('#timeend>input').val(end.format().toString().substring(11, 16));

                $('#save').bind("click", function () {
                    if (resourceObj == undefined) {
                        var resourceId = $('#selectRoom').val();
                    } else {
                        var resourceId = resourceObj.id;
                    }
                    var monHoc = $('#txtMonHoc').val();
                    var loaiPhong = $('#selectRoomType').val();
                    var lophoc = $('#txtLopHoc').val();
                    eventData = {
                        id: id,
                        resourceId: resourceId,
                        title: "test"/*lophoc + " - " + monHoc + " " + $("#selectRoom").find(":selected").text() + " " + loaiPhong*/,
                        start: $.fullCalendar.moment(start).time($('#timestart>input').val()),
                        end: $.fullCalendar.moment(end).time($('#timeend>input').val()),
                        color: "red"
                    };
                    /* events.push({
                        id: id,
                        resourceId: '4',
                        start: start,
                        end: end,
                        title: 'C1507L - MP - Lab 4',
                        backgroundColor: "#3c8dbc", //yellow
                        borderColor: "#3c8dbc", //yellow
                    });
                    */

                    $("#add-modal").modal('hide');
                    $('#calendar').fullCalendar('renderEvent', eventData, true);
                    $('#calendar').fullCalendar('unselect');
                    $('#save').unbind("click");
                    id++;

                });
                $("#close").click(function () {
                    $('#calendar').fullCalendar('unselect');
                    $('#save').unbind("click");
                });
            }
        });
    }

    function load_events() {
        //Date for the calendar events (dummy data)
        //var date = new Date();
        //var d = date.getDate(),
        //        m = date.getMonth(),
        //        y = date.getFullYear();
        //var events = [];//2,4,6
        
        //var first_event = d;
        //var id = 1;
        //for (var i = 1; i <= 10; i++) {
        //    var _d = first_event;
        //    var start = new Date(y, m, _d, 8, 0);
        //    var end = new Date(y, m, _d, 11, 0);

        //    first_event += 2;
        //    //var event_date = moment(new Date(y, m, first_event)).day();
        //    //if (event_date == SUNDAY || event_date == MONDAY)//Sunday
        //    //{
        //    //    first_event += 1;
        //    //}

        //    events.push({
        //        id: id,
        //        resourceId: '1',
        //        start: start,
        //        end: end,
        //        title: 'C1504L - TGP - Lab 1',
        //        backgroundColor: "#00a65a", //yellow
        //        borderColor: "#00a65a", //yellow
        //    });
        //    id++;
        //    events.push({
        //        id: id,
        //        resourceId: '3',
        //        start: start,
        //        end: end,
        //        title: 'C1506L - IM - Lab 3',
        //        backgroundColor: "#00c0ef", //yellow
        //        borderColor: "#00c0ef", //yellow
        //    });
        //    id++;
        //}

        $.ajax({
            url: ROOTPATH + "/ManageSchedule/GetClassSession_Data",
            type: 'POST',
            content: "application/json;charset=utf-8",
            dataType: 'json',
            data: {
                
            },
            success: function (data) {
                var events = data;

                for (var i = 0; i < events.length; i++)
                {
                    events[i].start = new Date(events[i].start);
                    events[i].end = new Date(events[i].end);
                }

                render_events(events);
            },
            error: function (xhr, textStatus, errorThrown) {
                // TODO: Show error
                console.log(xhr.responseText);
                ProcessAjaxError(xhr);
            }
        });

        
    }

    load_events();

    /* ADDING EVENTS */
    //var currColor = "#3c8dbc"; //Red by default
    ////Color chooser button
    //var colorChooser = $("#color-chooser-btn");
    //$("#color-chooser > li > a").click(function (e) {
    //    e.preventDefault();
    //    //Save color
    //    currColor = $(this).css("color");
    //    //Add color effect to button
    //    $('#add-new-event').css({ "background-color": currColor, "border-color": currColor });
    //});
    //$("#add-new-event").click(function (e) {
    //    e.preventDefault();
    //    //Get value and make sure it is not null
    //    var val = $("#new-event").val();
    //    if (val.length == 0) {
    //        return;
    //    }

    //    //Create events
    //    var event = $("<div />");
    //    event.css({ "background-color": currColor, "border-color": currColor, "color": "#fff" }).addClass("external-event");
    //    event.html(val);
    //    $('#external-events').prepend(event);

    //    //Add draggable funtionality
    //    ini_events(event);

    //    //Remove event from text input
    //    $("#new-event").val("");
    //});

    ////Date range as a button
    //$('#daterange-btn').daterangepicker(
    //    {
    //        ranges: {
    //            'Tất cả ngày tháng': [null, null],
    //            'Hôm nay': [moment(), moment()],
    //            'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    //            '7 ngày qua': [moment().subtract(6, 'days'), moment()],
    //            '30 ngày qua': [moment().subtract(29, 'days'), moment()],
    //            'Tuần này': [moment().startOf('isoweek'), moment().endOf('isoweek')],
    //            'Tháng này': [moment().startOf('month'), moment().endOf('month')],
    //            'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    //        },
    //        startDate: moment().subtract(29, 'days'),
    //        endDate: moment()
    //    },
    //    function (start, end) {
    //        $("input[name='Start_Date']").val(start.format('DD/MM/YYYY'));
    //        $("input[name='End_Date']").val(end.format('DD/MM/YYYY'));
    //        var text = start.format('DD/MM/YYYY') + " - " + end.format('DD/MM/YYYY');
    //        if (!start.isValid())
    //            text = "Tất cả ngày tháng";
    //        $("#daterange-btn span").html(text);

    //        //FilterDeal(false);
    //    }
    //);

    //var isElemOverDiv = function (draggedItem, dropArea) {
    //    // Prep coords for our two elements
    //    var a = $(draggedItem).offset();
    //    a.right = $(draggedItem).outerWidth() + a.left;
    //    a.bottom = $(draggedItem).outerHeight() + a.top;

    //    var b = $(dropArea).offset();
    //    a.right = $(dropArea).outerWidth() + b.left;
    //    a.bottom = $(dropArea).outerHeight() + b.top;

    //    // Compare
    //    if (a.left >= b.left
    //        && a.top >= b.top
    //        && a.right <= b.right
    //        && a.bottom <= b.bottom) { return true; }
    //    return false;
    //}

    //$(function () {
    //    var availableTags = [
    //    "C1507G",
    //    "C1407G",
    //    "C1307G"
    //    ];
    //    $("#txtLopHoc").autocomplete({
    //        source: availableTags
    //    });
    //});

    //$("#timestart").datetimepicker({
    //    //language: 'en',
    //    weekStart: 1,
    //    todayBtn: 0,
    //    autoclose: 1,
    //    todayHighlight: 0,
    //    startView: 1,
    //    minView: 0,
    //    maxView: 1,
    //    forceParse: 0
    //});
    //$("#timeend").datetimepicker({
    //    //language: 'en',
    //    weekStart: 1,
    //    todayBtn: 0,
    //    autoclose: 1,
    //    todayHighlight: 0,
    //    startView: 1,
    //    minView: 0,
    //    maxView: 1,
    //    forceParse: 0
    //});
    //$('.form_date').datetimepicker({
    //    //language: 'en',
    //    weekStart: 1,
    //    todayBtn: 1,
    //    autoclose: 1,
    //    todayHighlight: 1,
    //    startView: 2,
    //    minView: 2,
    //    forceParse: 0
    //});

});