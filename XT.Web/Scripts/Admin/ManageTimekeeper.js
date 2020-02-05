function findIP(onNewIP) { //  onNewIp - your listener function for new IPs
    var myPeerConnection = window.RTCPeerConnection || window.mozRTCPeerConnection || window.webkitRTCPeerConnection; //compatibility for firefox and chrome
    var pc = new myPeerConnection({ iceServers: [] }),
        noop = function () { },
        localIPs = {},
        ipRegex = /([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/g,
        key;

    function ipIterate(ip) {
        if (!localIPs[ip]) onNewIP(ip);
        localIPs[ip] = true;
    }
    pc.createDataChannel(""); //create a bogus data channel
    pc.createOffer(function (sdp) {
        sdp.sdp.split('\n').forEach(function (line) {
            if (line.indexOf('candidate') < 0) return;
            line.match(ipRegex).forEach(ipIterate);
        });
        pc.setLocalDescription(sdp, noop, noop);
    }, noop); // create offer and set local description
    pc.onicecandidate = function (ice) { //listen for candidate events
        if (!ice || !ice.candidate || !ice.candidate.candidate || !ice.candidate.candidate.match(ipRegex)) return;
        ice.candidate.candidate.match(ipRegex).forEach(ipIterate);
    };
}

function addIP(ip) {
    $('#IP').val(ip);
    getLocation();

}

findIP(addIP);
$('[data-toggle="popover"]').popover();

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        error_alert("Trình duyệt không hỗ trợ định vị.");
    }
}

function showPosition(position) {
    $('#Location').val(position.coords.latitude + "," + position.coords.longitude);
}

function getAddressByGoogle(targetElmnt) {
    if (targetElmnt.length > 0) {
        var address = targetElmnt.attr('data-address');
        if (address) {
            $.ajax({
                url: 'https://maps.googleapis.com/maps/api/geocode/json',
                data: {
                    sensor: false,
                    latlng: address,
                    key: $('#ggmapapikey').val()
                },
                success: function (res) {
                    if (res && res.results.length > 0) {
                        targetElmnt.text('Địa điểm: ' + res.results["0"].formatted_address);
                    }
                },
                error: function (res) {
                    targetElmnt.text('Địa điểm: Không rõ');
                }
            });
        }
    }
}

function getAddress(elmnt) {
    var li_address = $(elmnt).parents('tr').find('li[data-address]');
    li_address.each(function (i, target) {
        var targetElmnt = $(target);
        getAddressByGoogle(targetElmnt);
    });
}

$(document).on("shown.bs.popover", ".btn-timekeeper", function () {
    var targetElmnt = $(this).closest("td").find(".google-location");
    getAddressByGoogle(targetElmnt);
})

function setNoCam() {
    $('.cam').addClass('hidden');
    $('.ip').removeClass('hidden');
    $('#file').val("1");
}

function startCamera() {
    Webcam.on('error', function (err) {
        // an error occurred (see 'err')
        setNoCam();
    });
    if ($(window).width() <= 800) {
        $('#previewImg').css("width", 290);
        $('#previewImg').css("height", 386);
        Webcam.set({
            width: 290,
            height: 386
        });

        if (Webcam.iOS) {
            $('#my_camera').css("display", "none");
        }
    } else {

        Webcam.set({
            width: 386,
            height: 290
        });
    }

    Webcam.attach('#my_camera');
};

function take_snapshot() {
    if (Webcam.loaded) {
        $('.preview').addClass("loading");
        var res = Webcam.snap(function (data_uri) {
            var raw_image_data = data_uri.replace(/^data\:image\/\w+\;base64\,/, '');
            document.getElementById('my_result').innerHTML = '<img class="img-responsive" src="' + data_uri + '"/>';
            $('#previewImg').addClass("hidden");
            $('.preview').removeClass("loading");

            $('#file').val(raw_image_data);
        });
        if (res == false) {
            setNoCam();
        }
    }
}

function ReloadTableDataAfterSubmit(d) {
    var $current_Modal = $(".modal.in");
    if ($current_Modal.length > 0) {
        var $form = $current_Modal.find("form");
        if ($form.attr("action") == "/Admin/AddTimekeeper") {
            if (d.success == true) {
                success_alert("Chấm công thành công", function () {
                    window.location.reload();
                });
            } else {
                var message = d.message != "" ? d.message : "Chấm công không thành công, vui lòng thử lại!";
                error_alert(message, function () {
                    window.location.reload();
                });
            }
        }
    }
}

$("li.google-location[data-address]").each(function () {
    getAddressByGoogle($(this));
})