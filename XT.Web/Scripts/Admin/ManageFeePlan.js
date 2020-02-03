function updateFeePlanModal($modal)
{
    var $FeePlan_Type = $modal.find("select[name='FeePlan_Type']");

    var name_feeplan = $FeePlan_Type.find("option:selected").html();
    var name_upgrade = "";
    var name_course = $modal.find("select[name='Courses'] option:selected").html();
    var fp_type = 1;
    var fp_price = 0;
    var fp_count = 1;
    var fp_months = 6;
    var fp_sem_count = 1;
    var MONTHLY = 6;//6 monthes each

    var type = $FeePlan_Type.val();
    if (type == 3)//upgrade
    {
        $modal.find(".type-upgrade").show();
        $modal.find(".type-nonupgrade").hide();

        name_upgrade = $modal.find("select[name='UpgradeType'] option:selected").html();
        name_course = $modal.find("select[name='FromCourses'] option:selected").html()
            + " to " + $modal.find("select[name='ToCourses'] option:selected").html();

        var sem_count_from = $modal.find("select[name='FromCourses'] option:selected").attr("data-semcount");
        var sem_count_to = $modal.find("select[name='ToCourses'] option:selected").attr("data-semcount");
        fp_sem_count = sem_count_to - sem_count_from;
        fp_type = $modal.find("select[name='UpgradeType']").val();
    }
    else {
        $modal.find(".type-upgrade").hide();
        $modal.find(".type-nonupgrade").show();
        fp_sem_count = $modal.find("select[name='Courses'] option:selected").attr("data-semcount");
        fp_type = type;
    }
    //if (fp_type == 2)//lumpsum
    //{
    //    fp_count = 1;
    //}
    if (fp_type == 1)//install
    {
        fp_count = fp_sem_count;
    }
    fp_months = MONTHLY * fp_sem_count;

    var feeplan_name = name_feeplan + " " + name_upgrade + " " + name_course;

    $modal.find("input[name='FeePlan_Name']").val(feeplan_name);
    $modal.find("input[name='FeePlan_Count']").val(fp_count);
    $modal.find("input[name='FeePlan_Months']").val(fp_months);
}

function updateFeePlanType(obj)
{
    var $this = obj;
    var $modal = $this.parents(".modal");

    updateFeePlanModal($modal);
}

$("body").on("change", ".modal-body select", function (e) {
    updateFeePlanType($(this));
});

$("body").on("shown.bs.modal", "#add-modal", function (e) {
    updateFeePlanModal($(this));
});
