function CheckAuthentication() {
    var isAuthenticated = $("#isAuthenticated").val();
    if (isAuthenticated == 0 || isAuthenticated == false || isAuthenticated == "false") {
        return false;
    }
    return true;
}

function doLoginAction(data, api_url, callback_func)
{
    $.ajax({
        type: "POST",
        url: api_url,
        content: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        success: function (d) {
            if (d.success) {
                if (callback_func)
                    callback_func("login-done");
                else {
                    var return_url_token = "returnurl=";
                    var return_url = null;
                    var url = location.href;
                    
                    if (url.indexOf(return_url_token) != -1)
                    {
                        return_url = url.substring(url.indexOf(return_url_token) + return_url_token.length, url.length);
                        return_url = return_url.replace("%2F", "");
                    }
                    if (return_url)
                    {
                        location.href = return_url;
                    }
                    else
                    {
                        window.location.reload();
                    }
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

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//Login Facebook

window.fbAsyncInit = function () {
    // Executed when the SDK is loaded
    FB.init({
        appId: $('#fbAppId').val(),
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    $("#fb-click").click(function (e) {
        e.preventDefault();
        loginFacebook();
    });
};

// Load the SDK asynchronously
(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));

function loginFacebook(callback_func) {
    showWaitingDialog("Đang kết nối Facebook, vui lòng chờ trong giây lát...");
    FB.login(function (response) {
        if (response.authResponse) {
            var accessToken = response.authResponse.accessToken;
            if (accessToken) {

                var data = {
                    accessToken: accessToken,
                    return_url: window.location.href
                };

                doLoginAction(data, "/Account/LoginFacebook", callback_func);
            }
        } else {
            alert('User cancelled login or did not fully authorize.');
        }
    }, { scope: 'public_profile,email' });

}

$("body").delegate(".modal", "show.bs.modal", function (event) {
    $(this).attr("modal-open", $("body").hasClass("modal-open"));
});
$("body").delegate(".modal", "hidden.bs.modal", function (event) {
    if ($(this).attr("modal-open") == "true")
        $("body").addClass("modal-open");
});

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
///Show Header Login Form

$("body").on("click", ".btn-navigate-login", function (e) {
    e.preventDefault();

    if ($(this).hasClass("active")) {
        $(".login-container").slideUp();
    }
    else {
        $(".login-container").slideDown();
    }

    $(this).toggleClass("active");
});

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
///Authorized Actions

var layout_login_dialog_isShow = false;
function showLoginDialog(callback_func) {
    if (layout_login_dialog_isShow)
        return;
    layout_login_dialog_isShow = true;
    BootstrapDialog.show({
        title: '',
        cssClass: 'login-dialog',
        closeByBackdrop: true,
        message: function (dialog) {
            var $content = $(
                        '<i class="icon_set_1_icon-77 article-popup-close" data-dismiss="modal" aria-hidden="true"></i>' +
                        '<div id="login">' +
                    		'<div class="text-center">' +'<img src="/images/logo_sticky.png" alt="" data-retina="true" class="logo">' +'</div>' +
                            '<hr>' +
                            '<form id="frmLoginDialog">' +
                                '<div class="row">' +
                                    '<div class="col-xs-12 login_social">' +
                                        '<a id="fb-click-popup" href="#" class="btn btn-primary btn-block"><i class="icon-facebook"></i>Facebook</a>' +
                                    '</div>' +
                                '</div>' +
                                '<div class="login-or">' +'<hr class="hr-or">' +'<span class="span-or">hoặc</span>' +'</div>' +
                                '<div class="form-group">' +
                                    '<label>Email</label>' +
                                    '<input id="username-container" type="text" class=" form-control " placeholder="Email">' +
                                '</div>' +
                                '<div class="form-group">' +
                                    '<label>Mật khẩu</label>' +
                                    '<input id="password-container" type="password" class=" form-control" placeholder="Mật khẩu">' +
                                '</div>' +
                                '<p class="small">' +
                                    '<a href="/khoi-phuc-mat-khau">Quên mật khẩu?</a>' +
                                '</p>' +
                                '<input type="submit" class="btn_full" value="Đăng nhập" />' +
                                '<a href="/dang-ky-nguoi-dung" class="btn_full_outline">Đăng ký</a>' +
                            '</form>' +
                        '</div>');

            $content.find('#fb-click-popup').click(function (event) {
                event.preventDefault();
                loginFacebook(callback_func);
                dialog.setClosable(true);
            });
            
            $content.find('#frmLoginDialog').submit(function (event) {
                event.preventDefault();

                var data = {
                    Username: $('#username-container').val(),
                    Password: $('#password-container').val(),
                };

                doLoginAction(data, "/Account/LoginJs", callback_func);

                //login(dialog, callback_func);
                //dialog.setClosable(true);
            });
            return $content;
        },
        onshow: function (dialog) {
            //$(".register-form").css("opacity", "0");
            $(".register-form").addClass("opacity-invisible");
        },
        onshown: function (dialog) {
            layout_login_dialog_isShow = false;
        },
        onhide: function (dialog) {
            layout_login_dialog_isShow = false;
        },
        onhidden: function (dialog) {
            layout_login_dialog_isShow = false;
            //$(".register-form").css("opacity", "1");
            $(".register-form").removeClass("opacity-invisible");
        }
    }).setType(BootstrapDialog.TYPE_PRIMARY);
}

function AuthorizedActionWithLoginBox(data, api_url, callback_func) {
    callback_func = typeof callback_func !== 'undefined' ? callback_func : "";
    if (data && api_url) {
        $.ajax({
            type: "POST",
            url: api_url,
            content: "application/json;charset=utf-8",
            dataType: "json",
            data: data,
            success: function (d) {//authenticated
                if (d.success) {//success
                    if (callback_func) {
                        callback_func(d);
                    }
                }
                else {//error
                    if (d.message)
                        alert(d.message);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status == 403) {//not authenticated
                    showLoginDialog(callback_func);
                    return;
                }
                // TODO: Show error
                console.log(xhr.responseText);
                ProcessAjaxError(xhr);
            }
        });
    }
}

function AuthorizedAction(data, api_url, callback_func) {
    callback_func = typeof callback_func !== 'undefined' ? callback_func : "";
    if (data && api_url) {
        $.ajax({
            type: "POST",
            url: api_url,
            content: "application/json;charset=utf-8",
            dataType: "json",
            data: data,
            success: function (d) {//authenticated
                if (callback_func) {
                    callback_func(true, d);//đã login
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status == 403) {//not authenticated
                    if (callback_func) {
                        callback_func(false);//không login
                    }
                    return;
                }
                // TODO: Show error
                console.log(xhr.responseText);
                ProcessAjaxError(xhr);
            }
        });
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//room cookies header

function setSaveBoxDone(btn)
{
    btn.css("color", "#777");
    btn.css("border-color", "#ccc");
    btn.attr("disabled", "disabled");
    btn.html('<i class="fa fa-heart"></i>&nbsp;Đã lưu');
}

function unsetSaveBoxDone(btn) {
    btn.attr("disabled", "");
    btn.html('<i class="fa fa-heart-o"></i>&nbsp;Lưu lại');
}

function showCookiesRoom() {
    var ids = getRoomListFromCookies();//getRoomIdListFromCookies
    if (ids && ids.length > 0) {
        $(".saveRoomList .count").html(ids.length);
        $(".saveRoomList").show();

        //console.log(ids);
    }
}

showCookiesRoom();

//Trang: Room/Index
//click button save to wishlist trong Room/Index
//save-box: button save ở phần Contact Box
$('body').on('click', '.save-box', function (e) {
    e.preventDefault();
    var $this = $(this);
    var id = $(this).attr('data-id');
    var data = {
        id: id
    };

    AuthorizedAction(data, '/Global/SaveRoom', function (isAuthorized) {
        if (!isAuthorized)
        {
            //save to cookies
            addRoomToCookies(id);
            showCookiesRoom();
        }

        //chuyển button sang đã lưu
        setSaveBoxDone($this);
    });
})

//Trang: Header
//click button Yêu thích ở header để vào trang User/Saves
//saveRoomList: button Yêu thích ở trên header chính
$('body').on('click', '.saveRoomList', function (e) {
    e.preventDefault();
    var $this = $(this);
    var ids = getRoomIdListFromCookies();
    if (!ids)
        ids = [];
    var data = {
        ids: ids.join()
    };

    AuthorizedActionWithLoginBox(data, "/Global/SaveRooms", function (d) {
        if (d == "login-done")
        {
            ActionData(data, "/Global/SaveRooms");
            //deleteRoomCookies();
        }
        else if (d.success) {
            //go to user profile
            window.location = d.redirect;//"/User/Saves";
        }
    });
})

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//room saved check in list (search, home, wishlist...)

function setSaveIconDone(icon) {
    //icon.css("color", "#e04f67");
    icon.parent().attr("class", "wishlist_close");
    icon.find(".tooltip-back").html("Xóa khỏi yêu thích");
    icon.find(".save-text").html("-");
}

function unsetSaveIconDone(icon) {
    //icon.css("color", "#e04f67");
    icon.parent().attr("class", "wishlist");
    icon.find(".tooltip-back").html("Lưu vào yêu thích");
    icon.find(".save-text").html("+");
}

//Trang: All
//check room đã save vào wishlist chưa để đổi trạng thái
//button-save-wishlist: là button hiện trên RoomGrid hay RoomList
function checkRoomsIsSaved()
{
    if ($(".button-save-wishlist").length > 0) {
        if ($("#IsAuthenticated").val() == "true") {
            AuthorizedAction({}, '/Global/GetWishlist', function (isAuthorized, data) {
                var ids = data;//array of ints

                $(".button-save-wishlist").each(function () {
                    var $this = $(this);
                    var id = $(this).data("id");

                    if ($.inArray(id, ids) != -1) {
                        setSaveIconDone($this);
                    }
                });
            });
        }
        else
        {
            var ids = getRoomIdListFromCookies();

            $(".button-save-wishlist").each(function () {
                var $this = $(this);
                var id = $(this).data("id");

                if ($.inArray(id, ids) != -1) {
                    setSaveIconDone($this);
                }
            });
        }
    }
}

checkRoomsIsSaved();

//Trang: All
//click và save vào wishlist (nếu login) hoặc save vào cookies (nếu chưa login)
//button-save-wishlist: là button hiện trên RoomGrid hay RoomList
$("body").on("click", ".button-save-wishlist", function (e) {
    e.preventDefault();

    var $this = $(this);
    var id = $this.data("id");
    var isAddOrRemove = $this.parent().hasClass("wishlist");

    if (!isAddOrRemove)
    {
        if (!confirm("Bạn có chắc xóa khỏi danh sách yêu thích?"))
            return;
    }

    var data = {
        id: id,
        isAddOrRemove: isAddOrRemove
    };
    AuthorizedAction(data, '/Global/SaveRoom', function (isAuthorized) {
        if (isAddOrRemove)
        {
            if (!isAuthorized) {
                //save to cookies
                addRoomToCookies(id);
                showCookiesRoom();
            }
            setSaveIconDone($this);
        }
        else
        {
            if (!isAuthorized) {
                //delete from cookies
                deleteRoomFromCookies(id);
                showCookiesRoom();
            }
            unsetSaveIconDone($this);
        }
    });
});