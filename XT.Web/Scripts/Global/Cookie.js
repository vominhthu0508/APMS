function _setCookie(name, value, days) {
    //var d = new Date();
    //d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    //var expires = "expires=" + d.toUTCString();
    //document.cookie = cname + "=" + cvalue + "; " + expires + "; path=/";
    var expires;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
    //document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/";
}

function _getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function _deleteCookie(name) {
    _setCookie(name, "", -1);
}

function deleteAllCookie(name) {
    var data = _getCookie(name);
    var count = 0;
    while (data) {
        _deleteCookie(name);
        data = _getCookie(name);
        count++;
        if (count == 100)
            break;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//room cookies for wishlist
var HOMELY_COOKIES_ID_ROOMS = "HOMELY_COOKIES_ID_ROOMS";
var HOMELY_COOKIES_ID_COMPARE = "HOMELY_COOKIES_ID_COMPARE";
var HOMELY_ROOM_EXPIRATION_DAYS = 365;
var HOMELY_COMPARE_EXPIRATION_DAYS = 1;

var RoomCookie = function (cookie_id, cookie_expired_days) {
    this.cookie_id = cookie_id;
    this.cookie_expired_days = cookie_expired_days;
};

RoomCookie.prototype = {
    deleteRoomCookies: function () {
        _deleteCookie(this.cookie_id);
    },
    deleteCookies: function () {
        _deleteCookie(this.cookie_id);
    },
    getItemInCookies: function(id){
        var ids = this.getRoomListFromCookies();
        if (ids) {
            for (var i = 0; i < ids.length; i++) {
                if (ids[i].id == id)
                    return i;
            }
        }

        return - 1;
    },
    existRoomInCookies: function (id) {
        return this.getItemInCookies(id) > -1;
    },
    resetRoomCookies: function (new_data) {
        new_data = JSON.stringify(new_data);

        _deleteCookie(this.cookie_id);
        _setCookie(this.cookie_id, new_data, this.cookie_expired_days);
    },
    //public functions
    getRoomListFromCookies: function () {
        var data = _getCookie(this.cookie_id);
        if (data) {
            //var ids = data.split(",").map(Number);
            //return ids;
            //$.parseJSON(data);
            try {
                var _data = $.parseJSON(data);
                if (!Array.isArray(_data))
                {
                    return [_data];
                }
                return _data;
            }
            catch (e) {
                //_deleteCookie(this.cookie_id);
                console.log(e);
            }
        }

        return [];
    },
    addRoomToCookies: function (id, image) {
        if (id) {
            var new_data = { id: id, image: image };
            if (!this.existRoomInCookies(id)) {
                //var data = { id: id, image: image };
                var ids = this.getRoomListFromCookies();
                ids.push(new_data);
                new_data = ids;
                //new_data = ids.join(",");
                //new_data = JSON.stringify(ids);
            }
            else
            {
                new_data = [new_data];
            }

            this.resetRoomCookies(new_data);
        }
    },
    deleteRoomFromCookies: function (id) {
        if (id) {
            var ids = this.getRoomListFromCookies();
            var index = this.getItemInCookies(id);
            if (index > -1) {
                ids.splice(index, 1);
            }

            //var new_data = JSON.stringify(ids);

            this.resetRoomCookies(ids);
        }
    }
};

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//room cookies

var roomCookie = new RoomCookie(HOMELY_COOKIES_ID_ROOMS, HOMELY_ROOM_EXPIRATION_DAYS);

function existRoomInCookies(id) {
    return roomCookie.existRoomInCookies(id);
}

function addRoomToCookies(id) {
    roomCookie.addRoomToCookies(id);
}

function deleteRoomFromCookies(id) {
    roomCookie.deleteRoomFromCookies(id);
}

function getRoomListFromCookies() {
    return roomCookie.getRoomListFromCookies();
}

function getRoomIdListFromCookies() {
    return roomCookie.getRoomListFromCookies().map(function (val, i) {
        if (val.id)
            return val.id;
        return val;
    });
}

function deleteRoomCookies() {
    roomCookie.deleteRoomCookies();
}

//function resetRoomCookies(new_data)
//{
//    _deleteCookie(HOMELY_COOKIES_ID_ROOMS);
//    _setCookie(HOMELY_COOKIES_ID_ROOMS, new_data, HOMELY_ROOM_EXPIRATION_DAYS);
//}

//function addRoomToCookies(id) {
//    //console.log("add to cookies, id = " + id);
//    if (id) {
//        var new_data = id;
//        var ids = getRoomListFromCookies();
//        if (ids && !existRoomInCookies(id)) {
//            ids.push(id);
//            new_data = ids.join(",");
//        }

//        resetRoomCookies(new_data);
//    }
//}

//function deleteRoomFromCookies(id) {
//    //console.log("delete from cookies, id = " + id);
//    if (id) {
//        var ids = getRoomListFromCookies();
//        var index = ids.indexOf(id);
//        if (index > -1) {
//            ids.splice(index, 1);
//        }

//        var new_data = ids.join(",");

//        resetRoomCookies(new_data);
//    }
//}

//function getRoomListFromCookies() {
//    var data = _getCookie(HOMELY_COOKIES_ID_ROOMS);
//    if (data) {
//        var ids = data.split(",").map(Number);
//        return ids;
//    }

//    return null;
//}

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
//room cookies for compare

var roomCookieCompare = new RoomCookie(HOMELY_COOKIES_ID_COMPARE, HOMELY_COMPARE_EXPIRATION_DAYS);
var MAX_COMPARE_LIST = 4;

function addRoomToCookiesCompare(id, image) {
    var list = getRoomListFromCookiesCompare();
    if (list && list.length == MAX_COMPARE_LIST)
        return;

    roomCookieCompare.addRoomToCookies(id, image);
}

function deleteRoomFromCookiesCompare(id) {
    roomCookieCompare.deleteRoomFromCookies(id);
}

function getRoomListFromCookiesCompare() {
    return roomCookieCompare.getRoomListFromCookies();
}

function deleteAllCookiesCompare() {
    roomCookieCompare.deleteCookies();
}
//_deleteCookie(HOMELY_COOKIES_ID_ROOMS);//code dùng để clear cookies khi debug