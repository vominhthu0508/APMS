
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
///sorting noajax

$("body").on("click", ".sorting.noajax", function (e) {
    var asc = true;

    if ($(this).hasClass("asc")) {
        asc = false;
        $(".Search_Result .table thead th").removeClass("asc").removeClass("desc");
        $(this).addClass("desc");
    }
    else {
        asc = true;
        $(".Search_Result .table thead th").removeClass("asc").removeClass("desc");
        $(this).addClass("asc");
    }

    var sorting_target = "." + $(this).attr("data-sorting");//get class of targeted sorting items

    //change color
    //$(".submenu-list ul li").removeClass("active");
    //$(compareFilter).parents("li").addClass("active");
    //$(".compare-filter").removeClass("active");
    //$(this).addClass("active");

    //get sorting values
    var arr = [];
    $(sorting_target).each(function () {
        var id = $(this).parents("tr").attr("data-id");
        var val = $(this).html();
        if ($(this).hasClass("number-float")) {
            val = parseFloat(val.replace(",", "."), 10);
        }
        arr.push({ id: id, val: val });
    });

    //sort!
    arr.sort(function (x, y) {
        return asc ? x.val - y.val : y.val - x.val;
    });

    //get new tr html
    var new_html = "";
    for (var i = 0; i < arr.length; i++) {
        var id = arr[i].id;
        new_html += $(".Search_Result .table tbody tr[data-id='" + id + "']")[0].outerHTML;
    }
    if ($(".Search_Result .table tbody tr.tr-total").length > 0) {
        new_html += $(".Search_Result .table tbody tr.tr-total")[0].outerHTML;
    }

    $(".Search_Result .table tbody").html(new_html);
});

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
///sorting ajax

$("body").on("click", ".sorting.ajax", function (e) {
    e.preventDefault();

    var asc = true;

    if ($(this).hasClass("asc")) {
        asc = false;
        $(".Search_Result .table thead th").removeClass("asc").removeClass("desc");
        $(this).addClass("desc");//false
    }
    else {
        asc = true;
        $(".Search_Result .table thead th").removeClass("asc").removeClass("desc");
        $(this).addClass("asc");//true
    }

    var sorting_target = $(this).attr("data-sorting");
    $("#filter-form input[name='sort_target']").val(sorting_target);
    $("#filter-form input[name='sort_rank']").val(asc);

    FilterReport();
});

$('body').on("click", "#filter-container .pagination-container .pagination > li > a", function (e) {
    e.preventDefault();
    var page = $(this).html();
    $("#filter-form input[name='page']").val(page);

    FilterReport();
});