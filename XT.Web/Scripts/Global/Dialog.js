////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//showWaitingDialog

var watingLoadingDialog;
function showWaitingDialog(message) {
    watingLoadingDialog = BootstrapDialog.show({
        title: 'Loading',
        message: message
    }).setType(BootstrapDialog.TYPE_PRIMARY);
}

function hideWaitingDialog() {
    if (watingLoadingDialog) {
        watingLoadingDialog.close();
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Action Form dùng cho các trang quản lý

function ActionData(data, api_url, redirect_url, callback_func) {
    redirect_url = typeof redirect_url !== 'undefined' ? redirect_url : "";
    callback_func = typeof callback_func !== 'undefined' ? callback_func : "";
    if (data && api_url) {
        $.ajax({
            type: "POST",
            url: api_url,
            content: "application/json;charset=utf-8",
            dataType: "json",
            data: data,
            success: function (d) {
                if (d.success == true) {
                    if (d.message) {
                        alert(d.message);
                    }
                    if (d.redirect)
                    {
                        window.location = d.redirect;
                        return;
                    }
                    if (redirect_url) {
                        window.location = redirect_url;
                    }
                    else {
                        if (callback_func)
                            callback_func();
                        else
                            window.location.reload();
                    }
                }
                else {
                    if (d.message) {
                        alert(d.message);
                    }
                    else {
                        alert("Không gửi được yêu cầu!");
                    }
                    if (callback_func)
                        callback_func();
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

function Action(id, api_url, redirect_url, callback_func) {
    ActionData({ id: id }, api_url, redirect_url, callback_func);
}

function ActionForm(form_data, api_url, redirect_url, callback_func) {
    ActionData(form_data, api_url, redirect_url, callback_func);
}

function ActionCallback(id, api_url, callback_func) {
    if (id && api_url) {
        $.ajax({
            type: "POST",
            url: api_url,
            content: "application/json;charset=utf-8",
            dataType: "json",
            data: {
                id: id
            },
            success: function (d) {
                if (d.success == true) {
                    if (typeof callback_func == 'function') {
                        callback_func();
                    }
                }
                else {
                    if (d.message) {
                        alert(d.message);
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

function AskAndActionData(action, data, api_url, redirect_url) {
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
                    ActionData(data, api_url, redirect_url);
                    dialog.close();
                }
            }]
    }).setType(BootstrapDialog.TYPE_DANGER);
}

function AskAndActionCallback(action, id, api_url, callback_func) {
    BootstrapDialog.show({
        cssClass: 'delete-car-dialog',
        title: 'Cảnh báo',
        message: 'Bạn có chắc là muốn ' + action + ' thông tin xe này không?',
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
                    ActionCallback(id, api_url, callback_func);
                    dialog.close();
                }
            }],
    }).setType(BootstrapDialog.TYPE_DANGER);
}