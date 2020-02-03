/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Upload Image

var file_images = [];
var file_images_id = 0;

function removeImageFromList(image_id) {
    var index = -1;
    for (var i = 0; i < file_images.length; i++) {
        if (file_images[i].id == image_id)
            index = i;
    }

    if (index != -1) {
        file_images.splice(index, 1);

        return true;
    }

    return false;
}

$("body").on("change", ".form-upload-images input[type='file']", function (event) {
    //0. get model name
    var name = $(this).attr("name");
    var multiple = $(this).attr("multiple");

    //1. load file and generate html code
    var fileList = event.target.files;
    var anyWindow = window.URL || window.webkitURL;
    var append_text = "";

    for (var i = 0; i < fileList.length; i++) {
        //get a blob to play with
        var objectUrl = anyWindow.createObjectURL(fileList[i]);

        append_text += '<div class="col-xs-3 has-bottom">'
                            + '<div class="image-ratio">'
                                + '<img class="modal-image img-responsive" src="' + objectUrl + '" />'
                                + '<button class="overlay-btn delete-photo-btn" data-image=' + file_images_id + '>'
                                    + '<i class="fa fa-trash-o"></i>'
                                + '</button>'
                            + '</div>'
                      + '</div>';

        // get rid of the blob
        window.URL.revokeObjectURL(fileList[i]);

        if (!multiple) {
            var image_id = $(this).attr("data-image");
            removeImageFromList(image_id);

            $(this).attr("data-image", file_images_id);
        }

        file_images.push({
            id: file_images_id,
            value: fileList[i],
            name: name
        });

        file_images_id++;
    }

    //2. add html codes to parent div
    if (multiple) {
        $(this).parents(".form-upload-images").find('.form-photos').append(append_text);
    }
    else {
        $(this).parents(".form-upload-images").find('.form-photos').html(append_text);
    }
    $(this).val("");

    console.log(file_images);

});

$("body").on("click", ".form-upload-images .form-photos .delete-photo-btn", function (event) {
    event.preventDefault();
    if (confirm("Bạn có muốn xóa hình này không?")) {
        var image_id = $(this).attr("data-image");//hình mới

        if (image_id) {
            if (removeImageFromList(image_id)) {
                $(this).parent().parent().remove();//remove col-xs-3
            }
            else {
                alert("Có lỗi xảy ra");
            }
        }
        else {
            image_id = $(this).attr("data-id");//hình cũ
            if (image_id) {
                $(this).parent().parent().remove();//remove col-xs-3
            }
        }
    }
});

$("body").on("click", ".delete-old-photo-btn", function (event) {
    event.preventDefault();
    if (confirm("Bạn có muốn xóa hình này không?")) {
        var $this = $(this);
        var id = $this.data("id");
        $.ajax({
            url: ROOTPATH + "/Admin/DeleteImage",
            type: 'POST',
            content: "application/json;charset=utf-8",
            dataType: 'json',
            data: {
                id: id
            },
            success: function (data) {
                $this.parent().parent().remove();//remove col-xs-3
            },
            error: function (xhr, textStatus, errorThrown) {
                // TODO: Show error
                console.log(xhr.responseText);
                ProcessAjaxError(xhr);
            }
        });
    }
});

$("body").on("click", ".modal-image", function (event) {
    var img = $(this);

    BootstrapDialog.show({
        title: 'Hình ảnh',
        cssClass: 'image-preview-dialog',
        closeByBackdrop: true,
        closable: true,
        message: function (dialog) {
            var $content = $(
                '<img src="' + img.attr("src") + '" style="width: 100%;" />');

            $('body').on('click', '.image-preview-dialog .modal-backdrop', function () {
                dialog.close();
            })

            return $content;
        },
    }).setType(BootstrapDialog.TYPE_INFO);
});

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Form List

