//Global Variables
var NAME_MAXLENGTH = 128;
var DESCRIPTION_MAXLENGTH = 4000;
var REVIEW_MAXLENGTH = 3000;
var DEFAULT_ROOM_IMAGE = "~/Images/image1.png";
var SEARCH_LINK = "tim-phong-tro-nha-tro";

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
///General Functions

String.prototype.removeMVCPath = function (n) {
    var str = this;
    if (str.indexOf("~/") == 0) {
        str = str.substr(1);
    }

    return str;
};

String.prototype.formatMoney = function (c, d, t) {
    return this;
};

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

function isScrolledIntoView(elem) {
    var docViewTop = $(window).scrollTop();
    var docViewBottom = docViewTop + $(window).height();

    var elemTop = $(elem).offset().top;
    var elemBottom = elemTop + $(elem).height();

    return ((elemBottom >= docViewTop) && (elemTop <= docViewBottom)
      && (elemBottom <= docViewBottom) && (elemTop >= docViewTop));
}

// Check định dạng Email
function xfomatEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\ ".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

//Quy đổi tiền
function GetSizePriceName(min, max, unit) {
    var name = "";

    if (min <= max) {
        if (min <= 0) {
            name = "Dưới " + max + " " + unit;
        }
        else {
            name = "Từ " + min + " đến " + max + " " + unit;
        }
    }
    else {
        name = "Trên " + min + " " + unit;
    }

    return name;
}

String.prototype.capitalizeFirstLetter = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

String.prototype.hasHtml = function () {
    return this.indexOf("<") >= 0;
}