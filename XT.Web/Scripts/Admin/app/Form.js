
///////////////////////////////////////////////////
var postAddLink = $("#postAddLink").val();
var postEditLink = $("#postEditLink").val();
var postDeleteLink = $("#postDeleteLink").val();
var isUploadImage = $("#postIsUploadImage").val();

function addDataForm(frm, api)//this.form
{
    if (typeof checkDataValidation == 'function') {
        if (checkDataValidation(frm) == false)
            return;
    }
    //if (typeof SendRequestToServer == 'function') {
    //    //Nếu có tồn tại Image thì post Image cùng với Form Data đến postAddLink và return (không ActionForm nữa)
    //    SendRequestToServer(this.form, postAddLink);
    //}

    var form_api = $(frm).attr("action");
    if (form_api) {
        api = form_api;
    }

    var form_isUploadImage = $(frm).attr("data-image");
    if (form_isUploadImage) {
        isUploadImage = form_isUploadImage;
    }

    var frmData = processFormData(frm);
    if (isUploadImage == "true") {
        //Nếu có tồn tại Image thì post Image cùng với Form Data đến postAddLink và return (không ActionForm nữa)
        SendRequestToServer(frm, convertJSONToFormData(frmData), api);
    }
    else {
        //if ($(frm).attr("data-popup") == "false") {
        //    //ActionFormWithoutPopup($(frm).serialize(), postAddLink);
        //    ActionFormWithoutPopup(frmData, api);
        //}
        //else {
        //    var redirect_url = $(frm).attr("data-redirect");
        //    ActionForm(frmData, api, redirect_url);
        //}
        var redirect_url = $(frm).attr("data-redirect");
        ActionForm(frmData, api, redirect_url);
    }
}

function processFormData(form) {
    ////////////////////////////////////////////////////////////////////////////
    //pre-processing
    $(form).find(".textbox-price").each(function () {
        var val = $(this).val();
        val = val.replaceAll(',', '').replaceAll('.', '');
        $(this).val(val);
    });
    $(form).find(".textbox-float").each(function () {
        var val = $(this).val();
        val = val.replaceAll('.', ',');
        $(this).val(val);
    });

    ////////////////////////////////////////////////////////////////////////////
    //process data - form.serializeArray
    var fd = $(form).serializeArray();
    //images
    for (var i in file_images) {
        fd.push({
            name: file_images[i].name,
            value: file_images[i].value
        });
    }
    file_images = [];
    file_images_id = 0;
    $(form).find(".form-photos").html("");
    //CKEditor
    var textarea = $(form).find("textarea");
    for (var i = 0; i < textarea.length; i++) {
        var id = textarea.eq(i).attr("id");
        var name = textarea.eq(i).attr("data-name");
        var isEditor = textarea.eq(i).attr("data-editor");
        if (isEditor == "true") {
            var value = CKEDITOR.instances[id].getData();
            fd.push({
                name: name,
                value: value
            });
        }
    }

    return fd;
}

function convertJSONToFormData(json) {
    var fd = new FormData();
    for (var i in json) {
        fd.append(json[i].name, json[i].value);
    }

    return fd;
}

$("form.form-submit-data").submit(function (e) {
    addDataForm(this, postAddLink);
});

function HideModal() {
    $(".modal").modal("hide");
    $('body').removeClass('modal-open');
    $('body').css("padding", "0px");
    $('.modal-backdrop').remove();
}

//không cần sửa
$("#btnAdd").click(function (e) {
    e.preventDefault();
    HideModal();

    var $frm = $(this.form);
    var api = postAddLink;
    var form_api = $frm.find("#postAddLink").val();
    if (form_api) {
        api = form_api;
    }
    addDataForm(this.form, api);
})

$('body').on("click", ".btnAdd", function (e) {
    e.preventDefault();
    HideModal();

    var $frm = $(this.form);
    var api = postAddLink;
    var form_api = $frm.find("#postAddLink").val();
    if (form_api) {
        api = form_api;
    }
    addDataForm(this.form, api);
});

$('body').on("click", ".btnEdit", function (e) {
    e.preventDefault();
    HideModal();

    var $frm = $(this.form);
    var api = postEditLink;
    var form_api = $frm.find("#postEditLink").val();
    if (form_api) {
        api = form_api;
    }
    addDataForm(this.form, api);
})

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
    }).setType(BootstrapDialog.TYPE_SUCCESS);
});

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

