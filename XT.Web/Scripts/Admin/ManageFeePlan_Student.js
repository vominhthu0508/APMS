$("body").on("change", "input[name='Discount_Amount']", function (e) {
    var discount = $(this).val();
    var nominal = $(this).parents(".modal").find("input[name='Nominal_Course_Fee']").val();

    var actual = nominal - discount;
    $(this).parents(".modal").find("input[name='Actual_Course_Fee']").val(actual);
});