function SetFormListItemName() {
    $(".form-list").each(function () {
        var data_model = $(this).attr("data-model");
        $(this).find(".form-list-item").each(function (index) {
            //set index
            $(this).find(".form-item-index").html(index + 1);
            //set name
            $(this).find(".form-control").each(function () {
                var data_name = $(this).attr("data-name");
                if (!data_name) {
                    data_name = $(this).attr("name");
                    $(this).attr("data-name", data_name);
                }
                $(this).attr("name", data_model + "[" + index + "]." + data_name);
            });
        });
    });

}

SetFormListItemName();

$(".btn-add-item").on("click", function () {
    var form_list = $(this).parents(".form-list");
    var first_item = form_list.find(".form-list-item").last();
    if (first_item.length > 0) {
        //clear item input contents
        var html = first_item[0].outerHTML;
        $(html).find(".form-control").val("");
        //$(html).find(".form-upload-images .form-photos").html("");
        html = $(html).find(".form-upload-images .form-photos").html("").prevObject[0].outerHTML;
        //add to parent
        form_list.find(".form-list-item").last().after($(html));
        SetFormListItemName();

        //total items
        var form_list_container = form_list.parent();
        var total = form_list_container.find(".form-list .form-list-item").length;
        form_list.find(".form-list-count").html(total);
    }
});

$(".btn-delete-item").on("click", function () {
    var form_list = $(this).parents(".form-list");
    $(this).parents(".form-list-item").remove();
    SetFormListItemName();

    //total items
    var form_list_container = form_list.parent();
    var total = form_list_container.find(".form-list .form-list-item").length;
    form_list.find(".form-list-count").html(total);
});

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Post to server
function shakeForm(form)
{
    $(form).addClass("animated shake");
    $(form).one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass("animated shake");
    });
}

function checkFormDataValidation(form) {
    //var validates = "Contact_Name,Contact_Phone,Contact_Email:email";
    $(form).find(".form-group").removeClass("has-error");
    var validates = $(form).attr("validates");
    if (validates) {
        var arr = validates.split(",");
        var check = true;
        for (i = 0; i < arr.length; i++) {
            var name = arr[i];
            var type = "text";
            var sub_arr = name.split(":");
            if (sub_arr.length == 2) {
                name = sub_arr[0];
                type = sub_arr[1];
            }

            var value = $(form).find("input[name='" + name + "']").val();
            switch (type) {
                case "text":
                    {
                        check = value && value != "";
                        break;
                    }
                case "email":
                    {
                        check = value && value != "" && xfomatEmail(value);
                        break;
                    }
            }

            if (!check) {
                console.log("invalidate!");

                $(form).find("input[name='" + name + "']").parents(".form-group").addClass("has-error");

                //shake form!
                shakeForm(form);

                return false;
            }
        }
    }

    return true;
}

$("body").on("click", "form #btnAdd", function (event) {
    event.preventDefault();

    uploadDataToServer(this.form);
})

function uploadDataToServer(form) {
    //check validation
    if (typeof checkDataValidation == 'function') {
        if (!checkDataValidation(form))
            return;
    }
    if (!checkFormDataValidation(form))
        return false;

    //var isUploadImage = false;
    //isUploadImage = $(form).find(".form-upload-images").length > 0;
    
    //preback_func
    var preback_func = null;
    if ($(form).attr("preback")) {
        preback_func = window[$(form).attr("preback")];
    }

    if (preback_func)
        preback_func(form);

    //callback_func
    var callback_func = null;
    if (typeof postFormCompleted == 'function')
        callback_func = postFormCompleted;
    else {
        if ($(form).attr("callback")) {
            callback_func = window[$(form).attr("callback")];
        }
    }

    //post to server
    if ($(form).attr("adding") == "true") {
        return;
    }
    $(form).attr("adding", "true");

    //var postAddLink = $("#postAddLink").val();
    var postAddLink = $(form).attr("action");
    SendRequestToServer(form, postAddLink, callback_func);

    //if (isUploadImage) {
    //    //Nếu có tồn tại Image thì post Image cùng với Form Data đến postAddLink và return (không ActionForm nữa)
    //    SendRequestToServer(form, postAddLink, callback_func);
    //}
    //else {
    //    ActionForm($(form).serialize(), postAddLink, null, callback_func);
    //}
}