$("body").on("change", ".btn-upload-image input[type='file']", function (event) {
    event.preventDefault();
    //var input = this;

    var name = $(this).attr("name");
    var multiple = $(this).attr("multiple");

    //1. load file and generate html code
    var fileList = event.target.files;
    var anyWindow = window.URL || window.webkitURL;
    var append_text = "";
    file_images = [];
    file_images_id = 0;

    var col_size = multiple ? 3 : 12;//fileList.length > 1 ? 3 : 12;
    for (var i = 0; i < fileList.length; i++) {
        //get a blob to play with
        var objectUrl = anyWindow.createObjectURL(fileList[i]);

        append_text += '<div class="col-md-' + col_size + ' has-bottom">'
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

var loadingInterval;
$("body").on("change", ".btn-import input[type='file']", function (event) {
    event.preventDefault();
    var $this = $(this);

    var file = event.target.files[0];
    if (file) {
        var $form = $(this.form);
        var url = $form.attr("action");

        var fd = $form.serializeArray(); //new FormData();
        fd.push({
            name: "file",
            value: file
        });
        fd = convertJSONToFormData(fd);
        //fd.append("file", file);

        function uploadProgress(evt) {
            if (evt.lengthComputable) {
                var progress = Math.round(evt.loaded * 100 / evt.total);
                //$(".form-loading #uploadProgress").css("width", progress + '%');
            }
        }

        function uploadComplete(evt) {
            $(".form-uploading #uploadProgress").css("width", '100%');
            if (loadingInterval) {
                clearInterval(loadingInterval);
            }
            $(".form-uploading").modal("hide");
            $this.val("");

            console.log("Upload completed!");

            //var d = jQuery.parseJSON(evt.srcElement.response);
            var srcElement = evt.target || evt.srcElement;

            var d = jQuery.parseJSON(srcElement.response);
            if (d.success == false) {
                if (d.message) {
                    alert(d.message);
                }
                else {
                    alert("Không gửi được yêu cầu!");
                }
            }
            else {
                window.location.reload();
            }
        }

        function uploadFailed(evt) {
            alert("Có lỗi khi upload hình! Vui lòng thực hiện lại");
        }

        if ($(".form-uploading").length > 0) {

            $(".form-uploading").modal("show");
        }
        else {
            BootstrapDialog.show({
                title: 'Uploading',
                cssClass: 'form-uploading',
                closeByBackdrop: false,
                closable: true,
                message: function (dialog) {
                    var $content = $("<div class='upload-progress-container'>"
                            + "<div style='width: 100%; margin-top: 20px;'>"
                                + "<div class='progress progress-striped active'>"
                                    + "<div id='uploadProgress' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>"
                                    + "</div>"
                                + "</div>"
                            + "</div>"
                        + "</div>");

                    //$('body').on('click', '.image-preview-dialog .modal-backdrop', function () {
                    //    dialog.close();
                    //})

                    return $content;
                },
            }).setType(BootstrapDialog.TYPE_INFO);
        }

        var progress = 0;
        loadingInterval = setInterval(function () {
            progress++;
            $(".form-uploading #uploadProgress").css("width", progress + '%');
        }, 1000);

        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);

        xhr.open("POST", url);
        xhr.send(fd);


    }
});

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//SendRequestToServer

function SendRequestToServer(form, fd, url) {
    function uploadProgress(evt) {
        if (evt.lengthComputable) {
            var progress = Math.round(evt.loaded * 100 / evt.total);
            $(form).find(".form-horizontal #uploadProgress").css("width", progress + '%');
        }
    }

    function uploadComplete(evt) {
        $(form).find(".form-horizontal #uploadProgress").css("width", '100%');

        console.log("Upload photos completed!");

        //var d = jQuery.parseJSON(evt.srcElement.response);
        var srcElement = evt.target || evt.srcElement;

        var d = jQuery.parseJSON(srcElement.response);

        if (d.success) {
            if (typeof ReloadTableDataAfterSubmit == 'function') {
                ReloadTableDataAfterSubmit();
            }
            else {
                if (d.redirect) {
                    window.location = d.redirect;
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
    }

    function uploadFailed(evt) {
        alert("Có lỗi khi upload hình! Vui lòng thực hiện lại");

        console.log("There was an error attempting to upload the file.");
    }

    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", uploadProgress, false);
    xhr.addEventListener("load", uploadComplete, false);
    xhr.addEventListener("error", uploadFailed, false);

    xhr.open("POST", url);
    xhr.send(fd);

    var upload_progress_container = $(form).find(".form-horizontal .upload-progress-container");
    if (upload_progress_container.length > 0)
        upload_progress_container.remove();
    $(form).find(".form-horizontal").append("<div class='form-group upload-progress-container'>"
                        + "<div style='width: 100%; margin-top: 20px;'>"
                            + "<div class='progress progress-striped active'>"
                                + "<div id='uploadProgress' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>"
                                + "</div>"
                            + "</div>"
                        + "</div>"
                    + "</div>");
}

