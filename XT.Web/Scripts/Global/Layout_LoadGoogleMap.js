///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
/////Load Google Map API

var isHasLoadGoogleMapAPI = false;
var isHasLoadChild = false;
window.gMapsCallback = function () {
    console.log("gMapsLoaded");
    //$(window).trigger('gMapsLoaded_ForLayout');
    if (isHasLoadChild) {//nếu child đã load rồi
        $(window).trigger('gMapsLoaded');
    }
    else {//nếu chưa thì chờ con load xong mới trigger
        $(window).bind('childLoaded', function () {
            $(window).trigger('gMapsLoaded');
        });
    }
}

function loadGoogleMapAPIScript() {
    var IsLoadMap = 1;
    if ($("#IsLoadMap").length > 0)
        IsLoadMap = $("#IsLoadMap").val();
    if (IsLoadMap == "1" || IsLoadMap == 1) {
        if (!isHasLoadGoogleMapAPI) {
            isHasLoadGoogleMapAPI = true;
            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = 'http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places&' +
                'callback=gMapsCallback';
            document.body.appendChild(script);
        }
    }
}

function loadChildDone() {
    isHasLoadChild = true;
    $(window).trigger('childLoaded');
}

loadGoogleMapAPIScript();