function SendRequestToServer(form, url, callback_func) {
    var $upload_progress_container = null;
    if (!$(form).attr("withoutProgress")) {
        var container = $(form).attr("progress-bar-container");
        if (container) {
            $upload_progress_container = $(container);
        }
        else {
            $upload_progress_container = $(form);
        }
    }

    function uploadProgress(evt) {
        if (evt.lengthComputable) {
            var progress = Math.round(evt.loaded * 100 / evt.total);
            if ($upload_progress_container) {
                $upload_progress_container.find(".form-progress-bar #uploadProgress").css("width", progress + '%');
            }
        }
    }

    function uploadComplete(evt) {
        $(form).attr("adding", "false");
        if ($upload_progress_container) {
            $upload_progress_container.find(".form-progress-bar #uploadProgress").css("width", '100%');
            $upload_progress_container.find(".form-progress-bar").remove();
        }

        console.log("Upload completed!");
        var srcElement = evt.target || evt.srcElement;

        var d = jQuery.parseJSON(srcElement.response);
        if (d.success == false) {//failed
            if (!$(form).attr("withoutAlert")) {
                if (d.message) {
                    alert(d.message);
                }
                else {
                    alert("Không gửi được yêu cầu!");
                }
            }
        }
        else {//success
            if (!$(form).attr("withoutAlert")) {
                if (d.message) {
                    alert(d.message);
                }
            }
        }
        if (d.reload) {
            window.location.reload();
        }
        else if (d.redirect) {
            location.href = d.redirect;
        }
        else {
            if (callback_func)
                callback_func(form, d);
        }
    }

    function uploadFailed(evt) {
        alert("Có lỗi xảy ra! Vui lòng thực hiện lại");

        console.log("There was an error in processing.");
    }

    var fd = new FormData();
    var text_fields = $(form).serializeArray();
    for (var i in text_fields) {
        fd.append(text_fields[i].name, text_fields[i].value);
    }

    //upload images
    for (var i in file_images) {
        fd.append(file_images[i].name, file_images[i].value);
    }
    var multiple = $(form).find(".form-upload-images");
    for (var j = 0; j < multiple.length; j++) {
        if (multiple.eq(j).find("input").eq(0).attr("multiple") === "multiple") {
            var imgcurrent = multiple.eq(j).find('.form-photos').eq(0).find('.imgCurrent');
            for (var i = 0; i < imgcurrent.length; i++) {
                var name = imgcurrent.eq(i).attr("data-name");
                console.log(name);
                fd.append(name, imgcurrent.eq(i).attr('data-id'));
            }
        }
    }

    //CKEDITOR textarea
    var textarea = $(form).find("textarea");
    for (var i = 0; i < textarea.length; i++) {
        var id = textarea.eq(i).attr("id");
        var name = textarea.eq(i).attr("data-name");
        var isEditor = textarea.eq(i).attr("data-editor");
        if (isEditor == "true") {
            var value = CKEDITOR.instances[id].getData();
            fd.append(name, value);
        }
    }

    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", uploadProgress, false);
    xhr.addEventListener("load", uploadComplete, false);
    xhr.addEventListener("error", uploadFailed, false);

    xhr.open("POST", url);
    xhr.send(fd);

    //add form progress bar
    if (!$(form).attr("withoutProgress")) {
        var upload_progress_content = "<div class='form-progress-bar'>"
                       + "<div class='form-group upload-progress-container'>"
                            + "<div style='width: 100%; margin-top: 20px;'>"
                                + "<div class='progress progress-bar-success active'>"
                                    + "<div id='uploadProgress' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>"
                                    + "</div>"
                                + "</div>"
                            + "</div>"
                        + "</div>"
                    + "</div>";

        var progress_bar = $upload_progress_container.find(".form-progress-bar");
        if (progress_bar.length > 0)
            progress_bar.remove();
        $upload_progress_container.append(upload_progress_content);
    }
}