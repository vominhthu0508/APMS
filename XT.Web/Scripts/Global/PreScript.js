/*Những script sẽ chạy bootstrap mà không cần preload layout script*/

///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
/////Header Scroll
var didScroll = false;

window.onscroll = doThisStuffOnScroll;

function getScrollTop()
{
    var doc = document.documentElement;
    return (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
}

function my$(selector)
{
    var ele = document.querySelectorAll(selector);
    if (ele.length == 1)
        return ele[0];

    return ele;
}

function addClass(ele, clas)
{
    if (!hasClass(ele, clas))
        ele.className += " " + clas;
}

function removeClass(ele, clas) {
    ele.className = ele.className.replace(new RegExp('\\b' + clas + '\\b'), '');
}

function hasClass(ele, clas)
{
    return ele.className.indexOf(clas) >= 0;
}

function doThisStuffOnScroll() {
    didScroll = true;

    var window_scrollTop = getScrollTop();
    var $header = my$("header");
    if (!hasClass($header, "fixed"))
    {
        if (window_scrollTop > 0) {
            //$("header").addClass("sticky");
            //$header.className += " sticky";
            addClass($header, "sticky");
        }
        else {
            //$("header").removeClass("sticky");
            //$header.className = $header.className.replace("sticky", "");
            removeClass($header, "sticky");
        }
    }
}

setInterval(function () {
    if (didScroll) {
        didScroll = false;
        //console.log('You scrolled');
    }
}, 100);