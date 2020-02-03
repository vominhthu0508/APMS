Element.prototype.remove = function () {
    this.parentElement.removeChild(this);
}

function isBlank(str) {
    return (!str || /^\s*$/.test(str));
}

// Load script element as a child of the body
function loadJS(src, async, callback) {
    if (!isBlank(src)) {
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.async = async;
        if (script.readyState) {  //IE
            script.onreadystatechange = function () {
                if (script.readyState == "loaded" || script.readyState == "complete") {
                    script.onreadystatechange = null;
                    if (callback) {
                        callback();
                    }
                }
            };
        } else {  //Others
            script.onload = function () {
                if (callback) {
                    callback();
                }
            };
        }
        script.src = src;
        document.body.appendChild(script);
    }
}

// Load All Resources
function loadResources() {
    //load layout.js
    var layoutScripts = document.getElementById("layoutScripts").value;

    loadJS(layoutScripts, true, function () {
        //load child scripts (index.js)
        var childScriptsElement = document.getElementById("childSCripts");
        if (childScriptsElement && childScriptsElement.innerHTML)
        {
            var script_arr = childScriptsElement.innerHTML.split("\n");
            for (var i = 0; i < script_arr.length; i++)
            {
                loadJS(script_arr[i], true);
            }
            //remove scripts
            document.getElementById("childSCripts").remove();
        }
        //remove scripts
        document.getElementById("layoutScripts").remove();

        //load google js
    });
}

// Check for browser support of event handling capability
//if (window.addEventListener) {
//    window.addEventListener("load", loadResources, false);
//} else if (window.attachEvent) {
//    window.attachEvent("onload", loadResources);
//} else {
//    window.onload = loadResources;
//}
//12_7_2014: load instanstly
loadResources();